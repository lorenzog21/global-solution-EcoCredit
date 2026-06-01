"""
Mock do dado satelital Sentinel-5P (ESA).

Sentinel-5P mede concentração de CO2 em coluna (XCO2) em PPM.
Em produção real: GET https://ads.atmosphere.copernicus.eu/api/v2
Para o MVP: simula XCO2 com base regional industrial.

Valor típico:
  - Atmosfera global 2024: ~418.5 PPM
  - Regiões industriais: +0 a +12 PPM de excesso regional
"""

import random


def get_regional_co2(latitude: float = -22.9) -> float:
    """
    Retorna concentração regional de CO2 em coluna (XCO2) em PPM.
    Simula dados do Sentinel-5P com variação geográfica.
    """
    base_xco2 = 418.5  # PPM médio global 2024
    # Offset industrial — regiões do SE brasileiro têm maior concentração
    industrial_offset = random.uniform(0, 12.0)
    noise = random.gauss(0, 0.5)
    return round(base_xco2 + industrial_offset + noise, 2)
