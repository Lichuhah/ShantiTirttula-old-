﻿using Newtonsoft.Json;
using Shanti.Dispatcher.Models.Mc;
using System.Text;

namespace Shanti.Dispatcher.Models.Hash
{
    public class Session
    {
        public McData Mc { get; set; }
        public string Token { get; set; }
        public List<List<McSensorData>> SensorsData { get; set;}
        public List<DispatcherTrigger> Triggers { get; set; }
        public List<McCommand> Commands { get; set; }
        public DateTime LastSendTime { get; set; }
        public DateTime CreateTime { get; set; }
        public Session()
        {
            SensorsData = new List<List<McSensorData>>();
            Triggers = new List<DispatcherTrigger>();
            Commands = new List<McCommand>();
        }
        public void AddSensordData(List<McSensorData> data)
        {
            this.SensorsData.Add(data);
            if(DateTime.UtcNow - LastSendTime > TimeSpan.FromSeconds(60))
            {
                SendSensorData();
            }
        }

        public void SendSensorData()
        {
            List<int> ids = SensorsData.First().Select(x => x.SensorId).Distinct().ToList();
            List<McSensorData> data = new List<McSensorData>();

            foreach (int id in ids)
            {
                float value = 0;
                foreach (List<McSensorData> package in SensorsData)
                {
                    value += package.Where(x => x.SensorId == id).Average(x => x.Value);
                }
                data.Add(new McSensorData { SensorId = id, Value = value/SensorsData.Count });
            }
            SendAverageDataToServer(data);
        }

        private bool SendAverageDataToServer(List<McSensorData> data)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://shantitest.somee.com/SensorData/send");
            request.Headers.Add("Authorization", "Bearer " + this.Token);
            request.Content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            this.LastSendTime = DateTime.UtcNow;
            this.SensorsData.Clear();
            try
            {
                HttpResponseMessage response = client.Send(request);
                string answer = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(answer);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
