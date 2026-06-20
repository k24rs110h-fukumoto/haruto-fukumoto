const int tx = 7;
const int rx = 6;

int l;
int m;
int i;
int j;
int n = 0;
int state = 0;
char name[2];
int bit[] = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
int duration = 20;

void setup()
{
  pinMode(tx, OUTPUT);
  pinMode(rx, INPUT);
  Serial.begin(9600);
  digitalWrite(tx, HIGH);
}

void loop()
{
  switch (state) {
    case 0:
      if (digitalRead(rx) == LOW) {
        delay(30);
        for (i = 0; i < 15; i++) {
          bit[i] = digitalRead(rx);
          Serial.print(bit[i]);
          n = n + bit[i];
          delay(duration);
        }
        state++;
      } else {
        delay(2);
      }
      break;

    case 1:
      if (n % 2 == 0) {
        for (i = 0; i < 2; i++) {
          m = 0;
          for (j = 0; j < 7; j++) {
            m = m + bit[i * 7 + j] * (1 << j);
          }
          name[i] = (char)m;
          Serial.print(name[i]);
        }
        Serial.println("");
      } else {
        Serial.println("OUT");
      }

      n = 0;
      m = 0;
      state = 0;
      delay(1000);
      break;
  }
}
