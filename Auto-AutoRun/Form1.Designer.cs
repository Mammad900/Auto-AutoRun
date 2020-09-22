namespace Auto_AutoRun
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.AppsTree = new System.Windows.Forms.TreeView();
            this.Tabs = new System.Windows.Forms.TabControl();
            this.ScreenshotTab = new System.Windows.Forms.TabPage();
            this.Screenshots = new System.Windows.Forms.FlowLayoutPanel();
            this.AppIcon = new System.Windows.Forms.PictureBox();
            this.AppTitle = new System.Windows.Forms.Label();
            this.Actions = new System.Windows.Forms.FlowLayoutPanel();
            this.VersionSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.VersionActions = new System.Windows.Forms.FlowLayoutPanel();
            this.Tabs.SuspendLayout();
            this.ScreenshotTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AppIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // AppsTree
            // 
            this.AppsTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.AppsTree.HideSelection = false;
            this.AppsTree.Location = new System.Drawing.Point(12, 12);
            this.AppsTree.Name = "AppsTree";
            this.AppsTree.Size = new System.Drawing.Size(224, 451);
            this.AppsTree.TabIndex = 0;
            this.AppsTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.AppsTree_AfterSelect);
            // 
            // Tabs
            // 
            this.Tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tabs.Controls.Add(this.ScreenshotTab);
            this.Tabs.Location = new System.Drawing.Point(242, 74);
            this.Tabs.Name = "Tabs";
            this.Tabs.SelectedIndex = 0;
            this.Tabs.Size = new System.Drawing.Size(569, 389);
            this.Tabs.TabIndex = 1;
            // 
            // ScreenshotTab
            // 
            this.ScreenshotTab.BackColor = System.Drawing.SystemColors.Control;
            this.ScreenshotTab.Controls.Add(this.Screenshots);
            this.ScreenshotTab.Location = new System.Drawing.Point(4, 22);
            this.ScreenshotTab.Name = "ScreenshotTab";
            this.ScreenshotTab.Padding = new System.Windows.Forms.Padding(3);
            this.ScreenshotTab.Size = new System.Drawing.Size(561, 363);
            this.ScreenshotTab.TabIndex = 2;
            this.ScreenshotTab.Text = "Screenshots";
            // 
            // Screenshots
            // 
            this.Screenshots.AutoScroll = true;
            this.Screenshots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Screenshots.Location = new System.Drawing.Point(3, 3);
            this.Screenshots.Name = "Screenshots";
            this.Screenshots.Size = new System.Drawing.Size(555, 357);
            this.Screenshots.TabIndex = 0;
            // 
            // AppIcon
            // 
            this.AppIcon.Location = new System.Drawing.Point(246, 12);
            this.AppIcon.Name = "AppIcon";
            this.AppIcon.Size = new System.Drawing.Size(32, 32);
            this.AppIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.AppIcon.TabIndex = 6;
            this.AppIcon.TabStop = false;
            // 
            // AppTitle
            // 
            this.AppTitle.AutoSize = true;
            this.AppTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AppTitle.Location = new System.Drawing.Point(280, 15);
            this.AppTitle.Name = "AppTitle";
            this.AppTitle.Size = new System.Drawing.Size(0, 25);
            this.AppTitle.TabIndex = 7;
            // 
            // Resources
            // 
            this.Actions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Actions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.Actions.Location = new System.Drawing.Point(293, 12);
            this.Actions.Name = "Resources";
            this.Actions.Size = new System.Drawing.Size(518, 32);
            this.Actions.TabIndex = 8;
            // 
            // VersionSelect
            // 
            this.VersionSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VersionSelect.FormattingEnabled = true;
            this.VersionSelect.Location = new System.Drawing.Point(293, 50);
            this.VersionSelect.Name = "VersionSelect";
            this.VersionSelect.Size = new System.Drawing.Size(158, 21);
            this.VersionSelect.TabIndex = 9;
            this.VersionSelect.SelectedIndexChanged += new System.EventHandler(this.VersionSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(243, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Version:";
            // 
            // VersionActions
            // 
            this.VersionActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.VersionActions.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.VersionActions.Location = new System.Drawing.Point(457, 50);
            this.VersionActions.Name = "VersionActions";
            this.VersionActions.Size = new System.Drawing.Size(354, 21);
            this.VersionActions.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 479);
            this.Controls.Add(this.VersionActions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.VersionSelect);
            this.Controls.Add(this.AppTitle);
            this.Controls.Add(this.AppIcon);
            this.Controls.Add(this.Tabs);
            this.Controls.Add(this.AppsTree);
            this.Controls.Add(this.Actions);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Tabs.ResumeLayout(false);
            this.ScreenshotTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AppIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView AppsTree;
        private System.Windows.Forms.TabControl Tabs;
        private System.Windows.Forms.TabPage ScreenshotTab;
        private System.Windows.Forms.FlowLayoutPanel Screenshots;
        private System.Windows.Forms.PictureBox AppIcon;
        private System.Windows.Forms.Label AppTitle;
        private System.Windows.Forms.FlowLayoutPanel Actions;
        private System.Windows.Forms.ComboBox VersionSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel VersionActions;
    }
}

