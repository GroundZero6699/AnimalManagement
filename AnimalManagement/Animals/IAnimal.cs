using System.Windows.Media.Imaging;

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
        Types type { get; }
        string species { get; }
    }
}
