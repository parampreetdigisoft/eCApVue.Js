using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECap.DTO
{
    public class ProductDTO
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
        public string MarkTestComplete { get; set; }
    }
}
