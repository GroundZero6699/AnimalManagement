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
    }
}
