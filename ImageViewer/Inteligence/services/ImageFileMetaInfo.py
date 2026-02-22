from SqliteDataService.SqliteDBService import ImageMetaDB
import pathlib as pl


class ImageFileMetaInfo:
   def __init__(self):
       self.database = ImageMetaDB()
   def Initilize(self):
       self.database._connect()
   def add_metaInfo(self,image_path,meta_info):
       return self.database.insert_image(image_path,pl.Path(image_path).name,meta_info)
   def update_metaInfo(self,id,meta_info):
       self.database.update_json(id,meta_info);
   def get_meta_info(self,image_path):
       return
   def is_processed(self,image_path)-> bool:
       return self.database.exist(image_path)