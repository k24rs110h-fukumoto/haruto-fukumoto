const int scl = 2;
const int sda = 3;
const int req = 4;
const int c_ack = 7;
int num = 0;
int data[] = {1,0,0,0};
int state = 0;
int duration = 500;
int val = 0;

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

void clock(int val) {
  digitalWrite(sda, data[val]);
  digitalWrite(scl, LOW);
  delay(duration);
  digitalWrite(scl, HIGH);
  delay(duration);
}

void loop()
{
  switch(state) {
    case 0:
      if(digitalRead(c_ack) == HIGH) {
        
        digitalWrite(sda, LOW);
        delay(duration);
        state++;
      }
      break;

    case 1:
      while (val > -1) {
        clock(val);
        val--;
      }

      digitalWrite(scl, HIGH);
      digitalWrite(sda, HIGH);
      digitalWrite(req, LOW);
      delay(50);
      state++;
      break;

    case 2:
      if (digitalRead(c_ack) == LOW) {
        digitalWrite(req, HIGH);
        delay(3000);
        state = 0;
        val = 3;
      }
      break;
  }
}
