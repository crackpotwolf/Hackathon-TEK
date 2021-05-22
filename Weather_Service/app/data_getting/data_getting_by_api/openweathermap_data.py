# from app.geometry_processing.geometry_processor import normalize_region
from app.geometry_processing.define_region_by_coordinates import get_region_by_coord
from app.json_processing.json_processor import write_to_json_in_dir, read_from_json_in_dir, delete_json_in_dir

from datetime import datetime, date, timedelta
from multiprocessing import Pool

import requests


def print_start_time(source_name):
  start_time = datetime.now()
  # app.logger.info("Start loading %s: %s" %
  #   (source_name.title(), str(start_time)))
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
  

class OPENWEATHERMAP_SOURCE_DATA:
  def __init__(self):
    self.temperature_for_regions = []
    self.forecasts_for_regions = []
    
  def get_temperature_for_regions(self, regions_data):
    
    def get_weather_data_by_params(
        latitude,
        longitude,
        start_unix,
        end_unix
      ):
      api_key = "1a4f881606d576e6724e309bb850d28c"
      url = "http://history.openweathermap.org/data/2.5/history/city?lat=%s&lon=%s&type=hour&start=%s&end=%s&appid=%s" %\
            (str(latitude), str(longitude), str(
                int(start_unix)), str(int(end_unix)), api_key)
      r = requests.get(url=url)
      if r.status_code == 200:
        return r.json()
      else:
        print("Problem with request by url=%s" % url)
        return None


    def grouped_data_by_date(raw_data):
      dates_data = {}

      for item in raw_data['list']:
        item_timestamp = item["dt"]
        item_datetime = datetime.fromtimestamp(item_timestamp)

        item_day = str(item_datetime.day) if item_datetime.day > 9 else \
            "0%s" % str(item_datetime.day)
        item_month = str(item_datetime.month) if item_datetime.month > 9 else \
            "0%s" % str(item_datetime.month)
        item_date = "%s.%s.%s" % (item_day, item_month, str(item_datetime.year))

        data_obj = {
            "temp": item["main"]["temp"],
            "pressure": item["main"]["pressure"],
            "humidity": item["main"]["humidity"],
            "wind_speed": item["wind"]["speed"],
            "wind_deg": item["wind"]["deg"],
            "clouds_all": item["clouds"]["all"]
        }

        if not item_date in dates_data.keys():
          dates_data[item_date] = [data_obj]
        else:
          dates_data[item_date].append(data_obj)

      return dates_data


    def get_avarage_or_max_values_for_day(dates_data):
      common_dates_data = {}

      temp_average = 0  # температура
      pressure_max = -10000  # давление
      humidity_max = -10000  # влажность
      wind_speed_max = -10000  # скорость ветра
      wind_deg_max = -10000  # угол ветра (при максимальной скорости)
      clouds_max = -10000  # облачность

      for date_key in dates_data.keys():
        for item in dates_data[date_key]:
          temp_average += item["temp"]
          pressure_max = item["pressure"] if pressure_max < item["pressure"] else pressure_max
          humidity_max = item["humidity"] if humidity_max < item["humidity"] else humidity_max
          wind_speed_max = item["wind_speed"] if wind_speed_max < item["wind_speed"] else wind_speed_max
          wind_deg_max = item["wind_deg"] if wind_deg_max < item["wind_deg"] else wind_deg_max
          clouds_max = item["clouds_all"] if clouds_max < item["clouds_all"] else clouds_max

        temp_average = temp_average / len(dates_data[date_key]) - 273.15
        temp_average = round(temp_average, 1)
        common_dates_data[date_key] = {
            "temp_average": temp_average,
            "pressure_max": pressure_max,
            "humidity_max": humidity_max,
            "wind_speed_max": wind_speed_max,
            "wind_deg_max": wind_deg_max,
            "clouds_max": clouds_max
        }

      return common_dates_data

    def get_weather_from_last_year_for_region(coordinates):
      # определение широты и долготы
      latitude = coordinates[0]
      longitude = coordinates[1]
      # запросы со смещением на 7 дней
      # - флаг определения окончания итераций
      fl_stop_iterations = False
      # - определение стартовой даты
      today = date.today()
      # - предыдущая дата итерации
      start_date = date(
          today.year-1, today.month, today.day)
      start_date = start_date + timedelta(days=1)
      # - предыдущая дата итерации
      end_date = start_date + timedelta(days=7)

      data_by_dates = {}

      while not fl_stop_iterations:
        if end_date > today:
          different_days = (end_date - today).days
          if different_days > 0 and different_days < 7:
            end_date = start_date + timedelta(days=different_days)
          else:
            fl_stop_iterations = True
        else:
          start_date_timestamp = datetime(
              start_date.year, start_date.month, start_date.day, 0, 0).timestamp()
          end_date_timestamp = datetime(
              end_date.year, end_date.month, end_date.day, 0, 0).timestamp()

          # получить данные о погоде за указанный период
          data = get_weather_data_by_params(
              latitude=latitude,
              longitude=longitude,
              start_unix=start_date_timestamp,
              end_unix=end_date_timestamp
          )
          # сгруппировать данные по датам
          data_grouped_by_date = grouped_data_by_date(
              raw_data=data,
          )
          # выделить средние/максимальные значение параметров за каждый найденный день
          main_data_by_date = get_avarage_or_max_values_for_day(
              dates_data=data_grouped_by_date
          )

          upd_data_by_dates = {**data_by_dates, **main_data_by_date}
          data_by_dates = upd_data_by_dates

          start_date = end_date
          end_date = start_date + timedelta(days=7)

      return data_by_dates

    
    dict_old_new_attr = {
      "Температура воздуха, °С  (Минимум)": "temp_min_0",
      "Температура воздуха, °С  (Средняя)": "temp_average_0",
      "Температура воздуха, °С  (Максимум)": "temp_max_0",
      "Температура воздуха, °С  (Отклонение от нормы)": "temp_dif_norm_0",
      "Осадки, мм": "precipitation_0"
    }
    new_attributes = []
    
    for i_station_id, station_id in enumerate(regions_data.keys()):
      print("Process station №%s (%s/%s)" %
            (str(station_id), str(i_station_id + 1), str(len(list(regions_data.keys())))))
      
      region_data = read_from_json_in_dir(
        filename="result_day_data_from_station%s.json" % str(station_id),
        dir="middle_data"
      )
      if len(region_data) > 0:
        regions_data[station_id] = region_data[station_id]
      else:
        region_item = regions_data[station_id]
        # нормализовать название региона
        region_item["station_region"] = get_region_by_coord(
            lat=region_item["station_coordinate"][0],
            lon=region_item["station_coordinate"][1]
        )
        if region_item["station_region"] != 'Unknown region':

          start_time = print_start_time("data from station №%s" % str(station_id))
          
          region_coordinates = region_item["station_coordinate"]
          region_addintion_data = get_weather_from_last_year_for_region(
            coordinates=region_coordinates
          )
          
          # определение новых атрибутов, если лист пустой
          if len(new_attributes) == 0:
            if len(list(region_addintion_data.keys())) > 0:
              first_item = list(region_addintion_data.values())[0]
              for attr in first_item.keys():
                new_attributes.append(attr)
          
          # сформировать обновленные объекты
          for station_date_key in region_item["weather_dates"].keys():
            region_date_item = region_item["weather_dates"][station_date_key]
            
            # заменить русские названия атрибутов на 
            new_weather_date = {}
            for header_key in dict_old_new_attr.keys():
              new_weather_date[dict_old_new_attr[header_key]] = \
                region_date_item[header_key]
            
            # добавить новые атрибуты
            for header_key in new_attributes:
              new_weather_date[header_key] = region_addintion_data[station_date_key][header_key] \
                if station_date_key in region_addintion_data.keys() else None
            
            # обновить объект в выгрузке 
            region_item["weather_dates"][station_date_key] = new_weather_date
          
          # write_to_json_in_dir(
          #   filename="result_day_data_from_station%s.json" % str(station_id),
          #   data={station_id: region_item},
          #   dir="middle_data"
          # )
          print_end_time("data from station №%s" % str(station_id), start_time)
          # отправить объект в БД
          api_method = "/api/Weather/Post"
          r = requests.get(url=URL_DB_SERVICE+api_method,
            json={station_id: region_item})
          if r.status_code == 200:
            print("History weather data of station №%s was posted successfully" %
              str(station_id))
            app.logger.warning("History weather data of station №%s was posted successfully" %
              str(station_id))
          else:
            print("History weather data of station №%s wasn't posted. Status %s" %
              (str(station_id), str(r.status_code)))
            app.logger.warning("History weather data of station №%s wasn't posted. Status %s" %
              (str(station_id), str(r.status_code)))
        
        else:
          print("Can't define region for station №%s" % str(station_id))
          app.logger.warning("Can't define region for station №%s" % str(station_id))
    
    self.temperature_for_regions = regions_data

  def get_forecast_for_region(self, station_region_data, station_id):
    def get_weather_by_params(
        latitude,
        longitude
      ):
      api_key = "1a4f881606d576e6724e309bb850d28c"
      url = "https://pro.openweathermap.org/data/2.5/forecast/climate?lat=%s&lon=%s&appid=%s" %\
            (str(latitude), str(longitude), api_key)
      r = requests.get(url=url)
      if r.status_code == 200:
        return r.json()
      else:
        print("Problem with request by url=%s" % url)
        return None


    def form_result_forecasts_unloading(forecasts_data):
      result_forecasts_unloading = {}
      for forecast_item in forecasts_data["list"]:
        # дата
        item_timestamp = forecast_item["dt"]
        item_datetime = datetime.fromtimestamp(item_timestamp)

        item_day = str(item_datetime.day) if item_datetime.day > 9 else \
            "0%s" % str(item_datetime.day)
        item_month = str(item_datetime.month) if item_datetime.month > 9 else \
            "0%s" % str(item_datetime.month)
        item_date = "%s.%s.%s" % (item_day, item_month, str(item_datetime.year))
        #
        data_obj = {
            "temp_min_0": None,
            "temp_average_0": None,
            "temp_max_0": None,
            "temp_dif_norm_0": None,
            "precipitation_0": None,
            "temp_average": round((forecast_item["temp"]["min"] + forecast_item["temp"]["max"]) / 2 - 273.15, 1),
            "pressure_max": forecast_item["pressure"],
            "humidity_max": forecast_item["humidity"],
            "wind_speed_max": forecast_item["speed"],
            "wind_deg_max": forecast_item["deg"],
            "clouds_max": forecast_item["clouds"]
        }
        #
        result_forecasts_unloading[item_date] = data_obj

      return result_forecasts_unloading


    def get_forecast_by_coordinate(coordinates):
      # определение широты и долготы
      latitude = coordinates[0]
      longitude = coordinates[1]
      # получить данные о прогнозе по координатам
      forecasts_data = get_weather_by_params(
          latitude=latitude,
          longitude=longitude
      )
      processed_forecast_data = form_result_forecasts_unloading(
          forecasts_data=forecasts_data
      )
      return processed_forecast_data

    station_region_forecast = get_forecast_by_coordinate(
      coordinates=station_region_data["station_coordinate"]
    )
    data_to_send = station_region_data
    data_to_send["weather_dates"] = station_region_forecast
    # write_to_json_in_dir(
    #   filename="forecast_for_station_%s.json" % str(station_id),
    #   data=data_to_send,
    #   dir="middle_data"
    # )
    # отправить объект в БД
    api_method = "/api/Weather/Post"
    r = requests.get(url=URL_DB_SERVICE+api_method,
      json={station_id: region_item})
    if r.status_code == 200:
      print("Forecast weather data of station №%s was posted successfully" %
        str(station_id))
      app.logger.warning("Forecast weather data of station №%s was posted successfully" %
        str(station_id))
    else:
      print("Forecast weather data of station №%s wasn't posted. Status %s" %
        (str(station_id), str(r.status_code)))
      app.logger.warning("Forecast weather data of station №%s wasn't posted. Status %s" %
        (str(station_id), str(r.status_code)))
      
    return data_to_send

  def get_forecasts_for_regions(self, regions_data):
    # цикл по всем станциям
    for i_station_id, station_id in enumerate(regions_data.keys()):
      print("Process station №%s (%s/%s)" %
            (str(station_id), str(i_station_id + 1), str(len(list(regions_data.keys())))))
      # получить данные по текущей станции
      station_region_data = regions_data[station_id]
      # сформировать для станции прогноз на 30 дней
      station_region_forecast = self.get_forecast_for_region(
        station_region_data=station_region_data, 
        station_id=station_id
      )
      # добавить прогноз в общую выгрузку
      self.forecasts_for_regions.append(station_region_forecast)
