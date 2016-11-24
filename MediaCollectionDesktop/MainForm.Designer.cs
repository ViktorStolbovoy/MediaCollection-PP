namespace MediaCollection
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.PanelLeft = new System.Windows.Forms.Panel();
			this.TbxSearch = new System.Windows.Forms.TextBox();
			this.RbnAudio = new System.Windows.Forms.RadioButton();
			this.RbnVideo = new System.Windows.Forms.RadioButton();
			this.TVTitles = new BrightIdeasSoftware.TreeListView();
			this.OlvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.TreeImageList = new System.Windows.Forms.ImageList(this.components);
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.TSMITitles = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMIBulkAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMIAddManually = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMIRemoveTitle = new System.Windows.Forms.ToolStripMenuItem();
			this.FilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.updateAllFromTMDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMITitlesConfig = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMIDevices = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMIBulkAddLocations = new System.Windows.Forms.ToolStripMenuItem();
			this.RatingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TSMISystem = new System.Windows.Forms.ToolStripMenuItem();
			this.PanelMain = new System.Windows.Forms.Panel();
			this.BtnOpenBrowser = new System.Windows.Forms.Button();
			this.TbxEpisode = new System.Windows.Forms.TextBox();
			this.LblEpisode = new System.Windows.Forms.Label();
			this.TbxDisk = new System.Windows.Forms.TextBox();
			this.LblDisk = new System.Windows.Forms.Label();
			this.TbxSeason = new System.Windows.Forms.TextBox();
			this.LblSeason = new System.Windows.Forms.Label();
			this.BtnSearhProvider = new System.Windows.Forms.Button();
			this.TbxImdbId = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.CbxKind = new System.Windows.Forms.ComboBox();
			this.label3 = new System.Windows.Forms.Label();
			this.BtnNew = new System.Windows.Forms.Button();
			this.BtnDeleteImage = new System.Windows.Forms.Button();
			this.BtnAddImage = new System.Windows.Forms.Button();
			this.BtnNextImage = new System.Windows.Forms.Button();
			this.BtnPrevImage = new System.Windows.Forms.Button();
			this.PbxImage = new System.Windows.Forms.PictureBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.LVRatings = new BrightIdeasSoftware.ObjectListView();
			this.olvColumnRatingName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumnRatingValue = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.BtnRemoveLocation = new System.Windows.Forms.Button();
			this.BtnAddLocation = new System.Windows.Forms.Button();
			this.CbxDevices = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.LVLocations = new BrightIdeasSoftware.ObjectListView();
			this.OlvBtnPlay = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.OlvBaseLocationName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.OlvLocationData = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.TbxDescription = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.BtnDiscard = new System.Windows.Forms.Button();
			this.BtnSave = new System.Windows.Forms.Button();
			this.TbxReleaseYear = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.TbxTitleName = new System.Windows.Forms.TextBox();
			this.lblName = new System.Windows.Forms.Label();
			this.PanelLeft.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TVTitles)).BeginInit();
			this.menuStrip.SuspendLayout();
			this.PanelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbxImage)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LVRatings)).BeginInit();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LVLocations)).BeginInit();
			this.SuspendLayout();
			// 
			// PanelLeft
			// 
			this.PanelLeft.Controls.Add(this.TbxSearch);
			this.PanelLeft.Controls.Add(this.RbnAudio);
			this.PanelLeft.Controls.Add(this.RbnVideo);
			this.PanelLeft.Controls.Add(this.TVTitles);
			this.PanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
			this.PanelLeft.Location = new System.Drawing.Point(0, 24);
			this.PanelLeft.Name = "PanelLeft";
			this.PanelLeft.Size = new System.Drawing.Size(954, 700);
			this.PanelLeft.TabIndex = 0;
			// 
			// TbxSearch
			// 
			this.TbxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.TbxSearch.Location = new System.Drawing.Point(115, 675);
			this.TbxSearch.Name = "TbxSearch";
			this.TbxSearch.Size = new System.Drawing.Size(173, 20);
			this.TbxSearch.TabIndex = 2;
			this.TbxSearch.TextChanged += new System.EventHandler(this.TbxSearch_TextChanged);
			// 
			// RbnAudio
			// 
			this.RbnAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RbnAudio.AutoSize = true;
			this.RbnAudio.Location = new System.Drawing.Point(62, 678);
			this.RbnAudio.Name = "RbnAudio";
			this.RbnAudio.Size = new System.Drawing.Size(52, 17);
			this.RbnAudio.TabIndex = 1;
			this.RbnAudio.Text = "Audio";
			this.RbnAudio.UseVisualStyleBackColor = true;
			// 
			// RbnVideo
			// 
			this.RbnVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.RbnVideo.AutoSize = true;
			this.RbnVideo.Checked = true;
			this.RbnVideo.Location = new System.Drawing.Point(4, 678);
			this.RbnVideo.Name = "RbnVideo";
			this.RbnVideo.Size = new System.Drawing.Size(52, 17);
			this.RbnVideo.TabIndex = 0;
			this.RbnVideo.TabStop = true;
			this.RbnVideo.Text = "Video";
			this.RbnVideo.UseVisualStyleBackColor = true;
			this.RbnVideo.CheckedChanged += new System.EventHandler(this.RbnVideo_CheckedChanged);
			// 
			// TVTitles
			// 
			this.TVTitles.AllColumns.Add(this.OlvColumnName);
			this.TVTitles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TVTitles.CellEditUseWholeCell = false;
			this.TVTitles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.OlvColumnName});
			this.TVTitles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.TVTitles.HideSelection = false;
			this.TVTitles.IsSimpleDragSource = true;
			this.TVTitles.IsSimpleDropSink = true;
			this.TVTitles.LabelWrap = false;
			this.TVTitles.Location = new System.Drawing.Point(0, 0);
			this.TVTitles.Name = "TVTitles";
			this.TVTitles.SelectColumnsOnRightClickBehaviour = BrightIdeasSoftware.ObjectListView.ColumnSelectBehaviour.ModelDialog;
			this.TVTitles.ShowCommandMenuOnRightClick = true;
			this.TVTitles.ShowGroups = false;
			this.TVTitles.ShowHeaderInAllViews = false;
			this.TVTitles.Size = new System.Drawing.Size(319, 670);
			this.TVTitles.SmallImageList = this.TreeImageList;
			this.TVTitles.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.TVTitles.TabIndex = 0;
			this.TVTitles.UseCompatibleStateImageBehavior = false;
			this.TVTitles.UseFilterIndicator = true;
			this.TVTitles.UseFiltering = true;
			this.TVTitles.UseHotItem = true;
			this.TVTitles.UseNotifyPropertyChanged = true;
			this.TVTitles.UseTranslucentHotItem = true;
			this.TVTitles.View = System.Windows.Forms.View.Details;
			this.TVTitles.VirtualMode = true;
			this.TVTitles.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.TVTitles_ItemSelectionChanged);
			// 
			// OlvColumnName
			// 
			this.OlvColumnName.AspectName = "TitleName";
			this.OlvColumnName.FillsFreeSpace = true;
			this.OlvColumnName.ImageAspectName = "Kind";
			this.OlvColumnName.IsEditable = false;
			this.OlvColumnName.Text = "Name";
			this.OlvColumnName.Width = 25;
			// 
			// TreeImageList
			// 
			this.TreeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("TreeImageList.ImageStream")));
			this.TreeImageList.TransparentColor = System.Drawing.Color.Transparent;
			this.TreeImageList.Images.SetKeyName(0, "movie1.png");
			this.TreeImageList.Images.SetKeyName(1, "season.png");
			this.TreeImageList.Images.SetKeyName(2, "movie_folder.png");
			this.TreeImageList.Images.SetKeyName(3, "cd.png");
			this.TreeImageList.Images.SetKeyName(4, "episode.png");
			this.TreeImageList.Images.SetKeyName(5, "song.png");
			this.TreeImageList.Images.SetKeyName(6, "authors.png");
			this.TreeImageList.Images.SetKeyName(7, "dvd.png");
			this.TreeImageList.Images.SetKeyName(8, "folder.png");
			this.TreeImageList.Images.SetKeyName(9, "folder_music.png");
			this.TreeImageList.Images.SetKeyName(10, "folder_music2.png");
			this.TreeImageList.Images.SetKeyName(11, "player_play.png");
			// 
			// menuStrip
			// 
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMITitles,
            this.TSMITitlesConfig});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(954, 24);
			this.menuStrip.TabIndex = 1;
			this.menuStrip.Text = "menuStrip";
			// 
			// TSMITitles
			// 
			this.TSMITitles.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIBulkAdd,
            this.TSMIAddManually,
            this.TSMIRemoveTitle,
            this.FilterToolStripMenuItem,
            this.updateAllFromTMDBToolStripMenuItem});
			this.TSMITitles.Name = "TSMITitles";
			this.TSMITitles.Size = new System.Drawing.Size(47, 20);
			this.TSMITitles.Text = "Titles";
			// 
			// TSMIBulkAdd
			// 
			this.TSMIBulkAdd.Name = "TSMIBulkAdd";
			this.TSMIBulkAdd.Size = new System.Drawing.Size(196, 22);
			this.TSMIBulkAdd.Text = "Bulk Add";
			this.TSMIBulkAdd.Click += new System.EventHandler(this.TSMIBulkAdd_Click);
			// 
			// TSMIAddManually
			// 
			this.TSMIAddManually.Name = "TSMIAddManually";
			this.TSMIAddManually.Size = new System.Drawing.Size(196, 22);
			this.TSMIAddManually.Text = "Add Manually";
			this.TSMIAddManually.Click += new System.EventHandler(this.TSMIAddManually_Click);
			// 
			// TSMIRemoveTitle
			// 
			this.TSMIRemoveTitle.Name = "TSMIRemoveTitle";
			this.TSMIRemoveTitle.Size = new System.Drawing.Size(196, 22);
			this.TSMIRemoveTitle.Text = "Remove";
			// 
			// FilterToolStripMenuItem
			// 
			this.FilterToolStripMenuItem.Name = "FilterToolStripMenuItem";
			this.FilterToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.FilterToolStripMenuItem.Text = "Filter";
			// 
			// updateAllFromTMDBToolStripMenuItem
			// 
			this.updateAllFromTMDBToolStripMenuItem.Name = "updateAllFromTMDBToolStripMenuItem";
			this.updateAllFromTMDBToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
			this.updateAllFromTMDBToolStripMenuItem.Text = "Update All From TMDB";
			// 
			// TSMITitlesConfig
			// 
			this.TSMITitlesConfig.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSMIDevices,
            this.TSMIBulkAddLocations,
            this.RatingsToolStripMenuItem,
            this.TSMISystem});
			this.TSMITitlesConfig.Name = "TSMITitlesConfig";
			this.TSMITitlesConfig.Size = new System.Drawing.Size(93, 20);
			this.TSMITitlesConfig.Text = "Configuration";
			// 
			// TSMIDevices
			// 
			this.TSMIDevices.Name = "TSMIDevices";
			this.TSMIDevices.Size = new System.Drawing.Size(125, 22);
			this.TSMIDevices.Text = "Devices";
			this.TSMIDevices.Click += new System.EventHandler(this.TSMIDevices_Click);
			// 
			// TSMIBulkAddLocations
			// 
			this.TSMIBulkAddLocations.Name = "TSMIBulkAddLocations";
			this.TSMIBulkAddLocations.Size = new System.Drawing.Size(125, 22);
			this.TSMIBulkAddLocations.Text = "Locations";
			this.TSMIBulkAddLocations.Click += new System.EventHandler(this.TSMIBulkAddLocations_Click);
			// 
			// RatingsToolStripMenuItem
			// 
			this.RatingsToolStripMenuItem.Name = "RatingsToolStripMenuItem";
			this.RatingsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
			this.RatingsToolStripMenuItem.Text = "Ratings";
			this.RatingsToolStripMenuItem.Click += new System.EventHandler(this.RatingsToolStripMenuItem_Click);
			// 
			// TSMISystem
			// 
			this.TSMISystem.Name = "TSMISystem";
			this.TSMISystem.Size = new System.Drawing.Size(125, 22);
			this.TSMISystem.Text = "System";
			this.TSMISystem.Click += new System.EventHandler(this.TSMISystem_Click);
			// 
			// PanelMain
			// 
			this.PanelMain.Controls.Add(this.BtnOpenBrowser);
			this.PanelMain.Controls.Add(this.TbxEpisode);
			this.PanelMain.Controls.Add(this.LblEpisode);
			this.PanelMain.Controls.Add(this.TbxDisk);
			this.PanelMain.Controls.Add(this.LblDisk);
			this.PanelMain.Controls.Add(this.TbxSeason);
			this.PanelMain.Controls.Add(this.LblSeason);
			this.PanelMain.Controls.Add(this.BtnSearhProvider);
			this.PanelMain.Controls.Add(this.TbxImdbId);
			this.PanelMain.Controls.Add(this.label5);
			this.PanelMain.Controls.Add(this.CbxKind);
			this.PanelMain.Controls.Add(this.label3);
			this.PanelMain.Controls.Add(this.BtnNew);
			this.PanelMain.Controls.Add(this.BtnDeleteImage);
			this.PanelMain.Controls.Add(this.BtnAddImage);
			this.PanelMain.Controls.Add(this.BtnNextImage);
			this.PanelMain.Controls.Add(this.BtnPrevImage);
			this.PanelMain.Controls.Add(this.PbxImage);
			this.PanelMain.Controls.Add(this.groupBox2);
			this.PanelMain.Controls.Add(this.groupBox1);
			this.PanelMain.Controls.Add(this.TbxDescription);
			this.PanelMain.Controls.Add(this.label2);
			this.PanelMain.Controls.Add(this.BtnDiscard);
			this.PanelMain.Controls.Add(this.BtnSave);
			this.PanelMain.Controls.Add(this.TbxReleaseYear);
			this.PanelMain.Controls.Add(this.label1);
			this.PanelMain.Controls.Add(this.TbxTitleName);
			this.PanelMain.Controls.Add(this.lblName);
			this.PanelMain.Dock = System.Windows.Forms.DockStyle.Right;
			this.PanelMain.Location = new System.Drawing.Point(319, 24);
			this.PanelMain.Name = "PanelMain";
			this.PanelMain.Size = new System.Drawing.Size(635, 700);
			this.PanelMain.TabIndex = 3;
			// 
			// BtnOpenBrowser
			// 
			this.BtnOpenBrowser.Location = new System.Drawing.Point(436, 152);
			this.BtnOpenBrowser.Name = "BtnOpenBrowser";
			this.BtnOpenBrowser.Size = new System.Drawing.Size(44, 23);
			this.BtnOpenBrowser.TabIndex = 26;
			this.BtnOpenBrowser.Text = "Go";
			this.BtnOpenBrowser.UseVisualStyleBackColor = true;
			// 
			// TbxEpisode
			// 
			this.TbxEpisode.Location = new System.Drawing.Point(213, 154);
			this.TbxEpisode.Name = "TbxEpisode";
			this.TbxEpisode.Size = new System.Drawing.Size(30, 20);
			this.TbxEpisode.TabIndex = 25;
			this.TbxEpisode.TextChanged += new System.EventHandler(this.OnTextChanged);
			this.TbxEpisode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressNumericOnly);
			// 
			// LblEpisode
			// 
			this.LblEpisode.AutoSize = true;
			this.LblEpisode.Location = new System.Drawing.Point(181, 157);
			this.LblEpisode.Name = "LblEpisode";
			this.LblEpisode.Size = new System.Drawing.Size(26, 13);
			this.LblEpisode.TabIndex = 24;
			this.LblEpisode.Text = "Ep.:";
			// 
			// TbxDisk
			// 
			this.TbxDisk.Location = new System.Drawing.Point(145, 154);
			this.TbxDisk.Name = "TbxDisk";
			this.TbxDisk.Size = new System.Drawing.Size(30, 20);
			this.TbxDisk.TabIndex = 23;
			this.TbxDisk.TextChanged += new System.EventHandler(this.OnTextChanged);
			this.TbxDisk.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressNumericOnly);
			// 
			// LblDisk
			// 
			this.LblDisk.AutoSize = true;
			this.LblDisk.Location = new System.Drawing.Point(108, 157);
			this.LblDisk.Name = "LblDisk";
			this.LblDisk.Size = new System.Drawing.Size(31, 13);
			this.LblDisk.TabIndex = 22;
			this.LblDisk.Text = "Disk:";
			// 
			// TbxSeason
			// 
			this.TbxSeason.Location = new System.Drawing.Point(72, 154);
			this.TbxSeason.Name = "TbxSeason";
			this.TbxSeason.Size = new System.Drawing.Size(30, 20);
			this.TbxSeason.TabIndex = 21;
			this.TbxSeason.TextChanged += new System.EventHandler(this.OnTextChanged);
			this.TbxSeason.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressNumericOnly);
			// 
			// LblSeason
			// 
			this.LblSeason.AutoSize = true;
			this.LblSeason.Location = new System.Drawing.Point(20, 157);
			this.LblSeason.Name = "LblSeason";
			this.LblSeason.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.LblSeason.Size = new System.Drawing.Size(46, 13);
			this.LblSeason.TabIndex = 20;
			this.LblSeason.Text = "Season:";
			// 
			// BtnSearhProvider
			// 
			this.BtnSearhProvider.Location = new System.Drawing.Point(486, 152);
			this.BtnSearhProvider.Name = "BtnSearhProvider";
			this.BtnSearhProvider.Size = new System.Drawing.Size(53, 23);
			this.BtnSearhProvider.TabIndex = 0;
			this.BtnSearhProvider.Text = "Search";
			this.BtnSearhProvider.UseMnemonic = false;
			this.BtnSearhProvider.UseVisualStyleBackColor = true;
			this.BtnSearhProvider.Click += new System.EventHandler(this.BtnSearhProvider_Click);
			// 
			// TbxImdbId
			// 
			this.TbxImdbId.Location = new System.Drawing.Point(316, 154);
			this.TbxImdbId.Name = "TbxImdbId";
			this.TbxImdbId.Size = new System.Drawing.Size(114, 20);
			this.TbxImdbId.TabIndex = 19;
			this.TbxImdbId.TextChanged += new System.EventHandler(this.OnTextChanged);
			this.TbxImdbId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressNumericOnly);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(261, 157);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(49, 13);
			this.label5.TabIndex = 18;
			this.label5.Text = "IMDB Id:";
			// 
			// CbxKind
			// 
			this.CbxKind.FormattingEnabled = true;
			this.CbxKind.Location = new System.Drawing.Point(545, 4);
			this.CbxKind.Name = "CbxKind";
			this.CbxKind.Size = new System.Drawing.Size(81, 21);
			this.CbxKind.TabIndex = 17;
			this.CbxKind.SelectedIndexChanged += new System.EventHandler(this.CbxKind_SelectedIndexChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(508, 7);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(31, 13);
			this.label3.TabIndex = 16;
			this.label3.Text = "Kind:";
			// 
			// BtnNew
			// 
			this.BtnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnNew.Location = new System.Drawing.Point(377, 672);
			this.BtnNew.Name = "BtnNew";
			this.BtnNew.Size = new System.Drawing.Size(93, 23);
			this.BtnNew.TabIndex = 15;
			this.BtnNew.Text = "New Entry";
			this.BtnNew.UseVisualStyleBackColor = true;
			this.BtnNew.Click += new System.EventHandler(this.BtnNew_Click);
			// 
			// BtnDeleteImage
			// 
			this.BtnDeleteImage.Image = global::MediaCollection.Resources.Remove_16xMD;
			this.BtnDeleteImage.Location = new System.Drawing.Point(9, 275);
			this.BtnDeleteImage.Name = "BtnDeleteImage";
			this.BtnDeleteImage.Size = new System.Drawing.Size(25, 25);
			this.BtnDeleteImage.TabIndex = 14;
			this.BtnDeleteImage.UseVisualStyleBackColor = true;
			this.BtnDeleteImage.Click += new System.EventHandler(this.BtnDeleteImage_Click);
			// 
			// BtnAddImage
			// 
			this.BtnAddImage.Image = global::MediaCollection.Resources.action_add_16xMD;
			this.BtnAddImage.Location = new System.Drawing.Point(41, 275);
			this.BtnAddImage.Name = "BtnAddImage";
			this.BtnAddImage.Size = new System.Drawing.Size(25, 25);
			this.BtnAddImage.TabIndex = 13;
			this.BtnAddImage.UseVisualStyleBackColor = true;
			this.BtnAddImage.Click += new System.EventHandler(this.BtnAddImage_Click);
			// 
			// BtnNextImage
			// 
			this.BtnNextImage.Image = global::MediaCollection.Resources.arrow_Forward_color_32xSM;
			this.BtnNextImage.Location = new System.Drawing.Point(337, 275);
			this.BtnNextImage.Name = "BtnNextImage";
			this.BtnNextImage.Size = new System.Drawing.Size(25, 25);
			this.BtnNextImage.TabIndex = 12;
			this.BtnNextImage.UseVisualStyleBackColor = true;
			this.BtnNextImage.Click += new System.EventHandler(this.BtnNextImage_Click);
			// 
			// BtnPrevImage
			// 
			this.BtnPrevImage.Image = global::MediaCollection.Resources.arrow_back_color_32xSM;
			this.BtnPrevImage.Location = new System.Drawing.Point(303, 275);
			this.BtnPrevImage.Name = "BtnPrevImage";
			this.BtnPrevImage.Size = new System.Drawing.Size(25, 25);
			this.BtnPrevImage.TabIndex = 11;
			this.BtnPrevImage.UseVisualStyleBackColor = true;
			this.BtnPrevImage.Click += new System.EventHandler(this.BtnPrevImage_Click);
			// 
			// PbxImage
			// 
			this.PbxImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.PbxImage.Location = new System.Drawing.Point(9, 306);
			this.PbxImage.Name = "PbxImage";
			this.PbxImage.Size = new System.Drawing.Size(353, 389);
			this.PbxImage.TabIndex = 10;
			this.PbxImage.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.LVRatings);
			this.groupBox2.Location = new System.Drawing.Point(371, 275);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(255, 202);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Ratings";
			// 
			// LVRatings
			// 
			this.LVRatings.AllColumns.Add(this.olvColumnRatingName);
			this.LVRatings.AllColumns.Add(this.olvColumnRatingValue);
			this.LVRatings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LVRatings.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.LVRatings.CellEditUseWholeCell = false;
			this.LVRatings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnRatingName,
            this.olvColumnRatingValue});
			this.LVRatings.Cursor = System.Windows.Forms.Cursors.Default;
			this.LVRatings.FullRowSelect = true;
			this.LVRatings.GridLines = true;
			this.LVRatings.Location = new System.Drawing.Point(6, 19);
			this.LVRatings.MinimumSize = new System.Drawing.Size(243, 167);
			this.LVRatings.Name = "LVRatings";
			this.LVRatings.ShowGroups = false;
			this.LVRatings.Size = new System.Drawing.Size(243, 167);
			this.LVRatings.TabIndex = 14;
			this.LVRatings.UseCompatibleStateImageBehavior = false;
			this.LVRatings.View = System.Windows.Forms.View.Details;
			this.LVRatings.VirtualMode = true;
			this.LVRatings.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.LVRatings_CellEditFinished);
			this.LVRatings.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.LVRatings_CellEditStarting);
			// 
			// olvColumnRatingName
			// 
			this.olvColumnRatingName.AspectName = "RatingName";
			this.olvColumnRatingName.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnRatingName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnRatingName.Text = "Rating";
			this.olvColumnRatingName.Width = 165;
			// 
			// olvColumnRatingValue
			// 
			this.olvColumnRatingValue.AspectName = "RatingValue";
			this.olvColumnRatingValue.AspectToStringFormat = "{0:0.#;\"\";\"\"}";
			this.olvColumnRatingValue.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnRatingValue.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnRatingValue.Text = "Score";
			this.olvColumnRatingValue.Width = 55;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.BtnRemoveLocation);
			this.groupBox1.Controls.Add(this.BtnAddLocation);
			this.groupBox1.Controls.Add(this.CbxDevices);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.LVLocations);
			this.groupBox1.Location = new System.Drawing.Point(9, 178);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(617, 91);
			this.groupBox1.TabIndex = 8;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Location";
			// 
			// BtnRemoveLocation
			// 
			this.BtnRemoveLocation.Image = global::MediaCollection.Resources.Remove_16xMD;
			this.BtnRemoveLocation.Location = new System.Drawing.Point(554, 60);
			this.BtnRemoveLocation.Name = "BtnRemoveLocation";
			this.BtnRemoveLocation.Size = new System.Drawing.Size(25, 25);
			this.BtnRemoveLocation.TabIndex = 19;
			this.BtnRemoveLocation.UseVisualStyleBackColor = true;
			this.BtnRemoveLocation.Click += new System.EventHandler(this.BtnRemoveLocation_Click);
			// 
			// BtnAddLocation
			// 
			this.BtnAddLocation.Image = global::MediaCollection.Resources.action_add_16xMD;
			this.BtnAddLocation.Location = new System.Drawing.Point(586, 60);
			this.BtnAddLocation.Name = "BtnAddLocation";
			this.BtnAddLocation.Size = new System.Drawing.Size(25, 25);
			this.BtnAddLocation.TabIndex = 18;
			this.BtnAddLocation.UseVisualStyleBackColor = true;
			// 
			// CbxDevices
			// 
			this.CbxDevices.FormattingEnabled = true;
			this.CbxDevices.Location = new System.Drawing.Point(102, 63);
			this.CbxDevices.Name = "CbxDevices";
			this.CbxDevices.Size = new System.Drawing.Size(156, 21);
			this.CbxDevices.TabIndex = 16;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(7, 66);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(89, 13);
			this.label4.TabIndex = 15;
			this.label4.Text = "Playback device:";
			// 
			// LVLocations
			// 
			this.LVLocations.AllColumns.Add(this.OlvBtnPlay);
			this.LVLocations.AllColumns.Add(this.OlvBaseLocationName);
			this.LVLocations.AllColumns.Add(this.OlvLocationData);
			this.LVLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LVLocations.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.LVLocations.CellEditUseWholeCell = false;
			this.LVLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.OlvBtnPlay,
            this.OlvBaseLocationName,
            this.OlvLocationData});
			this.LVLocations.Cursor = System.Windows.Forms.Cursors.Default;
			this.LVLocations.FullRowSelect = true;
			this.LVLocations.GridLines = true;
			this.LVLocations.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			this.LVLocations.Location = new System.Drawing.Point(6, 19);
			this.LVLocations.Name = "LVLocations";
			this.LVLocations.ShowGroups = false;
			this.LVLocations.Size = new System.Drawing.Size(605, 40);
			this.LVLocations.SmallImageList = this.TreeImageList;
			this.LVLocations.TabIndex = 14;
			this.LVLocations.UseCompatibleStateImageBehavior = false;
			this.LVLocations.View = System.Windows.Forms.View.Details;
			this.LVLocations.VirtualMode = true;
			this.LVLocations.ButtonClick += new System.EventHandler<BrightIdeasSoftware.CellClickEventArgs>(this.LVLocations_ButtonClick);
			this.LVLocations.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.LVLocations_CellEditFinishing);
			this.LVLocations.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.LVLocations_CellEditStarting);
			// 
			// OlvBtnPlay
			// 
			this.OlvBtnPlay.AutoCompleteEditor = false;
			this.OlvBtnPlay.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
			this.OlvBtnPlay.ButtonSize = new System.Drawing.Size(40, 16);
			this.OlvBtnPlay.ButtonSizing = BrightIdeasSoftware.OLVColumn.ButtonSizingMode.FixedBounds;
			this.OlvBtnPlay.CellVerticalAlignment = System.Drawing.StringAlignment.Center;
			this.OlvBtnPlay.IsButton = true;
			this.OlvBtnPlay.IsEditable = false;
			this.OlvBtnPlay.Searchable = false;
			this.OlvBtnPlay.Sortable = false;
			this.OlvBtnPlay.Text = "Play";
			this.OlvBtnPlay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.OlvBtnPlay.UseFiltering = false;
			this.OlvBtnPlay.Width = 50;
			// 
			// OlvBaseLocationName
			// 
			this.OlvBaseLocationName.AspectName = "LocationBase";
			this.OlvBaseLocationName.Width = 200;
			// 
			// OlvLocationData
			// 
			this.OlvLocationData.AspectName = "LocationData";
			this.OlvLocationData.CellEditUseWholeCell = true;
			this.OlvLocationData.FillsFreeSpace = true;
			this.OlvLocationData.Width = 300;
			// 
			// TbxDescription
			// 
			this.TbxDescription.Location = new System.Drawing.Point(72, 31);
			this.TbxDescription.Multiline = true;
			this.TbxDescription.Name = "TbxDescription";
			this.TbxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.TbxDescription.Size = new System.Drawing.Size(554, 117);
			this.TbxDescription.TabIndex = 7;
			this.TbxDescription.TextChanged += new System.EventHandler(this.OnTextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 31);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(63, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Description:";
			// 
			// BtnDiscard
			// 
			this.BtnDiscard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnDiscard.Location = new System.Drawing.Point(533, 672);
			this.BtnDiscard.Name = "BtnDiscard";
			this.BtnDiscard.Size = new System.Drawing.Size(93, 23);
			this.BtnDiscard.TabIndex = 5;
			this.BtnDiscard.Text = "Discard";
			this.BtnDiscard.UseVisualStyleBackColor = true;
			this.BtnDiscard.Click += new System.EventHandler(this.BtnDiscard_Click);
			// 
			// BtnSave
			// 
			this.BtnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnSave.Enabled = false;
			this.BtnSave.Location = new System.Drawing.Point(563, 152);
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.Size = new System.Drawing.Size(63, 23);
			this.BtnSave.TabIndex = 4;
			this.BtnSave.Text = "Save";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// TbxReleaseYear
			// 
			this.TbxReleaseYear.Location = new System.Drawing.Point(436, 4);
			this.TbxReleaseYear.Name = "TbxReleaseYear";
			this.TbxReleaseYear.Size = new System.Drawing.Size(66, 20);
			this.TbxReleaseYear.TabIndex = 3;
			this.TbxReleaseYear.TextChanged += new System.EventHandler(this.OnTextChanged);
			this.TbxReleaseYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OnKeyPressNumericOnly);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(398, 7);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(32, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Year:";
			// 
			// TbxTitleName
			// 
			this.TbxTitleName.Location = new System.Drawing.Point(72, 4);
			this.TbxTitleName.Name = "TbxTitleName";
			this.TbxTitleName.Size = new System.Drawing.Size(320, 20);
			this.TbxTitleName.TabIndex = 1;
			this.TbxTitleName.TextChanged += new System.EventHandler(this.OnTextChanged);
			// 
			// lblName
			// 
			this.lblName.AutoSize = true;
			this.lblName.Location = new System.Drawing.Point(31, 7);
			this.lblName.Name = "lblName";
			this.lblName.Size = new System.Drawing.Size(38, 13);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "Name:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(954, 724);
			this.Controls.Add(this.PanelMain);
			this.Controls.Add(this.PanelLeft);
			this.Controls.Add(this.menuStrip);
			this.MainMenuStrip = this.menuStrip;
			this.Name = "MainForm";
			this.Text = "Media Collection Desktop";
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.PanelLeft.ResumeLayout(false);
			this.PanelLeft.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TVTitles)).EndInit();
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.PanelMain.ResumeLayout(false);
			this.PanelMain.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbxImage)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.LVRatings)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.LVLocations)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Panel PanelLeft;
		private BrightIdeasSoftware.TreeListView TVTitles;
		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem TSMITitles;
		private System.Windows.Forms.ToolStripMenuItem TSMIBulkAdd;
		private System.Windows.Forms.ToolStripMenuItem TSMIAddManually;
		private System.Windows.Forms.ToolStripMenuItem TSMIRemoveTitle;
		private System.Windows.Forms.ToolStripMenuItem TSMITitlesConfig;
		private System.Windows.Forms.ToolStripMenuItem TSMIDevices;
		private System.Windows.Forms.ToolStripMenuItem TSMIBulkAddLocations;
		private System.Windows.Forms.ToolStripMenuItem TSMISystem;
		private System.Windows.Forms.Panel PanelMain;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.TextBox TbxReleaseYear;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TbxTitleName;
		private System.Windows.Forms.Label lblName;
		private BrightIdeasSoftware.OLVColumn OlvColumnName;
		private System.Windows.Forms.Button BtnDiscard;
		private System.Windows.Forms.TextBox TbxDescription;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button BtnNextImage;
		private System.Windows.Forms.Button BtnPrevImage;
		private System.Windows.Forms.PictureBox PbxImage;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button BtnAddImage;
		private System.Windows.Forms.Button BtnDeleteImage;
		private System.Windows.Forms.ComboBox CbxKind;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button BtnNew;
		private System.Windows.Forms.TextBox TbxSearch;
		private System.Windows.Forms.RadioButton RbnAudio;
		private System.Windows.Forms.RadioButton RbnVideo;
		private System.Windows.Forms.ImageList TreeImageList;
		private BrightIdeasSoftware.ObjectListView LVRatings;
		private BrightIdeasSoftware.OLVColumn olvColumnRatingName;
		private BrightIdeasSoftware.OLVColumn olvColumnRatingValue;
		private BrightIdeasSoftware.ObjectListView LVLocations;
		private BrightIdeasSoftware.OLVColumn OlvBtnPlay;
		private BrightIdeasSoftware.OLVColumn OlvBaseLocationName;
		private BrightIdeasSoftware.OLVColumn OlvLocationData;
		private System.Windows.Forms.Button BtnRemoveLocation;
		private System.Windows.Forms.Button BtnAddLocation;
		private System.Windows.Forms.ComboBox CbxDevices;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox TbxImdbId;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button BtnSearhProvider;
		private System.Windows.Forms.ToolStripMenuItem FilterToolStripMenuItem;
		private System.Windows.Forms.TextBox TbxEpisode;
		private System.Windows.Forms.Label LblEpisode;
		private System.Windows.Forms.TextBox TbxDisk;
		private System.Windows.Forms.Label LblDisk;
		private System.Windows.Forms.TextBox TbxSeason;
		private System.Windows.Forms.Label LblSeason;
		private System.Windows.Forms.Button BtnOpenBrowser;
		private System.Windows.Forms.ToolStripMenuItem updateAllFromTMDBToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem RatingsToolStripMenuItem;
	}
}

