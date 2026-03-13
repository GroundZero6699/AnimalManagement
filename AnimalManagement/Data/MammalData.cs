/*
 * Author: Christoffer Wiik
 * Date: 2026-02-04
 * Description: General data class represents general mammal data.
 */

using AnimalManagement.Animals;
using AnimalManagement.Animals.Mammals;

namespace AnimalManagement.Controller
{
    /// <summary>
    /// General data for a mammal type.
    /// </summary>
    internal class MammalData
    {
        public int nrOfTeeth { get; set; }
        public string fangs { get; set; }
        public string color { get; set; }
        public string animalSpecies { get; set; }
    }
}
