namespace BoggleClient.Open
{
    partial class OpenView
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
            this.ServerTextbox = new System.Windows.Forms.TextBox();
            this.ServerRegisterButton = new System.Windows.Forms.Button();
            this.CancelRegisterButton = new System.Windows.Forms.Button();
            this.NameTextbox = new System.Windows.Forms.TextBox();
            this.DurationTextbox = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.DurationLabel = new System.Windows.Forms.Label();
            this.SearchGamesButton = new System.Windows.Forms.Button();
            this.CancelSearchButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ServerTextbox
            // 
            this.ServerTextbox.Location = new System.Drawing.Point(124, 71);
            this.ServerTextbox.Name = "ServerTextbox";
            this.ServerTextbox.Size = new System.Drawing.Size(100, 22);
            this.ServerTextbox.TabIndex = 0;
            // 
            // ServerRegisterButton
            // 
            this.ServerRegisterButton.Location = new System.Drawing.Point(110, 178);
            this.ServerRegisterButton.Name = "ServerRegisterButton";
            this.ServerRegisterButton.Size = new System.Drawing.Size(149, 34);
            this.ServerRegisterButton.TabIndex = 1;
            this.ServerRegisterButton.Text = "Register with Server";
            this.ServerRegisterButton.UseVisualStyleBackColor = true;
            this.ServerRegisterButton.Click += new System.EventHandler(this.ServerRegisterButton_Click);
            // 
            // CancelRegisterButton
            // 
            this.CancelRegisterButton.Location = new System.Drawing.Point(110, 218);
            this.CancelRegisterButton.Name = "CancelRegisterButton";
            this.CancelRegisterButton.Size = new System.Drawing.Size(149, 34);
            this.CancelRegisterButton.TabIndex = 2;
            this.CancelRegisterButton.Text = "Cancel Registration";
            this.CancelRegisterButton.UseVisualStyleBackColor = true;
            this.CancelRegisterButton.Click += new System.EventHandler(this.CancelRegisterButton_Click);
            // 
            // NameTextbox
            // 
            this.NameTextbox.Location = new System.Drawing.Point(124, 121);
            this.NameTextbox.Name = "NameTextbox";
            this.NameTextbox.Size = new System.Drawing.Size(100, 22);
            this.NameTextbox.TabIndex = 3;
            // 
            // DurationTextbox
            // 
            this.DurationTextbox.Location = new System.Drawing.Point(430, 74);
            this.DurationTextbox.Name = "DurationTextbox";
            this.DurationTextbox.Size = new System.Drawing.Size(100, 22);
            this.DurationTextbox.TabIndex = 4;
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(12, 74);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(106, 17);
            this.ServerLabel.TabIndex = 6;
            this.ServerLabel.Text = "Server Address";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(29, 121);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(89, 17);
            this.NameLabel.TabIndex = 7;
            this.NameLabel.Text = "Player Name";
            // 
            // DurationLabel
            // 
            this.DurationLabel.AutoSize = true;
            this.DurationLabel.Location = new System.Drawing.Point(320, 74);
            this.DurationLabel.Name = "DurationLabel";
            this.DurationLabel.Size = new System.Drawing.Size(104, 17);
            this.DurationLabel.TabIndex = 8;
            this.DurationLabel.Text = "Game Duration";
            // 
            // SearchGamesButton
            // 
            this.SearchGamesButton.Location = new System.Drawing.Point(376, 178);
            this.SearchGamesButton.Name = "SearchGamesButton";
            this.SearchGamesButton.Size = new System.Drawing.Size(149, 34);
            this.SearchGamesButton.TabIndex = 9;
            this.SearchGamesButton.Text = "Search for Game";
            this.SearchGamesButton.UseVisualStyleBackColor = true;
            this.SearchGamesButton.Click += new System.EventHandler(this.SearchGamesButton_Click);
            // 
            // CancelSearchButton
            // 
            this.CancelSearchButton.Location = new System.Drawing.Point(376, 219);
            this.CancelSearchButton.Name = "CancelSearchButton";
            this.CancelSearchButton.Size = new System.Drawing.Size(149, 33);
            this.CancelSearchButton.TabIndex = 10;
            this.CancelSearchButton.Text = "Stop Searching";
            this.CancelSearchButton.UseVisualStyleBackColor = true;
            this.CancelSearchButton.Click += new System.EventHandler(this.CancelSearchButton_Click);
            // 
            // OpenView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CancelSearchButton);
            this.Controls.Add(this.SearchGamesButton);
            this.Controls.Add(this.DurationLabel);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.DurationTextbox);
            this.Controls.Add(this.NameTextbox);
            this.Controls.Add(this.CancelRegisterButton);
            this.Controls.Add(this.ServerRegisterButton);
            this.Controls.Add(this.ServerTextbox);
            this.Name = "OpenView";
            this.Text = "BoggleClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ServerTextbox;
        private System.Windows.Forms.Button ServerRegisterButton;
        private System.Windows.Forms.Button CancelRegisterButton;
        private System.Windows.Forms.TextBox NameTextbox;
        private System.Windows.Forms.TextBox DurationTextbox;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Label DurationLabel;
        private System.Windows.Forms.Button SearchGamesButton;
        private System.Windows.Forms.Button CancelSearchButton;
    }
}

