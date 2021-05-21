using BookStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace BookStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BookContext db;

        public HomeController()
        {
            db = new BookContext();
        }

        private Dictionary<int, Book> BooksInCart
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["cart"] == null)
                {
                    System.Web.HttpContext.Current.Session["cart"] = new Dictionary<int, Book>();
                }

                return System.Web.HttpContext.Current.Session["cart"] as Dictionary<int, Book>;
            }
        }

        public ActionResult Index()
        {
            var booksFromDb = db.Books.ToList();

            foreach (var book in booksFromDb)
            {
                if (BooksInCart.TryGetValue(book.Id, out var bookInCart))
                {
                    book.Quantity -= bookInCart.Quantity;
                }
            }

            return View(booksFromDb.Where(b => b.Quantity > 0).ToList());
        }

        [HttpPost]
        public ActionResult AddToCart(BookForAdd bookForAdd)
        {
            var oneBook = db.Books.FirstOrDefault(b => b.Id == bookForAdd.Id);

            if (!BooksInCart.TryGetValue(bookForAdd.Id, out var checkBook) && oneBook.Quantity >= bookForAdd.Quantity)
            {
                BooksInCart.Add(
                    bookForAdd.Id,
                    new Book
                    {
                        Id = bookForAdd.Id,
                        Author = oneBook.Author,
                        Name = oneBook.Name,
                        Price = oneBook.Price,
                        Quantity = bookForAdd.Quantity
                    });
            }
            else if(checkBook.Quantity + bookForAdd.Quantity <= oneBook.Quantity)
            {
                checkBook.Quantity += bookForAdd.Quantity;
            }

            return RedirectToAction("Index");
        }
        
        public ActionResult ItemViewPartial(Book book)
        {
            var bookForAdd = new BookForAdd 
            { 
                Id = book.Id,
                Author = book.Author,
                Name = book.Name,
                Price = book.Price,
                Quantity = book.Quantity
            };

            return PartialView("_ItemViewPartial", bookForAdd);
        }

        public ActionResult CartLine()
        {
            ViewData["cart"] = BooksInCart;
            return View();
        }

        [Authorize]
        public ActionResult AddBook()
        {
            return View("AddBook");
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddBookPost(Book book)
        {
            db.Books.Add(book);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult EditBooks()
        {
            return View("EditBooks", db.Books.ToList());
        }

        [Authorize]
        public ActionResult EditBookPartial(Book book)
        {
            return PartialView("_EditBookPartial", book);
        }

        [HttpPost]
        [Authorize]
        public ActionResult EditBookQuantity(Book book)
        {
            var bookFromDb = db.Books.FirstOrDefault(b => b.Id == book.Id);
            bookFromDb.Quantity = book.Quantity;

            db.SaveChanges();

            return RedirectToAction("EditBooks");
        }

        [Authorize]
        public ActionResult PurchaseList()
        {
            var purchases = new List<PurchaseViewModel>();

            foreach (var purchaseFromDb in db.Purchases)
            {
                purchases.Add(new PurchaseViewModel
                {
                    Id = purchaseFromDb.Id,
                    Person = purchaseFromDb.Person,
                    Email = purchaseFromDb.Email,
                    Phone = purchaseFromDb.Phone,
                    Books = purchaseFromDb.BooksPurchases
                        .Select(p => new Book
                        {
                            Id = p.Book.Id,
                            Name = p.Book.Name,
                            Author = p.Book.Author,
                            Price = p.Book.Price,
                            Quantity = p.Quantity
                        })
                        .ToList(),
                    TotalPrice = purchaseFromDb.BooksPurchases.Sum(p => p.Quantity * p.Book.Price)
                });
            }

            return View("PurchaseList", purchases);
        }

        public ActionResult PurchaseViewPartial()
        {
            return PartialView("_PurchaseViewPartial");
        }

        [HttpPost]
        public ActionResult MakeOrder(PurchaseForMake purchaseForMake)
        {
            if (ModelState.IsValid)
            {
                foreach (var b in BooksInCart)
                {
                    var book = db.Books.FirstOrDefault(p => p.Id == b.Value.Id);
                    book.Quantity -= b.Value.Quantity;
                }

                var purchase = new Purchase
                {
                    Person = purchaseForMake.Person,
                    Email = purchaseForMake.Email,
                    Phone = purchaseForMake.Phone
                };

                db.Purchases.Add(purchase);
                db.SaveChanges();

                var booksPurchases = BooksInCart
                    .Select(b => new BooksPurchases
                    {
                        BookId = b.Value.Id,
                        PurchaseId = purchase.Id,
                        Quantity = b.Value.Quantity
                    })
                    .ToList();
                db.BooksPurchases.AddRange(booksPurchases);

                db.SaveChanges();
                BooksInCart.Clear();

                return RedirectToAction("Index");
            }
            else return RedirectToAction("CartLine");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult SignIn(User user)
        {
            var userInDb = db.Users.FirstOrDefault(u => string.Equals(u.Username, user.Username) && string.Equals(u.Password, user.Password));

            if (userInDb == null)
            {
                ModelState.AddModelError("", "Пользователь с таким логином или паролем не найден");
                return View("Login");
            }

            FormsAuthentication.SetAuthCookie(user.Username, true);
            return RedirectToAction("Index");
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}