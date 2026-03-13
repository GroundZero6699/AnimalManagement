using AnimalManagement.Animals;
using System.Collections.ObjectModel;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-11
 * Description: stores a collection of animal objects.
 */

namespace AnimalManagement.Manager
{ 
    /// <summary>
    /// Manages a collection of Animal objects, providing methods to add and remove animals.
    /// </summary>
    public class ListManager : IListManager
    {
        /// <summary>
        /// A collection of Animal objects that supports notifications when items are added, removed, or the entire list
        /// is refreshed.
        /// </summary>
        private ObservableCollection<IAnimal> animalList;

        /// <summary>
        /// Gets the collection of animals.
        /// </summary>
        public ObservableCollection<IAnimal> getAnimals => animalList;

        /// <summary>
        /// Initializes a new instance of the ListManager class with an empty collection of animals.
        /// </summary>
        public ListManager()
        {
            animalList = new ObservableCollection<IAnimal>();
        }

        /// <summary>
        /// Adds an animal to the animal list.
        /// </summary>
        /// <param name="animal">The animal to add.</param>
        public void addAnimal(IAnimal animal)
        {
            animalList.Add(animal);
        }

        /// <summary>
        /// Removes the specified animal from the animal list.
        /// </summary>
        /// <param name="animal">The animal to remove.</param>
        /// <returns>true if the animal was successfully removed; otherwise, false.</returns>
        public bool removeAnimal(IAnimal animal)
        {
            return animalList.Remove(animal);
        }
    }
}
