namespace Coachseek.API.Tools
{
    partial class MainForm
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
            this.EmailLabel = new System.Windows.Forms.Label();
            this.EmailTextBox = new System.Windows.Forms.TextBox();
            this.UnsubscribeButton = new System.Windows.Forms.Button();
            this.IsTestingCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // EmailLabel
            // 
            this.EmailLabel.AutoSize = true;
            this.EmailLabel.Location = new System.Drawing.Point(13, 13);
            this.EmailLabel.Name = "EmailLabel";
            this.EmailLabel.Size = new System.Drawing.Size(35, 13);
            this.EmailLabel.TabIndex = 0;
            this.EmailLabel.Text = "Email:";
            // 
            // EmailTextBox
            // 
            this.EmailTextBox.Location = new System.Drawing.Point(68, 13);
            this.EmailTextBox.Name = "EmailTextBox";
            this.EmailTextBox.Size = new System.Drawing.Size(451, 20);
            this.EmailTextBox.TabIndex = 1;
            // 
            // UnsubscribeButton
            // 
            this.UnsubscribeButton.Location = new System.Drawing.Point(363, 49);
            this.UnsubscribeButton.Name = "UnsubscribeButton";
            this.UnsubscribeButton.Size = new System.Drawing.Size(75, 23);
            this.UnsubscribeButton.TabIndex = 2;
            this.UnsubscribeButton.Text = "Unsubscribe";
            this.UnsubscribeButton.UseVisualStyleBackColor = true;
            this.UnsubscribeButton.Click += new System.EventHandler(this.UnsubscribeButton_Click);
            // 
            // IsTestingCheckBox
            // 
            this.IsTestingCheckBox.AutoSize = true;
            this.IsTestingCheckBox.Checked = true;
            this.IsTestingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IsTestingCheckBox.Location = new System.Drawing.Point(68, 54);
            this.IsTestingCheckBox.Name = "IsTestingCheckBox";
            this.IsTestingCheckBox.Size = new System.Drawing.Size(61, 17);
            this.IsTestingCheckBox.TabIndex = 3;
            this.IsTestingCheckBox.Text = "Testing";
            this.IsTestingCheckBox.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 102);
            this.Controls.Add(this.IsTestingCheckBox);
            this.Controls.Add(this.UnsubscribeButton);
            this.Controls.Add(this.EmailTextBox);
            this.Controls.Add(this.EmailLabel);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EmailLabel;
        private System.Windows.Forms.TextBox EmailTextBox;
        private System.Windows.Forms.Button UnsubscribeButton;
        private System.Windows.Forms.CheckBox IsTestingCheckBox;
    }
}

