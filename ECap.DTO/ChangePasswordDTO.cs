using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECap.DTO
{
   public class ChangePasswordDTO
    {
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
