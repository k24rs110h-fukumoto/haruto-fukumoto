const int scl = 13;
const int start = 2;

const int duration = 500;
int state = 0;

void setup()
{
  pinMode(scl, OUTPUT);
  pinMode(start, INPUT);
  
  digitalWrite(scl, HIGH);
}

void loop()
{
  delay(10);
  
  switch(state){
    case 0:
      if(digitalRead(start) == HIGH) {
        state++;
      }
      break;
    case 1:
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    case 2:
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    case 3:
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state++;
      break;
    case 4:
      digitalWrite(scl, LOW);
      delay(duration);
      digitalWrite(scl, HIGH);
      delay(duration);
      state = 0;
      break;
  }
}
