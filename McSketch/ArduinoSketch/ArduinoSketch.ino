int light_value = 0;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(A0, INPUT);  
  pinMode(4, OUTPUT); 
}

void loop() {
  lightSensor();
 //digitalWrite(4, light_value);
  Serial.println(1);
  Serial.println(light_value);
  Serial.println(-1);
  delay(10);
}

void lightSensor(){
  float value = analogRead(A0);
  value = 1 - value/1000;
  light_value = value*100;
  //Serial.println(1);
  //Serial.println(light_value);
}
