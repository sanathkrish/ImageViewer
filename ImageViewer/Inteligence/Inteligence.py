
from types import NoneType
from typing import Any
import cv2
import dlib
import uuid
from PIL import Image
from pathlib import Path
import os
import threading

from services import FaceComparision, ImageFileMetaInfo

import SqliteDataService.SqliteDBService
import communication.FaceComparisionPipe
import communication.FindFaces
import time
stop_event = threading.Event()
def face_compare():
    PIPE_NAME = r'\\.\pipe\image_compare'
    instance = communication.FaceComparisionPipe.FaceComparisionPipe()
    instance.connect(PIPE_NAME)
    if(instance.is_connected == True):
       instance.compare_face()
       print("Done")
def get_faces():
    PIPE_NAME = r'\\.\pipe\image_faces'
    instance = communication.FindFaces.FindFaces()
    instance.connect(PIPE_NAME)
    if(instance.is_connected == True):
       instance.get_face()
       print("Done")
if __name__ == "__main__":
     # threads = [
     #   threading.Thread(target=face_compare, daemon=True),
     #   threading.Thread(target=get_faces, daemon=True),
     # ]
     # for t in threads:
     #        t.start()
     # try:
     #     while True:
     #        time.sleep(1)
     # except KeyboardInterrupt:
     #        stop_event.set()
     get_faces();
   
   # faceComparision = FaceComparision.FaceComparision()
   # result = faceComparision.compare_face("""F:\\faces\KMR_6761_f433eda7-3fac-4b36-9d0c-7906b360e5fd.jpg""","""F:\\faces\KMR_6795_17b56eee-affd-4b7e-9cfd-1e18459f2cb4.jpg""")
   # print(result)
   # 
