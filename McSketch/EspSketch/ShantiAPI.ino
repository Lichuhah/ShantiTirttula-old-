WiFiClientSecure client;
int shantiPort = 443;
String shantiHost = "shantitest.somee.com";
String shantiDispHost = "shantidisp.somee.com";

void APIinit(){
  int r = 0;
  client.setFingerprint("F2 09 F2 84 E3 83 28 6F 71 D2 25 E1 AD 09 95 23 95 BC D0 81");
  client.setTimeout(5000);
    while((!client.connect(shantiDispHost, shantiPort)) && (r < 15)){
      delay(30);
      r++;
  }

  if(r==15) {
    apiIsConnected = false;
  }
  else {
    Serial.println("api is connected");
    apiIsConnected = true;
  }
}

void sendSensorValue(String url, int sensors[], float values[]){
  String json = getSensorJson(sensors, values);
  Serial.println(json);
  client.print(String("POST ") + url + " HTTP/1.1\r\n" +
               "Host: " + shantiDispHost + "\r\n" +
               "Serial: " + _serialNum + "\r\n" +  
               "Mac: " + WiFi.macAddress() + "\r\n" +   
               "Content-Type: application/json"+ "\r\n" +
               "Content-Length: " + (json.length()+2) + "\r\n\r\n" +
                json + "\r\n");
                
  while (client.connected()) {
    String line = client.readStringUntil('\n');
    Serial.println(line);
    if (line == "\r") {
      Serial.println("headers received");
      break;
    }
  }

  Serial.println("reply was:");
  Serial.println("==========");
  String line;
  while(client.available()){        
    line = client.readStringUntil('\n');  //Read Line by Line
    Serial.println(line); //Print response
  }
  Serial.println("==========");
  Serial.println("closing connection");
}

String readCommand(){
    String url = "/command/get";
    client.print(String("GET ") + url + " HTTP/1.1\r\n" +
          "Host: " + shantiDispHost + "\r\n" + 
          "Serial: " + _serialNum + "\r\n" +  
          "Mac: " + WiFi.macAddress() + "\r\n");

    while (client.connected()) {
      String line = client.readStringUntil('\n');
      Serial.println(line);
      if (line == "\r") {
        Serial.println("headers received");
        break;
      }
    }

    Serial.println("reply was:");
    Serial.println("==========");
    String line;
    String jsonData;
    if(client.available()){        
      line = client.readStringUntil('\n');  //Read Line by Line
       Serial.println(line);
      jsonData = client.readStringUntil('\n');
       Serial.println(jsonData);
      Serial.println(jsonData); //Print response
    }
    Serial.println("==========");
    Serial.println("closing connection");

    return jsonData;
}

String getSensorJson(int sensors[], float values[]){
  String json = "[";
  for(int i=0; i<2; i++){
    json += "{";
    json += "\"Value\":\"";
    json += values[i];
    json += "\",\"SensorId\":\"";
    json += sensors[i];
    json += "\"}";
    if(i!=1) json+=",";
  }
  json += "]";
  return json;
}
