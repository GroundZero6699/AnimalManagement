/*
 * Author: Christoffer Wiik
 * Date: 2026-02-04
 * Description: Data class representing the general data for a Reptile.
 */

using AnimalManagement.Animals.Reptiles;

namespace AnimalManagement.Controller
{
    /// <summary>
    /// General data for a reptile.
    /// </summary>
    public class ReptileData
    {
        public double bodyLength { get; set; }
        public string habitat { get; set; }
        public string tail { get; set; }
        public string animalSpecies { get; set; }
    }
}
