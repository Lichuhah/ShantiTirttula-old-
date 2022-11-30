using Newtonsoft.Json;
using Shanti.Emulator;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

Random rnd = new Random(DateTime.UtcNow.Millisecond);

ESP esp1 = new ESP { Key = "AAAAAAAAAAAAAAAA", MAC = "AA:AA:AA:AA:AA:AA" };
ESP esp2 = new ESP { Key = "BBBBBBBBBBBBBBBB", MAC = "BB:BB:BB:BB:BB:BB" };
ESP esp3 = new ESP { Key = "CCCCCCCCCCCCCCCC", MAC = "CC:CC:CC:CC:CC:CC" };
List<ESP> espList = new List<ESP>() { esp1, esp2, esp3 };

while (true)
{
    sendSensorData();
    Thread.Sleep(500);
}

void sendSensorData()
{
    foreach (ESP esp in espList)
    {
        SensorData sensor = new SensorData { SensorId = 1, Value = (float)rnd.NextDouble() };
        httpPost(esp1, sensor);
        //sensor = new SensorData { SensorId = 2, Value = (float)rnd.NextDouble() };
        //httpPost(esp, sensor);
        //sensor = new SensorData { SensorId = 3, Value = (float)rnd.NextDouble() };
        //httpPost(esp, sensor);
        //sensor = new SensorData { SensorId = 4, Value = (float)rnd.NextDouble() };
        //httpPost(esp,sensor);
    }
}

void httpPost(ESP esp, SensorData data)
{
    HttpClient client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7160/Sensor/send");
    request.Content = new StringContent(new Newtonsoft.Json., Encoding.UTF8, "application/json");

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