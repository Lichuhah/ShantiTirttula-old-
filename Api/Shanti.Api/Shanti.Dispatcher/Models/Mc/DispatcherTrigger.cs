namespace Shanti.Dispatcher.Models.Mc
{
    public class DispatcherTrigger
    {
        public int ControllerId { get; set; }
        public float TriggerValue { get; set; }
        public int DeviceId { get; set; }
        public int Pin { get; set; }
        public bool IsPwm { get; set; }
        public float CommandValue { get; set; }
    }
}
