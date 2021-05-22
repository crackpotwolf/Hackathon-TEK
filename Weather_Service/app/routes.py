# -*- coding: utf-8 -*-

from os import stat
from flask import Blueprint, request, jsonify

from app import app
from app.data_getting.data_getter import DATA_GETTER
from app.json_processing.json_processor import *


# регистрация модуля api
api = Blueprint('api', __name__, url_prefix='/api')


@api.route('/', methods=['DELETE', 'GET', 'POST', 'PUT'])
def return_message_error():
  app.logger.info("Wrong URL")
  return jsonify({
      'status': 'Failed',
      'info': 'Wrong URL'
  }), 404


@api.route('/collect_all_weather_data', methods=['DELETE', 'GET', 'POST', 'PUT'])
def collect_all_weather_data():
    """"""
    if request.method == 'POST':
        try:

          # сбор данных о погоде
          print("START GETTING ALL WEATHER DATA")
          app.logger.info("START GETTING ALL WEATHER DATA")

          data_getter_obj = DATA_GETTER()
          # загрузка данных
          data_getter_obj.load_all_data()

          print("END GETTING ALL WEATHER DATA")
          app.logger.info("END GETTING ALL WEATHER DATA")

          return jsonify({
            'status': 'Success',
            'info':  'Process of getting all weather data was completed successfully. Check adapter log'
          }), 200
          
        except Exception as ex:
          return jsonify({
              'status': 'Failed',
              'info':  'Process of getting all weather data is failed with error: %s'
              % ex}), 400

    else:
        # остальные методы не поддерживаются
        app.logger.info(f'Method {request.method} not supported')
        return jsonify({
            'status': 'Failed',
            'info': f'Method {request.method} not supported'
        }), 404


@api.route('/collect_forecast_for_station', methods=['DELETE', 'GET', 'POST', 'PUT'])
def collect_all_weather_data():
    """"""
    if request.method == "POST":
      
      message_info = "Process of forming full topology is failed"

      try:
        # идентификатор отдельной ТП
        station_id = request.args.get("station_id")

        if not station_id is None:

          try:

            # сбор прогноза погоды для отдельной станции
            print("START GETTING FORECAST WEATHER DATA FOR STATION №%s"%str(station_id))
            app.logger.info(
                "START GETTING FORECAST WEATHER DATA FOR STATION №%s" % str(station_id))

            data_getter_obj = DATA_GETTER()
            # загрузка данных прогноза
            data_getter_obj.load_forecast_for_station(station_id=station_id)

            print("END GETTING FORECAST WEATHER DATA FOR STATION №%s" %
                  str(station_id))
            app.logger.info(
                "END GETTING FORECAST WEATHER DATA FOR STATION №%s" % str(station_id))

            return jsonify({
                'status': 'Success',
                'info': 'Process of getting forecast weather data for Station №%s was completed successfully. Check adapter log' % str(station_id)
            }), 200

          except Exception as ex:
            return jsonify({
                'status': 'Failed',
                'info':  'Process of getting forecast weather data for Station №%s is failed with error: %s'
                % (str(station_id), ex)}), 400
        else:
          message_info = "There is 'station_id' value is None or not numeric"
          app.logger.exception(message_info)
          print(message_info)

        return jsonify({
          'status': 'Failed',
          'info': message_info
        }), 500

      except Exception as ex:
        app.logger.exception(ex)
        return jsonify({
          'status': 'Failed',
          'info': message_info
        }), 500

    else:
      # остальные методы не поддерживаются
      app.logger.info(f'Method {request.method} not supported')
      print(f'Method {request.method} not supported')
      return jsonify({
        'status': 'Failed', 
        'info': f'Method {request.method} not supported'
      }), 404
