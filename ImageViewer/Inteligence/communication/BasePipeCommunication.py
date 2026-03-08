import win32pipe
import win32file

class BasePipeCommunication:
    def __init__(self):
        pass
    def create_pipe(self, pipe_name):
        self.pipe = win32pipe.CreateNamedPipe(
                    pipe_name,
                    win32pipe.PIPE_ACCESS_DUPLEX,
                    win32pipe.PIPE_TYPE_MESSAGE |
                    win32pipe.PIPE_READMODE_MESSAGE |
                    win32pipe.PIPE_WAIT,
                    1,
                    65536,
                    65536,
                    0,
                    None
                )
    def wait_for_client(self):
        win32pipe.ConnectNamedPipe(self.pipe, None);
        result,pingBytes = win32file.ReadFile(self.pipe,65536)
        pingData =  pingBytes.decode('utf-8')
        if( pingData.strip() == 'ping'):
            win32file.WriteFile(self.pipe,b"pong")
            print('Pinged and ponged')
