using Newtonsoft.Json;
using Shanti.Emulator;
using System.Text;

Random rnd = new Random(DateTime.UtcNow.Millisecond);

ESP esp1 = new ESP { Key = "AAAAAAAAAAAAAAAA", MAC = "AA:AA:AA:AA:AA:AA" };
ESP esp2 = new ESP { Key = "BBBBBBBBBBBBBBBB", MAC = "BB:BB:BB:BB:BB:BB" };
ESP esp3 = new ESP { Key = "CCCCCCCCCCCCCCCC", MAC = "CC:CC:CC:CC:CC:CC" };
List<ESP> espList = new List<ESP>() { esp1, esp2, esp3 };

//while (true)
//{
//    sendSensorData();
//    Thread.Sleep(10000000);
//}

SensorData sensor = new SensorData { DeviceId = 1, Value = (float)rnd.NextDouble() };
httpPost(esp1, sensor);

void sendSensorData()
{
    foreach (ESP esp in espList)
    {
        SensorData sensor = new SensorData { DeviceId = 1, Value = (float)rnd.NextDouble() };
        httpPost(esp, sensor);
        sensor = new SensorData { DeviceId = 2, Value = (float)rnd.NextDouble() };
        httpPost(esp, sensor);
        sensor = new SensorData { DeviceId = 3, Value = (float)rnd.NextDouble() };
        httpPost(esp, sensor);
        sensor = new SensorData { DeviceId = 4, Value = (float)rnd.NextDouble() };
        httpPost(esp,sensor);
    }
}

void httpPost(ESP esp, SensorData data)
{
    HttpClient client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7160/Sensor/send");
    request.Headers.Add("Serial", esp.Key);
    request.Headers.Add("MAC", esp.MAC);
    request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
    try
    {
        HttpResponseMessage response = client.Send(request);
        string answer = response.Content.ReadAsStringAsync().Result;
        Console.WriteLine(answer);
    } catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}