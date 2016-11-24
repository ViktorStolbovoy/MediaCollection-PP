namespace MediaCollection
{
	partial class Locations
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
			this.BtnAdd = new System.Windows.Forms.Button();
			this.panelMain = new System.Windows.Forms.Panel();
			this.BtnDelete = new System.Windows.Forms.Button();
			this.LVLocations = new BrightIdeasSoftware.ObjectListView();
			this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumnKind = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.panelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LVLocations)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnAdd
			// 
			this.BtnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnAdd.Location = new System.Drawing.Point(685, 5);
			this.BtnAdd.Name = "BtnAdd";
			this.BtnAdd.Size = new System.Drawing.Size(75, 23);
			this.BtnAdd.TabIndex = 4;
			this.BtnAdd.Text = "Add";
			this.BtnAdd.UseVisualStyleBackColor = true;
			this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
			// 
			// panelMain
			// 
			this.panelMain.Controls.Add(this.BtnDelete);
			this.panelMain.Controls.Add(this.LVLocations);
			this.panelMain.Controls.Add(this.BtnAdd);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(772, 463);
			this.panelMain.TabIndex = 12;
			// 
			// BtnDelete
			// 
			this.BtnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnDelete.ForeColor = System.Drawing.Color.Red;
			this.BtnDelete.Location = new System.Drawing.Point(685, 35);
			this.BtnDelete.Name = "BtnDelete";
			this.BtnDelete.Size = new System.Drawing.Size(75, 23);
			this.BtnDelete.TabIndex = 13;
			this.BtnDelete.Text = "Delete";
			this.BtnDelete.UseVisualStyleBackColor = true;
			this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
			// 
			// LVLocations
			// 
			this.LVLocations.AllColumns.Add(this.olvColumnKind);
			this.LVLocations.AllColumns.Add(this.olvColumnName);
			this.LVLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LVLocations.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.LVLocations.CellEditUseWholeCell = false;
			this.LVLocations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnKind,
            this.olvColumnName});
			this.LVLocations.Cursor = System.Windows.Forms.Cursors.Default;
			this.LVLocations.FullRowSelect = true;
			this.LVLocations.GridLines = true;
			this.LVLocations.Location = new System.Drawing.Point(4, 3);
			this.LVLocations.Name = "LVLocations";
			this.LVLocations.ShowGroups = false;
			this.LVLocations.Size = new System.Drawing.Size(675, 456);
			this.LVLocations.TabIndex = 12;
			this.LVLocations.UseCompatibleStateImageBehavior = false;
			this.LVLocations.View = System.Windows.Forms.View.Details;
			this.LVLocations.VirtualMode = true;
			this.LVLocations.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.LVLocations_CellEditFinished);
			this.LVLocations.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.LVLocations_CellEditStarting);
			this.LVLocations.SelectedIndexChanged += new System.EventHandler(this.LVLocations_SelectedIndexChanged);
			// 
			// olvColumnName
			// 
			this.olvColumnName.AspectName = "Name";
			this.olvColumnName.DisplayIndex = 0;
			this.olvColumnName.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnName.Text = "Name";
			this.olvColumnName.Width = 500;
			// 
			// olvColumnKind
			// 
			this.olvColumnKind.AspectName = "Kind";
			this.olvColumnKind.DisplayIndex = 1;
			this.olvColumnKind.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnKind.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnKind.Text = "Type";
			// 
			// Locations
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(772, 463);
			this.Controls.Add(this.panelMain);
			this.Name = "Locations";
			this.Text = "Locations";
			this.Shown += new System.EventHandler(this.Locations_Shown);
			this.panelMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.LVLocations)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnAdd;
		private System.Windows.Forms.Panel panelMain;
		private BrightIdeasSoftware.ObjectListView LVLocations;
		private System.Windows.Forms.Button BtnDelete;
		private BrightIdeasSoftware.OLVColumn olvColumnName;
		private BrightIdeasSoftware.OLVColumn olvColumnKind;
	}
}