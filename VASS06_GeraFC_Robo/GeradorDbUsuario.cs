using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace VASS06_GeraFC_Robo
{
    public static class GeradorDbUsuario
    {
        public static void Gerar(ref RoboData data)
        {
            // Caminhos de Origem e Destino
            string resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/DB_Anwender");
            string originPath = Path.Combine(resourcesPath, "DBAnwender_Template.txt");
            string fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}.xml"; // Ex: 110010R01.xml
            string exportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export/DB_Anwender");
            string destinationPath = Path.Combine(exportDir, fileName);

            // Bloco 1: Preparar diretórios e copiar o template base
            // =================================================================
            try
            {
                if (!Directory.Exists(exportDir))
                    Directory.CreateDirectory(exportDir);

                if (!File.Exists(originPath))
                {
                    MessageBox.Show($"Arquivo de template não encontrado:\n{originPath}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                File.Copy(originPath, destinationPath, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao criar arquivo de destino: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Bloco 2: Processar Templates e Preencher Dados
            // =================================================================
            try
            {
                string dbContent = File.ReadAllText(destinationPath);

                // Carregar templates de itens
                string eTemplate = LoadTemplateBlock(Path.Combine(resourcesPath, "E_Template.txt"));
                string aTemplate = LoadTemplateBlock(Path.Combine(resourcesPath, "A_Template.txt"));
                string fmTemplate = LoadTemplateBlock(Path.Combine(resourcesPath, "FM_Template.xml"));

                // Templates de Ferramentas
                string klTemplate = ReadFileSafe(Path.Combine(resourcesPath, "KL_Template.txt")); // Kleben
                string skTemplate = ReadFileSafe(Path.Combine(resourcesPath, "SK_Template.txt")); // Schweissen
                string czTemplate = ReadFileSafe(Path.Combine(resourcesPath, "CZ_Template.txt")); // Durchsetzfügen (Clinch)
                string kwTemplate = ReadFileSafe(Path.Combine(resourcesPath, "KW_Template.txt")); // Kappenwechsler

                // 2.1 Entradas (Inputs)
                StringBuilder inputsContent = new StringBuilder();
                if (data.DgvEntradas != null)
                {
                    foreach (DataGridViewRow row in data.DgvEntradas.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string numero = row.Cells[0]?.Value?.ToString()?.Trim() ?? "";
                        string descricao = row.Cells[4]?.Value?.ToString()?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(numero) && !string.IsNullOrEmpty(eTemplate))
                        {
                            string item = ReplaceDescription(eTemplate, descricao);
                            item = item.Replace("[numero_entrada]", numero);
                            inputsContent.AppendLine(item);
                        }
                    }
                }

                // 2.2 Saídas (Outputs)
                StringBuilder outputsContent = new StringBuilder();
                if (data.DgvSaidas != null)
                {
                    foreach (DataGridViewRow row in data.DgvSaidas.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string numero = row.Cells[0]?.Value?.ToString()?.Trim() ?? "";
                        string descricao = row.Cells[4]?.Value?.ToString()?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(numero) && !string.IsNullOrEmpty(aTemplate))
                        {
                            string item = ReplaceDescription(aTemplate, descricao);
                            item = item.Replace("[numero_saida]", numero);
                            outputsContent.AppendLine(item);
                        }
                    }
                }

                // 2.3 FMs (Fertigmeldung)
                StringBuilder fmsContent = new StringBuilder();
                if (data.DgvFMs != null)
                {
                    foreach (DataGridViewRow row in data.DgvFMs.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string numero = row.Cells[0]?.Value?.ToString()?.Trim() ?? "";
                        string descricao = row.Cells[1]?.Value?.ToString()?.Trim() ?? "";

                        if (!string.IsNullOrEmpty(numero) && !string.IsNullOrEmpty(fmTemplate))
                        {
                            string item = fmTemplate;
                            // Remove números fixos do template se existirem (ex: FM1 -> FM[numero])
                            if (item.Contains("Name=\"FM1\"")) item = item.Replace("Name=\"FM1\"", $"Name=\"FM{numero}\"");
                            else item = item.Replace("[numero_fm]", numero);

                            item = ReplaceDescription(item, descricao);
                            item = item.Replace("[descricao_fm]", descricao);
                            fmsContent.AppendLine(item);
                        }
                    }
                }

                // 2.4 Ferramentas (Tools)
                // =================================================================
                StringBuilder toolsContent = new StringBuilder();
                if (data.DgvFerramentas != null)
                {
                    // Contadores para gerar nomes automáticos
                    int skCount = 1, klCount = 1, czCount = 1, kwCount = 1;

                    foreach (DataGridViewRow row in data.DgvFerramentas.Rows)
                    {
                        if (row.IsNewRow) continue;
                        string textoFerramenta = row.Cells[0]?.Value?.ToString()?.Trim() ?? "";
                        string textoUpper = textoFerramenta.ToUpper();

                        string selectedTemplate = "";
                        string nomeVar = "";

                        // LÓGICA ATUALIZADA: Verifica palavras completas E abreviações
                        if (textoUpper.Contains("KLEB") || textoUpper.Contains("GLUE") || textoUpper.Contains("KL"))
                        {
                            selectedTemplate = klTemplate;
                            nomeVar = $"KL{klCount++}";
                        }
                        else if (textoUpper.Contains("SCHWEISS") || textoUpper.Contains("WELD") || textoUpper.Contains("ZANGE") || textoUpper.Contains("SK"))
                        {
                            selectedTemplate = skTemplate;
                            nomeVar = $"SK{skCount++}";
                        }
                        else if (textoUpper.Contains("DURCHSETZ") || textoUpper.Contains("CLINCH") || textoUpper.Contains("TOX") || textoUpper.Contains("CZ"))
                        {
                            selectedTemplate = czTemplate;
                            nomeVar = $"CZ{czCount++}";
                        }
                        else if (textoUpper.Contains("WECHSLER") || textoUpper.Contains("CHANGE") || textoUpper.Contains("KW"))
                        {
                            selectedTemplate = kwTemplate;
                            nomeVar = $"KW{kwCount++}";
                        }

                        if (!string.IsNullOrEmpty(selectedTemplate))
                        {
                            // Se o texto da célula já parecer um código (ex: "KL01", "SK2"), usa ele direto.
                            // Regex verifica se começa com 2 letras seguidas de digitos, ex: AA1...
                            if (textoFerramenta.Length <= 6 && Regex.IsMatch(textoFerramenta, @"^[A-Z]{2}\d+"))
                                nomeVar = textoFerramenta;

                            string item = selectedTemplate.Replace("[nome_ferramenta]", nomeVar);

                            // Usa o nome da ferramenta como descrição no comentário
                            item = ReplaceDescription(item, textoFerramenta);

                            toolsContent.AppendLine(item);
                        }
                    }
                }

                // 3. Substituição Final no Arquivo
                dbContent = dbContent.Replace("[nomeDB]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                     .Replace("[numeroDB]", data.DBAnwenderNumber.ToString())
                                     .Replace("[lista_de_FMs]", fmsContent.ToString())
                                     .Replace("[lista_de_entrada]", inputsContent.ToString())
                                     .Replace("[lista_de_saida]", outputsContent.ToString())
                                     .Replace("[lista_de_ferramentas]", toolsContent.ToString());

                File.WriteAllText(destinationPath, dbContent);

                // Incrementa número do DB
                if (data.DBAnwenderNumber < 149) data.DBAnwenderNumber++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao processar dados do DB de Usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Métodos Auxiliares

        private static string ReadFileSafe(string path)
        {
            return File.Exists(path) ? File.ReadAllText(path) : "";
        }

        private static string LoadTemplateBlock(string path)
        {
            if (!File.Exists(path)) return "";
            string content = File.ReadAllText(path);

            // Tenta pegar apenas o primeiro bloco <Member>...</Member>
            int start = content.IndexOf("<Member");
            if (start == -1) return content;
            int end = content.IndexOf("</Member>", start);
            if (end == -1) return content;

            return content.Substring(start, (end + 9) - start);
        }

        private static string ReplaceDescription(string content, string newDescription)
        {
            // Substitui o texto dentro de <MultiLanguageText Lang="de-DE">...</MultiLanguageText>
            string pattern = @"(<MultiLanguageText Lang=""de-DE"">)(.*?)(</MultiLanguageText>)";
            return Regex.Replace(content, pattern, $"$1{newDescription}$3");
        }
    }
}