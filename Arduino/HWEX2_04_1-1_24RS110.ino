//送信側
const int scl = 2;
const int sda = 3;
const int req = 4;
const int c_ack = 7;
int num = 0;
int state = 0;
int data[] = {1, 0, 0, 1};
int duration = 500;
 
void setup()
{
  pinMode(scl, OUTPUT);
  pinMode(sda, OUTPUT);
  pinMode(req, OUTPUT);
  pinMode(c_ack, INPUT);
  Serial.begin(9600);
  digitalWrite(scl, HIGH);
  digitalWrite(sda, HIGH);
  digitalWrite(req, HIGH);
  delay(1000);
}
 
void loop()
{
  switch(state){
    case 0:
    if(digitalRead(c_ack) == HIGH){
      digitalWrite(sda,LOW);
      state++;
    }
    break;
    case 1:
    digitalWrite(scl, LOW);
    digitalWrite(sda, data[3]);
    delay(duration);
    digitalWrite(scl, HIGH);
    delay(duration);
    state++;
    break;
    case 2:
    digitalWrite(req, LOW);
    digitalWrite(scl, HIGH);
    digitalWrite(sda, HIGH);
    state++;
    break;
    case 3:
    if(digitalRead(c_ack) == LOW){
      digitalWrite(req, HIGH);
      delay(300);
    } else {}
    state = 0;
    break;
  }
}
