def delay(ms):
    """
    Delay for the given number of milliseconds.
    """
    pass


def udelay(us):
    """
    Delay for the given number of microseconds.
    """
    pass


def millis():
    """
    Returns the number of milliseconds since the board was last reset.
    """
    pass


def micros():
    """
    Returns the number of microseconds since the board was last reset.
    """
    pass


def elapsed_millis(start):
    """
    Returns the number of milliseconds which have elapsed since ``start``.
    """
    pass


def elapsed_micros(start):
    """
    Returns the number of microseconds which have elapsed since ``start``.
    """
    pass


def hard_reset():
    """
    Resets the pyboard in a manner similar to pushing the external RESET
    button.
    """


def bootloader():
    """
    Activate the bootloader without BOOT\* pins.
    """
    pass


def disable_irq():
    """
    Disable interrupt requests.
    """
    pass


def enable_irq(state=True):
    """
    Enable interrupt requests.
    """
    pass


def freq(sysclk, hclk, pclk1, pclk2):
    """
    If given no arguments, returns a tuple of clock frequencies.
    """
    pass


def wfi():
    """
    Wait for an internal or external interrupt.
    """
    pass


def stop():
    """
    Put the pyboard in a "sleeping" state.
    """


def standby():
    """
    Put the pyboard into a "deep sleep" state.
    """
    pass


def info(dump_alloc_table):
    """
    Print out lots of information about the board.
    """
    pass


def main(filename):
    """
    Set the filename of the main script to run after boot.py is finished.
    """
    pass


def mount(device, mountpoint, *, readonly=False, mkfs=False):
    """
    Mount a block device and make it available as part of the filesystem.
    """
    pass


def repl_uart(uart):
    """
    Get or set the UART object where the REPL is repeated on.
    """
    pass


def rng():
    """
    Return a 30-bit hardware generated random number.
    """
    pass


def sync():
    """
    Sync all file systems.
    """
    pass


def unique_id():
    """
    Returns a string of 12 bytes (96 bits), which is the unique ID of the MCU.
    """
    pass


class Accel:

    def filtered_xyz(self):
        """
        Get a 3-tuple of filtered x, y and z values.
        """
        pass

    def tilt(self):
        """
        Get the tilt register.
        """
        pass

    def x(self):
        """
        Get the x-axis value.
        """
        pass

    def y(self):
        """
        Get the y-axis value.
        """
        pass

    def z(self):
        """
        Get the z-axis value.
        """
        pass

    def write(self, register, value):
        pass

    def read(self, register):
        pass


class ADC:

    def __init__(self, pin):
        """
        Create an ADC object associated with the given pin.
        This allows you to then read analog values on that pin.
        """
        pass

    def read(self):
        """
        Read the value on the analog pin and return it.  The returned value
        will be between 0 and 4095.
        """
        pass

    def read_timed(buf, timer):
        """
        Read analog values into ``buf`` at a rate set by the ``timer`` object.
        """
        pass

class CAN:

    NORMAL = "NORMAL"
    LOOPBACK = "LOOPBACK"
    SILENT = "SILENT"
    SILENT_LOOPBACK = "SILENT_LOOPBACK"
    
    LIST16 = "LIST16"
    MASK16 = "MASK16"
    LIST32 = "LIST32"
    MASK32 = "MASK32"
    
    def __init__(self, bus, mode=None, extframe=False, prescaler=100, *, sjw=1, bs1=6, bs2=8):
        """
        Construct a CAN object on the given bus.
        """
        pass

    @classmethod
    def initfilterbanks(cls, nr):
        """
        Reset and disable all filter banks and assign how many banks should be available for CAN(1).
        """
        pass

    def init(self, mode, extframe=False, prescaler=100, *, sjw=1, bs1=6, bs2=8):
        """
        Initialise the CAN bus with the given parameters
        """
        pass

    def deinit(self):
        """
        Turn off the CAN bus.
        """
        pass

    def setfilter(self, bank, mode, fifo, params, *, rtr):
        """
        Configure a filter bank
        """
        pass


class DAC:

    NORMAL = "NORMAL"
    CIRCULAR = "CIRCULAR"
    
    def __init__(self, port, bits=8):
        """
        Construct a new DAC object.
        """
        pass

    def init(self, bits=8):
        """
        Reinitialise the DAC.  ``bits`` can be 8 or 12.
        """
        pass

    def noise(self, freq):
        """
        Generate a pseudo-random noise signal.
        """
        pass

    def triangle(self, freq):
        """
        Generate a triangle wave.
        """
        pass

    def write(self, value):
        """
        Direct access to the DAC output.
        """
        pass

    def write_timed(self, data, freq, *, mode=DAC.NORMAL):
        """
        Initiates a burst of RAM to DAC using a DMA transfer.
        """
        pass


