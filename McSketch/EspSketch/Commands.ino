void runCommands(){
  if(apiIsConnected){
     executeCommand(readCommand());
     delay(10);
  }
}

void executeCommand(String command){
  if(command!="free"){
  DynamicJsonBuffer jsonBuffer;
  JsonObject& root = jsonBuffer.parseObject(command);
  int pin = root["Pin"].as<int>();
  int value = root["Value"].as<int>();
  bool isPWM = root["IsPwn"].as<bool>();
  if(isPWM){
    analogWrite(pin, value);
  } else {
    if(value==1){ digitalWrite(pin, HIGH); }
    else { digitalWrite(pin, LOW); }
  } 
  }
}
