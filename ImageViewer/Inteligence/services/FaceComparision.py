import deepface
class FaceComparision:
    def __init__(self):
        self.deepFace = deepface.DeepFace
    def compare_face(self, image1_path, image2_path):
        return self.deepFace.verify(image1_path, image2_path)
        # self.face_recognition = face_recognition
    # def compare_faces(self, image1_path, image2_path):
    #     # Load the images and encode the faces
    #     image1 = self.face_recognition.load_image_file(image1_path)
    #     image2 = self.face_recognition.load_image_file(image2_path)
    #     encodings1 = self.face_recognition.face_encodings(image1)
    #     encodings2 = self.face_recognition.face_encodings(image2)
    #     if not encodings1 or not encodings2:
    #         raise ValueError("No faces found in one of the images.")
    #     # Compare the first face found in each image
    #     result = self.face_recognition.compare_faces([encodings1[0]], encodings2[0])
    #     return result[0]