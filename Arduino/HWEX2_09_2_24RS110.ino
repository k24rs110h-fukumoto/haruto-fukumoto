#define CTOP 1000
#define CMP 750

int count = 0;

int main(void)
{
    DDRD |= 0b00000100;
    DDRB |= 0b00100000;

    for (;;)
    {
        if (count == CMP)
        {
            PORTB ^= 0b00100000;
        }

        if (count >= CTOP)
        {
            PORTD ^= 0b00000100;
            count = 0;
        }

        count++;
        _delay_ms(1);
    }

    return 0;
}
