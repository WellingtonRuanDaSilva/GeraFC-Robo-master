namespace VASS06_GeraFC_Robo
{
    partial class FormLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lbl_Usuario = new System.Windows.Forms.Label();
            this.lbl_Senha = new System.Windows.Forms.Label();
            this.txb_Usuario = new System.Windows.Forms.TextBox();
            this.txb_Senha = new System.Windows.Forms.TextBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.btn_Fechar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(211, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 24);
            this.label2.TabIndex = 17;
            this.label2.Text = "VASS 06";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(128, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 24);
            this.label1.TabIndex = 16;
            this.label1.Text = "Gerador FC Robo";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(291, 113);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 15;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // lbl_Usuario
            // 
            this.lbl_Usuario.AutoSize = true;
            this.lbl_Usuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Usuario.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Usuario.Location = new System.Drawing.Point(24, 155);
            this.lbl_Usuario.Name = "lbl_Usuario";
            this.lbl_Usuario.Size = new System.Drawing.Size(65, 16);
            this.lbl_Usuario.TabIndex = 19;
            this.lbl_Usuario.Text = "Usuário:";
            // 
            // lbl_Senha
            // 
            this.lbl_Senha.AutoSize = true;
            this.lbl_Senha.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Senha.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Senha.Location = new System.Drawing.Point(24, 192);
            this.lbl_Senha.Name = "lbl_Senha";
            this.lbl_Senha.Size = new System.Drawing.Size(55, 16);
            this.lbl_Senha.TabIndex = 18;
            this.lbl_Senha.Text = "Senha:";
            // 
            // txb_Usuario
            // 
            this.txb_Usuario.BackColor = System.Drawing.SystemColors.GrayText;
            this.txb_Usuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_Usuario.ForeColor = System.Drawing.SystemColors.Window;
            this.txb_Usuario.Location = new System.Drawing.Point(95, 155);
            this.txb_Usuario.Name = "txb_Usuario";
            this.txb_Usuario.Size = new System.Drawing.Size(186, 22);
            this.txb_Usuario.TabIndex = 20;
            this.txb_Usuario.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Press_Enter);
            // 
            // txb_Senha
            // 
            this.txb_Senha.BackColor = System.Drawing.SystemColors.GrayText;
            this.txb_Senha.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_Senha.ForeColor = System.Drawing.SystemColors.Window;
            this.txb_Senha.Location = new System.Drawing.Point(95, 192);
            this.txb_Senha.Name = "txb_Senha";
            this.txb_Senha.PasswordChar = '•';
            this.txb_Senha.Size = new System.Drawing.Size(186, 22);
            this.txb_Senha.TabIndex = 21;
            this.txb_Senha.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Press_Enter);
            // 
            // btn_Login
            // 
            this.btn_Login.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btn_Login.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Login.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Login.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_Login.Location = new System.Drawing.Point(27, 243);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(118, 40);
            this.btn_Login.TabIndex = 22;
            this.btn_Login.Text = "Login";
            this.btn_Login.UseVisualStyleBackColor = false;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // btn_Fechar
            // 
            this.btn_Fechar.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btn_Fechar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_Fechar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Fechar.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_Fechar.Location = new System.Drawing.Point(163, 243);
            this.btn_Fechar.Name = "btn_Fechar";
            this.btn_Fechar.Size = new System.Drawing.Size(118, 40);
            this.btn_Fechar.TabIndex = 23;
            this.btn_Fechar.Text = "Fechar";
            this.btn_Fechar.UseVisualStyleBackColor = false;
            this.btn_Fechar.Click += new System.EventHandler(this.btn_Fechar_Click);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(307, 295);
            this.Controls.Add(this.btn_Fechar);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.txb_Senha);
            this.Controls.Add(this.txb_Usuario);
            this.Controls.Add(this.lbl_Usuario);
            this.Controls.Add(this.lbl_Senha);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormLogin";
            this.Text = "InfoRMI - Gerador FC Robo - VASS6 TIA Portal";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lbl_Usuario;
        private System.Windows.Forms.Label lbl_Senha;
        private System.Windows.Forms.TextBox txb_Usuario;
        private System.Windows.Forms.TextBox txb_Senha;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.Button btn_Fechar;
    }
}