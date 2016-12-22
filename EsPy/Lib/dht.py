# DHT11/DHT22 driver for MicroPython on ESP8266
# MIT license; Copyright (c) 2016 Damien P. George

import esp

class DHTBase:
    def __init__(self, pin):
        pass

    def measure(self):
        pass

class DHT11(DHTBase):
    def humidity(self):
        pass

    def temperature(self):
        pass

class DHT22(DHTBase):
    def humidity(self):
        pass

    def temperature(self):
        pass