class ExtInt:

    IRQ_FALLING = "IRQ_FALLING"
    IRQ_RISING = "IRQ_RISING"
    IRQ_RISING_FALLING = "IRQ_RISING_FALLING"

    def __init__(self, pin, mode, pull, callback):
        """
        Create an ExtInt object
        """
        pass

    @classmethod
    def regs(cls):
        """
        Dump the values of the EXTI registers.
        """

    def disable(self, ):
        """
        Disable the interrupt associated with the ExtInt object.
        This could be useful for debouncing.
        """
        pass

    def enable(self, ):
        """
        Enable a disabled interrupt.
        """
        pass

    def line(self, ):
        """
        Return the line number that the pin is mapped to.
        """
        pass

    def swint(self, ):
        """
        Trigger the callback from software.
        """
        pass


class I2C:

    MASTER = "MASTER"
    SLAVE = "SLAVE"

    def __init__(self, bus):
        """
        Construct an I2C object on the given bus.
        """
        pass


    def deinit(self):
        """
        Turn off the I2C bus.
        """
        pass

    def init(self, mode, *, addr=0x12, baudrate=400000, gencall=False):
        """
        Initialise the I2C bus with the given parameters.
        """
        pass

    def is_ready(self, addr):
        """
        Check if an I2C device responds to the given address.  Only valid when in master mode.
        """
        pass

    def mem_read(self, data, addr, memaddr, *, timeout=5000, addr_size=8):
        """
        Read from the memory of an I2C device.
        """
        pass

    def mem_write(self, data, addr, memaddr, *, timeout=5000, addr_size=8):
        """
        Write to the memory of an I2C device.
        """
        pass

    def recv(self, recv, addr=0x00, *, timeout=5000):
        """
        Receive data on the bus.
        """
        pass

    def send(self, send, addr=0x00, *, timeout=5000):
        """
        Send data on the bus.
        """
        pass

    def scan(self):
        """
        Scan all I2C addresses from 0x01 to 0x7f and return a list of those that respond.
        """
        pass

class LCD:

    def __init__(self, skin_position):
        """
        Construct an LCD object in the given skin position.  ``skin_position`` can be 'X' or 'Y', and
        should match the position where the LCD pyskin is plugged in.
        """
        pass

    def command(self, instr_data, buf):
        """
        Send an arbitrary command to the LCD.  Pass 0 for ``instr_data`` to send an
        instruction, otherwise pass 1 to send data.  ``buf`` is a buffer with the
        instructions/data to send.
        """

    def contrast(self, value):
        """
        Set the contrast of the LCD.  Valid values are between 0 and 47.
        """
        pass

    def fill(self, colour):
        """
        Fill the screen with the given colour (0 or 1 for white or black).
        """
        pass

    def get(self, x, y):
        """
        Get the pixel at the position ``(x, y)``.  Returns 0 or 1.
        """
        pass

    def light(self, value):
        """
        Turn the backlight on/off.  True or 1 turns it on, False or 0 turns it off.
        """
        pass

    def pixel(self, x, y, colour):
        """
        Set the pixel at ``(x, y)`` to the given colour (0 or 1).
        """
        pass

    def show(self, ):
        """
        Show the hidden buffer on the screen.
        """
        pass

    def text(self, str, x, y, colour):
        """
        Draw the given text to the position ``(x, y)`` using the given colour (0 or 1).
        """
        pass

    def write(self, str):
        """
        Write the string ``str`` to the screen.  It will appear immediately.
        """
        pass


class LED:
    def __init__(self, id):
        """
        Create an LED object associated with the given LED
        """
        pass

    def intensity(self, value):
        """
        Get or set the LED intensity.  Intensity ranges between 0 (off) and 255 (full on).
        """

    def off(self, ):
        """
        Turn the LED off.
        """
        pass

    def on(self, ):
        """
        Turn the LED on, to maximum intensity.
        """
        pass

    def toggle(self, ):
        """
        Toggle the LED between on (maximum intensity) and off.
        """
        pass


