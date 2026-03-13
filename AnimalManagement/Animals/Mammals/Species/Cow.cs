using AnimalManagement.Animals.Common;
using AnimalManagement.Controller;
using System.Text;
using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-29
 * Description: Represents a cow object.
 */

namespace AnimalManagement.Animals.Mammals.Species
{
    internal class Cow : Mammal
    {
        public string tagged { get; set; }
        public int tagNumber { get; set; }
        public double milkContent { get; set; }

        /// <summary>
        /// Constructor for a cow object.
        /// </summary>
        /// <param name="data"> Mammal data object </param>
        /// <param name="_tagged"> States if object is tagged </param>
        /// <param name="_tagNumber"> Specifies the tag number </param>
        /// <param name="_milkContent"> A numeric value of content of milk </param>
        public Cow(AnimalData animal, MammalData data, string _tagged, int _tagNumber, double _milkContent) : base(data)
        {
            name = animal.name;
            age = animal.age;
            weight = animal.weight;
            gender = animal.gender;
            type = animal.type;
            image = animal.image;

            tagged = _tagged;
            tagNumber = _tagNumber;
            milkContent = _milkContent;
        }

        /// <summary>
        /// Updates the animal, mammal, and specific properties such as tagged, tagNumber, and milkContent using the
        /// provided fields.
        /// </summary>
        /// <param name="fields">A collection of Fields objects containing the data to update.</param>
        public override void update(IEnumerable<Fields> fields)
        {
            updateAnimal(fields);
            updateMammal(fields);
            var fieldList = fields.Cast<Fields>().ToList();

            var tag = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "tagged", StringComparison.OrdinalIgnoreCase))?.Value;
            if (tag != null)
            {
                tagged = tag.ToString();
            }

            var tagNum = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "tagNumber", StringComparison.OrdinalIgnoreCase))?.Value;
            if (tagNum != null && int.TryParse(tagNum.ToString(), out int parsedTagNum))
            {
                tagNumber = parsedTagNum;
            }

            var milk = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "milkContent", StringComparison.OrdinalIgnoreCase))?.Value;
            if(milk != null && double.TryParse(milk.ToString(), out double parsedMilkContent))
            {
                milkContent = parsedMilkContent;
            }
        }

        /// <summary>
        /// Sets a general lifespan for animal
        /// </summary>
        public override int lifeSpan
        {
            get
            {
                return 6;
            }
        }

        /// <summary>
        /// Returns a general sleeping pattern
        /// </summary>
        public override string sleep
        {
            get
            {
                return "Sleeps whenever";
            }
        }

        /// <summary>
        /// Creates a dictionary of foods for animal
        /// </summary>
        public override Dictionary<string, string> food
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "Food type", "Hay"},
                    { "Amount", "1 - 2 Bales"},
                    { "Time", "Lunch time" },
                    { "Daily", "Vitamin supplements" }
                };
            }
        }

        /// <summary>
        /// A queue of events for animal
        /// </summary>
        public override Queue<string> events => new Queue<string>(new[]
        {
            "Stare at people",
            "Chew grass",
            "Roam the fields",
            "Get panic over nothing",
            "Refuse to go inside"
        });

        /// <summary>
        /// Creates a formatted string representing a cow object.
        /// </summary>
        /// <returns> A formatted string representing a cow object </returns>
        public override string ToString()
        {
            string formatted = string.Format("{0}\nTagged: {1}\nTag Number: {2}\nMilk Content: {3}",
                base.ToString(), tagged, tagNumber, milkContent);
            return formatted;
        }

        /// <summary>
        /// Formats a string representation of an animal
        /// </summary>
        /// <returns> Formatted string </returns>
        public override string toInfoString()
        {
            var formatted = new StringBuilder();

            formatted.AppendLine($"Name: {name}");
            formatted.AppendLine($"Sleeping habit: {sleep}");
            formatted.AppendLine($"Lifespan: {lifeSpan}-10 years");
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
