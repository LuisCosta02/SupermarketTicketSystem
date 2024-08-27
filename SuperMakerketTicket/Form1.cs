using Npgsql;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
namespace SuperMakerketTicket

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

           /* this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;*/

            
            Screen firstScreen = Screen.AllScreens[0];
            Screen secondScreen = Screen.AllScreens[1];

            
            this.StartPosition = FormStartPosition.Manual;
            this.Location = firstScreen.Bounds.Location;

            
            Form3 form3 = new Form3();
            Form4 form4 = new Form4();

           
            form3.WindowState = FormWindowState.Maximized;
            form3.FormBorderStyle = FormBorderStyle.None;
            form3.StartPosition = FormStartPosition.Manual;
            form3.Location = secondScreen.Bounds.Location;

            
            this.TopMost = true;
            form3.TopMost = true;
            form4.TopMost = true; 

            
            form3.Show();
            form4.Show();

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            ProcessarTicket(button1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            ProcessarTicket(button2.Text);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProcessarTicket(button3.Text);

        }

        private void ProcessarTicket(string nomeServico)
        {
            int numTicket = ObterNumeroProximoTicket(nomeServico);

            using (var conn = new NpgsqlConnection(SessionManager.connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("INSERT INTO ticketsofday (num_ticket, service, service_ticket) VALUES (@num_ticket, @service, @service_ticket)", conn))
                {
                    cmd.Parameters.AddWithValue("num_ticket", numTicket);
                    cmd.Parameters.AddWithValue("service", nomeServico);
                    cmd.Parameters.AddWithValue("service_ticket", numTicket);
                    cmd.ExecuteNonQuery();
                }
            }

            SessionManager.ActualTicket = numTicket;
        }
        private int ObterNumeroProximoTicket(string service)
        {
            int numTicket = 1;


            using (var conn = new NpgsqlConnection(SessionManager.connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT COALESCE(MAX(service_ticket), 0) + 1 FROM ticketsofday WHERE service = @service", conn))
                {
                    cmd.Parameters.AddWithValue("service", service);
                    numTicket = (int)cmd.ExecuteScalar();
                }
            }

            return numTicket;
        }
    }
}
