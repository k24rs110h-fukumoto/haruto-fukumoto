const int scl = 13;
const int sda = 10;
const int c_ack = 7;
const int c_req = 6;
const int start = 2;

const int duration = 500;
int state = 0;
int data[] = {1, 0, 1, 0, 0, 0, 0, 0};

void setup()
{
  pinMode(scl, OUTPUT);
  pinMode(sda, OUTPUT);
  pinMode(c_ack, INPUT_PULLUP);
  pinMode(c_req, OUTPUT);
  pinMode(start, INPUT);
  
  digitalWrite(scl, HIGH);
  digitalWrite(c_req, HIGH);
  digitalWrite(sda, HIGH);
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
    case 2:
    case 3:
    case 4:
    case 5:
    case 6:
    case 7:
    case 8:
      digitalWrite(sda, data[8 - state]);
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    case 9:
      digitalWrite(c_req, LOW);
      digitalWrite(sda, HIGH);
      if(digitalRead(c_ack) == LOW){
          state = 0;
      }
  }
}
