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
            this.RemainingBar = new System.Windows.Forms.ProgressBar();
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
            this.TopLeftLabel = new System.Windows.Forms.Label();
            this.TopMiddleLabel = new System.Windows.Forms.Label();
            this.TopRightLabel = new System.Windows.Forms.Label();
            this.MiddleLeftLabel = new System.Windows.Forms.Label();
            this.MiddleMiddleLabel = new System.Windows.Forms.Label();
            this.MiddleRightLabel = new System.Windows.Forms.Label();
            this.BottomLeftLabel = new System.Windows.Forms.Label();
            this.BottomMiddleLabel = new System.Windows.Forms.Label();
            this.BottomRightLabel = new System.Windows.Forms.Label();
            this.HelpSubmitLabel = new System.Windows.Forms.Label();
            this.BoggleTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // RemainingBar
            // 
            this.RemainingBar.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.RemainingBar.Location = new System.Drawing.Point(125, 18);
            this.RemainingBar.Name = "RemainingBar";
            this.RemainingBar.Size = new System.Drawing.Size(663, 23);
            this.RemainingBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.RemainingBar.TabIndex = 0;
            // 
            // RemainingSideLabel
            // 
            this.RemainingSideLabel.AutoSize = true;
            this.RemainingSideLabel.Location = new System.Drawing.Point(9, 18);
            this.RemainingSideLabel.Name = "RemainingSideLabel";
            this.RemainingSideLabel.Size = new System.Drawing.Size(110, 17);
            this.RemainingSideLabel.TabIndex = 1;
            this.RemainingSideLabel.Text = "Time Remaining";
            // 
            // RemainingDataLabel
            // 
            this.RemainingDataLabel.AutoSize = true;
            this.RemainingDataLabel.Location = new System.Drawing.Point(436, 18);
            this.RemainingDataLabel.Name = "RemainingDataLabel";
            this.RemainingDataLabel.Size = new System.Drawing.Size(36, 17);
            this.RemainingDataLabel.TabIndex = 2;
            this.RemainingDataLabel.Text = "0:00";
            // 
            // PlayerNameLabel
            // 
            this.PlayerNameLabel.AutoSize = true;
            this.PlayerNameLabel.Location = new System.Drawing.Point(401, 109);
            this.PlayerNameLabel.Name = "PlayerNameLabel";
            this.PlayerNameLabel.Size = new System.Drawing.Size(85, 17);
            this.PlayerNameLabel.TabIndex = 3;
            this.PlayerNameLabel.Text = "PlayerName";
            // 
            // PlayerScoreLabel
            // 
            this.PlayerScoreLabel.AutoSize = true;
            this.PlayerScoreLabel.Location = new System.Drawing.Point(569, 109);
            this.PlayerScoreLabel.Name = "PlayerScoreLabel";
            this.PlayerScoreLabel.Size = new System.Drawing.Size(85, 17);
            this.PlayerScoreLabel.TabIndex = 4;
            this.PlayerScoreLabel.Text = "PlayerScore";
            // 
            // OpponentNameLabel
            // 
            this.OpponentNameLabel.AutoSize = true;
            this.OpponentNameLabel.Location = new System.Drawing.Point(401, 220);
            this.OpponentNameLabel.Name = "OpponentNameLabel";
            this.OpponentNameLabel.Size = new System.Drawing.Size(108, 17);
            this.OpponentNameLabel.TabIndex = 5;
            this.OpponentNameLabel.Text = "OpponentName";
            // 
            // OpponentScoreLabel
            // 
            this.OpponentScoreLabel.AutoSize = true;
            this.OpponentScoreLabel.Location = new System.Drawing.Point(569, 220);
            this.OpponentScoreLabel.Name = "OpponentScoreLabel";
            this.OpponentScoreLabel.Size = new System.Drawing.Size(108, 17);
            this.OpponentScoreLabel.TabIndex = 6;
            this.OpponentScoreLabel.Text = "OpponentScore";
            // 
            // WordTextbox
            // 
            this.WordTextbox.Location = new System.Drawing.Point(477, 318);
            this.WordTextbox.Name = "WordTextbox";
            this.WordTextbox.Size = new System.Drawing.Size(100, 22);
            this.WordTextbox.TabIndex = 7;
            this.WordTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WordTextbox_KeyPress);
            // 
            // WordLabel
            // 
            this.WordLabel.AutoSize = true;
            this.WordLabel.Location = new System.Drawing.Point(425, 321);
            this.WordLabel.Name = "WordLabel";
            this.WordLabel.Size = new System.Drawing.Size(46, 17);
            this.WordLabel.TabIndex = 8;
            this.WordLabel.Text = "Word:";
            // 
            // CancelGameButton
            // 
            this.CancelGameButton.Location = new System.Drawing.Point(652, 396);
            this.CancelGameButton.Name = "CancelGameButton";
            this.CancelGameButton.Size = new System.Drawing.Size(136, 42);
            this.CancelGameButton.TabIndex = 9;
            this.CancelGameButton.Text = "Cancel Game";
            this.CancelGameButton.UseVisualStyleBackColor = true;
            this.CancelGameButton.Click += new System.EventHandler(this.CancelGameButton_Click);
            // 
            // BoggleTable
            // 
            this.BoggleTable.ColumnCount = 3;
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.BoggleTable.Controls.Add(this.TopLeftLabel, 0, 0);
            this.BoggleTable.Controls.Add(this.TopMiddleLabel, 1, 0);
            this.BoggleTable.Controls.Add(this.TopRightLabel, 2, 0);
            this.BoggleTable.Controls.Add(this.MiddleLeftLabel, 0, 1);
            this.BoggleTable.Controls.Add(this.MiddleMiddleLabel, 1, 1);
            this.BoggleTable.Controls.Add(this.MiddleRightLabel, 2, 1);
            this.BoggleTable.Controls.Add(this.BottomLeftLabel, 0, 2);
            this.BoggleTable.Controls.Add(this.BottomMiddleLabel, 1, 2);
            this.BoggleTable.Controls.Add(this.BottomRightLabel, 2, 2);
            this.BoggleTable.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.BoggleTable.Location = new System.Drawing.Point(12, 78);
            this.BoggleTable.Margin = new System.Windows.Forms.Padding(0);
            this.BoggleTable.Name = "BoggleTable";
            this.BoggleTable.RowCount = 3;
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.BoggleTable.Size = new System.Drawing.Size(360, 360);
            this.BoggleTable.TabIndex = 10;
            // 
            // TopLeftLabel
            // 
            this.TopLeftLabel.AutoSize = true;
            this.TopLeftLabel.Location = new System.Drawing.Point(3, 0);
            this.TopLeftLabel.Name = "TopLeftLabel";
            this.TopLeftLabel.Size = new System.Drawing.Size(17, 17);
            this.TopLeftLabel.TabIndex = 0;
            this.TopLeftLabel.Text = "A";
            // 
            // TopMiddleLabel
            // 
            this.TopMiddleLabel.AutoSize = true;
            this.TopMiddleLabel.Location = new System.Drawing.Point(123, 0);
            this.TopMiddleLabel.Name = "TopMiddleLabel";
            this.TopMiddleLabel.Size = new System.Drawing.Size(17, 17);
            this.TopMiddleLabel.TabIndex = 1;
            this.TopMiddleLabel.Text = "B";
            // 
            // TopRightLabel
            // 
            this.TopRightLabel.AutoSize = true;
            this.TopRightLabel.Location = new System.Drawing.Point(243, 0);
            this.TopRightLabel.Name = "TopRightLabel";
            this.TopRightLabel.Size = new System.Drawing.Size(17, 17);
            this.TopRightLabel.TabIndex = 2;
            this.TopRightLabel.Text = "C";
            // 
            // MiddleLeftLabel
            // 
            this.MiddleLeftLabel.AutoSize = true;
            this.MiddleLeftLabel.Location = new System.Drawing.Point(3, 120);
            this.MiddleLeftLabel.Name = "MiddleLeftLabel";
            this.MiddleLeftLabel.Size = new System.Drawing.Size(18, 17);
            this.MiddleLeftLabel.TabIndex = 3;
            this.MiddleLeftLabel.Text = "D";
            // 
            // MiddleMiddleLabel
            // 
            this.MiddleMiddleLabel.AutoSize = true;
            this.MiddleMiddleLabel.Location = new System.Drawing.Point(123, 120);
            this.MiddleMiddleLabel.Name = "MiddleMiddleLabel";
            this.MiddleMiddleLabel.Size = new System.Drawing.Size(17, 17);
            this.MiddleMiddleLabel.TabIndex = 4;
            this.MiddleMiddleLabel.Text = "E";
            // 
            // MiddleRightLabel
            // 
            this.MiddleRightLabel.AutoSize = true;
            this.MiddleRightLabel.Location = new System.Drawing.Point(243, 120);
            this.MiddleRightLabel.Name = "MiddleRightLabel";
            this.MiddleRightLabel.Size = new System.Drawing.Size(16, 17);
            this.MiddleRightLabel.TabIndex = 5;
            this.MiddleRightLabel.Text = "F";
            // 
            // BottomLeftLabel
            // 
            this.BottomLeftLabel.AutoSize = true;
            this.BottomLeftLabel.Location = new System.Drawing.Point(3, 240);
            this.BottomLeftLabel.Name = "BottomLeftLabel";
            this.BottomLeftLabel.Size = new System.Drawing.Size(19, 17);
            this.BottomLeftLabel.TabIndex = 6;
            this.BottomLeftLabel.Text = "G";
            // 
            // BottomMiddleLabel
            // 
            this.BottomMiddleLabel.AutoSize = true;
            this.BottomMiddleLabel.Location = new System.Drawing.Point(123, 240);
            this.BottomMiddleLabel.Name = "BottomMiddleLabel";
            this.BottomMiddleLabel.Size = new System.Drawing.Size(18, 17);
            this.BottomMiddleLabel.TabIndex = 7;
            this.BottomMiddleLabel.Text = "H";
            // 
            // BottomRightLabel
            // 
            this.BottomRightLabel.AutoSize = true;
            this.BottomRightLabel.Location = new System.Drawing.Point(243, 240);
            this.BottomRightLabel.Name = "BottomRightLabel";
            this.BottomRightLabel.Size = new System.Drawing.Size(11, 17);
            this.BottomRightLabel.TabIndex = 8;
            this.BottomRightLabel.Text = "I";
            // 
            // HelpSubmitLabel
            // 
            this.HelpSubmitLabel.AutoSize = true;
            this.HelpSubmitLabel.Location = new System.Drawing.Point(401, 343);
            this.HelpSubmitLabel.Name = "HelpSubmitLabel";
            this.HelpSubmitLabel.Size = new System.Drawing.Size(328, 17);
            this.HelpSubmitLabel.TabIndex = 11;
            this.HelpSubmitLabel.Text = "To submit a word, simply type it in and press enter.";
            // 
            // GameView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.HelpSubmitLabel);
            this.Controls.Add(this.BoggleTable);
            this.Controls.Add(this.CancelGameButton);
            this.Controls.Add(this.WordLabel);
            this.Controls.Add(this.WordTextbox);
            this.Controls.Add(this.OpponentScoreLabel);
            this.Controls.Add(this.OpponentNameLabel);
            this.Controls.Add(this.PlayerScoreLabel);
            this.Controls.Add(this.PlayerNameLabel);
            this.Controls.Add(this.RemainingDataLabel);
            this.Controls.Add(this.RemainingSideLabel);
            this.Controls.Add(this.RemainingBar);
            this.Name = "GameView";
            this.Text = "BoggleClient";
            this.BoggleTable.ResumeLayout(false);
            this.BoggleTable.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar RemainingBar;
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
        private System.Windows.Forms.Label TopLeftLabel;
        private System.Windows.Forms.Label TopMiddleLabel;
        private System.Windows.Forms.Label TopRightLabel;
        private System.Windows.Forms.Label MiddleLeftLabel;
        private System.Windows.Forms.Label MiddleMiddleLabel;
        private System.Windows.Forms.Label MiddleRightLabel;
        private System.Windows.Forms.Label BottomLeftLabel;
        private System.Windows.Forms.Label BottomMiddleLabel;
        private System.Windows.Forms.Label BottomRightLabel;
        private System.Windows.Forms.Label HelpSubmitLabel;
    }
}