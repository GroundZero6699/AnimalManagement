/*
 * Author: Christoffer Wiik
 * Date: 2026-03-13
 * Description: Represents the XML structure for a snake object.
 */

namespace AnimalManagement.Manager.Serialize
{
    public class SnakeXml : ReptileXml
    {
        public string venomous { get; set; }
        public string pattern { get; set; }
    }
}
