#define CTOP 1000

int count = 0;

void setup()
{
  DDRD = 0b00000100;
}

void loop()
{
  if(count == CTOP)
  {
    PORTD |= 0b00000100;
  } else if(count >= CTOP*2)
  {
    PORTD &= 0b11111011;
    count = 0;
  }
  count++;
  _delay_ms(1);
}

int main(void)
{
  setup();
  for (;;)
  {
    loop();
  }
  return 0;
}
