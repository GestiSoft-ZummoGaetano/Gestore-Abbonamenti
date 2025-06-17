namespace GestisoftInstallerAbbonamenti
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            progressBar1 = new ProgressBar();
            label1 = new Label();
            textInfo = new TextBox();
            btnFine = new Button();
            btnCacella = new Button();
            btnStart = new Button();
            SuspendLayout();
            // 
            // progressBar1
            // 
            progressBar1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            progressBar1.ForeColor = Color.FromArgb(128, 255, 128);
            progressBar1.Location = new Point(2, 52);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(794, 10);
            progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(2, 13);
            label1.Name = "label1";
            label1.Size = new Size(181, 18);
            label1.TabIndex = 1;
            label1.Text = "STATO INSTALLAZIONE";
            // 
            // textInfo
            // 
            textInfo.Location = new Point(11, 100);
            textInfo.Margin = new Padding(20);
            textInfo.Multiline = true;
            textInfo.Name = "textInfo";
            textInfo.ReadOnly = true;
            textInfo.ScrollBars = ScrollBars.Vertical;
            textInfo.Size = new Size(777, 275);
            textInfo.TabIndex = 2;
            textInfo.Text = "Starting...\r\ninstallation start...";
            // 
            // btnFine
            // 
            btnFine.Location = new Point(703, 415);
            btnFine.Name = "btnFine";
            btnFine.Size = new Size(85, 32);
            btnFine.TabIndex = 3;
            btnFine.Text = "FINE";
            btnFine.UseVisualStyleBackColor = true;
            btnFine.Visible = false;
            btnFine.Click += btnFine_Click;
            // 
            // btnCacella
            // 
            btnCacella.Location = new Point(703, 415);
            btnCacella.Name = "btnCacella";
            btnCacella.Size = new Size(85, 32);
            btnCacella.TabIndex = 4;
            btnCacella.Text = "CANCELLA";
            btnCacella.UseVisualStyleBackColor = true;
            btnCacella.Visible = false;
            btnCacella.Click += btnCacella_Click;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(703, 415);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(85, 32);
            btnStart.TabIndex = 5;
            btnStart.Text = "START";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(800, 459);
            Controls.Add(btnStart);
            Controls.Add(btnCacella);
            Controls.Add(btnFine);
            Controls.Add(textInfo);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Name = "Form1";
            Text = "Installer - Gestione Abbonamenti";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ProgressBar progressBar1;
        private Label label1;
        private TextBox textInfo;
        private Button btnFine;
        private Button btnCacella;
        private Button btnStart;
    }
}
