using System.Windows;
using System.Windows.Controls;

/*
 * Author: Christoffer Wiik
 * Date: 2026-02-10
 * Description: Binds templates with dynamic fields.
 */

namespace AnimalManagement.Animals.Common
{
    /// <summary>
    /// Binds dynamic fields to corresponding templates.
    /// </summary>
    public class FieldTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Template for text fields.
        /// </summary>
        public DataTemplate TextTemplate { get; set; }

        /// <summary>
        /// Template for combobox.
        /// </summary>
        public DataTemplate DropdownTemplate { get; set; }

        /// <summary>
        /// Template for radiobuttons.
        /// </summary>
        public DataTemplate RadioButtonTemplate { get; set; }

        /// <summary>
        /// Template for sliders.
        /// </summary>
        public DataTemplate SliderTemplate { get; set; }

        /// <summary>
        /// Template for numeric fields.
        /// </summary>
        public DataTemplate NumberTemplate { get; set; }

        /// <summary>
        /// Returns the correct template for the given field.
        /// </summary>
        /// <param name="item"> Field object to check </param>
        /// <param name="container"> Container to applicate template in </param>
        /// <returns> Datatemplate that matches fields type, default if not valid </returns>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is not Fields field)
            {
                return base.SelectTemplate(item, container);
            }

            return field.type switch
            {
                Field.Text => TextTemplate,
                Field.Dropdown => DropdownTemplate,
                Field.RadioButton => RadioButtonTemplate,
                Field.Slider => SliderTemplate,
                Field.Number => NumberTemplate,
                _ => TextTemplate
            };
        }
    }
}
