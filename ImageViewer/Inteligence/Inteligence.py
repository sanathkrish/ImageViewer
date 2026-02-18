
import cv2
import dlib
import uuid
from PIL import Image
from pathlib import Path
import os


class Inteligence:
    def __init__(self):
        self.face_cascade = cv2.CascadeClassifier(cv2.data.haarcascades + 'haarcascade_frontalface_default.xml')
    def detect_faces(self, imagePath):
        image = cv2.imread(imagePath)
        gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
        detector = dlib.get_frontal_face_detector()
        faces = detector(gray)
        print(len(faces))
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
                # cv2.imshow("Faces", cropped_face)
                # cv2.waitKey(0)
                pat = f"F:\\faces\\{Path(imagePath).stem}_{uuid.uuid4()}.jpg"
                print(pat)
                cv2.imwrite(pat, cropped_face)
            except:
                print("Failed for : "+imagePath)

# istance = Inteligence()
# folder = Path(f"f:\\")
# skip = {"node_module"}
# image_extensions = (
#         ".jpg", ".jpeg", ".png",
#         ".bmp", ".gif", ".tiff",
#         ".webp"
#     )
# image_files = []
# for root, dirs, files in os.walk(folder):
#     dirs[:] = [d for d in dirs if d not in skip]
#     for file in files:
#         if file.lower().endswith(image_extensions):
#             image_files.append(os.path.join(root, file))
# print(len(image_files))
# for file in image_files:
#     istance.detect_faces(file);

import SqliteDataService.SqliteDBService
if __name__ == "__main__":
    sqlite = SqliteDataService.SqliteDBService.ImageMetaDB();
    sqlite._connect();

