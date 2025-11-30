using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace VASS06_GeraFC_Robo
{
    public static class GeradorDbInstancia
    {
        public static void Gerar(ref RoboData data)
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

                //Logica de Geração de ferramentas
                //==============================================

                //Geração de DB de garra
                if(templateName == "#FB_Rob_Greifer_DB.xml")
                {
                    if(data.toolsAmount > 0)
                    {
                        for (int i=0; i<data.toolsAmount; i++)
                        {
                            string nomeFerramenta = data.DgvFerramentas.Rows[i].Cells[0].Value.ToString().Trim();
                            if (!string.IsNullOrEmpty(nomeFerramenta) && nomeFerramenta.ToUpper().StartsWith("G0"))
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}{nomeFerramenta}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                    .Replace("[nome_ferramenta]", nomeFerramenta)
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    continue;
                }
                else if (templateName == "#FB_Rob_Kappenw_DB.xml")
                {
                    if (data.toolsAmount > 0)
                    {
                        for (int i = 0; i < data.toolsAmount; i++)
                        {
                            string nomeFerramenta = data.DgvFerramentas.Rows[i].Cells[0].Value.ToString();
                            if (!string.IsNullOrEmpty(nomeFerramenta) && nomeFerramenta.ToUpper().StartsWith("KW"))
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}{nomeFerramenta}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                    .Replace("[nome_ferramenta]", nomeFerramenta)
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    continue;
                }
                else if (templateName == "#FB_Rob_Kleben_DB.xml")
                {
                    if (data.toolsAmount > 0)
                    {
                        for (int i = 0; i < data.toolsAmount; i++)
                        {
                            string nomeFerramenta = data.DgvFerramentas.Rows[i].Cells[0].Value.ToString().Trim();
                            if (!string.IsNullOrEmpty(nomeFerramenta) && nomeFerramenta.ToUpper().StartsWith("KL"))
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}{nomeFerramenta}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                    .Replace("[nome_ferramenta]", nomeFerramenta)
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    continue;
                }
                else if (templateName == "#FB_Rob_Schweissen_DB.xml")
                {
                    if (data.toolsAmount > 0)
                    {
                        for (int i = 0; i < data.toolsAmount; i++)
                        {
                            string nomeFerramenta = data.DgvFerramentas.Rows[i].Cells[0].Value.ToString().Trim();
                            if (!string.IsNullOrEmpty(nomeFerramenta) && nomeFerramenta.ToUpper().StartsWith("SK"))
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}{nomeFerramenta}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                    .Replace("[nome_ferramenta]", nomeFerramenta)
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    continue;
                }
                else if (templateName == "#FB_Rob_MIGMAG_DB.xml")
                {
                    if (data.toolsAmount > 0)
                    {
                        for (int i = 0; i < data.toolsAmount; i++)
                        {
                            string nomeFerramenta = data.DgvFerramentas.Rows[i].Cells[0].Value.ToString().Trim();
                            if (!string.IsNullOrEmpty(nomeFerramenta) && nomeFerramenta.ToUpper().StartsWith("MS"))
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}{nomeFerramenta}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                    .Replace("[nome_ferramenta]", nomeFerramenta)
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                    continue;
                }
                //==============================================

                //Logica de FMs
                if (templateName == "#FB_Status_Global_DB.xml")
                {
                    fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}_FM{templateName}";
                    string numeroFM = "_FM";
                    content = File.ReadAllText(FBTemplate)
                        .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                        .Replace("[numero_fm]", numeroFM)
                        .Replace("[numero_db]", data.DBInstanzenNumber.ToString());
                    destinationPath = Path.Combine(destinationFolder, fileName);
                    File.WriteAllText(destinationPath, content);
                    if (data.DBInstanzenNumber < 20000) { data.DBInstanzenNumber++; }
                    if (data.FmAmount > 0)
                    {
                        for (int i = 0; i < data.FmAmount; i++)
                        {
                            numeroFM = data.DgvFMs.Rows[i].Cells[0].Value.ToString().Trim();
                            string descricaoFM = data.DgvFMs.Rows[i].Cells[1].Value.ToString().Trim();
                            if (!string.IsNullOrEmpty(descricaoFM))
                            {
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}_FM{numeroFM}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                    .Replace("[numero_fm]", $"_FM{numeroFM}")
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                    }
                }

                //Logica de Frgs
                if (templateName == "#FB_Rob_Frg_DB.xml")
                {
                        for (int i = 0; i < 3; i++)
                        {
                            {
                                string numeroFrg = (i + 1).ToString();
                                fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}_Frg{numeroFrg}{templateName}";
                                content = File.ReadAllText(FBTemplate)
                                    .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                    .Replace("[numero_frg]", $"_Frg{numeroFrg}")
                                    .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                                destinationPath = Path.Combine(destinationFolder, fileName);
                                File.WriteAllText(destinationPath, content);
                                if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                            }
                        }
                }

                //Logica de StellFrgs
                if (templateName == "#FB_Rob_Frg_DB.xml")
                {
                    for (int i = 0; i < 3; i++)
                    {
                        {
                            string numeroFrg = (i + 1).ToString();
                            fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}_StellFrg{numeroFrg}{templateName}";
                            content = File.ReadAllText(FBTemplate)
                                .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                                .Replace("[numero_frg]", $"_StellFrg{numeroFrg}")
                                .Replace("[numero_db]", data.DBInstanzenNumber.ToString());

                            destinationPath = Path.Combine(destinationFolder, fileName);
                            File.WriteAllText(destinationPath, content);
                            if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;
                        }
                    }
                }

                // Lógica de generica para templates

                fileName = $"{data.SKNumber}{data.StationNumber}{data.RobNumber}{templateName}";
                    content = File.ReadAllText(FBTemplate)
                        .Replace("[nome_robo]", $"{data.SKNumber}{data.StationNumber}{data.RobNumber}")
                        .Replace("[numero_db]", data.DBInstanzenNumber.ToString());
                    destinationPath = Path.Combine(destinationFolder, fileName);
                    File.WriteAllText(destinationPath, content);
                    if (data.DBInstanzenNumber < 20000) data.DBInstanzenNumber++;               
            }
        }
    }
}
