const int tx = 7;
const int rx = 6;

char name[] = {'F', 'h'};
int bit[17];
int i;
int j;
int m;
int l;
int n = 0;
int duration = 20;

void setup(){
  pinMode(tx, OUTPUT);
  pinMode(rx, INPUT);
  Serial.begin(9600);
  digitalWrite(tx, HIGH);
  delay(1000);
}

void loop(){
  Serial.print(name[0], BIN);
  Serial.print(name[1], BIN);
  Serial.print(" ");
  bit[0] = 0;
  for(i = 0; i < 2; i++) {
    m = (int)name[i];
    for(j = (i)*7; j < (i + 1) * 7; j++) {
      l = m % 2;
      m = (int)m/2;
      bit[j + 1] = l;
    }
  }
  for(i = 1; i < 15; i++) {
    n = n + bit[i];
  }
  if (n % 2 == 0) {
    bit[15] = 0;
  }else {
    bit[15] = 1;
  }
  bit[16] = 1;
  for (i = 0; i < 17; i++) {
    digitalWrite(tx, bit[i]);
    Serial.print(bit[i]);
    delay(duration);
  }

  digitalWrite(tx, HIGH);
  Serial.println("");
  n = 0;
  delay(10000);
}
