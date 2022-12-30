using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECap.DTO
{
    public class UserInfoDTO
    {
        public ManageUserDTO User { get; set; }

        public IEnumerable<ProductDTO>? Products { get; set; }
        public IEnumerable<AddonDTO>? Addons { get; set; }
        
    }
}
