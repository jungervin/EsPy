from micropython import const
import _onewire as _ow

class OneWireError(Exception):
    pass

class OneWire:
    SEARCH_ROM = const(0xf0)
    MATCH_ROM = const(0x55)
    SKIP_ROM = const(0xcc)

    def __init__(self, pin):
        pass
    
    def reset(self, required=False):
        pass

    def readbit(self):
        pass

    def readbyte(self):
        pass

    def readinto(self, buf):
        pass

    def writebit(self, value):
        pass

    def writebyte(self, value):
        pass

    def write(self, buf):
        pass

    def select_rom(self, rom):
        pass

    def scan(self):
        pass

    def _search_rom(self, l_rom, diff):
        pass

    def crc8(self, data):
        pass