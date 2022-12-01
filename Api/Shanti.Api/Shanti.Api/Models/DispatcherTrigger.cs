namespace Shanti.Api.Models
{
    public class DispatcherTrigger
    {
        public string Serial { get; set; }
        public float TriggerValue { get; set; }
        public int DeviceId { get; set; }
        public int SensorId { get; set; }
        public int Pin { get; set; }
        public bool IsPwm { get; set; }
        public float CommandValue { get; set; }
    }
}
