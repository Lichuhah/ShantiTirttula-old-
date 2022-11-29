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
int currentTics = 0;

void setup() {
  Serial.begin(9600);
  pinMode(16, OUTPUT);
  pinMode(4, OUTPUT);
  pinMode(5, OUTPUT);
  digitalWrite(16, LOW);
  digitalWrite(4, LOW);
  digitalWrite(5, LOW);
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
  HTTP.handleClient();
  currentTics++;
  if(currentTics==10000){
    APIinit();
    readSensors();
    currentTics=0;
  }
  //culerWork(0.60);
  delay(1);
}


void readCommands(){
  WiFiClientSecure client;
  int port = 443;
  String host = "shantitest.somee.com";
  String url = "/mccommand/getcommand?key="+_serialNum;

  //client.setFingerprint(fingerprint);
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
   if(b){ digitalWrite(16,HIGH); } else { digitalWrite(16,LOW); }
  }
  if(a=="water"){
   if(b){ digitalWrite(5,HIGH); } else { digitalWrite(5,LOW); }
  }
  if(a=="cooler"){
   if(b){ digitalWrite(4,HIGH); } else { digitalWrite(4,LOW); }
  }
  
}
