double light_value = 0;
double temp_value = 0;
double wet_value = 0;
double water_value = 0;
volatile int  flow_frequency;

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  pinMode(A0, INPUT);  
  pinMode(A1, INPUT); 
  pinMode(A2, INPUT); 
  pinMode(2, INPUT); 
  pinMode(4, OUTPUT); 
  attachInterrupt(0, flow, RISING);
  sei();
}

void flow ()                 
{ 
   flow_frequency++;
} 

void loop() {
  lightSensor();
  tempSensor();
  wetSensor();
  waterSensor();
 //digitalWrite(4, light_value);
  Serial.println(-1);
  Serial.println(light_value);
  Serial.println(temp_value);
  Serial.println(wet_value);
  Serial.println(-2);
  delay(5000);
}

void lightSensor(){
  float value = analogRead(A2);
  value = 1 - value/1000;
  light_value = value*100;
}

double tempSensor(){
  float value = analogRead(A0);
  int range = 125;
  double Tf = value * (range/(1024/5.0*3.3));
  temp_value = (Tf-32)*5/9;
}

double wetSensor(){
  float value = analogRead(A1);
  wet_value = value/950;  
}

double waterSensor(){
  flow_frequency = 0;
  delay(1000);
  water_value=(flow_frequency*60/7.5);
}
