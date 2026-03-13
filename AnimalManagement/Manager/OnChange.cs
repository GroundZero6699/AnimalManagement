using AnimalManagement.Animals;
using System.ComponentModel;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-11
 * Description: Listener for the listview to update textareas with selected animal text.
 */

namespace AnimalManagement.Manager
{
    /// <summary>
    /// Notifies clients that a property value has changed, supporting data binding scenarios.
    /// </summary>
    public class OnChange : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Holds the currently selected animal.
        /// </summary>
        private Animal selectedAnimal;

        /// <summary>
        /// Gets or sets the currently selected Animal.
        /// </summary>
        public Animal selected
        {
            get => selectedAnimal;
            set
            {
                selectedAnimal = value;
                OnPropertyChanged(nameof(selected));
            }
        }
        
        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
