using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaTicketSystem.Models
{
    public class Salon
    {
        public string Name { get; set; } // Salon adı
        public List<Film> Films { get; set; } // Salon altındaki filmler
    }
}
