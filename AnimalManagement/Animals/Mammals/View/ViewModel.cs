using AnimalManagement.Animals.Common;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Represents the model of a mammal view binds templates to species.
 */

namespace AnimalManagement.Animals.Mammals.View
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
        /// <param name="species"> Mammal species </param>
        public ViewModel(string species) 
        {
            var template = new Template();

            Fields = species switch
            {
                "Cat" => template.createCat(),
                "Dog" => template.createDog(),
                "Cow" => template.createCow(),
                _ => new List<Fields>()
            };
        }
    }
}
