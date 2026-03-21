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
            var lines = animalList.Where(animal => animal != null).Select(animal =>
            {
                var list = new List<string>();

                list.Add($"{animal.id},{animal.name},{animal.age},{animal.weight},{animal.gender},{animal.imagePath}");

                list.Add($"{animal.type},{animal.species}");

                if (animal is Mammal mammal)
                {
                    list.Add($"{mammal.nrOfTeeth},{mammal.fangs},{mammal.color}");
                    switch (animal.species)
                    {
                        case "Cat":
                            var cat = (Cat)mammal;
                            list.Add($"{cat.breed},{cat.livingType}");
                            break;
                        case "Dog":
                            var dog = (Dog)mammal;
                            list.Add($"{dog.breed},{dog.chipped},{dog.ears}");
                            break;
                        case "Cow":
                            var cow = (Cow)mammal;
                            list.Add($"{cow.tagged},{cow.tagNumber},{cow.milkContent}");
                            break;
                    }
                }
                else if (animal is Reptile reptile)
                {
                    list.Add($"{reptile.bodyLength},{reptile.habitat},{reptile.tail}");
                    switch (animal.species)
                    {
                        case "Lizard":
                            var lizard = (Lizard)reptile;
                            list.Add($"{lizard.venomous}");
                            break;
                        case "Snake":
                            var snake = (Snake)reptile;
                            list.Add($"{snake.venom},{snake.pattern}");
                            break;
                        case "Turtle":
                            var turtle = (Turtle)reptile;
                            list.Add($"{turtle.shellWidth},{turtle.shellHardness}");
                            break;
                    }
                }
                return list;
            });

            return string.Join(Environment.NewLine, lines);
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
            var lines = File.ReadAllLines(filePath);

            animalList = new ObservableCollection<IAnimal>(
                Enumerable
                    .Range(0, lines.Length)
                    .Where(i => i % 4 == 0)
                    .Select(i => parseAnimal(lines, i))
                    .Where(a => a != null));
        }

        private IAnimal parseAnimal(string[] lines, int i)
        {
            {
                var baseInfo = lines[i].Split(',');
                var type = lines[i + 1].Split(',');
                var species = lines[i + 2].Split(',');
                var specific = lines[i + 3].Split(',');

                var data = new AnimalData
                {
                    name = baseInfo[1],
                    age = int.Parse(baseInfo[2]),
                    weight = double.Parse(baseInfo[3]),
                    gender = Enum.Parse<Genders>(baseInfo[4]),
                    image = new BitmapImage(new Uri(baseInfo[5], UriKind.RelativeOrAbsolute)),
                    imagePath = baseInfo[5],
                };

                string typeString = type[0];
                string speciesString = type[1];

                if (typeString == "Mammal")
                {
                    var mammal = new MammalData
                    {
                        animalSpecies = speciesString,
                        nrOfTeeth = int.Parse(specific[0]),
                        fangs = specific[1],
                        color = specific[2]
                    };

                    return speciesString switch
                    {
                        "Cat" => new Cat(data, mammal, specific[0], specific[1]),
                        "Dog" => new Dog(data, mammal, specific[0], specific[1], specific[2]),
                        "Cow" => new Cow(data, mammal, specific[0], int.Parse(specific[1]), double.Parse(specific[2])),
                        _ => throw new Exception("Unknown species")
                    };
                }
                else if (typeString == "Reptile")
                {
                    var reptile = new ReptileData
                    {
                        animalSpecies = speciesString,
                        bodyLength = double.Parse(specific[0]),
                        habitat = specific[1],
                        tail = specific[2]
                    };
                    return speciesString switch
                    {
                        "Lizard" => new Lizard(data, reptile, specific[3]),
                        "Snake" => new Snake(data, reptile, specific[3], specific[4]),
                        "Turtle" => new Turtle(data, reptile, double.Parse(specific[3]), int.Parse(specific[4])),
                        _ => throw new Exception("Unknown species")
                    };
                }
            }
            return null;
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
