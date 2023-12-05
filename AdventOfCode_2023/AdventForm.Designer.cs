namespace AdventOfCode_2023
{
    partial class AdventForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGenerate = new System.Windows.Forms.Button();
            this.cboSelectDay = new System.Windows.Forms.ComboBox();
            this.lblSelectDay = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblResult = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblSolution = new System.Windows.Forms.Label();
            this.lblSolutionDisplay = new System.Windows.Forms.Label();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(380, 11);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(94, 29);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // cboSelectDay
            // 
            this.cboSelectDay.FormattingEnabled = true;
            this.cboSelectDay.Location = new System.Drawing.Point(148, 12);
            this.cboSelectDay.Name = "cboSelectDay";
            this.cboSelectDay.Size = new System.Drawing.Size(226, 28);
            this.cboSelectDay.TabIndex = 1;
            this.cboSelectDay.SelectedIndexChanged += new System.EventHandler(this.cboSelectDay_SelectedIndexChanged);
            // 
            // lblSelectDay
            // 
            this.lblSelectDay.AutoSize = true;
            this.lblSelectDay.Location = new System.Drawing.Point(63, 16);
            this.lblSelectDay.Name = "lblSelectDay";
            this.lblSelectDay.Size = new System.Drawing.Size(79, 20);
            this.lblSelectDay.TabIndex = 2;
            this.lblSelectDay.Text = "Select Day";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(148, 108);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(326, 27);
            this.txtResult.TabIndex = 3;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(93, 111);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(49, 20);
            this.lblResult.TabIndex = 4;
            this.lblResult.Text = "Result";
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 173);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(575, 26);
            this.statusStrip.TabIndex = 6;
            this.statusStrip.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblStatus.Size = new System.Drawing.Size(560, 20);
            this.lblStatus.Spring = true;
            this.lblStatus.Text = "#STATUS";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSolution
            // 
            this.lblSolution.AutoSize = true;
            this.lblSolution.Location = new System.Drawing.Point(78, 141);
            this.lblSolution.Name = "lblSolution";
            this.lblSolution.Size = new System.Drawing.Size(64, 20);
            this.lblSolution.TabIndex = 8;
            this.lblSolution.Text = "Solution";
            // 
            // lblSolutionDisplay
            // 
            this.lblSolutionDisplay.AutoSize = true;
            this.lblSolutionDisplay.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSolutionDisplay.Location = new System.Drawing.Point(148, 141);
            this.lblSolutionDisplay.Name = "lblSolutionDisplay";
            this.lblSolutionDisplay.Size = new System.Drawing.Size(93, 20);
            this.lblSolutionDisplay.TabIndex = 9;
            this.lblSolutionDisplay.Text = "#SOLUTION";
            // 
            // AdventForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(575, 199);
            this.Controls.Add(this.lblSolutionDisplay);
            this.Controls.Add(this.lblSolution);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.lblSelectDay);
            this.Controls.Add(this.cboSelectDay);
            this.Controls.Add(this.btnGenerate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "AdventForm";
            this.Text = "Advent of Code 2023";
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnGenerate;
        private ComboBox cboSelectDay;
        private Label lblSelectDay;
        private TextBox txtResult;
        private Label lblResult;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private Label lblSolution;
        private Label lblSolutionDisplay;
    }
}