class Pin:

    AF_OD = "AF_OD"
    AF_PP = "AF_PP"
    ANALOG = "ANALOG"
    IN = "IN"
    OUT_OD = "OUT_OD"
    OUT_PP = "OUT_PP"
    PULL_DOWN = "PULL_DOWN"
    PULL_NONE = "PULL_NONE"
    PULL_UP = "PULL_UP"

    def __init__(self, id):
        """
        Create a new Pin object associated with the id.
        """
        pass

    @classmethod
    def af_list(cls, ):
        """
        Returns an array of alternate functions available for this pin.
        """
        pass

    @classmethod
    def debug(cls, state):
        """
        Get or set the debugging state (``True`` or ``False`` for on or off).
        """
        pass

    @classmethod
    def dict(cls, dict):
        """
        Get or set the pin mapper dictionary.
        """
        pass

    @classmethod
    def mapper(cls, fun):
        """
        Get or set the pin mapper function.
        """
        pass

    def init(self, mode, pull=Pin.PULL_NONE, af=-1):
        """
        Initialise the pin:
        """
        pass

    def value(self, value):
        """
        Get or set the digital logic level of the pin.
        """
        pass

    def __str__(self):
        """
        Return a string describing the pin object.
        """
        pass

    def af(self):
        """
        Returns the currently configured alternate-function of the pin.
        """
        pass

    def gpio(self):
        """
        Returns the base address of the GPIO block associated with this pin.
        """
        pass

    def mode(self):
        """
        Returns the currently configured mode of the pin.
        """
        pass

    def name(self):
        """
        Get the pin name.
        """
        pass

    def names(self):
        """
        Returns the cpu and board names for this pin.
        """
        pass

    def pin(self):
        """
        Get the pin number.
        """
        pass

    def port(self):
        """
        Get the pin port.
        """
        pass

    def pull(self):
        """
        Returns the currently configured pull of the pin.
        """
        pass

class PinAF:

    def __str__(self):
        """
        Return a string describing the alternate function.
        """
        pass

    def index(self):
        """
        Return the alternate function index.
        """
        pass

    def name(self):
        """
        Return the name of the alternate function.
        """
        pass

    def reg(self):
        """
        Return the base register associated with the peripheral assigned to this
        alternate function.
        """
        pass


class RTC:

    def __init__(self):
        """
        Create an RTC object.
        """
        pass

    def datetime(self, datetimetuple):
        """
        Get or set the date and time of the RTC.
        """
        pass

    def wakeup(self, timeout, callback=None):
        """
        Set the RTC wakeup timer to trigger repeatedly at every ``timeout``
        milliseconds.
        """
        pass

    def info(self):
        """
        Get information about the startup time and reset source.
        """
        pass

    def calibration(self, cal):
        """
        Get or set RTC calibration.
        """
        pass

class Servo:

    def __init__(self, id):
        """
        Create a servo object.  ``id`` is 1-4, and corresponds to pins X1 through X4.
        """
        pass

    def angle(self, angle, time=0):
        """
        If no arguments are given, this function returns the current angle.
        """
        pass

    def speed(self, speed, time=0):
        """
        If no arguments are given, this function returns the current speed.
        """
        pass

    def pulse_width(self, value):
        """
        If no arguments are given, this function returns the current raw pulse-width
        value.
        """
        pass

    def calibration(self, pulse_min, pulse_max, pulse_centre, pulse_angle_90, pulse_speed_100):
        """
        If no arguments are given, this function returns the current calibration
        data, as a 5-tuple.
        """
        pass


class SPI:

    MASTER = "MASTER"
    SLAVE = "SLAVE"
    LSB = "LSB"
    MSB = "MSB"

    def __init__(self, bus):
        """
        Construct an SPI object on the given bus.
        """
        pass

    def deinit(self):
        """
        Turn off the SPI bus.
        """
        pass

    def init(self, mode, baudrate=328125, *, prescaler, polarity=1, phase=0, bits=8, firstbit=SPI.MSB, ti=False, crc=None):
        """
        Initialise the SPI bus with the given parameters:
        """
        pass

    def recv(self, recv, *, timeout=5000):
        """
        Receive data on the bus:
        """
        pass

    def send(self, send, *, timeout=5000):
        """
        Send data on the bus:
        """
        pass

    def send_recv(self, send, recv=None, *, timeout=5000):
        """
        Send and receive data on the bus at the same time:
        """
        pass


