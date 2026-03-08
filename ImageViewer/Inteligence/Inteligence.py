
from types import NoneType
from typing import Any
import cv2
import dlib
import uuid
from PIL import Image
from pathlib import Path
import os

from services import FaceComparision, ImageFileMetaInfo



import SqliteDataService.SqliteDBService
if __name__ == "__main__":
    import communication.FaceComparisionPipe
    PIPE_NAME = r'\\.\pipe\facepipe'
    instance = communication.FaceComparisionPipe.FaceComparisionPipe()
    instance.connect(PIPE_NAME)
    if(instance.is_connected == True):
        instance.compare_face()
   # faceComparision = FaceComparision.FaceComparision()
   # result = faceComparision.compare_face("""F:\\faces\KMR_6761_f433eda7-3fac-4b36-9d0c-7906b360e5fd.jpg""","""F:\\faces\KMR_6795_17b56eee-affd-4b7e-9cfd-1e18459f2cb4.jpg""")
   # print(result)
   # print("Done")
