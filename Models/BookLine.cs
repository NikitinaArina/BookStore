namespace BookStore.Models
{
    public class BookLine
    {

        public BookLine(Book book, int quantity)
        {
            Book = book;
            Quantity = quantity;
        }

        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}