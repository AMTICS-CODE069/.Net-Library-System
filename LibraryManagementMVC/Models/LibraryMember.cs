namespace LibraryManagementMVC.Models
{
    /// <summary>
    /// Represents a library member's book-issue record.
    /// </summary>
    public class LibraryMember
    {
        public int MemberId { get; set; }

        [Required(ErrorMessage = "Member Name is required")]
        [StringLength(100, ErrorMessage = "Member Name cannot exceed 100 characters")]
        [Display(Name = "Member Name")]
        public string MemberName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Department is required")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Book ID is required")]
        [Display(Name = "Book ID")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Issue Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Issue Date")]
        public DateTime IssueDate { get; set; } = DateTime.Now;

        // Nullable type: a book may not yet have been returned, so ReturnDate
        // can legitimately be absent (null) until the librarian records the return.
        [DataType(DataType.Date)]
        [Display(Name = "Return Date")]
        public DateTime? ReturnDate { get; set; }

        // Convenience navigation property (not bound from the form) so views
        // can show book details alongside the member's issue record.
        [BindNever]
        public Book? IssuedBook { get; set; }
    }
}
