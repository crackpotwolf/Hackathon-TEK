from app import app
from requests import status_codes
from app.json_processing.json_processor import *

from datetime import datetime
import requests

from config import *


def print_start_time(source_name):
  start_time = datetime.now()
  app.logger.info("Start loading %s: %s" %
    (source_name.title(), str(start_time)))
  print("Start loading %s: %s" % (source_name.title(), str(start_time)))
  return start_time


def print_end_time(source_name, start_time):
  end_time = datetime.now()
  app.logger.info("End loading %s: %s" %
    (source_name.title(), str(end_time)))
  print("End loading %s: %s" % (source_name.title(), str(end_time)))
  app.logger.info("LOADING TIME %s: %s" %
    (source_name.title(), str(end_time - start_time)))
  print("LOADING TIME %s: %s" %
        (source_name.upper(), str(end_time - start_time)))


class DATA_GETTER:
  def __init__(self):
    self.data = {}

  def load_data(self):
    api_method = ""
    start_time = print_start_time("weather data from DataBase")
    r = requests.post(
        url=app.config["URL_DB_SERVICE"] + api_method
    )
    
    self.data = r.json() if r.status_code == 200 else None
    print_end_time("weather data from DataBase", start_time)
