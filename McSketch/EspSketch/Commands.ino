void runCommands(){
  if(apiIsConnected){
     executeCommand(readCommand());
     delay(10);
  }
}

void executeCommand(JsonObject& root){
  int pin = root["Pin"].as<int>();
  int value = root["Value"].as<int>();
  bool isPWM = root["isPWM"].as<bool>();
  if(isPWM){
    analogWrite(pin, value);
  } else {
    if(value==1){ digitalWrite(pin, HIGH); }
    else { digitalWrite(pin, LOW); }
  } 
}
