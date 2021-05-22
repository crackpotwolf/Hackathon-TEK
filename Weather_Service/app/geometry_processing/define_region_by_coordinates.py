import pandas as pd
import numpy as np
import json
import urllib.request
from turfpy.measurement import boolean_point_in_polygon
from geojson import Point, Feature, MultiPolygon


def get_regions_polygons():
    regions_url = 'https://gist.githubusercontent.com/max107/6571147/raw/1849675dceead974319c02214456565ad6e77164/Regions.json'
    with urllib.request.urlopen(regions_url) as url:
        regions_coord = json.loads(url.read().decode())

    regions_poly = pd.DataFrame(columns=['Region', 'Polygon'])
    for region in regions_coord.keys():
        polygon = []
        for area in regions_coord[region]:
            area_poly = []
            for pair in regions_coord[region][area]:
                point = (pair[0], pair[1])
                area_poly.append(point)
            polygon.append((area_poly,))
        regions_poly = regions_poly.append({'Region': region, 'Polygon': Feature(
            geometry=MultiPolygon(polygon))}, ignore_index=True)

    return regions_poly


def get_region_by_coord(lat: float, lon: float):
    point = Feature(geometry=Point([lat, lon]))
    regions_poly = get_regions_polygons()
    res = 'Unknown region'
    for index2, item in regions_poly.iterrows():
        region = item['Region']
        polygon = item['Polygon']
        if(boolean_point_in_polygon(point, polygon)):
            res = region
    return res
