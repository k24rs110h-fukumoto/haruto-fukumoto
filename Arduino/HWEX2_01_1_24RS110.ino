int led_red = 2;
int led_yellow = 3;
int led_green = 4;
int out_red = 10;
int in_red = 9;
int state = 0;

void setup()
{
  pinMode(led_red, OUTPUT);
  pinMode(led_yellow, OUTPUT);
  pinMode(led_green, OUTPUT);
  pinMode(out_red, OUTPUT);
  pinMode(in_red, INPUT);
  
  digitalWrite(led_red, HIGH);
  digitalWrite(led_yellow, LOW);
  digitalWrite(led_green, LOW);
  digitalWrite(out_red, LOW);
  delay(3000);
}

void loop()
{
  switch (state)
  {
    case 0:
    digitalWrite(led_red, LOW);
    digitalWrite(led_yellow, LOW);
    digitalWrite(led_green, HIGH);
    delay(2000);
    state++;
    break;
    
    case 1:
    digitalWrite(led_red, LOW);
    digitalWrite(led_yellow, HIGH);
    digitalWrite(led_green, LOW);
    delay(1000);
    state++;
    break;
    
    case 2:
    digitalWrite(led_red, HIGH);
    digitalWrite(led_yellow, LOW);
    digitalWrite(led_green, LOW);
    delay(3000);
    digitalWrite(out_red, HIGH);
    delay(1000);
    digitalWrite(out_red, LOW);
    state++;
    break;
    
    case 3:
    if(digitalRead(in_red) == HIGH)
    {
      state = 0;
    }
    break;
  }
}
