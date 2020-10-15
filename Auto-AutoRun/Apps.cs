﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;

/// <summary>
/// The API for autorun app searcher
/// </summary>
namespace Autorun_API
{
    /// <summary>
    /// A static class containing the function to search for programs and the classes used by the API
    /// </summary>
    public static class Apps
    {

        public static string ReadDocFile(string file)
        {
            string html;
            switch (Path.GetExtension(file))
            {
                case ".md":
                case ".markdown":
                    var markdown = new MarkdownSharp.Markdown();
                    html= PrependHTML +"<div class='markdown-body'>"+ markdown.Transform(File.ReadAllText(file))+"</div>";
                    break;
                case ".html":
                case ".htm":
                case ".xhtml":
                    html = File.ReadAllText(file);
                    break;
                case ".txt":
                    html = PrependHTML+"<div class='markdown-body'>" + (File.ReadAllText(file).Replace("\r\n", "<br/>").Replace("\n\r", "<br/>").Replace("\r", "<br/>").Replace("\n", "<br/>")) + "</div>";
                    break;
                default:
                    throw new NotSupportedException("The file format cannot be converted to HTML or isn't supported by Auto-Autorun");
            }
            return html;
        }

        private static IEnumerable<string> EnumerateDir(string dir)
        {
            dir = dir.Trim('\\');
            if (File.Exists(dir + OrderDirsFileName))
                return from line in File.ReadAllLines(dir + OrderDirsFileName)
                       select dir + @"\" + line;
            else
                return Directory.EnumerateDirectories(dir);
        }
        private static IEnumerable<string> EnumerateFile(string dir)
        {
            dir = dir.Trim('\\');
            if (File.Exists(dir +OrderFilesFileName))
                return from line in File.ReadAllLines(dir + OrderFilesFileName)
                       select dir + @"\" + line;
            else
                return Directory.EnumerateFiles(dir);
        }

        /// <summary>
        /// The root directory to scan for apps
        /// </summary>
        public static string RootDirectory;

        /// <summary>
        /// The folder which contains the documentation for an app/ collection
        /// </summary>
        const string DocumentationDirectory = @"_docs\";

        /// <summary>
        /// The file which defines the order of files in the directory
        /// </summary>
        const string OrderFilesFileName = @"\order-files.txt";

        /// <summary>
        /// The file which defines the order of dirs in the directory
        /// </summary>
        const string OrderDirsFileName = @"\order-forlders.txt";

        /// <summary>
        /// The style prepended to the HTML code generated by <see cref="MarkdownSharp.Markdown"/>
        /// <para>
        /// It isn't declared as <code>const</code> so that the user can override it.
        /// </para>
        /// </summary>
        static string PrependHTML = Auto_AutoRun.Properties.Resources.github_style_css;

        /// <summary>
        /// Scans the given directory and puts the results in <see cref="RootNode"/>
        /// <para>It actually calls <see cref="CollectionNode(string)"/>and sets the <see cref="CollectionNode.Name"/> to the name of the given folder, or if it is the root folder, the label of the drive.</para> 
        /// </summary>
        /// <param name="root">The directory to scan for, which is copied into <see cref="RootDirectory"/></param>
        public static CollectionNode Load(string root)
        {
            RootDirectory = root;
            var RootNode = new CollectionNode(RootDirectory);
            var Driveroot = Path.GetPathRoot(RootDirectory);
            if (Driveroot == RootDirectory) // It is a root dir
            {
                var drives = DriveInfo.GetDrives();
                //drives[0].
                var goodDrive =
                    from item in drives
                    where item.RootDirectory.FullName == Driveroot
                    select item.VolumeLabel;
                RootNode.Name = goodDrive.First();
            }
            else
                RootNode.Name = Path.GetFileName(RootDirectory.Trim('\\'));
            return RootNode;
        }

        /// <summary>
        /// It represents an app/collection
        /// </summary>
        public class CollectionNode
        {
            /// <summary>
            /// Creates a new instance of the <see cref="CollectionNode"/> class and scans it's folder.
            /// </summary>
            /// <param name="dir">The folder containing the files of the app/collection</param>
            public CollectionNode(string dir)
            {
                Directory = dir;
                Name = Path.GetFileName(dir.Trim('\\'));

                if (System.IO.Directory.Exists(dir + DocumentationDirectory))
                    Docs = new Documentation(dir + DocumentationDirectory);

                #region Enumerate childs, actions and versions
                foreach (var File in EnumerateDir(dir))
                {
                    var file = File + '\\';
                    DirectoryInfo diri = new DirectoryInfo(dir);
                    if (diri.Attributes.HasFlag(FileAttributes.Hidden))
                        continue;
                    if (Path.GetFileName(File)[0] == '_')
                    {
                        loadAction(File);
                        continue;
                    }
                    var child = new CollectionNode(file);
                    if (child.Name.StartsWith("v"))
                    {
                        var ver = new Version();
                        ver.Name = child.Name.Substring(1);
                        ver.Actions.Clear();
                        ver.Actions.AddRange(child.Actions);

                        foreach (string item in new string[] {
                                "info.md",
                                "info.markdown",
                                "info.html",
                                "info.htm",
                                "info.xhtml",
                                "info.txt"
                            })
                        {

                            if (System.IO.File.Exists(file + item))
                            {
                                ver.VersionInfo = ReadDocFile(file + item);
                                break;
                            }
                        }
                        Versions.Add(ver);
                        continue;
                    }
                    Childs.Add(child);
                }
                #endregion
            }

            private void loadAction(string path)
            {
                var dirnm = Path.GetFileName(path);
                var label = dirnm.Substring(1);
                if (label == "docs")
                    return;
                try
                {
                    Actions.Add(new NodeAction(path, label));
                }
                catch (Exception) { }
            }

