namespace Obscura.FinanceTracker.Base.Entities
{
    /// <summary>
    /// Common audit and soft-delete fields shared by every domain entity.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>Unique identifier of the entity.</summary>
        public Guid Id { get; set; }

        /// <summary>UTC timestamp at which the entity was created.</summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>Id of the user who created the entity, if known.</summary>
        public Guid? CreatedBy { get; set; }

        /// <summary>UTC timestamp of the last update, if any.</summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>Id of the user who last updated the entity, if known.</summary>
        public Guid? UpdatedBy { get; set; }

        /// <summary>Whether the entity is soft-deleted. Soft-deleted entities are excluded by default query filters.</summary>
        public bool IsDeleted { get; set; } = false;

        /// <summary>UTC timestamp at which the entity was soft-deleted, if any.</summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>Id of the user who soft-deleted the entity, if known.</summary>
        public Guid? DeletedBy { get; set; }
    }
}
