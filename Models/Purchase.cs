using System.Collections.Generic;

namespace BookStore.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Person { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public virtual ICollection<BooksPurchases> BooksPurchases { get; set; }
    }
}