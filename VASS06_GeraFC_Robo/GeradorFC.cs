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
            networkContent = Criar_Bloco("NW_FB_Rob_PN_A_DB_Template.xml", "Status Entradas", 4, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter, numero_SK: data.SKNumber);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // NETWORK: Liberação de Folge
            // =================================================================
            for (int i = 0; i < data.folgesAmount; i++)
            {
                string numeroFolges = data.DgvFolges.Rows[i].Cells[0].Value.ToString().Trim();
                if (!string.IsNullOrEmpty(numeroFolges))
                {
                    networkContent = Criar_Network($"Freigabe Folge {numeroFolges}", new[] { "Info_Marcador" }, new[] { "NA" }, $"{RobName}.FrgFolg{data.DgvFolges.Rows[i].Cells[0].Value}", "Coil", ref idCounter);
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
            networkContent = Criar_RobFolge("NW_FB_RobFolge_8_DB_Template.xml", "Bildung Folgen", 20, RobName, "1500", data.DBAnwenderNumber.ToString(), resultadoFolges, ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");


            // Network - Typinfo
            // =================================================================
            networkContent = Criar_Network("Typ and Roboter", new[] { "Info_Marcador" }, new[] { "NA" }, $"E233_240_Typinfo_SPS", "coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Network - Typinfo
            // =================================================================
            networkContent = Criar_Network("Typ and Roboter", new[] { "Info_Marcador" }, new[] { "NA" }, $"E233_240_Typinfo_SPS", "coil", ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");

            // Network - Folge Start
            // =================================================================


            // Network - Start de folge de manutenção
            // =================================================================


            // Network - Segurança de maquina auxiliar
            // =================================================================


            // Network - Segurança de maquina
            // =================================================================


            // Network - Ponte de Consistency check
            // =================================================================


            // Bloco - Sistema do robo (FB_Rob)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Rob_DB_Template.xml", "Sistema do Robô", 44, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;


            // Bloco - Correção de ponto de solda (FB_Rob_Korr)
            // =================================================================
            networkContent = Criar_Bloco("NW_FB_Rob_Korr_DB_Template.xml", "Correção de ponto de solda", 7, RobName, "1500", data.DBInstanzenNumber.ToString(), ref idCounter);
            fcContent = fcContent.Replace("[proxima_network]", networkContent + Environment.NewLine + "[proxima_network]");
            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;

            // Network - Seleção de manutenção 
            // =================================================================


            // Bloco - Fins de trabalho (FB_Rob_FM)
            // =================================================================


            // Blocos - Status de Fins de trabalho (FB_Status_Glocal) Quantos precisar por FM
            // =================================================================


            // Bloco - Interlocks (FB_Rob_Frg_ver)
            // =================================================================


            // Blocos - Variaveis de saida (FB_Rob_Frg)
            // =================================================================


            // Blocos - Variaveis de entrada (FB_Rob_StellFrg)
            // =================================================================


            // Blocos - Ferramentas (Greifer - garra / Kleben - cola ...)
            // =================================================================


            // Bloco - Medição (FB_RobMedien)
            // =================================================================


            // Bloco - Numero de erro (FB_Rob_FehlerNr)
            // =================================================================


            // Network - Tempo de ciclo por tipo
            // =================================================================


            // Network - Tempo de ciclo
            // =================================================================


            // Network - Tempo de ciclo
            // =================================================================


            // Bloco - Saida do robo (FB_Rob_PN_E)
            // =================================================================


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


        private static string Criar_Bloco(string template, string title, int tagAmount, string RobName, string dbInstanzenNumber, string dbAnwenderNumber, ref int idCounter, string numero_SK = "")
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string NWFBTemplatePath = File.ReadAllText(Path.Combine(basePath, "Resources/FC/Blocos", template));

            string networkContent = NWFBTemplatePath.Replace("[titulo_network]", title)
                                                    .Replace("[numero_DB_Anwender]", dbAnwenderNumber)
                                                    .Replace("[numero_DB_Instanzen]", dbInstanzenNumber);

            networkContent = networkContent.Replace("[nome_robo]", RobName);

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

        private static string Criar_RobFolge(string template, string title, int tagAmount, string robName, string dbInstanzenNumber, string dbAnwenderNumber, string[] folges, ref int idCounter)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string NWFBTemplatePath = File.ReadAllText(Path.Combine(basePath, "Resources/FC/Blocos", template));
            int entradasRestantes = 8;

            string networkContent = NWFBTemplatePath.Replace("[titulo_network]", title)
                                                    .Replace("[numero_DB_Anwender]", dbAnwenderNumber)
                                                    .Replace("[numero_DB_Instanzen]", dbInstanzenNumber);

            if (folges[0] != "")
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
                                           .Replace("[nome_bloco]", robName )
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

    
