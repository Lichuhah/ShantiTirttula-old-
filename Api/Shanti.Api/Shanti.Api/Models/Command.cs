namespace Shanti.Api.Models
{
    public class Command
    {
        public int ControllerId { get; set; }
        public int DeviceId { get; set; }
        public float Value { get; set; }
    }
}
