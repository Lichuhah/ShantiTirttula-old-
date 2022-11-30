void readSensors(){
  if(apiIsConnected){
    while(true){
      int signal = Serial.parseInt();
      if(signal==-1){
        float light = Serial.parseFloat();
        float temp = Serial.parseFloat();
        float wet = Serial.parseFloat();
        int sensors[3] = {1,2,3};
        float datas[3] = {light, temp, wet};
        sendSensorValue("/sensor/send",sensors,datas);
        break;
      }
    }
  }
}
