import cv2
import dlib
import uuid
from pathlib import Path
import os
from types import NoneType
from typing import Any
from services import ImageFileMetaInfo



class GetFaces:
    def __init__(self):
      self.face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')

    def detect_faces(self, imagePath)-> list[str]:
        image = cv2.imread(imagePath)
        gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        detector = dlib.get_frontal_face_detector()
        faces = detector(gray)
        faceFiles:list[Any] =list();
        for i, face in enumerate(faces):
            try:
                x1 = face.left()
                y1 = face.top()
                x2 = face.right()
                y2 = face.bottom()
                # Ensure coordinates are valid
                x1 = max(0, x1)
                y1 = max(0, y1)

                cropped_face = image[y1:y2, x1:x2]
                faceFiles.append(cropped_face)
            except Exception as e:
                print("Failed for : "+imagePath)
                continue;
        return faceFiles
    def get_faces(self, image_path,out_put_folder:str):
        savedFiles : list[str] = list()
        try:

            faces = self.detect_faces(image_path)
            if faces is not NoneType and len(faces) > 0:
                for i, face in enumerate(faces):
                    if(len(out_put_folder) != 0):
                        pat = f"{out_put_folder}/{Path(image_path).stem}_{uuid.uuid4()}.jpg"
                        savedFiles.append(pat)
                        cv2.imwrite(pat, face)
        except Exception as e:
            print("Error processing image: " + str(e))
            pass
        return savedFiles;
    def find_face_indepth(self,dir_path,outPutFolder:str):
        image_extensions = (
            ".jpg", ".jpeg", ".png",
            ".bmp", ".gif", ".tiff",
            ".webp"
        )
        imgFileMetaInfo = ImageFileMetaInfo.ImageFileMetaInfo()
        imgFileMetaInfo.Initilize()
        savedFiles : list[str] = list()
        skip = {"node_module", "faces",".thumbnails_1"}
     
        image_files = []
        for root, dirs, files in os.walk(dir_path):
            dirs[:] = [d for d in dirs if d not in skip]
            for file in files:
                if file.lower().endswith(image_extensions):
                    image_files.append(os.path.join(root, file))
        for file in image_files:
            try:
                if imgFileMetaInfo.is_processed(file):
                    continue
                facesList = self.face_cascade.detect_faces(file);
                if facesList is not NoneType and len(facesList) > 0:
                    imgFileMetaInfo.add_metaInfo(file,{"faces":facesList})
            except Exception as e:
                continue;





