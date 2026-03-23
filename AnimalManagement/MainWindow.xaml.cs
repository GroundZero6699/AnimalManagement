using AnimalManagement.Animals;
using AnimalManagement.Animals.Mammals;
using AnimalManagement.Animals.Reptiles;
using AnimalManagement.Animals.Reptiles.View;
using AnimalManagement.Controller;
using AnimalManagement.Manager;
using AnimalManagement.Manager.View;
using Microsoft.Win32;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-02-04
 * Description: Handles main windows creation and actions
 */

namespace AnimalManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string getName() => nameBox.Text;
        public string getAge() => ageBox.Text;
        public string getWeight() => weightBox.Text;
        public Genders getGender() => (Genders)genderBox.SelectedItem;
        public Types getAnimalTypes() => (Types)typeChooser.SelectedItem;
        public Enum getSubTypes() => (Enum)animalListBox.SelectedItem;
        public AnimalData generalData { get; set; }
        private IListManager manager { get; set; }
        public OnChange change { get; set; } = new OnChange();
        private string selectedImagePath;
        private BitmapImage img;
        private GridViewColumnHeader headerClicked = null;
        private ListSortDirection lastDirection = ListSortDirection.Ascending;

        /// <summary>
        /// Contructor initializes the main view and fills text areas with enum values
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            setGenders();
            setTypes();
            manager = new ListManager();
            DataContext = this;
            change.PropertyChanged += Change_PropertyChanged;
        }

        /// <summary>
        /// Shows a about view for the application.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for menu action </param>
        private void showHelp(object sender, EventArgs e)
        {
            MessageBox.Show("Animal Manager\nVersion 1.0\nCreator Christoffer",
                            "About",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        /// <summary>
        /// Closes application
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for menu action </param>
        private void terminate(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Selects correct animal type view based on type and species
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button action </param>
        /// <exception cref="NotImplementedException">
        /// Thrown if selected animal type has no associated view.
        /// </exception>
        private void createNewAnimal(object sender, RoutedEventArgs e)
        {
            var types = typeChooser.SelectedItem;
            if (types == null)
            {
                MessageBox.Show("Error occured no animal type choosen");
                return;
            }
            var type = (Types)types;
            generalData.type = type;

            var selectedSpecies = animalListBox.SelectedItem;
            if (selectedSpecies == null)
            {
                MessageBox.Show("Error occured choose a species");
                return;
            }
            Window view = type switch
            {
                Types.Mammal => new MammalView((MammalType)animalListBox.SelectedItem, generalData),
                Types.Reptile => new ReptileView((ReptileType)animalListBox.SelectedItem, generalData),
                _ => throw new NotImplementedException()
            };

            if (view.ShowDialog() == true)
            {
                IAnimal animal = type switch
                {
                    Types.Mammal => ((MammalView)view).animal,
                    Types.Reptile => ((ReptileView)view).animal
                };
                showCreatedAnimal(animal);
                manager.addAnimal(animal);
                updateList();
                clearText();
            }
        }

        /// <summary>
        /// Shows the newly created animal in text area.
        /// </summary>
        /// <param name="animal"> An animal object </param>
        private void showCreatedAnimal(IAnimal animal)
        {
            animalInfo.Text = animal.ToString();
        }

        /// <summary>
        /// Creates a data object of a animals general information before showing animal types.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button action </param>
        private void showAnimalClick(object sender, RoutedEventArgs e)
        {
            if (validate())
            {
                generalData = new AnimalData
                {
                    name = nameBox.Text,
                    age = int.Parse(ageBox.Text),
                    weight = double.Parse(weightBox.Text),
                    gender = (Genders)genderBox.SelectedItem,
                    image = currentImage(),
                    imagePath = selectedImagePath
                };
                animalPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Fill in General Data first!");
            }
        }

        /// <summary>
        /// Changes the species in the text area depending on type.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button action </param>
        private void animalChange(object sender, SelectionChangedEventArgs e)
        {
            if (sender == typeChooser)
            {
                if (typeChooser.SelectedItem is Types types)
                {
                    setSubTypes(types);
                }
            }
            else if (sender == animalListBox)
            {
                if (animalListBox.SelectedItem is Enum species)
                {
                    switch (species)
                    {
                        case MammalType:
                            typeChooser.SelectedItem = Types.Mammal;
                            break;
                        case ReptileType:
                            typeChooser.SelectedItem = Types.Reptile;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Resets comboboxes and textfields to default/null
        /// </summary>
        private void clearText()
        {
            nameBox.Clear();
            ageBox.Clear();
            weightBox.Clear();
            genderBox.SelectedIndex = -1;
            imageBox.Source = null;
            animalPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Fills the dropdown component with enum values representing genders.
        /// </summary>
        private void setGenders()
        {
            genderBox.ItemsSource = Enum.GetValues(typeof(Genders));
        }

        /// <summary>
        /// Fills the text area with enum values representing animal types.
        /// </summary>
        private void setTypes()
        {
            typeChooser.ItemsSource = Enum.GetValues(typeof(Types));
        }

        /// <summary>
        /// Fills teaxt area with species related to the animal type
        /// </summary>
        /// <param name="types"> An animal type as enum </param>
        private void setSubTypes(Types types)
        {
            switch (types)
            {
                case Types.Mammal:
                    animalListBox.ItemsSource = Enum.GetValues(typeof(MammalType));
                    break;
                case Types.Reptile:
                    animalListBox.ItemsSource = Enum.GetValues(typeof(ReptileType));
                    break;
            }
        }

        /// <summary>
        /// Validation method to check user input.
        /// </summary>
        /// <returns> Boolean value false if error occurs else true </returns>
        private bool validate()
        {
            if (string.IsNullOrWhiteSpace(getName()))
            {
                MessageBox.Show("Name cannot be empty");
                return false;
            }

            if (!int.TryParse(getAge(), out int age))
            {
                MessageBox.Show("Age must be a number");
                return false;
            }

            if (!double.TryParse(getWeight(), out double weight))
            {
                MessageBox.Show("Weight must be a number");
                return false;
            }

            if (genderBox.SelectedItem == null)
            {
                MessageBox.Show("Select a gender");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Opens up a file chooser to be able to load an image.
        /// Filter by file extensions.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button action </param>
        private void loadImage(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                Title = "Select animal image",
                Filter = "Image Files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg"
            };

            if (fileDialog.ShowDialog() == true)
            {
                selectedImagePath = fileDialog.FileName;
                img = new BitmapImage(new Uri(selectedImagePath, UriKind.Absolute));

                if (generalData != null)
                {
                    generalData.image = img;
                    generalData.imagePath = selectedImagePath;
                }
                imageBox.Source = img;
            }
        }

        /// <summary>
        /// Holds the current animal image
        /// </summary>
        /// <returns> An animal image as Bitmap </returns>
        private BitmapImage currentImage()
        {
            return imageBox.Source as BitmapImage;
        }

        /// <summary>
        /// Triggers all species to show in text area if checked.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for checkbox </param>
        private void showAllAnimals(object sender, RoutedEventArgs e)
        {
            animalListBox.ItemsSource = Enum.GetValues(typeof(MammalType)).Cast<Enum>()
                .Concat(Enum.GetValues(typeof(ReptileType)).Cast<Enum>()).ToList();

            if (animalListBox.SelectedItem is Enum types)
            {
                switch (types)
                {
                    case MammalType:
                        typeChooser.SelectedItem = Types.Mammal;
                        break;
                    case ReptileType:
                        typeChooser.SelectedItem = Types.Reptile;
                        break;
                }
            }
        }

        /// <summary>
        /// Triggers hidding species not related to the animal type. 
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for checkbox </param>
        private void hideAnimals(object sender, RoutedEventArgs e)
        {
            if (typeChooser.SelectedItem is Types type)
            {
                setSubTypes(type);
            }
        }

        /// <summary>
        /// Deletes the selected animal and updates the list view.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button click </param>
        private void deleteAnimal(object sender, RoutedEventArgs e)
        {
            if (listAnimals.SelectedItem is Animal selectedAnimal)
            {
                if (manager.removeAnimal(selectedAnimal))
                {
                    updateList();
                    animalInfo.Text = "";
                    infoBox.Text = "";
                    animalImage.Source = null;
                }
                else
                {
                    MessageBox.Show("Error occured");
                }
            }
            else
            {
                MessageBox.Show("No animal selected");
            }
            updateList();
        }

        /// <summary>
        /// Fills in textfields with choosen animals data to be changeable.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data for button click </param>
        private void changeAnimal(object sender, RoutedEventArgs e)
        {
            if (listAnimals.SelectedItem is Animal selected)
            {
                var change = new ChangeView(selected.type, selected.species, selected);
                if (change.ShowDialog() == true)
                {
                    var updated = change.updatedAnimal;

                    selected.update(updated);
                    fillFields(selected);
                }
            }
            else
            {
                MessageBox.Show("Select animal to change");
            }
        }

        /// <summary>
        /// Updates the listview of registerd animals.
        /// </summary>
        private void updateList()
        {
            listAnimals.ItemsSource = manager.getAnimals;
        }

        /// <summary>
        /// Fills textfields and textareas with selected animals data and image.
        /// </summary>
        /// <param name="selected"> Animal object </param>
        private void fillFields(Animal selected)
        {
            if (selected == null)
            {
                clearText();
                return;
            }

            infoBox.Text = selected.toInfoString();
            animalInfo.Text = selected.ToString();
            animalImage.Source = selected.image;
        }

        /// <summary>
        /// Changes the information in textareas to selected animal object
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data </param>
        private void Change_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "selected")
            {
                fillFields(change.selected);
            }
        }

        /// <summary>
        /// Opens a file dialog to select and load an XML, JSON, or text file, then updates the list display.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data </param>
        private void open(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Choose file",
                Filter = "All Files (*.xml;*.json;*.txt)|*.xml;*.json;*.txt|" +
                         "XML Files (*.xml)|*.xml|" +
                         "JSON Files (*.json)|*.json|" +
                         "Text Files (*.txt)|*.txt"
            };
            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                manager.loadFromFile(filePath);
                updateList();
            }
        }

        /// <summary>
        /// Opens a save file dialog allowing the user to select a file path and saves data to the chosen file.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data </param>
        private void saveAs(object sender, RoutedEventArgs e)
        {
            var dialog = new SaveFileDialog
            {
                Title = "Save file",
                Filter = "All Files (*.xml;*.json;*.txt)|*.xml;*.json;*.txt|" +
                         "XML Files (*.xml)|*.xml|" +
                         "JSON Files (*.json)|*.json|" +
                         "Text Files (*.txt)|*.txt"
            };
            if (dialog.ShowDialog() == true)
            {
                var filePath = dialog.FileName;
                
                manager.saveToFile(filePath);
            }
        }

        /// <summary>
        /// Checks if there is a current file path and saves to it
        /// otherwise opens a save file dialog to select a file path and saves data to the chosen file.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data </param>
        private void save(object sender, RoutedEventArgs e)
        {
            if (manager.hasCurrentFilePath)
            {
                manager.saveToFile(manager.currentFilePath);
                return;
            }

            var dialog = new SaveFileDialog
            {
                Title = "Save file",
                Filter = "All Files (*.xml;*.json;*.txt)|*.xml;*.json;*.txt|" +
                         "XML Files (*.xml)|*.xml|" +
                         "JSON Files (*.json)|*.json|" +
                         "Text Files (*.txt)|*.txt"
            };

            if(dialog.ShowDialog() == true)
            {
                manager.currentFilePath = dialog.FileName;
                manager.saveToFile(manager.currentFilePath);
            }
        }

        /// <summary>
        /// Clears all registered animals after confirming the action with the user
        /// resulting in an empty list and reset state.
        /// </summary>
        /// <param name="sender"> Event trigger </param>
        /// <param name="e"> Event data </param>
        private void freshStart(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show(
                "Are you sure? All unsaved progress will be lost forevah (ಥ_ಥ)",
                "Clean slate?",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);

            if(result == MessageBoxResult.OK)
            {
                manager.cleanSlate();
                animalInfo.Text = "";
                infoBox.Text = "";
                animalImage.Source = null;
            }
        }

        private void sort(object sender, RoutedEventArgs e)
        {
            var header = sender as GridViewColumnHeader;
            if(header == null)
            {
                return;
            }

            string sortedBy = header.Tag.ToString();
            ListSortDirection direction;

            if(header == headerClicked)
            {
                direction = lastDirection == ListSortDirection.Ascending
                    ? ListSortDirection.Descending : ListSortDirection.Ascending;
            }
            else
            {
                direction = ListSortDirection.Ascending;
            }

            var view = CollectionViewSource.GetDefaultView(listAnimals.ItemsSource);
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(sortedBy, direction));
            view.Refresh();

            headerClicked = header;
            lastDirection = direction;
        }
    }
}