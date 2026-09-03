namespace LibraryManagementMVC.Models
{
    /// <summary>
    /// Represents a book available in the college library.
    /// </summary>
    public class Book
    {
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Available copies cannot be negative")]
        [Display(Name = "Available Copies")]
        public int AvailableCopies { get; set; }
    }
}
