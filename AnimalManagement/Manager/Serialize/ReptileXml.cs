/*
 * Author: Christoffer Wiik
 * Date: 2026-03-13
 * Description: Represents the XML structure for a Reptile type.
 */

namespace AnimalManagement.Manager.Serialize
{
    public class ReptileXml : AnimalXml
    {
        public double bodyLength { get; set; }
        public string habitat { get; set; }
        public string tail { get; set; }
    }
}
