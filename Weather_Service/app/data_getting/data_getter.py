# from app import app
from requests import status_codes
from app.data_getting.parsing.pogodaklimat_parser import POGODAKLIMAT_SOURCE_DATA
from app.data_getting.data_getting_by_api.openweathermap_data import OPENWEATHERMAP_SOURCE_DATA
from app.json_processing.json_processor import *

from datetime import datetime
import requests

from config import *

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

  def load_all_data(self):

    # получить данные по API
    def get_data_by_api(regions_data):
      start_time = print_start_time("data from Openweathermap")
      today = datetime.today()
      today_string = "%s.%s.%s" % (
          str(today.day), str(today.month), str(today.year))
      start_year = 2020
      # получить исторические данные Openweathermap
      openweathermap_data_obj = OPENWEATHERMAP_SOURCE_DATA()
      openweathermap_data_obj.get_temperature_for_regions(
          regions_data=regions_data)
      # write_to_json_in_dir(
      #   filename="history_openweathermap_data_obj_%s_%s.json" % (
      #     str(start_year), today_string),
      #   data=openweathermap_data_obj.temperature_for_regions,
      #   dir="result_data"
      # )
      
      # получить прогноз Openweathermap
      openweathermap_data_obj.get_forecasts_for_regions(
          regions_data=regions_data)
      # write_to_json_in_dir(
      #   filename="forecast_openweathermap_data_obj_%s_%s.json" % (
      #       str(start_year), today_string),
      #   data=openweathermap_data_obj.forecasts_for_regions,
      #   dir="result_data"
      # )
      print_end_time("data from Openweathermap", start_time)
      self.data = {**openweathermap_data_obj.temperature_for_regions, \
                   **openweathermap_data_obj.forecasts_for_regions}
      
    # получить данные с помощью парсера
    def get_data_from_parser():
      start_time = print_start_time("data from Pogodaklimat")
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
      print_end_time("data from Pogodaklimat", start_time)
      return pogodataklimat_parser_obj.data_station_temperature_day

    # запуск получения данных парсерами
    regions_data = get_data_from_parser()
    
    # запуск получения данных по API
    get_data_by_api(regions_data=regions_data)
    
  def load_forecast_for_station(self, station_id):
    start_time = print_start_time("data from Openweathermap")
    today = datetime.today()
    today_string = "%s.%s.%s" % (
        str(today.day), str(today.month), str(today.year))
    
    # получить данные станции из БД
    api_method = "/api/Weather/Get/%s"%str(station_id)
    r = requests.get(url=URL_DB_SERVICE+api_method)
    region_data = r.json() if r.status_code == 200 else None
    
    if not region_data is None:
      # получить прогноз для станции Openweathermap
      openweathermap_data_obj = OPENWEATHERMAP_SOURCE_DATA()
      forecast_for_region = openweathermap_data_obj.get_forecast_for_region(
          station_region_data=region_data,
          station_id=station_id)
      # write_to_json_in_dir(
      #     filename="forecast_openweathermap_station_%s_%s.json" % (
      #         str(station_id), today_string),
      #     data=forecast_for_region,
      #     dir="result_data"
      # )
      print_end_time("data from Openweathermap", start_time)
      self.data = forecast_for_region
    else:
      self.data = None
