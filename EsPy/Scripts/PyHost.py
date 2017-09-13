import socket

TCP_IP = '127.0.0.1'
TCP_PORT = 5005

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.bind((TCP_IP, TCP_PORT))
s.listen(1)
conn, addr = s.accept()
print 'Connection address:', addr

from struct import *
import json
import jedi
import sys
import os

jedi.settings.no_completion_duplicates = True
jedi.settings.use_filesystem_cache = True

print(os.path.dirname(os.path.realpath(__file__)))
lib = os.path.dirname(os.path.realpath(__file__)) + '\..\Lib'
sys.path = []
sys.path.append(lib)
modules = []
modules.append(lib + "\machine.py")
modules.append(lib + "\onewire.py")
modules.append(lib + "\umqtt\simple.py")
#jedi.preload_module(modules)
print(sys.path)
print(modules)

class Proc(object):
    def __init__(self, j):
        self.__dict__ = json.loads(j)
         
def serialize_completions(completions):
    items = []
    params ={}
    try:
        for completion in completions:
            #print(dir(item))
            if completion.module_name == '__builtin__':
                continue
            #print(completion.module_path)

            params = []
            if hasattr(completion, 'params'):
                for p in completion.params:
                    params.append(p.name)

            item = {
                #'column': completion.column,
                'name': completion.name, 
                'complete': completion.complete, 
                #'description': completion.description,
                #'doc': completion.doc,
                #'docstring': completion.docstring(False), 
                ## #'follow_definition': item.follow_definition(), 
                #'full_name': completion.full_name, 
                ## #'goto_assignments': item.goto_assignments(), 
                ## 'in_builtin_module': completion.in_builtin_module, 
                'is_keyword': completion.is_keyword, 
                ##'line': completion.line, 
                #'module_name': completion.module_name, 
                #'module_path': completion.module_path, 
                #'name_with_symbols': completion.name_with_symbols, 
                ##'parameters': params, #completion.params, 
                ##'parent': completion.parent, 
                ##'raw_doc': completion.raw_doc, 
                ##'start_pos': completion.start_pos,
                #'type': completion.type
            }
            items.append(item)
        return json.dumps({'completions': items})
    except Exception as e:
        print("EXCEPTION: ", e)
        return "ERR"


while 1:
    data = conn.recv(4)
    dlen = unpack('i', data)
    data = ""

    while len(data) < dlen[0]:
        data += conn.recv(dlen[0])

    obj = Proc(data)           
    result = 'No suggestion';
    print "MODULE: " + obj.Module
    print "METHOD: " + obj.Method
  
    if obj.Module == 'jedi':
        if obj.Method == 'completion':
                script = jedi.Script(obj.Script['Source'], obj.Script['Line'], obj.Script['Column']) 
                comp = script.completions()
                print(comp)
                result = serialize_completions(comp)
        elif obj.Method == "hello":
            result = json.dumps({"respose": "hello"})
        elif obj.Method == "bye":
            print("bye")
            conn.close()
            #input("Press Enter to continue...")
            sys.exit()
            #result = json.dumps({"respose": "bye"})
            break

    # response
    dlen = len(result)
    conn.send(pack('i', dlen))
    n = conn.send(result)
    result = ""
    print("LEN: ", dlen, "SENT: ", n)

conn.close()
#input("Press Enter to continue...")
sys.exit()
