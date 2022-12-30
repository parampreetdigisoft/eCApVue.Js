namespace ECap.Web.Models
{
    public class UserInfoViewModel
    {
        public ManageUserViewModel User { get; set; }
        public IEnumerable<Product>? Products { get; set; }
        public IEnumerable<AddOn>? Addons { get; set; }

    }
}
