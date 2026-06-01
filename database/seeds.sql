-- EcoCredit — Seeds de exemplo para demonstração

INSERT INTO TB_COMPANY (company_id, name, cnpj, sector)
VALUES ('comp-001', 'Petro Energia S.A.', '12.345.678/0001-90', 'OIL_GAS');

INSERT INTO TB_COMPANY (company_id, name, cnpj, sector)
VALUES ('comp-002', 'AgroVerde Ltda.', '98.765.432/0001-10', 'AGRO');

INSERT INTO TB_USER (user_id, company_id, email, password_hash, role)
VALUES ('user-001', 'comp-001', 'admin@petro.com',
    '$2b$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/lewFBwbodMdCEhFhq', 'ADMIN');

INSERT INTO TB_USER (user_id, company_id, email, password_hash, role)
VALUES ('user-002', 'comp-001', 'analista@petro.com',
    '$2b$12$LQv3c1yqBWVHxkd0LHAkCOYz6TtxMQJqhN8/lewFBwbodMdCEhFhq', 'ANALYST');

INSERT INTO TB_FACILITY (facility_id, company_id, name, facility_type, latitude, longitude, emission_limit_tco2)
VALUES ('fac-001', 'comp-001', 'Refinaria RJ', 'REFINERY', -22.9068, -43.1729, 5000.00);

INSERT INTO TB_FACILITY (facility_id, company_id, name, facility_type, latitude, longitude, emission_limit_tco2)
VALUES ('fac-002', 'comp-001', 'Plataforma P-51', 'RIG', -23.5505, -43.1729, 2000.00);

INSERT INTO TB_FACILITY (facility_id, company_id, name, facility_type, latitude, longitude, emission_limit_tco2)
VALUES ('fac-003', 'comp-002', 'Fazenda Cerrado MT', 'FARM', -15.5989, -56.0949, 1200.00);

INSERT INTO TB_EMISSION_RECORD (facility_id, gas_type, quantity_tco2e, source, sensor_id, raw_ppm)
VALUES ('fac-001', 'CO2', 4250.00, 'IOT_SENSOR', 'sensor-co2-fac-001', 1087.5);

INSERT INTO TB_EMISSION_RECORD (facility_id, gas_type, quantity_tco2e, source, sensor_id, raw_ppm)
VALUES ('fac-001', 'CH4', 125.00, 'IOT_SENSOR', 'sensor-ch4-fac-001', 2.8);

INSERT INTO TB_EMISSION_RECORD (facility_id, gas_type, quantity_tco2e, source, sensor_id, raw_ppm)
VALUES ('fac-002', 'CO2', 1200.00, 'IOT_SENSOR', 'sensor-co2-fac-002', 875.0);

INSERT INTO TB_CARBON_CREDIT (company_id, quantity_tco2, credit_type, price_usd, expiry_date, registry_code)
VALUES ('comp-001', 2000.00, 'CBIO', 15.50, DATE '2026-12-31', 'CBIO-2024-001234');

INSERT INTO TB_CARBON_CREDIT (company_id, quantity_tco2, credit_type, price_usd, expiry_date, registry_code)
VALUES ('comp-001', 1500.00, 'REDD', 22.00, DATE '2027-06-30', 'REDD+-BRA-0089');

INSERT INTO TB_ALERT (facility_id, alert_type, severity, message)
VALUES ('fac-001', 'THRESHOLD', 'HIGH',
    'Refinaria RJ atingiu 85% do limite mensal de emissões.');

INSERT INTO TB_ALERT (facility_id, alert_type, severity, message)
VALUES ('fac-001', 'CREDIT_LOW', 'MEDIUM',
    'Saldo de créditos de carbono abaixo de 20% da meta anual.');

COMMIT;
