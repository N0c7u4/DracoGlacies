/**
*  @Autor : St3v3n-4n4
*  @Modificacion : 08-02-2023
*  @commit : Codigo de arduino micro para el ejecutable en windows 10 y script de linux
*  @Instalador de windows 10 : https://github.com/St3v3n-4n4/DracoGlacies/blob/main/Instalador%20Windows%2010/DracoGlacies.exe 
**/


String dato,pwm;
boolean conexion=false;
long t=0,tiempo=0;

void setup() {
  Serial.begin(9600);
  pinMode(6,OUTPUT);
  digitalWrite(6,0);
  
}

void loop() {
    tiempo=millis();
   if((tiempo-t)>(5000) && conexion==true)
      {
        conexion=false;
        digitalWrite(6,0);
      }
  
  if(Serial.available())
  {
      t=tiempo;
      dato=Serial.readString();
      
      // dato=1210717  ... Password de inicio
      if(dato.toInt()==1210717 && conexion==false)
      {
        Serial.println(dato);
        conexion=true;      
      }      
      // dato=X-PWM ... Ejemplo on -> dato=1-127 off -> dato=0-127
      if(String(dato[1])=="-" && conexion==true)
      {
        if(String(dato[0]).toInt()==1)
        {
          digitalWrite(6,1);
        }
        else
        {
          digitalWrite(6,0);
        }
        pwm="";
        for(int i=2;i<dato.length();i++)
        {
          pwm=pwm+dato[i];
        }
        analogWrite(3,pwm.toInt());
      }    
  }
}
