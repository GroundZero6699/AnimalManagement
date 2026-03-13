/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Represents the model of a reptile view binds templates to species.
 */

using AnimalManagement.Animals.Common;

namespace AnimalManagement.Animals.Reptiles.View
{
    public class ViewModel
    {
        /// <summary>
        /// List of fields.
        /// </summary>
        public List<Fields> Fields {  get; set; }

        /// <summary>
        /// Binds templates to species.
        /// </summary>
        /// <param name="species"> Reptile species </param>
        public ViewModel(string species) 
        {
            var template = new ReptileTemplate();

            Fields = species switch
            {
                "Lizard" => template.createLizard(),
                "Turtle" => template.createTurtle(),
                "Snake" => template.createSnake(),
                _ => new List<Fields>()
            };
        }
    }
}
