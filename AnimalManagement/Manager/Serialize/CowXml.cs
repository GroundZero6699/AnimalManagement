/*
 * Author: Christoffer Wiik
 * Date: 2026-03-13
 * Description: Represents the XML structure for a cow object.
 */

namespace AnimalManagement.Manager.Serialize
{
    public class CowXml : MammalXml
    {
        public string tagged { get; set; }
        public int tagNumber { get; set; }
        public double milkContent { get; set; }
    }
}
