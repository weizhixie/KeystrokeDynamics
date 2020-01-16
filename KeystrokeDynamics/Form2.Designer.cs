namespace KeystrokeDynamics
{
    partial class Form2
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
            this.QNumlbl = new System.Windows.Forms.Label();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.NextBtn = new System.Windows.Forms.Button();
            this.QuestionLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 188);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Enter above";
            // 
            // QNumlbl
            // 
            this.QNumlbl.AutoSize = true;
            this.QNumlbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.QNumlbl.Location = new System.Drawing.Point(94, 111);
            this.QNumlbl.Name = "QNumlbl";
            this.QNumlbl.Size = new System.Drawing.Size(2, 22);
            this.QNumlbl.TabIndex = 12;
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(160, 188);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(602, 26);
            this.InputTextBox.TabIndex = 11;
            // 
            // NextBtn
            // 
            this.NextBtn.Location = new System.Drawing.Point(278, 269);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(197, 71);
            this.NextBtn.TabIndex = 10;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // QuestionLbl
            // 
            this.QuestionLbl.AutoSize = true;
            this.QuestionLbl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.QuestionLbl.Location = new System.Drawing.Point(278, 111);
            this.QuestionLbl.Name = "QuestionLbl";
            this.QuestionLbl.Size = new System.Drawing.Size(2, 22);
            this.QuestionLbl.TabIndex = 9;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QNumlbl);
            this.Controls.Add(this.InputTextBox);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.QuestionLbl);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label QNumlbl;
        private System.Windows.Forms.TextBox InputTextBox;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Label QuestionLbl;
    }
}