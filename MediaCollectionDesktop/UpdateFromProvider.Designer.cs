namespace MediaCollection
{
	partial class UpdateFromProvider
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
            this.BtnPrevious = new System.Windows.Forms.Button();
            this.BtnNext = new System.Windows.Forms.Button();
            this.TbxName = new System.Windows.Forms.TextBox();
            this.BtnSearch = new System.Windows.Forms.Button();
            this.LVResults = new BrightIdeasSoftware.ObjectListView();
            this.OlvPoster = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.ImageListPosters = new System.Windows.Forms.ImageList(this.components);
            this.groupBoxApplyWhat = new System.Windows.Forms.GroupBox();
            this.CbxOverrideDescription = new System.Windows.Forms.CheckBox();
            this.CbxOverrideYear = new System.Windows.Forms.CheckBox();
            this.CbxOverrideTitle = new System.Windows.Forms.CheckBox();
            this.TbxDescription = new System.Windows.Forms.TextBox();
            this.TbxReleaseYear = new System.Windows.Forms.TextBox();
            this.RbnMovie = new System.Windows.Forms.RadioButton();
            this.RbnTv = new System.Windows.Forms.RadioButton();
            this.groupBoxKind = new System.Windows.Forms.GroupBox();
            this.GbxManualEntry = new System.Windows.Forms.GroupBox();
            this.buttonManSave = new System.Windows.Forms.Button();
            this.labelManDescription = new System.Windows.Forms.Label();
            this.labelManYear = new System.Windows.Forms.Label();
            this.BtnSaveForInspection = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.LVResults)).BeginInit();
            this.groupBoxApplyWhat.SuspendLayout();
            this.groupBoxKind.SuspendLayout();
            this.GbxManualEntry.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnPrevious
            // 
            this.BtnPrevious.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnPrevious.Location = new System.Drawing.Point(12, 28);
            this.BtnPrevious.Name = "BtnPrevious";
            this.BtnPrevious.Size = new System.Drawing.Size(75, 23);
            this.BtnPrevious.TabIndex = 0;
            this.BtnPrevious.Text = "Previous";
            this.BtnPrevious.UseVisualStyleBackColor = true;
            this.BtnPrevious.Click += new System.EventHandler(this.BtnPrevious_Click);
            // 
            // BtnNext
            // 
            this.BtnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnNext.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BtnNext.Location = new System.Drawing.Point(838, 28);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(75, 23);
            this.BtnNext.TabIndex = 1;
            this.BtnNext.Text = "Next";
            this.BtnNext.UseVisualStyleBackColor = true;
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // TbxName
            // 
            this.TbxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbxName.Location = new System.Drawing.Point(93, 28);
            this.TbxName.Name = "TbxName";
            this.TbxName.Size = new System.Drawing.Size(328, 20);
            this.TbxName.TabIndex = 2;
            this.TbxName.TextChanged += new System.EventHandler(this.TbxName_TextChanged);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnSearch.Location = new System.Drawing.Point(549, 28);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(75, 23);
            this.BtnSearch.TabIndex = 3;
            this.BtnSearch.Text = "Search";
            this.BtnSearch.UseVisualStyleBackColor = true;
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // LVResults
            // 
            this.LVResults.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.LVResults.AllColumns.Add(this.OlvPoster);
            this.LVResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LVResults.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
            this.LVResults.CellEditUseWholeCell = false;
            this.LVResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.OlvPoster});
            this.LVResults.Cursor = System.Windows.Forms.Cursors.Default;
            this.LVResults.FullRowSelect = true;
            this.LVResults.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.LVResults.LargeImageList = this.ImageListPosters;
            this.LVResults.Location = new System.Drawing.Point(12, 62);
            this.LVResults.Name = "LVResults";
            this.LVResults.RowHeight = 256;
            this.LVResults.ShowGroups = false;
            this.LVResults.Size = new System.Drawing.Size(901, 432);
            this.LVResults.SmallImageList = this.ImageListPosters;
            this.LVResults.TabIndex = 15;
            this.LVResults.UseCompatibleStateImageBehavior = false;
            this.LVResults.View = System.Windows.Forms.View.LargeIcon;
            this.LVResults.CellToolTipShowing += new System.EventHandler<BrightIdeasSoftware.ToolTipShowingEventArgs>(this.LVResults_CellToolTipShowing);
            this.LVResults.DoubleClick += new System.EventHandler(this.LVResults_DoubleClick);
            // 
            // OlvPoster
            // 
            this.OlvPoster.AspectName = "Title";
            this.OlvPoster.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OlvPoster.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.OlvPoster.ImageAspectName = "PosterName";
            this.OlvPoster.IsEditable = false;
            this.OlvPoster.IsTileViewColumn = true;
            this.OlvPoster.Text = "Poster";
            this.OlvPoster.Width = 130;
            // 
            // ImageListPosters
            // 
            this.ImageListPosters.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ImageListPosters.ImageSize = new System.Drawing.Size(160, 240);
            this.ImageListPosters.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // groupBoxApplyWhat
            // 
            this.groupBoxApplyWhat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxApplyWhat.Controls.Add(this.CbxOverrideDescription);
            this.groupBoxApplyWhat.Controls.Add(this.CbxOverrideYear);
            this.groupBoxApplyWhat.Controls.Add(this.CbxOverrideTitle);
            this.groupBoxApplyWhat.Location = new System.Drawing.Point(630, 16);
            this.groupBoxApplyWhat.Name = "groupBoxApplyWhat";
            this.groupBoxApplyWhat.Size = new System.Drawing.Size(202, 40);
            this.groupBoxApplyWhat.TabIndex = 16;
            this.groupBoxApplyWhat.TabStop = false;
            this.groupBoxApplyWhat.Text = "Override On Apply";
            this.groupBoxApplyWhat.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // CbxOverrideDescription
            // 
            this.CbxOverrideDescription.AutoSize = true;
            this.CbxOverrideDescription.Checked = true;
            this.CbxOverrideDescription.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverrideDescription.Location = new System.Drawing.Point(120, 16);
            this.CbxOverrideDescription.Name = "CbxOverrideDescription";
            this.CbxOverrideDescription.Size = new System.Drawing.Size(79, 17);
            this.CbxOverrideDescription.TabIndex = 3;
            this.CbxOverrideDescription.Text = "Description";
            this.CbxOverrideDescription.UseVisualStyleBackColor = true;
            // 
            // CbxOverrideYear
            // 
            this.CbxOverrideYear.AutoSize = true;
            this.CbxOverrideYear.Checked = true;
            this.CbxOverrideYear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverrideYear.Location = new System.Drawing.Point(66, 16);
            this.CbxOverrideYear.Name = "CbxOverrideYear";
            this.CbxOverrideYear.Size = new System.Drawing.Size(48, 17);
            this.CbxOverrideYear.TabIndex = 1;
            this.CbxOverrideYear.Text = "Year";
            this.CbxOverrideYear.UseVisualStyleBackColor = true;
            // 
            // CbxOverrideTitle
            // 
            this.CbxOverrideTitle.AutoSize = true;
            this.CbxOverrideTitle.Checked = true;
            this.CbxOverrideTitle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbxOverrideTitle.Location = new System.Drawing.Point(6, 16);
            this.CbxOverrideTitle.Name = "CbxOverrideTitle";
            this.CbxOverrideTitle.Size = new System.Drawing.Size(54, 17);
            this.CbxOverrideTitle.TabIndex = 0;
            this.CbxOverrideTitle.Text = "Name";
            this.CbxOverrideTitle.UseVisualStyleBackColor = true;
            // 
            // TbxDescription
            // 
            this.TbxDescription.Location = new System.Drawing.Point(79, 43);
            this.TbxDescription.Multiline = true;
            this.TbxDescription.Name = "TbxDescription";
            this.TbxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TbxDescription.Size = new System.Drawing.Size(816, 62);
            this.TbxDescription.TabIndex = 5;
            // 
            // TbxReleaseYear
            // 
            this.TbxReleaseYear.Location = new System.Drawing.Point(79, 17);
            this.TbxReleaseYear.Name = "TbxReleaseYear";
            this.TbxReleaseYear.Size = new System.Drawing.Size(75, 20);
            this.TbxReleaseYear.TabIndex = 4;
            this.TbxReleaseYear.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressNumericOnly);
            // 
            // RbnMovie
            // 
            this.RbnMovie.AutoSize = true;
            this.RbnMovie.Checked = true;
            this.RbnMovie.Location = new System.Drawing.Point(6, 18);
            this.RbnMovie.Name = "RbnMovie";
            this.RbnMovie.Size = new System.Drawing.Size(54, 17);
            this.RbnMovie.TabIndex = 17;
            this.RbnMovie.TabStop = true;
            this.RbnMovie.Text = "Movie";
            this.RbnMovie.UseVisualStyleBackColor = true;
            // 
            // RbnTv
            // 
            this.RbnTv.AutoSize = true;
            this.RbnTv.Location = new System.Drawing.Point(66, 17);
            this.RbnTv.Name = "RbnTv";
            this.RbnTv.Size = new System.Drawing.Size(39, 17);
            this.RbnTv.TabIndex = 18;
            this.RbnTv.TabStop = true;
            this.RbnTv.Text = "TV";
            this.RbnTv.UseVisualStyleBackColor = true;
            // 
            // groupBoxKind
            // 
            this.groupBoxKind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxKind.Controls.Add(this.RbnMovie);
            this.groupBoxKind.Controls.Add(this.RbnTv);
            this.groupBoxKind.Location = new System.Drawing.Point(427, 16);
            this.groupBoxKind.Name = "groupBoxKind";
            this.groupBoxKind.Size = new System.Drawing.Size(116, 40);
            this.groupBoxKind.TabIndex = 19;
            this.groupBoxKind.TabStop = false;
            this.groupBoxKind.Text = "Kind";
            // 
            // GbxManualEntry
            // 
            this.GbxManualEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GbxManualEntry.Controls.Add(this.BtnSaveForInspection);
            this.GbxManualEntry.Controls.Add(this.buttonManSave);
            this.GbxManualEntry.Controls.Add(this.labelManDescription);
            this.GbxManualEntry.Controls.Add(this.labelManYear);
            this.GbxManualEntry.Controls.Add(this.TbxReleaseYear);
            this.GbxManualEntry.Controls.Add(this.TbxDescription);
            this.GbxManualEntry.Location = new System.Drawing.Point(12, 501);
            this.GbxManualEntry.Name = "GbxManualEntry";
            this.GbxManualEntry.Size = new System.Drawing.Size(901, 112);
            this.GbxManualEntry.TabIndex = 20;
            this.GbxManualEntry.TabStop = false;
            this.GbxManualEntry.Text = "Manual Entry";
            // 
            // buttonManSave
            // 
            this.buttonManSave.Location = new System.Drawing.Point(777, 15);
            this.buttonManSave.Name = "buttonManSave";
            this.buttonManSave.Size = new System.Drawing.Size(118, 23);
            this.buttonManSave.TabIndex = 8;
            this.buttonManSave.Text = "Save Manual Entry";
            this.buttonManSave.UseVisualStyleBackColor = true;
            this.buttonManSave.Click += new System.EventHandler(this.buttonManSave_Click);
            // 
            // labelManDescription
            // 
            this.labelManDescription.AutoSize = true;
            this.labelManDescription.Location = new System.Drawing.Point(10, 48);
            this.labelManDescription.Name = "labelManDescription";
            this.labelManDescription.Size = new System.Drawing.Size(63, 13);
            this.labelManDescription.TabIndex = 7;
            this.labelManDescription.Text = "Description:";
            // 
            // labelManYear
            // 
            this.labelManYear.AutoSize = true;
            this.labelManYear.Location = new System.Drawing.Point(7, 20);
            this.labelManYear.Name = "labelManYear";
            this.labelManYear.Size = new System.Drawing.Size(32, 13);
            this.labelManYear.TabIndex = 6;
            this.labelManYear.Text = "Year:";
            // 
            // BtnSaveForInspection
            // 
            this.BtnSaveForInspection.Location = new System.Drawing.Point(624, 15);
            this.BtnSaveForInspection.Name = "BtnSaveForInspection";
            this.BtnSaveForInspection.Size = new System.Drawing.Size(147, 23);
            this.BtnSaveForInspection.TabIndex = 9;
            this.BtnSaveForInspection.Text = "Save For Inspection";
            this.BtnSaveForInspection.UseVisualStyleBackColor = true;
            this.BtnSaveForInspection.Click += new System.EventHandler(this.BtnSaveForInspection_Click);
            // 
            // UpdateFromProvider
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(925, 618);
            this.Controls.Add(this.GbxManualEntry);
            this.Controls.Add(this.groupBoxKind);
            this.Controls.Add(this.groupBoxApplyWhat);
            this.Controls.Add(this.LVResults);
            this.Controls.Add(this.BtnSearch);
            this.Controls.Add(this.TbxName);
            this.Controls.Add(this.BtnNext);
            this.Controls.Add(this.BtnPrevious);
            this.MinimumSize = new System.Drawing.Size(730, 576);
            this.Name = "UpdateFromProvider";
            this.Text = "Update From Provider";
            ((System.ComponentModel.ISupportInitialize)(this.LVResults)).EndInit();
            this.groupBoxApplyWhat.ResumeLayout(false);
            this.groupBoxApplyWhat.PerformLayout();
            this.groupBoxKind.ResumeLayout(false);
            this.groupBoxKind.PerformLayout();
            this.GbxManualEntry.ResumeLayout(false);
            this.GbxManualEntry.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BtnPrevious;
		private System.Windows.Forms.Button BtnNext;
		private System.Windows.Forms.TextBox TbxName;
		private System.Windows.Forms.Button BtnSearch;
		private BrightIdeasSoftware.ObjectListView LVResults;
		private BrightIdeasSoftware.OLVColumn OlvPoster;
		private System.Windows.Forms.GroupBox groupBoxApplyWhat;
		private System.Windows.Forms.CheckBox CbxOverrideYear;
		private System.Windows.Forms.CheckBox CbxOverrideTitle;
		private System.Windows.Forms.CheckBox CbxOverrideDescription;
		private System.Windows.Forms.ImageList ImageListPosters;
		private System.Windows.Forms.RadioButton RbnMovie;
		private System.Windows.Forms.RadioButton RbnTv;
		private System.Windows.Forms.TextBox TbxDescription;
		private System.Windows.Forms.TextBox TbxReleaseYear;
		private System.Windows.Forms.GroupBox groupBoxKind;
		private System.Windows.Forms.GroupBox GbxManualEntry;
		private System.Windows.Forms.Label labelManDescription;
		private System.Windows.Forms.Label labelManYear;
		private System.Windows.Forms.Button buttonManSave;
        private System.Windows.Forms.Button BtnSaveForInspection;
    }
}