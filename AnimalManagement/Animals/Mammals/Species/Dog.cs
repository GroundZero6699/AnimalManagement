using AnimalManagement.Animals.Common;
using AnimalManagement.Controller;
using System.Text;
using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-29
 * Description: Represents a dog object.
 */

namespace AnimalManagement.Animals.Mammals.Species
{
    internal class Dog : Mammal
    {
        public string breed {  get; set; }
        public string chipped { get; set; }
        public string ears { get; set; }

        /// <summary>
        /// Constructor for a dog object.
        /// </summary>
        /// <param name="data"> Mammal data object </param>
        /// <param name="_breed"> Representing the dog breed </param>
        /// <param name="_chipped"> States id the dog is chipped </param>
        /// <param name="_ears"> Specifying ear type </param>
        public Dog(AnimalData animal, MammalData data, string _breed, string _chipped, string _ears) : base(data)
        {
            name = animal.name;
            age = animal.age;
            weight = animal.weight;
            type = animal.type;
            gender = animal.gender;
            image = animal.image;

            breed = _breed;
            chipped = _chipped;
            ears = _ears;
        }

        /// <summary>
        /// Updates the animal, mammal, and specific properties such as breed, chipped status, and ear type using the
        /// provided fields.
        /// </summary>
        /// <param name="fields">A collection of Fields containing the data to update.</param>
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
            
            var chippedField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "chipped", StringComparison.OrdinalIgnoreCase))?.Value;
            if (chippedField != null)
            {
                chipped = chippedField.ToString();
            }

            var earsField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "earType", StringComparison.OrdinalIgnoreCase))?.Value;
            if (earsField != null)
            {
                ears = earsField.ToString();
            }
        }

        /// <summary>
        /// Sets a general lifespan for animal
        /// </summary>
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
        public override string sleep
        {
            get
            {
                return "16 hours a day";
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
                    { "Food type", "Meat alt Fish"},
                    { "Amount", "550 g"},
                    { "Time", "Morning and evening" },
                    { "Snacks", "Liver pieces" }
                };
            }
        }

        /// <summary>
        /// A queue of events for animal
        /// </summary>
        public override Queue<string> events => new Queue<string>(new[]
        {
            "Eat breakfast",
            "Bark at neighbour",
            "Trash the place",
            "Escape to chase mailman",
            "Snatch the ham"
        });

        /// <summary>
        /// Creates a formatted string representing a dog object.
        /// </summary>
        /// <returns> A formatteed string representing a dog object </returns>
        public override string ToString()
        {
            string formatted = string.Format("{0}\nBreed: {1}\nChipped: {2}\nEar Type: {3}",
                base.ToString(), breed, chipped, ears);
            return formatted;
        }

        /// <summary>
        /// Formats a string representation of animal
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
