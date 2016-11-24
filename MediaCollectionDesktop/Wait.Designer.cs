namespace MediaCollection
{
	partial class Wait
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
			this.BtnRetry = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.LblStatus = new System.Windows.Forms.Label();
			this.TimerSpent = new System.Windows.Forms.Timer(this.components);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// BtnRetry
			// 
			this.BtnRetry.Location = new System.Drawing.Point(48, 65);
			this.BtnRetry.Name = "BtnRetry";
			this.BtnRetry.Size = new System.Drawing.Size(75, 23);
			this.BtnRetry.TabIndex = 0;
			this.BtnRetry.Text = "Retry";
			this.BtnRetry.UseVisualStyleBackColor = true;
			this.BtnRetry.Visible = false;
			// 
			// BtnCancel
			// 
			this.BtnCancel.Location = new System.Drawing.Point(129, 65);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(75, 23);
			this.BtnCancel.TabIndex = 1;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::MediaCollection.Resources.build_Selection_32xLG;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(52, 37);
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// LblStatus
			// 
			this.LblStatus.Location = new System.Drawing.Point(70, 12);
			this.LblStatus.Name = "LblStatus";
			this.LblStatus.Size = new System.Drawing.Size(134, 37);
			this.LblStatus.TabIndex = 3;
			this.LblStatus.Text = "label1";
			// 
			// TimerSpent
			// 
			this.TimerSpent.Enabled = true;
			this.TimerSpent.Interval = 1000;
			this.TimerSpent.Tick += new System.EventHandler(this.TimerSpent_Tick);
			// 
			// Wait
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(210, 96);
			this.Controls.Add(this.LblStatus);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.BtnCancel);
			this.Controls.Add(this.BtnRetry);
			this.Name = "Wait";
			this.Text = "Wait";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Wait_FormClosed);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnRetry;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label LblStatus;
		private System.Windows.Forms.Timer TimerSpent;
	}
}