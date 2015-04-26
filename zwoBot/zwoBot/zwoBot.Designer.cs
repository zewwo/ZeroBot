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
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.UI_TB_Channel = new System.Windows.Forms.TextBox();
            this.UI_TB_Nick = new System.Windows.Forms.TextBox();
            this.UI_TB_AltNick = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.UI_L_Connected = new System.Windows.Forms.Label();
            this.UI_B_Connect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(145, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 45);
            this.label1.TabIndex = 0;
            this.label1.Text = "zwoBot";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(97, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(229, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Created by Kevin Nguyen (zewwo)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(128, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 19);
            this.label3.TabIndex = 2;
            this.label3.Text = "www.github.com/zewwo";
            // 
            // UI_TB_Channel
            // 
            this.UI_TB_Channel.Location = new System.Drawing.Point(166, 105);
            this.UI_TB_Channel.Name = "UI_TB_Channel";
            this.UI_TB_Channel.Size = new System.Drawing.Size(142, 20);
            this.UI_TB_Channel.TabIndex = 3;
            this.UI_TB_Channel.Text = "#teamfs";
            // 
            // UI_TB_Nick
            // 
            this.UI_TB_Nick.Location = new System.Drawing.Point(166, 131);
            this.UI_TB_Nick.Name = "UI_TB_Nick";
            this.UI_TB_Nick.Size = new System.Drawing.Size(142, 20);
            this.UI_TB_Nick.TabIndex = 4;
            this.UI_TB_Nick.Text = "beta000";
            // 
            // UI_TB_AltNick
            // 
            this.UI_TB_AltNick.Location = new System.Drawing.Point(166, 157);
            this.UI_TB_AltNick.Name = "UI_TB_AltNick";
            this.UI_TB_AltNick.Size = new System.Drawing.Size(142, 20);
            this.UI_TB_AltNick.TabIndex = 5;
            this.UI_TB_AltNick.Text = "zwoBot";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(86, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "Channel : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(111, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 19);
            this.label5.TabIndex = 7;
            this.label5.Text = "Nick : ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(89, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 19);
            this.label6.TabIndex = 8;
            this.label6.Text = "Alt Nick : ";
            // 
            // UI_L_Connected
            // 
            this.UI_L_Connected.Font = new System.Drawing.Font("Calibri", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_L_Connected.ForeColor = System.Drawing.Color.Red;
            this.UI_L_Connected.Location = new System.Drawing.Point(96, 180);
            this.UI_L_Connected.Name = "UI_L_Connected";
            this.UI_L_Connected.Size = new System.Drawing.Size(212, 29);
            this.UI_L_Connected.TabIndex = 9;
            this.UI_L_Connected.Text = "Not Connected";
            this.UI_L_Connected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UI_B_Connect
            // 
            this.UI_B_Connect.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UI_B_Connect.Location = new System.Drawing.Point(153, 212);
            this.UI_B_Connect.Name = "UI_B_Connect";
            this.UI_B_Connect.Size = new System.Drawing.Size(109, 30);
            this.UI_B_Connect.TabIndex = 10;
            this.UI_B_Connect.Text = "Connect";
            this.UI_B_Connect.UseVisualStyleBackColor = true;
            this.UI_B_Connect.Click += new System.EventHandler(this.UI_B_Connect_Click);
            // 
            // zwoBot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 254);
            this.Controls.Add(this.UI_B_Connect);
            this.Controls.Add(this.UI_L_Connected);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.UI_TB_AltNick);
            this.Controls.Add(this.UI_TB_Nick);
            this.Controls.Add(this.UI_TB_Channel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "zwoBot";
            this.Text = "zwoBot";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox UI_TB_Channel;
        private System.Windows.Forms.TextBox UI_TB_Nick;
        private System.Windows.Forms.TextBox UI_TB_AltNick;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label UI_L_Connected;
        private System.Windows.Forms.Button UI_B_Connect;
    }
}

