using System;
using System.IO;
using System.Windows.Forms;
using OfficeOpenXml;

namespace VASS06_GeraFC_Robo
{
    public partial class FormPrincipal : Form
    {
        private ExcelPackage package;
        public ExcelWorksheet worksheet;
        private string selectedSheetName;

        // --- NOVAS POSIÇÕES DEFINIDAS ---
        private int securityFirstRow = 24;   // P27 -> Q24
        private int toolsFirstRow = 10;      // Mantido (sem info contrária)
        private int interlockFirstRow = 4;   // Acompanha o cabeçalho geral (era 9)
        private int fmFirstRow = 30;         // B28 -> D30
        private int folgesFirstRow = 4;      // B15 -> A4
        private int inputsFirstRow = 4;      // E9 -> G4
        private int outputsFirstRow = 4;     // Acompanha inputs

        private int securityCurrentRow;
        private int toolsCurrentRow;
        private int interlockCurrentRow;
        private int fmCurrentRow;
        private int folgesCurrentRow;
        private int inputsCurrentRow;
        private int outputsCurrentRow;

        // Limites de leitura (ajustados para varrer até o fim ou um numero fixo seguro)
        // Como o método usa worksheet.Dimension.End.Row, esses "LastRow" fixos
        // são usados apenas nos loops de contagem se não for dinâmico.
        // Vou manter os valores originais como referência mínima, mas o loop principal usa 'endRow'.
        private int toolsLastRow = 24;
        private int interlockLastRow = 24;
        private int fmLastRow = 60; // Aumentado por segurança
        private int folgesLastRow = 24;
        private int inputsLastRow = 100; // Aumentado
        private int outputsLastRow = 100; // Aumentado

        private int securityAmount = 0;
        private int toolsAmount = 0;
        private int interlockAmount = 0;
        private int fmAmount = 0;
        private int folgesAmount = 0;
        private int inputsAmount = 0;
        private int outputsAmount = 0;

        private string SKNumber = "";
        private string stationNumber = "";
        private string robNumber = "";

        // === ALTERAÇÃO AQUI: Valores iniciais ajustados para 100 ===
        private int DBAnwenderNumber = 100;
        private int DBInstanzenNumber = 2000;
        private int FCNumber = 100;

