using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace VASS06_GeraFC_Robo
{
    public partial class FormLogin : Form
    {

        public FormLogin()
        {
            InitializeComponent();
        }

        private async void btn_Login_Click(object sender, EventArgs e)
        {
            string usuario = txb_Usuario.Text;
            string senha = txb_Senha.Text;

            var dadosLogin = new
            {
                email = usuario,
                password = senha
            };

            string json = JsonConvert.SerializeObject(dadosLogin);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.PostAsync("https://projix.app/APIs/main.dll/login", content);

                    string responseBody = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = JObject.Parse(responseBody);
                        string token = jsonResponse["token"]?.ToString();

                        if (!string.IsNullOrEmpty(token))
                        {
                            // Token recebido com sucesso — abre o form principal com o token
                            FormPrincipal formPrincipal = new FormPrincipal();
                            formPrincipal.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Usuário ou senha incorretos. Verifique suas credenciais e tente novamente.", "InfoRMI - GeraFC_Robo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Login falhou: {responseBody}", "InfoRMI - GeraFC_Robo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erro ao conectar com o servidor: {ex.Message}", "InfoRMI - GeraFC_Robo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void btn_Fechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Press_Enter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_Login.PerformClick();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
