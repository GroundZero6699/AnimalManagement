using AnimalManagement.Animals.Common;
using AnimalManagement.Controller;
using System.Text;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-29
 * Description: Represents a snake object.
 */

namespace AnimalManagement.Animals.Reptiles.Species
{
    public class Snake : Reptile
    {
        public string venom {  get; set; }
        public string pattern { get; set; }

        public Snake() { }
        
        /// <summary>
        /// Constructor for a snake object.
        /// </summary>
        /// <param name="data"> Data object for general reptile data </param>
        /// <param name="_venom"> Value for state if snake is venomous or not </param>
        /// <param name="_pattern"> Value representing visuals of snake </param>
        public Snake(AnimalData animal, ReptileData data, string _venom, string _pattern) : base(data)
        {
            name = animal.name;
            age = animal.age;
            weight = animal.weight;
            gender = animal.gender;
            type = animal.type;
            image = animal.image;

            venom = _venom;
            pattern = _pattern;
        }

        /// <summary>
        /// Updates the animal and reptile properties using the provided fields, including venom and pattern
        /// information.
        /// </summary>
        /// <param name="fields">A collection of Fields objects containing updated property values.</param>
        public override void update(IEnumerable<Fields> fields)
        {
            updateAnimal(fields);
            updateReptile(fields);
            var fieldList = fields.Cast<Fields>().ToList();

            var venomous = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "venom", StringComparison.OrdinalIgnoreCase))?.Value;
            if (venomous != null)
            {
                venom = venomous.ToString();
            }

            var patternField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "pattern", StringComparison.OrdinalIgnoreCase))?.Value;
            if(patternField != null)
            {
                pattern = patternField.ToString();
            }
        }

        /// <summary>
        /// Sets a general lifespan of the animal
        /// </summary>
        [XmlIgnore]
        public override int lifeSpan
        {
            get
            {
                return 10;
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
                return "16 hours a day";
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
                            { "Food type", "Mice/Rats, Lizards and bugs"},
                            { "Size", "1-1.5% of bodys widest section"},
                            { "Time", "Every 5-7 days" }
                        };
                    case "Forest":
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Birds, Eggs, Mice and frogs"},
                            { "Size", "1-1.5% of bodys widest section"},
                            { "Time", "Every 5-7 days" }
                        };
                    case "Swamp":
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Amphibians, Fish, Snails and worms"},
                            { "Size", "1-1.5% of bodys widest section"},
                            { "Time", "Every 5-7 days" }
                        };
                    default:
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Mice/Rabbits, Bugs, Birds and eggs"},
                            { "Size", "1-1.5% of bodys widest section"},
                            { "Time", "Every 5-7 days" }
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
            "Strangle some food",
            "Hide from the vet",
            "Get vaccinated"
        });

        /// <summary>
        /// Creates a formatted string representing a snake object.
        /// </summary>
        /// <returns> A formatted string representing a snake object </returns>
        public override string ToString()
        {
            string formatted = string.Format("{0}\nVenomous: {1}\nPattern: {2}",
                base.ToString(), venom, pattern);
            return formatted;
        }

        /// <summary>
        /// Formats a string representation of the animals foods and lifespan
        /// </summary>
        /// <returns> Formatted string </returns>
        public override string toInfoString()
        {
            var formatted = new StringBuilder();

            formatted.AppendLine($"Name: {name}");
            formatted.AppendLine($"Sleeping habit: {sleep}");
            formatted.AppendLine($"Lifespan: {lifeSpan}-15 years");
            formatted.AppendLine();

            formatted.AppendLine($"Diet:");
            foreach (var pair in food)
            {
                formatted.AppendLine($" {pair.Key}: {pair.Value}");
            }
            formatted.AppendLine();
            formatted.AppendLine($"Events:");
            foreach (var thing in events)
            {
                formatted.AppendLine($" <--> {thing}");
            }
            return formatted.ToString();
        }
    }
}
