from .BasePipeCommunication import BasePipeCommunication
from services import FaceComparision
from win32file import ReadFile, WriteFile
from time import sleep

class FaceComparisionPipe(BasePipeCommunication):
    def __init__(self):
        self.is_connected = False
        self.faceComparision = FaceComparision.FaceComparision()
        pass
    def connect(self,pipe_name):
        self.create_pipe(pipe_name)
        try:
            self.wait_for_client()
            self.is_connected = True
            print("Client connected to pipe")

        except Exception as ex:
            print("Failed to connect to client")
            pass
    def compare_face(self):
        print("Waiting for data from client...")
        while True:
            try:
                sleep(10)
                result, data = ReadFile(self.pipe, 65536)
                if result == 0:
                    print("Data received from client")
                    data_str = data.decode('utf-8')
                    image1_path, image2_path = data_str.split(',')
                    comparison_result = self.faceComparision.compare_face(image1_path, image2_path)
                    WriteFile(self.pipe, str(comparison_result).encode('utf-8'))
            except Exception as ex:
                print("Error reading from pipe:", ex)
                continue
