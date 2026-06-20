#include <SoftwareSerial.h>
#include <stdlib.h>

SoftwareSerial mySerial(6, 7);

const int led = 3;
int x = 0;

void setup()
{
  Serial.begin(9600);
  mySerial.begin(300);
  pinMode(led, OUTPUT);
  digitalWrite(led, LOW);
}

void loop()
{
  if (mySerial.available() > 0) {
    digitalWrite(led, LOW);
    x = mySerial.read();
    Serial.println((char)x);

    if (x == 'b') {
      digitalWrite(led, HIGH);
      delay(500);
      digitalWrite(led, LOW);
    }
  }
}
