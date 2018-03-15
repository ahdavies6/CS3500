namespace BoggleClient.Score
{
    partial class ScoreView
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
            this.PlayerWordsDataLabel = new System.Windows.Forms.Label();
            this.AbovePlayerWordsLabel = new System.Windows.Forms.Label();
            this.AbovePlayerScoresLabel = new System.Windows.Forms.Label();
            this.PlayerScoresDataLabel = new System.Windows.Forms.Label();
            this.AboveOpponentWordsLabel = new System.Windows.Forms.Label();
            this.AboveOpponentScoresLabel = new System.Windows.Forms.Label();
            this.OpponentWordsDataLabel = new System.Windows.Forms.Label();
            this.OpponentScoresDataLabel = new System.Windows.Forms.Label();
            this.ReturnButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PlayerWordsDataLabel
            // 
            this.PlayerWordsDataLabel.Location = new System.Drawing.Point(60, 98);
            this.PlayerWordsDataLabel.Name = "PlayerWordsDataLabel";
            this.PlayerWordsDataLabel.Size = new System.Drawing.Size(96, 292);
            this.PlayerWordsDataLabel.TabIndex = 0;
            this.PlayerWordsDataLabel.Text = "word";
            // 
            // AbovePlayerWordsLabel
            // 
            this.AbovePlayerWordsLabel.AutoSize = true;
            this.AbovePlayerWordsLabel.Location = new System.Drawing.Point(63, 58);
            this.AbovePlayerWordsLabel.Name = "AbovePlayerWordsLabel";
            this.AbovePlayerWordsLabel.Size = new System.Drawing.Size(89, 17);
            this.AbovePlayerWordsLabel.TabIndex = 1;
            this.AbovePlayerWordsLabel.Text = "PlayerWords";
            // 
            // AbovePlayerScoresLabel
            // 
            this.AbovePlayerScoresLabel.AutoSize = true;
            this.AbovePlayerScoresLabel.Location = new System.Drawing.Point(177, 58);
            this.AbovePlayerScoresLabel.Name = "AbovePlayerScoresLabel";
            this.AbovePlayerScoresLabel.Size = new System.Drawing.Size(45, 17);
            this.AbovePlayerScoresLabel.TabIndex = 2;
            this.AbovePlayerScoresLabel.Text = "Score";
            // 
            // PlayerScoresDataLabel
            // 
            this.PlayerScoresDataLabel.Location = new System.Drawing.Point(180, 98);
            this.PlayerScoresDataLabel.Name = "PlayerScoresDataLabel";
            this.PlayerScoresDataLabel.Size = new System.Drawing.Size(132, 292);
            this.PlayerScoresDataLabel.TabIndex = 3;
            this.PlayerScoresDataLabel.Text = "0";
            // 
            // AboveOpponentWordsLabel
            // 
            this.AboveOpponentWordsLabel.AutoSize = true;
            this.AboveOpponentWordsLabel.Location = new System.Drawing.Point(426, 58);
            this.AboveOpponentWordsLabel.Name = "AboveOpponentWordsLabel";
            this.AboveOpponentWordsLabel.Size = new System.Drawing.Size(112, 17);
            this.AboveOpponentWordsLabel.TabIndex = 4;
            this.AboveOpponentWordsLabel.Text = "OpponentWords";
            // 
            // AboveOpponentScoreLabel
            // 
            this.AboveOpponentScoresLabel.AutoSize = true;
            this.AboveOpponentScoresLabel.Location = new System.Drawing.Point(579, 58);
            this.AboveOpponentScoresLabel.Name = "AboveOpponentScoreLabel";
            this.AboveOpponentScoresLabel.Size = new System.Drawing.Size(45, 17);
            this.AboveOpponentScoresLabel.TabIndex = 5;
            this.AboveOpponentScoresLabel.Text = "Score";
            // 
            // OpponentWordsDataLabel
            // 
            this.OpponentWordsDataLabel.Location = new System.Drawing.Point(415, 98);
            this.OpponentWordsDataLabel.Name = "OpponentWordsDataLabel";
            this.OpponentWordsDataLabel.Size = new System.Drawing.Size(100, 292);
            this.OpponentWordsDataLabel.TabIndex = 6;
            this.OpponentWordsDataLabel.Text = "word";
            // 
            // OpponentScoresDataLabel
            // 
            this.OpponentScoresDataLabel.Location = new System.Drawing.Point(573, 98);
            this.OpponentScoresDataLabel.Name = "OpponentScoresDataLabel";
            this.OpponentScoresDataLabel.Size = new System.Drawing.Size(100, 292);
            this.OpponentScoresDataLabel.TabIndex = 7;
            this.OpponentScoresDataLabel.Text = "0";
            // 
            // ReturnButton
            // 
            this.ReturnButton.Location = new System.Drawing.Point(631, 393);
            this.ReturnButton.Name = "ReturnButton";
            this.ReturnButton.Size = new System.Drawing.Size(157, 45);
            this.ReturnButton.TabIndex = 8;
            this.ReturnButton.Text = "Return To Main Menu";
            this.ReturnButton.UseVisualStyleBackColor = true;
            this.ReturnButton.Click += new System.EventHandler(this.ReturnButton_Click);
            // 
            // ScoreView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ReturnButton);
            this.Controls.Add(this.OpponentScoresDataLabel);
            this.Controls.Add(this.OpponentWordsDataLabel);
            this.Controls.Add(this.AboveOpponentScoresLabel);
            this.Controls.Add(this.AboveOpponentWordsLabel);
            this.Controls.Add(this.PlayerScoresDataLabel);
            this.Controls.Add(this.AbovePlayerScoresLabel);
            this.Controls.Add(this.AbovePlayerWordsLabel);
            this.Controls.Add(this.PlayerWordsDataLabel);
            this.Name = "ScoreView";
            this.Text = "BoggleClient";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PlayerWordsDataLabel;
        private System.Windows.Forms.Label AbovePlayerWordsLabel;
        private System.Windows.Forms.Label AbovePlayerScoresLabel;
        private System.Windows.Forms.Label PlayerScoresDataLabel;
        private System.Windows.Forms.Label AboveOpponentWordsLabel;
        private System.Windows.Forms.Label AboveOpponentScoresLabel;
        private System.Windows.Forms.Label OpponentWordsDataLabel;
        private System.Windows.Forms.Label OpponentScoresDataLabel;
        private System.Windows.Forms.Button ReturnButton;
    }
}