# EsPy, an ESP8266 MicroPython IDE

### Minimum Requirements:

- MS Windows 7
- MS .NET Framework 4.6

### Features:

- Code editor
- Interactive terminal
- File manager
- esptool

### Installation:

Only unzip the downloaded file.

To run the esptool, you need to install python environment.

**After Python intsallation:**

- python -m pip install --upgarde pip  
- python -m pip install esptool

###Quick Start:

1. On the menubar choose Device => Ports and select one
2. Press the connect button on the toolbar 
3. Press the Soft Reset button on the toolbar
4. Press the New button on the toolbar and create a new py file
5. Type the code and press the Run button on the toolbar. (When the statusbar color is orange, it means the device is busy)

**quick test:**
```python
  from machine import Pin
  import time

  p = Pin(2, Pin.OUT)

  for i in range(5):
    print(i)
    time.sleep_ms(500)
    p.low()
    time.sleep_ms(500)
    p.high()
```

**Good to know:**
- Filemanager is enabled when device is connected and not busy
- Esptool enabled when the port is closed

### Download:
https://github.com/jungervin/EsPy/raw/master/EsPy/EsPy.1.0.0.2.7z


### Images:
![alt text](https://raw.githubusercontent.com/jungervin/EsPy/master/EsPy/Helps/images/espy.png "Main screen")

![alt text](https://raw.githubusercontent.com/jungervin/EsPy/master/EsPy/Helps/images/filemanager.png "File Manager")

![alt text](https://raw.githubusercontent.com/jungervin/EsPy/master/EsPy/Helps/images/esptool.png "esptool")
