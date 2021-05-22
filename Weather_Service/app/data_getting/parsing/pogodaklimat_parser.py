from app.json_processing.json_processor import write_to_json_in_dir, read_from_json_in_dir

from bs4 import BeautifulSoup
from calendar import monthrange
from datetime import datetime

import requests


class POGODAKLIMAT_SOURCE_DATA:
  def __init__(self):
    self.russian_stations_ids = []
    self.data_station_temperature_day = {}

  def parse_pogodaklimat_day_data(self, start_year, stations_ids):
    min_year = 1881  # мимнимальный год
    # установка начального года
    start_year = start_year if start_year > min_year else min_year
    # определение текущего года и месяца
    today = datetime.today()
    current_year = today.year
    current_month = today.month
    current_day = today.day

    for i_station_id, station_id in enumerate(stations_ids):
      print("Process station №%s (%s/%s)"%(str(station_id), str(i_station_id + 1), str(len(stations_ids))))
      
      data_by_station_id = read_from_json_in_dir(
          filename="pogodaklimat_day_data_from_station%s.json" % str(
              station_id),
        dir="middle_data"
      )
      if len(data_by_station_id) > 0:
        self.data_station_temperature_day[station_id] = data_by_station_id
      
      else:
      
        station_name = None
        station_region = None
        station_coordinate = None
        
        # старт парсинга
        for i_year in range(start_year, current_year+1, 1):
          max_month = 12 if i_year < current_year else current_month
          for i_month in range(1, max_month+1):
            current_url = "http://www.pogodaiklimat.ru/monitor.php?id=%s&month=%s&year=%s" %\
                (str(station_id), str(i_month), str(i_year))
            r = requests.get(current_url)
            r.encoding = r.apparent_encoding

            b_soup = BeautifulSoup(r.text, 'html.parser')
            
            # анализ текста для определения наименования станции, региона станции, координаты станции
            if station_name is None:
              info_text_obj = b_soup.find_all("ul", {"class": "climate-list"})[0]
              info_text = info_text_obj.text
              # название станции
              station_name = info_text.split(
                  "Информация о погоде получена с метеорологической станции ")[1].split(" (")[0]
              # название региона
              station_region = info_text.split(
                  station_name + " (")[1].split(")")[0].strip()
              station_name = station_name.strip()
              # координаты
              # - широта
              latitude = info_text.split(
                  " широта ")[1].split(", долгота ")[0].strip()
              latitude = float(latitude)
              # - долгота
              longitude = info_text.split(
                  " долгота ")[1].split(", высота ")[0].strip()
              longitude = float(longitude)
              station_coordinate = [latitude, longitude]
            
            # анализ таблицы
            rows = b_soup.find_all('tr')

            headers_rus = []
            main_headers = []
            sub_headers = []
            
            i_day = 0
            fl_continue = True
            
            for i_row, row in enumerate(rows):
              if fl_continue:
                if i_row < 2:
                  # определение заголовков в таблице
                  if i_row == 0:
                    # забрать основные заголовки
                    main_headers = row.text.split("\n")
                    upd_main_headers = [
                        header for header in main_headers if header != '']
                    main_headers = upd_main_headers
                  else:
                    # забрать подзаголовки
                    sub_headers = row.text.split("\n")
                    upd_sub_headers = [
                        header for header in sub_headers if header != '']
                    sub_headers = upd_sub_headers
                    # сформирвать общие заголовки
                    for i_main_header, main_header in enumerate(main_headers):
                      if i_main_header != 1:
                        headers_rus.append(main_header)
                      else:
                        for sub_header in sub_headers:
                          new_header = "%s (%s)" % (main_header, sub_header)
                          headers_rus.append(new_header)
                else:
                  i_day += 1
                  monthrange_range = monthrange(i_year, i_month)
                  if i_day <= monthrange_range[1]:
                  
                    if i_day != current_day or \
                      (i_day == current_day and (i_year != current_year or i_month != current_month)):

                      i_day_str = str(i_day) if len(
                          str(i_day)) == 2 else "0"+str(i_day)
                      i_month_str = str(i_month) if len(
                          str(i_month)) == 2 else "0"+str(i_month)
                      i_date = "%s.%s.%s" % (i_day_str, i_month_str, str(i_year))
                      
                      # считывание основных значений строк
                      row_tags = [sub_tag for sub_tag in row.contents if not sub_tag in ["\n", " ", ""]]
                      
                      obj_info_by_day = {}
                      for i, header in enumerate(headers_rus):
                          if i > 0:
                            txt_value = row_tags[i].text.replace('+', '')
                            obj_info_by_day[header] = float(txt_value) \
                              if not txt_value is None and \
                                not txt_value.lower() in ["none", "null", " ", ""] \
                                  else None

                      if not station_id in self.data_station_temperature_day.keys():
                        # добавление данных
                        self.data_station_temperature_day[station_id] = {
                          "station_name": station_name,
                          "station_region": station_region,
                          "station_coordinate": station_coordinate,
                          "weather_dates": {
                            i_date: obj_info_by_day
                          }
                        }
                      else:
                        self.data_station_temperature_day[station_id]["weather_dates"][i_date] = obj_info_by_day
                    
                    else:
                      if i_year == current_year and i_month == current_month:
                        fl_continue = False
                  else:
                    fl_continue = False
              else:
                break

        write_to_json_in_dir(
            filename="pogodaklimat_day_data_from_station%s.json" %str(station_id),
            data=self.data_station_temperature_day[station_id],
            dir="middle_data"
        )

  def parse_pogodaklimat_station_ids(self):
    # определение текущего года и месяца
    today = datetime.today()
    current_year = today.year
    current_month = today.month
    
    # получить номера станций по ссылке за текущие год и месяц
    url = "http://www.pogodaiklimat.ru/monitors.php?id=rus&month=%s&year=%s" % (
        str(current_month), str(current_year))
    r = requests.get(url)
    r.encoding = r.apparent_encoding

    b_soup = BeautifulSoup(r.text, 'html.parser')
    rows = b_soup.find_all('tr')
    
    # старт парсинга
    for i_row, row in enumerate(rows):
      # определение заголовков в таблице
        if i_row > 1:
          # считывание основных значений строк
          row_text = row.text[1:-1]
          cells_values = row_text.split("\n")
          if len(cells_values) == 1:
            if cells_values[0] != "Россия":
              break
            else:
              continue
          else:
            station_id = int(cells_values[0]) if not cells_values[0] is None and \
              not cells_values[0].lower() in ["none", "null", "", " "] else None
            self.russian_stations_ids.append(station_id)

        # headers_rus = []
        # main_headers = []
        # sub_headers = []
        # for i_row, row in enumerate(rows):
        #   if i_row < 2:
        #     # определение заголовков в таблице
        #     if i_row == 0:
        #       # забрать основные заголовки
        #       main_headers = row.text.split("\n")
        #       upd_main_headers = [
        #           header for header in main_headers if header != '']
        #       main_headers = upd_main_headers
        #     else:
        #       # забрать подзаголовки
        #       sub_headers = row.text.split("\n")
        #       upd_sub_headers = [
        #           header for header in sub_headers if header != '']
        #       sub_headers = upd_sub_headers
        #       # сформирвать общие заголовки
        #       headers_rus += main_headers[:2]
        #       for i_main_header, main_header in enumerate(main_headers):
        #         if i_main_header < 2:
        #           headers_rus.append(main_header)
        #         else:
        #           for i_sub_header, sub_header in enumerate(sub_headers):
        #             if (i_main_header < 3 and i_sub_header < i_main_header) or \
        #               (i_main_header >= 3 and i_sub_header >= i_main_header):
        #               new_header = "%s (%s)" % (main_header, sub_header)
        #               headers_rus.append(new_header)
        #   else:
        #     # считывание основных значений строк
        #     row_text = row.text[1:-1]
        #     cells_values = row_text.split("\n")
        #     if len(cells_values) == 1:
        #       if cells_values[0] != "Россия":
        #         break
        #       else:
        #         continue
        #     else:
        #       new_obj = {}
        #       for i_header, header in enumerate(headers_rus):
        #         if i_header < len(cells_values):
        #           new_obj[header] = cells_values[i_header]
        #         # else:
        #         #   new_obj[header] = "%s&id=%s" % (current_url, new_obj[headers_rus[0]])

        #       self.data_station_temperature_month.append(new_obj)
