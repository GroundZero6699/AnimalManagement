/*
 * Author: Christoffer Wiik
 * Date: 2026-03-13
 * Description: Represents the XML structure for a turtle object.
 */

namespace AnimalManagement.Manager.Serialize
{
    public class TurtleXml : ReptileXml
    {
        public double shellWidth { get; set; }
        public int shellHardness { get; set; }
    }
}
