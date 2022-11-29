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

String _ssid     = "TP-LINK_F7FE"; // Для хранения SSID
String _password = "393-63-461"; // Для хранения пароля сети
String _newssid = "";
String _newpass = "";
String _passwordAP = ""; // пароль точки доступа
String _ssidAP = "Shanti";   // SSID AP точки доступа
String SSDP_Name = ""; // Имя SSDP
String _serialNum="";
String _login="";
String jsonConfig = "{}";
int currentTics = 0;
bool apiIsConnected = false;

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
    runCommands();
    currentTics=0;
  }
  delay(1);
}