class Switch:

    def __init__(self):
        """
        Create and return a switch object.
        """
        pass

    def switch(self):
        """
        Return the switch state: ``True`` if pressed down, ``False`` otherwise.
        """
        pass

    def callback(self, fun):
        """
        Register the given function to be called when the switch is pressed down.
        """
        pass

class Timer:

    def __init__(self, id):
        """
        Construct a new timer object of the given id.
        """
        pass

    def init(self, *, freq, prescaler, period):
        """
        Initialise the timer.
        """
        pass

    def deinit(self):
        """
        Deinitialises the timer.
        """
        pass

    def callback(self, fun):
        """
        Set the function to be called when the timer triggers.
        """
        pass

    def channel(self, channel, mode):
        """
        If only a channel number is passed, then a previously initialized channel
        object is returned (or ``None`` if there is no previous channel).
        """
        pass

    def counter(self, value):
        """
        Get or set the timer counter.
        """

    def freq(self, value):
        """
        Get or set the frequency for the timer (changes prescaler and period if set).
        """
        pass

    def period(self, value):
        """
        Get or set the period of the timer.
        """
        pass

    def prescaler(self, value):
        """
        Get or set the prescaler for the timer.
        """
        pass

    def source_freq(self):
        """
        Get the frequency of the source of the timer.
        """
        pass


class TimerChannel:

    def callback(self, fun):
        """
        Set the function to be called when the timer channel triggers.
        """
        pass

    def capture(self, value):
        """
        Get or set the capture value associated with a channel.
        """
        pass

    def compare(self, value):
        """
        Get or set the compare value associated with a channel.
        """
        pass

    def pulse_width(self, value):
        """
        Get or set the pulse width value associated with a channel.
        """

    def pulse_width_percent(self, value):
        """
        Get or set the pulse width percentage associated with a channel.
        """
        pass


class UART:

    RTS = "RTS"
    CTS = "CTS"

    def __init__(self, bus):
        """
        Construct a UART object on the given bus.
        """
        pass

    def init(self, baudrate, bits=8, parity=None, stop=1, \*, timeout=1000, flow=None, timeout_char=0, read_buf_len=64):
        """
        Initialise the UART bus with the given parameters:
        """
        pass

    def deinit(self):
        """
        Turn off the UART bus.
        """
        pass

    def any(self):
        """
        Return ``True`` if any characters waiting, else ``False``.
        """
        pass

    def writechar(self, char):
        """
        Write a single character on the bus.
        """
        pass

    def read(self, nbytes):
        """
        Read characters.
        """
        pass

    def readall(self):
        """
        Read as much data as possible.
        """
        pass

    def readchar(self):
        """
        Receive a single character on the bus.
        """
        pass

    def readinto(self, buf, nbytes):
        """
        Read bytes into the ``buf``.
        """
        pass

    def readline(self):
        """
        Read a line, ending in a newline character.
        """
        pass

    def write(self, buf):
        """
        Write the buffer of bytes to the bus.
        """
        pass

    def sendbreak(self):
        """
        Send a break condition on the bus.
        """
        pass


class USB_VCP:

    def __init__(self):
        """
        Create a new USB_VCP object.
        """
        pass

    def setinterrupt(self, chr):
        """
        Set the character which interrupts running Python code.
        """
        pass

    def isconnected(self):
        """
        Return ``True`` if USB is connected as a serial device, else ``False``.
        """
        pass

    def any(self):
        """
        Return ``True`` if any characters waiting, else ``False``.
        """
        pass

    def close(self):
        """
        This method does nothing. It exists so the USB_VCP object can act as a file.
        """
        pass

    def read(self, nbytes):
        """
        Read at most ``nbytes`` from the serial device and return them as a bytes object.
        """
        pass

    def readall(self):
        """
        Read all available bytes from the serial device and return them as
        a bytes object, or ``None`` if no pending data available.
        """
        pass

    def readinto(self, buf, maxlen):
        """
        Read bytes from the serial device and store them into ``buf``, which
        should be a buffer-like object.
        """
        pass

    def readline(self):
        """
        Read a whole line from the serial device.
        """
        pass

    def readlines(self):
        """
        Read as much data as possible from the serial device, breaking it into lines.
        """
        pass

    def write(self, buf):
        """
        Write the bytes from ``buf`` to the serial device.
        """
        pass

    def recv(self, data, *, timeout=5000):
        """
        Receive data on the bus.
        """
        pass

    def send(self, data, *, timeout=5000):
        """
        Send data over the USB VCP.
        """
        pass
