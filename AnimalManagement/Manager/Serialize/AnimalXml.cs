using AnimalManagement.Animals;

/*
 * Author: Christoffer Wiik
 * Date: 2026-03-13
 * Description: Represents the XML structure for a animal.
 */

namespace AnimalManagement.Manager.Serialize
{
    public class AnimalXml
    {
        public string name { get; set; }
        public int age { get; set; }
        public double weight { get; set; }
        public Genders gender { get; set; }
        public string imagePath { get; set; }
        public Types type { get; set; }
        public string species { get; set; }
    }
}
