using AnimalManagement.Animals;
using AnimalManagement.Animals.Mammals;
using AnimalManagement.Animals.Reptiles;
using AnimalManagement.Manager.Serialize;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-18
 * Description: Maps animal information to and from a xml friendly format.
 */

namespace AnimalManagement.Manager.Mapper
{
    public class AnimalMapper
    {
        private readonly MammalMapp mammal = new();
        private readonly ReptileMapp reptile = new();

        /// <summary>
        /// Maps an IAnimal to an AnimalXml.
        /// The type of the animal is determined by the type property of the IAnimal, 
        /// and the appropriate mapping method is called based on that type.
        /// </summary>
        /// <param name="animal"> Animal object </param>
        /// <returns> AnimalXml object </returns>
        /// <exception cref="ArgumentException"> Throws message on error </exception>
        public AnimalXml toXml(IAnimal animal)
        {
            return animal.type switch
            {
                Types.Mammal => mammal.toXml((Mammal)animal),
                Types.Reptile => reptile.toXml((Reptile)animal),
                _ => throw new ArgumentException("Unsupported type")
            };
        }

        /// <summary>
        /// Maps an AnimalXml to an IAnimal.
        /// </summary>
        /// <param name="animal"> AnimalXml object </param>
        /// <returns> Animal object </returns>
        /// <exception cref="ArgumentException"> Throws message on error </exception>
        public IAnimal fromXml(AnimalXml animal)
        {
            return animal.type switch
            {
                Types.Mammal => mammal.fromXml((MammalXml)animal),
                Types.Reptile => reptile.fromXml((ReptileXml)animal),
                _ => throw new ArgumentException("Unsupported type")
            };
        }
    }
}
