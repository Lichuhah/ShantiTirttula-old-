namespace Shanti.Api.Models
{
    public class NewTrigger
    {
        public int ControllerId { get; set; }
        public int SensorId { get; set; }
        public int DeviceId { get; set; }
        public float TriggerValue { get; set; }
        public float CommandValue { get; set; }
    }
}
