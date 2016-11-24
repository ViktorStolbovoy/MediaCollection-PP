namespace MediaCollection
{
	partial class BulkUpdate
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
			this.label1 = new System.Windows.Forms.Label();
			this.CbxDevice = new System.Windows.Forms.ComboBox();
			this.CbxLocation = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.BtnApply = new System.Windows.Forms.Button();
			this.BtnScan = new System.Windows.Forms.Button();
			this.panelTop = new System.Windows.Forms.Panel();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.PanelUpdateStatus = new System.Windows.Forms.Panel();
			this.LblUpdateStatus = new System.Windows.Forms.Label();
			this.LVNew = new BrightIdeasSoftware.ObjectListView();
			this.OlvColumnTitleNew = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.OlvColumnDataType = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.OlvColumnRelativePath = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.LVMissing = new BrightIdeasSoftware.ObjectListView();
			this.olvColumnName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumnKind = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumnLocationData = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.olvColumnNewLocationData = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.OlvColumnDelete = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
			this.groupBox1.SuspendLayout();
			this.panelTop.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.PanelUpdateStatus.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LVNew)).BeginInit();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.LVMissing)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(10, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(70, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "This Device: ";
			// 
			// CbxDevice
			// 
			this.CbxDevice.Enabled = false;
			this.CbxDevice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CbxDevice.FormattingEnabled = true;
			this.CbxDevice.Location = new System.Drawing.Point(86, 19);
			this.CbxDevice.Name = "CbxDevice";
			this.CbxDevice.Size = new System.Drawing.Size(167, 21);
			this.CbxDevice.TabIndex = 1;
			this.CbxDevice.SelectedIndexChanged += new System.EventHandler(this.CbxDevice_SelectedIndexChanged);
			// 
			// CbxLocation
			// 
			this.CbxLocation.Enabled = false;
			this.CbxLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.CbxLocation.FormattingEnabled = true;
			this.CbxLocation.Location = new System.Drawing.Point(317, 19);
			this.CbxLocation.Name = "CbxLocation";
			this.CbxLocation.Size = new System.Drawing.Size(177, 21);
			this.CbxLocation.TabIndex = 2;
			this.CbxLocation.SelectedIndexChanged += new System.EventHandler(this.CbxLocation_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(260, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Location: ";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.BtnApply);
			this.groupBox1.Controls.Add(this.BtnScan);
			this.groupBox1.Controls.Add(this.CbxDevice);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.CbxLocation);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(704, 59);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Scan Parameters";
			// 
			// BtnApply
			// 
			this.BtnApply.Enabled = false;
			this.BtnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnApply.Location = new System.Drawing.Point(581, 19);
			this.BtnApply.Name = "BtnApply";
			this.BtnApply.Size = new System.Drawing.Size(105, 23);
			this.BtnApply.TabIndex = 5;
			this.BtnApply.Text = "Apply Changes";
			this.BtnApply.UseVisualStyleBackColor = true;
			this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
			// 
			// BtnScan
			// 
			this.BtnScan.Enabled = false;
			this.BtnScan.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.BtnScan.Location = new System.Drawing.Point(500, 19);
			this.BtnScan.Name = "BtnScan";
			this.BtnScan.Size = new System.Drawing.Size(75, 23);
			this.BtnScan.TabIndex = 4;
			this.BtnScan.Text = "Scan";
			this.BtnScan.UseVisualStyleBackColor = true;
			this.BtnScan.Click += new System.EventHandler(this.BtnScan_Click);
			// 
			// panelTop
			// 
			this.panelTop.Controls.Add(this.groupBox1);
			this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTop.Location = new System.Drawing.Point(0, 0);
			this.panelTop.Name = "panelTop";
			this.panelTop.Size = new System.Drawing.Size(1051, 65);
			this.panelTop.TabIndex = 7;
			// 
			// splitContainer
			// 
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(0, 65);
			this.splitContainer.Name = "splitContainer";
			this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.groupBox3);
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.groupBox2);
			this.splitContainer.Size = new System.Drawing.Size(1051, 635);
			this.splitContainer.SplitterDistance = 320;
			this.splitContainer.TabIndex = 8;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.PanelUpdateStatus);
			this.groupBox3.Controls.Add(this.LVNew);
			this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.Location = new System.Drawing.Point(4, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(1044, 314);
			this.groupBox3.TabIndex = 10;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "New";
			// 
			// PanelUpdateStatus
			// 
			this.PanelUpdateStatus.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.PanelUpdateStatus.Controls.Add(this.LblUpdateStatus);
			this.PanelUpdateStatus.Cursor = System.Windows.Forms.Cursors.WaitCursor;
			this.PanelUpdateStatus.Location = new System.Drawing.Point(387, 68);
			this.PanelUpdateStatus.Name = "PanelUpdateStatus";
			this.PanelUpdateStatus.Size = new System.Drawing.Size(337, 177);
			this.PanelUpdateStatus.TabIndex = 11;
			this.PanelUpdateStatus.Visible = false;
			// 
			// LblUpdateStatus
			// 
			this.LblUpdateStatus.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LblUpdateStatus.Location = new System.Drawing.Point(0, 0);
			this.LblUpdateStatus.Name = "LblUpdateStatus";
			this.LblUpdateStatus.Size = new System.Drawing.Size(337, 177);
			this.LblUpdateStatus.TabIndex = 0;
			this.LblUpdateStatus.Text = "Processing...";
			this.LblUpdateStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LVNew
			// 
			this.LVNew.AllColumns.Add(this.OlvColumnTitleNew);
			this.LVNew.AllColumns.Add(this.OlvColumnDataType);
			this.LVNew.AllColumns.Add(this.OlvColumnRelativePath);
			this.LVNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LVNew.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.LVNew.CellEditEnterChangesRows = true;
			this.LVNew.CellEditTabChangesRows = true;
			this.LVNew.CellEditUseWholeCell = false;
			this.LVNew.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.OlvColumnTitleNew,
            this.OlvColumnDataType,
            this.OlvColumnRelativePath});
			this.LVNew.Cursor = System.Windows.Forms.Cursors.Default;
			this.LVNew.EmptyListMsg = "";
			this.LVNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LVNew.FullRowSelect = true;
			this.LVNew.GridLines = true;
			this.LVNew.HideSelection = false;
			this.LVNew.LabelWrap = false;
			this.LVNew.Location = new System.Drawing.Point(6, 19);
			this.LVNew.MultiSelect = false;
			this.LVNew.Name = "LVNew";
			this.LVNew.ShowGroups = false;
			this.LVNew.Size = new System.Drawing.Size(1032, 289);
			this.LVNew.TabIndex = 11;
			this.LVNew.UseCompatibleStateImageBehavior = false;
			this.LVNew.View = System.Windows.Forms.View.Details;
			// 
			// OlvColumnTitleNew
			// 
			this.OlvColumnTitleNew.AspectName = "Title";
			this.OlvColumnTitleNew.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OlvColumnTitleNew.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.OlvColumnTitleNew.Text = "Title";
			this.OlvColumnTitleNew.Width = 300;
			// 
			// OlvColumnDataType
			// 
			this.OlvColumnDataType.AspectName = "DataType";
			this.OlvColumnDataType.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OlvColumnDataType.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.OlvColumnDataType.Text = "Type";
			this.OlvColumnDataType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.OlvColumnDataType.Width = 80;
			// 
			// OlvColumnRelativePath
			// 
			this.OlvColumnRelativePath.AspectName = "RelativePath";
			this.OlvColumnRelativePath.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.OlvColumnRelativePath.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.OlvColumnRelativePath.IsEditable = false;
			this.OlvColumnRelativePath.Text = "Path";
			this.OlvColumnRelativePath.Width = 600;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.LVMissing);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.Location = new System.Drawing.Point(4, -1);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(1044, 305);
			this.groupBox2.TabIndex = 9;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Missing";
			// 
			// LVMissing
			// 
			this.LVMissing.AllColumns.Add(this.olvColumnName);
			this.LVMissing.AllColumns.Add(this.olvColumnKind);
			this.LVMissing.AllColumns.Add(this.olvColumnLocationData);
			this.LVMissing.AllColumns.Add(this.olvColumnNewLocationData);
			this.LVMissing.AllColumns.Add(this.OlvColumnDelete);
			this.LVMissing.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LVMissing.CellEditActivation = BrightIdeasSoftware.ObjectListView.CellEditActivateMode.DoubleClick;
			this.LVMissing.CellEditEnterChangesRows = true;
			this.LVMissing.CellEditTabChangesRows = true;
			this.LVMissing.CellEditUseWholeCell = false;
			this.LVMissing.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnName,
            this.olvColumnKind,
            this.olvColumnLocationData,
            this.olvColumnNewLocationData,
            this.OlvColumnDelete});
			this.LVMissing.Cursor = System.Windows.Forms.Cursors.Default;
			this.LVMissing.EmptyListMsg = "";
			this.LVMissing.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LVMissing.FullRowSelect = true;
			this.LVMissing.GridLines = true;
			this.LVMissing.HasCollapsibleGroups = false;
			this.LVMissing.Location = new System.Drawing.Point(6, 19);
			this.LVMissing.Name = "LVMissing";
			this.LVMissing.Size = new System.Drawing.Size(1032, 280);
			this.LVMissing.TabIndex = 10;
			this.LVMissing.UseCompatibleStateImageBehavior = false;
			this.LVMissing.View = System.Windows.Forms.View.Details;
			// 
			// olvColumnName
			// 
			this.olvColumnName.AspectName = "Name";
			this.olvColumnName.DisplayIndex = 1;
			this.olvColumnName.Groupable = false;
			this.olvColumnName.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnName.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnName.Text = "Title";
			this.olvColumnName.Width = 300;
			// 
			// olvColumnKind
			// 
			this.olvColumnKind.AspectName = "Kind";
			this.olvColumnKind.DisplayIndex = 2;
			this.olvColumnKind.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnKind.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnKind.Text = "Type";
			this.olvColumnKind.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnKind.Width = 80;
			// 
			// olvColumnLocationData
			// 
			this.olvColumnLocationData.AspectName = "LocationData";
			this.olvColumnLocationData.DisplayIndex = 3;
			this.olvColumnLocationData.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnLocationData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnLocationData.Text = "Path";
			this.olvColumnLocationData.Width = 450;
			// 
			// olvColumnNewLocationData
			// 
			this.olvColumnNewLocationData.AspectName = "NewLocationData";
			this.olvColumnNewLocationData.DisplayIndex = 4;
			this.olvColumnNewLocationData.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.olvColumnNewLocationData.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.olvColumnNewLocationData.Text = "New Path";
			this.olvColumnNewLocationData.Width = 450;
			// 
			// OlvColumnDelete
			// 
			this.OlvColumnDelete.AspectName = "ShouldDelete";
			this.OlvColumnDelete.CheckBoxes = true;
			this.OlvColumnDelete.DisplayIndex = 0;
			this.OlvColumnDelete.Text = "Delete";
			this.OlvColumnDelete.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.OlvColumnDelete.Width = 50;
			// 
			// BulkUpdate
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1051, 700);
			this.Controls.Add(this.splitContainer);
			this.Controls.Add(this.panelTop);
			this.Name = "BulkUpdate";
			this.Text = "Bulk Update";
			this.Shown += new System.EventHandler(this.BulkUpdate_Shown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panelTop.ResumeLayout(false);
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.PanelUpdateStatus.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.LVNew)).EndInit();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.LVMissing)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox CbxDevice;
		private System.Windows.Forms.ComboBox CbxLocation;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button BtnApply;
		private System.Windows.Forms.Button BtnScan;
		private System.Windows.Forms.Panel panelTop;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.GroupBox groupBox2;
		private BrightIdeasSoftware.ObjectListView LVMissing;
		private System.Windows.Forms.GroupBox groupBox3;
		private BrightIdeasSoftware.ObjectListView LVNew;
		private System.Windows.Forms.Panel PanelUpdateStatus;
		private System.Windows.Forms.Label LblUpdateStatus;
		private BrightIdeasSoftware.OLVColumn olvColumnName;
		private BrightIdeasSoftware.OLVColumn olvColumnKind;
		private BrightIdeasSoftware.OLVColumn olvColumnLocationData;
		private BrightIdeasSoftware.OLVColumn olvColumnNewLocationData;
		private BrightIdeasSoftware.OLVColumn OlvColumnTitleNew;
		private BrightIdeasSoftware.OLVColumn OlvColumnDataType;
		private BrightIdeasSoftware.OLVColumn OlvColumnRelativePath;
		private BrightIdeasSoftware.OLVColumn OlvColumnDelete;
	}
}