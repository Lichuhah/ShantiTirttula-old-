using Shanti.Dispatcher.Models.Mc;

namespace Shanti.Dispatcher.Models.Hash
{
    public class Session
    {
        public McData Mc { get; set; }
        public string Token { get; set; }
        public List<McSensorData> SensorsData { get; set;}
        public DateTime LastSend { get; set; }
    }
}
