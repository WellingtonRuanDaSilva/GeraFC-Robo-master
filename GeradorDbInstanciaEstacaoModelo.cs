using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace VASS06_GeraFC
{
    public static class GeradorDbInstancia
    {
        public static void Gerar(ref EstacaoData data)
        {
            // Define os caminhos das pastas de origem (templates) e destino (export)
            string originFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "DB_Instanzen");
            string destinationFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export", "DB_Instanzen");

            Directory.CreateDirectory(destinationFolder);

            // Busca todos os arquivos de template XML na pasta de origem
            string[] FBTemplates = Directory.GetFiles(originFolder, "*.xml");

            // Itera sobre cada template encontrado
            foreach (string FBTemplate in FBTemplates)
            {
                string templateName = Path.GetFileName(FBTemplate);
                string fileName = "";
                string content = "";
                string destinationPath = "";
                HashSet<string> processedVI = new HashSet<string>();

                // Bloco 1: Lógica para pular templates de operador se a estação não for SubBA
                // =================================================================
                if (!data.IsSubBA)
                {
                    if (templateName == "#FB_MIA_DB.xml" || templateName == "#FB_Ruf_DB.xml")
                    {
                        continue; // Pula para o próximo template
                    }
                }

                // Bloco 2: Geração de DBs de Instância para Dispositivos de Segurança (Rolltor)
                // =================================================================
                if (templateName == "#FB_Rolltor_DB.xml")
                {
                    // Apenas gera o arquivo se houver dispositivos de segurança na lista
                    if (data.SafetyAmount > 0)
                    {
                        for (int device = 0; device < data.SafetyAmount; device++)
                        {
                            if (data.DgvSeguranca.Rows[device].Cells[0].Value.ToString() == "SF")
                            {
                                string SFName = data.DgvSeguranca.Rows[device].Cells[1].Value.ToString();
                                fileName = $"{data.SKNumber}{data.StationNumber}{SFName}AE1{templateName}";

                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);

                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    // O continue garante que este template NUNCA será processado pela lógica genérica.
                    continue;
                }
                // Bloco 3: Geração de DBs de Instância para Ilhas de Válvula
                // =================================================================
                else if (templateName == "#FB_Ventil_01E_DB.xml" && data.CylindersAmount > 0)
                {
                    for (int vi = 0; vi < data.CylindersAmount; vi++)
                    {
                        string VINumber = data.DgvCilindros.Rows[vi].Cells[0]?.Value?.ToString()?.Trim();
                        if (!string.IsNullOrEmpty(VINumber) && processedVI.Add(VINumber))
                        {
                            fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{VINumber}KKP01E{templateName}";

                            content = File.ReadAllText(FBTemplate)
                                .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                .Replace("[numero_VI]", VINumber)
                                .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                            destinationPath = Path.Combine(destinationFolder, fileName);
                            File.WriteAllText(destinationPath, content);

                            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                        }
                    }
                    continue;
                }
                // Bloco 4: Geração de DBs para Cilindros (com base no tipo de FB)
                // =================================================================
                // 4.1 Cilindros Comuns
                else if (templateName == "#FB_Ventil_DB.xml" && data.CylindersAmount > 0)
                {
                    for (int cylinder = 0; cylinder < data.CylindersAmount; cylinder++)
                    {
                        if (data.DgvCilindros.Rows[cylinder].Cells[4].Value.ToString() == "")
                        {
                            string cylinderNumber = data.DgvCilindros.Rows[cylinder].Cells[1].Value.ToString();
                            string cylinderDescription = data.DgvCilindros.Rows[cylinder].Cells[3].Value.ToString();
                            fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{cylinderNumber}{templateName}";

                            content = File.ReadAllText(FBTemplate)
                                .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                .Replace("[numero_cilindro]", cylinderNumber)
                                .Replace("[descricao_cilindro]", cylinderDescription)
                                .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                            destinationPath = Path.Combine(destinationFolder, fileName);
                            File.WriteAllText(destinationPath, content);

                            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                        }
                    }
                    continue;
                }
                // 4.2 Cilindros Manuais
                else if (templateName == "#FB_Ventil_Hand_SP_DB.xml" && data.CylindersAmount > 0)
                {
                    for (int cylinder = 0; cylinder < data.CylindersAmount; cylinder++)
                    {
                        if (data.DgvCilindros.Rows[cylinder].Cells[4].Value.ToString().ToUpper() == "HAND")
                        {
                            string cylinderNumber = data.DgvCilindros.Rows[cylinder].Cells[1].Value.ToString();
                            string cylinderDescription = data.DgvCilindros.Rows[cylinder].Cells[3].Value.ToString();
                            fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{cylinderNumber}{templateName}";

                            content = File.ReadAllText(FBTemplate)
                                .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                .Replace("[numero_cilindro]", cylinderNumber)
                                .Replace("[descricao_cilindro]", cylinderDescription)
                                .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                            destinationPath = Path.Combine(destinationFolder, fileName);
                            File.WriteAllText(destinationPath, content);

                            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                        }
                    }
                    continue;
                }
                // 4.3 Cilindros com Freio Pneumático
                else if (templateName == "#FB_Ventil_300_DB.xml" && data.CylindersAmount > 0)
                {
                    for (int cylinder = 0; cylinder < data.CylindersAmount; cylinder++)
                    {
                        if (data.DgvCilindros.Rows[cylinder].Cells[4].Value.ToString().ToUpper() == "300")
                        {
                            string cylinderNumber = data.DgvCilindros.Rows[cylinder].Cells[1].Value.ToString();
                            string cylinderDescription = data.DgvCilindros.Rows[cylinder].Cells[3].Value.ToString();
                            fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{cylinderNumber}{templateName}";

                            content = File.ReadAllText(FBTemplate)
                                .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                .Replace("[numero_cilindro]", cylinderNumber)
                                .Replace("[descricao_cilindro]", cylinderDescription)
                                .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                            destinationPath = Path.Combine(destinationFolder, fileName);
                            File.WriteAllText(destinationPath, content);

                            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                        }
                    }
                    continue;
                }
                // 4.4 Cilindros com Ventosa (Sauger)
                else if (templateName == "#FB_Ventil_Sauger_DB.xml" && data.CylindersAmount > 0)
                {
                    for (int cylinder = 0; cylinder < data.CylindersAmount; cylinder++)
                    {
                        if (data.DgvCilindros.Rows[cylinder].Cells[4].Value.ToString().ToUpper() == "SAUGER")
                        {
                            string cylinderNumber = data.DgvCilindros.Rows[cylinder].Cells[1].Value.ToString();
                            string cylinderDescription = data.DgvCilindros.Rows[cylinder].Cells[3].Value.ToString();
                            fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{cylinderNumber}{templateName}";

                            content = File.ReadAllText(FBTemplate)
                                .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                .Replace("[numero_cilindro]", cylinderNumber)
                                .Replace("[descricao_cilindro]", cylinderDescription)
                                .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                            destinationPath = Path.Combine(destinationFolder, fileName);
                            File.WriteAllText(destinationPath, content);

                            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                        }
                    }
                    continue;
                }
                // Bloco 5: Geração de DBs de Instância para Sensores
                // =================================================================
                else if (templateName == "#FB_Bauteilkontrolle_DB.xml")
                {
                    for (int sensor = 0; sensor < data.SensorsAmount; sensor++)
                    {
                        string sensorName = data.DgvSensores.Rows[sensor].Cells[0].Value.ToString();
                        string sensorIndex = data.DgvSensores.Rows[sensor].Cells[1].Value.ToString().Trim();
                        string sensorDescription = data.DgvSensores.Rows[sensor].Cells[2].Value.ToString().Trim();

                        if (string.IsNullOrEmpty(sensorIndex)) // Sensor sem índice
                        {
                            fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{sensorName}{templateName}";
                            content = File.ReadAllText(FBTemplate)
                                .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                .Replace("[numero_sensor]", sensorName)
                                .Replace("[descricao_sensor]", sensorDescription)
                                .Replace("[indice_sensor]", "")
                                .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                            destinationPath = Path.Combine(destinationFolder, fileName);
                            File.WriteAllText(destinationPath, content);
                            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                        }
                        else // Sensor com múltiplos índices (ex: a/b)
                        {
                            string[] indexList = sensorIndex.Split('/');
                            char startIndex = indexList[0][0];
                            char endIndex = indexList[1][0];

                            if (endIndex < startIndex) (startIndex, endIndex) = (endIndex, startIndex);

                            for (char index = startIndex; index <= endIndex; index++)
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{sensorName}{index.ToString().ToLower()}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                    .Replace("[numero_sensor]", sensorName)
                                    .Replace("[descricao_sensor]", sensorDescription)
                                    .Replace("[indice_sensor]", index.ToString())
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    continue;
                }
                // Bloco 6: Geração de DBs para Inversores (SEW, Elefant, Heber)
                // =================================================================
                else if (templateName == "#FB_SEW_AMX2_RB_2P_DB.xml" || templateName == "#FB_Elefant2_AMX_2P_DB.xml")
                {
                    // Apenas gera o arquivo se a estação for do tipo RB e houver inversores
                    if ((data.StationType.ToUpper() == "RB" || data.StationType.ToUpper() == "RB1") && data.InvertersAmount > 0)
                    {
                        for (int i = 0; i < data.InvertersAmount; i++)
                        {
                            string nomeDispositivo = data.DgvInversores.Rows[i].Cells[0].Value.ToString().Trim();
                            string descricaoDispositivo = data.DgvInversores.Rows[i].Cells[1].Value.ToString().Trim();

                            if (!string.IsNullOrEmpty(nomeDispositivo) && descricaoDispositivo.ToUpper() == "ROLLENBAHN")
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{nomeDispositivo}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                    .Replace("[nome_dispositivo]", nomeDispositivo)
                                    .Replace("[descricao_dispositivo]", descricaoDispositivo)
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    // O continue garante que este template NUNCA será processado pela lógica genérica,
                    // mesmo se as condições acima não forem atendidas.
                    continue;
                }
                else if (templateName == "#FB_HubRemech_DB.xml" || templateName == "#FB_SEW_AMA_Bin_DB.xml")
                {
                    // Apenas processa se houver inversores na lista
                    if (data.InvertersAmount > 0)
                    {
                        for (int i = 0; i < data.InvertersAmount; i++)
                        {
                            string nomeDispositivo = data.DgvInversores.Rows[i].Cells[0].Value.ToString().Trim();
                            string descricaoDispositivo = data.DgvInversores.Rows[i].Cells[1].Value.ToString().Trim();
                            if (!string.IsNullOrEmpty(nomeDispositivo) && descricaoDispositivo.ToUpper() == "HEBER")
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{nomeDispositivo}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                    .Replace("[nome_dispositivo]", nomeDispositivo)
                                    .Replace("[descricao_dispositivo]", descricaoDispositivo)
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    // O continue garante que este template não será processado pela lógica genérica
                    continue;
                }
                // Novo Bloco  Geração de DBs para Módulos SEW
                else if (templateName == "#FB_SEW_Modulo_DB.xml")
                {
                    // Apenas gera o arquivo se a estação for do tipo DT1 e houver inversores
                    if (data.StationType.ToUpper() == "DT1" && data.InvertersAmount > 0)
                    {
                        for (int i = 0; i < data.InvertersAmount; i++)
                        {
                            string nomeDispositivo = data.DgvInversores.Rows[i].Cells[0].Value.ToString().Trim();
                            string descricaoDispositivo = data.DgvInversores.Rows[i].Cells[1].Value.ToString().Trim();

                            // Verifica se a descrição é "DREHTISCH"
                            if (!string.IsNullOrEmpty(nomeDispositivo) && descricaoDispositivo.ToUpper() == "DREHTISCH")
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{nomeDispositivo}{templateName}";

                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_estacao]", $"{data.SKNumber}{data.StationNumber}{data.StationType}")
                                    .Replace("[nome_dispositivo]", nomeDispositivo)
                                    .Replace("[descricao_dispositivo]", descricaoDispositivo)
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    // O continue garante que este template NUNCA será processado pela lógica genérica.
                    continue;
                }


                // Bloco 7: Lógica genérica para templates que não se encaixam nas categorias acima
                // =================================================================
                if (templateName == "#FB_Status_DB.xml")
                {
                    fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}_FM{templateName}";
                }
                else
                {
                    fileName = $"{data.SKNumber}{data.StationNumber}{data.StationType}{templateName}";
                }

                content = File.ReadAllText(FBTemplate);

                string nomeEstacao = (data.StationType == "WZ")
                    ? $"{data.SKNumber}{data.StationNumber}_WZ"
                    : $"{data.SKNumber}{data.StationNumber}{data.StationType}";

                content = content.Replace("[nome_estacao]", nomeEstacao)
                                 .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                destinationPath = Path.Combine(destinationFolder, fileName);
                File.WriteAllText(destinationPath, content);

                if (data.DBInstanzenNumber < 20000)
                {
                    data.DBInstanzenNumber++;
                }
            }
        }
    }
}
