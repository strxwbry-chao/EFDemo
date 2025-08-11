namespace EFDemo
{
    /// <summary>
    /// Customer entity representing a customer in our domain.
    /// This is part of the Domain-Driven Design (DDD) approach where entities
    /// represent real-world business objects with identity and behavior.
    /// 
    /// LEARNING NOTE: An entity is different from a value object because:
    /// - Entities have identity (usually an ID)
    /// - They can change over time
    /// - Two entities with same data but different IDs are different
    /// </summary>
    public class Customer
    {
        /// <summary>
        /// Primary key for the Customer entity.
        /// LEARNING NOTE: This is the unique identifier that Entity Framework
        /// will use to track this entity in the database.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Customer's first name.
        /// LEARNING NOTE: We use string? (nullable string) because we're using
        /// nullable reference types in .NET 9, which helps prevent null reference exceptions.
        /// </summary>
        public string FirstName { get; set; } = string.Empty;

        /// <summary>
        /// Customer's last name.
        /// </summary>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the customer is active in the system.
        /// LEARNING NOTE: This is a common pattern for "soft deletes" - instead of
        /// actually deleting records, we mark them as inactive for audit purposes.
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// When the customer record was created.
        /// LEARNING NOTE: Audit fields like this are important for tracking
        /// data changes and are often required for business compliance.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the customer record was last updated.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Business method to get the customer's full name.
        /// LEARNING NOTE: Domain entities can contain business logic and behavior,
        /// not just data. This keeps business rules close to the data they operate on.
        /// </summary>
        /// <returns>The full name of the customer</returns>
        public string GetFullName()
        {
            return $"{FirstName} {LastName}".Trim();
        }

        /// <summary>
        /// Business method to deactivate a customer.
        /// LEARNING NOTE: Instead of exposing setters directly, we can create
        /// meaningful business methods that encapsulate the rules and side effects.
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Business method to activate a customer.
        /// </summary>
        public void Activate()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Updates the customer's name and sets the updated timestamp.
        /// LEARNING NOTE: This method ensures that whenever we update the name,
        /// we also update the audit timestamp automatically.
        /// </summary>
        public void UpdateName(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
