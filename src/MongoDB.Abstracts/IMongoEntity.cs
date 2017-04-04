using System;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// An <c>interface</c> for a MongoDB Entity
    /// </summary>
    public interface IMongoEntity
    {
        /// <summary>
        /// Gets or sets the identifier for the entity.
        /// </summary>
        /// <value>
        /// The identifier for the entity.
        /// </value>
        string Id { get; set; }

        /// <summary>
        /// Gets or sets the date the entity was created.
        /// </summary>
        /// <value>
        /// The date the entity was created.
        /// </value>
        DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date the entity was updated.
        /// </summary>
        /// <value>
        /// The date the entity was updated.
        /// </value>
        DateTime Updated { get; set; }
    }
}