        public FormPrincipal()
        {
            InitializeComponent();
            config_DataGridView_Segurança();
            config_DataGridView_Interlocks();
            config_DataGridView_Ferramentas();
            config_DataGridView_FMs();
            config_DataGridView_Folges();
            config_DataGridView_Entradas();
            config_DataGridView_Saidas();

            // === ALTERAÇÃO AQUI: Define o texto inicial como "100" ===
            txb_DBUsuario.Text = "100";
            txb_NumeroFC.Text = "100";

            // === ALTERAÇÃO AQUI: Limites superiores aumentados para remover o alerta ===
            // DB Usuario: Limite aumentado para 1000 (era 149)
            configTXB(txb_DBUsuario, 1, 1000);

            // DB Instancia: Mantido
            configTXB(txb_DBInstancia, 2000, 20000);

            // Numero FC: Limite aumentado para 1000 (era 99)
            configTXB(txb_NumeroFC, 1, 1000);
        }

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void btn_AbrirPlanilha_Click(object sender, EventArgs e)
        {
            OpenFileDialog filepath = new OpenFileDialog();
            filepath.Filter = "Arquivos Excel (*.xlsx)|*.xlsx|Todos os arquivos (*.*)|*.*";
            filepath.Title = "Selecionar arquivo Excel";

            if (filepath.ShowDialog() == DialogResult.OK)
            {
                FileInfo fileinfo = new FileInfo(filepath.FileName);
                package = new ExcelPackage(fileinfo);
                if (package.Workbook.Worksheets.Count > 0)
                {
                    MessageBox.Show("Pasta de trabalho carregada com sucesso.", "InfoRMI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ltb_Robos.Items.Clear();
                    foreach (var sheet in package.Workbook.Worksheets)
                    {
                        if (sheet.Name.Contains("R0"))
                        {
                            ltb_Robos.Items.Add(sheet.Name);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("O arquivo não contém nenhuma planilha.", "InfoRMI", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ltb_Robos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ltb_Robos.SelectedItem != null && package != null)
            {
                selectedSheetName = ltb_Robos.SelectedItem.ToString();
                worksheet = package.Workbook.Worksheets[selectedSheetName];

                if (worksheet != null)
                {
                    // Lógica B1: 2 chars SK, 4 chars Estação, 3 chars Robô
                    string fullID = worksheet.Cells["B1"].Text;
                    if (!string.IsNullOrEmpty(fullID) && fullID.Length >= 9)
                    {
                        txb_Grupo.Text = fullID.Substring(0, 2);
                        txb_Estacao.Text = fullID.Substring(2, 4);
                        txb_Robo.Text = fullID.Substring(6, 3);
                    }
                    else
                    {
                        txb_Grupo.Text = "";
                        txb_Estacao.Text = "";
                        txb_Robo.Text = "";
                    }
                }
            }

            SKNumber = txb_Grupo.Text;
            stationNumber = txb_Estacao.Text;
            robNumber = txb_Robo.Text;

            // Reinicia contadores
            securityAmount = 0;
            toolsAmount = 0;
            interlockAmount = 0;
            fmAmount = 0;
            folgesAmount = 0;
            inputsAmount = 0;
            outputsAmount = 0;

            // Define linhas iniciais para leitura (reset)
            securityCurrentRow = securityFirstRow;   // 24
            toolsCurrentRow = toolsFirstRow;         // 10
            interlockCurrentRow = interlockFirstRow; // 4
            fmCurrentRow = fmFirstRow;               // 30
            folgesCurrentRow = folgesFirstRow;       // 4
            inputsCurrentRow = inputsFirstRow;       // 4
            outputsCurrentRow = outputsFirstRow;     // 4

            int endRow = worksheet.Dimension.End.Row;

            // === LOOPS DE CONTAGEM ===

            // Segurança: Coluna Q (17) a partir da linha 24
            for (int row = securityFirstRow; row <= endRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 17].Text)) securityAmount++;
            }

            // Ferramentas: Coluna T (20) a partir da linha 10 (Mantido original)
            for (int row = toolsFirstRow; row <= toolsLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 20].Text)) toolsAmount++;
            }

