-- EcoCredit — Consultas de simulação e relatório
-- Compatível com Oracle 19c+

-- CONSULTA 1: Emissão total por empresa no mês atual vs. limite
SELECT
    c.name                          AS empresa,
    c.sector                        AS setor,
    ROUND(SUM(er.quantity_tco2e), 2) AS total_emitido_tco2e,
    ROUND(SUM(f.emission_limit_tco2), 2) AS limite_mensal_tco2,
    ROUND(
        (SUM(er.quantity_tco2e) / NULLIF(SUM(f.emission_limit_tco2), 0)) * 100,
        1
    )                               AS percentual_utilizado,
    CASE
        WHEN (SUM(er.quantity_tco2e) / NULLIF(SUM(f.emission_limit_tco2), 0)) >= 1 THEN 'CRITICO'
        WHEN (SUM(er.quantity_tco2e) / NULLIF(SUM(f.emission_limit_tco2), 0)) >= 0.8 THEN 'ALERTA'
        ELSE 'NORMAL'
    END                             AS status_compliance
FROM TB_EMISSION_RECORD er
JOIN TB_FACILITY f         ON f.facility_id = er.facility_id
JOIN TB_COMPANY c          ON c.company_id  = f.company_id
WHERE TRUNC(er.recorded_at, 'MM') = TRUNC(SYSTIMESTAMP, 'MM')
GROUP BY c.name, c.sector
ORDER BY percentual_utilizado DESC;

-- CONSULTA 2: Saldo de créditos de carbono vs. emissões do mês
SELECT
    c.name                  AS empresa,
    COALESCE(SUM(CASE WHEN cc.status = 'AVAILABLE' THEN cc.quantity_tco2 END), 0) AS creditos_disponiveis,
    COALESCE(SUM(er.quantity_tco2e), 0) AS emissoes_mes,
    COALESCE(SUM(CASE WHEN cc.status = 'AVAILABLE' THEN cc.quantity_tco2 END), 0)
        - COALESCE(SUM(er.quantity_tco2e), 0) AS saldo_creditos,
    CASE
        WHEN COALESCE(SUM(CASE WHEN cc.status = 'AVAILABLE' THEN cc.quantity_tco2 END), 0)
             - COALESCE(SUM(er.quantity_tco2e), 0) < 0 THEN 'NAO_CONFORME'
        ELSE 'CONFORME'
    END AS status_esg
FROM TB_COMPANY c
LEFT JOIN TB_CARBON_CREDIT cc ON cc.company_id = c.company_id
LEFT JOIN TB_FACILITY f       ON f.company_id  = c.company_id
LEFT JOIN TB_EMISSION_RECORD er ON er.facility_id = f.facility_id
    AND TRUNC(er.recorded_at, 'MM') = TRUNC(SYSTIMESTAMP, 'MM')
GROUP BY c.name;

-- CONSULTA 3: Alertas críticos ativos com detalhes de instalação
SELECT
    a.alert_type,
    a.severity,
    a.message,
    f.name          AS instalacao,
    c.name          AS empresa,
    a.created_at
FROM TB_ALERT a
JOIN TB_FACILITY f  ON f.facility_id = a.facility_id
JOIN TB_COMPANY c   ON c.company_id  = f.company_id
WHERE a.resolved = 0
  AND a.severity IN ('HIGH', 'CRITICAL')
ORDER BY a.created_at DESC;

-- CONSULTA 4: Emissão por tipo de gás no mês (para gráfico mobile)
SELECT
    er.gas_type,
    ROUND(SUM(er.quantity_tco2e), 2) AS total_tco2e,
    COUNT(*)                          AS num_registros
FROM TB_EMISSION_RECORD er
WHERE TRUNC(er.recorded_at, 'MM') = TRUNC(SYSTIMESTAMP, 'MM')
GROUP BY er.gas_type
ORDER BY total_tco2e DESC;

-- CONSULTA 5: Ranking de instalações por % do limite atingido
SELECT
    f.name              AS instalacao,
    f.facility_type,
    f.emission_limit_tco2 AS limite_mensal,
    ROUND(SUM(er.quantity_tco2e), 2) AS emitido,
    ROUND((SUM(er.quantity_tco2e) / f.emission_limit_tco2) * 100, 1) AS pct_limite
FROM TB_FACILITY f
LEFT JOIN TB_EMISSION_RECORD er ON er.facility_id = f.facility_id
    AND TRUNC(er.recorded_at, 'MM') = TRUNC(SYSTIMESTAMP, 'MM')
WHERE f.active = 1
GROUP BY f.name, f.facility_type, f.emission_limit_tco2
ORDER BY pct_limite DESC NULLS LAST;
