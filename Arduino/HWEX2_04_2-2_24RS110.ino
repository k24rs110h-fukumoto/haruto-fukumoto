const int scl = 2;
const int sda = 3;
const int req = 4;
const int c_ack = 7;
int state = 0;
int scl0 =1;
int num = 0;
int data[] = {0,0,0,0};
int val = 0;
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
  switch(state) {
    case 0:
      if(digitalRead(scl) == HIGH && digitalRead(sda) == LOW) {
          state++;
        } else { }
      break;
    case 1:
      if(scl0 == LOW && digitalRead(scl) == HIGH) {
          data[3] = digitalRead(sda);
          state++;
        } else {
          scl0 = digitalRead(scl);
          delay(10);
        }
      break;
    case 2:
      if(scl0 == LOW && digitalRead(scl) == HIGH) {
          data[2] = digitalRead(sda);
          state++;
        } else {
          scl0 = digitalRead(scl);
          delay(10);
        }
      break;
    case 3:
      if(scl0 == LOW && digitalRead(scl) == HIGH) {
          data[1] = digitalRead(sda);
          state++;
        } else {
          scl0 = digitalRead(scl);
          delay(10);
        }
      break;
    case 4:
      if(scl0 == LOW && digitalRead(scl) == HIGH) {
          data[0] = digitalRead(sda);
          state++;
        } else {
          scl0 = digitalRead(scl);
          delay(10);
        }
      break;
    case 5:
      if(digitalRead(req) == LOW) {
          num = data[0] * 8 + data[1] * 4 + data[2] * 2 + data[3];
          Serial.print("num=");
          Serial.println(num);
          digitalWrite(c_ack, LOW);
          delay(50);
          state++;
        } else { }
      break;
    case 6:
      if(digitalRead(req) == HIGH) {          
          digitalWrite(c_ack, HIGH);
          state = 0;
        }
      break;
  }
}
