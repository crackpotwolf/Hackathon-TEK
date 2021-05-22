import os


class BaseConfig(object):
    DEBUG = False


class ProductionConfig(BaseConfig):
    DEBUG = False
    PATH_TO_DATA = '/app/Data/'

    PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
    PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
    PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")

    LOG_WEATHER_DATA = '/app/Data/weather_data_getter.log.log'
    URL_DB_SERVICE = "http://192.168.100.40:9001"


class LocalConfig(BaseConfig):
    DEBUG = True
    ASSETS_DEBUG = True

    PATH_TO_DATA = os.getcwd()
    PATH_TO_DATA = os.path.join(PATH_TO_DATA, "data")

    PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
    PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
    PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")

    LOG_WEATHER_DATA = 'weather_data_getter.log.log'
    URL_DB_SERVICE = "http://192.168.100.40:9001"


# #####################################################
# # для локального запуска скриптов
# PATH_TO_DATA = os.getcwd()
# PATH_TO_DATA = os.path.join(PATH_TO_DATA, "data")

# PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
# PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
# PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")

# URL_DB_SERVICE = "http://192.168.100.40:9001"
