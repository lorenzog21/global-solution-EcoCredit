-- EcoCredit — Schema SQL
-- Compatível com Oracle 19c+ / SQL Server 2019+

-- =============================================
-- TABELA: COMPANY (Tenant principal)
-- =============================================
CREATE TABLE TB_COMPANY (
    company_id    VARCHAR2(36)   DEFAULT SYS_GUID() PRIMARY KEY,
    name          VARCHAR2(200)  NOT NULL,
    cnpj          VARCHAR2(18)   UNIQUE NOT NULL,
    sector        VARCHAR2(20)   NOT NULL
                  CHECK (sector IN ('OIL_GAS','ENERGY','AGRO','MINING','INDUSTRY')),
    country       VARCHAR2(100)  DEFAULT 'Brazil',
    active        NUMBER(1)      DEFAULT 1 NOT NULL,
    created_at    TIMESTAMP      DEFAULT SYSTIMESTAMP NOT NULL
);

-- =============================================
-- TABELA: TB_USER
-- =============================================
CREATE TABLE TB_USER (
    user_id       VARCHAR2(36)   DEFAULT SYS_GUID() PRIMARY KEY,
    company_id    VARCHAR2(36)   NOT NULL,
    email         VARCHAR2(200)  UNIQUE NOT NULL,
    password_hash VARCHAR2(255)  NOT NULL,
    role          VARCHAR2(10)   NOT NULL
                  CHECK (role IN ('ADMIN','ANALYST','VIEWER')),
    active        NUMBER(1)      DEFAULT 1 NOT NULL,
    created_at    TIMESTAMP      DEFAULT SYSTIMESTAMP NOT NULL,
    CONSTRAINT fk_user_company FOREIGN KEY (company_id) REFERENCES TB_COMPANY(company_id)
);

-- =============================================
-- TABELA: TB_FACILITY (Instalações industriais)
-- =============================================
CREATE TABLE TB_FACILITY (
    facility_id         VARCHAR2(36)    DEFAULT SYS_GUID() PRIMARY KEY,
    company_id          VARCHAR2(36)    NOT NULL,
    name                VARCHAR2(200)   NOT NULL,
    facility_type       VARCHAR2(20)    NOT NULL
                        CHECK (facility_type IN ('PLANT','FARM','MINE','RIG','FLEET','REFINERY')),
    latitude            NUMBER(10,6),
    longitude           NUMBER(10,6),
    emission_limit_tco2 NUMBER(14,2)    NOT NULL,
    active              NUMBER(1)       DEFAULT 1 NOT NULL,
    created_at          TIMESTAMP       DEFAULT SYSTIMESTAMP NOT NULL,
    CONSTRAINT fk_facility_company FOREIGN KEY (company_id) REFERENCES TB_COMPANY(company_id)
);

-- =============================================
-- TABELA: TB_EMISSION_RECORD
-- =============================================
CREATE TABLE TB_EMISSION_RECORD (
    record_id           VARCHAR2(36)    DEFAULT SYS_GUID() PRIMARY KEY,
    facility_id         VARCHAR2(36)    NOT NULL,
    gas_type            VARCHAR2(10)    NOT NULL
                        CHECK (gas_type IN ('CO2','CH4','N2O','HFC','SF6')),
    quantity_tco2e      NUMBER(14,4)    NOT NULL,
    source              VARCHAR2(15)    NOT NULL
                        CHECK (source IN ('IOT_SENSOR','SATELLITE','MANUAL')),
    sensor_id           VARCHAR2(100),
    raw_ppm             NUMBER(10,2),
    satellite_co2_regional NUMBER(10,2),
    recorded_at         TIMESTAMP       DEFAULT SYSTIMESTAMP NOT NULL,
    CONSTRAINT fk_emission_facility FOREIGN KEY (facility_id) REFERENCES TB_FACILITY(facility_id)
);

CREATE INDEX idx_emission_facility_date ON TB_EMISSION_RECORD(facility_id, recorded_at);

-- =============================================
-- TABELA: TB_CARBON_CREDIT
-- =============================================
CREATE TABLE TB_CARBON_CREDIT (
    credit_id       VARCHAR2(36)    DEFAULT SYS_GUID() PRIMARY KEY,
    company_id      VARCHAR2(36)    NOT NULL,
    quantity_tco2   NUMBER(14,2)    NOT NULL,
    credit_type     VARCHAR2(10)    NOT NULL
                    CHECK (credit_type IN ('VCM','REDD','EU_ETS','CBIO','GOLD')),
    status          VARCHAR2(10)    DEFAULT 'AVAILABLE' NOT NULL
                    CHECK (status IN ('AVAILABLE','USED','EXPIRED')),
    price_usd       NUMBER(10,2),
    issued_date     DATE            DEFAULT SYSDATE,
    expiry_date     DATE            NOT NULL,
    registry_code   VARCHAR2(100),
    CONSTRAINT fk_credit_company FOREIGN KEY (company_id) REFERENCES TB_COMPANY(company_id)
);

-- =============================================
-- TABELA: TB_ALERT
-- =============================================
CREATE TABLE TB_ALERT (
    alert_id        VARCHAR2(36)    DEFAULT SYS_GUID() PRIMARY KEY,
    facility_id     VARCHAR2(36)    NOT NULL,
    alert_type      VARCHAR2(20)    NOT NULL
                    CHECK (alert_type IN ('THRESHOLD','CREDIT_LOW','REGULATORY','SENSOR_FAIL')),
    severity        VARCHAR2(10)    NOT NULL
                    CHECK (severity IN ('LOW','MEDIUM','HIGH','CRITICAL')),
    message         VARCHAR2(500)   NOT NULL,
    resolved        NUMBER(1)       DEFAULT 0 NOT NULL,
    resolved_at     TIMESTAMP,
    resolved_note   VARCHAR2(500),
    created_at      TIMESTAMP       DEFAULT SYSTIMESTAMP NOT NULL,
    CONSTRAINT fk_alert_facility FOREIGN KEY (facility_id) REFERENCES TB_FACILITY(facility_id)
);

-- =============================================
-- ÍNDICES ADICIONAIS
-- =============================================
CREATE INDEX idx_alert_facility   ON TB_ALERT(facility_id, resolved);
CREATE INDEX idx_credit_company   ON TB_CARBON_CREDIT(company_id, status);
CREATE INDEX idx_user_company     ON TB_USER(company_id);
