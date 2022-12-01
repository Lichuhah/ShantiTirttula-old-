void runCommands(String command){
  if(apiIsConnected){
     executeCommand(command);
  }
}

void executeCommand(String command){
  if(command.length()> 6){
    DynamicJsonBuffer jsonBuffer;
    JsonArray& array = jsonBuffer.parseArray(command);
    for(JsonVariant v : array) {
      int pin = v["Pin"].as<int>();
      int value = v["Value"].as<int>();
      bool isPWM = v["IsPwm"].as<bool>();
      Serial.println(pin);
      Serial.println(value);
      Serial.println(isPWM);
      if(isPWM){
        analogWrite(pin, value);
      } else {
        if(value==1){ digitalWrite(pin, HIGH); }
      else { digitalWrite(pin, LOW); }
      } 
    }
  }
}
