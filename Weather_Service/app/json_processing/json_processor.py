# from app import app
from config import *

import json
import os


def write_to_json_in_dir(filename, data, dir):
  # path_to_save = os.path.join(app.config['PATH_TO_DATA'], dir)
  path_to_save = os.path.join(PATH_TO_DATA, dir)
  if not os.path.exists(path_to_save):
    # создаем папку и имя файла
    os.makedirs(path_to_save)
  filepath = os.path.join(path_to_save, filename)
  with open(filepath, "w", encoding="utf-8") as filejson:
    json.dump(data, filejson, ensure_ascii=False)


def read_from_json_in_dir(filename, dir):
  # path_to_read = os.path.join(app.config['PATH_TO_DATA'], dir)
  path_to_read = os.path.join(PATH_TO_DATA, dir)
  if not os.path.exists(path_to_read):
    return []
  else:
    path_to_read = os.path.join(path_to_read, filename)
    if not os.path.exists(path_to_read):
      return []
    else:
      with open(path_to_read, "r", encoding="utf-8") as filejson:
        return json.load(filejson)
