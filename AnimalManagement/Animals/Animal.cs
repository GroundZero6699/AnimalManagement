using AnimalManagement.Animals.Common;
using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Represents a Animal.
 */

namespace AnimalManagement.Animals
{
    /// <summary>
    /// Base class representing the highest level of each animal
    /// holds general animal data.
    /// </summary>
    public abstract class Animal : IAnimal
    {
        private static int _id = 1;
        public int id { get; }
        public string name { get; protected set; }
        public int age { get; protected set; }
        public double weight { get; protected set; }
        public Genders gender { get; protected set; }
        public BitmapImage image { get; protected set; }
        public virtual string species => GetType().Name;
        public Types type { get; protected set; }
        public virtual string sleep => "Unknown sleeping habits";
        public abstract int lifeSpan { get; }
        public abstract Dictionary<string, string> food { get; }
        public abstract Queue<string> events { get; }

        /// <summary>
        /// Constructor representing a animal.
        /// Sets a uniqe id number to each animal.
        /// </summary>
        public Animal() 
        {
            id = _id++;
        }

        /// <summary>
        /// Creates a formatted string representing a animals general data.
        /// </summary>
        /// <returns> A formatted string representing the general data for a animal </returns>
        public override string ToString()
        {
            string formatted = string.Format("Id: {0}\nName: {1}\nAge: {2}\nWeight: {3:F2} KG\nGender: {4}",
                id, name, age, weight, gender);
            return formatted;
        }

        /// <summary>
        /// Updates the animal's name, age, weight, and image properties based on the provided collection of fields.
        /// </summary>
        /// <param name="fields">A collection of Fields objects containing updated values for the animal's properties.</param>
        protected void updateAnimal(IEnumerable<Fields> fields)
        {
            if(fields == null)
            {
                return;
            }
            var fieldList = fields.Cast<Fields>().ToList();

            var nameField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "name", StringComparison.OrdinalIgnoreCase))?.Value;
            if (nameField != null)
            {
                name = nameField.ToString();
            }

            var ageField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "age", StringComparison.OrdinalIgnoreCase))?.Value;
            if (ageField != null && int.TryParse(ageField.ToString(), out int ageValue))
            {
                age = ageValue;
            }

            var weightField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "weight", StringComparison.OrdinalIgnoreCase))?.Value;
            if (weightField != null && double.TryParse(weightField.ToString(), out double weightValue))
            {
                weight = weightValue;
            }
            
            var imageField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "image", StringComparison.OrdinalIgnoreCase))?.Value;
            if (imageField != null && imageField is BitmapImage bitmapImage)
            {
                image = bitmapImage;
            }
        }

        /// <summary>
        /// Updates the specified collection of fields.
        /// </summary>
        /// <param name="fields">A collection of Fields objects to be updated.</param>
        public abstract void update(IEnumerable<Fields> fields);

        /// <summary>
        /// Creates a formatted string representation of a animal
        /// </summary>
        /// <returns> Formatted string </returns>
        public abstract string toInfoString();
    }
}
