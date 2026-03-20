from Models.PipeResonse import PipeResponse
from .BasePipeCommunication import BasePipeCommunication
from services import FaceComparision
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
        except Exception as ex:
            print("Failed to connect to client")
            pass
    def compare_face(self):
        print("Waiting for data from client...")
        while True:
            try:
                response = PipeResponse('pass',None);
                data = self.read_message()
                if len(data)>0:
                    print("Data received from client")
                    json_data = json.loads(data);
                    #image1_path, image2_path = data_str.split(',')
                    comparison_result = self.faceComparision.compare_face(json_data['left_image'], json_data['right_image'])
                    response = PipeResponse('pass',str(comparison_result));
            except Exception as ex:
               print("Error reading from pipe:", ex)
               response = PipeResponse('fail',None);
               self.send_message(response);
               continue;

