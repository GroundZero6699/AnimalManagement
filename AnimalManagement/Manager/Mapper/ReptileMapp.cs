using AnimalManagement.Animals.Reptiles;
using AnimalManagement.Animals.Reptiles.Species;
using AnimalManagement.Manager.Serialize;
using System.Windows;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-18
 * Description: Maps reptile information to and from a xml friendly format.
 */

namespace AnimalManagement.Manager.Mapper
{
    public class ReptileMapp
    {
        /// <summary>
        /// Maps xml friendly animal information to a reptile object.
        /// </summary>
        /// <param name="reptile"> ReptileXml object </param>
        /// <returns> Reptile object </returns>
        /// <exception cref="Exception"> Throws Message on error </exception>
        public Reptile fromXml(ReptileXml reptile)
        {
            return reptile.species switch
            {
                "Lizard" => createLizard(reptile),
                "Snake" => createSnake(reptile),
                "Turtle" => createTurtle(reptile),
                _ => throw new Exception("Reptile species not supported.")
            };
        }

        /// <summary>
        /// Maps xml friendly animal information to a lizard object.
        /// </summary>
        /// <param name="reptile"> ReptileXml object </param>
        /// <returns> Lizard object </returns>
        private Lizard createLizard(ReptileXml reptile)
        {
            var lizard = new Lizard
            {
                name = reptile.name,
                age = reptile.age,
                weight = reptile.weight,
                gender = reptile.gender,
                specie = reptile.species,
                type = reptile.type,
                imagePath = reptile.imagePath,

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                venomous = ((LizardXml)reptile).venomous
            };

            MessageBox.Show($"Loading image for {lizard.name} from path: {lizard.imagePath}");
            lizard.loadImage();

            return lizard;
        }

        /// <summary>
        /// Maps xml friendly animal information to a turtle object.
        /// </summary>
        /// <param name="reptile"> ReptileXml object </param>
        /// <returns> Snake object </returns>
        private Snake createSnake(ReptileXml reptile)
        {
            var snake = new Snake
            {
                name = reptile.name,
                age = reptile.age,
                weight = reptile.weight,
                gender = reptile.gender,
                specie = reptile.species,
                type = reptile.type,
                imagePath = reptile.imagePath,

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                venom = ((SnakeXml)reptile).venomous,
                pattern = ((SnakeXml)reptile).pattern
            };

            snake.loadImage();

            return snake;
        }

        /// <summary>
        /// Maps xml friendly animal information to a turtle object.
        /// </summary>
        /// <param name="reptile"> ReptileXml object </param>
        /// <returns> Turtle object </returns>
        private Turtle createTurtle(ReptileXml reptile)
        {
            var turtle = new Turtle
            {
                name = reptile.name,
                age = reptile.age,
                weight = reptile.weight,
                gender = reptile.gender,
                specie = reptile.species,
                type = reptile.type,
                imagePath = reptile.imagePath,

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                shellWidth = ((TurtleXml)reptile).shellWidth,
                shellHardness = ((TurtleXml)reptile).shellHardness
            };

            turtle.loadImage();

            return turtle;
        }
        /// <summary>
        /// Maps mammal information to a xml friendly format.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="reptile"> Reptile data </param>
        /// <returns> Xml friendly animal object </returns>
        /// <exception cref="Exception"> Thrown when the animal type is not supported </exception>
        public AnimalXml toXml(Reptile reptile)
        {
            var general = new AnimalXml
            {
                name = reptile.name,
                age = reptile.age,
                weight = reptile.weight,
                gender = reptile.gender,
                imagePath = reptile.imagePath,
                type = reptile.type,
                species = reptile.species,
                derivedType = reptile.GetType().Name
            };

            var reptileXml = new ReptileXml
            {
                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail
            };
            return reptile.species switch
            {
                "Lizard" => mapLizard(general, reptileXml, (Lizard)reptile),
                "Snake" => mapSnake(general, reptileXml, (Snake)reptile),
                "Turtle" => mapTurtle(general, reptileXml, (Turtle)reptile),
                _ => throw new Exception("Reptile species not supported.")
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="reptile"> Reptile data </param>
        /// <param name="lizard"> Lizard data </param>
        /// <returns> Xml friendly lizard objekt </returns>
        private LizardXml mapLizard(AnimalXml general, ReptileXml reptile, Lizard lizard)
        {
            return new LizardXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,
                derivedType = nameof(LizardXml),

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                venomous = lizard.venomous
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="reptile"> Reptile data </param>
        /// <param name="snake"> Snake data </param>
        /// <returns> Xml friendly snake objekt </returns>
        private SnakeXml mapSnake(AnimalXml general, ReptileXml reptile, Snake snake)
        {
            return new SnakeXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,
                derivedType = nameof(SnakeXml),

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                venomous = snake.venom,
                pattern = snake.pattern
            };
        }

        /// <summary>
        /// Maps animal information to a xml friendly format.
        /// To be serialized and saved to file.
        /// </summary>
        /// <param name="general"> Animal data </param>
        /// <param name="reptile"> Reptile data </param>
        /// <param name="turtle"> Turtle data </param>
        /// <returns> Xml friendly turtle objekt </returns>
        private TurtleXml mapTurtle(AnimalXml general, ReptileXml reptile, Turtle turtle)
        {
            return new TurtleXml
            {
                name = general.name,
                age = general.age,
                weight = general.weight,
                gender = general.gender,
                imagePath = general.imagePath,
                type = general.type,
                species = general.species,
                derivedType = nameof(TurtleXml),

                bodyLength = reptile.bodyLength,
                habitat = reptile.habitat,
                tail = reptile.tail,

                shellWidth = turtle.shellWidth,
                shellHardness = turtle.shellHardness
            };
        }
    }
}
