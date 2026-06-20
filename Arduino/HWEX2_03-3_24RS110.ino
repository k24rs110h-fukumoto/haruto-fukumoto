const int scl = 13;
const int sda = 10;
const int c_ack = 7;
const int c_req = 6;
const int start = 2;

const int duration = 500;
int state = 0;
int data[] = {0, 1, 0, 1};

void setup()
{
  pinMode(scl, OUTPUT);
  pinMode(sda, OUTPUT);
  pinMode(c_ack, INPUT_PULLUP);
  pinMode(c_req, OUTPUT);
  pinMode(start, INPUT);
  
  digitalWrite(scl, HIGH);
  digitalWrite(c_req, HIGH);
  digitalWrite(sda, LOW);
}

void loop()
{
  delay(10);
  
  switch(state){
    case 0:
      digitalWrite(c_req, HIGH);
      if(digitalRead(start) == HIGH) {
          state++;
      }
      break;
    case 1:
      digitalWrite(sda, data[4 - state]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    case 2:
      digitalWrite(sda, data[4 - state]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    case 3:
      digitalWrite(sda, data[4 - state]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    case 4:
      digitalWrite(sda, data[4 - state]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    case 5:
      digitalWrite(c_req, LOW);
      if(digitalRead(c_ack) == LOW){
          state = 0;
      }
  }
}
