using AnimalManagement.Animals.Mammals;
using AnimalManagement.Animals.Mammals.Species;
using AnimalManagement.Manager.Serialize;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-18
 * Description: Maps mammal information to and from a xml friendly format.
 */

namespace AnimalManagement.Manager.Mapper
{
    public class MammalMapp
    {
        public Mammal fromXml(MammalXml mammal)
        {
            return mammal.species switch
            {
                "Dog" => createDog(mammal),
                "Cat" => createCat(mammal),
                "Cow" => createCow(mammal),
                _ => throw new Exception("Mammal species not supported.")
            };
        }

        /// <summary>
        /// Maps xml friendly mammal information to a dog object.
        /// </summary>
        /// <param name="mammal"> Mammal Xml object </param>
        /// <returns> Dog object </returns>
        private Dog createDog(MammalXml mammal)
        {
            var dog = new Dog
            {
                name = mammal.name,
                age = mammal.age,
                weight = mammal.weight,
                gender = mammal.gender,
                specie = mammal.species,
                type = mammal.type,
                imagePath = mammal.imagePath,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                breed = ((DogXml)mammal).breed,
                chipped = ((DogXml)mammal).chipped,
                ears = ((DogXml)mammal).ears
            };

            dog.loadImage();
            return dog;
        }

        /// <summary>
        /// Maps xml friendly mammal information to a cat object.
        /// </summary>
        /// <param name="mammal"> Mammal Xml object </param>
        /// <returns> Cat object </returns>
        private Cat createCat(MammalXml mammal)
        {
            var cat = new Cat
            {
                name = mammal.name,
                age = mammal.age,
                weight = mammal.weight,
                gender = mammal.gender,
                specie = mammal.species,
                type = mammal.type,
                imagePath = mammal.imagePath,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                breed = ((CatXml)mammal).breed,
                livingType = ((CatXml)mammal).livingType
            };

            cat.loadImage();
            return cat;
        }

        /// <summary>
        /// Maps xml friendly mammal information to a cow object.
        /// </summary>
        /// <param name="mammal"> Mammal Xml object </param>
        /// <returns> Cow object </returns>
        private Cow createCow(MammalXml mammal)
        {
            var cow = new Cow
            {
                name = mammal.name,
                age = mammal.age,
                weight = mammal.weight,
                gender = mammal.gender,
                specie = mammal.species,
                type = mammal.type,
                imagePath = mammal.imagePath,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                tagged = ((CowXml)mammal).tagged,
                tagNumber = ((CowXml)mammal).tagNumber,
                milkContent = ((CowXml)mammal).milkContent
            };

            cow.loadImage();
            return cow;
        }
        /// <summary>
        /// Maps mammal information to a xml friendly format.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="mammal"> Mammal data </param>
        /// <returns> Xml friendly animal object </returns>
        /// <exception cref="Exception"> Thrown when the animal type is not supported </exception>
        public MammalXml toXml(Mammal mammal)
        {
            var general = new AnimalXml
            {
                name = mammal.name,
                age = mammal.age,
                weight = mammal.weight,
                gender = mammal.gender,
                imagePath = mammal.imagePath,
                type = mammal.type,
                species = mammal.species,
            };

            var mammalXml = new MammalXml
            {
                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color
            };

            return mammal.species switch
            {
                "Dog" => mapDog(general, mammalXml, (Dog)mammal),
                "Cat" => mapCat(general, mammalXml, (Cat)mammal),
                "Cow" => mapCow(general, mammalXml, (Cow)mammal),
                _ => throw new Exception("Mammal species not supported.")
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="mammal"> Mammal data </param>
        /// <param name="dog"> Dog data </param>
        /// <returns> Xml friendly dog objekt </returns>
        private DogXml mapDog(AnimalXml general, MammalXml mammal, Dog dog)
        {
            return new DogXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                breed = dog.breed,
                chipped = dog.chipped,
                ears = dog.ears
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="mammal"> Mammal data </param>
        /// <param name="cat"> Cat data </param>
        /// <returns> Xml friendly cat objekt </returns>
        private CatXml mapCat(AnimalXml general, MammalXml mammal, Cat cat)
        {
            return new CatXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                breed = cat.breed,
                livingType = cat.livingType
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="mammal"> Mammal data </param>
        /// <param name="cow"> Cow data </param>
        /// <returns> Xml friendly cow objekt </returns>
        private CowXml mapCow(AnimalXml general, MammalXml mammal, Cow cow)
        {
            return new CowXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,

                nrOfTeeth = mammal.nrOfTeeth,
                fangs = mammal.fangs,
                color = mammal.color,

                tagged = cow.tagged,
                tagNumber = cow.tagNumber,
                milkContent = cow.milkContent
            };
        }
    }
}
