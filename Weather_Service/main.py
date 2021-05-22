# from app import app

from app.data_getting.data_getter import DATA_GETTER


if __name__ == "__main__":
  data_getter_obj = DATA_GETTER()
  # загрузка данных
  # data_getter_obj.load_all_data()
  data_getter_obj.load_forecast_for_station(station_id=None)
