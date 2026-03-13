using AnimalManagement.Animals;
using AnimalManagement.Animals.Common;
using AnimalManagement.Animals.Mammals.Species;
using AnimalManagement.Animals.Reptiles.Species;
using AnimalManagement.Controller;
using System.Collections.ObjectModel;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-11
 * Description: Represents a view model for change view.
 */

namespace AnimalManagement.Manager.View
{
    /// <summary>
    /// Creates collections of fields using templates to dynamicly create fields
    /// and insert existing values
    /// </summary>
    internal class ViewModel
    {
        public AnimalData data {  get; }
        public List<Fields> generalFields { get; }
        public ObservableCollection<Fields> SpeciesFields { get; }

        /// <summary>
        /// Creates a view model for each animal by using the type of animal
        /// </summary>
        /// <param name="_data"> Data object of general animaldata </param>
        /// <param name="species"> String value representing the animal species </param>
        /// <param name="type"> A type object mammal or reptile </param>
        /// <param name="animal"> The animal object </param>
        /// <param name="typeData"> The data object of a animal type </param>
        public ViewModel(AnimalData _data, string species, Types type, Animal animal, object typeData)
        {
            data = _data;
            FieldTemplates template = new FieldTemplates();
            
            generalFields = new List<Fields>
            {
                new Fields { bindName = "name", Label = "Name", Value = _data.name },
                new Fields { bindName = "age", Label = "Age", Value = _data.age },
                new Fields { bindName = "weight", Label = "Weight", Value = _data.weight },
                new Fields { bindName = "image", Label = "Image", Value = _data.image }
            };

            SpeciesFields = type switch
            {
                Types.Mammal => species switch
                {
                    "Cat" => new ObservableCollection<Fields>(
                        template.changeMammal((MammalData)typeData)
                        .Concat(template.changeCat((Cat)animal))),

                    "Dog" => new ObservableCollection<Fields>(
                        template.changeMammal((MammalData)typeData)
                        .Concat(template.changeDog((Dog)animal))),

                    "Cow" => new ObservableCollection<Fields>(
                        template.changeMammal((MammalData)typeData)
                        .Concat(template.changeCow((Cow)animal))),

                    _ => new ObservableCollection<Fields>()
                },

                Types.Reptile => species switch
                {
                    "Snake" => new ObservableCollection<Fields>(
                        template.changeReptile((ReptileData)typeData)
                        .Concat(template.changeSnake((Snake)animal))),

                    "Lizard" => new ObservableCollection<Fields>(
                        template.changeReptile((ReptileData)typeData)
                        .Concat(template.changeLizard((Lizard)animal))),

                    "Turtle" => new ObservableCollection<Fields>(
                        template.changeReptile((ReptileData)typeData)
                        .Concat(template.changeTurtle((Turtle)animal))),

                    _ => new ObservableCollection<Fields>()
                },

                _ => new ObservableCollection<Fields>()
            };
        }
    }
}
