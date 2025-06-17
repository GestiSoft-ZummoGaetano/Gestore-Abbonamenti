namespace GestisoftInstallerAbbonamenti
{
    partial class GetVredencial
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
            button1 = new Button();
            cmbUser = new TextBox();
            label1 = new Label();
            label2 = new Label();
            cmbPwd = new TextBox();
            label3 = new Label();
            cmdComune = new TextBox();
            label4 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(287, 153);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(114, 47);
            button1.TabIndex = 0;
            button1.Text = "Aggiungi";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // cmbUser
            // 
            cmbUser.Location = new Point(91, 87);
            cmbUser.Margin = new Padding(3, 4, 3, 4);
            cmbUser.Name = "cmbUser";
            cmbUser.Size = new Size(114, 27);
            cmbUser.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 91);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 2;
            label1.Text = "UserName";
            label1.Click += label1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(249, 91);
            label2.Name = "label2";
            label2.Size = new Size(70, 20);
            label2.TabIndex = 4;
            label2.Text = "Password";
            label2.Click += label2_Click;
            // 
            // cmbPwd
            // 
            cmbPwd.Location = new Point(327, 87);
            cmbPwd.Margin = new Padding(3, 4, 3, 4);
            cmbPwd.Name = "cmbPwd";
            cmbPwd.PasswordChar = '*';
            cmbPwd.Size = new Size(114, 27);
            cmbPwd.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 167);
            label3.Name = "label3";
            label3.Size = new Size(139, 20);
            label3.TabIndex = 6;
            label3.Text = "Comune Da Inserire";
            label3.Click += label3_Click;
            // 
            // cmdComune
            // 
            cmdComune.Location = new Point(153, 163);
            cmdComune.Margin = new Padding(3, 4, 3, 4);
            cmdComune.Name = "cmdComune";
            cmdComune.Size = new Size(114, 27);
            cmdComune.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(165, 12);
            label4.Name = "label4";
            label4.Size = new Size(130, 20);
            label4.TabIndex = 7;
            label4.Text = "AUTENTICAZIONE";
            // 
            // GetVredencial
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(465, 260);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(cmdComune);
            Controls.Add(label2);
            Controls.Add(cmbPwd);
            Controls.Add(label1);
            Controls.Add(cmbUser);
            Controls.Add(button1);
            Margin = new Padding(3, 4, 3, 4);
            Name = "GetVredencial";
            Text = "Autenticazione";
            FormClosing += GetVredencial_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox cmbUser;
        private Label label1;
        private Label label2;
        private TextBox cmbPwd;
        private Label label3;
        private TextBox cmdComune;
        private Label label4;
    }
}