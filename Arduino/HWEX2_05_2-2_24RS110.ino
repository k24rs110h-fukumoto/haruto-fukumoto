const int scl = 2;
const int req = 3;
const int c_ack = 4;
const int sda0 = 9;
const int sda1 = 10;
const int sda2 = 11;
const int sda3 = 12;

int state = 0;
int scl0 = HIGH;
int num = 0;
int data[] = {0, 0, 0, 0, 0, 0, 0, 0};
int val = 7;
int duration = 500;

void setup()
{
  pinMode(scl, INPUT);
  pinMode(req, INPUT);
  pinMode(sda0, INPUT);
  pinMode(sda1, INPUT);
  pinMode(sda2, INPUT);
  pinMode(sda3, INPUT);
  pinMode(c_ack, OUTPUT);

  digitalWrite(c_ack, HIGH);

  Serial.begin(9600);
  delay(50);
}

void loop()
{
  switch(state) {
    case 0:
      if(digitalRead(scl) == HIGH &&
         digitalRead(sda0) == LOW ) {
        scl0 = digitalRead(scl);
        state++;
      } else {
        delay(10);
      }
      break;

    case 1:
      if(scl0 == LOW && digitalRead(scl) == HIGH) {
        data[val] = digitalRead(sda3);
        data[val - 1] = digitalRead(sda2);
        data[val - 2] = digitalRead(sda1);
        data[val - 3] = digitalRead(sda0);

        val = val - 4;

        if(val < 0) {
          state++;
        }
      }

      scl0 = digitalRead(scl);
      delay(10);
      break;

    case 2:
      if(digitalRead(req) == LOW) {
        num = data[7];

        for(int i = 6; i > -1; i--) {
          num = num * 2 + data[i];
        }

        Serial.print("num = ");
        Serial.println(num);

        Serial.print("initial = ");
        Serial.println((char)num);

        digitalWrite(c_ack, LOW);
        delay(50);
        state++;
      } else {
        delay(10);
      }
      break;

    case 3:
      if(digitalRead(req) == HIGH) {
        digitalWrite(c_ack, HIGH);
        state = 0;
        val = 7;
        num = 0;
      } else {
        delay(10);
      }
      break;
  }
}
