using AnimalManagement.Animals;
using AnimalManagement.Animals.Mammals;
using AnimalManagement.Animals.Mammals.Species;
using AnimalManagement.Animals.Reptiles;
using AnimalManagement.Animals.Reptiles.Species;
using AnimalManagement.Controller;
using AnimalManagement.Manager.Mapper;
using AnimalManagement.Manager.Serialize;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
        private readonly AnimalMapper mapper = new();
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
            var list = animalList.Select(animal => mapper.toXml(animal)).ToList();


            var json = System.Text.Json.JsonSerializer.Serialize(list, jsonoption);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Serializes the animal list to XML format and saves it to the specified file path.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        private void saveAsXml(string filePath)
        {
            var list = animalList.Select(animal => mapper.toXml(animal)).ToList();

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

                builder.AppendLine(string.Format("{0},{1},{2},{3},{4},{5}",
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
                    image = new BitmapImage(new Uri(baseInfo[5], UriKind.RelativeOrAbsolute)),
                    imagePath = baseInfo[5]
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
        /// <exception cref="ArgumentException"> Thrown when file path is null </exception>
        private void loadJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var document = System.Text.Json.JsonSerializer.Deserialize<List<JsonElement>>(json, jsonoption);

            var animalXml = document.Select(doc => chopIt(doc)).ToList();

            var animals = animalXml.Select(anim => mapper.fromXml(anim)).ToList();

            foreach(var a in animals)
            {
                if (!string.IsNullOrEmpty(a.imagePath))
                {
                    a.loadImage();
                }
            }

            animalList = new ObservableCollection<IAnimal>(animals);
        }

        /// <summary>
        /// Sets up the JsonSerializerOptions for deserialization, 
        /// including handling of fields and enum values as strings.
        /// </summary>
        private readonly JsonSerializerOptions jsonoption = new()
        {
            IncludeFields = true,
            Converters = { new JsonStringEnumConverter() }
        };

        /// <summary>
        /// Deserializes a JsonElement into an AnimalXml object based on the "type" and "species" properties.
        /// </summary>
        /// <param name="element"> Json element representing animal entry </param>
        /// <returns> AnimalXml subclass </returns>
        /// <exception cref="Exception"> Thrown when unknown type or species is encountered </exception>
        private AnimalXml chopIt(JsonElement element)
        {
            string species = element.GetProperty("species").GetString();
            string type = element.GetProperty("type").GetString();

            return type switch
            {
                "Mammal" => species switch
                {
                    "Cat" => element.Deserialize<CatXml>(jsonoption),
                    "Dog" => element.Deserialize<DogXml>(jsonoption),
                    "Cow" => element.Deserialize<CowXml>(jsonoption),
                    _ => throw new Exception("Unknown species")
                },

                "Reptile" => species switch
                {
                    "Lizard" => element.Deserialize<LizardXml>(jsonoption),
                    "Snake" => element.Deserialize<SnakeXml>(jsonoption),
                    "Turtle" => element.Deserialize<TurtleXml>(jsonoption),
                    _ => throw new Exception("Unknown species")
                },

                _ => throw new Exception("Unknown type")
            };
        }

        /// <summary>
        /// Loads animal data from an XML file.
        /// Deserializes it into a list of Animal objects, and updates the animal list.
        /// </summary>
        /// <param name="filePath"> Path to file </param>
        private void loadXml(string filePath)
        {
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
            using var stream = File.OpenRead(filePath);
            var animalsXml = (List<AnimalXml>)serial.Deserialize(stream);

            var animals = animalsXml.Select(an => mapper.fromXml(an)).ToList();

            foreach (var a in animals)
            {
                if (!string.IsNullOrEmpty(a.imagePath))
                {
                    a.loadImage();
                }
            }
            animalList = new ObservableCollection<IAnimal>(animals);
        }
    }
}
