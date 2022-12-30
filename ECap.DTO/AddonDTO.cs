using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECap.DTO
{
    public class AddonDTO
    {
        public int UA_Id { get; set; }
        public int Test_Id { get; set; }
        public int UA_AddonId { get; set; }
        public string AddonName { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }

    }
}
