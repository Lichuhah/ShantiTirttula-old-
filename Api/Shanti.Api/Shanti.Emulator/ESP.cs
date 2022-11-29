namespace Shanti.Emulator
{
    public class ESP
    {
        public string MAC { get; set; }
        public string Key { get; set; }

        public List<SensorData> SensorsData { get; set; }
        public ESP()
        {
            SensorsData = new List<SensorData>();
        }
    }
}
