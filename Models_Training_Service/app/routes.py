# -*- coding: utf-8 -*-
from flask import Blueprint, request, jsonify

from app import app
from app.model_training.model_trainer import MODEL_TRAINER
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


@api.route('/train_model', methods=['DELETE', 'GET', 'POST', 'PUT'])
def collect_all_weather_data():
    """"""
    if request.method == 'POST':
        try:

          # сбор данных о погоде
          print("START TRAINING MODEL")
          app.logger.info("START TRAINING MODEL")

          model_trainer_obj = MODEL_TRAINER()
          model_trainer_obj.create_model()

          print("END TRAINING MODEL")
          app.logger.info("END TRAINING MODEL")

          return jsonify({
            'status': 'Success',
            'info':  'Process of training model was completed successfully. Check adapter log'
          }), 200
          
        except Exception as ex:
          return jsonify({
              'status': 'Failed',
              'info':  'Process of training model is failed with error: %s'
              % ex}), 400

    else:
        # остальные методы не поддерживаются
        app.logger.info(f'Method {request.method} not supported')
        return jsonify({
            'status': 'Failed',
            'info': f'Method {request.method} not supported'
        }), 404
