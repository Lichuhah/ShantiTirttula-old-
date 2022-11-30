WiFiClientSecure client;
int shantiPort = 443;
String shantiHost = "shantitest.somee.com";
String shantiDispHost = "shantidisp.somee.com";

void APIinit(){
  int r = 0;
  client.setFingerprint("65 DD AD A7 A5 50 DB E8 38 9E 79 13 DC AF D3 60 BF BF 9F B9");
  client.setTimeout(5000);
    while((!client.connect(shantiHost, shantiPort)) && (r < 15)){
      delay(30);
      r++;
  }

  if(r==15) {
    apiIsConnected = false;
  }
  else {
    apiIsConnected = true;
  }
}

void sendSensorValue(String url, int sensors[], float values[]){
  String json = getSensorJson(sensors, values);
  Serial.println(json);
  client.print(String("POST ") + url + " HTTP/1.1\r\n" +
               "Host: " + shantiDispHost + "\r\n" +
               "Serial: " + _serial + "\r\n" +  
               "Mac: " + WiFi.macAddress() + "\r\n" +   
               "Content-Type: application/json"+ "\r\n" +
               "Content-Length: " + (json.length()+2) + "\r\n\r\n" +
                json + "\r\n");
                
  while (client.connected()) {
    String line = client.readStringUntil('\n');
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

JsonObject& readCommand(){
    String url = "/command/get";
    client.print(String("GET ") + url + " HTTP/1.1\r\n" +
          "Host: " + shantiDispHost + "\r\n" + 
          "Serial: " + _serial + "\r\n" +  
          "Mac: " + WiFi.macAddress() + "\r\n" +        
          "Connection: close\r\n\r\n");

    while (client.connected()) {
      String line = client.readStringUntil('\n');
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
      jsonData = client.readStringUntil('\n');
      Serial.println(jsonData); //Print response
    }
    Serial.println("==========");
    Serial.println("closing connection");

    DynamicJsonBuffer jsonBuffer;
    JsonObject& root = jsonBuffer.parseObject(jsonData);
    return root;
}

String getSensorJson(int sensors[], float values[]){
  String json = "[";
  for(int i=0; i<sizeof(sensors)/sizeof(sensors[0]); i++){
    json += "{";
    json += "\"\"value\":\"";
    json += value;
    json += "\",\"device\":\"";
    json += sensor;
    json += "\"}";
  }
  json += "]";
  return json;
}
