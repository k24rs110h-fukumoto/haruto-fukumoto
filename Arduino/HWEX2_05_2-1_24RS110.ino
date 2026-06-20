const int scl = 2;
const int req = 3;
const int c_ack = 4;
const int sda0 = 9;
const int sda1 = 10;
const int sda2 = 11;
const int sda3 = 12;

int state = 0;
int data[] = {0, 1, 1, 0, 0, 0, 1, 0};
int duration = 500;

void setup()
{
  pinMode(scl, OUTPUT);
  pinMode(req, OUTPUT);
  pinMode(c_ack, INPUT);
  pinMode(sda0, OUTPUT);
  pinMode(sda1, OUTPUT);
  pinMode(sda2, OUTPUT);
  pinMode(sda3, OUTPUT);

  Serial.begin(9600);

  digitalWrite(sda0, HIGH);
  digitalWrite(sda1, HIGH);
  digitalWrite(sda2, HIGH);
  digitalWrite(sda3, HIGH);
  digitalWrite(scl, HIGH);
  digitalWrite(req, HIGH);

  delay(2000);
}

void clock(int val)
{
  digitalWrite(sda3, data[val]);
  digitalWrite(sda2, data[val - 1]);
  digitalWrite(sda1, data[val - 2]);
  digitalWrite(sda0, data[val - 3]);

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
        digitalWrite(sda0, LOW);
        digitalWrite(sda1, LOW);
        digitalWrite(sda2, LOW);
        digitalWrite(sda3, LOW);
        digitalWrite(scl, HIGH);
        delay(2000);
        state++;
      } else {
        delay(10);
      }
      break;

    case 1:
      clock(7);
      clock(3);

      digitalWrite(scl, HIGH);
      digitalWrite(sda0, HIGH);
      digitalWrite(sda1, HIGH);
      digitalWrite(sda2, HIGH);
      digitalWrite(sda3, HIGH);
      digitalWrite(req, LOW);
      delay(50);
      state++;
      break;

    case 2:
      if(digitalRead(c_ack) == LOW) {
        digitalWrite(req, HIGH);
        state = 0;
        delay(3000);
      } else {
        delay(10);
      }
      break;
  }
}
