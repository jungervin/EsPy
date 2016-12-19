# DS18x20 temperature sensor driver for MicroPython.
# MIT license; Copyright (c) 2016 Damien P. George

from micropython import const

_CONVERT = const(0x44)
_RD_SCRATCH = const(0xbe)
_WR_SCRATCH = const(0x4e)

class DS18X20:
    def __init__(self, onewire):
        pass
    
    def scan(self):
        pass

    def convert_temp(self):
        pass

    def read_scratch(self, rom):
        pass

    def write_scratch(self, rom, buf):
        pass

    def read_temp(self, rom):
        pass