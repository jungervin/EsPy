import esp

class FlashBdev:

    SEC_SIZE = 4096
    START_SEC = esp.flash_user_start() // SEC_SIZE
    NUM_BLK = 0x6b

    def __init__(self, blocks=NUM_BLK):
        pass

    def readblocks(self, n, buf):
        pass

    def writeblocks(self, n, buf):
        pass

    def ioctl(self, op, arg):
        pass

def set_bl_flash_size(real_size):
    pass

# If bootloader size ID doesn't correspond to real Flash size,
# fix bootloader value and reboot.
size = esp.flash_id() >> 16
# Check that it looks like realistic power of 2 for flash sizes
# commonly used with esp8266
if 22 >= size >= 18:
    size = 1 << size
    if size != esp.flash_size():
        import machine
        import time
        print("Bootloader Flash size appear to have been set incorrectly, trying to fix")
        set_bl_flash_size(size)
        machine.reset()
        while 1: time.sleep(1)

size = esp.flash_size()
if size < 1024*1024:
    bdev = None
else:
    # 20K at the flash end is reserved for SDK params storage
    bdev = FlashBdev((size - 20480) // FlashBdev.SEC_SIZE - FlashBdev.START_SEC)
