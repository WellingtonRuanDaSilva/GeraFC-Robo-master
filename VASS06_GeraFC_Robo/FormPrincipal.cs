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

        private int securityFirstRow = 27;
        private int toolsFirstRow = 10;
        private int interlockFirstRow = 9;
        private int fmFirstRow = 22;
        private int folgesFirstRow = 9;
        private int inputsFirstRow = 9;
        private int outputsFirstRow = 9;

        private int securityCurrentRow =27;
        private int toolsCurrentRow = 10;
        private int interlockCurrentRow = 9;
        private int fmCurrentRow = 28;
        private int folgesCurrentRow = 15;
        private int inputsCurrentRow = 9;
        private int outputsCurrentRow = 9;

        private int toolsLastRow = 24;
        private int interlockLastRow = 24;
        private int fmLastRow = 41;
        private int folgesLastRow = 24;
        private int inputsLastRow = 46;
        private int outputsLastRow = 46;

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

        private int DBAnwenderNumber = 10;
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
            configTXB(txb_DBUsuario, 10, 149);
            configTXB(txb_DBInstancia, 2000, 20000);
            configTXB(txb_NumeroFC, 14, 99);
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
                    txb_Estacao.Text = worksheet.Cells["C6"].Text;
                    txb_Grupo.Text = worksheet.Cells["C4"].Text;
                    txb_Robo.Text = worksheet.Cells["C9"].Text;
                }
            }

            SKNumber = txb_Grupo.Text;
            stationNumber = txb_Estacao.Text;
            robNumber =  txb_Robo.Text;

            securityAmount = 0;
            securityCurrentRow = 27;
            toolsAmount = 0;
            toolsCurrentRow = 10;
            interlockAmount = 0;
            interlockCurrentRow = 9;
            fmAmount = 0;
            fmCurrentRow = 28;
            folgesAmount = 0;
            folgesCurrentRow = 15;
            inputsAmount = 0;
            inputsCurrentRow = 9;
            outputsAmount = 0;
            outputsCurrentRow = 9;

            int endRow = worksheet.Dimension.End.Row;
            for (int row = securityFirstRow; row <= endRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 16].Text)) securityAmount++;
            }
            for (int row = toolsFirstRow; row <= toolsLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 20].Text)) toolsAmount++;
            }
            for (int row = interlockFirstRow; row <= interlockLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 16].Text)) interlockAmount++;
            }
            for (int row = fmFirstRow; row <= fmLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 2].Text)) fmAmount++;
            }
            for (int row = folgesFirstRow; row <= folgesLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 2].Text)) folgesAmount++;
            }
            for (int row = inputsFirstRow; row <= inputsLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 5].Text)) inputsAmount++;
            }
            for (int row = outputsFirstRow; row <= outputsLastRow; row++)
            {
                if (!string.IsNullOrEmpty(worksheet.Cells[row, 5].Text)) outputsAmount++;
            }

            dgv_Segurança.Rows.Clear();
            dgv_Ferramentas.Rows.Clear();
            dgv_Interlocks.Rows.Clear();
            dgv_FMs.Rows.Clear();
            dgv_Folges.Rows.Clear();
            dgv_Entradas.Rows.Clear();
            dgv_Saidas.Rows.Clear();

            for (int i = 0; i < securityAmount; i++)
            {
                dgv_Segurança.Rows.Add(
                    worksheet.Cells[securityCurrentRow, 16].Text
                );
                securityCurrentRow++;
            }

            for (int i = 0; i < toolsAmount; i++)
            {
                dgv_Ferramentas.Rows.Add(
                    worksheet.Cells[toolsCurrentRow, 20].Text
                );
                toolsCurrentRow++;
            }

            for (int i = 0; i < interlockAmount; i++)
            {
                dgv_Interlocks.Rows.Add(
                    worksheet.Cells[interlockCurrentRow, 16].Text,
                    worksheet.Cells[interlockCurrentRow, 17].Text,
                    worksheet.Cells[interlockCurrentRow, 18].Text
                );
                interlockCurrentRow++;
            }

            for (int i = 0; i < fmAmount; i++)
            {
                dgv_FMs.Rows.Add(
                    worksheet.Cells[fmCurrentRow, 2].Text,
                    worksheet.Cells[fmCurrentRow, 3].Text
                );
                fmCurrentRow++;
            }

            for (int i = 0; i < folgesAmount; i++)
            {
                dgv_Folges.Rows.Add(
                    worksheet.Cells[folgesCurrentRow, 2].Text,
                    worksheet.Cells[folgesCurrentRow, 3].Text
                );
                folgesCurrentRow++;
            }

            for (int i = 0; i < inputsAmount; i++)
            {
                dgv_Entradas.Rows.Add(
                    worksheet.Cells[inputsCurrentRow, 6].Text,
                    worksheet.Cells[inputsCurrentRow, 7].Text,
                    worksheet.Cells[inputsCurrentRow, 8].Text,
                    worksheet.Cells[inputsCurrentRow, 9].Text,
                    worksheet.Cells[inputsCurrentRow, 10].Text
                );
                inputsCurrentRow++;
            }

            for (int i = 0; i < outputsAmount; i++)
            {
                dgv_Saidas.Rows.Add(
                    worksheet.Cells[outputsCurrentRow, 6].Text,
                    worksheet.Cells[outputsCurrentRow, 11].Text,
                    worksheet.Cells[outputsCurrentRow, 12].Text,
                    worksheet.Cells[outputsCurrentRow, 13].Text,
                    worksheet.Cells[outputsCurrentRow, 14].Text
                );
                outputsCurrentRow++;
            }

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

        //====== Configura TextBox para aceitar apenas números dentro de um intervalo ======
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
