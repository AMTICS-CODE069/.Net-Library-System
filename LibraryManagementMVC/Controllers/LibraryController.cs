namespace LibraryManagementMVC.Controllers
{
    public class LibraryController : Controller
    {
        // In-memory "database" of books available in the college library.
        // static so the sample data + issued list persist across requests
        // for the lifetime of the running application.
        private static readonly List<Book> _books = new()
        {
            new Book { BookId = 101, Title = "Introduction to Algorithms", Author = "Cormen, Leiserson, Rivest, Stein", Category = "Computer Science", Price = 850.00m, AvailableCopies = 4 },
            new Book { BookId = 102, Title = "Clean Code", Author = "Robert C. Martin", Category = "Software Engineering", Price = 650.00m, AvailableCopies = 3 },
            new Book { BookId = 103, Title = "The C Programming Language", Author = "Kernighan & Ritchie", Category = "Programming", Price = 400.00m, AvailableCopies = 5 },
            new Book { BookId = 104, Title = "Database System Concepts", Author = "Silberschatz, Korth, Sudarshan", Category = "Databases", Price = 720.00m, AvailableCopies = 2 },
            new Book { BookId = 105, Title = "Operating System Concepts", Author = "Silberschatz, Galvin, Gagne", Category = "Operating Systems", Price = 690.00m, AvailableCopies = 3 },
        };

        // Temporary in-memory collection holding issued-book records.
        private static readonly List<LibraryMember> _issuedBooks = new();

        private static int _nextMemberId = 1;

        // GET: /Library/Index
        // Displays all currently issued books in a Bootstrap table.
        public IActionResult Index()
        {
            // Attach the matching Book to each member record for display purposes.
            foreach (var member in _issuedBooks)
            {
                member.IssuedBook = _books.FirstOrDefault(b => b.BookId == member.BookId);
            }

            return View(_issuedBooks);
        }

        // GET: /Library/IssueBook
        // Shows the book-issue form.
        [HttpGet]
        public IActionResult IssueBook()
        {
            ViewBag.Books = _books;
            return View(new LibraryMember { IssueDate = DateTime.Now });
        }

        // POST: /Library/IssueBook
        // Handles form submission for issuing a book to a student.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult IssueBook(LibraryMember member)
        {
            // Ensure the Book ID entered actually exists in the catalog.
            var book = _books.FirstOrDefault(b => b.BookId == member.BookId);
            if (book == null)
            {
                ModelState.AddModelError(nameof(member.BookId), "No book exists with this Book ID.");
            }
            else if (book.AvailableCopies <= 0)
            {
                ModelState.AddModelError(nameof(member.BookId), $"'{book.Title}' has no available copies right now.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Books = _books;
                return View(member);
            }

            member.MemberId = _nextMemberId++;
            _issuedBooks.Add(member);

            // Reduce available copies since a copy has just been issued.
            book!.AvailableCopies--;

            TempData["SuccessMessage"] = $"Book issued successfully to {member.MemberName}.";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Library/BookDetails/5
        // Displays full details for a single issued-book record.
        public IActionResult BookDetails(int id)
        {
            var member = _issuedBooks.FirstOrDefault(m => m.MemberId == id);
            if (member == null)
            {
                return NotFound();
            }

            member.IssuedBook = _books.FirstOrDefault(b => b.BookId == member.BookId);

            return View(member);
        }

        // GET: /Library/About
        public IActionResult About()
        {
            return View();
        }

        // GET: /Library/Error
        public IActionResult Error()
        {
            return View();
        }
    }
}
