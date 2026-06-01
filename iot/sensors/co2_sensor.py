"""
Simulador de sensor de CO2 industrial.

Mede concentração de dióxido de carbono em PPM.
Conversão: 1 PPM CO2 = ~0.0018 tCO2e/hora para instalação de médio porte.
Baseline atmosférico: ~415 PPM; instalações industriais: 800–1500 PPM.
"""

import random


def read(base_ppm: float = 1100.0) -> dict:
    """
    Simula leitura de sensor de CO2.
    Adiciona ruído gaussiano ±5% para simular variação real.
    """
    noise = random.gauss(0, base_ppm * 0.05)
    ppm = max(400.0, base_ppm + noise)
    # Conversão aproximada PPM → tCO2e/hora para instalação industrial
    quantity_tco2e = round((ppm / 1000) * 0.08, 4)
    return {
        "gas_type": "CO2",
        "raw_ppm": round(ppm, 2),
        "quantity_tco2e": quantity_tco2e,
    }
