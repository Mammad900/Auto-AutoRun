using Autorun_API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Auto_AutoRun
{
    public partial class Form1 : Form
    {
        string[] args;
        public Form1(string[] args)
        {
            this.args = args;
            InitializeComponent();
        }
        Apps.CollectionNode rootnode;
        private void Form1_Load(object sender, EventArgs e)
        {
            SuspendLayout();

            var path = ( (args.Length==1) ? (args[0]) : (Environment.CurrentDirectory + "\\") );

            if (path.StartsWith("\"")) path = path.Substring(1);
            if (path.EndsWith("\"")) path = path.Substring(0, path.Length - 1);
            if (!path.EndsWith("\\")) path += "\\";
            path = path.Replace("/", "\\");

            System.ComponentModel.BackgroundWorker back = new System.ComponentModel.BackgroundWorker();
            back.DoWork += (object sender2, System.ComponentModel.DoWorkEventArgs e2) =>
            {
                var s= e2.Argument as string;
                var res = Apps.Load(s);
                Invoke(new Action<Apps.CollectionNode>((Apps.CollectionNode root) =>
                {
                    rootnode = root;
                    AppsTree.Nodes.Add(populateTree(rootnode));
                    AppsTree.SelectedNode = AppsTree.Nodes[0];

                    var fil = Environment.CurrentDirectory + "\\Icon.ico";
                    ShowIcon = true;
                    if (File.Exists(fil))
                    {
                        Icon = new Icon(fil);
                    }
                    else if ((rootnode.Docs != null) && (rootnode.Docs.Icon != null))
                    {
                        var bmp = new Bitmap(rootnode.Docs.Icon);
                        var thumb = (Bitmap)bmp.GetThumbnailImage(24, 24, null, IntPtr.Zero);
                        thumb.MakeTransparent();
                        Icon = Icon.FromHandle(thumb.GetHicon());
                    }
                    else
                    {
                        ShowIcon = false;
                    }
                    Text = rootnode.Name;
                    ResumeLayout();
                }),res);
            };
            back.RunWorkerAsync(path);
        }

        TreeNode populateTree(Apps.CollectionNode root)
        {
            TreeNode node = new TreeNode(root.Name);
            node.Tag = root;
            node.Expand();
            foreach (var item in root.Childs)
            {
                node.Nodes.Add(populateTree(item));
            }
            return node;
        }

        private void AppsTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SuspendLayout();


            Apps.CollectionNode app = (Apps.CollectionNode)e.Node.Tag;

            #region Name and icon
            AppTitle.Text = app.Name;
            if (app.Docs!=null)
            {
                AppIcon.Image = app.Docs.Icon;
            }
            else
            {
                AppIcon.Image = null;
            }
            #endregion

            #region Versions
            VersionSelect.BeginUpdate();


            VersionSelect.Items.Clear();
            if (app.Versions.Count > 0)
            {
                VersionSelect.Visible = true;
                VersionActions.Visible = true;
                label1.Visible = true;
                VersionSelect.Tag = app.Versions;
                foreach (var item in app.Versions)
                {
                    VersionSelect.Items.Add(item.Name);
                }
                VersionSelect.SelectedIndex = VersionSelect.Items.Count - 1;
            }
            else
            {
                VersionSelect.Visible = false;
                VersionActions.Visible = false;
                label1.Visible = false;
            }

            VersionSelect.EndUpdate();
            #endregion

            #region Docs
            Tabs.SuspendLayout();

            if (app.Docs == null)
                Tabs.Visible = false;
            else
            {
                Tabs.Visible = true;

                #region Pages
                foreach (TabPage tab in Tabs.TabPages)
                {
                    if (tab == ScreenshotTab)
                        continue;
                    Tabs.TabPages.Remove(tab);
                }
                
                foreach(var page in app.Docs.Pages)
                {
                    var tab = new TabPage(page[0]);

                    var browser = new WebBrowser();
                    browser.Dock = DockStyle.Fill;
                    browser.Url = new Uri("about:blank");
                    browser.Document.Write(page[1]);
                    browser.Navigating += webBrowser_Navigating;
                    tab.Controls.Add(browser);

                    Tabs.TabPages.Insert(Math.Max(Tabs.TabPages.Count - 1,0), tab);
                }
                #endregion

                #region Screen shots

                if (app.Docs.ScreenShots == null)
                {
                    if (Tabs.TabPages.Contains(ScreenshotTab))
                        Tabs.TabPages.Remove(ScreenshotTab);
                }
                else
                {
                    ScreenshotTab.Text = app.Docs.ScreenShotsDirectoryName;
                    if (!Tabs.TabPages.Contains(ScreenshotTab))
                        Tabs.TabPages.Add(ScreenshotTab);

                    Screenshots.Controls.Clear();
                    foreach (var item in app.Docs.ScreenShots)
                    {
                        var pb = new PictureBox();
                        pb.Image = item;
                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                        pb.Size = GetScreenShotSize(pb, Screenshots);
                        Screenshots.Controls.Add(pb);
                    }
                }
                #endregion
            }

            Tabs.ResumeLayout();
            #endregion

            #region Actions
            Actions.SuspendLayout();

            Actions.Controls.Clear();
            foreach (var item in app.Actions)
            {
                Button btn = new Button();
                btn.Size = new Size(75, 23);
                btn.Text = item.Name;
                btn.Tag = item;
                btn.Click += new EventHandler( OpenResourceButton);
                btn.MouseUp += new MouseEventHandler( OpenResourceFolderButton);
                Actions.Controls.Add(btn);
            }

            Actions.ResumeLayout();
            #endregion


            ResumeLayout();
        }

        private void OpenResourceButton(object sender, EventArgs e)
        {
            try
            {
                ((sender as Button).Tag as Apps.CollectionNode.NodeAction).Open();
            }
            catch (Exception e2)
            {
                DialogResult res= MessageBox.Show("Failed to open file: " + e2.Message, "Error opening file", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                if (res == DialogResult.Retry)
                {
                    OpenResourceButton(sender, e);
                }
            }
        }
        private void OpenResourceFolderButton(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
                ((sender as Button).Tag as Apps.CollectionNode.NodeAction).ShowFile();
        }

        private void VersionSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            var versions = combo.Tag as List<Apps.CollectionNode.Version>;
            var version = versions [combo.SelectedIndex];

            #region Actions

            VersionActions.Controls.Clear();

            foreach (var item in version.Actions)
            {
                Button btn = new Button();
                btn.Text = item.Name;
                btn.Tag = item;
                btn.Margin = new Padding(2, 0, 2, 0);
                btn.Height = 21;
                btn.Click += OpenResourceButton;
                btn.MouseClick += OpenResourceFolderButton;
                VersionActions.Controls.Add(btn);
            }

            #endregion

            #region Delete version tab

            foreach (TabPage item in Tabs.TabPages)
            {
                if(item.Text.StartsWith("Version: "))
                {
                    Tabs.TabPages.Remove(item);
                }
            }
            #endregion

            #region New version tab
            if (version.VersionInfo != null)
            {
                TabPage tab = new TabPage("Version: " + version.Name);
                WebBrowser browser = new WebBrowser();
                browser.Url = new Uri("about:blank");
                browser.Document.Write(version.VersionInfo);
                browser.Dock = DockStyle.Fill;
                browser.Navigating += webBrowser_Navigating;
                tab.Controls.Add(browser);
                Tabs.TabPages.Add(tab);
            }
            #endregion
        }

        private void webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!(e.Url.ToString().Equals("about:blank", StringComparison.InvariantCultureIgnoreCase)))
            {
                System.Diagnostics.Process.Start(e.Url.ToString());
                e.Cancel = true;
            }
        }

        private void Screenshots_SizeChanged(object sender, EventArgs e)
        {
            Screenshots.SuspendLayout();
            foreach (Control ctrl in Screenshots.Controls)
            {
                ctrl.Size = GetScreenShotSize(ctrl as PictureBox, Screenshots);
            }
            Screenshots.ResumeLayout();
        }
        private Size GetScreenShotSize(PictureBox image,Control container)
        {
            var h = 5 + (int)Math.Min(container.ClientSize.Width * ((image.Image.Height * 1.0) / image.Image.Width), 500);
            var w = Screenshots.ClientSize.Width - 10;
            return new Size(w, h);
        }
    }

}
