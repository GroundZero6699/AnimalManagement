using AnimalManagement.Animals;
using AnimalManagement.Animals.Mammals;
using AnimalManagement.Animals.Mammals.Species;
using AnimalManagement.Animals.Reptiles;
using AnimalManagement.Animals.Reptiles.Species;
using AnimalManagement.Controller;
using AnimalManagement.Manager.Serialize;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Media.Imaging;
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

        /// <summary>
        /// Checks the file extension of the provided file path
        /// and calls the appropriate saving method based on the file format.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        /// <exception cref="Exception"> Thrown when format is not supported </exception>
        public void saveToFile(string filePath)
        {
            if (string.IsNullOrWhiteSpace(Path.GetExtension(filePath)))
            {
                filePath += ".txt";
            }
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

        /// <summary>
        /// Serializes the animal list to JSON format and saves it to the specified file path.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        private void saveAsJson(string filePath)
        {
            var serial = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };

            string json = JsonConvert.SerializeObject(animalList, serial);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Serializes the animal list to XML format and saves it to the specified file path.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        private void saveAsXml(string filePath)
        {
            var list = new List<AnimalXml>();
            foreach (var animal in animalList)
            {
                list.Add(mapped(animal));
            }

            var serial = new XmlSerializer(typeof(List<AnimalXml>),
                new Type[]
                {
                    typeof(DogXml),
                    typeof(CatXml),
                    typeof(CowXml),
                    typeof(LizardXml),
                    typeof(SnakeXml),
                    typeof(TurtleXml)
                });
            using var stream = File.Create(filePath);
            serial.Serialize(stream, list);
        }

        /// <summary>
        /// Build a string representation of the animal list in a specific format
        /// where each animal's properties are separated by commas.
        /// </summary>
        /// <returns> formatted string </returns>
        private string buildTxt()
        {
            var builder = new StringBuilder();
            foreach (IAnimal animal in animalList)
            {
                if (animal == null) continue;

                builder.AppendLine(string.Format("{0},{1},{2},{3},{4},{5},{6}",
                                animal.id, animal.name, animal.age, animal.weight, animal.gender, animal.image));

                if (animal.type == Types.Mammal)
                {
                    builder.AppendLine(string.Format("{0},{1}", animal.type, animal.species));
                    var mammal = (Mammal)animal;
                    builder.AppendLine(string.Format("{0},{1},{2}", mammal.nrOfTeeth, mammal.fangs, mammal.color));
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
                    var reptile = (Reptile)animal;
                    builder.AppendLine(string.Format("{0},{1},{2}", reptile.bodyLength, reptile.habitat, reptile.tail));
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

        /// <summary>
        /// Checks the file extension of the provided file path and calls
        /// the appropriate loading method based on the file format.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        /// <exception cref="Exception"> Thrown when file format is not supported </exception>
        public void loadFromFile(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();

            switch (extension)
            {
                case ".txt":
                    loadText(filePath);
                    break;
                case ".json":
                    loadJson(filePath);
                    break;
                case ".xml":
                    loadXml(filePath);
                    break;
                default:
                    throw new Exception("File format not supported!");
            }
        }

        /// <summary>
        /// Loads animal data from a text file.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        private void loadText(string filePath)
        {
            var line = File.ReadAllLines(filePath);

            int i = 0;

            while (i < line.Length)
            {
                var baseInfo = line[i].Split(',');

                AnimalData animalData = new AnimalData
                {
                    name = baseInfo[1],
                    age = int.Parse(baseInfo[2]),
                    weight = double.Parse(baseInfo[3]),
                    gender = Enum.Parse<Genders>(baseInfo[4]),
                    image = new BitmapImage(new Uri(baseInfo[5], UriKind.RelativeOrAbsolute))
                };


                var typeInfo = line[i + 1].Split(',');
                string type = typeInfo[0];
                string species = typeInfo[1];

                IAnimal animal = null;

                if (type == "Mammal")
                {
                    var mammalInfo = line[i + 2].Split(',');
                    MammalData mammal = new MammalData
                    {
                        animalSpecies = species,
                        nrOfTeeth = int.Parse(baseInfo[6]),
                        fangs = baseInfo[7],
                        color = baseInfo[8]
                    };
                    switch (species)
                    {
                        case "Cat":
                            var catInfo = line[i + 3].Split(',');
                            animal = new Cat(animalData, mammal, catInfo[0], catInfo[1]);
                            break;
                        case "Dog":
                            var dogInfo = line[i + 3].Split('.');
                            animal = new Dog(animalData, mammal, dogInfo[0], dogInfo[1], dogInfo[2]);
                            break;
                        case "Cow":
                            var cowInfo = line[i + 3].Split(',');
                            animal = new Cow(animalData, mammal, cowInfo[0], int.Parse(cowInfo[1]), double.Parse(cowInfo[2]));
                            break;
                    }
                }
                else if (type == "Reptile")
                {
                    var reptileInfo = line[i + 2].Split(',');
                    ReptileData reptile = new ReptileData
                    {
                        animalSpecies = species,
                        bodyLength = double.Parse(reptileInfo[0]),
                        habitat = reptileInfo[1],
                        tail = reptileInfo[2]
                    };
                    switch (species)
                    {
                        case "Lizard":
                            var lizardInfo = line[i + 3].Split(',');
                            animal = new Lizard(animalData, reptile, lizardInfo[0]);
                            break;
                        case "Snake":
                            var snakeInfo = line[i + 3].Split(',');
                            animal = new Snake(animalData, reptile, snakeInfo[0], snakeInfo[1]);
                            break;
                        case "Turtle":
                            var turtleInfo = line[i + 3].Split(',');
                            animal = new Turtle(animalData, reptile, double.Parse(turtleInfo[0]), int.Parse(turtleInfo[1]));
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Loads animal data from a JSON file.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        private void loadJson(string filePath)
        {
            string json = File.ReadAllText(currentFilePath);
            var serial = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
            var animals = JsonConvert.DeserializeObject<List<IAnimal>>(json, serial);
            if (animals != null)
            {
                animalList = new ObservableCollection<IAnimal>(animals);
            }
        }

        /// <summary>
        /// Loads animal data from an XML file.
        /// Deserializes it into a list of Animal objects, and updates the animal list.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        private void loadXml(string filePath)
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
            using var stream = File.OpenRead(currentFilePath);
            var animals = (List<Animal>)serial.Deserialize(stream);
            animalList = new ObservableCollection<IAnimal>(animals);
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// </summary>
        /// <param name="animal"> Animal data</param>
        /// <returns> Xml friendly animal object </returns>
        /// <exception cref="Exception"> Thrown when the animal type is not supported </exception>
        private AnimalXml mapped(IAnimal animal)
        {
            var general = new AnimalXml
            {
                name = animal.name,
                age = animal.age,
                weight = animal.weight,
                gender = animal.gender,
                imagePath = animal.image.UriSource.ToString(),
                species = animal.species,
                type = animal.type
            };

            switch (animal.type)
            {
                case Types.Mammal:
                    var mammal = (Mammal)animal;
                    return createMammal(general, mammal);
                case Types.Reptile:
                    var reptile = (Reptile)animal;
                    return createReptile(general, reptile);
                default:
                    throw new Exception("Animal type not supported.");
            }
        }

        /// <summary>
        /// Maps mammal information to a xml friendly format.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="mammal"> Mammal data </param>
        /// <returns> Xml friendly animal object </returns>
        /// <exception cref="Exception"> Thrown when the animal type is not supported </exception>
        private AnimalXml createMammal(AnimalXml general, Mammal mammal)
        {
            var mammalXml = new MammalXml
            {
                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color
            };

            return mammal.species switch
            {
                "Dog" => mapDog(general, mammalXml, (Dog)mammal),
                "Cat" => mapCat(general, mammalXml, (Cat)mammal),
                "Cow" => mapCow(general, mammalXml, (Cow)mammal),
                _ => throw new Exception("Mammal species not supported.")
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="mammal"> Mammal data </param>
        /// <param name="dog"> Dog data </param>
        /// <returns> Xml friendly dog objekt </returns>
        private DogXml mapDog(AnimalXml general, MammalXml mammal, Dog dog)
        {
            return new DogXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                breed = dog.breed,
                chipped = dog.chipped,
                ears = dog.ears
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="mammal"> Mammal data </param>
        /// <param name="cat"> Cat data </param>
        /// <returns> Xml friendly cat objekt </returns>
        private CatXml mapCat(AnimalXml general, MammalXml mammal, Cat cat)
        {
            return new CatXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                breed = cat.breed,
                livingType = cat.livingType
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="mammal"> Mammal data </param>
        /// <param name="cow"> Cow data </param>
        /// <returns> Xml friendly cow objekt </returns>
        private CowXml mapCow(AnimalXml general, MammalXml mammal, Cow cow)
        {
            return new CowXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                tagged = cow.tagged,
                tagNumber = cow.tagNumber,
                milkContent = cow.milkContent
            };
        }

        /// <summary>
        /// Maps mammal information to a xml friendly format.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="reptile"> Reptile data </param>
        /// <returns> Xml friendly animal object </returns>
        /// <exception cref="Exception"> Thrown when the animal type is not supported </exception>
        private AnimalXml createReptile(AnimalXml general, Reptile reptile)
        {
            var reptileXml = new ReptileXml
            {
                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail
            };
            return reptile.species switch
            {
                "Lizard" => mapLizard(general, reptileXml, (Lizard)reptile),
                "Snake" => mapSnake(general, reptileXml, (Snake)reptile),
                "Turtle" => mapTurtle(general, reptileXml, (Turtle)reptile),
                _ => throw new Exception("Reptile species not supported.")
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="reptile"> Reptile data </param>
        /// <param name="lizard"> Lizard data </param>
        /// <returns> Xml friendly lizard objekt </returns>
        private LizardXml mapLizard(AnimalXml general, ReptileXml reptile, Lizard lizard)
        {
            return new LizardXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                venomous = lizard.venomous
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="reptile"> Reptile data </param>
        /// <param name="snake"> Snake data </param>
        /// <returns> Xml friendly snake objekt </returns>
        private SnakeXml mapSnake(AnimalXml general, ReptileXml reptile, Snake snake)
        {
            return new SnakeXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                venomous = snake.venom,
                pattern = snake.pattern
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="reptile"> Reptile data </param>
        /// <param name="turtle"> Turtle data </param>
        /// <returns> Xml friendly turtle objekt </returns>
        private TurtleXml mapTurtle(AnimalXml general, ReptileXml reptile, Turtle turtle)
        {
            return new TurtleXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                shellWidth = turtle.shellWidth,
                shellHardness = turtle.shellHardness
            };
        }
    }
}
