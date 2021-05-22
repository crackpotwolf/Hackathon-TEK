from app import app

from app.data_getting.data_getter import DATA_GETTER
from app.json_processing.json_processor import *

import pandas as pd
from datetime import datetime
from sklearn.utils import shuffle
import numpy as np
from sklearn.model_selection import train_test_split

from sklearn.preprocessing import MinMaxScaler
from sklearn.preprocessing import LabelEncoder
from sklearn.metrics import mean_squared_error
from keras.models import Sequential
from keras.layers import Dense
from keras.layers import LSTM


from matplotlib import pyplot
from sklearn.metrics import mean_squared_error



from sklearn.model_selection import train_test_split
from sklearn.multiclass import OneVsRestClassifier
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.linear_model import LogisticRegression
from sklearn.naive_bayes import MultinomialNB
from sklearn.pipeline import Pipeline
from sklearn.svm import LinearSVC
from sklearn.metrics import accuracy_score
from sklearn.metrics import mean_squared_error


class MODEL_TRAINER:
  def __init__(self):
    self.result = []
    
  def create_model(self, data):
    
    def normalize_data(data):
      def marking(data, days=3):
        '''
        Разметка вероятности возникновения аварии
        '''
        values = data.values
        # Идем с конца
        for indx in range(data.shape[0]-1, 0, -1):
            # Нашли аварию
            if values[indx, -1] == 100:
                # Отмечаем {days} дней назад
                for i in range(1, days):
                    # print(indx, i, )
                    tmp = indx-i
                    if values[tmp, -1] != 0:
                        break

                    values[tmp, -1] = (100 / (days)) * (days - i)
        return pd.DataFrame(values, columns=data.columns)

      def series_to_supervised(data, n_in=1, n_out=1, dropnan=True, y_columns='Id-2', x_columns=None):
          '''
          Готовит обучающую выборку
          '''
          shape = data.shape

          y = data[y_columns].values
          x = data[x_columns].values

          # print(values)
          start = shape[0] - n_out
          x_indexes = [i for i in range(shape[1]-1)]
          # print(x_indexes)
          X, Y = [], []
          for i in range(start, 0, -1):
              if i-n_in < 0:
                  break

              X.append(np.concatenate(x[i-n_in:i]))
              # print(values[start:start+n_out, y_indexes])
              Y.append(y[i:i+n_out])
          return np.array(X), np.array(Y)

      def get_data_weather(df, n_in=30, n_out=7, test_size=0.33):
          columns_data = ['TempMin0', 'TempAverage0', 'TempMax0', 'TempDifNorm0', 'Percipitation',
                          'TempAverage', 'PressureMax', 'HumidityMax', 'WindSpeedMax', 'WindDegMax', 'CloudsMax']
          marked = marking(df)
          marked[columns_data + ['Id-2']] = scale(marked[columns_data + ['Id-2']])
          x, y = series_to_supervised(
              marked, n_in, n_out, x_columns=columns_data, y_columns='Id-2')
          return train_test_split(x, y, test_size=test_size, random_state=42)

      scaler = MinMaxScaler(feature_range=(0, 1))

      def scale(df):
          # normalize features
          scaled = scaler.fit_transform(df.values)

          return scaled

      X_train, X_test, y_train, y_test = get_data_weather(df, n_out=1)

      X_train = X_train.reshape((X_train.shape[0], 1, X_train.shape[1]))
      X_test = X_test.reshape((X_test.shape[0], 1, X_test.shape[1]))
      
      return X_train, X_test, y_train, y_test
    
    def train_model(data):
      X_train, X_test, y_train, y_test = normalize_data(data=pd.DataFrame(data))

      # design network
      model = Sequential()
      model.add(LSTM(50, input_shape=(X_train.shape[1], X_train.shape[2])))
      model.add(Dense(y_train.shape[1]))
      model.compile(loss='mae', optimizer='adam')
      
      return model
    
    data_getter_obj = DATA_GETTER()
    weather_data = data_getter_obj.load_data()
    if not weather_data is None:
      
      new_model = train_model(
        data=weather_data
      )
    else:
      print("Process of getting weather data is failed. Weather data is None")
      app.logger.warning("Process of getting weather data is failed. Weather data is None")
