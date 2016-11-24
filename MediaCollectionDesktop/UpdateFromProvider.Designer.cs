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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.CbxOverrideDescription = new System.Windows.Forms.CheckBox();
			this.CbxOverrideYear = new System.Windows.Forms.CheckBox();
			this.CbxOverrideTitle = new System.Windows.Forms.CheckBox();
			this.RbnMovie = new System.Windows.Forms.RadioButton();
			this.RbnTv = new System.Windows.Forms.RadioButton();
			((System.ComponentModel.ISupportInitialize)(this.LVResults)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// BtnPrevious
			// 
			this.BtnPrevious.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BtnPrevious.Location = new System.Drawing.Point(12, 12);
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
			this.BtnNext.Location = new System.Drawing.Point(627, 13);
			this.BtnNext.Name = "BtnNext";
			this.BtnNext.Size = new System.Drawing.Size(75, 23);
			this.BtnNext.TabIndex = 1;
			this.BtnNext.Text = "Next";
			this.BtnNext.UseVisualStyleBackColor = true;
			this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
			// 
			// TbxName
			// 
			this.TbxName.Location = new System.Drawing.Point(94, 13);
			this.TbxName.Name = "TbxName";
			this.TbxName.Size = new System.Drawing.Size(216, 20);
			this.TbxName.TabIndex = 2;
			this.TbxName.TextChanged += new System.EventHandler(this.TbxName_TextChanged);
			// 
			// BtnSearch
			// 
			this.BtnSearch.Location = new System.Drawing.Point(316, 13);
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
			this.LVResults.Size = new System.Drawing.Size(690, 417);
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
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.CbxOverrideDescription);
			this.groupBox1.Controls.Add(this.CbxOverrideYear);
			this.groupBox1.Controls.Add(this.CbxOverrideTitle);
			this.groupBox1.Location = new System.Drawing.Point(13, 485);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(689, 40);
			this.groupBox1.TabIndex = 16;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Override On Apply";
			// 
			// CbxOverrideDescription
			// 
			this.CbxOverrideDescription.AutoSize = true;
			this.CbxOverrideDescription.Checked = true;
			this.CbxOverrideDescription.CheckState = System.Windows.Forms.CheckState.Checked;
			this.CbxOverrideDescription.Location = new System.Drawing.Point(7, 17);
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
			this.CbxOverrideYear.Location = new System.Drawing.Point(152, 17);
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
			this.CbxOverrideTitle.Location = new System.Drawing.Point(92, 17);
			this.CbxOverrideTitle.Name = "CbxOverrideTitle";
			this.CbxOverrideTitle.Size = new System.Drawing.Size(54, 17);
			this.CbxOverrideTitle.TabIndex = 0;
			this.CbxOverrideTitle.Text = "Name";
			this.CbxOverrideTitle.UseVisualStyleBackColor = true;
			// 
			// RbnMovie
			// 
			this.RbnMovie.AutoCheck = false;
			this.RbnMovie.AutoSize = true;
			this.RbnMovie.Checked = true;
			this.RbnMovie.Location = new System.Drawing.Point(94, 40);
			this.RbnMovie.Name = "RbnMovie";
			this.RbnMovie.Size = new System.Drawing.Size(54, 17);
			this.RbnMovie.TabIndex = 17;
			this.RbnMovie.Text = "Movie";
			this.RbnMovie.UseVisualStyleBackColor = true;
			// 
			// RbnTv
			// 
			this.RbnTv.AutoSize = true;
			this.RbnTv.Location = new System.Drawing.Point(155, 40);
			this.RbnTv.Name = "RbnTv";
			this.RbnTv.Size = new System.Drawing.Size(39, 17);
			this.RbnTv.TabIndex = 18;
			this.RbnTv.TabStop = true;
			this.RbnTv.Text = "TV";
			this.RbnTv.UseVisualStyleBackColor = true;
			// 
			// UpdateFromProvider
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(714, 537);
			this.Controls.Add(this.RbnTv);
			this.Controls.Add(this.RbnMovie);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.LVResults);
			this.Controls.Add(this.BtnSearch);
			this.Controls.Add(this.TbxName);
			this.Controls.Add(this.BtnNext);
			this.Controls.Add(this.BtnPrevious);
			this.MinimumSize = new System.Drawing.Size(730, 576);
			this.Name = "UpdateFromProvider";
			this.Text = "UpdateFromProvider";
			((System.ComponentModel.ISupportInitialize)(this.LVResults)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
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
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox CbxOverrideYear;
		private System.Windows.Forms.CheckBox CbxOverrideTitle;
		private System.Windows.Forms.CheckBox CbxOverrideDescription;
		private System.Windows.Forms.ImageList ImageListPosters;
		private System.Windows.Forms.RadioButton RbnMovie;
		private System.Windows.Forms.RadioButton RbnTv;
	}
}