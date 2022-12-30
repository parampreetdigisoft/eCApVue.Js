namespace ECap.Web.Models
{
    public class Product
    {
        public int PurchaseId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductNameEn { get; set; }
        public string ProductLang { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ProductDesc { get; set; }
        public string Price { get; set; }
        public DateTime Completedate { get; set; }
        public bool IsProductActive { get; set; }
        public string InvoiceNo { get; set; }
        public int StatusCode { get; set; }
        public string Score { get; set; }
        public string Full { get; set; }
        public string Pro { get; set; }
    }
}
