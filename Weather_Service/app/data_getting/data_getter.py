# from app import app
from app.data_getting.parsing.pogodaklimat_parser import POGODAKLIMAT_SOURCE_DATA
from app.json_processing.json_processor import *

from datetime import datetime
import requests


def get_request(url):
  return requests.get(url=url)

# def post_request(url, params):
#   return requests.post(url=url, json=params)


def print_start_time(source_name):
  start_time = datetime.now()
  # app.logger.info("Start loading %s: %s" %
  #   (source_name.title(), str(start_time)))
  print("Start loading %s: %s" % (source_name.title(), str(start_time)))
  return start_time


def print_end_time(source_name, start_time):
  end_time = datetime.now()
  # app.logger.info("End loading %s: %s" %
  #   (source_name.title(), str(end_time)))
  print("End loading %s: %s" % (source_name.title(), str(end_time)))
  # app.logger.info("LOADING TIME %s: %s" %
  #   (source_name.title(), str(end_time - start_time)))
  print("LOADING TIME %s: %s" %
        (source_name.upper(), str(end_time - start_time)))


class DATA_GETTER:
  def __init__(self):
    self.data = {}

  def load_data(self):

    # получить данные по API
    def load_data_by_api():
      # данные потребителей, сопоставленные с объектами 0.4
      start_time = print_start_time("data from ###")

      self.data = read_from_json_in_dir(
          filename="data_from_###.json",
          dir="input_data"
      )

      if len(self.data) == 0:
        api_method = ""
        result = get_request(app.config['URL_SOURCE'] + api_method)
        self.data += result.json()
        write_to_json_in_dir(
            filename="data_from_###.json",
            data=self.data,
            dir="input_data"
        )
      print_end_time("data from ###", start_time)

    # получить данные с помощью парсера
    def get_data_from_parser():
      today = datetime.today()
      today_string = "%s.%s.%s" % (
          str(today.day), str(today.month), str(today.year))
      start_year = 2020
      # получить данные из POGDAKLIMAT
      pogodataklimat_parser_obj = POGODAKLIMAT_SOURCE_DATA()
      # получить идентификаторы станций
      pogodataklimat_parser_obj.parse_pogodaklimat_station_ids()
      stations_ids = pogodataklimat_parser_obj.russian_stations_ids
      # получить данные погоды для каждой найденной станции
      pogodataklimat_parser_obj.parse_pogodaklimat_day_data(
          start_year=start_year,
          stations_ids=stations_ids
      )
      write_to_json_in_dir(
          filename="pogodaklimat_day_data_from%s_%s.json" % (
              str(start_year), today_string),
          data=pogodataklimat_parser_obj.data_station_temperature_day,
          dir="result_data"
      )

    # запуск получения данных по API
    # load_data_by_api()

    # запуск получения данных по API
    get_data_from_parser()
