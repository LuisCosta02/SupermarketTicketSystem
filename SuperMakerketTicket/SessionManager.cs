using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperMakerketTicket
{
    internal class SessionManager
    {
        public static string connectionString = "Host=;Database=;Username=;Password=;Persist Security Info=True";

        public static int ActualTicket { get; set; }

        public static string ActualService { get; set; }
    }
}