            /// <summary>
            /// Contains the child nodes of this node.
            /// </summary>
            public List<CollectionNode> Childs = new List<CollectionNode>();

            public List<Version> Versions = new List<Version>();
            public List<NodeAction> Actions = new List<NodeAction>();

            /// <summary>
            /// Contains the documentation of this node.
            /// </summary>
            public Documentation Docs;

            /// <summary>
            /// The directory in which this node exists
            /// </summary>
            public string Directory;

            /// <summary>
            /// The title of the node. It is not set by the constructor, but by <see cref="Load(string)"/>
            /// </summary>
            public string Name;

            /// <summary>
            /// Represents an action for a node, such as install, patch, etc.
            /// </summary>
            public class NodeAction
            {
                /// <summary>
                /// Creates a new instance of the <see cref="NodeAction"/> class.
                /// </summary>
                /// <param name="dir">The folder (path) that contains the action files.</param>
                /// <param name="fileName">The name of the directory (not path) without the underline.</param>
                public NodeAction(string dir, string fileName)
                {
                    if (!dir.EndsWith("\\"))
                    {
                        dir += "\\";
                    }
                    Directory = dir;
                    Name = fileName;
                    foreach (var file in EnumerateFile(dir))
                    {
                        if (file.ToLower().EndsWith($@"\{fileName.ToLower()}.exe"))
                            FilePath = file;
                        if (file.ToLower().EndsWith($@"\{fileName.ToLower()}.zip"))
                            FilePath = file;
                        if (file.ToLower().EndsWith($@"\{fileName.ToLower()}.rar"))
                            FilePath = file;
                        if (file.ToLower().EndsWith($@"\{fileName.ToLower()}.msi"))
                            FilePath = file;
                        if (file.ToLower().EndsWith($@"\{fileName.ToLower()}.txt"))
                        {
                            var fn = File.ReadAllText(file).Trim('\n', ' ', '\t', '\\');
                            FilePath = dir + fn;
                        }
                    }
                    if (FilePath == null)
                    {
                        throw new FileNotFoundException();
                    }
                }

                /// <summary>
                /// The folder (path) that contains the action files.
                /// </summary>
                public string Directory;

                /// <summary>
                /// The path of the file which needs to be run to invoke this action.
                /// </summary>
                public string FilePath;

                /// <summary>
                /// The name of this action, for example "Install", "Patch", "Open"
                /// </summary>
                public string Name;

                /// <summary>
                /// Invokes the action (runs the file at <see cref="FilePath"/>)
                /// </summary>
                public void Open()
                {
                    Process.Start(FilePath);
                }

                /// <summary>
                /// Shows the file at <see cref="FilePath"/> in file explorer.
                /// </summary>
                public void ShowFile()
                {
                    Process.Start("explorer.exe", string.Format("/select,\"{0}\"", FilePath));
                }
            }

            /// <summary>
            /// Represents the documentation
            /// </summary>
            public class Documentation
            {
                /// <summary>
                /// Contains the files included in the screenshots directory
                /// <para>Will be null if the folder does not exist.</para>
                /// </summary>
                public List<Image> ScreenShots;

                public List<string[]> Pages = new List<string[]>();

                /// <summary>
                /// The folder which contains the documentation files.
                /// </summary>
                public string Directory;

                /// <summary>
                /// The content of 'icon.png', 'icon.ico', 'icon.jpg', 'icon.jpeg', 'icon.bmp', or 'icon.jfif'
                /// </summary>
                public Image Icon;

                /// <summary>
                /// Contains the name of the screen-shots directory excluding the underscore
                /// </summary>
                public string ScreenShotsDirectoryName;

                /// <summary>
                /// Initializes a new instance of the <see cref="Documentation"/> class.
                /// </summary>
                /// <param name="dir">The folder which contains markdown files and 'screenshots' folder</param>
                public Documentation(string dir)
                {
                    Directory = dir;
                    MarkdownSharp.Markdown markdown = new MarkdownSharp.Markdown();
                    foreach (var file in EnumerateFile(dir))
                    {
                        foreach (string item in new string[] {
                                ".md",
                                ".markdown",
                                ".html",
                                ".htm",
                                ".xhtml",
                                ".txt"
                            })
                        {
                            if (Path.GetExtension(file) == item)
                            {
                                Pages.Add(new string[] { Path.GetFileNameWithoutExtension(file), ReadDocFile(file) });
                                break;
                            }
                        }
                    }

                    #region Icon
                    tryOpenIcon(dir + "icon.png");
                    tryOpenIcon(dir + "icon.ico");
                    tryOpenIcon(dir + "icon.jpg");
                    tryOpenIcon(dir + "icon.jpeg");
                    tryOpenIcon(dir + "icon.jfif");
                    tryOpenIcon(dir + "icon.bmp");
                    #endregion

                    #region Screenshots

                    var docsdirs = EnumerateDir(dir);
                    if (docsdirs.Count() == 0) return;
                    var scrdir = docsdirs.First();
                    ScreenShotsDirectoryName = scrdir.Split('\\').Last();
                    ScreenShots = new List<Image>();
                    foreach (var item in EnumerateFile(scrdir))
                    {
                        try
                        {
                            ScreenShots.Add(Image.FromFile(item));
                        }
                        catch
                        {
                        }
                    }
                    #endregion
                }

                private void tryOpenIcon(string file)
                {
                    if (File.Exists(file))
                    {
                        Icon = Image.FromFile(file);
                    }
                }
            }

            public class Version
            {
                public List<NodeAction> Actions = new List<NodeAction>();
                public string Name;
                public string VersionInfo;
            }
        }
    }
}