using AnimalManagement.Animals;
using AnimalManagement.Animals.Mammals.Species;
using AnimalManagement.Animals.Reptiles.Species;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;

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
        /// Holds current filepath for saving/loading.
        /// </summary>
        public string? currentFilePath { get; set; }

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
        /// Clear all animal objects from list.
        /// </summary>
        public void cleanSlate()
        {
            animalList.Clear();
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

        public void saveToFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".txt":
                    File.WriteAllText(filePath, buildTxt());
                    break;
                case ".json":
                    saveAsJson(filePath);
                    break;
                case ".xml":
                    saveAsXml(filePath);
                    break;
                default:
                    throw new Exception("File format not supported.");
            }
        }

        private void saveAsJson(string filePath)
        {
            var serial = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            string json = JsonConvert.SerializeObject(animalList, serial);
            File.WriteAllText(filePath, json);
        }

        private void saveAsXml(string filePath)
        {
            var serial = new XmlSerializer(typeof(List<Animal>),
                new Type[]
                {
                    typeof(Dog),
                    typeof(Cat),
                    typeof(Cow),
                    typeof(Lizard),
                    typeof(Snake),
                    typeof(Turtle)
                });
            using var stream = File.Create(filePath);

            var concrete = animalList.Cast<Animal>().ToList();
            serial.Serialize(stream, concrete);
        }

        private string buildTxt()
        {
            var builder = new StringBuilder();
            builder.AppendLine("********************");
            foreach (IAnimal animal in animalList)
            {
                if (animal == null) continue;

                builder.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                                animal.id, animal.name, animal.age, animal.weight, animal.gender, animal.image));

                if (animal.type == Types.Mammal)
                {
                    builder.AppendLine(string.Format("{0},{1}", animal.type, animal.species));
                    switch (animal.species)
                    {
                        case "Dog":
                            var dog = (Dog)animal;
                            builder.AppendLine(string.Format("{0},{1},{2}", dog.breed, dog.chipped, dog.ears));
                            break;
                        case "Cat":
                            var cat = (Cat)animal;
                            builder.AppendLine(string.Format("{0},{1}", cat.breed, cat.livingType));
                            break;
                        case "Cow":
                            var cow = (Cow)animal;
                            builder.AppendLine(string.Format("{0},{1},{2}", cow.tagged, cow.tagNumber, cow.milkContent));
                            break;
                    }
                }
                else if (animal.type == Types.Reptile)
                {
                    builder.AppendLine(string.Format("{0},{1}", animal.type, animal.species));
                    switch (animal.species)
                    {
                        case "Lizard":
                            var lizard = (Lizard)animal;
                            builder.AppendLine(string.Format("{0}", lizard.venomous));
                            break;
                        case "Snake":
                            var snake = (Snake)animal;
                            builder.AppendLine(string.Format("{0},{1}", snake.venom, snake.pattern));
                            break;
                        case "Turtle":
                            var turtle = (Turtle)animal;
                            builder.AppendLine(string.Format("{0},{1}", turtle.shellWidth, turtle.shellHardness));
                            break;
                    }
                }
            }
            return builder.ToString();
        }

        public void loadFromFile(string filePath)
        {

        }
    }
}
