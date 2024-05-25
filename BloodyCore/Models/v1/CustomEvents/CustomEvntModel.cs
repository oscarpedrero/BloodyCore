namespace Bloody.Core.Models.v1.CustomEvents
{
    public class CustomEvntModel
    {
        public string From { get; internal set; }
        public string To { get; set; }
        public string Type { get; set; }
        public string Data { get; set; }
    }
}
