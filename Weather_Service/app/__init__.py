from app import routes
from app.routes import api
from flask import Flask

import logging
from logging.handlers import RotatingFileHandler
from config import ProductionConfig, LocalConfig

import os


app = Flask(__name__)

ENVIRONMENT = os.environ.get("ENVIRONMENT")
if ENVIRONMENT == 'Production':
    app.config.from_object(ProductionConfig)
else:
    app.config.from_object(LocalConfig)

log_weather_getter_handler = RotatingFileHandler(app.config['LOG_WEATHER_DATA'],
  mode='a', maxBytes=5*1024*1024, backupCount=2, encoding="utf-8", delay=0)
logging.basicConfig(level=logging.INFO,
  format='%(asctime)s %(levelname)s %(name)s %(threadName)s - %(module)s : %(message)s',
  handlers={log_weather_getter_handler}
)

# регистрация модуля api
app.register_blueprint(api, url_prefix='/api')

app.run(host='0.0.0.0', port=8008)
