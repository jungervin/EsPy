# MicroPython SSD1306 OLED driver, I2C and SPI interfaces

from micropython import const
import time
import framebuf


# register definitions
SET_CONTRAST        = const(0x81)
SET_ENTIRE_ON       = const(0xa4)
SET_NORM_INV        = const(0xa6)
SET_DISP            = const(0xae)
SET_MEM_ADDR        = const(0x20)
SET_COL_ADDR        = const(0x21)
SET_PAGE_ADDR       = const(0x22)
SET_DISP_START_LINE = const(0x40)
SET_SEG_REMAP       = const(0xa0)
SET_MUX_RATIO       = const(0xa8)
SET_COM_OUT_DIR     = const(0xc0)
SET_DISP_OFFSET     = const(0xd3)
SET_COM_PIN_CFG     = const(0xda)
SET_DISP_CLK_DIV    = const(0xd5)
SET_PRECHARGE       = const(0xd9)
SET_VCOM_DESEL      = const(0xdb)
SET_CHARGE_PUMP     = const(0x8d)


class SSD1306:
    def __init__(self, width, height, external_vcc):
        pass

    def init_display(self):
        pass

    def poweroff(self):
        pass

    def contrast(self, contrast):
        pass

    def invert(self, invert):
        pass

    def show(self):
        pass

    def fill(self, col):
        pass

    def pixel(self, x, y, col):
        pass

    def scroll(self, dx, dy):
        pass

    def text(self, string, x, y, col=1):
        pass


class SSD1306_I2C(SSD1306):
    def __init__(self, width, height, i2c, addr=0x3c, external_vcc=False):
        pass

    def write_cmd(self, cmd):
        pass

    def write_data(self, buf):
        pass

    def poweron(self):
        pass


class SSD1306_SPI(SSD1306):
    def __init__(self, width, height, spi, dc, res, cs, external_vcc=False):
        pass

    def write_cmd(self, cmd):
        pass

    def write_data(self, buf):
        pass

    def poweron(self):
        pass