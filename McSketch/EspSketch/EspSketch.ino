#include <ESP8266WiFi.h>        
#include <ESP8266WebServer.h>   
#include <ESP8266SSDP.h>   
#include <ESP8266HTTPClient.h> 
#include <FS.h>
#include <ArduinoJson.h>
#include <WiFiClientSecure.h>
IPAddress apIP(192, 168, 4, 1);

// Web интерфейс для устройства
ESP8266WebServer HTTP(80);
// Для файловой системы
File fsUploadFile;

// Определяем переменные wifi
String _ssid     = "TP-LINK_F7FE"; // Для хранения SSID
String _password = "393-63-461"; // Для хранения пароля сети
String _newssid = "";
String _newpass = "";
String _ssidAP = "Shanti";   // SSID AP точки доступа
String _passwordAP = ""; // пароль точки доступа
String SSDP_Name = ""; // Имя SSDP
String _serialNum="";
String _login="";
String jsonConfig = "{}";
const char* fingerprint = "65 DD AD A7 A5 50 DB E8 38 9E 79 13 DC AF D3 60 BF BF 9F B9";
int currentTics = 0;

void setup() {
  Serial.begin(9600);
  pinMode(5, OUTPUT);
  Serial.println("");
  //Запускаем файловую систему
  Serial.println("Start 4-FS");
  FS_init();
  Serial.println("Step7-FileConfig");
  loadConfig();
  Serial.println("Start 1-WIFI");
  //Запускаем WIFI
  WIFIinit();
  //Настраиваем и запускаем SSDP интерфейс
  Serial.println("Start 3-SSDP");
  SSDP_init();
  //Настраиваем и запускаем HTTP интерфейс
  Serial.println("Start 2-WebServer");
  HTTP_init();
}

void loop() {
  //HTTP.handleClient();
  currentTics++;
  if(currentTics==10000){
    readSensors();
  }
  delay(1);
}

void readSensors(){
    //readLight();
    readCommands();
    currentTics=0;
}

void readLight(){
    while(true){
      int signal = Serial.parseInt();
      if(signal==1){
        sendLight(Serial.parseInt());
        break;
      }
    }
}

void sendLight(int value){
  WiFiClientSecure client;
  int port = 443;
  String host = "shantitest.somee.com";
  String url = "/mcdata/sendlight";
  String json = "{";
  json += "\"serial\":\"";
  json += _serialNum;
  json += "\",\"value\":\"";
  json += value;
  json += "\"}";

  client.setFingerprint(fingerprint);
  client.setTimeout(5000);
  int r=0; //retry counter
  while((!client.connect(host, port)) && (r < 30)){
      delay(100);
      Serial.print(".");
      r++;
  }

  if(r==30) {
    Serial.println("Connection failed");
  }
  else {
    Serial.println("Connected to web");
  }
  client.print(String("POST ") + url + " HTTP/1.1\r\n" +
               "Host: " + host + "\r\n" +
               "Content-Type: application/json"+ "\r\n" +
               "Content-Length: " + (json.length()+2) + "\r\n\r\n" +
                json + "\r\n" +
               "Connection: close\r\n\r\n");

  Serial.println("request sent");
                  
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

void readCommands(){
  WiFiClientSecure client;
  int port = 443;
  String host = "shantitest.somee.com";
  String url = "/mccommand/getcommand?key="+_serialNum;

  client.setFingerprint(fingerprint);
  client.setTimeout(1000);
  int r=0; //retry counter
  while((!client.connect(host, port)) && (r < 30)){
      delay(100);
      Serial.print(".");
      r++;
  }

  if(r==30) {
    Serial.println("Connection failed");
  }
  else {
    Serial.println("Connected to web");
  }

   client.print(String("GET ") + url + " HTTP/1.1\r\n" +
               "Host: " + host + "\r\n" +           
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
  String a = root["a"].as<String>(); 
  bool b = root["b"].as<bool>();
  if(a=="light"){
    if(b){
      digitalWrite(5,HIGH);
    } else {
      digitalWrite(5,LOW);
    }
  }
}
