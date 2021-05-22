from nltk.corpus import stopwords
import string
from nltk.tokenize import word_tokenize
import pymorphy2
from nltk.stem import SnowballStemmer
from app.geometry_processing.regions_norm_names import ALL_REGIONS

import pandas as pd
import numpy as np
from transliterate import translit
from numba import jit
import random
from datetime import datetime
from pprint import pprint
import re


import nltk
nltk.download('stopwords')
nltk.download('punkt')

stop_words = set(stopwords.words('russian') +
                 stopwords.words('english') + list(string.punctuation))

# Сброс ограничений на количество выводимых рядов
pd.set_option('display.max_rows', 15)

# Сброс ограничений на число столбцов
pd.set_option('display.max_columns', None)

# # Сброс ограничений на количество символов в записи
# pd.set_option('display.max_colwidth', None)
morph = pymorphy2.MorphAnalyzer()
snowball = SnowballStemmer(language="russian")

# Лемматизация
def lem(x):
  lemma = morph.parse(x)
  return lemma[0].normal_form if len(lemma) != 0 else x

# Стемминг
def stemming(x):
  return snowball.stem(x)

# Нормализация
def normalize(arr):
  _arr = [lem(x) for x in arr]
  return [stemming(x) for x in _arr]


replace_name_area = re.compile(
    'область|республика|край|округ|автономн[\w]*|город|[\(\).]', flags=re.IGNORECASE)


def find_by_name(arr, name):
  '''
  Поиск региона
  '''
  # Нормализация имени для поиска
  _names = name.split(',')
  def preprocessing_data(x): return re.split(
      '[\s-]', replace_name_area.sub('', x).strip())
  _names = [normalize(preprocessing_data(name)) for name in _names]
  print(_names)

  defaults = {
      # 'петербург': "Санкт-Петербург",
      # 'москв': 'Московская область',
      # 'крым': 'Крым',
      # 'Ямало'
      'Санкт-Петербург': ['петербург'],
      'Московская область': ['москв'],
      'Крым': ['крым'],
      'Ямало-Ненецкий автономный округ': ['яма', 'ненецк'],
  }
  for key, values in defaults.items():
      # Москва
      for name in _names:
          # if  all([True if key in name else False for name in _names]):
          if all([True if value in name else False for value in values]):
              yield key
              return

  # Нормализация регионов
  _arr = []
  for item in arr:
      norm = list(filter(lambda x: x != '',
        normalize(preprocessing_data(item))))
      if len(norm) != 0:
          _arr.append((item, norm))

  # Поиск совпадений
  for source, item in _arr:
      for name in _names:
          if any([True if __name in item else False for __name in name][:2]):
              yield source
                
def normalize_region(region):
  '''
  Нормализация региона
  '''
  return list(find_by_name(ALL_REGIONS, region))[0]


def normalize_regions(regions):
  '''
  Нормализация списка регионов
  '''
  res = {}
  for region in regions:
      res[region] = normalize_region(region=region)
  return res
