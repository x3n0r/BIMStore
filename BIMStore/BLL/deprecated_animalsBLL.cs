using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIMStore.BLL
{
    class animalsBLL
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string species { get; set; }
        public string race { get; set; }
        public DateTime date_of_birth { get; set; }
        public string notes { get; set; }
        public int cust_id { get; set; }

    }
}
