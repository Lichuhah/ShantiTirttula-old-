using Newtonsoft.Json;
using Shanti.Emulator;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

Random rnd = new Random(DateTime.UtcNow.Millisecond);
DateTime StartTime = DateTime.UtcNow;
ESP esp1 = new ESP { Key = "AAAAAAAAAAAAAAAA", MAC = "AA:AA:AA:AA:AA:AA" };
ESP esp2 = new ESP { Key = "BBBBBBBBBBBBBBBB", MAC = "BB:BB:BB:BB:BB:BB" };
ESP esp3 = new ESP { Key = "CCCCCCCCCCCCCCCC", MAC = "CC:CC:CC:CC:CC:CC" };
List<ESP> espList = new List<ESP>() { esp1, esp2, esp3 };

while (true)
{
    sendSensorData();
    Thread.Sleep(1000);
}

void sendSensorData()
{
    foreach (ESP esp in espList)
    {
        SensorData sensor1 = new SensorData { SensorId = 1, Value = (float)rnd.NextDouble() };
        SensorData sensor2 = new SensorData { SensorId = 2, Value = (float)rnd.NextDouble() };
        httpPost(esp, new List<SensorData> { sensor1, sensor2 });
        Thread.Sleep(5);
    }
}

void httpPost(ESP esp, List<SensorData> data)
{
    HttpClient client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7160/Sensor/send");
    request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    request.Headers.Add("Serial", esp.Key);
    request.Headers.Add("Mac", esp.MAC);
    try
    {
        client.SendAsync(request);
        Console.WriteLine((DateTime.UtcNow - StartTime).TotalSeconds);
    } catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}