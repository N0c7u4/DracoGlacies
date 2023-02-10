#!/bin/bash

# Archivo de configuracion...
temp=$(cat ~/.DracoGlacies.conf | grep tempPorcentaje | awk -F '=' '{print $2}')
IFS=' ' read -ra tempV <<< "$(echo $temp)"

if [ "$1" = "info" ];
then
	echo -e "Archivo de Configuracion... dir : ~/.DraciGlacies.conf\n\n"
	echo -e "|temperaturas \t\t|  40°C\t| 50°C\t| 60°C\t| 70°C\t| 80°C\t| 90°C\t| 100°C\t|"
	echo -e "================================================================================="
	echo -e "|Valores Porcentaje\t|  ${tempV[0]}%\t| ${tempV[1]}%\t| ${tempV[2]}%\t| ${tempV[3]}%\t| ${tempV[4]}%\t| ${tempV[5]}%\t| ${tempV[6]}%\t|"
	exit
fi

# Buscamos puertos
IFS=' ' read -ra puertos <<< "$(echo $(sudo dmesg | grep -E 'Arduino Micro|ttyACM' | grep 'tty' |awk -F ':' '{print $3 }'))"
# si no hay ninguno...
if [ ${#puertos[@]} -eq 0 ];
then
	echo -e "No se encontro el Arduino Micro\n\tPorfavor oprima el boton de RESET del arduino..."
	echo "Vuelve a intenar ejecutar el script luego de hacerlo..."
	exit
fi
# Verificar que no se repita el nombre del puerto y si es asi eliminar...
elementos=$(echo "${#puertos[@]}")
n=0
id=0
for i in ${puertos[@]}
do
	for j in ${puertos[@]}
	do
		if [ "$i" = "$j" ];
		then
			n=$(($n + 1))
		fi
	done
	if [ $n -gt 1 ];
	then
		echo "si hay iguales y es : $i posicion : $id"
		puertos=( "${puertos[@]:0:$id}" "${puertos[@]:$(($elementos - 1))}")
	fi
	n=0
	id=$(($id + 1))
done
elementos=$(echo "${#puertos[@]}")
# verificamos si hay mas de un dispositivo arduino micro conectado
if [ $elementos -gt 1 ];
then
	echo "Seleccione el dispositivo a usar :"
	n=0
	for i in ${puertos[@]}
	do
		echo "($n) $i"
		n=$(($n + 1))
	done
	read -ra opcion
	device=${puertos[$opcion]}

else
	device=${puertos[0]}
fi
echo -e "Ardunio Micro encontrado... Dispositivo : $device\n"
echo "Asignando permisos al dispositivo..."
sudo chmod 666 /dev/$device
echo $(ls -l /dev/ttyACM0)
echo -e "Permisos asignados...\n\n"

echo -e "conectando...\n\n" 

function pyserial 
{ 
python3 -c "
import serial
micro = serial.Serial('/dev/$device',9600)
micro.write(b'$1')
"
}


NVIDIA=$(cat ~/.DracoGlacies.conf | awk -F '=' '/nvidia/ {print $2}')
if [ "$NVIDIA" != "" ];
then
	echo -e "Configuracion deteccion de Temperatura de la Tarjeta NVIDIA...\n\n"
fi

function DracoRun
{


pyserial 1210717
echo -e "Control de Ventiladores : Activado...\n\n\n"
sleep 0.5




while [ true ]
do
	clear
	cpuTemp=$(sensors | awk '/Package id / {print $4 }' | awk -F '.' '{print $1}' | awk -F '+' '{print $2}')

	if [ "$NVIDIA" = "si" ];
	then
		gpuTemp=$(nvidia-smi -q | awk '/GPU Current Temp/ {print $5}')
		if [ $cpuTemp -ge $gpuTemp ];
		then
			TempM=$cpuTemp
		else
			TempM=$gpuTemp
		fi
	else
		if [ "$NVIDIA" = "no" ];
		then
			TempM=$cpuTemp
		fi
	fi

	# optener el valor de pwm de 0 a 255 -> 0% a 100%
	if [ $TempM -le 40 ];
	then
		pwm=$((255 * ${tempV[0]} / 100))
	else
		if [ 40 -lt $TempM -a $TempM -le 50 ];
		then
			pwm=$((255 * ${tempV[1]} / 100))
		else
			if [ 50 -lt $TempM -a $TempM -le 60 ];
			then
				pwm=$((255 * ${tempV[2]} / 100))
			else
				if [ 60 -lt $TempM -a $TempM -le 70 ];
				then
					pwm=$((255 * ${tempV[3]} / 100))
				else
					if [ 80 -lt $TempM -a $TempM -le 90 ];
					then
						pwm=$((255 * ${tempV[4]} / 100))
					else
						if [ 90 -lt $TempM -a $TempM -le 100 ];
						then
							pwm=$((255 * ${tempV[5]} / 100))
						else
							pwm=$((255 * ${tempV[6]} / 100))
						fi
					fi
				fi
			fi
		fi
	fi
	if [ "$NVIDIA" = "si" ];
	then
		echo -e "|CPU : $cpuTemp°C \t| GPU : $gpuTemp°C \t| FAN : $((100 * $pwm / 255))% \t| PWM : $pwm \t|"	
	else
		echo -e "|CPU & GPU : $cpuTemp°C \t| FAN : $((100 * $pwm / 255))% \t| PWM : $pwm \t|"
	fi
	
	sleep 1
	pyserial "1-$pwm"

done

}

DracoRun
