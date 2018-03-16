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
            this.ServerRegisterButton = new System.Windows.Forms.Button();
            this.CancelRegisterButton = new System.Windows.Forms.Button();
            this.NameTextbox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.SearchGamesButton = new System.Windows.Forms.Button();
            this.CancelSearchButton = new System.Windows.Forms.Button();
            this.DurationTextbox = new System.Windows.Forms.TextBox();
            this.DurationLabel = new System.Windows.Forms.Label();
            this.ServerTextbox = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.MainPanel = new System.Windows.Forms.TableLayoutPanel();
            this.MainPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ServerRegisterButton
            // 
            this.ServerRegisterButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ServerRegisterButton.Location = new System.Drawing.Point(85, 325);
            this.ServerRegisterButton.Name = "ServerRegisterButton";
            this.ServerRegisterButton.Size = new System.Drawing.Size(166, 34);
            this.ServerRegisterButton.TabIndex = 1;
            this.ServerRegisterButton.Text = "Register with Server";
            this.ServerRegisterButton.UseVisualStyleBackColor = true;
            this.ServerRegisterButton.Click += new System.EventHandler(this.ServerRegisterButton_Click);
            // 
            // CancelRegisterButton
            // 
            this.CancelRegisterButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CancelRegisterButton.Location = new System.Drawing.Point(85, 365);
            this.CancelRegisterButton.Name = "CancelRegisterButton";
            this.CancelRegisterButton.Size = new System.Drawing.Size(166, 34);
            this.CancelRegisterButton.TabIndex = 2;
            this.CancelRegisterButton.Text = "Cancel Registration";
            this.CancelRegisterButton.UseVisualStyleBackColor = true;
            this.CancelRegisterButton.Click += new System.EventHandler(this.CancelRegisterButton_Click);
            // 
            // NameTextbox
            // 
            this.NameTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MainPanel.SetColumnSpan(this.NameTextbox, 2);
            this.NameTextbox.Location = new System.Drawing.Point(147, 68);
            this.NameTextbox.Name = "NameTextbox";
            this.NameTextbox.Size = new System.Drawing.Size(218, 22);
            this.NameTextbox.TabIndex = 3;
            // 
            // NameLabel
            // 
            this.NameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(24, 71);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(89, 17);
            this.NameLabel.TabIndex = 7;
            this.NameLabel.Text = "Player Name";
            // 
            // SearchGamesButton
            // 
            this.SearchGamesButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SearchGamesButton.Location = new System.Drawing.Point(534, 325);
            this.SearchGamesButton.Name = "SearchGamesButton";
            this.SearchGamesButton.Size = new System.Drawing.Size(166, 34);
            this.SearchGamesButton.TabIndex = 9;
            this.SearchGamesButton.Text = "Search for Game";
            this.SearchGamesButton.UseVisualStyleBackColor = true;
            this.SearchGamesButton.Click += new System.EventHandler(this.SearchGamesButton_Click);
            // 
            // CancelSearchButton
            // 
            this.CancelSearchButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.CancelSearchButton.Location = new System.Drawing.Point(534, 365);
            this.CancelSearchButton.Name = "CancelSearchButton";
            this.CancelSearchButton.Size = new System.Drawing.Size(166, 33);
            this.CancelSearchButton.TabIndex = 10;
            this.CancelSearchButton.Text = "Stop Searching";
            this.CancelSearchButton.UseVisualStyleBackColor = true;
            this.CancelSearchButton.Click += new System.EventHandler(this.CancelSearchButton_Click);
            // 
            // DurationTextbox
            // 
            this.DurationTextbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.MainPanel.SetColumnSpan(this.DurationTextbox, 2);
            this.DurationTextbox.Location = new System.Drawing.Point(534, 42);
            this.DurationTextbox.Name = "DurationTextbox";
            this.MainPanel.SetRowSpan(this.DurationTextbox, 2);
            this.DurationTextbox.Size = new System.Drawing.Size(225, 22);
            this.DurationTextbox.TabIndex = 4;
            // 
            // DurationLabel
            // 
            this.DurationLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DurationLabel.AutoSize = true;
            this.DurationLabel.Location = new System.Drawing.Point(410, 44);
            this.DurationLabel.Name = "DurationLabel";
            this.MainPanel.SetRowSpan(this.DurationLabel, 2);
            this.DurationLabel.Size = new System.Drawing.Size(104, 17);
            this.DurationLabel.TabIndex = 8;
            this.DurationLabel.Text = "Game Duration";
            // 
            // ServerTextbox
            // 
            this.ServerTextbox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MainPanel.SetColumnSpan(this.ServerTextbox, 2);
            this.ServerTextbox.Location = new System.Drawing.Point(147, 15);
            this.ServerTextbox.Name = "ServerTextbox";
            this.ServerTextbox.Size = new System.Drawing.Size(218, 22);
            this.ServerTextbox.TabIndex = 0;
            // 
            // ServerLabel
            // 
            this.ServerLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(16, 18);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(106, 17);
            this.ServerLabel.TabIndex = 6;
            this.ServerLabel.Text = "Server Address";
            // 
            // MainPanel
            // 
            this.MainPanel.ColumnCount = 8;
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.5F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.5F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.5F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.5F));
            this.MainPanel.Controls.Add(this.ServerLabel, 0, 0);
            this.MainPanel.Controls.Add(this.ServerTextbox, 1, 0);
            this.MainPanel.Controls.Add(this.NameTextbox, 1, 1);
            this.MainPanel.Controls.Add(this.NameLabel, 0, 1);
            this.MainPanel.Controls.Add(this.DurationTextbox, 5, 0);
            this.MainPanel.Controls.Add(this.DurationLabel, 4, 0);
            this.MainPanel.Location = new System.Drawing.Point(0, 0);
            this.MainPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.RowCount = 6;
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66944F));
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.MainPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66611F));
            this.MainPanel.Size = new System.Drawing.Size(791, 322);
            this.MainPanel.TabIndex = 11;
            // 
            // OpenView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CancelSearchButton);
            this.Controls.Add(this.ServerRegisterButton);
            this.Controls.Add(this.CancelRegisterButton);
            this.Controls.Add(this.SearchGamesButton);
            this.Controls.Add(this.MainPanel);
            this.Name = "OpenView";
            this.Text = "BoggleClient";
            this.MainPanel.ResumeLayout(false);
            this.MainPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ServerRegisterButton;
        private System.Windows.Forms.Button CancelRegisterButton;
        private System.Windows.Forms.TextBox NameTextbox;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Button SearchGamesButton;
        private System.Windows.Forms.Button CancelSearchButton;
        private System.Windows.Forms.TextBox DurationTextbox;
        private System.Windows.Forms.Label DurationLabel;
        private System.Windows.Forms.TextBox ServerTextbox;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.TableLayoutPanel MainPanel;
    }
}

