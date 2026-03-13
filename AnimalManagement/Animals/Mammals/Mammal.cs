using AnimalManagement.Controller;
using AnimalManagement.Animals.Common;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Represents a Mammal.
 */

namespace AnimalManagement.Animals.Mammals
{
    internal abstract class Mammal : Animal
    {
        public int nrOfTeeth { get; set; }
        public string fangs { get; set; }
        public string color { get; set; }
        public string specie { get; }
        public override string species => specie;

        /// <summary>
        /// Constructor for a mammal object.
        /// </summary>
        /// <param name="data"> Mammal data object </param>
        public Mammal(MammalData data)
        {
            nrOfTeeth = data.nrOfTeeth;
            fangs = data.fangs;
            color = data.color;
            specie = data.animalSpecies;
        }

        /// <summary>
        /// Creates a formatted string to represent a mammal object.
        /// </summary>
        /// <returns> A formatted string representing a mammal object </returns>
        public override string ToString()
        {
            string formatted = string.Format("{0}\nNumber of teeth: {1}\nFangs: {2}\nColor: {3}",
                base.ToString(), nrOfTeeth, fangs, color);
            return formatted;
        }

        /// <summary>
        /// Updates the mammal's number of teeth, fangs, and color based on the provided fields.
        /// </summary>
        /// <param name="fields">A collection of Fields containing values to update the mammal's properties.</param>
        protected void updateMammal(IEnumerable<Fields> fields)
        {
            if (fields == null)
            {
                return;
            }
            var fieldList = fields.Cast<Fields>().ToList();

            var teethField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "teeth", StringComparison.OrdinalIgnoreCase))?.Value;
            if (teethField != null)
            {
                nrOfTeeth = Convert.ToInt32(teethField);
            }

            var fangsField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "fangs", StringComparison.OrdinalIgnoreCase))?.Value;
            if (fangsField != null)
            {
                fangs = fangsField.ToString();
            }

            var colorField = fieldList.FirstOrDefault(f => string.Equals(f.bindName, "color", StringComparison.OrdinalIgnoreCase))?.Value;
            if(colorField != null)
            {
                color = colorField.ToString();
            }
        }
    }
}
