# NeoPixel driver for MicroPython on ESP8266
# MIT license; Copyright (c) 2016 Damien P. George

from esp import neopixel_write


class NeoPixel:
    ORDER = (1, 0, 2, 3)

    def __init__(self, pin, n, bpp=3):
        pass

    def __setitem__(self, index, val):
        pass

    def __getitem__(self, index):
        pass

    def fill(self, color):
        pass

    def write(self):
        pass
