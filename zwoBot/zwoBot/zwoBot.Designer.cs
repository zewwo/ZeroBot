namespace zwoBot
{
    partial class zwoBot
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
            this.UI_RTB_Chat = new System.Windows.Forms.RichTextBox();
            this.UI_LB_Users = new System.Windows.Forms.ListBox();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.SuspendLayout();
            // 
            // UI_RTB_Chat
            // 
            this.UI_RTB_Chat.Location = new System.Drawing.Point(12, 12);
            this.UI_RTB_Chat.Name = "UI_RTB_Chat";
            this.UI_RTB_Chat.Size = new System.Drawing.Size(365, 290);
            this.UI_RTB_Chat.TabIndex = 0;
            this.UI_RTB_Chat.Text = "";
            // 
            // UI_LB_Users
            // 
            this.UI_LB_Users.FormattingEnabled = true;
            this.UI_LB_Users.Location = new System.Drawing.Point(383, 12);
            this.UI_LB_Users.Name = "UI_LB_Users";
            this.UI_LB_Users.Size = new System.Drawing.Size(98, 290);
            this.UI_LB_Users.TabIndex = 1;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // zwoBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 315);
            this.Controls.Add(this.UI_LB_Users);
            this.Controls.Add(this.UI_RTB_Chat);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "zwoBot";
            this.Text = "zwoBot";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.zwoBot_FormClosing);
            this.Load += new System.EventHandler(this.zwoBot_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox UI_RTB_Chat;
        private System.Windows.Forms.ListBox UI_LB_Users;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
    }
}

