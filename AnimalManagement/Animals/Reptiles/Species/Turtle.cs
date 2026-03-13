using AnimalManagement.Animals.Common;
using AnimalManagement.Controller;
using System.Text;
using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-29
 * Description: Represents a turtle object.
 */

namespace AnimalManagement.Animals.Reptiles.Species
{
    internal class Turtle : Reptile
    {
        public double shellWidth { get; set; }
        public int shellHardness { get; set; }
        
        /// <summary>
        /// Constructor representing a turtle object.
        /// </summary>
        /// <param name="data"> Reptile data object </param>
        /// <param name="_shellWidth"> The size of the shell as a double </param>
        /// <param name="_shellHardness"> The hardness index a an int </param>
        public Turtle(AnimalData animal, ReptileData data, double _shellWidth, int _shellHardness) : base(data)
        {
            name = animal.name;
            age = animal.age;
            weight = animal.weight;
            gender = animal.gender;
            type = animal.type;
            image = animal.image;

            shellWidth = _shellWidth;
            shellHardness = _shellHardness;
        }

        /// <summary>
        /// Updates animal, reptile, and shell properties based on the provided fields.
        /// </summary>
        /// <param name="fields">A collection of Fields containing property values to update.</param>
        public override void update(IEnumerable<Fields> fields)
        {
            updateAnimal(fields);
            updateReptile(fields);
            var fieldList = fields.Cast<Fields>().ToList();

            var shellHard = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "shellHardness", StringComparison.OrdinalIgnoreCase))?.Value;
            if(shellHard != null)
            {
                shellHardness = Convert.ToInt32(shellHard);
            }

            var shellW = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "shellWidth", StringComparison.OrdinalIgnoreCase))?.Value;
            if(shellW != null)
            {
                shellWidth = Convert.ToDouble(shellW);
            }
        }

        /// <summary>
        /// Returns a dictionary of foods for the animal depending on habitat
        /// </summary>
        public override Dictionary<string, string> food
        {
            get
            {
                switch (base.habitat)
                {
                    case "Desert":
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Grass and flowers"},
                            { "Amount", "5-10% of body weight"},
                            { "Time", "Throughout the day" }
                        };
                    case "Forest":
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Plants and Insects"},
                            { "Amount", "5-10% of body weight"},
                            { "Time", "Morning and evening" }
                        };
                    case "Swamp":
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Fruits, Insects and fish"},
                            { "Amount", "5-10% of body weight"},
                            { "Time", "Morning and afternoon" }
                        };
                    default:
                        return new Dictionary<string, string>
                        {
                            { "Food type", "Fiber rich grasses and vegetables"},
                            { "Amount", "5-10% of body weight"},
                            { "Time", "Early mornings" }
                        };
                }
            }
        }

        /// <summary>
        /// Sets a general lifespan of animal
        /// </summary>
        public override int lifeSpan
        {
            get
            {
                return 30;
            }
        }

        /// <summary>
        /// Returns a general sleeping pattern
        /// </summary>
        public override string sleep
        {
            get
            {
                return "4-7 hours per night";
            }
        }

        /// <summary>
        /// A queue of events for animal
        /// </summary>
        public override Queue<string> events => new Queue<string>(new[]
        {
            "Morning bath",
            "Strolling looking for food",
            "Lunch nap",
            "Argue with the neighbour",
            "Supper time"
        });

        /// <summary>
        /// Creates a formatteed string representing a turtle object.
        /// </summary>
        /// <returns> A formetted string representing a turtle object </returns>
        public override string ToString()
        {
            string formatted = string.Format("{0}\nShell Width: {1}\nShell Hardness: {2}",
                base.ToString(), shellWidth, shellHardness);
            return formatted;
        }

        /// <summary>
        /// Formats a string representation of the animals foods and life span 
        /// </summary>
        /// <returns> Formatted string </returns>
        public override string toInfoString()
        {
            var formatted = new StringBuilder();

            formatted.AppendLine($"Name: {name}");
            formatted.AppendLine($"Sleeping habit: {sleep}");
            formatted.AppendLine($"Lifespan: {lifeSpan}-100 years");
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
