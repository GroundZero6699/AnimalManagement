using AnimalManagement.Animals.Common;
using AnimalManagement.Animals.Reptiles.Species;
using AnimalManagement.Controller;
using System.Windows;
using System.Windows.Controls;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Handles creation of reptiles window and its interactions.
 */

namespace AnimalManagement.Animals.Reptiles.View
{
    /// <summary>
    /// Interaction logic for ReptileView.xaml
    /// </summary>
    public partial class ReptileView : Window
    {
        public string species { get; }
        public Animal animal { get; private set; }
        private AnimalData generalData;

        /// <summary>
        /// Initializes the reptile view.
        /// </summary>
        /// <param name="_species"> Reptile species </param>
        /// <param name="_generalData"> Data object of general animal data </param>
        public ReptileView(ReptileType _species, AnimalData _generalData)
        {
            InitializeComponent();
            species = _species.ToString();
            DataContext = new ViewModel(species.ToString());
            generalData = _generalData;
        }

        /// <summary>
        /// Handles the button action when selected a type and species.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button action </param>
        private void createButton(object sender, RoutedEventArgs e)
        {
            animal = createAnimal(sender, e);
            DialogResult = true;
            Close();
        }

        /// <summary>
        /// Creates a data object of generaldata related to reptiles.
        /// </summary>
        /// <returns> Data object of a reptile </returns>
        private ReptileData createData()
        {
            ReptileData animalData = new ReptileData
            {
                bodyLength = double.TryParse(lengthBox.Text, out var length) ? length : 0.0,
                habitat = habitatLabel.Content.ToString(),
                tail = tailYes.IsChecked == true ? "Yes" : "No",
                animalSpecies = species
            };
            return animalData;
        }

        /// <summary>
        /// Validates input before creating a new animal object.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button action </param>
        /// <returns> A newly created animnal object populated using data objects </returns>
        /// <exception cref="NotImplementedException">
        /// Throws if selected species have no creation logic.
        /// </exception>
        public Animal createAnimal(object sender, RoutedEventArgs e)
        {
            var viewModel = (ViewModel)DataContext;
            ReptileData animalData = createData();

            var dynamic = viewModel.Fields.ToDictionary(
                f => f.bindName,
                f => f.Value);

            Animal animal = species switch
            {
                "Lizard" => new Lizard(
                    generalData,
                    animalData,
                    validateString(dynamic, "venom")
                    ),

                "Snake" => new Snake(
                    generalData,
                    animalData,
                    validateString(dynamic, "venom"),
                    validateString(dynamic, "pattern")
                    ),

                "Turtle" => new Turtle(
                    generalData,
                    animalData,
                    validateDouble(dynamic, "shellWidth"),
                    validateInt(dynamic, "shellHardness")
                    ),

                _ => throw new NotImplementedException()
            };

            return animal;
        }

        /// <summary>
        /// Validates input fields and convert to double value.
        /// </summary>
        /// <param name="dynamic"> Dictionary with text fields and corresponding values </param>
        /// <param name="name"> Name of binded text field </param>
        /// <returns> Converted double value or 0.0 if input is invalid </returns>
        private double validateDouble(Dictionary<string, object> dynamic, string name)
        {
            string text = validateString(dynamic, name);
            return double.TryParse(text, out double value) ? value : 0.0;
        }

        /// <summary>
        /// Validates input fields and convert into int value.
        /// </summary>
        /// <param name="dynamic"> Dictionary of text fields and it corresponding value </param>
        /// <param name="name"> Name of binded text field</param>
        /// <returns> Converted int value or 0 if input is invalid </returns>
        private int validateInt(Dictionary<string, object> dynamic, string name)
        {
            string text = validateString(dynamic, name);
            return int.TryParse(text, out var result) ? result : 0;
        }

        /// <summary>
        /// Validates input fields and sets default value if null.
        /// </summary>
        /// <param name="dynamic"> Dictionary of text field and it corresponding value </param>
        /// <param name="name"> Name of binded text field </param>
        /// <returns> text fields value if valid else returns a default value </returns>
        private string validateString(Dictionary<string, object> dynamic, string name)
        {
            if (!dynamic.TryGetValue(name, out var value))
            {
                return "Undefined";
            }

            string text = value?.ToString();
            return string.IsNullOrWhiteSpace(text) ? "Undefined" : text;
        }

        /// <summary>
        /// Changes the text in the label for habitat slider.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for slider change </param>
        public void habitatSliderChanger(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            switch ((int)e.NewValue)
            {
                case 0: habitatLabel.Content = "Desert"; break;
                case 1: habitatLabel.Content = "Forest"; break;
                case 2: habitatLabel.Content = "Swamp"; break;
                case 3: habitatLabel.Content = "Grassland"; break;

            }
        }

        /// <summary>
        /// Closes the reptile window.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button action </param>
        private void cancelClick(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Fetches the string value from the radio button.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data from radio button </param>
        private void checkedButton(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radio)
            {
                string value = radio.Content as string ?? radio.DataContext as string;

                var field = radio.Tag as Fields;

                if (field != null)
                {
                    field.Value = value;
                }
            }
        }
    }
}
