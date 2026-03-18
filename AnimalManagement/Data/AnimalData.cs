using AnimalManagement.Animals;
using System.Windows.Media.Imaging;

/*
 * Author: Christoffer Wiik
 * Date: 2026-02-04
 * Description: General data for a animal.
 */

namespace AnimalManagement.Controller
{
    /// <summary>
    /// Represents General data for an animal.
    /// </summary>
    public class AnimalData
    {
        public string name { get; set; }
        public int id { get; }
        public int age { get; set; }
        public double weight { get; set; }
        public Genders gender { get; set; }
        public Types type { get; set; }
        public BitmapImage image { get; set; }
        public string imagePath { get; set; }
    }
}
