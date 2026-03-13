using System.ComponentModel;
using System.Runtime.CompilerServices;

/*
 * Author: Christoffer Wiik
 * Date: 2026-02-10
 * Description: Represents a dynamic field used in the reptile views.
 */

namespace AnimalManagement.Animals.Common
{
    public class Fields : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a propertys values changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="name">The name of the property that changed.</param>
        void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        /// <summary>
        /// Displays a label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Type of field.
        /// </summary>
        public Field type { get; set; }

        /// <summary>
        /// To bind a name to a field.
        /// </summary>
        public string bindName { get; set; }

        public object _value;

        /// <summary>
        /// Current value of field.
        /// </summary>
        public object Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(displayValue));
            }
        }

        /// <summary>
        /// List of selectable options for dropdown components.
        /// </summary>
        public IEnumerable<string> Options { get; set; }

        /// <summary>
        /// Minimum numeric value.
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Maximam numeric value.
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Size of steps for numeric fields.
        /// </summary>
        public double Step { get; set; }

        /// <summary>
        /// Displays the values from the options list
        /// in the dropdown component.
        /// </summary>
        public string displayValue
        {
            get
            {
                if (Options == null)
                {
                    return Value?.ToString();
                }

                int index = Convert.ToInt32(Value);
                return Options.ElementAtOrDefault(index);
            }
        }
    }
}
