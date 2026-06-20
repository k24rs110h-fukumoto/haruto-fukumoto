#include <SoftwareSerial.h>
#include <stdlib.h>

SoftwareSerial mySerial(6, 7);

char x = 'F'; //01000110

void setup()
{
  Serial.begin(9600);
  mySerial.begin(300);
  delay(1000);
}

void loop()
{
  Serial.println(x, BIN);
  mySerial.print(x);
  delay(3000);
}
