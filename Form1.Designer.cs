namespace SMM2DL
{
    partial class frmSMM2
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Loading Data...");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSMM2));
            this.txtLLevel = new System.Windows.Forms.TextBox();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.lblLastLoad = new System.Windows.Forms.Label();
            this.lblSave = new System.Windows.Forms.Label();
            this.stsMainStatus = new System.Windows.Forms.StatusStrip();
            this.stsStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lstMarioLevels = new System.Windows.Forms.ListView();
            this.clmhName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhToD = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhWorld = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhReleaseDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhCreator = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmhType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mnuRightClick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuExtract = new System.Windows.Forms.ToolStripMenuItem();
            this.stsMainStatus.SuspendLayout();
            this.mnuRightClick.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtLLevel
            // 
            this.txtLLevel.Location = new System.Drawing.Point(125, 450);
            this.txtLLevel.Name = "txtLLevel";
            this.txtLLevel.Size = new System.Drawing.Size(65, 20);
            this.txtLLevel.TabIndex = 1;
            this.txtLLevel.Text = "706";
            this.txtLLevel.TextChanged += new System.EventHandler(this.LevelChanged);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(110, 480);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(645, 20);
            this.txtFolder.TabIndex = 2;
            this.txtFolder.TextChanged += new System.EventHandler(this.SaveLChanged);
            // 
            // lblLastLoad
            // 
            this.lblLastLoad.AutoSize = true;
            this.lblLastLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastLoad.Location = new System.Drawing.Point(10, 450);
            this.lblLastLoad.Name = "lblLastLoad";
            this.lblLastLoad.Size = new System.Drawing.Size(112, 16);
            this.lblLastLoad.TabIndex = 3;
            this.lblLastLoad.Text = "Last level to load:";
            // 
            // lblSave
            // 
            this.lblSave.AutoSize = true;
            this.lblSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSave.Location = new System.Drawing.Point(10, 480);
            this.lblSave.Name = "lblSave";
            this.lblSave.Size = new System.Drawing.Size(97, 16);
            this.lblSave.TabIndex = 4;
            this.lblSave.Text = "Save Location:";
            // 
            // stsMainStatus
            // 
            this.stsMainStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stsStatus});
            this.stsMainStatus.Location = new System.Drawing.Point(0, 544);
            this.stsMainStatus.Name = "stsMainStatus";
            this.stsMainStatus.Size = new System.Drawing.Size(764, 22);
            this.stsMainStatus.TabIndex = 5;
            this.stsMainStatus.Text = "statusStrip1";
            // 
            // stsStatus
            // 
            this.stsStatus.Name = "stsStatus";
            this.stsStatus.Size = new System.Drawing.Size(39, 17);
            this.stsStatus.Text = "Ready";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(64, 36);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lstMarioLevels
            // 
            this.lstMarioLevels.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lstMarioLevels.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lstMarioLevels.AutoArrange = false;
            this.lstMarioLevels.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmhName,
            this.clmhToD,
            this.clmhWorld,
            this.clmhReleaseDate,
            this.clmhCreator,
            this.clmhLevel,
            this.clmhType});
            this.lstMarioLevels.FullRowSelect = true;
            this.lstMarioLevels.HideSelection = false;
            this.lstMarioLevels.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lstMarioLevels.LabelWrap = false;
            this.lstMarioLevels.Location = new System.Drawing.Point(0, 0);
            this.lstMarioLevels.Name = "lstMarioLevels";
            this.lstMarioLevels.Size = new System.Drawing.Size(770, 425);
            this.lstMarioLevels.SmallImageList = this.imageList1;
            this.lstMarioLevels.Sorting = global::SMM2DL.Properties.Settings.Default.lstMarioLevels_Sorting;
            this.lstMarioLevels.TabIndex = 0;
            this.lstMarioLevels.UseCompatibleStateImageBehavior = false;
            this.lstMarioLevels.View = System.Windows.Forms.View.Details;
            this.lstMarioLevels.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lstMarioLevels_Sort);
            this.lstMarioLevels.MouseClick += new System.Windows.Forms.MouseEventHandler(this.MouseCheck);
            this.lstMarioLevels.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ExtractEvent);
            // 
            // clmhName
            // 
            this.clmhName.Text = "Name";
            this.clmhName.Width = 300;
            // 
            // clmhToD
            // 
            this.clmhToD.Text = "ToD";
            this.clmhToD.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clmhToD.Width = 50;
            // 
            // clmhWorld
            // 
            this.clmhWorld.Text = "World";
            this.clmhWorld.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // clmhReleaseDate
            // 
            this.clmhReleaseDate.Text = "Release Date";
            this.clmhReleaseDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clmhReleaseDate.Width = 80;
            // 
            // clmhCreator
            // 
            this.clmhCreator.Text = "Creator";
            this.clmhCreator.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clmhCreator.Width = 90;
            // 
            // clmhLevel
            // 
            this.clmhLevel.Text = "Level Number";
            this.clmhLevel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // clmhType
            // 
            this.clmhType.Text = "Game Type";
            this.clmhType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.clmhType.Width = 120;
            // 
            // mnuRightClick
            // 
            this.mnuRightClick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuExtract});
            this.mnuRightClick.Name = "mnuRightClick";
            this.mnuRightClick.Size = new System.Drawing.Size(111, 26);
            // 
            // mnuExtract
            // 
            this.mnuExtract.Name = "mnuExtract";
            this.mnuExtract.Size = new System.Drawing.Size(110, 22);
            this.mnuExtract.Text = "Extract";
            this.mnuExtract.Click += new System.EventHandler(this.MouseExtract);
            // 
            // frmSMM2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(764, 566);
            this.Controls.Add(this.stsMainStatus);
            this.Controls.Add(this.lblSave);
            this.Controls.Add(this.lblLastLoad);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.txtLLevel);
            this.Controls.Add(this.lstMarioLevels);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSMM2";
            this.RightToLeftLayout = true;
            this.Text = "Mario Maker 2 Level Downloader - dbzfanatic";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.LoadList);
            this.stsMainStatus.ResumeLayout(false);
            this.stsMainStatus.PerformLayout();
            this.mnuRightClick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lstMarioLevels;
        private System.Windows.Forms.ColumnHeader clmhToD;
        private System.Windows.Forms.ColumnHeader clmhWorld;
        private System.Windows.Forms.ColumnHeader clmhReleaseDate;
        private System.Windows.Forms.ColumnHeader clmhCreator;
        private System.Windows.Forms.ColumnHeader clmhLevel;
        private System.Windows.Forms.ColumnHeader clmhType;
        private System.Windows.Forms.TextBox txtLLevel;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Label lblLastLoad;
        private System.Windows.Forms.Label lblSave;
        private System.Windows.Forms.StatusStrip stsMainStatus;
        private System.Windows.Forms.ToolStripStatusLabel stsStatus;
        public System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ColumnHeader clmhName;
        public System.Windows.Forms.ContextMenuStrip mnuRightClick;
        private System.Windows.Forms.ToolStripMenuItem mnuExtract;
    }
}

