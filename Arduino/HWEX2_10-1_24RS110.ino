int main()
{
  DDRD = 0b11100000;
  DDRB = 0b00011100;
  
  PORTD = 0b11100000;
  PORTB = 0b00011100;
  
  for(;;)
  {
    for(int row = 5; row <= 7; row++)
    {
      for(int column = 4; column >= 2; column--)
      {
         PORTD = ~(1 << row);
         PORTB = (1 << column);;
         _delay_ms(500);
      }
    }
  }
  
  return 0;
}
