﻿using Autorun_API;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Auto_AutoRun
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            var rootnode = Apps.Load(@"E:\App");
            AppsTree.Nodes.Add(populateTree(rootnode));
            AppsTree.SelectedNode = AppsTree.Nodes[0];
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
            #endregion

            #region Docs
            if (app.Docs == null)
                Tabs.Visible = false;
            else
            {
                Tabs.Visible = true;

                foreach(TabPage tab in Tabs.TabPages)
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
                    tab.Controls.Add(browser);

                    Tabs.TabPages.Insert(Math.Max(Tabs.TabPages.Count - 1,0), tab);
                }

                if (app.Docs.ScreenShots == null)
                {
                    if (Tabs.TabPages.Contains(ScreenshotTab))
                        Tabs.TabPages.Remove(ScreenshotTab);
                }
                else
                {
                    if (!Tabs.TabPages.Contains(ScreenshotTab))
                        Tabs.TabPages.Add(ScreenshotTab);

                    Screenshots.Controls.Clear();
                    foreach (var item in app.Docs.ScreenShots)
                    {
                        var pb = new PictureBox();
                        pb.Image = item;
                        pb.SizeMode = PictureBoxSizeMode.Zoom;
                        pb.Size = new Size(600, 400);
                        Screenshots.Controls.Add(pb);
                    }
                }
            }
            #endregion
            
            #region Actions
            Resources.Controls.Clear();
            foreach (var item in app.Actions)
            {
                Button btn = new Button();
                btn.Size = new Size(75, 23);
                btn.Text = item.Name;
                btn.Tag = item;
                btn.Click += new EventHandler( OpenResourceButton);
                btn.MouseUp += new MouseEventHandler( OpenResourceFolderButton);
                Resources.Controls.Add(btn);
            }
            #endregion
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
                tab.Controls.Add(browser);
                Tabs.TabPages.Add(tab);
            }
            #endregion
        }
    }
}
