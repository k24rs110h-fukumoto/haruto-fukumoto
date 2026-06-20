//受信側
const int scl = 2;
const int sda = 3;
const int req = 4;
const int c_ack = 7;
int num = 0;
int state = 0;
int scl0 = 1;
int data[] = {0, 0, 0, 0};
int duration = 500;
 
void setup()
{
  pinMode(scl, INPUT);
  pinMode(sda, INPUT);
  pinMode(req, INPUT);
  pinMode(c_ack, OUTPUT);
  digitalWrite(c_ack, HIGH);
  Serial.begin(9600);
}
 
void loop()
{
  switch(state){
    case 0:
    if(digitalRead(scl) == HIGH && digitalRead(sda) == LOW){
      delay(10);
      state++;
    }
    break;
    case 1:
    if(scl0 == LOW && digitalRead(scl) == HIGH){
       data[3] = digitalRead(sda);
       delay(10);
     state++;      
    }else{
      scl0 = digitalRead(scl);
      delay(10);
    }
    break;
    case 2:
    if (digitalRead(req) == LOW){
      num = data[3];
      Serial.print("num = ");
      Serial.println(num);
      digitalWrite(c_ack, LOW);
      delay(50);
      state++;
    }
    break;
    case 3:
    if(digitalRead(req) == HIGH){
      state = 0;
      digitalWrite(c_ack, HIGH);
    } 
    break;
  }
}
