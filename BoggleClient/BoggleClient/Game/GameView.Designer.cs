namespace BoggleClient.Game
{
    partial class GameView
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
            this.RemainingSideLabel = new System.Windows.Forms.Label();
            this.RemainingDataLabel = new System.Windows.Forms.Label();
            this.PlayerNameLabel = new System.Windows.Forms.Label();
            this.PlayerScoreLabel = new System.Windows.Forms.Label();
            this.OpponentNameLabel = new System.Windows.Forms.Label();
            this.OpponentScoreLabel = new System.Windows.Forms.Label();
            this.WordTextbox = new System.Windows.Forms.TextBox();
            this.WordLabel = new System.Windows.Forms.Label();
            this.CancelGameButton = new System.Windows.Forms.Button();
            this.BoggleTable = new System.Windows.Forms.TableLayoutPanel();
            this.BoggleFace16 = new System.Windows.Forms.Label();
            this.BoggleFace15 = new System.Windows.Forms.Label();
            this.BoggleFace14 = new System.Windows.Forms.Label();
            this.BoggleFace13 = new System.Windows.Forms.Label();
            this.BoggleFace12 = new System.Windows.Forms.Label();
            this.BoggleFace4 = new System.Windows.Forms.Label();
            this.BoggleFace1 = new System.Windows.Forms.Label();
            this.BoggleFace2 = new System.Windows.Forms.Label();
            this.BoggleFace7 = new System.Windows.Forms.Label();
            this.BoggleFace9 = new System.Windows.Forms.Label();
            this.BoggleFace11 = new System.Windows.Forms.Label();
            this.BoggleFace5 = new System.Windows.Forms.Label();
            this.BoggleFace6 = new System.Windows.Forms.Label();
            this.BoggleFace8 = new System.Windows.Forms.Label();
            this.BoggleFace10 = new System.Windows.Forms.Label();
            this.BoggleFace3 = new System.Windows.Forms.Label();
            this.HelpSubmitLabel = new System.Windows.Forms.Label();
            this.RemainingBar = new System.Windows.Forms.ProgressBar();
            this.GameInfoPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BoggleTable.SuspendLayout();
            this.GameInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RemainingSideLabel
            // 
            this.RemainingSideLabel.AutoSize = true;
            this.RemainingSideLabel.Location = new System.Drawing.Point(12, 28);
            this.RemainingSideLabel.Name = "RemainingSideLabel";
            this.RemainingSideLabel.Size = new System.Drawing.Size(110, 17);
            this.RemainingSideLabel.TabIndex = 1;
            this.RemainingSideLabel.Text = "Time Remaining";
            // 
            // RemainingDataLabel
            // 
            this.RemainingDataLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RemainingDataLabel.AutoSize = true;
            this.RemainingDataLabel.Location = new System.Drawing.Point(63, 65);
            this.RemainingDataLabel.Margin = new System.Windows.Forms.Padding(0);
            this.RemainingDataLabel.Name = "RemainingDataLabel";
            this.RemainingDataLabel.Size = new System.Drawing.Size(36, 17);
            this.RemainingDataLabel.TabIndex = 2;
            this.RemainingDataLabel.Text = "0:00";
            this.RemainingDataLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlayerNameLabel
            // 
            this.PlayerNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PlayerNameLabel.Location = new System.Drawing.Point(37, 50);
            this.PlayerNameLabel.Name = "PlayerNameLabel";
            this.PlayerNameLabel.Size = new System.Drawing.Size(85, 17);
            this.PlayerNameLabel.TabIndex = 3;
            this.PlayerNameLabel.Text = "PlayerName";
            this.PlayerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlayerScoreLabel
            // 
            this.PlayerScoreLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PlayerScoreLabel.AutoSize = true;
            this.PlayerScoreLabel.Location = new System.Drawing.Point(198, 50);
            this.PlayerScoreLabel.Name = "PlayerScoreLabel";
            this.PlayerScoreLabel.Size = new System.Drawing.Size(85, 17);
            this.PlayerScoreLabel.TabIndex = 4;
            this.PlayerScoreLabel.Text = "PlayerScore";
            // 
            // OpponentNameLabel
            // 
            this.OpponentNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OpponentNameLabel.AutoSize = true;
            this.OpponentNameLabel.Location = new System.Drawing.Point(26, 167);
            this.OpponentNameLabel.Name = "OpponentNameLabel";
            this.OpponentNameLabel.Size = new System.Drawing.Size(108, 17);
            this.OpponentNameLabel.TabIndex = 5;
            this.OpponentNameLabel.Text = "OpponentName";
            // 
            // OpponentScoreLabel
            // 
            this.OpponentScoreLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OpponentScoreLabel.AutoSize = true;
            this.OpponentScoreLabel.Location = new System.Drawing.Point(186, 167);
            this.OpponentScoreLabel.Name = "OpponentScoreLabel";
            this.OpponentScoreLabel.Size = new System.Drawing.Size(108, 17);
            this.OpponentScoreLabel.TabIndex = 6;
            this.OpponentScoreLabel.Text = "OpponentScore";
            // 
            // WordTextbox
            // 
            this.WordTextbox.Location = new System.Drawing.Point(611, 330);
            this.WordTextbox.Name = "WordTextbox";
            this.WordTextbox.Size = new System.Drawing.Size(272, 22);
            this.WordTextbox.TabIndex = 7;
            this.WordTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WordTextbox_KeyPress);
            // 
            // WordLabel
            // 
            this.WordLabel.AutoSize = true;
            this.WordLabel.Location = new System.Drawing.Point(559, 330);
            this.WordLabel.Name = "WordLabel";
            this.WordLabel.Size = new System.Drawing.Size(46, 17);
            this.WordLabel.TabIndex = 8;
            this.WordLabel.Text = "Word:";
            // 
            // CancelGameButton
            // 
            this.CancelGameButton.Location = new System.Drawing.Point(653, 464);
            this.CancelGameButton.Name = "CancelGameButton";
            this.CancelGameButton.Size = new System.Drawing.Size(136, 42);
            this.CancelGameButton.TabIndex = 9;
            this.CancelGameButton.Text = "Cancel Game";
            this.CancelGameButton.UseVisualStyleBackColor = true;
            this.CancelGameButton.Click += new System.EventHandler(this.CancelGameButton_Click);
            // 
            // BoggleTable
            // 
            this.BoggleTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.BoggleTable.ColumnCount = 4;
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoggleTable.Controls.Add(this.BoggleFace16, 3, 3);
            this.BoggleTable.Controls.Add(this.BoggleFace15, 2, 3);
            this.BoggleTable.Controls.Add(this.BoggleFace14, 1, 3);
            this.BoggleTable.Controls.Add(this.BoggleFace13, 0, 3);
            this.BoggleTable.Controls.Add(this.BoggleFace12, 3, 2);
            this.BoggleTable.Controls.Add(this.BoggleFace4, 3, 0);
            this.BoggleTable.Controls.Add(this.BoggleFace1, 0, 0);
            this.BoggleTable.Controls.Add(this.BoggleFace2, 1, 0);
            this.BoggleTable.Controls.Add(this.BoggleFace7, 2, 1);
            this.BoggleTable.Controls.Add(this.BoggleFace9, 0, 2);
            this.BoggleTable.Controls.Add(this.BoggleFace11, 2, 2);
            this.BoggleTable.Controls.Add(this.BoggleFace5, 0, 1);
            this.BoggleTable.Controls.Add(this.BoggleFace6, 1, 1);
            this.BoggleTable.Controls.Add(this.BoggleFace8, 3, 1);
            this.BoggleTable.Controls.Add(this.BoggleFace10, 1, 2);
            this.BoggleTable.Controls.Add(this.BoggleFace3, 2, 0);
            this.BoggleTable.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.BoggleTable.Location = new System.Drawing.Point(15, 81);
            this.BoggleTable.Margin = new System.Windows.Forms.Padding(0);
            this.BoggleTable.Name = "BoggleTable";
            this.BoggleTable.RowCount = 4;
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.BoggleTable.Size = new System.Drawing.Size(480, 480);
            this.BoggleTable.TabIndex = 10;
            // 
            // BoggleFace16
            // 
            this.BoggleFace16.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace16.AutoSize = true;
            this.BoggleFace16.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace16.Location = new System.Drawing.Point(387, 387);
            this.BoggleFace16.Name = "BoggleFace16";
            this.BoggleFace16.Size = new System.Drawing.Size(63, 63);
            this.BoggleFace16.TabIndex = 15;
            this.BoggleFace16.Text = "P";
            // 
            // BoggleFace15
            // 
            this.BoggleFace15.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace15.AutoSize = true;
            this.BoggleFace15.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace15.Location = new System.Drawing.Point(263, 387);
            this.BoggleFace15.Name = "BoggleFace15";
            this.BoggleFace15.Size = new System.Drawing.Size(69, 63);
            this.BoggleFace15.TabIndex = 14;
            this.BoggleFace15.Text = "O";
            // 
            // BoggleFace14
            // 
            this.BoggleFace14.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace14.AutoSize = true;
            this.BoggleFace14.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace14.Location = new System.Drawing.Point(146, 387);
            this.BoggleFace14.Name = "BoggleFace14";
            this.BoggleFace14.Size = new System.Drawing.Size(66, 63);
            this.BoggleFace14.TabIndex = 13;
            this.BoggleFace14.Text = "N";
            // 
            // BoggleFace13
            // 
            this.BoggleFace13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace13.AutoSize = true;
            this.BoggleFace13.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace13.Location = new System.Drawing.Point(24, 387);
            this.BoggleFace13.Name = "BoggleFace13";
            this.BoggleFace13.Size = new System.Drawing.Size(72, 63);
            this.BoggleFace13.TabIndex = 12;
            this.BoggleFace13.Text = "M";
            // 
            // BoggleFace12
            // 
            this.BoggleFace12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace12.AutoSize = true;
            this.BoggleFace12.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace12.Location = new System.Drawing.Point(390, 266);
            this.BoggleFace12.Name = "BoggleFace12";
            this.BoggleFace12.Size = new System.Drawing.Size(57, 63);
            this.BoggleFace12.TabIndex = 11;
            this.BoggleFace12.Text = "L";
            // 
            // BoggleFace4
            // 
            this.BoggleFace4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace4.AutoSize = true;
            this.BoggleFace4.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace4.Location = new System.Drawing.Point(385, 28);
            this.BoggleFace4.Name = "BoggleFace4";
            this.BoggleFace4.Size = new System.Drawing.Size(66, 63);
            this.BoggleFace4.TabIndex = 9;
            this.BoggleFace4.Text = "D";
            // 
            // BoggleFace1
            // 
            this.BoggleFace1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace1.AutoSize = true;
            this.BoggleFace1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace1.Location = new System.Drawing.Point(28, 28);
            this.BoggleFace1.Name = "BoggleFace1";
            this.BoggleFace1.Size = new System.Drawing.Size(63, 63);
            this.BoggleFace1.TabIndex = 0;
            this.BoggleFace1.Text = "A";
            // 
            // BoggleFace2
            // 
            this.BoggleFace2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace2.AutoSize = true;
            this.BoggleFace2.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace2.Location = new System.Drawing.Point(147, 28);
            this.BoggleFace2.Name = "BoggleFace2";
            this.BoggleFace2.Size = new System.Drawing.Size(63, 63);
            this.BoggleFace2.TabIndex = 1;
            this.BoggleFace2.Text = "B";
            // 
            // BoggleFace7
            // 
            this.BoggleFace7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace7.AutoSize = true;
            this.BoggleFace7.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace7.Location = new System.Drawing.Point(263, 147);
            this.BoggleFace7.Name = "BoggleFace7";
            this.BoggleFace7.Size = new System.Drawing.Size(69, 63);
            this.BoggleFace7.TabIndex = 5;
            this.BoggleFace7.Text = "G";
            // 
            // BoggleFace9
            // 
            this.BoggleFace9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace9.AutoSize = true;
            this.BoggleFace9.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace9.Location = new System.Drawing.Point(39, 266);
            this.BoggleFace9.Name = "BoggleFace9";
            this.BoggleFace9.Size = new System.Drawing.Size(42, 63);
            this.BoggleFace9.TabIndex = 6;
            this.BoggleFace9.Text = "I";
            // 
            // BoggleFace11
            // 
            this.BoggleFace11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace11.AutoSize = true;
            this.BoggleFace11.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace11.Location = new System.Drawing.Point(266, 266);
            this.BoggleFace11.Name = "BoggleFace11";
            this.BoggleFace11.Size = new System.Drawing.Size(63, 63);
            this.BoggleFace11.TabIndex = 8;
            this.BoggleFace11.Text = "K";
            // 
            // BoggleFace5
            // 
            this.BoggleFace5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace5.AutoSize = true;
            this.BoggleFace5.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace5.Location = new System.Drawing.Point(28, 147);
            this.BoggleFace5.Name = "BoggleFace5";
            this.BoggleFace5.Size = new System.Drawing.Size(63, 63);
            this.BoggleFace5.TabIndex = 3;
            this.BoggleFace5.Text = "E";
            // 
            // BoggleFace6
            // 
            this.BoggleFace6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace6.AutoSize = true;
            this.BoggleFace6.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace6.Location = new System.Drawing.Point(149, 147);
            this.BoggleFace6.Name = "BoggleFace6";
            this.BoggleFace6.Size = new System.Drawing.Size(60, 63);
            this.BoggleFace6.TabIndex = 4;
            this.BoggleFace6.Text = "F";
            // 
            // BoggleFace8
            // 
            this.BoggleFace8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace8.AutoSize = true;
            this.BoggleFace8.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace8.Location = new System.Drawing.Point(385, 147);
            this.BoggleFace8.Name = "BoggleFace8";
            this.BoggleFace8.Size = new System.Drawing.Size(66, 63);
            this.BoggleFace8.TabIndex = 10;
            this.BoggleFace8.Text = "H";
            // 
            // BoggleFace10
            // 
            this.BoggleFace10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace10.AutoSize = true;
            this.BoggleFace10.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace10.Location = new System.Drawing.Point(152, 266);
            this.BoggleFace10.Name = "BoggleFace10";
            this.BoggleFace10.Size = new System.Drawing.Size(54, 63);
            this.BoggleFace10.TabIndex = 7;
            this.BoggleFace10.Text = "J";
            // 
            // BoggleFace3
            // 
            this.BoggleFace3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BoggleFace3.AutoSize = true;
            this.BoggleFace3.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoggleFace3.Location = new System.Drawing.Point(265, 28);
            this.BoggleFace3.Name = "BoggleFace3";
            this.BoggleFace3.Size = new System.Drawing.Size(66, 63);
            this.BoggleFace3.TabIndex = 2;
            this.BoggleFace3.Text = "C";
            // 
            // HelpSubmitLabel
            // 
            this.HelpSubmitLabel.AutoSize = true;
            this.HelpSubmitLabel.Location = new System.Drawing.Point(521, 393);
            this.HelpSubmitLabel.Name = "HelpSubmitLabel";
            this.HelpSubmitLabel.Size = new System.Drawing.Size(328, 17);
            this.HelpSubmitLabel.TabIndex = 11;
            this.HelpSubmitLabel.Text = "To submit a word, simply type it in and press enter.";
            // 
            // RemainingBar
            // 
            this.RemainingBar.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.RemainingBar.Location = new System.Drawing.Point(125, 28);
            this.RemainingBar.Name = "RemainingBar";
            this.RemainingBar.Size = new System.Drawing.Size(793, 23);
            this.RemainingBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.RemainingBar.TabIndex = 0;
            // 
            // GameInfoPanel
            // 
            this.GameInfoPanel.ColumnCount = 2;
            this.GameInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GameInfoPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GameInfoPanel.Controls.Add(this.PlayerNameLabel, 0, 0);
            this.GameInfoPanel.Controls.Add(this.PlayerScoreLabel, 1, 0);
            this.GameInfoPanel.Controls.Add(this.OpponentNameLabel, 0, 1);
            this.GameInfoPanel.Controls.Add(this.OpponentScoreLabel, 1, 1);
            this.GameInfoPanel.Location = new System.Drawing.Point(562, 81);
            this.GameInfoPanel.Margin = new System.Windows.Forms.Padding(0);
            this.GameInfoPanel.Name = "GameInfoPanel";
            this.GameInfoPanel.RowCount = 2;
            this.GameInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GameInfoPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.GameInfoPanel.Size = new System.Drawing.Size(321, 235);
            this.GameInfoPanel.TabIndex = 12;
            // 
            // GameView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 581);
            this.Controls.Add(this.GameInfoPanel);
            this.Controls.Add(this.HelpSubmitLabel);
            this.Controls.Add(this.BoggleTable);
            this.Controls.Add(this.CancelGameButton);
            this.Controls.Add(this.WordLabel);
            this.Controls.Add(this.WordTextbox);
            this.Controls.Add(this.RemainingDataLabel);
            this.Controls.Add(this.RemainingSideLabel);
            this.Controls.Add(this.RemainingBar);
            this.Name = "GameView";
            this.Text = "BoggleClient";
            this.BoggleTable.ResumeLayout(false);
            this.BoggleTable.PerformLayout();
            this.GameInfoPanel.ResumeLayout(false);
            this.GameInfoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label RemainingSideLabel;
        private System.Windows.Forms.Label RemainingDataLabel;
        private System.Windows.Forms.Label PlayerNameLabel;
        private System.Windows.Forms.Label PlayerScoreLabel;
        private System.Windows.Forms.Label OpponentNameLabel;
        private System.Windows.Forms.Label OpponentScoreLabel;
        private System.Windows.Forms.TextBox WordTextbox;
        private System.Windows.Forms.Label WordLabel;
        private System.Windows.Forms.Button CancelGameButton;
        private System.Windows.Forms.TableLayoutPanel BoggleTable;
        private System.Windows.Forms.Label BoggleFace1;
        private System.Windows.Forms.Label BoggleFace2;
        private System.Windows.Forms.Label BoggleFace3;
        private System.Windows.Forms.Label BoggleFace5;
        private System.Windows.Forms.Label BoggleFace6;
        private System.Windows.Forms.Label BoggleFace7;
        private System.Windows.Forms.Label BoggleFace9;
        private System.Windows.Forms.Label BoggleFace10;
        private System.Windows.Forms.Label BoggleFace11;
        private System.Windows.Forms.Label HelpSubmitLabel;
        private System.Windows.Forms.ProgressBar RemainingBar;
        private System.Windows.Forms.TableLayoutPanel GameInfoPanel;
        private System.Windows.Forms.Label BoggleFace12;
        private System.Windows.Forms.Label BoggleFace8;
        private System.Windows.Forms.Label BoggleFace4;
        private System.Windows.Forms.Label BoggleFace16;
        private System.Windows.Forms.Label BoggleFace15;
        private System.Windows.Forms.Label BoggleFace14;
        private System.Windows.Forms.Label BoggleFace13;
    }
}