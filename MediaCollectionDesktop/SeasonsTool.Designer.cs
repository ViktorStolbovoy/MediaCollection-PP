namespace MediaCollection
{
	partial class SeasonsTool
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
			this.TbxSeries = new System.Windows.Forms.TextBox();
			this.TbxSeasonPattern = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.BtnCreateSeries = new System.Windows.Forms.Button();
			this.ClbSeasons = new System.Windows.Forms.CheckedListBox();
			this.ClbEpisodes = new System.Windows.Forms.CheckedListBox();
			this.GbxSeries = new System.Windows.Forms.GroupBox();
			this.GbxSeasons = new System.Windows.Forms.GroupBox();
			this.PnlStatus = new System.Windows.Forms.Panel();
			this.BtnAutoMove = new System.Windows.Forms.Button();
			this.BtnCreateSeasons = new System.Windows.Forms.Button();
			this.TbxSeasonsToCreate = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.BtnSearchForSeasons = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.BtnRecategorise = new System.Windows.Forms.Button();
			this.TbxEpisodesRegexp = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.GbxSeries.SuspendLayout();
			this.GbxSeasons.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Name:";
			// 
			// TbxSeries
			// 
			this.TbxSeries.Location = new System.Drawing.Point(50, 13);
			this.TbxSeries.Name = "TbxSeries";
			this.TbxSeries.Size = new System.Drawing.Size(264, 20);
			this.TbxSeries.TabIndex = 1;
			this.TbxSeries.TextChanged += new System.EventHandler(this.TbxSeries_TextChanged);
			// 
			// TbxSeasonPattern
			// 
			this.TbxSeasonPattern.Location = new System.Drawing.Point(50, 19);
			this.TbxSeasonPattern.Name = "TbxSeasonPattern";
			this.TbxSeasonPattern.Size = new System.Drawing.Size(264, 20);
			this.TbxSeasonPattern.TabIndex = 3;
			this.TbxSeasonPattern.TextChanged += new System.EventHandler(this.TbxSeasonPattern_TextChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 22);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(44, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Pattern:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(10, 67);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(0, 13);
			this.label3.TabIndex = 4;
			// 
			// BtnCreateSeries
			// 
			this.BtnCreateSeries.Location = new System.Drawing.Point(320, 11);
			this.BtnCreateSeries.Name = "BtnCreateSeries";
			this.BtnCreateSeries.Size = new System.Drawing.Size(75, 23);
			this.BtnCreateSeries.TabIndex = 6;
			this.BtnCreateSeries.Text = "Create";
			this.BtnCreateSeries.UseVisualStyleBackColor = true;
			this.BtnCreateSeries.Click += new System.EventHandler(this.BtnCreateSeries_Click);
			// 
			// ClbSeasons
			// 
			this.ClbSeasons.FormattingEnabled = true;
			this.ClbSeasons.Location = new System.Drawing.Point(50, 45);
			this.ClbSeasons.Name = "ClbSeasons";
			this.ClbSeasons.ScrollAlwaysVisible = true;
			this.ClbSeasons.Size = new System.Drawing.Size(264, 94);
			this.ClbSeasons.TabIndex = 7;
			// 
			// ClbEpisodes
			// 
			this.ClbEpisodes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ClbEpisodes.FormattingEnabled = true;
			this.ClbEpisodes.Location = new System.Drawing.Point(12, 48);
			this.ClbEpisodes.Name = "ClbEpisodes";
			this.ClbEpisodes.ScrollAlwaysVisible = true;
			this.ClbEpisodes.Size = new System.Drawing.Size(381, 154);
			this.ClbEpisodes.TabIndex = 8;
			// 
			// GbxSeries
			// 
			this.GbxSeries.Controls.Add(this.label1);
			this.GbxSeries.Controls.Add(this.TbxSeries);
			this.GbxSeries.Controls.Add(this.BtnCreateSeries);
			this.GbxSeries.Location = new System.Drawing.Point(12, 12);
			this.GbxSeries.Name = "GbxSeries";
			this.GbxSeries.Size = new System.Drawing.Size(401, 40);
			this.GbxSeries.TabIndex = 11;
			this.GbxSeries.TabStop = false;
			this.GbxSeries.Text = "Series";
			// 
			// GbxSeasons
			// 
			this.GbxSeasons.Controls.Add(this.PnlStatus);
			this.GbxSeasons.Controls.Add(this.BtnAutoMove);
			this.GbxSeasons.Controls.Add(this.BtnCreateSeasons);
			this.GbxSeasons.Controls.Add(this.TbxSeasonsToCreate);
			this.GbxSeasons.Controls.Add(this.label6);
			this.GbxSeasons.Controls.Add(this.BtnSearchForSeasons);
			this.GbxSeasons.Controls.Add(this.label5);
			this.GbxSeasons.Controls.Add(this.TbxSeasonPattern);
			this.GbxSeasons.Controls.Add(this.label2);
			this.GbxSeasons.Controls.Add(this.ClbSeasons);
			this.GbxSeasons.Location = new System.Drawing.Point(12, 59);
			this.GbxSeasons.Name = "GbxSeasons";
			this.GbxSeasons.Size = new System.Drawing.Size(399, 176);
			this.GbxSeasons.TabIndex = 12;
			this.GbxSeasons.TabStop = false;
			this.GbxSeasons.Text = "Seasons";
			// 
			// PnlStatus
			// 
			this.PnlStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.PnlStatus.Location = new System.Drawing.Point(69, 22);
			this.PnlStatus.Name = "PnlStatus";
			this.PnlStatus.Size = new System.Drawing.Size(200, 100);
			this.PnlStatus.TabIndex = 12;
			this.PnlStatus.Visible = false;
			// 
			// BtnAutoMove
			// 
			this.BtnAutoMove.Location = new System.Drawing.Point(318, 46);
			this.BtnAutoMove.Name = "BtnAutoMove";
			this.BtnAutoMove.Size = new System.Drawing.Size(75, 23);
			this.BtnAutoMove.TabIndex = 11;
			this.BtnAutoMove.Text = "Auto Move";
			this.BtnAutoMove.UseVisualStyleBackColor = true;
			this.BtnAutoMove.Click += new System.EventHandler(this.BtnAutoMove_Click);
			// 
			// BtnCreateSeasons
			// 
			this.BtnCreateSeasons.Location = new System.Drawing.Point(318, 143);
			this.BtnCreateSeasons.Name = "BtnCreateSeasons";
			this.BtnCreateSeasons.Size = new System.Drawing.Size(75, 23);
			this.BtnCreateSeasons.TabIndex = 7;
			this.BtnCreateSeasons.Text = "Create";
			this.BtnCreateSeasons.UseVisualStyleBackColor = true;
			this.BtnCreateSeasons.Click += new System.EventHandler(this.BtnCreateSeasons_Click);
			// 
			// TbxSeasonsToCreate
			// 
			this.TbxSeasonsToCreate.Location = new System.Drawing.Point(50, 145);
			this.TbxSeasonsToCreate.Name = "TbxSeasonsToCreate";
			this.TbxSeasonsToCreate.Size = new System.Drawing.Size(264, 20);
			this.TbxSeasonsToCreate.TabIndex = 9;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(9, 148);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(41, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "Create:";
			// 
			// BtnSearchForSeasons
			// 
			this.BtnSearchForSeasons.Location = new System.Drawing.Point(318, 17);
			this.BtnSearchForSeasons.Name = "BtnSearchForSeasons";
			this.BtnSearchForSeasons.Size = new System.Drawing.Size(75, 23);
			this.BtnSearchForSeasons.TabIndex = 7;
			this.BtnSearchForSeasons.Text = "Search";
			this.BtnSearchForSeasons.UseVisualStyleBackColor = true;
			this.BtnSearchForSeasons.Click += new System.EventHandler(this.BtnSearchForSeasons_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 45);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(40, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Found:";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.BtnRecategorise);
			this.groupBox1.Controls.Add(this.TbxEpisodesRegexp);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.ClbEpisodes);
			this.groupBox1.Location = new System.Drawing.Point(12, 242);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(401, 206);
			this.groupBox1.TabIndex = 13;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Related Discs / Episodes";
			// 
			// BtnRecategorise
			// 
			this.BtnRecategorise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.BtnRecategorise.Location = new System.Drawing.Point(313, 19);
			this.BtnRecategorise.Name = "BtnRecategorise";
			this.BtnRecategorise.Size = new System.Drawing.Size(80, 23);
			this.BtnRecategorise.TabIndex = 11;
			this.BtnRecategorise.Text = "Recategorise";
			this.BtnRecategorise.UseVisualStyleBackColor = true;
			this.BtnRecategorise.Click += new System.EventHandler(this.BtnRecategorise_Click);
			// 
			// TbxEpisodesRegexp
			// 
			this.TbxEpisodesRegexp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.TbxEpisodesRegexp.Location = new System.Drawing.Point(50, 21);
			this.TbxEpisodesRegexp.Name = "TbxEpisodesRegexp";
			this.TbxEpisodesRegexp.Size = new System.Drawing.Size(257, 20);
			this.TbxEpisodesRegexp.TabIndex = 10;
			this.TbxEpisodesRegexp.Text = "S (?<s>\\d{2}) E (?<e>\\d{2})";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(9, 24);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(44, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Regex: ";
			// 
			// SeasonsTool
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(421, 455);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.GbxSeasons);
			this.Controls.Add(this.GbxSeries);
			this.Controls.Add(this.label3);
			this.Name = "SeasonsTool";
			this.Text = "SeasonsTool";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SeasonsTool_FormClosed);
			this.GbxSeries.ResumeLayout(false);
			this.GbxSeries.PerformLayout();
			this.GbxSeasons.ResumeLayout(false);
			this.GbxSeasons.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox TbxSeries;
		private System.Windows.Forms.TextBox TbxSeasonPattern;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button BtnCreateSeries;
		private System.Windows.Forms.CheckedListBox ClbSeasons;
		private System.Windows.Forms.CheckedListBox ClbEpisodes;
		private System.Windows.Forms.GroupBox GbxSeries;
		private System.Windows.Forms.GroupBox GbxSeasons;
		private System.Windows.Forms.Button BtnCreateSeasons;
		private System.Windows.Forms.TextBox TbxSeasonsToCreate;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button BtnSearchForSeasons;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button BtnAutoMove;
        private System.Windows.Forms.Panel PnlStatus;
        private System.Windows.Forms.Button BtnRecategorise;
        private System.Windows.Forms.TextBox TbxEpisodesRegexp;
        private System.Windows.Forms.Label label4;
    }
}