namespace LibraryManagementMVC.Extensions
{
    /// <summary>
    /// Extension methods that add issue/return business logic to LibraryMember,
    /// keeping the model class itself free of behaviour.
    /// </summary>
    public static class LibraryExtensions
    {
        // Standard loan period for the college library.
        private const int LoanPeriodDays = 14;

        // How many days before the due date counts as "Due Soon".
        private const int DueSoonThresholdDays = 2;

        // Fine charged per overdue day.
        private const decimal FinePerDay = 10m;

        /// <summary>
        /// Computes the due date for a book (Issue Date + standard loan period).
        /// </summary>
        public static DateTime GetDueDate(this LibraryMember member)
        {
            return member.IssueDate.AddDays(LoanPeriodDays);
        }

        /// <summary>
        /// Determines the current status of an issued book based on today's
        /// date and the member's ReturnDate.
        /// </summary>
        public static string GetDueStatus(this LibraryMember member)
        {
            // A ReturnDate value means the book has already been returned.
            if (member.ReturnDate.HasValue)
            {
                return "Returned";
            }

            DateTime dueDate = member.GetDueDate();
            int daysToDue = (dueDate.Date - DateTime.Now.Date).Days;

            if (daysToDue < 0)
            {
                return "Overdue";
            }

            if (daysToDue <= DueSoonThresholdDays)
            {
                return "Due Soon";
            }

            // Book is currently issued, not yet due, and no return date recorded.
            return "Issued";
        }

        /// <summary>
        /// Calculates the library fine (₹10 per overdue day). Returns 0 when the
        /// book is not overdue.
        /// </summary>
        public static decimal CalculateFine(this LibraryMember member)
        {
            DateTime dueDate = member.GetDueDate();

            // If the book was returned, compare against the actual return date;
            // otherwise compare against today (fine still accruing).
            DateTime compareDate = member.ReturnDate ?? DateTime.Now;

            int overdueDays = (compareDate.Date - dueDate.Date).Days;

            return overdueDays > 0 ? overdueDays * FinePerDay : 0m;
        }
    }
}
