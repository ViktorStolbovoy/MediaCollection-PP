namespace MediaCollection
{
	partial class Ratings
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
			this.BtnDelete = new System.Windows.Forms.Button();
			this.LVRatings = new BrightIdeasSoftware.ObjectListView();
			this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.BtnAdd = new System.Windows.Forms.Button();
			this.olvColumnRatingMin = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumnRatingMax = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumnRatingStep = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			((System.ComponentModel.ISupportInitialize)(this.LVRatings)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnDelete
			// 
			this.BtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnDelete.ForeColor = System.Drawing.Color.Red;
			this.BtnDelete.Location = new System.Drawing.Point(355, 32);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.Size = new System.Drawing.Size(75, 23);
			this.BtnDelete.TabIndex = 16;
			this.BtnDelete.Text = "Delete";
			this.BtnDelete.UseVisualStyleBackColor = true;
			this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
			// 
			// LVRatings
			// 
			this.LVRatings.AllColumns.Add(this.olvColumnName);
			this.LVRatings.AllColumns.Add(this.olvColumnRatingMin);
			this.LVRatings.AllColumns.Add(this.olvColumnRatingMax);
			this.LVRatings.AllColumns.Add(this.olvColumnRatingStep);
			this.LVRatings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LVRatings.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.LVRatings.CellEditUseWholeCell = false;
			this.LVRatings.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName,
            this.olvColumnRatingMin,
            this.olvColumnRatingMax,
            this.olvColumnRatingStep});
			this.LVRatings.Cursor = System.Windows.Forms.Cursors.Default;
			this.LVRatings.FullRowSelect = true;
			this.LVRatings.GridLines = true;
			this.LVRatings.Location = new System.Drawing.Point(3, 3);
			this.LVRatings.Name = "LVRatings";
			this.LVRatings.ShowGroups = false;
			this.LVRatings.ShowHeaderInAllViews = false;
			this.LVRatings.Size = new System.Drawing.Size(346, 272);
			this.LVRatings.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.LVRatings.TabIndex = 15;
			this.LVRatings.UseCompatibleStateImageBehavior = false;
			this.LVRatings.View = System.Windows.Forms.View.Details;
			this.LVRatings.VirtualMode = true;
			this.LVRatings.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.LVLocations_CellEditFinished);
			this.LVRatings.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.LVLocations_CellEditStarting);
			// 
			// olvColumnName
			// 
			this.olvColumnName.AspectName = "RatingName";
			this.olvColumnName.CellEditUseWholeCell = true;
			this.olvColumnName.FillsFreeSpace = true;
			this.olvColumnName.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnName.Text = "Name";
			this.olvColumnName.Width = 140;
			// 
			// BtnAdd
			// 
			this.BtnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnAdd.Location = new System.Drawing.Point(355, 3);
			this.BtnAdd.Name = "BtnAdd";
			this.BtnAdd.Size = new System.Drawing.Size(75, 23);
			this.BtnAdd.TabIndex = 14;
			this.BtnAdd.Text = "Add";
			this.BtnAdd.UseVisualStyleBackColor = true;
			this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
			// 
			// olvColumnRatingMin
			// 
			this.olvColumnRatingMin.AspectName = "RatingMin";
			this.olvColumnRatingMin.AutoCompleteEditor = false;
			this.olvColumnRatingMin.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
			this.olvColumnRatingMin.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnRatingMin.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnRatingMin.Text = "Min";
			// 
			// olvColumnRatingMax
			// 
			this.olvColumnRatingMax.AspectName = "RatingMax";
			this.olvColumnRatingMax.AutoCompleteEditor = false;
			this.olvColumnRatingMax.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
			this.olvColumnRatingMax.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnRatingMax.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnRatingMax.Text = "Max";
			// 
			// olvColumnRatingStep
			// 
			this.olvColumnRatingStep.AspectName = "RatingStep";
			this.olvColumnRatingStep.AutoCompleteEditor = false;
			this.olvColumnRatingStep.AutoCompleteEditorMode = System.Windows.Forms.AutoCompleteMode.None;
			this.olvColumnRatingStep.Text = "Step";
			// 
			// Ratings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(432, 278);
			this.Controls.Add(this.BtnDelete);
			this.Controls.Add(this.LVRatings);
			this.Controls.Add(this.BtnAdd);
			this.Name = "Ratings";
			this.Text = "Ratings";
			((System.ComponentModel.ISupportInitialize)(this.LVRatings)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnDelete;
		private BrightIdeasSoftware.ObjectListView LVRatings;
		private BrightIdeasSoftware.OLVColumn olvColumnName;
		private System.Windows.Forms.Button BtnAdd;
		private BrightIdeasSoftware.OLVColumn olvColumnRatingMin;
		private BrightIdeasSoftware.OLVColumn olvColumnRatingMax;
		private BrightIdeasSoftware.OLVColumn olvColumnRatingStep;
	}
}