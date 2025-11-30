using System.Windows.Forms;

namespace VASS06_GeraFC_Robo
{
    public class RoboData
    {
        // Propriedades Simples
        public string SKNumber { get; set; }
        public string StationNumber { get; set; }
        public string RobNumber { get; set; }

        // Contadores (que serão modificados)
        public int DBAnwenderNumber { get; set; }
        public int DBInstanzenNumber { get; set; }
        public int FCNumber { get; set; }

        // Contagens de Itens (para loops)
        public int securityAmount { get; set; }
        public int toolsAmount { get; set; }
        public int interlockAmount { get; set; }
        public int FmAmount { get; set; }
        public int folgesAmount { get; set; }
        public int inputsAmount { get; set; }
        public int outputsAmount { get; set; }

        // Referências de UI (para ler os dados das tabelas)
        public DataGridView DgvSegurança { get; set; }
        public DataGridView DgvFerramentas { get; set; }
        public DataGridView DgvInterlocks { get; set; }
        public DataGridView DgvFMs { get; set; }
        public DataGridView DgvFolges { get; set; }
        public DataGridView DgvEntradas { get; set; }
        public DataGridView DgvSaidas { get; set; }

    }
}
