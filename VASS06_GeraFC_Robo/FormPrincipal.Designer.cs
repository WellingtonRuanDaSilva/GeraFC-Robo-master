namespace VASS06_GeraFC_Robo
{
    partial class FormPrincipal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrincipal));
            this.btn_AbrirPlanilha = new System.Windows.Forms.Button();
            this.ltb_Robos = new System.Windows.Forms.ListBox();
            this.lbl_Grupo = new System.Windows.Forms.Label();
            this.lbl_Estacao = new System.Windows.Forms.Label();
            this.txb_Estacao = new System.Windows.Forms.TextBox();
            this.txb_Grupo = new System.Windows.Forms.TextBox();
            this.lbl_Robo = new System.Windows.Forms.Label();
            this.dgv_Segurança = new System.Windows.Forms.DataGridView();
            this.dgv_Interlocks = new System.Windows.Forms.DataGridView();
            this.dgv_FMs = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_GerarArquivos = new System.Windows.Forms.Button();
            this.dgv_Folges = new System.Windows.Forms.DataGridView();
            this.lbl_Segurança = new System.Windows.Forms.Label();
            this.lbl_Interlocks = new System.Windows.Forms.Label();
            this.lbl_FMs = new System.Windows.Forms.Label();
            this.lbl_Folges = new System.Windows.Forms.Label();
            this.txb_Robo = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_Ranges = new System.Windows.Forms.Label();
            this.lbl_Usuario = new System.Windows.Forms.Label();
            this.txb_DBUsuario = new System.Windows.Forms.TextBox();
            this.txb_DBInstancia = new System.Windows.Forms.TextBox();
            this.lbl_Instancia = new System.Windows.Forms.Label();
            this.lbl_NumeroFC = new System.Windows.Forms.Label();
            this.txb_NumeroFC = new System.Windows.Forms.TextBox();
            this.lbl_Entradas = new System.Windows.Forms.Label();
            this.dgv_Entradas = new System.Windows.Forms.DataGridView();
            this.lbl_Saidas = new System.Windows.Forms.Label();
            this.dgv_Saidas = new System.Windows.Forms.DataGridView();
            this.lbl_Ferramentas = new System.Windows.Forms.Label();
            this.dgv_Ferramentas = new System.Windows.Forms.DataGridView();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Segurança)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Interlocks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Folges)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Entradas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Saidas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Ferramentas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_AbrirPlanilha
            // 
            this.btn_AbrirPlanilha.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btn_AbrirPlanilha.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_AbrirPlanilha.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_AbrirPlanilha.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_AbrirPlanilha.Location = new System.Drawing.Point(7, 131);
            this.btn_AbrirPlanilha.Name = "btn_AbrirPlanilha";
            this.btn_AbrirPlanilha.Size = new System.Drawing.Size(254, 52);
            this.btn_AbrirPlanilha.TabIndex = 0;
            this.btn_AbrirPlanilha.Text = "Abrir Planilha";
            this.btn_AbrirPlanilha.UseVisualStyleBackColor = false;
            this.btn_AbrirPlanilha.Click += new System.EventHandler(this.btn_AbrirPlanilha_Click);
            // 
            // ltb_Robos
            // 
            this.ltb_Robos.BackColor = System.Drawing.SystemColors.GrayText;
            this.ltb_Robos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ltb_Robos.ForeColor = System.Drawing.SystemColors.Window;
            this.ltb_Robos.FormattingEnabled = true;
            this.ltb_Robos.ItemHeight = 16;
            this.ltb_Robos.Location = new System.Drawing.Point(8, 189);
            this.ltb_Robos.Name = "ltb_Robos";
            this.ltb_Robos.Size = new System.Drawing.Size(253, 164);
            this.ltb_Robos.TabIndex = 2;
            this.ltb_Robos.SelectedIndexChanged += new System.EventHandler(this.ltb_Robos_SelectedIndexChanged);
            // 
            // lbl_Grupo
            // 
            this.lbl_Grupo.AutoSize = true;
            this.lbl_Grupo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Grupo.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Grupo.Location = new System.Drawing.Point(13, 397);
            this.lbl_Grupo.Name = "lbl_Grupo";
            this.lbl_Grupo.Size = new System.Drawing.Size(53, 16);
            this.lbl_Grupo.TabIndex = 3;
            this.lbl_Grupo.Text = "Grupo:";
            // 
            // lbl_Estacao
            // 
            this.lbl_Estacao.AutoSize = true;
            this.lbl_Estacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Estacao.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Estacao.Location = new System.Drawing.Point(13, 371);
            this.lbl_Estacao.Name = "lbl_Estacao";
            this.lbl_Estacao.Size = new System.Drawing.Size(68, 16);
            this.lbl_Estacao.TabIndex = 4;
            this.lbl_Estacao.Text = "Estação:";
            // 
            // txb_Estacao
            // 
            this.txb_Estacao.BackColor = System.Drawing.SystemColors.GrayText;
            this.txb_Estacao.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_Estacao.ForeColor = System.Drawing.SystemColors.Window;
            this.txb_Estacao.Location = new System.Drawing.Point(87, 370);
            this.txb_Estacao.Name = "txb_Estacao";
            this.txb_Estacao.ReadOnly = true;
            this.txb_Estacao.Size = new System.Drawing.Size(105, 22);
            this.txb_Estacao.TabIndex = 5;
            // 
            // txb_Grupo
            // 
            this.txb_Grupo.BackColor = System.Drawing.SystemColors.GrayText;
            this.txb_Grupo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_Grupo.ForeColor = System.Drawing.SystemColors.Window;
            this.txb_Grupo.Location = new System.Drawing.Point(87, 396);
            this.txb_Grupo.Name = "txb_Grupo";
            this.txb_Grupo.ReadOnly = true;
            this.txb_Grupo.Size = new System.Drawing.Size(47, 22);
            this.txb_Grupo.TabIndex = 6;
            // 
            // lbl_Robo
            // 
            this.lbl_Robo.AutoSize = true;
            this.lbl_Robo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Robo.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Robo.Location = new System.Drawing.Point(13, 426);
            this.lbl_Robo.Name = "lbl_Robo";
            this.lbl_Robo.Size = new System.Drawing.Size(49, 16);
            this.lbl_Robo.TabIndex = 7;
            this.lbl_Robo.Text = "Robo:";
            // 
            // dgv_Segurança
            // 
            this.dgv_Segurança.AllowUserToAddRows = false;
            this.dgv_Segurança.AllowUserToDeleteRows = false;
            this.dgv_Segurança.AllowUserToResizeColumns = false;
            this.dgv_Segurança.AllowUserToResizeRows = false;
            this.dgv_Segurança.BackgroundColor = System.Drawing.SystemColors.GrayText;
            this.dgv_Segurança.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Segurança.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Segurança.EnableHeadersVisualStyles = false;
            this.dgv_Segurança.GridColor = System.Drawing.SystemColors.Window;
            this.dgv_Segurança.Location = new System.Drawing.Point(305, 106);
            this.dgv_Segurança.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_Segurança.Name = "dgv_Segurança";
            this.dgv_Segurança.ReadOnly = true;
            this.dgv_Segurança.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_Segurança.RowHeadersVisible = false;
            this.dgv_Segurança.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_Segurança.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_Segurança.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Segurança.Size = new System.Drawing.Size(80, 165);
            this.dgv_Segurança.TabIndex = 9;
            // 
            // dgv_Interlocks
            // 
            this.dgv_Interlocks.AllowUserToAddRows = false;
            this.dgv_Interlocks.AllowUserToDeleteRows = false;
            this.dgv_Interlocks.AllowUserToResizeColumns = false;
            this.dgv_Interlocks.AllowUserToResizeRows = false;
            this.dgv_Interlocks.BackgroundColor = System.Drawing.SystemColors.GrayText;
            this.dgv_Interlocks.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Interlocks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Interlocks.EnableHeadersVisualStyles = false;
            this.dgv_Interlocks.GridColor = System.Drawing.SystemColors.Window;
            this.dgv_Interlocks.Location = new System.Drawing.Point(411, 106);
            this.dgv_Interlocks.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_Interlocks.Name = "dgv_Interlocks";
            this.dgv_Interlocks.ReadOnly = true;
            this.dgv_Interlocks.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_Interlocks.RowHeadersVisible = false;
            this.dgv_Interlocks.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_Interlocks.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dgv_Interlocks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Interlocks.Size = new System.Drawing.Size(158, 374);
            this.dgv_Interlocks.TabIndex = 10;
            // 
            // dgv_FMs
            // 
            this.dgv_FMs.AllowUserToAddRows = false;
            this.dgv_FMs.AllowUserToDeleteRows = false;
            this.dgv_FMs.AllowUserToResizeColumns = false;
            this.dgv_FMs.AllowUserToResizeRows = false;
            this.dgv_FMs.BackgroundColor = System.Drawing.SystemColors.GrayText;
            this.dgv_FMs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_FMs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_FMs.EnableHeadersVisualStyles = false;
            this.dgv_FMs.GridColor = System.Drawing.SystemColors.Window;
            this.dgv_FMs.Location = new System.Drawing.Point(585, 106);
            this.dgv_FMs.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_FMs.MaximumSize = new System.Drawing.Size(180, 180);
            this.dgv_FMs.Name = "dgv_FMs";
            this.dgv_FMs.ReadOnly = true;
            this.dgv_FMs.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_FMs.RowHeadersVisible = false;
            this.dgv_FMs.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_FMs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_FMs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_FMs.Size = new System.Drawing.Size(140, 180);
            this.dgv_FMs.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Window;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(78, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(175, 24);
            this.label1.TabIndex = 13;
            this.label1.Text = "Gerador FC Robo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(161, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 24);
            this.label2.TabIndex = 14;
            this.label2.Text = "VASS 06";
            // 
            // btn_GerarArquivos
            // 
            this.btn_GerarArquivos.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.btn_GerarArquivos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_GerarArquivos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_GerarArquivos.ForeColor = System.Drawing.SystemColors.Window;
            this.btn_GerarArquivos.Location = new System.Drawing.Point(7, 470);
            this.btn_GerarArquivos.Name = "btn_GerarArquivos";
            this.btn_GerarArquivos.Size = new System.Drawing.Size(254, 52);
            this.btn_GerarArquivos.TabIndex = 15;
            this.btn_GerarArquivos.Text = "Gerar Arquivos";
            this.btn_GerarArquivos.UseVisualStyleBackColor = false;
            this.btn_GerarArquivos.Click += new System.EventHandler(this.btn_GerarArquivos_Click);
            // 
            // dgv_Folges
            // 
            this.dgv_Folges.AllowUserToAddRows = false;
            this.dgv_Folges.AllowUserToDeleteRows = false;
            this.dgv_Folges.AllowUserToResizeColumns = false;
            this.dgv_Folges.AllowUserToResizeRows = false;
            this.dgv_Folges.BackgroundColor = System.Drawing.SystemColors.GrayText;
            this.dgv_Folges.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Folges.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Folges.EnableHeadersVisualStyles = false;
            this.dgv_Folges.GridColor = System.Drawing.SystemColors.Window;
            this.dgv_Folges.Location = new System.Drawing.Point(585, 316);
            this.dgv_Folges.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_Folges.MaximumSize = new System.Drawing.Size(180, 164);
            this.dgv_Folges.Name = "dgv_Folges";
            this.dgv_Folges.ReadOnly = true;
            this.dgv_Folges.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_Folges.RowHeadersVisible = false;
            this.dgv_Folges.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_Folges.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_Folges.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Folges.Size = new System.Drawing.Size(140, 164);
            this.dgv_Folges.TabIndex = 16;
            // 
            // lbl_Segurança
            // 
            this.lbl_Segurança.AutoSize = true;
            this.lbl_Segurança.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Segurança.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Segurança.Location = new System.Drawing.Point(302, 90);
            this.lbl_Segurança.Name = "lbl_Segurança";
            this.lbl_Segurança.Size = new System.Drawing.Size(82, 16);
            this.lbl_Segurança.TabIndex = 18;
            this.lbl_Segurança.Text = "Segurança";
            // 
            // lbl_Interlocks
            // 
            this.lbl_Interlocks.AutoSize = true;
            this.lbl_Interlocks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Interlocks.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Interlocks.Location = new System.Drawing.Point(408, 90);
            this.lbl_Interlocks.Name = "lbl_Interlocks";
            this.lbl_Interlocks.Size = new System.Drawing.Size(74, 16);
            this.lbl_Interlocks.TabIndex = 19;
            this.lbl_Interlocks.Text = "Interlocks";
            // 
            // lbl_FMs
            // 
            this.lbl_FMs.AutoSize = true;
            this.lbl_FMs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_FMs.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_FMs.Location = new System.Drawing.Point(582, 90);
            this.lbl_FMs.Name = "lbl_FMs";
            this.lbl_FMs.Size = new System.Drawing.Size(125, 16);
            this.lbl_FMs.TabIndex = 20;
            this.lbl_FMs.Text = "Fins de Trabalho";
            // 
            // lbl_Folges
            // 
            this.lbl_Folges.AutoSize = true;
            this.lbl_Folges.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Folges.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Folges.Location = new System.Drawing.Point(590, 300);
            this.lbl_Folges.Name = "lbl_Folges";
            this.lbl_Folges.Size = new System.Drawing.Size(55, 16);
            this.lbl_Folges.TabIndex = 21;
            this.lbl_Folges.Text = "Folges";
            // 
            // txb_Robo
            // 
            this.txb_Robo.BackColor = System.Drawing.SystemColors.GrayText;
            this.txb_Robo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_Robo.ForeColor = System.Drawing.SystemColors.Window;
            this.txb_Robo.Location = new System.Drawing.Point(87, 423);
            this.txb_Robo.Name = "txb_Robo";
            this.txb_Robo.ReadOnly = true;
            this.txb_Robo.Size = new System.Drawing.Size(47, 22);
            this.txb_Robo.TabIndex = 24;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(278, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2, 510);
            this.panel1.TabIndex = 25;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.panel2.Location = new System.Drawing.Point(280, 63);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1025, 2);
            this.panel2.TabIndex = 26;
            // 
            // lbl_Ranges
            // 
            this.lbl_Ranges.AutoSize = true;
            this.lbl_Ranges.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Ranges.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Ranges.Location = new System.Drawing.Point(302, 12);
            this.lbl_Ranges.Name = "lbl_Ranges";
            this.lbl_Ranges.Size = new System.Drawing.Size(61, 16);
            this.lbl_Ranges.TabIndex = 27;
            this.lbl_Ranges.Text = "Ranges";
            // 
            // lbl_Usuario
            // 
            this.lbl_Usuario.AutoSize = true;
            this.lbl_Usuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Usuario.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Usuario.Location = new System.Drawing.Point(302, 38);
            this.lbl_Usuario.Name = "lbl_Usuario";
            this.lbl_Usuario.Size = new System.Drawing.Size(90, 16);
            this.lbl_Usuario.TabIndex = 28;
            this.lbl_Usuario.Text = "DB Usuário:";
            // 
            // txb_DBUsuario
            // 
            this.txb_DBUsuario.BackColor = System.Drawing.SystemColors.GrayText;
            this.txb_DBUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_DBUsuario.ForeColor = System.Drawing.SystemColors.Window;
            this.txb_DBUsuario.Location = new System.Drawing.Point(398, 35);
            this.txb_DBUsuario.Name = "txb_DBUsuario";
            this.txb_DBUsuario.Size = new System.Drawing.Size(47, 22);
            this.txb_DBUsuario.TabIndex = 29;
            this.txb_DBUsuario.Text = "10";
            this.txb_DBUsuario.TextChanged += new System.EventHandler(this.txbDBUsuario_TextChanged);
            // 
            // txb_DBInstancia
            // 
            this.txb_DBInstancia.BackColor = System.Drawing.SystemColors.GrayText;
            this.txb_DBInstancia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_DBInstancia.ForeColor = System.Drawing.SystemColors.Window;
            this.txb_DBInstancia.Location = new System.Drawing.Point(565, 35);
            this.txb_DBInstancia.Name = "txb_DBInstancia";
            this.txb_DBInstancia.Size = new System.Drawing.Size(47, 22);
            this.txb_DBInstancia.TabIndex = 31;
            this.txb_DBInstancia.Text = "2000";
            this.txb_DBInstancia.TextChanged += new System.EventHandler(this.txbDBInstancia_TextChanged);
            // 
            // lbl_Instancia
            // 
            this.lbl_Instancia.AutoSize = true;
            this.lbl_Instancia.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Instancia.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Instancia.Location = new System.Drawing.Point(469, 38);
            this.lbl_Instancia.Name = "lbl_Instancia";
            this.lbl_Instancia.Size = new System.Drawing.Size(98, 16);
            this.lbl_Instancia.TabIndex = 30;
            this.lbl_Instancia.Text = "DB Instância:";
            // 
            // lbl_NumeroFC
            // 
            this.lbl_NumeroFC.AutoSize = true;
            this.lbl_NumeroFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_NumeroFC.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_NumeroFC.Location = new System.Drawing.Point(647, 38);
            this.lbl_NumeroFC.Name = "lbl_NumeroFC";
            this.lbl_NumeroFC.Size = new System.Drawing.Size(88, 16);
            this.lbl_NumeroFC.TabIndex = 32;
            this.lbl_NumeroFC.Text = "Número FC:";
            // 
            // txb_NumeroFC
            // 
            this.txb_NumeroFC.BackColor = System.Drawing.SystemColors.GrayText;
            this.txb_NumeroFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txb_NumeroFC.ForeColor = System.Drawing.SystemColors.Window;
            this.txb_NumeroFC.Location = new System.Drawing.Point(735, 35);
            this.txb_NumeroFC.Name = "txb_NumeroFC";
            this.txb_NumeroFC.Size = new System.Drawing.Size(47, 22);
            this.txb_NumeroFC.TabIndex = 33;
            this.txb_NumeroFC.Text = "100";
            this.txb_NumeroFC.TextChanged += new System.EventHandler(this.txbNumeroFC_TextChanged);
            // 
            // lbl_Entradas
            // 
            this.lbl_Entradas.AutoSize = true;
            this.lbl_Entradas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Entradas.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Entradas.Location = new System.Drawing.Point(757, 90);
            this.lbl_Entradas.Name = "lbl_Entradas";
            this.lbl_Entradas.Size = new System.Drawing.Size(69, 16);
            this.lbl_Entradas.TabIndex = 35;
            this.lbl_Entradas.Text = "Entradas";
            // 
            // dgv_Entradas
            // 
            this.dgv_Entradas.AllowUserToAddRows = false;
            this.dgv_Entradas.AllowUserToDeleteRows = false;
            this.dgv_Entradas.AllowUserToResizeColumns = false;
            this.dgv_Entradas.AllowUserToResizeRows = false;
            this.dgv_Entradas.BackgroundColor = System.Drawing.SystemColors.GrayText;
            this.dgv_Entradas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Entradas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Entradas.EnableHeadersVisualStyles = false;
            this.dgv_Entradas.GridColor = System.Drawing.SystemColors.Window;
            this.dgv_Entradas.Location = new System.Drawing.Point(758, 106);
            this.dgv_Entradas.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_Entradas.MaximumSize = new System.Drawing.Size(290, 372);
            this.dgv_Entradas.Name = "dgv_Entradas";
            this.dgv_Entradas.ReadOnly = true;
            this.dgv_Entradas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_Entradas.RowHeadersVisible = false;
            this.dgv_Entradas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_Entradas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_Entradas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Entradas.Size = new System.Drawing.Size(250, 372);
            this.dgv_Entradas.TabIndex = 34;
            // 
            // lbl_Saidas
            // 
            this.lbl_Saidas.AutoSize = true;
            this.lbl_Saidas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Saidas.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Saidas.Location = new System.Drawing.Point(1035, 90);
            this.lbl_Saidas.Name = "lbl_Saidas";
            this.lbl_Saidas.Size = new System.Drawing.Size(56, 16);
            this.lbl_Saidas.TabIndex = 37;
            this.lbl_Saidas.Text = "Saídas";
            // 
            // dgv_Saidas
            // 
            this.dgv_Saidas.AllowUserToAddRows = false;
            this.dgv_Saidas.AllowUserToDeleteRows = false;
            this.dgv_Saidas.AllowUserToResizeColumns = false;
            this.dgv_Saidas.AllowUserToResizeRows = false;
            this.dgv_Saidas.BackgroundColor = System.Drawing.SystemColors.GrayText;
            this.dgv_Saidas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Saidas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Saidas.EnableHeadersVisualStyles = false;
            this.dgv_Saidas.GridColor = System.Drawing.SystemColors.Window;
            this.dgv_Saidas.Location = new System.Drawing.Point(1036, 106);
            this.dgv_Saidas.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_Saidas.MaximumSize = new System.Drawing.Size(290, 372);
            this.dgv_Saidas.Name = "dgv_Saidas";
            this.dgv_Saidas.ReadOnly = true;
            this.dgv_Saidas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_Saidas.RowHeadersVisible = false;
            this.dgv_Saidas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_Saidas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_Saidas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Saidas.Size = new System.Drawing.Size(250, 372);
            this.dgv_Saidas.TabIndex = 36;
            // 
            // lbl_Ferramentas
            // 
            this.lbl_Ferramentas.AutoSize = true;
            this.lbl_Ferramentas.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_Ferramentas.ForeColor = System.Drawing.SystemColors.Window;
            this.lbl_Ferramentas.Location = new System.Drawing.Point(302, 284);
            this.lbl_Ferramentas.Name = "lbl_Ferramentas";
            this.lbl_Ferramentas.Size = new System.Drawing.Size(94, 16);
            this.lbl_Ferramentas.TabIndex = 39;
            this.lbl_Ferramentas.Text = "Ferramentas";
            // 
            // dgv_Ferramentas
            // 
            this.dgv_Ferramentas.AllowUserToAddRows = false;
            this.dgv_Ferramentas.AllowUserToDeleteRows = false;
            this.dgv_Ferramentas.AllowUserToResizeColumns = false;
            this.dgv_Ferramentas.AllowUserToResizeRows = false;
            this.dgv_Ferramentas.BackgroundColor = System.Drawing.SystemColors.GrayText;
            this.dgv_Ferramentas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgv_Ferramentas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Ferramentas.EnableHeadersVisualStyles = false;
            this.dgv_Ferramentas.GridColor = System.Drawing.SystemColors.Window;
            this.dgv_Ferramentas.Location = new System.Drawing.Point(305, 300);
            this.dgv_Ferramentas.Margin = new System.Windows.Forms.Padding(0);
            this.dgv_Ferramentas.Name = "dgv_Ferramentas";
            this.dgv_Ferramentas.ReadOnly = true;
            this.dgv_Ferramentas.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgv_Ferramentas.RowHeadersVisible = false;
            this.dgv_Ferramentas.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgv_Ferramentas.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgv_Ferramentas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgv_Ferramentas.Size = new System.Drawing.Size(80, 178);
            this.dgv_Ferramentas.TabIndex = 38;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(16, 22);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(245, 103);
            this.pictureBox1.TabIndex = 40;
            this.pictureBox1.TabStop = false;
            // 
            // FormPrincipal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSlateGray;
            this.ClientSize = new System.Drawing.Size(1319, 534);
            this.Controls.Add(this.lbl_Ferramentas);
            this.Controls.Add(this.dgv_Ferramentas);
            this.Controls.Add(this.lbl_Saidas);
            this.Controls.Add(this.dgv_Saidas);
            this.Controls.Add(this.lbl_Entradas);
            this.Controls.Add(this.dgv_Entradas);
            this.Controls.Add(this.txb_NumeroFC);
            this.Controls.Add(this.lbl_NumeroFC);
            this.Controls.Add(this.txb_DBInstancia);
            this.Controls.Add(this.lbl_Instancia);
            this.Controls.Add(this.txb_DBUsuario);
            this.Controls.Add(this.lbl_Usuario);
            this.Controls.Add(this.lbl_Ranges);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txb_Robo);
            this.Controls.Add(this.lbl_Folges);
            this.Controls.Add(this.lbl_FMs);
            this.Controls.Add(this.lbl_Interlocks);
            this.Controls.Add(this.lbl_Segurança);
            this.Controls.Add(this.dgv_Folges);
            this.Controls.Add(this.btn_GerarArquivos);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgv_FMs);
            this.Controls.Add(this.dgv_Interlocks);
            this.Controls.Add(this.dgv_Segurança);
            this.Controls.Add(this.lbl_Robo);
            this.Controls.Add(this.txb_Grupo);
            this.Controls.Add(this.txb_Estacao);
            this.Controls.Add(this.lbl_Estacao);
            this.Controls.Add(this.lbl_Grupo);
            this.Controls.Add(this.ltb_Robos);
            this.Controls.Add(this.btn_AbrirPlanilha);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormPrincipal";
            this.Text = "InfoRMI - Gerador FC Robo - VASS6 TIA Portal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormPrincipal_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Segurança)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Interlocks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_FMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Folges)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Entradas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Saidas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Ferramentas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_AbrirPlanilha;
        private System.Windows.Forms.ListBox ltb_Robos;
        private System.Windows.Forms.Label lbl_Grupo;
        private System.Windows.Forms.Label lbl_Estacao;
        private System.Windows.Forms.TextBox txb_Estacao;
        private System.Windows.Forms.TextBox txb_Grupo;
        private System.Windows.Forms.Label lbl_Robo;
        private System.Windows.Forms.DataGridView dgv_Segurança;
        private System.Windows.Forms.DataGridView dgv_Interlocks;
        private System.Windows.Forms.DataGridView dgv_FMs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_GerarArquivos;
        private System.Windows.Forms.DataGridView dgv_Folges;
        private System.Windows.Forms.Label lbl_Segurança;
        private System.Windows.Forms.Label lbl_Interlocks;
        private System.Windows.Forms.Label lbl_FMs;
        private System.Windows.Forms.Label lbl_Folges;
        private System.Windows.Forms.TextBox txb_Robo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_Ranges;
        private System.Windows.Forms.Label lbl_Usuario;
        private System.Windows.Forms.TextBox txb_DBUsuario;
        private System.Windows.Forms.TextBox txb_DBInstancia;
        private System.Windows.Forms.Label lbl_Instancia;
        private System.Windows.Forms.Label lbl_NumeroFC;
        private System.Windows.Forms.TextBox txb_NumeroFC;
        private System.Windows.Forms.Label lbl_Entradas;
        private System.Windows.Forms.DataGridView dgv_Entradas;
        private System.Windows.Forms.Label lbl_Saidas;
        private System.Windows.Forms.DataGridView dgv_Saidas;
        private System.Windows.Forms.Label lbl_Ferramentas;
        private System.Windows.Forms.DataGridView dgv_Ferramentas;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}