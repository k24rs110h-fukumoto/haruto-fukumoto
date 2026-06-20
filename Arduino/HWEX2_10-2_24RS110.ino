int main()
{
  unsigned char led[4][3] = {
  {
    0b00010000,
    0b00000000,
    0b00000100
  },
  {
    0b00001000,
    0b00000000,
    0b00001000
  },
  {
    0b00000100,
    0b00000000,
    0b00010000
  },
  {
    0b00001000,
    0b00000000,
    0b00001000
  }
};
  
  int state = 0;
  int pattern = 0;
  
  init();
  Serial.begin(9600);
  
  DDRD = 0b10100000;
  DDRB = 0b00011100;
  
  PORTD = 0b11100100;
  PORTB = 0b00000000;
  
  for(;;)
  {
    for(int row = 5; row <= 7; row++)
    {
      PORTD = ~(1 << row);
      PORTB = led[pattern][row-5];;
      _delay_ms(1);
      switch(state)
      {
        case 0:
          if(~PIND >> 2 & 1 == 1)
          {
            Serial.println("pushe!");
            state++;
            pattern++;
            
            if(pattern >= 4)
            {
              pattern = 0;
            }
          }
          break;
        case 1:
          if(PIND >> 2 & 1 == 1)
          {
            Serial.println("released!");
            state = 0;
          }
          break;
      }
    }
  }
  
  return 0;
}
