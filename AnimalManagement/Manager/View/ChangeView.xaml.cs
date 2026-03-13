using AnimalManagement.Animals;
using AnimalManagement.Animals.Common;
using AnimalManagement.Animals.Mammals;
using AnimalManagement.Animals.Reptiles;
using AnimalManagement.Controller;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-04
 * Description: the logic side of the change view.
 */

namespace AnimalManagement.Manager.View
{
    /// <summary>
    /// Interaction logic for ChangeView
    /// </summary>
    public partial class ChangeView : Window
    {
        private Animal animal;
        private AnimalData animalData;
        private MammalData mammalData;
        private ReptileData reptileData;
        private List<Fields> fields;

        /// <summary>
        /// Gets the list of updated animal fields.
        /// </summary>
        public List<Fields> updatedAnimal {  get; private set; }

        /// <summary>
        /// Initializes a new instance of the ChangeView class, setting up animal data and view model based on the
        /// provided animal type and species.
        /// </summary>
        /// <param name="type">The type of animal view to display.</param>
        /// <param name="species">The species of the animal.</param>
        /// <param name="_animal">The animal instance to be displayed and edited.</param>
        public ChangeView(Types type, string species, Animal _animal)
        {
            InitializeComponent();

            animal = _animal;
            animalData = setAnimalData(animal);

            object typeData = null;

            if (animal is Mammal mammal)
            {
                mammalData = new MammalData
                {
                    nrOfTeeth = mammal.nrOfTeeth,
                    fangs = mammal.fangs,
                    color = mammal.color
                };
                typeData = mammalData;
            }
            else if(animal is Reptile reptile)
            {
                reptileData = new ReptileData
                {
                    bodyLength = reptile.bodyLength,
                    habitat = reptile.habitat,
                    tail = reptile.tail
                };
                typeData = reptileData;
            }

            DataContext = new ViewModel(animalData, species, type, animal, typeData);
            var model = (ViewModel)DataContext;
            fields = model.generalFields.Concat(model.SpeciesFields).ToList();
        }

        /// <summary>
        /// Creates a animalData object
        /// </summary>
        /// <param name="animal"> The current Animal object </param>
        /// <returns> An AnimalData object </returns>
        private AnimalData setAnimalData(Animal animal)
        {
            animalData = new AnimalData
            {
                name = animal.name,
                age = animal.age,
                weight = animal.weight,
                image = animal.image,
            };

            return animalData;
        }

        /// <summary>
        /// Loads a image as BitmapImage
        /// </summary>
        /// <param name="sender"> Event Trigger </param>
        /// <param name="e"> Event Data </param>
        public void loadNewImage(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "Select animal image",
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            bool? result = fileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = fileDialog.FileName;
                var img = new BitmapImage(new Uri(filePath));

                imageField.Source = img;
            }
        }


        /// <summary>
        /// Cancel the update and closes view
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data </param>
        private void cancelUpdate(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        /// <summary>
        /// Updates the objects data with the new values from the fields list and closes the view
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data </param>
        private void updateAnimal(object sender, RoutedEventArgs e)
        {
            foreach(var f in fields)
            {
                Debug.WriteLine($"{f.bindName} = {f.Value}");
            }
            if (validate())
            {
                var nameEntry = fields.FirstOrDefault(f => string.Equals(f.bindName, "name", StringComparison.OrdinalIgnoreCase));
                if(nameEntry != null) 
                {
                    nameEntry.Value = string.IsNullOrWhiteSpace(nameField.Text)
                        ? animal.name : nameField.Text;
                }
                var ageEntry = fields.FirstOrDefault(f => string.Equals(f.bindName, "age", StringComparison.OrdinalIgnoreCase));
                if (ageEntry != null)
                {
                    ageEntry.Value = int.TryParse(ageField.Text, out var age) ? age : animal.age;
                }

                var weightEntry = fields.FirstOrDefault(f => string.Equals(f.bindName, "weight", StringComparison.OrdinalIgnoreCase));
                if (weightEntry != null)
                {
                    weightEntry.Value = double.TryParse(weightField.Text, out var weight) ? weight : animal.weight;
                }
                var imageEntry = fields.FirstOrDefault(f => string.Equals(f.bindName, "image", StringComparison.OrdinalIgnoreCase));
                if (imageEntry != null)
                {
                    imageEntry.Value = imageField?.Source as BitmapImage;
                }
                updatedAnimal = fields;
                DialogResult = true;
                Close();
            }
        }

        /// <summary>
        /// Handles the Loaded event for a RadioButton, setting its checked state based on the associated field and its
        /// alternation index.
        /// </summary>
        /// <param name="sender"> The RadioButton that triggered the event </param>
        /// <param name="e"> Event data </param>
        private void loaded(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton r)
            {
                var field = r.Tag as Fields;
                if (field == null)
                {
                    return;
                }

                DependencyObject parent = VisualTreeHelper.GetParent(r);
                ContentPresenter contentPresenter = null;
                while (parent != null)
                {
                    contentPresenter = parent as ContentPresenter;
                    if (contentPresenter != null)
                    {
                        break;
                    }
                    parent = VisualTreeHelper.GetParent(parent);
                }

                int option = -1;
                if (contentPresenter != null)
                {
                    var value = contentPresenter.GetValue(ItemsControl.AlternationIndexProperty);
                    if (value is int intValue)
                    {
                        option = intValue;
                    }
                }

                int fieldIndex = GetSelectedIndexFromField(field);
                r.IsChecked = option == fieldIndex;
            }
        }

        /// <summary>
        /// Resolve the intended selected index for a Fields object.
        /// Accepts numeric index types and string values that match Options.
        /// </summary>
        private int GetSelectedIndexFromField(Fields field)
        {
            if (field == null || field.Value == null) return -1;

            if (field.Value is int i) return i;
            if (field.Value is double d) return (int)d;

            if (field.Value is string s)
            {
                if (field.Options != null)
                {
                    var opts = field.Options.ToArray();
                    for (int idx = 0; idx < opts.Length; idx++)
                    {
                        if (string.Equals(opts[idx], s, StringComparison.OrdinalIgnoreCase))
                            return idx;
                    }
                }

                if (int.TryParse(s, out var parsed)) return parsed;
            }

            return -1;
        }

        /// <summary>
        /// Validates input fields for general animal data
        /// </summary>
        /// <returns> Boolean false if null true otherwise </returns>
        private bool validate()
        {
            if(nameField == null)
            {
                return false;
            }
            if(ageField == null) 
            { 
                return false; 
            }
            if(weightField == null)
            {
                return false;
            }
            if(imageField == null)
            {
                return false;
            }
            return true;
        }
    }
}
