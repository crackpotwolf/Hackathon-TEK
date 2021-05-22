import os

# определение IP-адреса


class BaseConfig(object):
    DEBUG = False


class ProductionConfig(BaseConfig):
    DEBUG = False
    PATH_TO_DATA = '/app/Data/'

    PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
    PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
    PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")

    LOG_WEATHER_DATA = '/app/Data/weather_data_getter.log.log'
    # KEY_DB_SERVICE = "PROD"
    # URL_DB_SERVICE = dict_keyword_ip_DB_SERVICE[KEY_DB_SERVICE]


class DevelopConfig(BaseConfig):
    DEBUG = False
    PATH_TO_DATA = '/app/Data'

    PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
    PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
    PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")

    LOG_WEATHER_DATA = '/app/Data/weather_data_getter.log.log'
    # KEY_DB_SERVICE = "DEV"
    # URL_DB_SERVICE = dict_keyword_ip_DB_SERVICE[KEY_DB_SERVICE]


class TestConfig(BaseConfig):
    DEBUG = False
    PATH_TO_DATA = '/app/Data'

    PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
    PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
    PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")

    LOG_WEATHER_DATA = '/app/Data/weather_data_getter.log.log'
    # KEY_DB_SERVICE = "TEST"
    # URL_DB_SERVICE = dict_keyword_ip_DB_SERVICE[KEY_DB_SERVICE]


class LocalConfig(BaseConfig):
    DEBUG = True
    ASSETS_DEBUG = True

    PATH_TO_DATA = os.getcwd()
    PATH_TO_DATA = os.path.join(PATH_TO_DATA, "data")

    PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
    PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
    PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")

    LOG_WEATHER_DATA = 'weather_data_getter.log.log'

    # # KEY_DB_SERVICE = "TEST"
    # KEY_DB_SERVICE = "DEV"
    # URL_DB_SERVICE = dict_keyword_ip_DB_SERVICE[KEY_DB_SERVICE]


#####################################################
# для локального запуска
PATH_TO_DATA = os.getcwd()
PATH_TO_DATA = os.path.join(PATH_TO_DATA, "data")

PATH_TO_INPUT_DATA = os.path.join(PATH_TO_DATA, "input_data")
PATH_TO_MIDDLE_DATA = os.path.join(PATH_TO_DATA, "middle_data")
PATH_TO_RESULT_DATA = os.path.join(PATH_TO_DATA, "result_data")
