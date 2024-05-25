using System.Collections.Generic;

namespace Bloody.Core.Models.v1.CustomEvents
{
    public class ModCustomEvent
    {
        public string Name { get; set; }
        public List<string> Types { get; set; }
        internal string RegisterBy;
    }
}
