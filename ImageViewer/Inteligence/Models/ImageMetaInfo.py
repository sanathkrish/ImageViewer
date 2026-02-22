from typing import Any


class ImageMetaInfo:
    def __init__(self,id,image_path,name,Json:dict[str,Any]):
        self.id = id
        self.image_path  = image_path
        self.name = name
        self.json = Json 
