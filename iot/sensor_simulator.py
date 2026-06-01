"""
EcoCredit — Simulador de Sensores IoT
Simula leituras de sensores industriais + dados satelitais Sentinel-5P (ESA)
e publica para a API do EcoCredit via POST /api/v1/emissions

Uso:
    pip install -r requirements.txt
    python sensor_simulator.py

Para testar sem API (modo offline): os payloads são salvos em iot/payloads/cache.jsonl
"""

import json
import random
import time
from datetime import datetime, timezone

try:
    import requests
    REQUESTS_AVAILABLE = True
except ImportError:
    REQUESTS_AVAILABLE = False
    print("⚠️  'requests' não instalado. Rodando em modo offline (apenas payloads no console).")

from sensors.co2_sensor import read as read_co2
from sensors.methane_sensor import read as read_methane
from sensors.satellite_mock import get_regional_co2

# ─────────────────────────────────────────────
# Configuração
# ─────────────────────────────────────────────
API_BASE_URL = "http://localhost:5000/api/v1"
API_TOKEN = "seu-jwt-aqui"   # Substitua por token real após POST /auth/login
PUBLISH_INTERVAL_SECONDS = 30

FACILITIES = [
    {
        "facility_id": "fac-001",
        "name": "Refinaria RJ",
        "type": "REFINERY",
        "sensors": ["CO2", "CH4"],
        "base_co2_ppm": 1100.0,
        "base_ch4_ppm": 2.8,
        "latitude": -22.9068,
    },
    {
        "facility_id": "fac-002",
        "name": "Plataforma P-51",
        "type": "RIG",
        "sensors": ["CO2"],
        "base_co2_ppm": 850.0,
        "base_ch4_ppm": 0,
        "latitude": -23.5505,
    },
]


# ─────────────────────────────────────────────
# Publicação para API
# ─────────────────────────────────────────────
def build_payload(facility: dict, sensor_reading: dict) -> dict:
    """Monta o payload JSON para POST /api/v1/emissions."""
    return {
        "facilityId": facility["facility_id"],
        "gasType": sensor_reading["gas_type"],
        "quantityTco2e": sensor_reading["quantity_tco2e"],
        "source": "IOT_SENSOR",
        "sensorId": f"sensor-{sensor_reading['gas_type'].lower()}-{facility['facility_id']}",
        "rawPpm": sensor_reading.get("raw_ppm"),
        "satelliteCo2Regional": get_regional_co2(facility.get("latitude", -22.9)),
    }


def publish_to_api(payload: dict) -> bool:
    """Publica payload para a API. Retorna True se bem-sucedido."""
    if not REQUESTS_AVAILABLE:
        save_to_local_cache(payload)
        return False

    try:
        headers = {
            "Authorization": f"Bearer {API_TOKEN}",
            "Content-Type": "application/json"
        }
        response = requests.post(
            f"{API_BASE_URL}/emissions",
            json=payload,
            headers=headers,
            timeout=10
        )
        if response.status_code == 201:
            print(f"  ✅ [{datetime.now(timezone.utc).isoformat()}] Publicado: "
                  f"{payload['facilityId']} | {payload['gasType']} | {payload['quantityTco2e']} tCO2e")
            return True
        else:
            print(f"  ⚠️  Erro {response.status_code}: {response.text[:100]}")
            save_to_local_cache(payload)
            return False
    except Exception:
        print(f"  📴 API offline — salvando payload em cache local")
        save_to_local_cache(payload)
        return False


def save_to_local_cache(payload: dict):
    """Salva payload localmente quando API está offline."""
    import os
    os.makedirs("payloads", exist_ok=True)
    with open("payloads/cache.jsonl", "a") as f:
        f.write(json.dumps(payload) + "\n")


def print_payload_sample(payload: dict):
    """Imprime payload formatado para demonstração."""
    print("\n📡 PAYLOAD IoT:")
    print(json.dumps(payload, indent=2, ensure_ascii=False))


# ─────────────────────────────────────────────
# Loop principal
# ─────────────────────────────────────────────
def simulate_cycle():
    """Executa um ciclo completo de leitura de todos os sensores."""
    print(f"\n{'─'*50}")
    print(f"🌍 Ciclo: {datetime.now(timezone.utc).strftime('%Y-%m-%d %H:%M:%S UTC')}")

    for facility in FACILITIES:
        print(f"\n🏭 {facility['name']} ({facility['type']})")

        if "CO2" in facility["sensors"]:
            reading = read_co2(facility["base_co2_ppm"])
            payload = build_payload(facility, reading)
            print_payload_sample(payload)
            publish_to_api(payload)

        if "CH4" in facility["sensors"] and facility["base_ch4_ppm"] > 0:
            reading = read_methane(facility["base_ch4_ppm"])
            payload = build_payload(facility, reading)
            print_payload_sample(payload)
            publish_to_api(payload)


if __name__ == "__main__":
    print("🚀 EcoCredit — Simulador IoT iniciado")
    print(f"   Intervalo: {PUBLISH_INTERVAL_SECONDS}s")
    print(f"   Instalações: {len(FACILITIES)}")
    print(f"   API: {API_BASE_URL}")
    print(f"   Modo: {'Online' if REQUESTS_AVAILABLE else 'Offline (cache local)'}\n")

    simulate_cycle()
    while True:
        time.sleep(PUBLISH_INTERVAL_SECONDS)
        simulate_cycle()
