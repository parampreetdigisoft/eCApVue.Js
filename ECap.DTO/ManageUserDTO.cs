using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECap.DTO
{
    public class ManageUserDTO
    {
        public int UserId { get; set; }
        public string User_Login { get; set; }
        public string User_Pwd { get; set; }
        public string User_ClientGroup { get; set; }
        public string LastTestCreationDate { get; set; }
        public string ProdLang { get; set; }
        public bool User_IsActive { get; set; }
        public string User_CompleteDate { get; set; }
        public DateTime UploadDate { get; set; }
        public string C_Name { get; set; }
        public string User_FirstName { get; set; }
        public string User_LastName { get; set; }
        public string User_Address1 { get; set; }
        public string User_Address2 { get; set; }
        public string User_City { get; set; }
        public string User_State { get; set; }
        public string User_Country { get; set; }
        public string User_Zipcode { get; set; }
        public string User_Phone { get; set; }
        public string User_AdminRole { get; set; }
        public bool UserLogin_Flag { get; set; }
        public bool SendMail_Flag { get; set; }
        public int? UserStatus { get; set; }
        public string User_Department { get; set; }
    }
}
