﻿namespace NetChat
{
    partial class ClientForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientForm));
            this.chatHistory = new System.Windows.Forms.TextBox();
            this.serverAddr = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.userNameTextBox = new System.Windows.Forms.TextBox();
            this.sendButton = new System.Windows.Forms.Button();
            this.msgBox = new System.Windows.Forms.TextBox();
            this.serverIPBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.startServerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // chatHistory
            // 
            this.chatHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chatHistory.BackColor = System.Drawing.Color.White;
            this.chatHistory.Cursor = System.Windows.Forms.Cursors.Hand;
            this.chatHistory.Location = new System.Drawing.Point(12, 64);
            this.chatHistory.Multiline = true;
            this.chatHistory.Name = "chatHistory";
            this.chatHistory.ReadOnly = true;
            this.chatHistory.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.chatHistory.Size = new System.Drawing.Size(319, 244);
            this.chatHistory.TabIndex = 0;
            this.chatHistory.Text = "Press Connect to get started!";
            this.chatHistory.TextChanged += new System.EventHandler(this.chatHistory_TextChanged);
            // 
            // serverAddr
            // 
            this.serverAddr.AutoSize = true;
            this.serverAddr.Location = new System.Drawing.Point(9, 14);
            this.serverAddr.Name = "serverAddr";
            this.serverAddr.Size = new System.Drawing.Size(51, 13);
            this.serverAddr.TabIndex = 2;
            this.serverAddr.Text = "Server-IP";
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(256, 8);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(75, 23);
            this.connectButton.TabIndex = 3;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // userNameTextBox
            // 
            this.userNameTextBox.Location = new System.Drawing.Point(67, 37);
            this.userNameTextBox.Name = "userNameTextBox";
            this.userNameTextBox.Size = new System.Drawing.Size(183, 20);
            this.userNameTextBox.TabIndex = 4;
            // 
            // sendButton
            // 
            this.sendButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.sendButton.Enabled = false;
            this.sendButton.Location = new System.Drawing.Point(256, 314);
            this.sendButton.Name = "sendButton";
            this.sendButton.Size = new System.Drawing.Size(75, 23);
            this.sendButton.TabIndex = 5;
            this.sendButton.Text = "Send";
            this.sendButton.UseVisualStyleBackColor = true;
            this.sendButton.Click += new System.EventHandler(this.sendButton_Click);
            // 
            // msgBox
            // 
            this.msgBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msgBox.Location = new System.Drawing.Point(12, 314);
            this.msgBox.Name = "msgBox";
            this.msgBox.Size = new System.Drawing.Size(238, 20);
            this.msgBox.TabIndex = 6;
            this.msgBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.msgBox_KeyDown);
            // 
            // serverIPBox
            // 
            this.serverIPBox.Location = new System.Drawing.Point(67, 11);
            this.serverIPBox.Name = "serverIPBox";
            this.serverIPBox.Size = new System.Drawing.Size(183, 20);
            this.serverIPBox.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Name: ";
            // 
            // startServerButton
            // 
            this.startServerButton.Location = new System.Drawing.Point(256, 35);
            this.startServerButton.Name = "startServerButton";
            this.startServerButton.Size = new System.Drawing.Size(75, 23);
            this.startServerButton.TabIndex = 9;
            this.startServerButton.Text = "Server";
            this.startServerButton.UseVisualStyleBackColor = true;
            this.startServerButton.Click += new System.EventHandler(this.startServerButton_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(343, 360);
            this.Controls.Add(this.startServerButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.serverIPBox);
            this.Controls.Add(this.msgBox);
            this.Controls.Add(this.sendButton);
            this.Controls.Add(this.userNameTextBox);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.serverAddr);
            this.Controls.Add(this.chatHistory);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox chatHistory;
        private System.Windows.Forms.Label serverAddr;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.TextBox userNameTextBox;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.TextBox msgBox;
        private System.Windows.Forms.TextBox serverIPBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startServerButton;
    }
}