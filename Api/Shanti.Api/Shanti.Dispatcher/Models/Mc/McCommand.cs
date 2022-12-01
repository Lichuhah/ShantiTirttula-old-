namespace Shanti.Dispatcher.Models.Mc
{
    public class McCommand
    {
        public string Serial { get; set; }
        public int Pin { get; set; }
        public bool IsPwm { get; set; }
        public float Value { get; set; }
    }
}
