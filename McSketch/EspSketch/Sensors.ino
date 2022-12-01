void readSensors(){
  if(apiIsConnected){
    while(true){
      int signal = Serial.parseInt();
      if(signal==-1){
        float light = Serial.parseFloat();
        float temp = Serial.parseFloat();
        //float wet = Serial.parseFloat();
        int sensors[2] = {1,2};
        float datas[2] = {temp, light};
        sendSensorValue("/sensor/send",sensors,datas);
        break;
      }
    }
  }
}