            // Interlocks: Coluna Q (17) a partir da linha 4
            for (int row = interlockFirstRow; row <= interlockLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 17].Text)) interlockAmount++;
            }

            // FMs: Coluna D (4) a partir da linha 30
            for (int row = fmFirstRow; row <= endRow; row++) // Usando endRow pois pode haver muitos FMs
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 4].Text)) fmAmount++;
            }

            // Folges: Coluna A (1) a partir da linha 4
            for (int row = folgesFirstRow; row <= folgesLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 1].Text)) folgesAmount++;
            }

            // Inputs: Coluna G (7) usada como check (E->G) a partir da linha 4
            for (int row = inputsFirstRow; row <= inputsLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 7].Text)) inputsAmount++;
            }

            // Outputs: Coluna G (7) usada como check (Compartilhada) a partir da linha 4
            for (int row = outputsFirstRow; row <= outputsLastRow; row++)
            {
                // Nota: Pode precisar de um critério melhor para distinguir Input de Output se estiverem misturados
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 7].Text)) outputsAmount++;
            }

            // Limpa Grids
            dgv_Segurança.Rows.Clear();
            dgv_Ferramentas.Rows.Clear();
            dgv_Interlocks.Rows.Clear();
            dgv_FMs.Rows.Clear();
            dgv_Folges.Rows.Clear();
            dgv_Entradas.Rows.Clear();
            dgv_Saidas.Rows.Clear();

            // === PREENCHIMENTO DOS GRIDS ===

            // Segurança (Q -> 17)
            for (int i = 0; i < securityAmount; i++)
            {
                dgv_Segurança.Rows.Add(worksheet.Cells[securityCurrentRow, 17].Text);
                securityCurrentRow++;
            }

            // Ferramentas (T -> 20)
            for (int i = 0; i < toolsAmount; i++)
            {
                dgv_Ferramentas.Rows.Add(worksheet.Cells[toolsCurrentRow, 20].Text);
                toolsCurrentRow++;
            }

            // Interlocks (Q, R, S -> 17, 18, 19)
            for (int i = 0; i < interlockAmount; i++)
            {
                dgv_Interlocks.Rows.Add(
                    worksheet.Cells[interlockCurrentRow, 17].Text,
                    worksheet.Cells[interlockCurrentRow, 18].Text,
                    worksheet.Cells[interlockCurrentRow, 19].Text
                );
                interlockCurrentRow++;
            }

            // FMs (D, E -> 4, 5)
            for (int i = 0; i < fmAmount; i++)
            {
                dgv_FMs.Rows.Add(
                    worksheet.Cells[fmCurrentRow, 4].Text, // ID
                    worksheet.Cells[fmCurrentRow, 5].Text  // Descrição
                );
                fmCurrentRow++;
            }

            // Folges (A, B -> 1, 2)
            for (int i = 0; i < folgesAmount; i++)
            {
                dgv_Folges.Rows.Add(
                    worksheet.Cells[folgesCurrentRow, 1].Text, // Folge
                    worksheet.Cells[folgesCurrentRow, 2].Text  // Descrição
                );
                folgesCurrentRow++;
            }

            // Entradas (Inputs)
            // Mapeamento: G(7)=Tipo, H(8)=Endereço, N(14)=Estação, I(9)=Ext, J(10)=Desc
            for (int i = 0; i < inputsAmount; i++)
            {
                dgv_Entradas.Rows.Add(
                    worksheet.Cells[inputsCurrentRow, 8].Text,  // Endereço (H)
                    worksheet.Cells[inputsCurrentRow, 7].Text,  // Tipo (G)
                    worksheet.Cells[inputsCurrentRow, 14].Text, // Estação (N)
                    worksheet.Cells[inputsCurrentRow, 9].Text,  // Ext (I)
                    worksheet.Cells[inputsCurrentRow, 10].Text  // Descrição (J)
                );
                inputsCurrentRow++;
            }

            // Saídas (Outputs)
            // Mapeamento: H(8)=Endereço, L(12)=Tipo, N(14)=Estação, M(13)=Ext, J(10)=Desc
            for (int i = 0; i < outputsAmount; i++)
            {
                dgv_Saidas.Rows.Add(
                    worksheet.Cells[outputsCurrentRow, 8].Text,  // Endereço (H)
                    worksheet.Cells[outputsCurrentRow, 12].Text, // Tipo (L)
                    worksheet.Cells[outputsCurrentRow, 14].Text, // Estação (N)
                    worksheet.Cells[outputsCurrentRow, 13].Text, // Ext (M)
                    worksheet.Cells[outputsCurrentRow, 10].Text  // Descrição (J - Assumida compartilhada)
                );
                outputsCurrentRow++;
            }

            // Ajuste de altura
            dgv_Segurança.Height = dgv_Segurança.ColumnHeadersHeight + (dgv_Segurança.RowCount * dgv_Segurança.RowTemplate.Height);
            dgv_Ferramentas.Height = dgv_Ferramentas.ColumnHeadersHeight + (dgv_Ferramentas.RowCount * dgv_Ferramentas.RowTemplate.Height);
            dgv_Interlocks.Height = dgv_Interlocks.ColumnHeadersHeight + (dgv_Interlocks.RowCount * dgv_Interlocks.RowTemplate.Height);
            dgv_FMs.Height = dgv_FMs.ColumnHeadersHeight + (dgv_FMs.RowCount * dgv_FMs.RowTemplate.Height);
            dgv_Folges.Height = dgv_Folges.ColumnHeadersHeight + (dgv_Folges.RowCount * dgv_Folges.RowTemplate.Height);
            dgv_Entradas.Height = dgv_Entradas.ColumnHeadersHeight + (dgv_Entradas.RowCount * dgv_Entradas.RowTemplate.Height);
            dgv_Saidas.Height = dgv_Saidas.ColumnHeadersHeight + (dgv_Saidas.RowCount * dgv_Saidas.RowTemplate.Height);
        }

        private void btn_GerarArquivos_Click(object sender, EventArgs e)
        {
            // ... (Restante do código permanece igual)
            if (worksheet == null || selectedSheetName == null)
            {
                MessageBox.Show("Nenhuma planilha selecionada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(stationNumber) || string.IsNullOrWhiteSpace(SKNumber))
            {
                MessageBox.Show("Por favor, insira um nome de estação válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                RoboData data = new RoboData
                {
                    SKNumber = this.SKNumber,
                    StationNumber = this.stationNumber,
                    RobNumber = this.robNumber,
                    DBAnwenderNumber = this.DBAnwenderNumber,
                    DBInstanzenNumber = this.DBInstanzenNumber,
                    FCNumber = this.FCNumber,
                    securityAmount = this.securityAmount,
                    toolsAmount = this.toolsAmount,
                    interlockAmount = this.interlockAmount,
                    FmAmount = this.fmAmount,
                    folgesAmount = this.folgesAmount,
                    inputsAmount = this.inputsAmount,
                    outputsAmount = this.outputsAmount,
                    DgvSegurança = this.dgv_Segurança,
                    DgvFerramentas = this.dgv_Ferramentas,
                    DgvInterlocks = this.dgv_Interlocks,
                    DgvFMs = this.dgv_FMs,
                    DgvFolges = this.dgv_Folges,
                    DgvEntradas = this.dgv_Entradas,
                    DgvSaidas = this.dgv_Saidas
                };

                GeradorDbUsuario.Gerar(ref data);
                GeradorDbInstancia.Gerar(ref data);
                GeradorFC_Robo.Gerar(ref data);

                this.DBAnwenderNumber = data.DBAnwenderNumber;
                this.DBInstanzenNumber = data.DBInstanzenNumber;
                this.FCNumber = data.FCNumber;

                txb_DBUsuario.Text = this.DBAnwenderNumber.ToString();
                txb_DBInstancia.Text = this.DBInstanzenNumber.ToString();
                txb_NumeroFC.Text = this.FCNumber.ToString();

                MessageBox.Show($"Arquivos gerados com sucesso para o Robo {SKNumber}{stationNumber}{robNumber}!", "InfoRMI", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show($"Erro na geração dos arquivos: {err}", "InfoRMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ... (Os demais métodos auxiliares como configTXB, createDGVStyles e configs dos grids permanecem inalterados)
        private void configTXB(TextBox txb, int limiteInferior, int limiteSuperior)
        {
            txb.KeyPress += (sender, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
            };
            txb.Leave += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(txb.Text))
                {
                    MessageBox.Show($"O campo não pode ficar vazio. Digite um valor entre {limiteInferior} e {limiteSuperior}.", "InfoRMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txb.Text = limiteInferior.ToString();
                    txb.Focus();
                }
                else if (int.TryParse(txb.Text, out int valor))
                {
                    if (valor < limiteInferior || valor > limiteSuperior)
                    {
                        MessageBox.Show($"Digite um valor entre {limiteInferior} e {limiteSuperior}.", "InfoRMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txb.Text = limiteInferior.ToString();
                        txb.Focus();
                    }
                }
            };
        }

        private void txbDBUsuario_TextChanged(object sender, EventArgs e)
        {
            if (txb_DBUsuario.Text != "") DBAnwenderNumber = Convert.ToInt32(txb_DBUsuario.Text);
        }

        private void txbDBInstancia_TextChanged(object sender, EventArgs e)
        {
            if (txb_DBInstancia.Text != "") DBInstanzenNumber = Convert.ToInt32(txb_DBInstancia.Text);
        }

        private void txbNumeroFC_TextChanged(object sender, EventArgs e)
        {
            if (txb_NumeroFC.Text != "") FCNumber = Convert.ToInt32(txb_NumeroFC.Text);
        }

        private void CreateDGVStyles(DataGridView dgv)
        {
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgv.AllowUserToAddRows = false;
            dgv.Height = dgv.ColumnHeadersHeight;
            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.Gray;
            dgv.DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        private void config_DataGridView_Segurança()
        {
            dgv_Segurança.ColumnCount = 1;
            dgv_Segurança.Columns[0].HeaderText = "Robo";
            dgv_Segurança.Columns[0].Width = 80;
            CreateDGVStyles(dgv_Segurança);
        }

        private void config_DataGridView_Interlocks()
        {
            dgv_Interlocks.ColumnCount = 3;
            dgv_Interlocks.Columns[0].HeaderText = "Verr.";
            dgv_Interlocks.Columns[1].HeaderText = "I/O";
            dgv_Interlocks.Columns[2].HeaderText = "Robo";
            dgv_Interlocks.Columns[0].Width = 40;
            dgv_Interlocks.Columns[1].Width = 40;
            dgv_Interlocks.Columns[2].Width = 80;
            CreateDGVStyles(dgv_Interlocks);
        }

        private void config_DataGridView_Ferramentas()
        {
            dgv_Ferramentas.ColumnCount = 1;
            dgv_Ferramentas.Columns[0].HeaderText = "Ferramentas";
            dgv_Ferramentas.Columns[0].Width = 80;
            CreateDGVStyles(dgv_Ferramentas);
        }

        private void config_DataGridView_FMs()
        {
            dgv_FMs.ColumnCount = 2;
            dgv_FMs.Columns[0].HeaderText = "FM";
            dgv_FMs.Columns[1].HeaderText = "Description";
            dgv_FMs.Columns[0].Width = 40;
            dgv_FMs.Columns[1].Width = 100;
            CreateDGVStyles(dgv_FMs);
        }

        private void config_DataGridView_Folges()
        {
            dgv_Folges.ColumnCount = 2;
            dgv_Folges.Columns[0].HeaderText = "Folge";
            dgv_Folges.Columns[1].HeaderText = "Description";
            dgv_Folges.Columns[0].Width = 40;
            dgv_Folges.Columns[1].Width = 100;
            CreateDGVStyles(dgv_Folges);
        }

        private void config_DataGridView_Entradas()
        {
            dgv_Entradas.ColumnCount = 5;
            dgv_Entradas.Columns[0].HeaderText = "I";
            dgv_Entradas.Columns[1].HeaderText = "Tipo";
            dgv_Entradas.Columns[2].HeaderText = "Estação";
            dgv_Entradas.Columns[3].HeaderText = "Ext.";
            dgv_Entradas.Columns[4].HeaderText = "Descrição";
            dgv_Entradas.Columns[0].Width = 20;
            dgv_Entradas.Columns[1].Width = 40;
            dgv_Entradas.Columns[2].Width = 60;
            dgv_Entradas.Columns[3].Width = 30;
            dgv_Entradas.Columns[4].Width = 100;
            CreateDGVStyles(dgv_Entradas);
        }

        private void config_DataGridView_Saidas()
        {
            dgv_Saidas.ColumnCount = 5;
            dgv_Saidas.Columns[0].HeaderText = "O";
            dgv_Saidas.Columns[1].HeaderText = "Tipo";
            dgv_Saidas.Columns[2].HeaderText = "Estação";
            dgv_Saidas.Columns[3].HeaderText = "Ext.";
            dgv_Saidas.Columns[4].HeaderText = "Descrição";
            dgv_Saidas.Columns[0].Width = 20;
            dgv_Saidas.Columns[1].Width = 40;
            dgv_Saidas.Columns[2].Width = 60;
            dgv_Saidas.Columns[3].Width = 30;
            dgv_Saidas.Columns[4].Width = 100;
            CreateDGVStyles(dgv_Saidas);
        }
    }
}