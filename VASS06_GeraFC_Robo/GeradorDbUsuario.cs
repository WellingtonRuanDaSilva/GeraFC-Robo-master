using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace VASS06_GeraFC_Robo
{
    public static class GeradorDbUsuario
    {
        public static void Gerar(ref EstacaoData data)
        {
            // Bloco 1: Copiar o template base do DB de Usuário
            // =================================================================
            string originPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/DB_Anwender", "DBAnwender_Template.xml");
            string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export/DB_Anwender", $"{data.SKNumber}{data.StationNumber}{data.StationType}.xml");

            try
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export"));
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export/DB_Anwender"));

                if (!File.Exists(originPath))
                {
                    MessageBox.Show("O arquivo DBAnwender_Template.xml não foi encontrado.", "InfoRMI", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string content = File.ReadAllText(originPath);
                File.WriteAllText(destinationPath, content);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao copiar o arquivo: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Bloco 2: Ler todos os templates e preencher o DB de Usuário
            // =================================================================
            try
            {
                if (!File.Exists(destinationPath))
                {
                    MessageBox.Show("O arquivo de destino não foi encontrado após a cópia.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Caminhos de todos os templates de componentes
                string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/DB_Anwender");
                string ktTemplatePath = Path.Combine(basePath, "KT_Template.xml");
                string fmTemplatePath = Path.Combine(basePath, "FM_Template.xml");
                string viTemplatePath = Path.Combine(basePath, "VI_Template.xml");
                string operatorTemplatePath = Path.Combine(basePath, "Operator_Template.xml");
                string baTemplatePath = Path.Combine(basePath, "BA_Template.xml");

                // Novos caminhos para os templates de cilindro
                string cylinderTemplatePath = Path.Combine(basePath, "Cylinder_Template.xml");
                string saugerTemplatePath = Path.Combine(basePath, "STB_Ventil_Sauger_Template.xml");
                string handTemplatePath = Path.Combine(basePath, "STB_Ventil_Hand_SP_Template.xml");
                string v300TemplatePath = Path.Combine(basePath, "STB_Ventil_300_Template.xml");


                // Leitura dos conteúdos dos templates
                string dbContent = File.ReadAllText(destinationPath);
                string ktTemplate = File.ReadAllText(ktTemplatePath);
                string fmTemplate = File.ReadAllText(fmTemplatePath);
                string viTemplate = File.ReadAllText(viTemplatePath);
                string operatorTemplate = File.ReadAllText(operatorTemplatePath);
                string baContent = File.ReadAllText(baTemplatePath);
                string inverterTemplate = "";

                // Leitura dos novos templates de cilindro
                string cylinderStdTemplate = File.ReadAllText(cylinderTemplatePath);
                string cylinderSaugerTemplate = File.ReadAllText(saugerTemplatePath);
                string cylinderHandTemplate = File.ReadAllText(handTemplatePath);
                string cylinder300Template = File.ReadAllText(v300TemplatePath);


                // Substituições iniciais: Nome e número do DB
                dbContent = dbContent.Replace("[nomeDB]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                     .Replace("[numeroDB]", data.DBAnwenderNumber.ToString());

                StringBuilder ktsContent = new StringBuilder();
                StringBuilder fmsContent = new StringBuilder();
                StringBuilder visContent = new StringBuilder();
                StringBuilder cylindersContent = new StringBuilder();
                StringBuilder invertersContent = new StringBuilder();
                StringBuilder operatorsContent = new StringBuilder();

                // Bloco 2.1: Geração das memórias de KT (Controle de Peça)
                // =================================================================
                HashSet<string> addedKT = new HashSet<string>();
                foreach (DataGridViewRow row in data.DgvSensores.Rows)
                {
                    if (row.IsNewRow) continue;
                    string name = row.Cells[0].Value.ToString();
                    if (!string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                    {
                        addedKT.Add(name.Substring(name.Length - 2));
                    }
                }
                foreach (string ktNumber in addedKT)
                {
                    string ktContent = ktTemplate.Replace("[numero_KT]", ktNumber);
                    ktsContent.AppendLine(ktContent);
                }

                // Bloco 2.2: Geração das memórias de FM (Fim de Trabalho)
                // =================================================================
                foreach (DataGridViewRow row in data.DgvFMs.Rows)
                {
                    if (row.IsNewRow) continue;
                    string numeroFM = row.Cells[0]?.Value?.ToString()?.Trim() ?? "";
                    string descricaoFM = row.Cells[1]?.Value?.ToString()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(numeroFM) && !string.IsNullOrEmpty(descricaoFM))
                    {
                        string fmContent = fmTemplate.Replace("[numero_fm]", numeroFM)
                                                     .Replace("[descricao_fm]", descricaoFM);
                        fmsContent.AppendLine(fmContent);
                    }
                }

                // Bloco 2.3: Geração das memórias de VI (Ilhas de Válvula)
                // =================================================================
                HashSet<string> addedVI = new HashSet<string>();
                foreach (DataGridViewRow row in data.DgvCilindros.Rows)
                {
                    if (row.IsNewRow) continue;
                    string numeroVI = row.Cells[0]?.Value?.ToString()?.Trim() ?? "";
                    if (!string.IsNullOrEmpty(numeroVI) && addedVI.Add(numeroVI))
                    {
                        string viContent = viTemplate.Replace("[numero_VI]", numeroVI);
                        visContent.AppendLine(viContent);
                    }
                }

                // Bloco 2.4: Geração das memórias de Cilindros (Lógica Refatorada)
                // =================================================================
                HashSet<string> addedCylinder = new HashSet<string>();
                foreach (DataGridViewRow row in data.DgvCilindros.Rows)
                {
                    if (row.IsNewRow) continue;
                    string numeroCilindro = row.Cells[1]?.Value?.ToString()?.Trim() ?? "";
                    if (string.IsNullOrEmpty(numeroCilindro) || !addedCylinder.Add(numeroCilindro))
                    {
                        continue;
                    }

                    string descricaoCilindro = row.Cells[3]?.Value?.ToString()?.Trim() ?? "";
                    string fbCilindro = row.Cells[4]?.Value?.ToString()?.Trim().ToUpper() ?? "";

                    string selectedTemplate;

                    // Seleciona o template correto com base no tipo de FB
                    if (fbCilindro == "SAUGER")
                    {
                        selectedTemplate = cylinderSaugerTemplate;
                    }
                    else if (fbCilindro == "HAND")
                    {
                        selectedTemplate = cylinderHandTemplate;
                    }
                    else if (fbCilindro == "300")
                    {
                        selectedTemplate = cylinder300Template;
                    }
                    else
                    {
                        selectedTemplate = cylinderStdTemplate;
                    }

                    // Substitui os placeholders no template selecionado
                    string cylinderContent = selectedTemplate
                        .Replace("[numero_cilindro]", numeroCilindro)
                        .Replace("[descricao_cilindro]", descricaoCilindro);

                    // O placeholder [tipo_de_cilindro] não é mais necessário, pois o tipo já está no template.
                    // Mas caso ainda exista no template padrão, podemos remover ou substituir.
                    cylinderContent = cylinderContent.Replace("[tipo_de_cilindro]", "STB_Ventil");


                    cylindersContent.AppendLine(cylinderContent);
                }

                // Bloco 2.5: Geração das memórias de Inversores (para RB)
                // =================================================================
                if (data.StationType.ToUpper() == "RB" || data.StationType.ToUpper() == "RB1")
                {
                    string inverterTemplatePath = Path.Combine(basePath, "STB_Elefant2_AMX_2P_Template.xml");
                    if (File.Exists(inverterTemplatePath))
                    {
                        inverterTemplate = File.ReadAllText(inverterTemplatePath);
                    }
                    else
                    {
                        MessageBox.Show($"Aviso: Template 'STB_Elefant2_AMX_2P_Template.xml' não encontrado.", "Template Faltando", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                if ((data.StationType.ToUpper() == "RB" || data.StationType.ToUpper() == "RB1") && !string.IsNullOrEmpty(inverterTemplate))
                {
                    string heberHubRemechTemplatePath = Path.Combine(basePath, "STB_HUBRemech_Template.xml");
                    string heberSewAmaBinTemplatePath = Path.Combine(basePath, "STB_SEW_AMA_BIN_Template.xml");
                    string heberHubRemechTemplate = File.Exists(heberHubRemechTemplatePath) ? File.ReadAllText(heberHubRemechTemplatePath) : "";
                    string heberSewAmaBinTemplate = File.Exists(heberSewAmaBinTemplatePath) ? File.ReadAllText(heberSewAmaBinTemplatePath) : "";

                    foreach (DataGridViewRow row in data.DgvInversores.Rows)
                    {
                        if (row.IsNewRow || row.Cells[0].Value == null) continue;

                        string nomeDispositivo = row.Cells[0]?.Value?.ToString()?.Trim();
                        string descricaoDispositivo = row.Cells[1]?.Value?.ToString()?.Trim();
                        string descricaoUpper = descricaoDispositivo.ToUpper();

                        if (descricaoUpper == "ROLLENBAHN" && !string.IsNullOrEmpty(inverterTemplate))
                        {
                            if (!string.IsNullOrEmpty(nomeDispositivo))
                            {
                                string inverterBlock = inverterTemplate
                                                           .Replace("[nome_dispositivo]", nomeDispositivo)
                                                           .Replace("[descricao_dispositivo]", descricaoDispositivo);
                                invertersContent.AppendLine(inverterBlock);
                            }
                        }
                        else if (descricaoUpper == "HEBER")
                        {
                            if (!string.IsNullOrEmpty(nomeDispositivo) && !string.IsNullOrEmpty(heberHubRemechTemplate) && !string.IsNullOrEmpty(heberSewAmaBinTemplate))
                            {
                                string hubRemechBlock = heberHubRemechTemplate
                                                           .Replace("[nome_dispositivo]", nomeDispositivo)
                                                           .Replace("[descricao_dispositivo]", descricaoDispositivo);
                                invertersContent.AppendLine(hubRemechBlock);
                                string sewAmaBinBlock = heberSewAmaBinTemplate
                                                           .Replace("[nome_dispositivo]", nomeDispositivo)
                                                           .Replace("[descricao_dispositivo]", descricaoDispositivo);
                                invertersContent.AppendLine(sewAmaBinBlock);
                            }
                        }
                    }
                }

                // Bloco 2.6: Geração das memórias de Operador (SubBA)
                // =================================================================
                if (data.IsSubBA)
                {
                    string operatorContent = operatorTemplate.Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}");
                    operatorsContent.AppendLine(operatorContent);
                }

                // Bloco 3: Montagem Final e Salvamento do Arquivo
                // =================================================================
                dbContent = dbContent.Replace("[lista_de_KTs]", ktsContent.ToString())
                                     .Replace("[lista_de_FMs]", fmsContent.ToString())
                                     .Replace("[lista_de_VIs]", visContent.ToString())
                                     .Replace("[lista_de_cilindros]", cylindersContent.ToString())
                                     .Replace("[lista_de_inversores]", invertersContent.ToString())
                                     .Replace("[memorias_de_operador]", operatorsContent.ToString())
                                     .Replace("[modo_de_operacao]", baContent.ToString());

                File.WriteAllText(destinationPath, dbContent);

                // Incrementa o número do DB para o próximo arquivo
                if (data.DBAnwenderNumber < 149)
                {
                    data.DBAnwenderNumber++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao preencher o arquivo DB de Usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

