using AnimalManagement.Controller;
using AnimalManagement.Animals.Common;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Represents a Reptile.
 */

namespace AnimalManagement.Animals.Reptiles
{
    internal abstract class Reptile : Animal
    {
        public double bodyLength { get; set; }
        public string habitat { get; set; }
        public string tail { get; set; }
        public string specie {  get; }
        public override string species => specie;

        /// <summary>
        /// Constructor representing a reptile object.
        /// </summary>
        /// <param name="data"> Data object of a reptiles general data </param>
        public Reptile(ReptileData data)
        {
            bodyLength = data.bodyLength;
            habitat = data.habitat;
            tail = data.tail;
            specie = data.animalSpecies;
        }

        /// <summary>
        /// Creates a formatted string representing a reptile object.
        /// </summary>
        /// <returns> A formatted string representing a reptile object </returns>
        public override string ToString()
        {
            string formatted = string.Format("{0}\nBody Length: {1}\nHabitat: {2}\nTail: {3}",
                base.ToString(), bodyLength, habitat, tail);
            return formatted;
        }

        /// <summary>
        /// Updates the reptile's body length, habitat, and tail properties based on the provided collection of fields.
        /// </summary>
        /// <param name="fields">A collection of Fields objects containing updated values for the reptile's properties.</param>
        protected void updateReptile(IEnumerable<Fields> fields)
        {
            if (fields == null)
            {
                return;
            }

            var fieldList = fields.Cast<Fields>().ToList();

            var length = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "bodyLength", StringComparison.OrdinalIgnoreCase))?.Value;
            if (length != null)
            {
                bodyLength = Convert.ToDouble(fieldList.First(f => f.bindName == "bodyLength").Value);
            }

            var habitatField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "habitat", StringComparison.OrdinalIgnoreCase))?.Value;
            if (habitatField != null)
            {
                habitat = habitatField.ToString();
            }

            var tailField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "tail", StringComparison.OrdinalIgnoreCase))?.Value;
            if (tailField != null)
            {
                tail = tailField.ToString();
            }
        }
    }
}
