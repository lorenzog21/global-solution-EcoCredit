"""
Simulador de sensor de CH4 (metano).

Metano tem GWP-100 = 25 (25x mais potente que CO2 em 100 anos).
Fontes industriais: válvulas, flanges, tanques de armazenamento.
"""

import random


def read(base_ppm: float = 2.8) -> dict:
    """
    Simula leitura de sensor de metano.
    Converte para CO2 equivalente usando GWP-100 = 25.
    """
    noise = random.gauss(0, base_ppm * 0.08)
    ppm = max(0.0, base_ppm + noise)
    # GWP 25: 1 tCH4 = 25 tCO2e
    quantity_tco2e = round((ppm / 1000) * 0.003 * 25, 4)
    return {
        "gas_type": "CH4",
        "raw_ppm": round(ppm, 3),
        "quantity_tco2e": quantity_tco2e,
    }
