using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMakerketTicket
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            

            DateTime dataAtual = DateTime.Now;
            string dataFormatada = dataAtual.ToString("dd/MM/yyyy");
            label2.Text = dataFormatada;

            timer1.Interval = 1000;
            timer1.Start();

            atualizarlabelinicio();

           this.WindowState = FormWindowState.Maximized;
           this.FormBorderStyle = FormBorderStyle.None;
           this.TopMost = true;


            PanellingPosition();



        }

        private void PanellingPosition() {
            if (SessionManager.ActualService == label1.Text)  //cafe
            {
                panel1.BackColor = Color.Gray;
                label5.ForeColor = Color.White;
                label4.BackColor = Color.Gray;



            }
            else if (SessionManager.ActualService == label8.Text) //bakery
            {
                panel3.BackColor = Color.Gray;
                label6.ForeColor = Color.White;
                label7.BackColor = Color.Gray;

                panel1.Anchor = AnchorStyles.Right;
                panel3.Anchor = AnchorStyles.Right;

                System.Drawing.Point oldlocationp3 = panel3.Location;
                panel3.Location = panel1.Location;
                panel1.Location = oldlocationp3;




            }
            else if (SessionManager.ActualService == label11.Text) //butch
            {
                panel4.BackColor = Color.Gray;
                label9.ForeColor = Color.White;
                label10.BackColor = Color.Gray;

                System.Drawing.Point oldlocationp4 = panel4.Location;
                panel4.Location = panel1.Location;
                panel1.Location = oldlocationp4;

            }
        }

        


        private void timer1_Tick_1(object sender, EventArgs e)
        {
            
            DateTime horaAtual = DateTime.Now;

           
            string horaFormatada = horaAtual.ToString("HH:mm:ss");

            
            label3.Text = horaFormatada;
        }

        public void AtualizarLabelNumeroSenha(int novoNumero, string service)
        {
            if (service == label1.Text) //cafe
            {
                label5.Text = novoNumero.ToString();
            }
            else if (service == label8.Text) //bakey
            {
                label6.Text = novoNumero.ToString();
            }
            else if (service ==  label11.Text) //butch
            {
                label9.Text = novoNumero.ToString();
            }
        }

        public void DecrementarLabelNumeroSenha(int decremento, string service)
        {
            int valorAtual = 0;
            Label labelToUpdate = null;

            if (service == label1.Text)
            {
                labelToUpdate = label5;
            }
            else if (service == label8.Text)
            {
                labelToUpdate = label6;
            }
            else if (service == label11.Text)
            {
                labelToUpdate = label9;
            }

            if (labelToUpdate != null && int.TryParse(labelToUpdate.Text, out valorAtual))
            {
                valorAtual -= decremento;
                if (valorAtual >= 0)                 {
                    labelToUpdate.Text = valorAtual.ToString();
                }
                else
                {
                    MessageBox.Show("O valor não pode ser negativo!");
                }
            }
        }

        public string ObterValorAtualLabel(string service)
        {
            if (service == label1.Text)
            {
                return label5.Text;
            }
            else if (service == label8.Text)
            {
                return label6.Text;
            }
            else if (service == label11.Text)
            {
                return label9.Text;
            }

            return "0";
        }

        private void AtualizarLabelNumeroTicketPorServico(string service, Label label)
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

            
            label.Text = maxTicket.ToString();

            
            if (service == SessionManager.ActualService)
            {
                SessionManager.ActualTicket = maxTicket;
            }
        }

        private void atualizarlabelinicio()
        {
            
            AtualizarLabelNumeroTicketPorServico(label1.Text, label5);
            AtualizarLabelNumeroTicketPorServico(label8.Text, label6);
            AtualizarLabelNumeroTicketPorServico(label11.Text, label9);   
        }




    }
}
