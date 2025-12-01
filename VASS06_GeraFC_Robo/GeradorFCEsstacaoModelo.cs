using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VASS06_GeraFC
{
    public static class GeradorFC
    {
        public static void Gerar(ref EstacaoData data)
        {
            // Bloco 1: Preparação inicial do arquivo FC
            // =================================================================
            string FCTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/FC", "Empty_FC_Template.xml");
            string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export/FC", $"FC{data.FCNumber}-{data.SKNumber}{data.StationNumber}{data.StationType}.xml");
            string emptyFCContent = File.ReadAllText(FCTemplatePath);

            string stationType = data.StationType;
            if (string.IsNullOrEmpty(stationType)) stationType = "V01";

            string fcContent = emptyFCContent.Replace("[nome_FC]", $"{data.SKNumber}{data.StationNumber}_{stationType}")
                                             .Replace("[numero_FC]", data.FCNumber.ToString());

            string nomeDbStba = data.IsSubBA ? $"{data.SKNumber}{data.StationNumber}" : data.SKNumber;

            // Bloco 2: Inicialização de variáveis para a geração das Networks
            // =================================================================
            int idCounter = 30; // Contador para garantir IDs únicos no XML
            string stationName = $"{data.SKNumber}{data.StationNumber}";
            string fullStationName = $"{data.SKNumber}{data.StationNumber}{stationType}";
            string[] contatos;
            string[] tiposContatos;
            string tagBobina;
            string tipoBobina;
            string networkContent;

            // Bloco 3: Processamento e Lógica dos Sensores (KTs)
            // =================================================================
            HashSet<string> sensoresSemIndice = new HashSet<string>();
            HashSet<string> sensoresComIndice = new HashSet<string>();
            HashSet<string> KTList = new HashSet<string>();
            foreach (DataGridViewRow row in data.DgvSensores.Rows)
            {
                if (row.IsNewRow) continue;
                if (string.IsNullOrEmpty(row.Cells[1].Value?.ToString()))
                {
                    sensoresSemIndice.Add($"{stationName}{stationType}{row.Cells[0].Value}");
                }
                else
                {
                    string sensor = row.Cells[0].Value.ToString();
                    KTList.Add(sensor.Substring(sensor.Length - 2));
                    string sensorIndex = row.Cells[1].Value.ToString();
                    char startIndex = sensorIndex.Split('/')[0][0];
                    char finalIndex = sensorIndex.Split('/')[1][0];
                    for (char index = startIndex; index <= finalIndex; index++)
                    {
                        sensoresComIndice.Add($"{stationName}{stationType}{sensor}{index}");
                    }
                }
            }
            string[] sensoresSemIndiceArray = sensoresSemIndice.ToArray();
            string[] sensoresComIndiceArray = sensoresComIndice.ToArray();
            string[] KTListArray = KTList.ToArray();

            // NETWORK: Somatória Controle de Peça por KT individual (Ocupado/Livre)
            // =================================================================
            for (int i = 0; i < KTListArray.Length; i++)
            {
                var sensorsOnKT = sensoresComIndiceArray.Where(s => KTListArray[i] == s.Substring(s.Length - 3, 2)).ToArray();

                // KTxx Ocupado
                contatos = sensorsOnKT;
                tiposContatos = Enumerable.Repeat("NA", sensorsOnKT.Length).ToArray();
                tagBobina = $"{stationName}{stationType}.KT{KTListArray[i]}";
                tipoBobina = "Coil";
                networkContent = Criar_Network($"Somatória Controle de Peça KT{KTListArray[i]} - Ocupado", contatos, tiposContatos, tagBobina, tipoBobina, ref idCounter);
                fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

                // KTxx Livre
                contatos = sensorsOnKT.Concat(new[] { "DB_ARG.Bus_OK" }).ToArray();
                tiposContatos = Enumerable.Repeat("NF", sensorsOnKT.Length).Concat(new[] { "NA" }).ToArray();
                tagBobina = $"{stationName}{stationType}./KT{KTListArray[i]}";
                networkContent = Criar_Network($"Somatória Controle de Peça KT{KTListArray[i]} - Livre", contatos, tiposContatos, tagBobina, tipoBobina, ref idCounter);
                fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            }

            // NETWORK: Somatória Controle de Peça GERAL (Ocupado/Livre)
            // =================================================================
            string[] nomeCompletoKT = KTListArray.Select(kt => $"{stationName}{stationType}.KT{kt}").ToArray();
            string[] totalContatos = nomeCompletoKT.Concat(sensoresSemIndiceArray).ToArray();

            // KT Ocupado Geral
            contatos = totalContatos;
            tiposContatos = Enumerable.Repeat("NA", totalContatos.Length).ToArray();
            tagBobina = $"{stationName}{stationType}.KT";
            tipoBobina = "Coil";
            networkContent = Criar_Network("Somatória Controle de Peça - Ocupado", contatos, tiposContatos, tagBobina, tipoBobina, ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // KT Livre Geral
            contatos = totalContatos.Concat(new[] { "DB_ARG.Bus_OK" }).ToArray();
            tiposContatos = Enumerable.Repeat("NF", totalContatos.Length).Concat(new[] { "NA" }).ToArray();
            tagBobina = $"{stationName}{stationType}./KT";
            networkContent = Criar_Network("Somatória Controle de Peça - Livre", contatos, tiposContatos, tagBobina, tipoBobina, ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // NETWORK: Perfil Livre (PF0 e PF) e Manutenção
            // =================================================================
            networkContent = Criar_Network("Perfil Livre", new[] { "Info_Marcador" }, new[] { "NA" }, $"{stationName}{stationType}.PF0", "Coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            networkContent = Criar_Network("Perfil Livre para Ferramenta", new[] { "Info_Marcador" }, new[] { "NA" }, $"{stationName}{stationType}.PF", "Coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            networkContent = Criar_Network("Manutenção solicitada/ativa", new[] { "Info_Marcador" }, new[] { "NA" }, $"{stationName}{stationType}.WartAng", "Coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // NETWORK: Posições da Estação (Básica e Avançada)
            // =================================================================
            var kerContacts = new List<string>();
            var kevContacts = new List<string>();
            for (int i = 0; i < data.CylindersAmount; i++)
            {
                kerContacts.Add($"{stationName}{stationType}.{data.DgvCilindros.Rows[i].Cells[1].Value}.KER");
                kevContacts.Add($"{stationName}{stationType}.{data.DgvCilindros.Rows[i].Cells[1].Value}.KEV");
            }
            kerContacts.Add("Info_Marcador");
            kevContacts.Add("Info_Marcador");

            networkContent = Criar_Network("Estação em posição básica", kerContacts.ToArray(), Enumerable.Repeat("NA", kerContacts.Count).ToArray(), $"{stationName}{stationType}.K10", "Coil", ref idCounter, comment: "Conferir posições dos cilindros com Plano P / Eplan");
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            networkContent = Criar_Network("Estação avançada", kevContacts.ToArray(), Enumerable.Repeat("NA", kevContacts.Count).ToArray(), $"{stationName}{stationType}.KSP-V", "Coil", ref idCounter, comment: "Conferir posições dos cilindros com Plano P / Eplan");
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // NETWORK: Lógica de Fim de Trabalho (FM)
            // =================================================================
            contatos = new[] { $"{stationName}{stationType}.K10", $"{stationName}{stationType}./KT", $"{stationName}{stationType}.PF" };
            tiposContatos = new[] { "NA", "NA", "NA" };
            networkContent = Criar_Network("Reset Fim de Trabalho Geral", contatos, tiposContatos, $"{stationName}{stationType}.FMReset", "Coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            if (data.IsSubBA)
            {
                contatos = new[] { $"{stationName}{stationType}.K10", $"{stationName}{stationType}.KT", $"{stationName}SF1.AE1.KEV", "Info_Marcador" };
            }
            else
            {
                contatos = new[] { $"{stationName}{stationType}.KSP-V", $"{stationName}{stationType}.KT", $"{stationName}{stationType}.PF", "Info_Marcador" };
            }
            networkContent = Criar_Network("Fim de Trabalho Geral", contatos, Enumerable.Repeat("NA", contatos.Length).ToArray(), $"{stationName}{stationType}.FM", "SCoil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Bloco - Sinalização FM
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Status_DB_Template.xml", "Sinalização fim de trabalho geral", 3, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // NETWORK: Reset e Inversão de FM
            // =================================================================
            networkContent = Criar_Network("Reset Fim de Trabalho", new[] { $"{stationName}{stationType}.FMReset" }, new[] { "NA" }, $"{stationName}{stationType}.FM", "RCoil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            networkContent = Criar_Network("Sem Nenhum Fim de Trabalho", new[] { $"{stationName}{stationType}.FM" }, new[] { "NF" }, $"{stationName}{stationType}./FM", "Coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Bloco - Status da Estação
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_ST_Status_DB_Template.xml", "Status Estação", 13, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, numero_SK: data.SKNumber);
            networkContent = networkContent.Replace("[K6Tag]", data.IsSubBA ? "BA.K6_TE" : "K6_TE").Replace("[NOME_DB_ST_BA]", nomeDbStba);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Bloco - Inversores
            // =================================================================
            if (stationType.ToUpper() == "RB" || stationType.ToUpper() == "RB1" || stationType.ToUpper() == "DT1")
            {
                foreach (DataGridViewRow row in data.DgvInversores.Rows)
                {
                    string nomeDispositivo = row.Cells[0].Value.ToString().Trim();
                    string desc = row.Cells[1].Value.ToString().Trim();
                    if (string.IsNullOrEmpty(nomeDispositivo)) continue;

                    if (desc.ToUpper() == "ROLLENBAHN")
                    {
                        networkContent = Criar_Bloco("NW_FB_Elefant2_AMX_2P_DB_Template.xml", $"{desc} {fullStationName}{nomeDispositivo}", 63, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent.Replace("[nome_dispositivo]", nomeDispositivo).Replace("[NOME_DB_ST_BA]", nomeDbStba) + Environment.NewLine + "[proxima_network]");

                        networkContent = Criar_Bloco("NW_FB_SEW_AMX2_RB_2P_Template.xml", $"Ansteuerung FU {fullStationName}{nomeDispositivo}", 34, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent.Replace("[nome_dispositivo]", nomeDispositivo).Replace("[NOME_DB_ST_BA]", nomeDbStba) + Environment.NewLine + "[proxima_network]");
                        if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                    }
                    else if (desc.ToUpper() == "HEBER")
                    {
                        networkContent = Criar_Bloco("NW_FB_HubRemech_DB_Template.xml", $"Ansteuerung Heber Remech {fullStationName}{nomeDispositivo}", 30, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent.Replace("[nome_dispositivo]", nomeDispositivo).Replace("[NOME_DB_ST_BA]", nomeDbStba) + Environment.NewLine + "[proxima_network]");

                        networkContent = Criar_Bloco("NW_FB_SEW_AMA_Bin_DB_Template.xml", $"Heber {nomeDispositivo}", 46, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent.Replace("[nome_dispositivo]", nomeDispositivo).Replace("[NOME_DB_ST_BA]", nomeDbStba) + Environment.NewLine + "[proxima_network]");
                        if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                    }
                    // NOVO BLOCO: Para Módulos SEW (Drehtisch)
                    else if (desc.ToUpper() == "DREHTISCH")
                    {
                        networkContent = Criar_Bloco("NW_FB_SEW_Modulo_DB_Template.xml", $"SEW Modulo {fullStationName}{nomeDispositivo}", 41, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent.Replace("[nome_dispositivo]", nomeDispositivo).Replace("[NOME_DB_ST_BA]", nomeDbStba) + Environment.NewLine + "[proxima_network]");
                        if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                    }
                }
            }

            // NETWORK: Fim de Ciclo (K6_TE)
            // =================================================================
            networkContent = Criar_Network("Fim de Ciclo", new[] { $"{stationName}{stationType}.PF", $"{stationName}{stationType}.FM" }, new[] { "NA", "NA" }, $"{stationName}{stationType}.K6_TE", "Coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Bloco - Lógicas de Operador (se SubBA)
            // =================================================================
            if (data.IsSubBA)
            {
                foreach (DataGridViewRow row in data.DgvSeguranca.Rows)
                {
                    if (row.Cells[0].Value.ToString() == "SF")
                    {
                        string sfName = row.Cells[1].Value.ToString();
                        // Janela - Segurança para Abrir
                        networkContent = Criar_Network("Janela de Segurança - Segurança para Abrir", new[] { $"{stationName}{stationType}.K91_Verkl", $"{stationName}{stationType}.BA.KWE7" }, new[] { "NF", "NA" }, $"{stationName}{sfName}.AE1.Ver_R1", "Coil", ref idCounter);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
                        // Janela - Liberação para Fechar
                        networkContent = Criar_Network("Janela de Segurança - Liberação par Fechar", new[] { $"{stationName}{stationType}.KT", $"{stationName}{stationType}.K10", $"{stationName}{stationType}.PF", $"{stationName}{sfName}.AE1.KEV" }, new[] { "NA", "NA", "NA", "NF" }, $"{stationName}{sfName}.AE1.Frg_V1", "Coil", ref idCounter);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
                        // Janela - Liberação para Abrir
                        networkContent = Criar_Network("Janela de Segurança - Liberação para Abrir", new[] { $"{stationName}{stationType}.K91_Verkl", $"{stationName}{sfName}I01{sfName}V", $"{data.SKNumber}.BA.K93_AnwLeer", $"{data.SKNumber}.BA.K92_PoT", $"{stationName}{sfName}.AE1.KER" }, new[] { "NF", "NF", "NF", "NF", "NF" }, $"{stationName}{sfName}.AE1.Frg_R1", "Coil", ref idCounter);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
                        // Bloco - Janela de Segurança
                        networkContent = Criar_Bloco("NW_FB_Rolltor_DB_Template.xml", "Janela de Segurança", 37, stationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, SFName: sfName);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
                    }
                }
                // Bloco - MIA (Sinalização de Operador)
                networkContent = Criar_Bloco("NW_FB_MIA_DB_Template.xml", "Sinalização de Operador", 17, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
                fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
                // Bloco - Werkerruf (Chamada de Operador)
                networkContent = Criar_Bloco("NW_FB_Ruf_DB_Template.xml", "Chamada de Operador", 26, stationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
                fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            }

            // Bloco - Válvula Piloto (Ilhas de Válvula)
            // =================================================================
            var valvesList = new HashSet<string>();
            for (int i = 0; i < data.CylindersAmount; i++)
            {
                string viNumber = data.DgvCilindros.Rows[i].Cells[0]?.Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(viNumber)) valvesList.Add(viNumber);
            }
            foreach (string vi in valvesList)
            {
                networkContent = Criar_Bloco("NW_FB_Ventil_01E_DB_Template.xml", "Válvula Piloto", 12, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, numero_VI: vi);
                fcContent = fcContent.Replace("[proxima_network]", networkContent.Replace("[NOME_DB_ST_BA]", nomeDbStba) + Environment.NewLine + "[proxima_network]");
            }

            // Bloco - Cilindros
            // =================================================================
            for (int i = 0; i < data.CylindersAmount; i++)
            {
                string valve = data.DgvCilindros.Rows[i].Cells[0].Value.ToString();
                string cylinder = data.DgvCilindros.Rows[i].Cells[1].Value.ToString();
                string indexCylinder = data.DgvCilindros.Rows[i].Cells[2].Value.ToString();
                string cylinderName = data.DgvCilindros.Rows[i].Cells[3].Value.ToString();
                string cylinderType = data.DgvCilindros.Rows[i].Cells[4].Value.ToString().ToUpper();
                string templateFile = "NW_FB_Ventil_DB_Template.xml";
                int tagAmount = 51;

                if (cylinderType == "HAND") { templateFile = "NW_FB_Ventil_Hand_DB_Template.xml"; tagAmount = 56; }
                else if (cylinderType == "SAUGER") { templateFile = "NW_FB_Ventil_Sauger_DB_Template.xml"; tagAmount = 45; }
                else if (cylinderType == "300") { templateFile = "NW_FB_Ventil_300_DB_Template.xml"; tagAmount = 59; }

                networkContent = Criar_FB_Ventil(templateFile, $"{cylinderName} {fullStationName}{cylinder} {indexCylinder}", tagAmount, fullStationName, "1500", data.DBAnwenderNumber.ToString(), valve, cylinder, indexCylinder, ref idCounter);
                fcContent = fcContent.Replace("[proxima_network]", networkContent.Replace("[NOME_DB_ST_BA]", nomeDbStba) + Environment.NewLine + "[proxima_network]");
            }

            // Bloco - Sensores (Bauteilkontrolle)
            // =================================================================
            for (int i = 0; i < data.SensorsAmount; i++)
            {
                string sensorNumber = $"{stationType}{data.DgvSensores.Rows[i].Cells[0].Value}";
                string sensorIndex = data.DgvSensores.Rows[i].Cells[1].Value.ToString();
                string sensorDescription = data.DgvSensores.Rows[i].Cells[2].Value.ToString();

                if (string.IsNullOrEmpty(sensorIndex))
                {
                    networkContent = Criar_Bloco("NW_FB_Bauteilkontrolle_DB_Template.xml", sensorDescription, 10, stationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, sensorNumber: sensorNumber, sensorIndex: "", sensorIndexFailure: "", sensorName: sensorDescription);
                    fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
                }
                else
                {
                    char startIndex = sensorIndex.Split('/')[0][0];
                    char finalIndex = sensorIndex.Split('/')[1][0];
                    for (char index = startIndex; index <= finalIndex; index++)
                    {
                        // A lógica original tinha uma falha aqui, simplificando para criar um bloco para cada índice.
                        networkContent = Criar_Bloco("NW_FB_Bauteilkontrolle_DB_Template.xml", sensorDescription, 10, stationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, sensorNumber: sensorNumber, sensorIndex: index.ToString(), sensorIndexFailure: index.ToString(), sensorName: sensorDescription);
                        fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
                    }
                }
            }

            // Bloco - Tempo de Ciclo (Taktzeit)
            // =================================================================
            networkContent = Criar_Network("Tempo de Ciclo - Stop", new[] { $"{stationName}{stationType}.PF", $"{stationName}{stationType}.FM" }, new[] { "NA", "NA" }, $"{stationName}{stationType}.TZStop", "Coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            networkContent = Criar_Bloco("NW_FB_Taktzeit_Plus_DB_Template.xml", "Tempo de Ciclo", 15, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, numero_SK: data.SKNumber);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Bloco - Status Geral da Estação (Sammelstatus)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Sammelstatus_DB_Template.xml", "Status Geral da Estação", 6, fullStationName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent);

            // Bloco Final: Salvar o arquivo FC completo
            // =================================================================
            try
            {
                Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export/FC"));
                File.WriteAllText(destinationPath, fcContent);
                if (data.FCNumber < 99) data.FCNumber++;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar o FC: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static string Criar_Network(string title, string[] tagsContacts, string[] contactTypes, string tagCoil, string coilType, ref int idCounter, string comment = "")
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string NWTemplatePath = File.ReadAllText(Path.Combine(basePath, "Resources/FC", "Empty_NW_Template.xml"));
            string TagTemplate = File.ReadAllText(Path.Combine(basePath, "Resources/FC", "Tag_Template.xml"));
            string naElementTemplate = File.ReadAllText(Path.Combine(basePath, "Resources/FC", "Element_NA_Template.xml"));
            string nfElementTemplate = File.ReadAllText(Path.Combine(basePath, "Resources/FC", "Element_NF_Template.xml"));
            string PowerRailTemplate = File.ReadAllText(Path.Combine(basePath, "Resources/FC", "Wire_PW_Template.xml"));
            string OutInTemplate = File.ReadAllText(Path.Combine(basePath, "Resources/FC", "Wire_OutIn_Template.xml"));
            string IdentTemplate = File.ReadAllText(Path.Combine(basePath, "Resources/FC", "Wire_Identification_Template.xml"));

            StringBuilder tagsBuilder = new StringBuilder();
            StringBuilder elementosBuilder = new StringBuilder();
            StringBuilder identificacoesBuilder = new StringBuilder();
            StringBuilder conexoesBuilder = new StringBuilder();

            string powerRailWireId = idCounter++.ToString();
            string primeiroContatoId = idCounter++.ToString();
            string powerRailWire = PowerRailTemplate.Replace("[ID_unico]", powerRailWireId).Replace("[ID_primeira_conexao]", primeiroContatoId);

            List<string> elemIds = new List<string>();
            List<string> tagIds = new List<string>();

            for (int i = 0; i < tagsContacts.Length; i++)
            {
                string tagId = idCounter++.ToString();
                string elemId = (i == 0) ? primeiroContatoId : idCounter++.ToString();
                elemIds.Add(elemId);
                tagIds.Add(tagId);

                string[] partes = tagsContacts[i].Split('.');
                string componentePrincipal = partes[0];
                string componenteSecundario = partes.Length >= 2 ? partes[1] : "";
                string componenteTerciario = partes.Length >= 3 ? partes[2] : "";

                string tag = TagTemplate.Replace("[ID_unico]", tagId).Replace("[componente_principal]", componentePrincipal);

                if (!string.IsNullOrEmpty(componenteSecundario))
                {
                    tag = tag.Replace("[componente_secundario]", componenteSecundario);
                }
                else
                {
                    tag = string.Join(Environment.NewLine, tag.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(linha => !linha.Contains("[componente_secundario]")));
                }

                if (!string.IsNullOrEmpty(componenteTerciario))
                {
                    tag = tag.Replace("[componente_terciario]", componenteTerciario);
                }
                else
                {
                    tag = string.Join(Environment.NewLine, tag.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(linha => !linha.Contains("[componente_terciario]")));
                }

                tagsBuilder.AppendLine(tag);

                string contactType = contactTypes[i].ToUpper();
                string elementTemplate = contactType == "NF" ? nfElementTemplate : naElementTemplate;

                string elemento = elementTemplate.Replace("[ID_unico]", elemId).Replace("[tipo_de_elemento]", "Contact");
                elementosBuilder.AppendLine(elemento);

                string wireIdent = IdentTemplate.Replace("[ID_unico]", idCounter++.ToString()).Replace("[ID_elemento]", elemId).Replace("[ID_tag]", tagId);
                identificacoesBuilder.AppendLine(wireIdent);

                if (i > 0)
                {
                    string wireId = idCounter++.ToString();
                    string wire = OutInTemplate.Replace("[ID_unico]", wireId).Replace("[ID_elemento_saida]", elemIds[i - 1]).Replace("[ID_elemento_entrada]", elemIds[i]);
                    conexoesBuilder.AppendLine(wire);
                }
            }

            string tagBobinaId = idCounter++.ToString();
            string bobinaId = idCounter++.ToString();
            string[] partesBobina = tagCoil.Split('.');
            string compPrincipal = partesBobina[0];
            string compSecundario = partesBobina.Length >= 2 ? partesBobina[1] : "";
            string compTerciario = partesBobina.Length >= 3 ? partesBobina[2] : "";
            string tagBobinaXml = TagTemplate.Replace("[ID_unico]", tagBobinaId).Replace("[componente_principal]", compPrincipal);

            if (!string.IsNullOrEmpty(compSecundario))
            {
                tagBobinaXml = tagBobinaXml.Replace("[componente_secundario]", compSecundario);
            }
            else
            {
                tagBobinaXml = string.Join(Environment.NewLine, tagBobinaXml.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(linha => !linha.Contains("[componente_secundario]")));
            }

            if (!string.IsNullOrEmpty(compTerciario))
            {
                tagBobinaXml = tagBobinaXml.Replace("[componente_terciario]", compTerciario);
            }
            else
            {
                tagBobinaXml = string.Join(Environment.NewLine, tagBobinaXml.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Where(linha => !linha.Contains("[componente_terciario]")));
            }

            tagsBuilder.AppendLine(tagBobinaXml);
            string elementoBobina = naElementTemplate.Replace("[ID_unico]", bobinaId).Replace("[tipo_de_elemento]", coilType);
            elementosBuilder.AppendLine(elementoBobina);
            string identificacaoBobina = IdentTemplate.Replace("[ID_unico]", idCounter++.ToString()).Replace("[ID_elemento]", bobinaId).Replace("[ID_tag]", tagBobinaId);
            identificacoesBuilder.AppendLine(identificacaoBobina);

            string bobinaWireId = idCounter++.ToString();
            string outInFinal = OutInTemplate.Replace("[ID_unico]", bobinaWireId).Replace("[ID_elemento_saida]", elemIds.Last()).Replace("[ID_elemento_entrada]", bobinaId);
            conexoesBuilder.AppendLine(outInFinal);

            string networkContent = NWTemplatePath.Replace("[titulo_network]", title)
                                                  .Replace("[lista_de_tags]", tagsBuilder.ToString())
                                                  .Replace("[lista_de_elementos]", elementosBuilder.ToString())
                                                  .Replace("[powerrail]", powerRailWire)
                                                  .Replace("[identificacao]", identificacoesBuilder.ToString())
                                                  .Replace("[in_out]", conexoesBuilder.ToString());

            if (comment != "")
            {
                networkContent = networkContent.Replace("[comentario_network]", comment);
            }
            else
            {
                networkContent = networkContent.Replace("<Text>[comentario_network]</Text>", "");
            }

            while (networkContent.Contains("[ID_unico]"))
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_unico]", (idCounter++).ToString());
            }

            return networkContent;
        }

        private static string Criar_Bloco(string template, string title, int tagAmount, string stationName, string dbInstanzenNumber, string dbAnwenderNumber, ref int idCounter, string numero_SK = "", string numero_VI = "", string sensorNumber = "", string sensorIndex = "", string sensorIndexFailure = "", string sensorName = "", string SFName = "")
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string NWFBTemplatePath = File.ReadAllText(Path.Combine(basePath, "Resources/FC/Blocos", template));

            string networkContent = NWFBTemplatePath.Replace("[titulo_network]", title)
                                                    .Replace("[numero_DB_Anwender]", dbAnwenderNumber)
                                                    .Replace("[numero_DB_Instanzen]", dbInstanzenNumber);

            if (numero_SK != "") networkContent = networkContent.Replace("[numero_SK]", numero_SK);
            if (numero_VI != "") networkContent = networkContent.Replace("[numero_VI]", numero_VI);

            if (template == "NW_FB_Bauteilkontrolle_DB_Template.xml")
            {
                if (sensorIndexFailure == sensorIndex)
                {
                    networkContent = networkContent.Replace("[nome_estacao][numero_sensor][indice_falha]", "Info_Marcador");
                }
                if (sensorNumber != "" && sensorName != "")
                {
                    networkContent = networkContent.Replace("[numero_sensor]", sensorNumber)
                                                   .Replace("[indice]", sensorIndex)
                                                   .Replace("[indice_falha]", sensorIndexFailure)
                                                   .Replace("[nome_sensor]", sensorName);
                }
            }

            if (template == "NW_FB_Rolltor_DB_Template.xml" && SFName != "")
            {
                networkContent = networkContent.Replace("[numero_janela]", SFName);
            }

            networkContent = networkContent.Replace("[nome_estacao]", stationName);

            string[] tagsIDs = new string[tagAmount];
            for (int i = 0; i < tagsIDs.Length; i++)
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_tag]", (idCounter).ToString());
                tagsIDs[i] = idCounter.ToString();
                idCounter++;
            }
            for (int i = 0; i < tagsIDs.Length; i++)
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_tag]", tagsIDs[i]); ;
            }

            if (template == "NW_FB_MIA_DB_Template.xml")
            {
                networkContent = networkContent.Replace("[ID_tag_werker]", (idCounter++).ToString())
                                               .Replace("[ID_tag_SF1V]", (idCounter++).ToString())
                                               .Replace("[ID_tag_KR12]", (idCounter++).ToString())
                                               .Replace("[ID_tag_SF1R]", (idCounter++).ToString())
                                               .Replace("[ID_conexao]", (idCounter++).ToString());
            }

            if (template == "NW_FB_Taktzeit_Plus_DB_Template.xml")
            {
                networkContent = networkContent.Replace("[ID_contato_KT]", (idCounter++).ToString())
                                               .Replace("[ID_contato_PF]", (idCounter++).ToString())
                                               .Replace("[ID_contato_/FM]", (idCounter++).ToString());
            }

            networkContent = networkContent.Replace("[ID_bloco]", (idCounter++).ToString()).Replace("[ID_instancia]", (idCounter++).ToString());

            while (networkContent.Contains("[ID_unico]"))
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_unico]", (idCounter++).ToString());
            }
            return networkContent;
        }

        private static string Criar_FB_Ventil(string template, string title, int tagAmount, string stationName, string dbInstanzenNumber, string dbAnwenderNumber, string VI, string cylinder, string indexCylinders, ref int idCounter)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string NWFBTemplatePath = File.ReadAllText(Path.Combine(basePath, "Resources/FC/Blocos", template));
            int entradasRestantes = 8;

            string networkContent = NWFBTemplatePath.Replace("[titulo_network]", title)
                                                    .Replace("[numero_DB_Anwender]", dbAnwenderNumber)
                                                    .Replace("[numero_DB_Instanzen]", dbInstanzenNumber);

            if (indexCylinders != "")
            {
                char initialIndex = indexCylinders.Split('-')[0][0];
                char finalIndex = indexCylinders.Split('-')[1][0];
                List<char> indexList = new List<char>();
                for (char index = initialIndex; index <= finalIndex; index++) indexList.Add(index);

                foreach (char index in indexList)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_estacao]", stationName);
                    if (template == "NW_FB_Ventil_Sauger_DB_Template.xml")
                    {
                        networkContent = Utils.ReplaceFirst(networkContent, "[cilindro][index][posicao]", $"GS{cylinder.Substring(cylinder.Length - 2)}BP1");
                    }
                    else if (template == "NW_FB_Ventil_300_DB_Template.xml")
                    {
                        networkContent = Utils.ReplaceFirst(networkContent, "[cilindro]", $"BGE{cylinder.Substring(cylinder.Length - 2)}");
                        networkContent = Utils.ReplaceFirst(networkContent, "[index]", index.ToString().ToLower());
                        networkContent = Utils.ReplaceFirst(networkContent, "[posicao]", "R");
                    }
                    else
                    {
                        networkContent = Utils.ReplaceFirst(networkContent, "[cilindro]", $"BGE{cylinder.Substring(cylinder.Length - 2)}");
                        networkContent = Utils.ReplaceFirst(networkContent, "[numero_cilindro]", cylinder.Substring(cylinder.Length - 2));
                        networkContent = Utils.ReplaceFirst(networkContent, "[index]", index.ToString().ToLower());
                        networkContent = Utils.ReplaceFirst(networkContent, "[posicao]", "R");
                    }
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional_vke0]", "");
                    entradasRestantes--;
                }
                for (int i = 0; i < entradasRestantes; i++)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_estacao][cilindro][index][posicao]", "DB_ARG");
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional_vke0]", "<Component Name=\"VKE=0\" />");
                }
                entradasRestantes = 8;

                foreach (char index in indexList)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_estacao]", stationName);
                    if (template == "NW_FB_Ventil_Sauger_DB_Template.xml")
                    {
                        networkContent = Utils.ReplaceFirst(networkContent, "[cilindro][index][posicao]", $"GS{cylinder.Substring(cylinder.Length - 2)}BP1");
                    }
                    else if (template == "NW_FB_Ventil_300_DB_Template.xml")
                    {
                        networkContent = Utils.ReplaceFirst(networkContent, "[cilindro]", $"BGE{cylinder.Substring(cylinder.Length - 2)}");
                        networkContent = Utils.ReplaceFirst(networkContent, "[index]", index.ToString().ToLower());
                        networkContent = Utils.ReplaceFirst(networkContent, "[posicao]", "V");
                    }
                    else
                    {
                        networkContent = Utils.ReplaceFirst(networkContent, "[cilindro]", $"BGE{cylinder.Substring(cylinder.Length - 2)}");
                        networkContent = Utils.ReplaceFirst(networkContent, "[numero_cilindro]", cylinder.Substring(cylinder.Length - 2));
                        networkContent = Utils.ReplaceFirst(networkContent, "[index]", index.ToString().ToLower());
                        networkContent = Utils.ReplaceFirst(networkContent, "[posicao]", "V");
                    }
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional_vke0]", "");
                    entradasRestantes--;
                }
                for (int i = 0; i < entradasRestantes; i++)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_estacao][cilindro][index][posicao]", "DB_ARG");
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional_vke0]", "<Component Name=\"VKE=0\" />");
                }
            }
            else
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[nome_estacao]", stationName);
                if (template == "NW_FB_Ventil_Sauger_DB_Template.xml")
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[cilindro][index][posicao]", $"GS{cylinder.Substring(cylinder.Length - 2)}BP1");
                }
                else if (template == "NW_FB_Ventil_300_DB_Template.xml")
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[cilindro]", $"BGE{cylinder.Substring(cylinder.Length - 2)}");
                    networkContent = Utils.ReplaceFirst(networkContent, "[index]", "");
                    networkContent = Utils.ReplaceFirst(networkContent, "[posicao]", "R");
                }
                else
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[cilindro]", cylinder);
                    networkContent = Utils.ReplaceFirst(networkContent, "[index]", "");
                    networkContent = Utils.ReplaceFirst(networkContent, "[posicao]", "R");
                }
                networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional_vke0]", "");
                entradasRestantes--;
                for (int i = 0; i < entradasRestantes; i++)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_estacao][cilindro][index][posicao]", "DB_ARG");
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional_vke0]", "<Component Name=\"VKE=0\" />");
                }
                entradasRestantes = 8;

                networkContent = Utils.ReplaceFirst(networkContent, "[nome_estacao]", stationName);
                if (template == "NW_FB_Ventil_Sauger_DB_Template.xml")
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[cilindro][index][posicao]", $"GS{cylinder.Substring(cylinder.Length - 2)}BP1");
                }
                else if (template == "NW_FB_Ventil_300_DB_Template.xml")
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[cilindro]", $"BGE{cylinder.Substring(cylinder.Length - 2)}");
                    networkContent = Utils.ReplaceFirst(networkContent, "[index]", "");
                    networkContent = Utils.ReplaceFirst(networkContent, "[posicao]", "V");
                }
                else
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[cilindro]", cylinder);
                    networkContent = Utils.ReplaceFirst(networkContent, "[index]", "");
                    networkContent = Utils.ReplaceFirst(networkContent, "[posicao]", "V");
                }
                networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional_vke0]", "");
                entradasRestantes--;
                for (int i = 0; i < entradasRestantes; i++)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_estacao][cilindro][index][posicao]", "DB_ARG");
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional_vke0]", "<Component Name=\"VKE=0\" />");
                }
            }

            string mask = new string('1', 8 - entradasRestantes);
            networkContent = networkContent.Replace("[mascara]", mask);
            string n_cilindro = cylinder.Substring(cylinder.Length - 2);
            networkContent = networkContent.Replace("[nome_estacao]", stationName)
                .Replace("[cilindro]", cylinder)
                .Replace("[numero_cilindro]", n_cilindro)
                .Replace("[numero_VI]", VI);

            string[] tagsIDs = new string[tagAmount];
            for (int i = 0; i < tagsIDs.Length; i++)
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_tag]", (idCounter).ToString());
                tagsIDs[i] = idCounter.ToString();
                idCounter++;
            }
            for (int i = 0; i < tagsIDs.Length; i++)
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_tag]", tagsIDs[i]); ;
            }

            networkContent = networkContent.Replace("[ID_bloco]", (idCounter++).ToString())
                                           .Replace("[ID_instancia]", (idCounter++).ToString())
                                           .Replace("[ID_bitToByte_R]", (idCounter++).ToString())
                                           .Replace("[ID_bitToByte_V]", (idCounter++).ToString())
                                           .Replace("[ID_conexao]", (idCounter++).ToString())
                                           .Replace("[titulo_network]", title);

            while (networkContent.Contains("[ID_unico]"))
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_unico]", (idCounter++).ToString());
            }
            return networkContent;
        }
    }
}
