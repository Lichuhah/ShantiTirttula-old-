WiFiClientSecure client;
int shantiPort = 443;
String shantiHost = "shantitest.somee.com";
bool apiIsConnected = false;


void APIinit(){
  int r = 0;
  client.setFingerprint("65 DD AD A7 A5 50 DB E8 38 9E 79 13 DC AF D3 60 BF BF 9F B9");
  client.setTimeout(5000);
    while((!client.connect(shantiHost, shantiPort)) && (r < 30)){
      delay(100);
      r++;
  }

  if(r==30) {
    apiIsConnected = false;
  }
  else {
    apiIsConnected = true;
  }
}

void sendSensorValue(String url, int sensor, float value){
  String json = getSensorJson(sensor, value);
  client.print(String("POST ") + url + " HTTP/1.1\r\n" +
               "Host: " + shantiHost + "\r\n" +
               "Content-Type: application/json"+ "\r\n" +
               "Content-Length: " + (json.length()+2) + "\r\n\r\n" +
                json + "\r\n\r\n");
}

String getSensorJson(int sensor, float value){
  String json = "{";
  json += "\"serial\":\"";
  json += _serialNum;
  json += "\",\"value\":\"";
  json += value;
   json += "\",\"device\":\"";
  json += sensor;
  json += "\"}";
  return json;
}
