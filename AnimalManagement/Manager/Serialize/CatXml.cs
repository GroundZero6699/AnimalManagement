/*
 * Author: Christoffer Wiik
 * Date: 2026-03-13
 * Description: Represents the XML structure for a cat object.
 */

namespace AnimalManagement.Manager.Serialize
{
    public class CatXml : MammalXml
    {
        public string breed { get; set; }
        public string livingType { get; set; }
    }
}
