namespace BookStore.Models
{
    public class BooksPurchases
    {
        public int Id { get; set; }
        public virtual Book Book { get; set; }
        public int BookId { get; set; }
        public virtual Purchase Purchase { get; set; }
        public int PurchaseId { get; set; }
        public int Quantity { get; set; }
    }
}