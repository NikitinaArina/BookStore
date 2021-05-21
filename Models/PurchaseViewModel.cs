using System.Collections.Generic;

namespace BookStore.Models
{
    public class PurchaseViewModel
    {
        public int Id { get; set; }
        public string Person { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TotalPrice { get; set; }
        public IEnumerable<Book> Books { get; set; }
    }
}