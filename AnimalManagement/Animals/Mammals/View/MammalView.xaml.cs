using AnimalManagement.Animals.Common;
using AnimalManagement.Animals.Mammals.Species;
using AnimalManagement.Animals.Mammals.View;
using AnimalManagement.Controller;
using System.Windows;
using System.Windows.Controls;

/*
 * Author: Christoffer Wiik
 * Date: 2026-01-26
 * Description: Handles creation of mammals window and its interactions.
 */

namespace AnimalManagement.Animals.Mammals
{
    /// <summary>
    /// Interaction logic for MammalView.xaml
    /// </summary>
    public partial class MammalView : Window
    {
        public string species { get; }
        public Animal animal { get; private set; }
        private AnimalData generalData;

        /// <summary>
        /// Initializes the mammal view.
        /// </summary>
        /// <param name="_species"> Mammal species </param>
        /// <param name="_generalData"> Data object of general animal data </param>
        public MammalView(MammalType _species, AnimalData _generalData)
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
        /// Creates a data object of generaldata related to mammals.
        /// </summary>
        /// <returns> Data object of a mammal </returns>
        private MammalData createData()
        {
            MammalData animalData = new MammalData
            {
                nrOfTeeth = int.TryParse(teethBox.Text, out var teeth) ? teeth : 0,
                fangs = fangsYes.IsChecked == true ? "Yes" : "No",
                color = string.IsNullOrWhiteSpace(colorField.Text) ? "Undefined" : colorField.Text,
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

            MammalData animalData = createData();

            var dynamic = viewModel.Fields.ToDictionary(
                f => f.bindName,
                f => f.Value);

            Animal animal = species switch
            {
                "Cat" => new Cat(
                    generalData,
                    animalData,
                    validateString(dynamic, "breed"),
                    validateString(dynamic, "livingType")
                    ),

                "Dog" => new Dog(
                    generalData,
                    animalData,
                    validateString(dynamic, "breed"),
                    validateBool(dynamic, "chipped"),
                    validateString(dynamic, "earType")
                    ),

                "Cow" => new Cow(
                    generalData,
                    animalData,
                    validateString(dynamic, "tagged"),
                    validateInt(dynamic, "tagNumber"),
                    validateDouble(dynamic, "milkContent")
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

        private string validateBool(Dictionary<string, object> dynamic, string name)
        {
            string text = validateString(dynamic, name);
            
            if(text == "1")
            {
                return "Yes";
            }
            return "No";
        }

        /// <summary>
        /// Validates input fields and convert into int value.
        /// </summary>
        /// <param name="dynamic"> Dictionary of text fields and it corresponding value </param>
        /// <param name="name"> Name of binded text field</param>
        /// <returns> Converted int value or 0 if input is invalid </returns>
        private int validateInt(Dictionary<string, object> dynamic, string key)
        {
            string text = validateString(dynamic, key);
            return int.TryParse(text, out var result) ? result : 0;
        }

        /// <summary>
        /// Validates input fields and sets default value if null.
        /// </summary>
        /// <param name="dynamic"> Dictionary of text field and it corresponding value </param>
        /// <param name="name"> Name of binded text field </param>
        /// <returns> text fields value if valid else returns a default value </returns>
        private string validateString(Dictionary<string, object> dynamic, string key)
        {
            if (!dynamic.TryGetValue(key, out var value))
            {
                return "Undefined";
            }

            string text = value?.ToString();
            return string.IsNullOrWhiteSpace(text) ? "Undefined" : text;
        }

        /// <summary>
        /// Fetches the string value from the radio button.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data from radio button </param>
        private void checkedButton(object sender,  RoutedEventArgs e)
        {
            if(sender is RadioButton radio)
            {
                string value = radio.Content as string ?? radio.DataContext as string;

                var field = radio.Tag as Fields;
                
                if(field != null)
                {
                    field.Value = value;
                }
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
    }
}
