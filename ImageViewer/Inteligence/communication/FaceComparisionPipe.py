from .BasePipeCommunication import BasePipeCommunication
class FaceComparisionPipe(BasePipeCommunication):
    def __init__(self):
        pass
    def connect(self,pipe_name):
        self.create_pipe(pipe_name)
        try:
            self.wait_for_client()
            print("Client connected to pipe")
        except Exception as ex:
            print("Failed to connect to client")
            pass