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
            this.blable1 = new System.Windows.Forms.Label();
            this.blable2 = new System.Windows.Forms.Label();
            this.blable3 = new System.Windows.Forms.Label();
            this.blable5 = new System.Windows.Forms.Label();
            this.blable6 = new System.Windows.Forms.Label();
            this.blable7 = new System.Windows.Forms.Label();
            this.blabel9 = new System.Windows.Forms.Label();
            this.blabel10 = new System.Windows.Forms.Label();
            this.blabel11 = new System.Windows.Forms.Label();
            this.HelpSubmitLabel = new System.Windows.Forms.Label();
            this.RemainingBar = new System.Windows.Forms.ProgressBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.blable4 = new System.Windows.Forms.Label();
            this.blable18 = new System.Windows.Forms.Label();
            this.blabel12 = new System.Windows.Forms.Label();
            this.blabel13 = new System.Windows.Forms.Label();
            this.blabel14 = new System.Windows.Forms.Label();
            this.blabel15 = new System.Windows.Forms.Label();
            this.blabel16 = new System.Windows.Forms.Label();
            this.BoggleTable.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // RemainingSideLabel
            // 
            this.RemainingSideLabel.AutoSize = true;
            this.RemainingSideLabel.Location = new System.Drawing.Point(16, 42);
            this.RemainingSideLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RemainingSideLabel.Name = "RemainingSideLabel";
            this.RemainingSideLabel.Size = new System.Drawing.Size(153, 25);
            this.RemainingSideLabel.TabIndex = 1;
            this.RemainingSideLabel.Text = "Time Remaining";
            // 
            // RemainingDataLabel
            // 
            this.RemainingDataLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.RemainingDataLabel.AutoSize = true;
            this.RemainingDataLabel.Location = new System.Drawing.Point(87, 98);
            this.RemainingDataLabel.Margin = new System.Windows.Forms.Padding(0);
            this.RemainingDataLabel.Name = "RemainingDataLabel";
            this.RemainingDataLabel.Size = new System.Drawing.Size(51, 25);
            this.RemainingDataLabel.TabIndex = 2;
            this.RemainingDataLabel.Text = "0:00";
            this.RemainingDataLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlayerNameLabel
            // 
            this.PlayerNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PlayerNameLabel.Location = new System.Drawing.Point(52, 75);
            this.PlayerNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PlayerNameLabel.Name = "PlayerNameLabel";
            this.PlayerNameLabel.Size = new System.Drawing.Size(117, 26);
            this.PlayerNameLabel.TabIndex = 3;
            this.PlayerNameLabel.Text = "PlayerName";
            this.PlayerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PlayerScoreLabel
            // 
            this.PlayerScoreLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PlayerScoreLabel.AutoSize = true;
            this.PlayerScoreLabel.Location = new System.Drawing.Point(272, 75);
            this.PlayerScoreLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PlayerScoreLabel.Name = "PlayerScoreLabel";
            this.PlayerScoreLabel.Size = new System.Drawing.Size(119, 25);
            this.PlayerScoreLabel.TabIndex = 4;
            this.PlayerScoreLabel.Text = "PlayerScore";
            // 
            // OpponentNameLabel
            // 
            this.OpponentNameLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OpponentNameLabel.AutoSize = true;
            this.OpponentNameLabel.Location = new System.Drawing.Point(35, 251);
            this.OpponentNameLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OpponentNameLabel.Name = "OpponentNameLabel";
            this.OpponentNameLabel.Size = new System.Drawing.Size(151, 25);
            this.OpponentNameLabel.TabIndex = 5;
            this.OpponentNameLabel.Text = "OpponentName";
            // 
            // OpponentScoreLabel
            // 
            this.OpponentScoreLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.OpponentScoreLabel.AutoSize = true;
            this.OpponentScoreLabel.Location = new System.Drawing.Point(256, 251);
            this.OpponentScoreLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OpponentScoreLabel.Name = "OpponentScoreLabel";
            this.OpponentScoreLabel.Size = new System.Drawing.Size(151, 25);
            this.OpponentScoreLabel.TabIndex = 6;
            this.OpponentScoreLabel.Text = "OpponentScore";
            // 
            // WordTextbox
            // 
            this.WordTextbox.Location = new System.Drawing.Point(721, 537);
            this.WordTextbox.Margin = new System.Windows.Forms.Padding(4);
            this.WordTextbox.Name = "WordTextbox";
            this.WordTextbox.Size = new System.Drawing.Size(442, 29);
            this.WordTextbox.TabIndex = 7;
            this.WordTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.WordTextbox_KeyPress);
            // 
            // WordLabel
            // 
            this.WordLabel.AutoSize = true;
            this.WordLabel.Location = new System.Drawing.Point(584, 498);
            this.WordLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WordLabel.Name = "WordLabel";
            this.WordLabel.Size = new System.Drawing.Size(66, 25);
            this.WordLabel.TabIndex = 8;
            this.WordLabel.Text = "Word:";
            // 
            // CancelGameButton
            // 
            this.CancelGameButton.Location = new System.Drawing.Point(898, 696);
            this.CancelGameButton.Margin = new System.Windows.Forms.Padding(4);
            this.CancelGameButton.Name = "CancelGameButton";
            this.CancelGameButton.Size = new System.Drawing.Size(187, 63);
            this.CancelGameButton.TabIndex = 9;
            this.CancelGameButton.Text = "Cancel Game";
            this.CancelGameButton.UseVisualStyleBackColor = true;
            this.CancelGameButton.Click += new System.EventHandler(this.CancelGameButton_Click);
            // 
            // BoggleTable
            // 
            this.BoggleTable.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.BoggleTable.ColumnCount = 4;
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 168F));
            this.BoggleTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 188F));
            this.BoggleTable.Controls.Add(this.blabel16, 3, 3);
            this.BoggleTable.Controls.Add(this.blabel15, 2, 3);
            this.BoggleTable.Controls.Add(this.blabel14, 1, 3);
            this.BoggleTable.Controls.Add(this.blabel13, 0, 3);
            this.BoggleTable.Controls.Add(this.blabel12, 3, 2);
            this.BoggleTable.Controls.Add(this.blable4, 3, 0);
            this.BoggleTable.Controls.Add(this.blable1, 0, 0);
            this.BoggleTable.Controls.Add(this.blable2, 1, 0);
            this.BoggleTable.Controls.Add(this.blable7, 2, 1);
            this.BoggleTable.Controls.Add(this.blabel9, 0, 2);
            this.BoggleTable.Controls.Add(this.blabel11, 2, 2);
            this.BoggleTable.Controls.Add(this.blable5, 0, 1);
            this.BoggleTable.Controls.Add(this.blable6, 1, 1);
            this.BoggleTable.Controls.Add(this.blable18, 3, 1);
            this.BoggleTable.Controls.Add(this.blabel10, 1, 2);
            this.BoggleTable.Controls.Add(this.blable3, 2, 0);
            this.BoggleTable.GrowStyle = System.Windows.Forms.TableLayoutPanelGrowStyle.FixedSize;
            this.BoggleTable.Location = new System.Drawing.Point(9, 80);
            this.BoggleTable.Margin = new System.Windows.Forms.Padding(0);
            this.BoggleTable.Name = "BoggleTable";
            this.BoggleTable.RowCount = 4;
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.BoggleTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.BoggleTable.Size = new System.Drawing.Size(690, 718);
            this.BoggleTable.TabIndex = 10;
            // 
            // blable1
            // 
            this.blable1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blable1.AutoSize = true;
            this.blable1.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blable1.Location = new System.Drawing.Point(40, 48);
            this.blable1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blable1.Name = "blable1";
            this.blable1.Size = new System.Drawing.Size(87, 85);
            this.blable1.TabIndex = 0;
            this.blable1.Text = "A";
            // 
            // blable2
            // 
            this.blable2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blable2.AutoSize = true;
            this.blable2.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blable2.Location = new System.Drawing.Point(206, 48);
            this.blable2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blable2.Name = "blable2";
            this.blable2.Size = new System.Drawing.Size(87, 85);
            this.blable2.TabIndex = 1;
            this.blable2.Text = "B";
            // 
            // blable3
            // 
            this.blable3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blable3.AutoSize = true;
            this.blable3.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blable3.Location = new System.Drawing.Point(371, 48);
            this.blable3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blable3.Name = "blable3";
            this.blable3.Size = new System.Drawing.Size(91, 85);
            this.blable3.TabIndex = 2;
            this.blable3.Text = "C";
            // 
            // blable5
            // 
            this.blable5.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blable5.AutoSize = true;
            this.blable5.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blable5.Location = new System.Drawing.Point(40, 229);
            this.blable5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blable5.Name = "blable5";
            this.blable5.Size = new System.Drawing.Size(87, 85);
            this.blable5.TabIndex = 3;
            this.blable5.Text = "E";
            // 
            // blable6
            // 
            this.blable6.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blable6.AutoSize = true;
            this.blable6.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blable6.Location = new System.Drawing.Point(208, 229);
            this.blable6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blable6.Name = "blable6";
            this.blable6.Size = new System.Drawing.Size(83, 85);
            this.blable6.TabIndex = 4;
            this.blable6.Text = "F";
            // 
            // blable7
            // 
            this.blable7.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blable7.AutoSize = true;
            this.blable7.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blable7.Location = new System.Drawing.Point(369, 229);
            this.blable7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blable7.Name = "blable7";
            this.blable7.Size = new System.Drawing.Size(95, 85);
            this.blable7.TabIndex = 5;
            this.blable7.Text = "G";
            // 
            // blabel9
            // 
            this.blabel9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blabel9.AutoSize = true;
            this.blabel9.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blabel9.Location = new System.Drawing.Point(54, 410);
            this.blabel9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blabel9.Name = "blabel9";
            this.blabel9.Size = new System.Drawing.Size(58, 85);
            this.blabel9.TabIndex = 6;
            this.blabel9.Text = "I";
            // 
            // blabel10
            // 
            this.blabel10.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blabel10.AutoSize = true;
            this.blabel10.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blabel10.Location = new System.Drawing.Point(212, 410);
            this.blabel10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blabel10.Name = "blabel10";
            this.blabel10.Size = new System.Drawing.Size(75, 85);
            this.blabel10.TabIndex = 7;
            this.blabel10.Text = "J";
            // 
            // blabel11
            // 
            this.blabel11.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blabel11.AutoSize = true;
            this.blabel11.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blabel11.Location = new System.Drawing.Point(373, 410);
            this.blabel11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blabel11.Name = "blabel11";
            this.blabel11.Size = new System.Drawing.Size(87, 85);
            this.blabel11.TabIndex = 8;
            this.blabel11.Text = "K";
            // 
            // HelpSubmitLabel
            // 
            this.HelpSubmitLabel.AutoSize = true;
            this.HelpSubmitLabel.Location = new System.Drawing.Point(716, 589);
            this.HelpSubmitLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.HelpSubmitLabel.Name = "HelpSubmitLabel";
            this.HelpSubmitLabel.Size = new System.Drawing.Size(447, 25);
            this.HelpSubmitLabel.TabIndex = 11;
            this.HelpSubmitLabel.Text = "To submit a word, simply type it in and press enter.";
            // 
            // RemainingBar
            // 
            this.RemainingBar.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.RemainingBar.Location = new System.Drawing.Point(172, 42);
            this.RemainingBar.Margin = new System.Windows.Forms.Padding(4);
            this.RemainingBar.Name = "RemainingBar";
            this.RemainingBar.Size = new System.Drawing.Size(1090, 34);
            this.RemainingBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.RemainingBar.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.PlayerNameLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.PlayerScoreLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.OpponentNameLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.OpponentScoreLabel, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(721, 122);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(442, 352);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // blable4
            // 
            this.blable4.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blable4.AutoSize = true;
            this.blable4.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blable4.Location = new System.Drawing.Point(550, 48);
            this.blable4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blable4.Name = "blable4";
            this.blable4.Size = new System.Drawing.Size(91, 85);
            this.blable4.TabIndex = 9;
            this.blable4.Text = "D";
            // 
            // blable18
            // 
            this.blable18.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blable18.AutoSize = true;
            this.blable18.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blable18.Location = new System.Drawing.Point(550, 229);
            this.blable18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blable18.Name = "blable18";
            this.blable18.Size = new System.Drawing.Size(91, 85);
            this.blable18.TabIndex = 10;
            this.blable18.Text = "H";
            // 
            // blabel12
            // 
            this.blabel12.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blabel12.AutoSize = true;
            this.blabel12.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blabel12.Location = new System.Drawing.Point(556, 410);
            this.blabel12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blabel12.Name = "blabel12";
            this.blabel12.Size = new System.Drawing.Size(79, 85);
            this.blabel12.TabIndex = 11;
            this.blabel12.Text = "L";
            // 
            // blabel13
            // 
            this.blabel13.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blabel13.AutoSize = true;
            this.blabel13.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blabel13.Location = new System.Drawing.Point(34, 588);
            this.blabel13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blabel13.Name = "blabel13";
            this.blabel13.Size = new System.Drawing.Size(99, 85);
            this.blabel13.TabIndex = 12;
            this.blabel13.Text = "M";
            // 
            // blabel14
            // 
            this.blabel14.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blabel14.AutoSize = true;
            this.blabel14.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blabel14.Location = new System.Drawing.Point(204, 588);
            this.blabel14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blabel14.Name = "blabel14";
            this.blabel14.Size = new System.Drawing.Size(91, 85);
            this.blabel14.TabIndex = 13;
            this.blabel14.Text = "N";
            // 
            // blabel15
            // 
            this.blabel15.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blabel15.AutoSize = true;
            this.blabel15.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blabel15.Location = new System.Drawing.Point(369, 588);
            this.blabel15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blabel15.Name = "blabel15";
            this.blabel15.Size = new System.Drawing.Size(95, 85);
            this.blabel15.TabIndex = 14;
            this.blabel15.Text = "O";
            // 
            // blabel16
            // 
            this.blabel16.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.blabel16.AutoSize = true;
            this.blabel16.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.blabel16.Location = new System.Drawing.Point(552, 588);
            this.blabel16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.blabel16.Name = "blabel16";
            this.blabel16.Size = new System.Drawing.Size(87, 85);
            this.blabel16.TabIndex = 15;
            this.blabel16.Text = "P";
            // 
            // GameView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1275, 871);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.HelpSubmitLabel);
            this.Controls.Add(this.BoggleTable);
            this.Controls.Add(this.CancelGameButton);
            this.Controls.Add(this.WordLabel);
            this.Controls.Add(this.WordTextbox);
            this.Controls.Add(this.RemainingDataLabel);
            this.Controls.Add(this.RemainingSideLabel);
            this.Controls.Add(this.RemainingBar);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GameView";
            this.Text = "BoggleClient";
            this.BoggleTable.ResumeLayout(false);
            this.BoggleTable.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
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
        private System.Windows.Forms.Label blable1;
        private System.Windows.Forms.Label blable2;
        private System.Windows.Forms.Label blable3;
        private System.Windows.Forms.Label blable5;
        private System.Windows.Forms.Label blable6;
        private System.Windows.Forms.Label blable7;
        private System.Windows.Forms.Label blabel9;
        private System.Windows.Forms.Label blabel10;
        private System.Windows.Forms.Label blabel11;
        private System.Windows.Forms.Label HelpSubmitLabel;
        private System.Windows.Forms.ProgressBar RemainingBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label blabel12;
        private System.Windows.Forms.Label blable18;
        private System.Windows.Forms.Label blable4;
        private System.Windows.Forms.Label blabel16;
        private System.Windows.Forms.Label blabel15;
        private System.Windows.Forms.Label blabel14;
        private System.Windows.Forms.Label blabel13;
    }
}