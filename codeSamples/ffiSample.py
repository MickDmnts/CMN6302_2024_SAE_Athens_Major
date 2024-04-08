# Imports the necessary C types
import ctypes 

# Base class definition
class Data(ctypes.Structure):
    _fields_ = [
        ('intValue', ctypes.c_int),
        ('floatValue', ctypes.c_float),
        ('boolValue', ctypes.c_bool)
    ]