#include <SoftwareSerial.h>
#include <stdlib.h>

SoftwareSerial mySerial(6, 7);

int x = 'b';

void setup()
{
  Serial.begin(9600);
  mySerial.begin(300);
  delay(1000);
}

void loop()
{
  Serial.println(x);
  mySerial.write(x);
  delay(3000);
}
