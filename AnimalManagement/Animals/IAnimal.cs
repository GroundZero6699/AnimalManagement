using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-02-13
 * Description: Interface representing an animal with identification, descriptive, and classification properties.
 */

namespace AnimalManagement.Animals
{
    /// <summary>
    /// Represents an animal with identification, descriptive, and classification properties.
    /// </summary>
    public interface IAnimal
    {
        int id { get; }
        string name { get; }
        int age { get; }
        double weight { get; }
        Genders gender { get; }
        BitmapImage image { get; }
        string imagePath { get; }
        Types type { get; }
        string species { get; }
        void loadImage();
    }
}
