#!/bin/bash

#Guardamos el archivo de configuracion en la carpeta del usuario
echo -e "Copiando Configuracion..."
cp 'Script Linux'/.DracoGlacies.conf ~/.DracoGlacies.conf
echo -e "Usted posee una Tarjeta de Video NVIDIA?\n Opciones Y/N (default N) : "
read -ra nvidia
if [ "$(echo $nvidia | grep -i 'Y')" = "" ];
then
    sed -i '2s/si/no/' ~/.DracoGlacies.conf
else
    NVIDIA=$(nvidia-smi -q | awk '/GPU Current Temp/ {print $5}')
    if [ "$NVIDIA" = "" ];
    then
        echo -e "Error: No se encuentra la temperatura de la tarjeta de video NVIDIA...\n"
        echo -e "Verifique que tenga los driver de NVIDIA instalados..."
        exit
    else
        echo -e "Temperatura de la Tarjeta de Video NVIDIA Encontrada Exitosamente...\n"
    fi
fi

echo -e "Copiando Script a /bin/DracoGlacies...\n"
sudo cp 'Script Linux'/DracoGlacies.sh /bin/DracoGlacies

serial_lib=$(pip list | awk '{print $1}' | grep -w 'pyserial')
if [ "$serial_lib" = "" ];
then
    echo -e "Instalando libreria de Python <PySerial>\n"
    pip install pyserial
    if [ $? -eq 0 ]; 
    then
       echo -e "\n\nInstalacion exitosa de la Libreria de Python <PySerial> \n"
    else
        echo -e "\n\nError: Compando <pip install pyserial> no funciono..."
    fi
else
    echo -e "\nLibreria Python <PySerial> ya esta instalada... \n"
fi

echo -e "Instalacion Terminada...\n\n Ejecucion del Script :\n\nbash DracoGlacies"
