using AnimalManagement.Animals.Common;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Templates for each species for dynamical view.
 */

namespace AnimalManagement.Animals.Mammals.View
{
    public class Template
    {
        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in cat view </returns>
        public List<Fields> createCat()
        {
            return new List<Fields>
            {
                new Fields { Label = "Breed:", type = Field.Text, bindName = "breed" },
                new Fields { Label = "Living Area:",
                    type = Field.RadioButton,
                    Options = new[] { "Indoor", "Outdoor", "Mixed" },
                    bindName = "livingType" }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in dog view </returns>
        public List<Fields> createDog()
        {
            return new List<Fields>
            {
                new Fields { Label = "Breed:", type = Field.Text, bindName = "breed" },
                new Fields { Label = "Chipped:",
                    type = Field.Slider,
                    Min = 0,
                    Max = 1,
                    Step = 1,
                    bindName = "chipped",
                    Options = new[] { "No", "Yes" }},
                new Fields { Label = "Ears",
                    type = Field.RadioButton,
                    Options = new[] { "Standing", "Hanging" },
                    bindName = "earType"}
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in cow view </returns>
        public List<Fields> createCow()
        {
            return new List<Fields>
            {
                new Fields { Label = "Tagged:",
                    type = Field.RadioButton,
                    Options = new[] { "Yes", "No" },
                    bindName = "tagged" },
                new Fields { Label = "Tag Number:",
                    type = Field.Number,
                    bindName = "tagNumber" },
                new Fields { Label = "Milk Content in Liters:",
                    type = Field.Slider,
                    Min = 0,
                    Max = 20,
                    Step = 1,
                    bindName = "milkContent"}
            };
        }
    }
}
