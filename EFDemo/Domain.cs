namespace EFDemo.Domain
{
    /// <summary>
    /// Customer entity - demonstrates a simple domain entity with the Repository and Specification patterns.
    /// Contains only the essential fields specified in the requirements.
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        public string GetFullName() => $"{FirstName} {LastName}".Trim();
    }
}
