const int scl = 2;
const int sda = 3;
const int req = 4;
const int c_ack = 7;
int state = 0;
int scl0 = 1;
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
        val = 3;
        num = 0;
        scl0 = digitalRead(scl);
        state++;
      }
      break;

    case 1:
      if(scl0 == LOW && digitalRead(scl) == HIGH) {
        data[val] = digitalRead(sda);
        val--;
        scl0 = digitalRead(scl);
      } else {
        scl0 = digitalRead(scl);
        delay(10);
      }
      if(val < 0) {
        state++;
      }
      break;

    case 2:
      if(digitalRead(req) == LOW) {
        num = data[3];

        for(int i = 2; i > -1; i--) {
          num = num * 2 + data[i];
        }

        Serial.print("num=");
        Serial.println(num);

        digitalWrite(c_ack, LOW);
        delay(50);
        state++;
      }
      break;

    case 3:
      if(digitalRead(req) == HIGH) {
        digitalWrite(c_ack, HIGH);
        state = 0;
      }
      break;
  }
}
