/*
 * Author: Christoffer Wiik
 * Date: 2026-03-13
 * Description: Represents the XML structure for a mammal type.
 */

namespace AnimalManagement.Manager.Serialize
{
    public class MammalXml : AnimalXml
    {
        public int nrOfTeeth { get; set; }
        public string fangs { get; set; }
        public string color { get; set; }
    }
}
