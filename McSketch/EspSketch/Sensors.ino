void readSensors(){
  if(apiIsConnected){
    while(true){
      int signal = Serial.parseInt();
      if(signal==-1){
        sendLight(Serial.parseFloat());
        delay(10);
        sendTemp(Serial.parseFloat());
        delay(10);
        sendWet(Serial.parseFloat());
        delay(10);
        break;
      }
    }
  }
}

void sendLight(float val){
  sendSensorValue("/mcdata/sendsensor",1,val);
}

void sendTemp(float val){
  sendSensorValue("/mcdata/sendsensor",2,val);
}

void sendWet(float val){
  sendSensorValue("/mcdata/sendsensor",3,val);
}
