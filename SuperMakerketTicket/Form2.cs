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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        Form1 f1 = new Form1();

        private void Form2_Load(object sender, EventArgs e)
        {
            LatestUpdateLabel();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //cafe
            SessionManager.ActualService = button1.Text;

            f1.Show();
            this.Hide();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //bakery
            SessionManager.ActualService = button2.Text;
            f1.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //butch
            SessionManager.ActualService = button3.Text;
            f1.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resetDay(); 

        }

        private void resetDay() {
            using (NpgsqlConnection conn = new NpgsqlConnection(SessionManager.connectionString))
            {
                conn.Open();

                
                using (NpgsqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        
                        string insertQuery = @"
                INSERT INTO TicketsBackup (id_ticket, generated_date, service, service_ticket)
                SELECT id_ticket, date, service, service_ticket
                FROM ticketsofday;
            ";

                       
                        using (NpgsqlCommand cmdInsert = new NpgsqlCommand(insertQuery, conn, transaction))
                        {
                            cmdInsert.ExecuteNonQuery();
                        }

                        
                        string deleteQuery = "DELETE FROM ticketsofday;";

                       
                        using (NpgsqlCommand cmdDelete = new NpgsqlCommand(deleteQuery, conn, transaction))
                        {
                            cmdDelete.ExecuteNonQuery();
                        }

                        
                        transaction.Commit();
                        MessageBox.Show("Restart feito com sucesso");
                        LatestUpdateLabel();

                    }
                    catch (Exception ex)
                    {
                        
                        transaction.Rollback();
                        MessageBox.Show("Ocorreu um erro: " + ex.Message);
                    }
                }
            }


        }

        private void LatestUpdateLabel()
        {
            // Variável para armazenar a data mais recente
            DateTime latestBackupDate = DateTime.MinValue;

            // Conectar ao banco de dados e executar a consulta
            using (var conn = new NpgsqlConnection(SessionManager.connectionString))
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("SELECT MAX(backup_date) FROM TicketsBackup", conn))
                {
                    var result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        latestBackupDate = (DateTime)result;
                    }
                }
            }

            // Atualizar label3 com a data mais recente, formatando-a conforme necessário
            if (latestBackupDate != DateTime.MinValue)
            {
                label3.Text = "" + latestBackupDate.ToString("dd/MM/yyyy HH:mm:ss");
            }
            else
            {
                label3.Text = "None.";
            }
        }


    }
}
