/*
 * Author: Christoffer Wiik
 * Date: 2026-03-13
 * Description: Represents the XML structure for a dog object.
 */

namespace AnimalManagement.Manager.Serialize
{
    public class DogXml : MammalXml
    {
        public string breed { get; set; }
        public string chipped { get; set; }
        public string ears { get; set; }
    }
}
