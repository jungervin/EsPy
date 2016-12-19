import socket
from struct import *
import json
import jedi
import sys
import os

jedi.settings.no_completion_duplicates = True
jedi.settings.use_filesystem_cache = True

print(os.path.dirname(os.path.realpath(__file__)))
lib = os.path.dirname(os.path.realpath(__file__)) + '\\..\\' + 'Lib'
sys.path.append(lib)
modules = []
modules.append(lib + "\\machine.py")
modules.append(lib + "\\onewire.py")
jedi.preload_module(modules)

print(sys.path)

class Proc(object):
    def __init__(self, j):
        self.__dict__ = json.loads(j)
         
TCP_IP = '127.0.0.1'
TCP_PORT = 5005

def serialize_completions(completions):
    items = []
    params ={}
    try:
        for completion in completions:
            #print(dir(item))
            #if completion.module_name == '__builtin__':
            #    continue
            #print(completion.module_path)

            params = []
            if hasattr(completion, 'params'):
                for p in completion.params:
                    params.append(p.name)
            item = {
                #'column': completion.column,
                'complete': completion.complete, 
                'description': completion.description, 
                'doc': completion.doc,
                'docstring': completion.docstring(False), 
                # #'follow_definition': item.follow_definition(), 
                 'full_name': completion.full_name, 
                # #'goto_assignments': item.goto_assignments(), 
                # 'in_builtin_module': completion.in_builtin_module, 
                'is_keyword': completion.is_keyword, 
                #'line': completion.line, 
                'module_name': completion.module_name, 
                'module_path': completion.module_path, 
                'name': completion.name, 
                'name_with_symbols': completion.name_with_symbols, 
                'parameters': params, #completion.params, 
                #'parent': completion.parent, 
                #'raw_doc': completion.raw_doc, 
                #'start_pos': completion.start_pos,
                'type': completion.type
            }
            items.append(item)
        return json.dumps({'completions': items})
    except Exception as e:
        print("EXCEPTION: ", e)
        pass

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((TCP_IP, TCP_PORT))
s.listen(1)
conn, addr = s.accept()
print 'Connection address:', addr

while 1:
    data = conn.recv(4)
    dlen = unpack('i', data)
    data = conn.recv(dlen[0])
    
    obj = Proc(data)
    result = 'No suggestion';
    print "MODULE: " + obj.Module
    print "METHOD: " + obj.Method
  
    if obj.Module == 'jedi':
        if obj.Method == 'completion':
            script = jedi.Script(obj.Script['Source'], obj.Script['Line'], obj.Script['Column']) 
            result = serialize_completions(script.completions())
        elif obj.Method == "hello":
            result = json.dumps({"respose": "hello"})
        elif obj.Method == "bye":
            #result = json.dumps({"respose": "bye"})
            break

    # response
    dlen = len(result)
    conn.send(pack('i', dlen))
    n = conn.send(result)
    result = ""
    print("LEN: ", dlen, "SENT: ", n)

conn.close()
sys.exit()
