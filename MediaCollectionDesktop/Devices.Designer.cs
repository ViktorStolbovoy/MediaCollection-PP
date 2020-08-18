namespace MediaCollection
{
	partial class Devices
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
			this.BtnAdd = new System.Windows.Forms.Button();
			this.panelMain = new System.Windows.Forms.Panel();
			this.BtnDelete = new System.Windows.Forms.Button();
			this.TVDevices = new BrightIdeasSoftware.TreeListView();
			this.olvColName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColData = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.panelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TVDevices)).BeginInit();
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
			this.panelMain.Controls.Add(this.TVDevices);
			this.panelMain.Controls.Add(this.BtnAdd);
			this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panelMain.Location = new System.Drawing.Point(0, 0);
			this.panelMain.Name = "panelMain";
			this.panelMain.Size = new System.Drawing.Size(772, 463);
			this.panelMain.TabIndex = 12; 
			// 
			// TVDevices
			// 
			this.TVDevices.AllColumns.Add(this.olvColName);
			this.TVDevices.AllColumns.Add(this.olvColType);
			this.TVDevices.AllColumns.Add(this.olvColData);
			this.TVDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TVDevices.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.TVDevices.CellEditUseWholeCell = false;
			this.TVDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColName,
            this.olvColType,
            this.olvColData});
			this.TVDevices.Cursor = System.Windows.Forms.Cursors.Default;
			this.TVDevices.FullRowSelect = true;
			this.TVDevices.GridLines = true;
			this.TVDevices.Location = new System.Drawing.Point(4, 3);
			this.TVDevices.Name = "TVDevices";
			this.TVDevices.ShowGroups = false;
			this.TVDevices.Size = new System.Drawing.Size(675, 456);
			this.TVDevices.TabIndex = 12;
			this.TVDevices.UseCompatibleStateImageBehavior = false;
			this.TVDevices.View = System.Windows.Forms.View.Details;
			this.TVDevices.VirtualMode = true;
			this.TVDevices.CellEditFinished += new BrightIdeasSoftware.CellEditEventHandler(this.TVDevices_CellEditFinished);
			this.TVDevices.CellEditFinishing += new BrightIdeasSoftware.CellEditEventHandler(this.TVDevices_CellEditFinishing);
			this.TVDevices.CellEditStarting += new BrightIdeasSoftware.CellEditEventHandler(this.TVDevices_CellEditStarting);
			// 
			// olvColName
			// 
			this.olvColName.AspectName = "Name";
			this.olvColName.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColName.Text = "Name";
			this.olvColName.Width = 156;
			// 
			// olvColType
			// 
			this.olvColType.AspectName = "Kind";
			this.olvColType.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColType.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColType.Text = "Type";
			this.olvColType.Width = 116;
			// 
			// olvColData
			// 
			this.olvColData.AspectName = "Data";
			this.olvColData.FillsFreeSpace = true;
			this.olvColData.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColData.Text = "Path";
			this.olvColData.Width = 383;
			// 
			// Devices
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(772, 463);
			this.Controls.Add(this.panelMain);
			this.Name = "Devices";
			this.Text = "Devices";
			this.Shown += new System.EventHandler(this.Devices_Shown);
			this.panelMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.TVDevices)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnAdd;
		private System.Windows.Forms.Panel panelMain;
		private BrightIdeasSoftware.TreeListView TVDevices;
		private System.Windows.Forms.Button BtnDelete;
		private BrightIdeasSoftware.OLVColumn olvColName;
		private BrightIdeasSoftware.OLVColumn olvColType;
		private BrightIdeasSoftware.OLVColumn olvColData;
	}
}