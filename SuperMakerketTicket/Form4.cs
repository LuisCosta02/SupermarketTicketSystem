using Npgsql;
using System;
using System.Windows.Forms;

namespace SuperMakerketTicket
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void AtualizarOutroForm(int numTicket)
        {
            Form3 form3 = Application.OpenForms["Form3"] as Form3;

            if (form3 != null)
            {
                int valorAtual;
                string service = SessionManager.ActualService;

                if (int.TryParse(form3.ObterValorAtualLabel(service), out valorAtual))
                {
                    int maxTicket = ObterNumeroMaximoTicket(service);

                    if (valorAtual < maxTicket)
                    {
                        int novoValor = valorAtual + 1;
                        form3.AtualizarLabelNumeroSenha(novoValor, service);
                        SessionManager.ActualTicket = novoValor;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AtualizarOutroForm(SessionManager.ActualTicket);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DecrementarOutroForm(SessionManager.ActualTicket);
        }

        private void DecrementarOutroForm(int numTicket)
        {
            Form3 form3 = Application.OpenForms["Form3"] as Form3;

            if (form3 != null)
            {
                int valorAtual;
                string service = SessionManager.ActualService;

                if (int.TryParse(form3.ObterValorAtualLabel(service), out valorAtual))
                {
                    int novoValor = valorAtual - 1;
                    if (novoValor >= 0) 
                    {
                        form3.DecrementarLabelNumeroSenha(1, service);
                        SessionManager.ActualTicket = novoValor;
                    }
                }
            }
        }

        private int ObterNumeroMaximoTicket(string service)
        {
            int maxTicket = 0;

            using (var conn = new NpgsqlConnection(SessionManager.connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT COALESCE(MAX(service_ticket), 0) FROM ticketsofday WHERE service = @service", conn))
                {
                    cmd.Parameters.AddWithValue("service", service);
                    maxTicket = (int)cmd.ExecuteScalar();
                }
            }

            return maxTicket;
        }

    }
}