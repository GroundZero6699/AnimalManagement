using AnimalManagement.Animals.Common;
using AnimalManagement.Controller;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-29
 * Description: Represents a cat object.
 */

namespace AnimalManagement.Animals.Mammals.Species
{
    internal class Cat : Mammal
    {
        public string breed {  get; set; }
        public string livingType { get; set; }

        public Cat() { }

        /// <summary>
        /// Constructor representing a cat object.
        /// </summary>
        /// <param name="data"> Mammal data object </param>
        /// <param name="_breed"> Specifies the breed </param>
        /// <param name="_livingType"> Specifies living area "indoor, outdoor, mixed" </param>
        public Cat(AnimalData animal, MammalData data, string _breed, string _livingType) : base(data)
        {
            name = animal.name;
            age = animal.age;
            weight = animal.weight;
            gender = animal.gender;
            type = animal.type;
            image = animal.image;

            breed = _breed;
            livingType = _livingType;
        }

        /// <summary>
        /// Updates the animal, mammal, breed, and living type properties using the provided fields.
        /// </summary>
        /// <param name="fields">A collection of Fields objects containing property values to update.</param>
        public override void update(IEnumerable<Fields> fields)
        {
            updateAnimal(fields);
            updateMammal(fields);
            var fieldList = fields.Cast<Fields>().ToList();

            var breedField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "breed", StringComparison.OrdinalIgnoreCase))?.Value;
            if (breedField != null)
            {
                breed = breedField.ToString();
            }

            var living = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "livingType", StringComparison.OrdinalIgnoreCase))?.Value;
            if (living != null)
            {
                livingType = living.ToString();
            }
        }

        /// <summary>
        /// Sets a general lifespan for animal
        /// </summary>
        public override int lifeSpan
        {
            get
            {
                return 15;
            }
        }

        /// <summary>
        /// Returns a general sleeping pattern
        /// </summary>
        public override string sleep
        {
            get
            {
                return "15 hours a day";
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
                    { "Food type", "Fresh Fish"},
                    { "Amount", "150g"},
                    { "Time", "Morning and evening" }
                };
            }
        }

        /// <summary>
        /// A queue of events for animal
        /// </summary>
        public override Queue<string> events => new Queue<string>(new[]
        {
            "Jump on hooman",
            "Whine about food for 2 h",
            "Climb curtains",
            "Stare at my hooman minions",
            "Sleep"
        });

        /// <summary>
        /// Creates a formatted string representing a cat object.
        /// </summary>
        /// <returns> A formatted string representing a cat object </returns>
        public override string ToString()
        {
            string formatted = string.Format("{0}\nBreed: {1}\nLiving Style: {2}",
                base.ToString(), breed, livingType);
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
            formatted.AppendLine($"Lifespan: {lifeSpan}-20 years");
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
