using Knowte.Plugin.Contracts.Collection.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knowte.Plugin.Contracts.Collection
{
    /// <summary>
    /// Implement this interface to create a plugin that is able to store notes in any alternative location.
    /// Instructions on how to write plugins, can be found in the file: "How to write plugins.md" in this project.
    /// </summary>
    public interface ICollectionProvider
    {
        /// <summary>
        /// Mandatory name for the provider
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Activates a collection based on collectionId
        /// </summary>
        /// <param name="collectionId">The collectionId</param>
        /// <returns>True if successful</returns>
        Task<bool> ActivateCollectionAsync(string collectionId);

        /// <summary>
        /// Get collectionId based on the collection title
        /// </summary>
        /// <param name="title">The collection title</param>
        /// <returns>The collectionId if the collection was found. An empty string if the collection was not found.</returns>
        Task<string> GetCollectionIdAsync(string title);

        /// <summary>
        /// Adds a collection based on its title
        /// </summary>
        /// <param name="title">The collection title</param>
        /// <param name="isActive">Indicates if the collection should be activate after adding</param>
        /// <returns>The collectionId if successful. An empty string if failure.</returns>
        Task<string> AddCollectionAsync(string title, bool isActive);

        /// <summary>
        /// Deletes a collection based on collectionId
        /// </summary>
        /// <param name="collectionId">The collectionId</param>
        /// <returns>True if successful</returns>
        Task<bool> DeleteCollectionAsync(string collectionId);

        /// <summary>
        /// Edits the title of a collection based on collectionId
        /// </summary>
        /// <param name="collectionId">The collectionId</param>
        /// <param name="title">The new title for the collection</param>
        /// <returns>True if successful</returns>
        Task<bool> EditCollectionAsync(string collectionId, string title);

        /// <summary>
        /// Gets all the collections
        /// </summary>
        /// <returns>A list of all collections</returns>
        Task<List<ICollection>> GetCollectionsAsync();
    }
}
