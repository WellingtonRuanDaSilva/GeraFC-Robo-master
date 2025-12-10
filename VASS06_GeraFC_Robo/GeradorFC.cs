using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VASS06_GeraFC_Robo
{
    public static class GeradorFC_Robo
    {
        public static void Gerar(ref RoboData data)
        {
            // Bloco 1: Preparação inicial do arquivo FC
            // =================================================================
            string FCTemplatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/FC", "Empty_FC_Template.xml");
            string destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Export/FC", $"FC{data.FCNumber}-{data.SKNumber}{data.StationNumber}{data.RobNumber}.xml");
            string emptyFCContent = File.ReadAllText(FCTemplatePath);

            string robNumber = data.RobNumber;
            if (string.IsNullOrEmpty(robNumber)) robNumber = "R01";

            string fcContent = emptyFCContent.Replace("[nome_FC]", $"{data.SKNumber}{data.StationNumber}_{robNumber}")
                                             .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}_{robNumber}")
                                             .Replace("[numero_FC]", data.FCNumber.ToString());


            // Bloco 2: Inicialização de variáveis para a geração das Networks
            // =================================================================
            int idCounter = 30; // Contador para garantir IDs únicos no XML
            string stationName = $"{data.SKNumber}{data.StationNumber}";
            string RobName = $"{data.SKNumber}{data.StationNumber}{robNumber}";
            string[] contatos;
            string[] tiposContatos;
            string tagBobina;
            string tipoBobina;
            string networkContent;


            // Bloco - Status da Entradas
            // =================================================================
            // Adicionado o parâmetro fcNumber: data.FCNumber.ToString()
            networkContent = Criar_Bloco("NW_FB_Rob_PN_A_DB_Template.xml", "Leer entradas", 4, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, numero_SK: data.SKNumber, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // NETWORK: Liberação de Folge
            // =================================================================
            for (int i = 0; i < data.folgesAmount; i++)
            {
                string numeroFolges = data.DgvFolges.Rows[i].Cells[0].Value.ToString().Trim();
                if (!string.IsNullOrEmpty(numeroFolges))
                {
                    networkContent = Criar_Network($"Liberación Folge {numeroFolges}", new[] { "Info_Marcador" }, new[] { "NA" }, $"{RobName}.FrgFolg{data.DgvFolges.Rows[i].Cells[0].Value}", "Coil", ref idCounter);
                    fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
                }
            }

            // Bloco - Folges
            // =================================================================
            List<string> linhas = new List<string>();

            foreach (DataGridViewRow row in data.DgvFolges.Rows)
            {
                if (!row.IsNewRow)
                {
                    string linha = row.Cells[0].Value?.ToString() ?? "";
                    if (!string.IsNullOrWhiteSpace(linha)) { 
                        linhas.Add(linha);
                    }
                }
            }

            string[] resultadoFolges = linhas.ToArray();
            networkContent = Criar_RobFolge("NW_FB_RobFolge_8_DB_Template.xml", "Construcción de Folgen", 20, RobName, "1500", data.DBAnwenderNumber.ToString(), resultadoFolges, ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Bloco - Typinfo (BIT_TO_BYTE)
            // =================================================================
            networkContent = Criar_Network_Personalizada(
                "NW_BIT_TO_BYTE_Template.xml",
                "Typ an Roboter",
                RobName,
                data.SKNumber,
                ref idCounter
            );
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Network - Liberação de Folge
            // =================================================================
            networkContent = Criar_Network("Liberação de Folge",
                                           new[] { "Info_Marcador", $"{RobName}.Rob.FrgFolge" },
                                           new[] { "NF", "NA" },
                                           $"{RobName}.FrgFolg",
                                           "Coil",
                                           ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Network - Folge Start
            // =================================================================
            List<string> contactsList = new List<string>();
            List<string> typesList = new List<string>();

            // 1. Primeiro contato: FrgFolg (NA)
            contactsList.Add($"{RobName}.FrgFolg");
            typesList.Add("NA");

            // 2. Loop para adicionar contato Fechado (NF) para cada FM da planilha
            foreach (DataGridViewRow row in data.DgvFMs.Rows)
            {
                if (row.IsNewRow) continue;
                string fmNum = row.Cells[0].Value?.ToString()?.Trim();
                // Verifica se existe número de FM definido
                if (!string.IsNullOrEmpty(fmNum))
                {
                    contactsList.Add($"{RobName}.FM{fmNum}");
                    typesList.Add("NF");
                }
            }

            // 3. Contato Fechado (NF) para o FM Geral
            contactsList.Add($"{RobName}.FM");
            typesList.Add("NF");

            // 4. Criação da Network acionando a bobina Rob.FolgeStart
            networkContent = Criar_Network("Inicio del Folge",
                                           contactsList.ToArray(),
                                           typesList.ToArray(),
                                           $"{RobName}.Rob.FolgeStart",
                                           "Coil",
                                           ref idCounter);

            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Network - Start de folge de manutenção
            // =================================================================
            networkContent = Criar_Network("Inicio del mantenimiento Folge",
                                           new[] { $"{RobName}.Rob.WartAng", $"{RobName}.Rob.A.A15_PF0", "Info_Marcador" },
                                           new[] { "NA", "NA", "NA" },
                                           $"{RobName}.Rob.W_FolgeStart",
                                           "Coil",
                                           ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");



            // Network - Segurança de maquina auxiliar
            // =================================================================
            // Utiliza o template NW_MaschSich_Aux_Template.xml
            networkContent = Criar_Network_Personalizada(
                "NW_MaschSich_Aux_Template.xml",
                "Seguridad de la máquina de liberación aux",
                RobName,
                data.SKNumber,
                ref idCounter
            );
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Network - Segurança de maquina
            // =================================================================
            networkContent = Criar_Network_Personalizada(
                "NW_MaschSich_Template.xml",
                "Seguridad de la máquina de liberación",
                RobName,
                data.SKNumber, // Passa o número SK para as tags globais K26, K25, etc.
                ref idCounter
            );
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Network - Ponte de Consistency check
            // =================================================================
            networkContent = Criar_Network("Comprobación de coherencia",
                                           new[] { "DB_ARG.VKE=1" },
                                           new[] { "NA" },
                                           $"{RobName}.Rob.UebFKonsi",
                                           "Coil",
                                           ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Bloco - Sistema do robo (FB_Rob)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Rob_DB_Template.xml", "Interfaz del sistema robótico", 40, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


            // Bloco - Correção de ponto de solda (FB_Rob_Korr)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Rob_Korr_DB_Template.xml", "El robot se detiene para corregir", 7, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;

            // Network - Seleção de manutenção 
            // =================================================================
            networkContent = Criar_Network("Selección de mantenimiento",
                                           new[] { $"{RobName}.Rob.WartExAng", "Info_Marcador" },
                                           new[] { "NA", "NA" },
                                           $"{RobName}.Rob.E.E18_Halt_WartBer",
                                           "Coil",
                                           ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Bloco - Fins de trabalho (FB_Rob_FM)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Rob_FM_DB_Template.xml", "Mensaje de fin de trabajo del robot", 19, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


            // Blocos - Status de Fins de trabalho (FB_Status_Glocal)
            // Cria um bloco para cada FM na planilha e um bloco final para o FM Geral
            // =================================================================

            // 1. Loop para criar blocos individuais (FM1, FM2, etc.)
            foreach (DataGridViewRow row in data.DgvFMs.Rows)
            {
                if (row.IsNewRow) continue;
                string fmNumber = row.Cells[0].Value?.ToString()?.Trim();
                string fmDesc = row.Cells[1].Value?.ToString()?.Trim();

                if (!string.IsNullOrEmpty(fmNumber))
                {
                    string title = $"Fin del trabajo {fmNumber} {fmDesc}";

                    // Gera o bloco base
                    networkContent = Criar_Bloco("NW_FB_Status_Global_DB_Template.xml", title, 3, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());

                    // Substitui a variável "FM" genérica pela específica (ex: FM1)
                    networkContent = networkContent.Replace("<Component Name=\"FM\" />", $"<Component Name=\"FM{fmNumber}\" />");

                    // Substitui o nome da instância "_FM1" pela específica (ex: _FM1, _FM2)
                    networkContent = networkContent.Replace("_FM1#FB_Status_Global_DB", $"_FM{fmNumber}#FB_Status_Global_DB");

                    fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

                    // Incrementa o DB de instância
                    if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                }
            }

            // 2. Status Global (FM Geral sem numeração)
            {
                string title = "Fin del trabajo general";
                networkContent = Criar_Bloco("NW_FB_Status_Global_DB_Template.xml", title, 3, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());

                // A variável no template já é "FM", então não precisamos alterar o Component Name.
                // Ajustamos apenas o nome da instância de "_FM1" para "_FM" (sem número) para evitar conflito e indicar Geral
                networkContent = networkContent.Replace("_FM1#FB_Status_Global_DB", "_FM#FB_Status_Global_DB");

                fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

                // Incrementa o DB de instância para este bloco extra
                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
            }

            // Bloco - Interlocks (FB_Rob_Frg_ver)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Rob_Frg_Ver_DB_Template.xml", "Enclavamiento de robots", 43, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


            // Blocos - Variaveis de saida (FB_Rob_Frg) Criar tres usando oss 3 templates Frg1 Frg2 e Frg3
            // =================================================================
            // Frg1
            networkContent = Criar_Bloco("NW_Frg1_FB_Rob_Frg_DB_Template.xml", "Perfiles/Posiciones Gratuitas", 13, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;

            // Frg2
            networkContent = Criar_Bloco("NW_Frg2_FB_Rob_Frg_DB_Template.xml", "Perfiles/Posiciones Gratuitas", 13, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;

            // Frg3
            networkContent = Criar_Bloco("NW_Frg3_FB_Rob_Frg_DB_Template.xml", "Perfiles/Posiciones Gratuitas", 13, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;

            // Network - Liberação
            // =================================================================
            networkContent = Criar_Network("Liberación",
                                           new[] { "Info_Marcador" },
                                           new[] { "NA" },
                                           $"{RobName}.E80",
                                           "Coil",
                                           ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Network - Livre/Ocupada
            // =================================================================
            networkContent = Criar_Network("Libre/Ocupado",
                                           new[] { "Info_Marcador" },
                                           new[] { "NA" },
                                           $"{RobName}.E57_KTV1",
                                           "Coil",
                                           ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Blocos - Variaveis de entrada (FB_Rob_StellFrg) Criar tres usando os 3 templates StellFrg1 StellFrg2 e StellFrg3
            // =================================================================
            // StellFrg1
            networkContent = Criar_Bloco("NW_StellFrg1_FB_Rob_StellFrg_DB_Template.xml", "Comunicados de posición", 17, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;

            // StellFrg2
            networkContent = Criar_Bloco("NW_StellFrg2_FB_Rob_StellFrg_DB_Template.xml", "Comunicados de posición", 17, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;

            // StellFrg3
            networkContent = Criar_Bloco("NW_StellFrg3_FB_Rob_StellFrg_DB_Template.xml", "Comunicados de posición", 17, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


            // Blocos - Ferramentas (Greifer - garra / Kleben - cola ...)
            // =================================================================
            int skCount = 1;
            int klCount = 1;
            int kwCount = 1;
            int gCount = 1;
            int msCount = 1;

            foreach (DataGridViewRow row in data.DgvFerramentas.Rows)
            {
                if (row.IsNewRow) continue;
                string toolName = row.Cells[0].Value?.ToString().Trim();
                if (string.IsNullOrEmpty(toolName)) continue;
                string toolUpper = toolName.ToUpper();

                string template = "";
                string title = "";
                int tags = 0;
                string instanceVar = ""; // O nome da instância (ex: KL1, G01)
                string placeholder = ""; // O texto padrão no template a ser substituído (ex: KL1)

                // Lógica de Seleção de Template
                if (toolUpper.Contains("GREIFER") || toolUpper.Contains("GARRA") || toolUpper.Contains("GRIPPER") || (toolUpper.StartsWith("G") && toolUpper.Length > 1 && char.IsDigit(toolUpper[1])))
                {
                    template = "NW_FB_Rob_Greifer_DB_Template.xml";
                    instanceVar = $"G{gCount:D2}"; // Formato G01, G02
                    title = $"Greifer {instanceVar}";
                    tags = 10;
                    placeholder = "G01";
                    gCount++;
                }
                else if (toolUpper.Contains("KLEB") || toolUpper.Contains("GLUE") || toolUpper.Contains("COLA") || toolUpper.StartsWith("KL"))
                {
                    template = "NW_FB_Rob_Kleben_DB_Templates.xml";
                    instanceVar = $"KL{klCount}";
                    title = $"Kleben {instanceVar}";
                    tags = 16;
                    placeholder = "KL1";
                    klCount++;
                }
                else if (toolUpper.Contains("SCHWEISS") || toolUpper.Contains("WELD") || toolUpper.Contains("SOLDA") || toolUpper.StartsWith("SK"))
                {
                    template = "NW_FB_Rob_Schweissen_DB_Template.xml";
                    instanceVar = $"SK{skCount}";
                    title = $"Schweißsteuerung {instanceVar}";
                    tags = 19;
                    placeholder = "SK1";
                    skCount++;
                }
                else if (toolUpper.Contains("KAPPEN") || toolUpper.Contains("WECHSLER") || toolUpper.StartsWith("KW"))
                {
                    template = "NW_FB_Rob_Kappenw_DB_Template.xml";
                    instanceVar = $"KW{kwCount}";
                    title = $"Prozess Kappenwechsler {instanceVar}";
                    tags = 15;
                    placeholder = "KW1";
                    kwCount++;
                }
                else if (toolUpper.Contains("MIG") || toolUpper.Contains("MAG") || toolUpper.StartsWith("MS"))
                {
                    template = "NW_FB_Rob_MIGMAG_DB_Template.xml";
                    instanceVar = $"MS{msCount}";
                    title = $"MIG-Löten {instanceVar}";
                    tags = 22;
                    placeholder = "MS1";
                    msCount++;
                }

                // Geração do Bloco
                if (!string.IsNullOrEmpty(template))
                {
                    networkContent = Criar_Bloco(template, title, tags, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());

                    // Substitui a referência específica da ferramenta no template pelo nome da instância atual
                    // Ex: Substitui todas as ocorrências de KL1 por KL2, ou G01 por G02
                    networkContent = networkContent.Replace(placeholder, instanceVar);

                    fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

                    if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                }
            }


            // Bloco - Medição (FB_RobMedien)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_RobMedien_DB_Template.xml", "Utilidades", 15, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


            // Bloco - Numero de erro (FB_Rob_FehlerNr)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Rob_FehlerNr_DB_Template.xml", "Número de falla del robot", 16, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


            // Network - Tempo de ciclo por tipo
            // =================================================================
            foreach (DataGridViewRow row in data.DgvFolges.Rows)
            {
                if (row.IsNewRow) continue;
                string folgeNum = row.Cells[0].Value?.ToString()?.Trim();

                // Gera uma network para cada Folge encontrada
                if (!string.IsNullOrEmpty(folgeNum))
                {
                    string templateFile = "NW_Taktzeit_Typ_Template.xml";
                    string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources/FC/Blocos", templateFile);

                    if (File.Exists(templatePath))
                    {
                        string content = File.ReadAllText(templatePath);

                        // 1. Substituições das variáveis principais
                        content = content.Replace("[nome_robo]", RobName)
                                         .Replace("[numero_folge]", folgeNum)
                                         .Replace("[numero_DB_Instanzen]", data.DBInstanzenNumber.ToString());

                        // 2. Substituição inteligente de IDs [ID_x] para garantir unicidade
                        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\[ID_\d+\]");
                        System.Text.RegularExpressions.MatchCollection matches = regex.Matches(content);
                        Dictionary<string, string> idMap = new Dictionary<string, string>();

                        foreach (System.Text.RegularExpressions.Match match in matches)
                        {
                            if (!idMap.ContainsKey(match.Value))
                            {
                                idMap[match.Value] = (idCounter++).ToString();
                            }
                        }
                        // Aplica os novos IDs no conteúdo
                        foreach (var pair in idMap)
                        {
                            content = content.Replace(pair.Key, pair.Value);
                        }

                        // 3. Adiciona a network ao arquivo final
                        fcContent = fcContent.Replace("[proxima_network]", content + Environment.NewLine + "[proxima_network]");

                        // 4. Incrementa o DB de Instância, pois cada bloco usa um DB próprio
                        if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                    }
                }
            }


            // Bloco - Tempo de ciclo (FB_Taktzeit_Plus)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Taktzeit_Plus_DB_Template.xml", "Tiempo de ciclo del robot", 10, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;



            // Bloco - Saida do robo (FB_Rob_PN_E)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Rob_PN_E_DB_Template.xml", "Escribir salida", 2, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;

            // Bloco - Status do robo (FB_Sammelstatus_Status_DB)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Sammelstatus_Status_DB_Template.xml", "Status Roboter", 6, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


            // Bloco - Status dos equipamentos (FB_Sammelstatus_End_DB)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Sammelstatus_End_DB_Template.xml", "Status Prozessgeräte", 6, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, fcNumber: data.FCNumber.ToString());
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


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


        private static string Criar_Bloco(string template, string title, int tagAmount, string RobName, string dbInstanzenNumber, string dbAnwenderNumber, ref int idCounter, string numero_SK = "", string fcNumber = "")
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string NWFBTemplatePath = File.ReadAllText(Path.Combine(basePath, "Resources/FC/Blocos", template));

            string networkContent = NWFBTemplatePath.Replace("[titulo_network]", title)
                                                    .Replace("[numero_DB_Anwender]", dbAnwenderNumber)
                                                    .Replace("[numero_DB_Instanzen]", dbInstanzenNumber);

            networkContent = networkContent.Replace("[nome_robo]", RobName);

            // Substituição da variável [numero_FC] se ela for passada e existir no template
            if (!string.IsNullOrEmpty(fcNumber))
            {
                networkContent = networkContent.Replace("[numero_FC]", fcNumber);
            }

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

            networkContent = networkContent.Replace("[ID_bloco]", (idCounter++).ToString()).Replace("[ID_instancia]", (idCounter++).ToString());

            while (networkContent.Contains("[ID_unico]"))
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_unico]", (idCounter++).ToString());
            }
            return networkContent;
        }

        private static string Criar_Network_Personalizada(string templateFile, string title, string robName, string skNumber, ref int idCounter)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string templatePath = Path.Combine(basePath, "Resources/FC/Blocos", templateFile);

            if (!File.Exists(templatePath))
                return "";

            string content = File.ReadAllText(templatePath);

            // Substituições de variáveis básicas
            content = content.Replace("[titulo_network]", title)
                             .Replace("[nome_robo]", robName)
                             .Replace("[numero_SK]", skNumber);

            // Substituição inteligente de IDs [ID_x]
            // Encontra todos os placeholders no formato [ID_0], [ID_1], etc.
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\[ID_\d+\]");
            System.Text.RegularExpressions.MatchCollection matches = regex.Matches(content);

            // Dicionário para mapear [ID_X] -> ID sequencial único
            Dictionary<string, string> idMap = new Dictionary<string, string>();

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (!idMap.ContainsKey(match.Value))
                {
                    idMap[match.Value] = (idCounter++).ToString();
                }
            }

            // Aplica as substituições dos IDs
            foreach (var pair in idMap)
            {
                content = content.Replace(pair.Key, pair.Value);
            }

            return content;
        }

        private static string Criar_RobFolge(string template, string title, int tagAmount, string robName, string dbInstanzenNumber, string dbAnwenderNumber, string[] folges, ref int idCounter, string fcNumber = "")
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string NWFBTemplatePath = File.ReadAllText(Path.Combine(basePath, "Resources/FC/Blocos", template));
            int entradasRestantes = 8;

            string networkContent = NWFBTemplatePath.Replace("[titulo_network]", title)
                                                    .Replace("[numero_DB_Anwender]", dbAnwenderNumber)
                                                    .Replace("[numero_DB_Instanzen]", dbInstanzenNumber);

            // Nova lógica para substituir [numero_FC]
            if (!string.IsNullOrEmpty(fcNumber))
            {
                networkContent = networkContent.Replace("[numero_FC]", fcNumber);
            }

            if (folges.Length > 0 && folges[0] != "")
            {
                foreach (string number in folges)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_robo]", robName);
                    networkContent = Utils.ReplaceFirst(networkContent, "[numero_folge]", number);
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional]", $"<Component Name=\"FrgFolg{number}\" />");
                    entradasRestantes--;
                }
                for (int i = 0; i < entradasRestantes; i++)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_robo]", "DB_ARG");
                    networkContent = Utils.ReplaceFirst(networkContent, "[numero_folge]", "0");
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional]", "<Component Name=\"VKE=0\" />");
                }
            }
            else
            {
                // Caso a lista de folges esteja vazia, preencher tudo com VKE=0 ou lógica padrão
                for (int i = 0; i < 8; i++)
                {
                    networkContent = Utils.ReplaceFirst(networkContent, "[nome_robo]", "DB_ARG");
                    networkContent = Utils.ReplaceFirst(networkContent, "[numero_folge]", "0");
                    networkContent = Utils.ReplaceFirst(networkContent, "[linha_adicional]", "<Component Name=\"VKE=0\" />");
                }
            }

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
                                           .Replace("[nome_bloco]", robName)
                                           .Replace("[nome_robo]", robName)
                                           .Replace("[titulo_network]", title);

            while (networkContent.Contains("[ID_unico]"))
            {
                networkContent = Utils.ReplaceFirst(networkContent, "[ID_unico]", (idCounter++).ToString());
            }
            return networkContent;
        }
    }
}

    
