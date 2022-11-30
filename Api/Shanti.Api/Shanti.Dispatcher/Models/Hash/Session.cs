using Shanti.Dispatcher.Models.Mc;

namespace Shanti.Dispatcher.Models.Hash
{
    public class Session
    {
        public McData Mc { get; set; }
        public string Token { get; set; }
        public List<McSensorData> SensorsData { get; set;}
        public DateTime LastSendTime { get; set; }
        public DateTime CreateTime { get; set; }
        public Session()
        {
            SensorsData = new List<McSensorData>();
        }
        public void AddSensordData(McSensorData data)
        {
            this.SensorsData.Add(data);
            if(DateTime.UtcNow - LastSendTime > TimeSpan.FromSeconds(60))
            {
                SendSensorData();
            }
        }

        public void SendSensorData()
        {
            IEnumerable<int> ids = SensorsData.Select(x => x.SensorId).Distinct();
            foreach(var id in ids)
            {
                double value = SensorsData.Where(x => x.SensorId == id).Average(x => x.Value);
            }
            LastSendTime = DateTime.UtcNow;
            SensorsData.Clear();
        }
    }
}
