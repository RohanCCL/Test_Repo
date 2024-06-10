using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL_Notification
{
    public class ConfigModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }

        public int CCL { get; set; }

        public int CCW { get; set; }

        public int CCR { get; set; }

        public int CCD { get; set; }

        public int CCK { get; set; }

        public int IF { get; set; }

        public bool ActiveStatus { get; set; }

        public virtual List<Plant> Plants { get; set; }
    }
}
