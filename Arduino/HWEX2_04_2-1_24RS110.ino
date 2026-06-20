const int scl = 2;
const int sda = 3;
const int req = 4;
const int c_ack = 7;
int num = 0;
int data[] = {0,0,0,0};
int state = 0;
int duration = 500;

void setup()
{
  pinMode(scl, OUTPUT);
  pinMode(sda, OUTPUT);
  pinMode(req, OUTPUT);
  pinMode(c_ack, INPUT);
  Serial.begin(9600);
  digitalWrite(sda, HIGH);
  digitalWrite(scl, HIGH);
  digitalWrite(req, HIGH);
  delay(2000);
}

void loop()
{
  switch(state) {
    case 0:
      if(digitalRead(c_ack) == HIGH) {
        digitalWrite(sda, LOW);
        state++;
      }
      break;
    
    case 1:
        digitalWrite(sda, data[3]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    
    case 2:
        digitalWrite(sda, data[2]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    
    case 3:
        digitalWrite(sda, data[1]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    
    case 4:
        digitalWrite(sda, data[0]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    
    case 5:
      digitalWrite(req, LOW);
      digitalWrite(scl, HIGH);
      digitalWrite(sda, HIGH);
      state++;
      break;
    
    case 6:
      if(digitalRead(c_ack) == LOW) {
          digitalWrite(req, HIGH);
          state = 0;
        }
      break;
  }
}
