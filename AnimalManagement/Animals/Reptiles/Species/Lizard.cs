using AnimalManagement.Animals.Common;
using AnimalManagement.Controller;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-29
 * Description: Represents a Lizard object.
 */

namespace AnimalManagement.Animals.Reptiles.Species
{
    public class Lizard : Reptile
    {
        public string venomous { get; set; }

        public Lizard() { }

        /// <summary>
        /// Constrictor for a lizard object.
        /// </summary>
        /// <param name="data"> Data object of general reptile data </param>
        /// <param name="venom"> String value </param>
        public Lizard(AnimalData animal, ReptileData data, string venom) : base(data)
        {
            name = animal.name;
            age = animal.age;
            weight = animal.weight;
            gender = animal.gender;
            type = animal.type;
            image = animal.image;

            venomous = venom;
        }

        /// <summary>
        /// Updates the animal and reptile properties using the provided fields and sets the venomous property if a
        /// matching field is found.
        /// </summary>
        /// <param name="fields">A collection of Fields objects containing the data to update.</param>
        public override void update(IEnumerable<Fields> fields)
        {
            updateAnimal(fields);
            updateReptile(fields);
            var fieldList = fields.Cast<Fields>().ToList();

            var venom = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "venom", StringComparison.OrdinalIgnoreCase))?.Value;
            if (venom != null)
            {
                venomous = venom.ToString();
            }
        }

        /// <summary>
        /// Sets a general lifespan to the animal
        /// </summary>
        [XmlIgnore]
        public override int lifeSpan
        {
            get
            {
                return 2;
            }
        }

        /// <summary>
        /// Returns a general sleeping pattern
        /// </summary>
        [XmlIgnore]
        public override string sleep
        {
            get
            {
                return "Nocturnal sleep during day";
            }
        }

        /// <summary>
        /// Returns a dictionary of foods for the animal depending on habitat
        /// </summary>
        [XmlIgnore]
        public override Dictionary<string, string> food
        {
            get
            {
                switch (base.habitat)
                {
                    case "Desert":
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Insects, Arachnids, Lizards and plants"},
                            { "Amount", "5-7 feeding per week"},
                            { "Time", "Every 1-2 days" }
                        };
                    case "Forest":
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Insects, Worms and snails, Small vertebrate"},
                            { "Amount", "5-7 feeding per week"},
                            { "Time", "Every 1-2 days" }
                        };
                    case "Swamp":
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Insects, Amphibians, Fish, Snails and plants"},
                            { "Amount", "5-7 feeding per week\""},
                            { "Time", "Every 1-2 days" }
                        };
                    default:
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Insects, Arachnids, Small lizards, Plants"},
                            { "Amount", "5-7 feeding per week\""},
                            { "Time", "Every 1-2 days" }
                        };
                }
            }
        }

        /// <summary>
        /// A queue of events for animal
        /// </summary>
        [XmlIgnore]
        public override Queue<string> events => new Queue<string>(new[]
        {
            "Sun bath",
            "Chase the insects",
            "Lunch nap",
            "Try escape",
            "Stare viciously at veterinarian"
        });

        /// <summary>
        /// Creates a formatted string and calls base to string method
        /// </summary>
        /// <returns> Formatted string representing a lizard object </returns>
        public override string ToString()
        {
            string formatted = string.Format("{0}\nVenomous: {1}",
                base.ToString(), venomous);
            return formatted;
        }

        /// <summary>
        /// Formats a string representation of the animals lifespan
        /// </summary>
        /// <returns> Formatted string </returns>
        public override string toInfoString()
        {
            var formatted = new StringBuilder();

            formatted.AppendLine($"Name: {name}");
            formatted.AppendLine($"Sleeping habit: {sleep}");
            formatted.AppendLine($"Lifespan: {lifeSpan}-7 years");
            formatted.AppendLine();

            formatted.AppendLine($"Diet:");
            foreach(var pair in food)
            {
                formatted.AppendLine($" {pair.Key}: {pair.Value}");
            }
            formatted.AppendLine();
            formatted.AppendLine($"Events:");
            foreach(var thing in events)
            {
                formatted.AppendLine($" <--> {thing}");
            }
            return formatted.ToString();
        }
    }
}
