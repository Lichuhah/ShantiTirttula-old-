using Newtonsoft.Json;
using Shanti.Emulator;
using System.Text;

HttpClient httpClient = new HttpClient();
Random rnd = new Random(DateTime.UtcNow.Millisecond);

ESP esp1 = new ESP { Key = "AAAAAAAAAAAAAAAA", MAC = "AA:AA:AA:AA:AA:AA" };
ESP esp2 = new ESP { Key = "BBBBBBBBBBBBBBBB", MAC = "BB:BB:BB:BB:BB:BB" };
ESP esp3 = new ESP { Key = "CCCCCCCCCCCCCCCC", MAC = "CC:CC:CC:CC:CC:CC" };
List<ESP> espList = new List<ESP>() { esp1, esp2, esp3 };

while (true)
{
    Thread.Sleep(1000);
    sendSensorData();
}

void sendSensorData()
{
    foreach (ESP esp in espList)
    {
        SensorData sensor = new SensorData { DeviceId = 1, Value = (float)rnd.NextDouble() };
        httpPost(sensor);
        sensor = new SensorData { DeviceId = 2, Value = (float)rnd.NextDouble() };
        httpPost(sensor);
        sensor = new SensorData { DeviceId = 3, Value = (float)rnd.NextDouble() };
        httpPost(sensor);
        sensor = new SensorData { DeviceId = 4, Value = (float)rnd.NextDouble() };
        httpPost(sensor);
    }
}

void httpPost(SensorData data)
{
    StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    send(content);
}

void send(StringContent cont)
{
    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7160/Test/sendSensor");
    request.Content = cont;
    var req = httpClient.SendAsync(request);
    var str = req.Result.Content.ReadAsStringAsync().Result;
    Console.WriteLine(str);
}