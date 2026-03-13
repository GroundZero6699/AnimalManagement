using AnimalManagement.Animals.Common;
using AnimalManagement.Animals.Mammals.Species;
using AnimalManagement.Animals.Reptiles.Species;
using AnimalManagement.Controller;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-11
 * Description: Templates for fields to the change view.
 */

namespace AnimalManagement.Manager.View
{
    internal class FieldTemplates
    {

        /// <summary>
        /// Template for mammal data speciefies fields and values
        /// </summary>
        /// <param name="animal"> Mammal data </param>
        /// <returns> List of fields </returns>
        public List<Fields> changeMammal(MammalData animal)
        {
            return new List<Fields>
            {
                new Fields
                {
                    Label = "Number of teeth",
                    type = Field.Text, bindName = "teeth",
                    Value = animal.nrOfTeeth
                },
                new Fields
                {
                    Label = "Fangs",
                    type = Field.RadioButton,
                    Value = animal.fangs,
                    bindName = "fangs",
                    Options = new[] { "Yes", "No" }
                },
                new Fields
                {
                    Label = "Color",
                    type = Field.Text,
                    bindName = "color",
                    Value = animal.color
                }
            };
        }

        /// <summary>
        /// Template for Reptile data speciefies fields and values
        /// </summary>
        /// <param name="animal"> Reptile data </param>
        /// <returns> List of fields </returns>
        public List<Fields> changeReptile(ReptileData animal)
        {
            var habitatIndex = animal.habitat switch
            {
                "Unknown Habitat" => 0,
                "Desert" => 1,
                "Forest" => 2,
                "Swamp" => 3,
                "Grassland" => 4,
                _ => 0
            };

            return new List<Fields>
            {
                new Fields
                {
                    Label = "Body Length",
                    type = Field.Text, bindName = "bodyLength",
                    Value = animal.bodyLength
                },
                new Fields
                {
                    Label = "Tail",
                    type = Field.RadioButton,
                    Value = animal.tail,
                    bindName = "tail",
                    Options = new[] { "Yes", "No" }
                },
                new Fields
                {
                    Label = "Habitat",
                    type = Field.Slider,
                    bindName = "habitat",
                    Min = 0,
                    Max = 4,
                    Step = 1,
                    Options = new[] { "Unknown Habitat", "Desert", "Forest", "Swamp", "Grassland" },
                    Value = habitatIndex
                }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in cat view </returns>
        public List<Fields> changeCat(Cat animal)
        {
            return new List<Fields>
            {
                new Fields { Label = "Breed:", type = Field.Text, bindName = "breed", Value = animal.breed},
                new Fields 
                { Label = "Living Area:",
                  type = Field.RadioButton,
                  Options = new[] { "Indoor", "Outdoor", "Mixed" },
                  bindName = "livingType",
                  Value = animal.livingType
                }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in dog view </returns>
        public List<Fields> changeDog(Dog animal)
        {
            var index = animal.chipped switch
            {
                "No" => 0,
                "Yes" => 1,
                _ => 0
            };
            return new List<Fields>
            {
                new Fields { Label = "Breed:", type = Field.Text, bindName = "breed", Value = animal.breed},
                new Fields { Label = "Chipped:",
                    type = Field.Slider,
                    Min = 0,
                    Max = 1,
                    Step = 1,
                    bindName = "chipped",
                    Options = new[] { "No", "Yes" },
                    Value = index
                },
                new Fields { Label = "Ears",
                    type = Field.RadioButton,
                    Options = new[] { "Standing", "Hanging" },
                    bindName = "earType",
                    Value = animal.ears
                }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in cow view </returns>
        public List<Fields> changeCow(Cow animal)
        {
            return new List<Fields>
            {
                new Fields { Label = "Tagged:",
                    type = Field.RadioButton,
                    Options = new[] { "Yes", "No" },
                    bindName = "tagged",
                    Value = animal.tagged
                },
                new Fields { Label = "Tag Number:",
                    type = Field.Number,
                    bindName = "tagNumber",
                    Value = animal.tagNumber },
                new Fields { Label = "Milk Content in Liters:",
                    type = Field.Slider,
                    Min = 0,
                    Max = 20,
                    Step = 1,
                    bindName = "milkContent",
                    Value = animal.milkContent
                }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in lizard view </returns>
        public List<Fields> changeLizard(Lizard animal)
        {
            return new List<Fields>
            {
                new Fields { Label = "Venomous:",
                    type = Field.Dropdown,
                    Options = new[] { "Yes", "No", "Occasionally" },
                    bindName = "venom",
                    Value = animal.venomous
                }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in snake view </returns>
        public List<Fields> changeSnake(Snake animal)
        {
            return new List<Fields>
            {
                new Fields { Label = "Venomous:",
                    type = Field.Dropdown,
                    Options = new[] { "Yes", "No", "Occasionally" },
                    bindName = "venom",
                    Value = animal.venom
                },
                new Fields { Label = "Pattern:", 
                    type = Field.Text, bindName = "pattern",
                    Value = animal.pattern }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in turtle view </returns>
        public List<Fields> changeTurtle(Turtle animal)
        {
            return new List<Fields>
            {
                new Fields { Label = "Shell Width in cm:", 
                    type = Field.Number, bindName = "shellWidth", 
                    Value = animal.shellWidth },
                new Fields { Label = "Shell Hardness index:", 
                    type = Field.Number, bindName = "shellHardness", 
                    Value = animal.shellHardness }
            };
        }
    }
}
