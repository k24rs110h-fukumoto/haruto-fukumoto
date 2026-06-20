#define CTOP 1000
#define CMP 750
#define SPEAKER_TOP 5

int count = 0;
int speakerCount = 0;

int main(void)
{
    DDRD |= 0b00001100;
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

        for (int i = 0; i < 10; i++)
        {
            if ((PORTD & 0b00000100) || (PORTB & 0b00100000))
            {
                speakerCount++;

                if (speakerCount >= SPEAKER_TOP)
                {
                    PORTD ^= 0b00001000;
                    speakerCount = 0;
                }
            }
            else
            {
                PORTD &= 0b11110111;
                speakerCount = 0;
            }

            _delay_us(100);
        }

        count++;
    }

    return 0;
}
