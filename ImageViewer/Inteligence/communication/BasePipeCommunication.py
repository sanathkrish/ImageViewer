from typing import Any
import win32pipe
import win32file
import json



class BasePipeCommunication:
    def __init__(self):
        self.is_connected = True
        pass
    def create_pipe(self, pipe_name):
        self.pipe = win32file.CreateFile(
            pipe_name,
            win32file.GENERIC_READ | win32file.GENERIC_WRITE,
            0,
            None,
            win32file.OPEN_EXISTING,
            0,
            None
        )
        # self.pipe = win32pipe.CreateNamedPipe(
        #             pipe_name,
        #             win32pipe.PIPE_ACCESS_DUPLEX,
        #             win32pipe.PIPE_TYPE_MESSAGE |
        #             win32pipe.PIPE_WAIT,
        #             1,
        #             65536,
        #             65536,
        #             0,
        #             None
        #         )
    def wait_for_client(self):
        result,pingBytes = win32file.win32file.ReadFile(self.pipe,65536)
        pingData =  pingBytes.decode('utf-8')
        if( pingData.strip() == 'ping'):
            win32file.win32file.WriteFile(self.pipe,b"pong")
            win32file.win32file.FlushFileBuffers(self.pipe)
            print('Pinged and ponged')
    def send_message(self,data:Any):
        win32file.WriteFile(self.pipe, json.dumps(data.__dict__).encode('utf-8'));
    def read_message(self):
        result, data = win32file.ReadFile(self.pipe, 65536)
        if result == 0:
            return data.decode('utf-8')
        return None;


