using AnimalManagement.Animals;
using System.Collections.ObjectModel;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-11
 * Description: Interface for the list manager to update when a change is made.
 */

namespace AnimalManagement.Manager
{
    /// <summary>
    /// Defines methods and a property for managing a collection of Animal objects.
    /// </summary>
    public interface IListManager
    {
        /// <summary>
        /// Gets a collection of Animal objects.
        /// </summary>
        ObservableCollection<IAnimal> getAnimals { get; }

        /// <summary>
        /// Holds referens to current filepath.
        /// </summary>
        public string? currentFilePath { get; set; }

        /// <summary>
        /// Indicates whether the current file path is set and not empty.
        /// </summary>
        public bool hasCurrentFilePath => !string.IsNullOrEmpty(currentFilePath);

        /// <summary>
        /// Clears all animal object from list.
        /// </summary>
        void cleanSlate();

        /// <summary>
        /// Adds an animal to the collection.
        /// </summary>
        /// <param name="animal">The animal to add.</param>
        void addAnimal(IAnimal animal);

        /// <summary>
        /// Removes the specified animal from the collection.
        /// </summary>
        /// <param name="animal">The animal to remove.</param>
        /// <returns>true if the animal was successfully removed; otherwise, false.</returns>
        bool removeAnimal(IAnimal animal);

        /// <summary>
        /// Saves current data to a file using filepath as location.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        void saveToFile(string filePath);

        /// <summary>
        /// Load data from a file using filepath as location.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        void loadFromFile(string filePath);
    }
}
