/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Templates for each species for dynamical view.
 */

using AnimalManagement.Animals.Common;

namespace AnimalManagement.Animals.Reptiles.View
{
    public class ReptileTemplate
    {
        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in lizard view </returns>
        public List<Fields> createLizard()
        {
            return new List<Fields>
            {
                new Fields { Label = "Venomous:",
                    type = Field.Dropdown,
                    Options = new[] { "Yes", "No", "Occasionally" },
                    bindName = "venom" }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in snake view </returns>
        public List<Fields> createSnake()
        {
            return new List<Fields>
            {
                new Fields { Label = "Venomous:",
                    type = Field.Dropdown,
                    Options = new[] { "Yes", "No", "Occasionally" },
                    bindName = "venom" },
                new Fields { Label = "Pattern:", type = Field.Text, bindName = "pattern" }
            };
        }

        /// <summary>
        /// Creates a list of fields specifying fields to as a template.
        /// </summary>
        /// <returns> A template of fields to be set in turtle view </returns>
        public List<Fields> createTurtle()
        {
            return new List<Fields>
            {
                new Fields { Label = "Shell Width in cm:", type = Field.Number, bindName = "shellWidth" },
                new Fields { Label = "Shell Hardness index:", type = Field.Number, bindName = "shellHardness" }
            };
        }
    }
}
