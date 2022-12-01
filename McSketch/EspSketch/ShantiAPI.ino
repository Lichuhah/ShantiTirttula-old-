WiFiClientSecure client;
int shantiPort = 443;
String shantiHost = "shantitest.somee.com";
String shantiDispHost = "shantidisp.somee.com";

void APIinit(){
  int r = 0;
  //client.setFingerprint("65 DD AD A7 A5 50 DB E8 38 9E 79 13 DC AF D3 60 BF BF 9F B9");
  client.setFingerprint("F2 09 F2 84 E3 83 28 6F 71 D2 25 E1 AD 09 95 23 95 BC D0 81");
  client.setTimeout(500);
    while((!client.connect(shantiDispHost, shantiPort)) && (r < 40)){
      delay(30);
      r++;
  }

  if(r==40) {
    apiIsConnected = false;
  }
  else {
    apiIsConnected = true;
  }
}

String sendSensorValue(String url, int sensors[], float values[]){
  if(client.connect(shantiDispHost, shantiPort)){            
    String json = getSensorJson(sensors, values);
    Serial.println(json);
    client.println("POST " + url + " HTTP/1.1");
    client.println("Host: " + shantiDispHost);
    client.println("Serial: " + _serialNum);
    client.println("Mac: " + WiFi.macAddress());
    client.println("Connection: Close");
    client.println("Content-Type: application/json;");
    client.print("Content-Length: ");
    client.println(json.length()+2);
    client.println();
    client.println(json);
    delay(1);
    String response = client.readString();
    int bodypos =  response.indexOf("\r\n\r\n") + 4;
    response = response.substring(bodypos);
    bodypos = response.indexOf("\r\n") + 1;
    response = response.substring(bodypos);
    bodypos = response.indexOf("\r\n");
    response = response.substring(0, bodypos);
    Serial.println(response);
    return response;
  }
}

String getSensorJson(int sensors[], float values[]){
  int leng = 2;
  String json = "[";
  for(int i=0; i<leng; i++){
    json += "{";
    json += "\"Value\":\"";
    json += values[i];
    json += "\",\"SensorId\":\"";
    json += sensors[i];
    json += "\"}";
    if(i!=leng-1) json+=",";
  }
  json += "]";
  return json;
}
