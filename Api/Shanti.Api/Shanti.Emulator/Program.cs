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
    ConsoleKeyInfo keyPress = Console.ReadKey(intercept: true);
    while (keyPress.Key != ConsoleKey.Enter)
    {
        Console.Write(keyPress.KeyChar.ToString().ToUpper());

        keyPress = Console.ReadKey(intercept: true);
    }
    SensorData sensor = new SensorData { SensorId = 2, Value = (float)rnd.NextDouble() };
    httpPost(esp1, new List<SensorData> { sensor });
    httpGet(esp1);
    //Thread.Sleep(5);
    //httpGet(esp1);
    //Thread.Sleep(10000);
}
//while (true)
//{
//    //sendSensorData();
//    for(int i=0; i<10; i++)
//    {
//        SensorData sensor = new SensorData { SensorId = 2, Value = i };
//        httpPost(esp1, new List<SensorData> { sensor });
//        Thread.Sleep(5);
//        httpGet(esp1);
//        Thread.Sleep(5000);
//    }
//    for (int i = 10; i > 0; i--)
//    {
//        SensorData sensor = new SensorData { SensorId = 2, Value = i };
//        httpPost(esp1, new List<SensorData> { sensor });
//        Thread.Sleep(5);
//        httpGet(esp1);
//        Thread.Sleep(1000);
//    }
//    Thread.Sleep(5000);
//}

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

async void httpGet(ESP esp)
{
    HttpClient client = new HttpClient();
    var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7160/Command/get");
    request.Headers.Add("Serial", esp.Key);
    request.Headers.Add("Mac", esp.MAC);
    try
    {
        var resp = await client.SendAsync(request);
        string res = await resp.Content.ReadAsStringAsync();
        Console.WriteLine(res);
    } catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}