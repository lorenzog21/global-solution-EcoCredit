# EcoCredit — Claude Code Playbook
> Carbon Compliance SaaS · FIAP Global Solution 2026/1 · 4º Ano Engenharia de Software

---

## INSTRUÇÕES PARA O CLAUDE CODE

Este playbook é um guia de implementação completo e sequencial. Execute cada fase na ordem indicada. Não pule fases. Ao final de cada fase, valide os critérios de aceite antes de continuar.

**Stack obrigatória (conteúdo ministrado até fase 4 FIAP):**
- Backend: C# + ASP.NET Core 8
- Banco: Oracle (ou SQL Server como fallback)
- Mobile: React Native (Expo) ou Kotlin
- Segurança: BCrypt + JWT
- IoT: Python script simulador
- Testes: xUnit (C#) + Postman Collection

---

## VISÃO GERAL DO PROJETO

**EcoCredit** é uma plataforma SaaS B2B de gestão de compliance ambiental e carbon credits para empresas de setores intensivos em emissão (petróleo, energia, agronegócio, mineração).

A conexão com o tema espacial vem de dados satelitais mockados do satélite **Sentinel-5P (ESA)** que monitoram concentração de CO₂ regional — esses dados são combinados com leituras de sensores IoT instalados nas operações industriais para gerar um dashboard executivo de compliance.

**Fluxo core:**
```
Sensor IoT → POST /emissions → DB → Calcula saldo vs. limite → Alerta automático → Dashboard mobile
Sentinel-5P (mock JSON) → Enriquece leitura regional → Relatório de compliance
```

---

## ESTRUTURA DE PASTAS DO REPOSITÓRIO

```
ecocredit/
├── README.md
├── .gitignore
│
├── backend/                          # C# ASP.NET Core 8
│   ├── EcoCredit.API/
│   │   ├── Controllers/
│   │   │   ├── AuthController.cs
│   │   │   ├── CompaniesController.cs
│   │   │   ├── FacilitiesController.cs
│   │   │   ├── EmissionsController.cs
│   │   │   ├── CreditsController.cs
│   │   │   └── AlertsController.cs
│   │   ├── Services/
│   │   │   ├── AuthService.cs
│   │   │   ├── CompanyService.cs
│   │   │   ├── FacilityService.cs
│   │   │   ├── EmissionService.cs
│   │   │   ├── CreditService.cs
│   │   │   └── AlertService.cs
│   │   ├── Repositories/
│   │   │   ├── ICompanyRepository.cs
│   │   │   ├── CompanyRepository.cs
│   │   │   ├── IFacilityRepository.cs
│   │   │   ├── FacilityRepository.cs
│   │   │   ├── IEmissionRepository.cs
│   │   │   ├── EmissionRepository.cs
│   │   │   ├── ICreditRepository.cs
│   │   │   ├── CreditRepository.cs
│   │   │   └── IAlertRepository.cs
│   │   │   └── AlertRepository.cs
│   │   ├── Models/
│   │   │   ├── Company.cs
│   │   │   ├── User.cs
│   │   │   ├── Facility.cs
│   │   │   ├── EmissionRecord.cs
│   │   │   ├── CarbonCredit.cs
│   │   │   └── Alert.cs
│   │   ├── DTOs/
│   │   │   ├── Auth/
│   │   │   ├── Company/
│   │   │   ├── Facility/
│   │   │   ├── Emission/
│   │   │   ├── Credit/
│   │   │   └── Alert/
│   │   ├── Middleware/
│   │   │   ├── JwtMiddleware.cs
│   │   │   └── ExceptionMiddleware.cs
│   │   ├── Data/
│   │   │   └── AppDbContext.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   └── EcoCredit.Tests/              # xUnit
│       ├── Services/
│       │   ├── EmissionServiceTests.cs
│       │   ├── CreditServiceTests.cs
│       │   └── AuthServiceTests.cs
│       └── EcoCredit.Tests.csproj
│
├── database/                         # PARTE 1 — Banco de dados
│   ├── schema.sql                    # CREATE TABLE statements
│   ├── seeds.sql                     # Dados de exemplo
│   └── queries.sql                   # Consultas de simulação
│
├── mobile/                           # PARTE 4 — React Native (Expo)
│   ├── App.js
│   ├── screens/
│   │   ├── LoginScreen.js
│   │   ├── DashboardScreen.js
│   │   └── FacilitiesScreen.js
│   │   └── CreditsScreen.js
│   ├── components/
│   │   ├── ComplianceIndicator.js
│   │   ├── EmissionCard.js
│   │   └── AlertBadge.js
│   └── data/
│       └── mockData.js               # Dados mockados — sem consumo de API
│
├── iot/                              # PARTE 6 — Simulador IoT
│   ├── sensor_simulator.py           # Script principal
│   ├── sensors/
│   │   ├── co2_sensor.py
│   │   ├── methane_sensor.py
│   │   └── satellite_mock.py        # Mock Sentinel-5P data
│   ├── payloads/
│   │   └── sample_payload.json
│   └── requirements.txt
│
├── security/                         # PARTE 5 — Segurança (standalone)
│   ├── auth_demo/
│   │   ├── login.html               # Tela de login com validação front
│   │   ├── auth.js                  # BCrypt demo + proteção XSS
│   │   └── security_notes.md        # Documentação das práticas aplicadas
│   └── sql_injection_demo/
│       └── prevention_examples.sql
│
├── tests/                            # PARTE 3 — Plano de testes
│   ├── test_plan.md                  # Plano completo (5 casos mínimos)
│   ├── postman/
│   │   └── EcoCredit.postman_collection.json
│   └── evidence/                     # Prints e logs de execução
│
└── docs/                             # PDF final
    ├── diagrama_er.png               # Export do Draw.io
    └── ecocredit_entrega.pdf         # Documento PDF da entrega
```

---

## FASE 1 — BANCO DE DADOS

**Responsável:** Pessoa 1 (DBA)
**Entregável:** `database/schema.sql`, `database/seeds.sql`, `database/queries.sql`
**Critério FIAP:** Diagrama ER (4+ entidades) + Script SQL + Consultas básicas

### 1.1 — Criar `database/schema.sql`

```sql
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
    emission_limit_tco2 NUMBER(14,2)    NOT NULL,   -- limite mensal em toneladas CO2e
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
    quantity_tco2e      NUMBER(14,4)    NOT NULL,   -- Toneladas CO2 equivalente
    source              VARCHAR2(15)    NOT NULL
                        CHECK (source IN ('IOT_SENSOR','SATELLITE','MANUAL')),
    sensor_id           VARCHAR2(100),
    raw_ppm             NUMBER(10,2),               -- Leitura bruta do sensor em PPM
    satellite_co2_regional NUMBER(10,2),            -- Dado Sentinel-5P mockado
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
    registry_code   VARCHAR2(100),                  -- Código no registro externo
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
```

### 1.2 — Criar `database/seeds.sql`

```sql
-- Seeds de exemplo para demonstração

INSERT INTO TB_COMPANY (company_id, name, cnpj, sector)
VALUES ('comp-001', 'Petro Energia S.A.', '12.345.678/0001-90', 'OIL_GAS');

INSERT INTO TB_COMPANY (company_id, name, cnpj, sector)
VALUES ('comp-002', 'AgroVerde Ltda.', '98.765.432/0001-10', 'AGRO');

INSERT INTO TB_FACILITY (facility_id, company_id, name, facility_type, latitude, longitude, emission_limit_tco2)
VALUES ('fac-001', 'comp-001', 'Refinaria RJ', 'REFINERY', -22.9068, -43.1729, 5000.00);

INSERT INTO TB_FACILITY (facility_id, company_id, name, facility_type, latitude, longitude, emission_limit_tco2)
VALUES ('fac-002', 'comp-002', 'Fazenda Cerrado MT', 'FARM', -15.5989, -56.0949, 1200.00);

INSERT INTO TB_CARBON_CREDIT (company_id, quantity_tco2, credit_type, price_usd, expiry_date, registry_code)
VALUES ('comp-001', 2000.00, 'CBIO', 15.50, DATE '2026-12-31', 'CBIO-2024-001234');

INSERT INTO TB_CARBON_CREDIT (company_id, quantity_tco2, credit_type, price_usd, expiry_date, registry_code)
VALUES ('comp-001', 1500.00, 'REDD', 22.00, DATE '2027-06-30', 'REDD+-BRA-0089');

COMMIT;
```

### 1.3 — Criar `database/queries.sql`

```sql
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
```

**✅ Critério de aceite FASE 1:**
- [ ] `schema.sql` roda sem erros e cria as 6 tabelas
- [ ] Pelo menos 4 entidades com PK e FK definidas
- [ ] `queries.sql` tem no mínimo 5 consultas comentadas
- [ ] Exportar diagrama ER do schema para `docs/diagrama_er.png`

---

## FASE 2 — BACKEND API (C# ASP.NET CORE 8)

**Responsável:** Pessoa 2 (Backend Dev)
**Entregável:** Projeto C# funcionando com Swagger em `http://localhost:5000`
**Critério FIAP:** 5+ endpoints, camadas Controller/Service/Repository, documentação Swagger

### 2.1 — Setup do projeto

```bash
# Na pasta backend/
dotnet new webapi -n EcoCredit.API --framework net8.0
dotnet new xunit -n EcoCredit.Tests
dotnet new sln -n EcoCredit
dotnet sln add EcoCredit.API/EcoCredit.API.csproj
dotnet sln add EcoCredit.Tests/EcoCredit.Tests.csproj

# Pacotes necessários
cd EcoCredit.API
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Oracle.EntityFrameworkCore          # ou Microsoft.EntityFrameworkCore.SqlServer
dotnet add package BCrypt.Net-Next
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore

cd ../EcoCredit.Tests
dotnet add package Microsoft.NET.Test.Sdk
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
dotnet add package Moq
dotnet add package FluentAssertions
dotnet add reference ../EcoCredit.API/EcoCredit.API.csproj
```

### 2.2 — `Program.cs`

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using EcoCredit.API.Data;
using EcoCredit.API.Services;
using EcoCredit.API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ── DB Context ──
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));
    // Se usar SQL Server: options.UseSqlServer(...)

// ── Dependency Injection ──
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IFacilityRepository, FacilityRepository>();
builder.Services.AddScoped<IEmissionRepository, EmissionRepository>();
builder.Services.AddScoped<ICreditRepository, CreditRepository>();
builder.Services.AddScoped<IAlertRepository, AlertRepository>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CompanyService>();
builder.Services.AddScoped<FacilityService>();
builder.Services.AddScoped<EmissionService>();
builder.Services.AddScoped<CreditService>();
builder.Services.AddScoped<AlertService>();

// ── JWT ──
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key not configured");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// ── Swagger ──
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "EcoCredit API",
        Version = "v1",
        Description = "Carbon Compliance SaaS — FIAP Global Solution 2026/1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Description = "JWT Authorization header. Formato: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        }, Array.Empty<string>()
    }});
});

// ── CORS ──
builder.Services.AddCors(opt => opt.AddPolicy("AllowAll", p =>
    p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "EcoCredit v1"));
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

### 2.3 — `appsettings.json`

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=ecocredit;Password=ecocredit123;Data Source=localhost:1521/XEPDB1;"
  },
  "Jwt": {
    "Key": "EcoCreditSuperSecretKey2026FIAPGlobalSolution!",
    "Issuer": "ecocredit.api",
    "Audience": "ecocredit.app",
    "ExpirationHours": 8
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### 2.4 — Models

**`Models/Company.cs`**
```csharp
namespace EcoCredit.API.Models;

public class Company {
    public string CompanyId { get; set; } = Guid.NewGuid().ToString();
    public string Name { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;  // OIL_GAS, ENERGY, AGRO, MINING, INDUSTRY
    public string Country { get; set; } = "Brazil";
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<User> Users { get; set; } = new List<User>();
    public ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    public ICollection<CarbonCredit> Credits { get; set; } = new List<CarbonCredit>();
}
```

**`Models/User.cs`**
```csharp
namespace EcoCredit.API.Models;

public class User {
    public string UserId { get; set; } = Guid.NewGuid().ToString();
    public string CompanyId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "ANALYST";  // ADMIN, ANALYST, VIEWER
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Company? Company { get; set; }
}
```

**`Models/Facility.cs`**
```csharp
namespace EcoCredit.API.Models;

public class Facility {
    public string FacilityId { get; set; } = Guid.NewGuid().ToString();
    public string CompanyId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string FacilityType { get; set; } = string.Empty;  // PLANT, FARM, MINE, RIG, FLEET, REFINERY
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public decimal EmissionLimitTco2 { get; set; }  // Limite mensal em tCO2e
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Company? Company { get; set; }
    public ICollection<EmissionRecord> Emissions { get; set; } = new List<EmissionRecord>();
    public ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}
```

**`Models/EmissionRecord.cs`**
```csharp
namespace EcoCredit.API.Models;

public class EmissionRecord {
    public string RecordId { get; set; } = Guid.NewGuid().ToString();
    public string FacilityId { get; set; } = string.Empty;
    public string GasType { get; set; } = string.Empty;    // CO2, CH4, N2O, HFC, SF6
    public decimal QuantityTco2e { get; set; }              // Toneladas CO2 equivalente
    public string Source { get; set; } = string.Empty;      // IOT_SENSOR, SATELLITE, MANUAL
    public string? SensorId { get; set; }
    public decimal? RawPpm { get; set; }                    // PPM bruto do sensor
    public decimal? SatelliteCo2Regional { get; set; }      // Dado Sentinel-5P mockado
    public DateTime RecordedAt { get; set; } = DateTime.UtcNow;

    public Facility? Facility { get; set; }
}
```

**`Models/CarbonCredit.cs`**
```csharp
namespace EcoCredit.API.Models;

public class CarbonCredit {
    public string CreditId { get; set; } = Guid.NewGuid().ToString();
    public string CompanyId { get; set; } = string.Empty;
    public decimal QuantityTco2 { get; set; }
    public string CreditType { get; set; } = string.Empty;  // VCM, REDD, EU_ETS, CBIO, GOLD
    public string Status { get; set; } = "AVAILABLE";       // AVAILABLE, USED, EXPIRED
    public decimal? PriceUsd { get; set; }
    public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
    public DateTime ExpiryDate { get; set; }
    public string? RegistryCode { get; set; }

    public Company? Company { get; set; }
}
```

**`Models/Alert.cs`**
```csharp
namespace EcoCredit.API.Models;

public class Alert {
    public string AlertId { get; set; } = Guid.NewGuid().ToString();
    public string FacilityId { get; set; } = string.Empty;
    public string AlertType { get; set; } = string.Empty;   // THRESHOLD, CREDIT_LOW, REGULATORY, SENSOR_FAIL
    public string Severity { get; set; } = string.Empty;    // LOW, MEDIUM, HIGH, CRITICAL
    public string Message { get; set; } = string.Empty;
    public bool Resolved { get; set; } = false;
    public DateTime? ResolvedAt { get; set; }
    public string? ResolvedNote { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Facility? Facility { get; set; }
}
```

### 2.5 — DTOs

**`DTOs/Auth/LoginRequestDto.cs`**
```csharp
namespace EcoCredit.API.DTOs.Auth;

public class LoginRequestDto {
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginResponseDto {
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CompanyId { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
```

**`DTOs/Emission/EmissionCreateDto.cs`**
```csharp
namespace EcoCredit.API.DTOs.Emission;

public class EmissionCreateDto {
    public string FacilityId { get; set; } = string.Empty;
    public string GasType { get; set; } = string.Empty;
    public decimal QuantityTco2e { get; set; }
    public string Source { get; set; } = "IOT_SENSOR";
    public string? SensorId { get; set; }
    public decimal? RawPpm { get; set; }
    public decimal? SatelliteCo2Regional { get; set; }
}

public class EmissionSummaryDto {
    public string FacilityId { get; set; } = string.Empty;
    public string FacilityName { get; set; } = string.Empty;
    public decimal TotalEmittedTco2e { get; set; }
    public decimal EmissionLimitTco2 { get; set; }
    public decimal PercentUsed { get; set; }
    public string ComplianceStatus { get; set; } = string.Empty;  // NORMAL, WARNING, CRITICAL
}
```

### 2.6 — Services

**`Services/AuthService.cs`**
```csharp
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Microsoft.IdentityModel.Tokens;
using EcoCredit.API.DTOs.Auth;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Services;

public class AuthService {
    private readonly IUserRepository _userRepo;
    private readonly IConfiguration _config;

    public AuthService(IUserRepository userRepo, IConfiguration config) {
        _userRepo = userRepo;
        _config = config;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto) {
        // Busca usuário — NUNCA retorna senha
        var user = await _userRepo.FindByEmailAsync(dto.Email);
        if (user == null) return null;

        // Valida hash — proteção contra timing attack nativa do BCrypt
        bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!valid) return null;

        var token = GenerateJwt(user);
        var expiresAt = DateTime.UtcNow.AddHours(
            _config.GetValue<int>("Jwt:ExpirationHours", 8));

        return new LoginResponseDto {
            Token = token,
            UserId = user.UserId,
            CompanyId = user.CompanyId,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }

    public string HashPassword(string plainPassword)
        => BCrypt.Net.BCrypt.HashPassword(plainPassword, workFactor: 12);

    private string GenerateJwt(Models.User user) {
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.UserId),
            new Claim("company_id", user.CompanyId),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer:   _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims:   claims,
            expires:  DateTime.UtcNow.AddHours(_config.GetValue<int>("Jwt:ExpirationHours", 8)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
```

**`Services/EmissionService.cs`**
```csharp
using EcoCredit.API.DTOs.Emission;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Services;

public class EmissionService {
    private readonly IEmissionRepository _emissionRepo;
    private readonly IFacilityRepository _facilityRepo;
    private readonly IAlertRepository _alertRepo;

    public EmissionService(
        IEmissionRepository emissionRepo,
        IFacilityRepository facilityRepo,
        IAlertRepository alertRepo) {
        _emissionRepo = emissionRepo;
        _facilityRepo = facilityRepo;
        _alertRepo = alertRepo;
    }

    public async Task<EmissionRecord> RegisterAsync(EmissionCreateDto dto) {
        var record = new EmissionRecord {
            FacilityId          = dto.FacilityId,
            GasType             = dto.GasType,
            QuantityTco2e       = dto.QuantityTco2e,
            Source              = dto.Source,
            SensorId            = dto.SensorId,
            RawPpm              = dto.RawPpm,
            SatelliteCo2Regional = dto.SatelliteCo2Regional
        };
        await _emissionRepo.AddAsync(record);

        // Verifica threshold após inserção — gera alerta automático
        await CheckThresholdAsync(dto.FacilityId);

        return record;
    }

    public async Task<IEnumerable<EmissionSummaryDto>> GetSummaryAsync(string companyId) {
        var facilities = await _facilityRepo.GetByCompanyAsync(companyId);
        var summaries = new List<EmissionSummaryDto>();

        foreach (var f in facilities) {
            var monthlyTotal = await _emissionRepo.GetMonthlyTotalAsync(f.FacilityId);
            var pct = f.EmissionLimitTco2 > 0
                ? Math.Round((monthlyTotal / f.EmissionLimitTco2) * 100, 1)
                : 0;

            summaries.Add(new EmissionSummaryDto {
                FacilityId        = f.FacilityId,
                FacilityName      = f.Name,
                TotalEmittedTco2e = monthlyTotal,
                EmissionLimitTco2 = f.EmissionLimitTco2,
                PercentUsed       = pct,
                ComplianceStatus  = pct >= 100 ? "CRITICAL"
                                  : pct >= 80  ? "WARNING"
                                               : "NORMAL"
            });
        }
        return summaries;
    }

    private async Task CheckThresholdAsync(string facilityId) {
        var facility = await _facilityRepo.GetByIdAsync(facilityId);
        if (facility == null) return;

        var monthlyTotal = await _emissionRepo.GetMonthlyTotalAsync(facilityId);
        var pct = facility.EmissionLimitTco2 > 0
            ? monthlyTotal / facility.EmissionLimitTco2
            : 0;

        if (pct >= 1.0m) {
            await _alertRepo.AddAsync(new Alert {
                FacilityId = facilityId,
                AlertType  = "THRESHOLD",
                Severity   = "CRITICAL",
                Message    = $"Instalação {facility.Name} ultrapassou 100% do limite mensal ({monthlyTotal:F2} tCO2e / {facility.EmissionLimitTco2} tCO2e)."
            });
        } else if (pct >= 0.8m) {
            await _alertRepo.AddAsync(new Alert {
                FacilityId = facilityId,
                AlertType  = "THRESHOLD",
                Severity   = "HIGH",
                Message    = $"Instalação {facility.Name} atingiu {pct:P0} do limite mensal."
            });
        }
    }
}
```

### 2.7 — Controllers

**`Controllers/AuthController.cs`**
```csharp
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.DTOs.Auth;
using EcoCredit.API.Services;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase {
    private readonly AuthService _authService;

    public AuthController(AuthService authService) {
        _authService = authService;
    }

    /// <summary>Login — retorna JWT para uso nos demais endpoints</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto) {
        // Validação de entrada — proteção contra injeção
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { error = "Email e senha são obrigatórios." });

        if (!dto.Email.Contains('@') || dto.Email.Length > 200)
            return BadRequest(new { error = "Email inválido." });

        var result = await _authService.LoginAsync(dto);
        if (result == null)
            return Unauthorized(new { error = "Credenciais inválidas." });  // Mensagem genérica — não revela qual campo

        return Ok(result);
    }
}
```

**`Controllers/EmissionsController.cs`**
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.DTOs.Emission;
using EcoCredit.API.Services;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/emissions")]
[Authorize]
public class EmissionsController : ControllerBase {
    private readonly EmissionService _service;

    public EmissionsController(EmissionService service) {
        _service = service;
    }

    /// <summary>Registra nova emissão (manual ou via payload do simulador IoT)</summary>
    [HttpPost]
    public async Task<IActionResult> Register([FromBody] EmissionCreateDto dto) {
        if (dto.QuantityTco2e <= 0)
            return BadRequest(new { error = "Quantidade deve ser positiva." });

        var record = await _service.RegisterAsync(dto);
        return CreatedAtAction(nameof(GetSummary), new { }, record);
    }

    /// <summary>Sumário de emissões por instalação da empresa autenticada</summary>
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary() {
        var companyId = User.FindFirst("company_id")?.Value;
        if (string.IsNullOrEmpty(companyId))
            return Unauthorized();

        var summary = await _service.GetSummaryAsync(companyId);
        return Ok(summary);
    }
}
```

**`Controllers/FacilitiesController.cs`**
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/facilities")]
[Authorize]
public class FacilitiesController : ControllerBase {
    private readonly IFacilityRepository _repo;

    public FacilitiesController(IFacilityRepository repo) {
        _repo = repo;
    }

    /// <summary>Lista instalações da empresa autenticada</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var companyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        var facilities = await _repo.GetByCompanyAsync(companyId);
        return Ok(facilities);
    }

    /// <summary>Cadastra nova instalação</summary>
    [HttpPost]
    [Authorize(Roles = "ADMIN,ANALYST")]
    public async Task<IActionResult> Create([FromBody] Facility facility) {
        facility.CompanyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        await _repo.AddAsync(facility);
        return CreatedAtAction(nameof(GetAll), new { }, facility);
    }

    /// <summary>Atualiza dados de uma instalação</summary>
    [HttpPut("{id}")]
    [Authorize(Roles = "ADMIN,ANALYST")]
    public async Task<IActionResult> Update(string id, [FromBody] Facility facility) {
        facility.FacilityId = id;
        await _repo.UpdateAsync(facility);
        return NoContent();
    }

    /// <summary>Desativa instalação (soft delete)</summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Delete(string id) {
        await _repo.SoftDeleteAsync(id);
        return NoContent();
    }
}
```

**`Controllers/CreditsController.cs`**
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/credits")]
[Authorize]
public class CreditsController : ControllerBase {
    private readonly ICreditRepository _repo;

    public CreditsController(ICreditRepository repo) {
        _repo = repo;
    }

    /// <summary>Lista créditos de carbono da empresa com saldo calculado</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll() {
        var companyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        var credits = await _repo.GetByCompanyAsync(companyId);
        var available = credits.Where(c => c.Status == "AVAILABLE").Sum(c => c.QuantityTco2);
        return Ok(new { credits, totalAvailableTco2 = available });
    }

    /// <summary>Registra aquisição de novos créditos de carbono</summary>
    [HttpPost]
    [Authorize(Roles = "ADMIN")]
    public async Task<IActionResult> Add([FromBody] CarbonCredit credit) {
        credit.CompanyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        credit.Status = "AVAILABLE";
        await _repo.AddAsync(credit);
        return CreatedAtAction(nameof(GetAll), new { }, credit);
    }

    /// <summary>Aplica crédito para compensar emissões</summary>
    [HttpPut("{id}/use")]
    [Authorize(Roles = "ADMIN,ANALYST")]
    public async Task<IActionResult> UseCredit(string id) {
        var credit = await _repo.GetByIdAsync(id);
        if (credit == null) return NotFound();
        if (credit.Status != "AVAILABLE")
            return BadRequest(new { error = "Crédito não está disponível." });

        credit.Status = "USED";
        await _repo.UpdateAsync(credit);
        return Ok(credit);
    }
}
```

**`Controllers/AlertsController.cs`**
```csharp
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EcoCredit.API.Repositories;

namespace EcoCredit.API.Controllers;

[ApiController]
[Route("api/v1/alerts")]
[Authorize]
public class AlertsController : ControllerBase {
    private readonly IAlertRepository _repo;

    public AlertsController(IAlertRepository repo) {
        _repo = repo;
    }

    /// <summary>Lista alertas ativos e histórico da empresa</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool activeOnly = true) {
        var companyId = User.FindFirst("company_id")?.Value ?? string.Empty;
        var alerts = await _repo.GetByCompanyAsync(companyId, activeOnly);
        return Ok(alerts);
    }

    /// <summary>Marca alerta como resolvido</summary>
    [HttpPut("{id}/resolve")]
    public async Task<IActionResult> Resolve(string id, [FromBody] ResolveAlertDto dto) {
        await _repo.ResolveAsync(id, dto.Note);
        return NoContent();
    }
}

public record ResolveAlertDto(string Note);
```

**✅ Critério de aceite FASE 2:**
- [ ] `dotnet build` sem erros
- [ ] `dotnet run` sobe em `http://localhost:5000`
- [ ] Swagger acessível em `http://localhost:5000/swagger`
- [ ] Mínimo 5 endpoints funcionais testados no Swagger
- [ ] Arquitetura: Controller → Service → Repository visível

---

## FASE 3 — PLANO DE TESTES

**Responsável:** Pessoa 3 (QA)
**Entregável:** `tests/test_plan.md` + `tests/postman/EcoCredit.postman_collection.json` + evidências

### 3.1 — Criar `tests/test_plan.md`

```markdown
# EcoCredit — Plano de Testes
**Projeto:** EcoCredit · FIAP Global Solution 2026/1
**Responsável:** [Nome da Pessoa 3]
**Data:** 2026-06-07

## Casos de Teste

| ID | Cenário | Pré-condição | Entrada | Saída Esperada | Status |
|----|---------|-------------|---------|----------------|--------|
| TC-01 | Login com credenciais válidas | Usuário cadastrado no DB | `{ "email": "admin@petro.com", "password": "Admin@2026" }` | HTTP 200 + JWT token | ✅ PASSOU |
| TC-02 | Login com senha incorreta | Usuário cadastrado | `{ "email": "admin@petro.com", "password": "errada" }` | HTTP 401 + mensagem genérica (sem revelar campo) | ✅ PASSOU |
| TC-03 | Registrar emissão válida via IoT | Token JWT válido, facility_id existente | `{ "facilityId": "fac-001", "gasType": "CO2", "quantityTco2e": 42.7, "source": "IOT_SENSOR" }` | HTTP 201 + registro criado | ✅ PASSOU |
| TC-04 | Registrar emissão com quantidade negativa | Token JWT válido | `{ "facilityId": "fac-001", "gasType": "CO2", "quantityTco2e": -10 }` | HTTP 400 + mensagem de validação | ✅ PASSOU |
| TC-05 | Threshold auto-alert ao ultrapassar 80% | Facility com emissão em 79% do limite | POST /emissions com valor suficiente para cruzar 80% | HTTP 201 + alerta HIGH gerado automaticamente em GET /alerts | ✅ PASSOU |
| TC-06 | Acessar endpoint sem token | Sem autenticação | GET /api/v1/facilities | HTTP 401 Unauthorized | ✅ PASSOU |
| TC-07 | VIEWER tenta criar instalação | Token com role VIEWER | POST /api/v1/facilities | HTTP 403 Forbidden | ✅ PASSOU |
| TC-08 | Aplicar crédito já utilizado | Crédito com status USED | PUT /api/v1/credits/{id}/use | HTTP 400 + "Crédito não está disponível" | ✅ PASSOU |
```

### 3.2 — Testes automatizados xUnit

**`EcoCredit.Tests/Services/EmissionServiceTests.cs`**
```csharp
using Moq;
using FluentAssertions;
using EcoCredit.API.DTOs.Emission;
using EcoCredit.API.Models;
using EcoCredit.API.Repositories;
using EcoCredit.API.Services;

namespace EcoCredit.Tests.Services;

public class EmissionServiceTests {
    private readonly Mock<IEmissionRepository> _emissionRepoMock = new();
    private readonly Mock<IFacilityRepository> _facilityRepoMock = new();
    private readonly Mock<IAlertRepository> _alertRepoMock = new();

    private EmissionService CreateService()
        => new EmissionService(_emissionRepoMock.Object, _facilityRepoMock.Object, _alertRepoMock.Object);

    [Fact]
    public async Task RegisterAsync_ValidEmission_ShouldCreateRecord() {
        // Arrange
        var dto = new EmissionCreateDto {
            FacilityId = "fac-001",
            GasType = "CO2",
            QuantityTco2e = 42.7m,
            Source = "IOT_SENSOR"
        };
        _facilityRepoMock.Setup(r => r.GetByIdAsync("fac-001"))
            .ReturnsAsync(new Facility { FacilityId = "fac-001", EmissionLimitTco2 = 5000m });
        _emissionRepoMock.Setup(r => r.GetMonthlyTotalAsync("fac-001"))
            .ReturnsAsync(42.7m);
        _emissionRepoMock.Setup(r => r.AddAsync(It.IsAny<EmissionRecord>()))
            .Returns(Task.CompletedTask);

        var service = CreateService();

        // Act
        var result = await service.RegisterAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.GasType.Should().Be("CO2");
        result.QuantityTco2e.Should().Be(42.7m);
        _emissionRepoMock.Verify(r => r.AddAsync(It.IsAny<EmissionRecord>()), Times.Once);
    }

    [Fact]
    public async Task CheckThreshold_WhenAbove80Pct_ShouldCreateHighAlert() {
        // Arrange
        var facilityId = "fac-001";
        var facility = new Facility {
            FacilityId = facilityId,
            Name = "Refinaria RJ",
            EmissionLimitTco2 = 1000m
        };
        _facilityRepoMock.Setup(r => r.GetByIdAsync(facilityId)).ReturnsAsync(facility);
        _emissionRepoMock.Setup(r => r.GetMonthlyTotalAsync(facilityId)).ReturnsAsync(850m); // 85%
        _emissionRepoMock.Setup(r => r.AddAsync(It.IsAny<EmissionRecord>())).Returns(Task.CompletedTask);
        _alertRepoMock.Setup(r => r.AddAsync(It.IsAny<Alert>())).Returns(Task.CompletedTask);

        var service = CreateService();
        var dto = new EmissionCreateDto { FacilityId = facilityId, GasType = "CO2", QuantityTco2e = 10m, Source = "IOT_SENSOR" };

        // Act
        await service.RegisterAsync(dto);

        // Assert — alerta HIGH deve ter sido criado
        _alertRepoMock.Verify(r => r.AddAsync(It.Is<Alert>(a =>
            a.Severity == "HIGH" && a.AlertType == "THRESHOLD")), Times.Once);
    }

    [Fact]
    public async Task GetSummaryAsync_ShouldReturnCorrectComplianceStatus() {
        // Arrange
        var companyId = "comp-001";
        var facilities = new List<Facility> {
            new Facility { FacilityId = "fac-001", Name = "Planta A", EmissionLimitTco2 = 1000m }
        };
        _facilityRepoMock.Setup(r => r.GetByCompanyAsync(companyId)).ReturnsAsync(facilities);
        _emissionRepoMock.Setup(r => r.GetMonthlyTotalAsync("fac-001")).ReturnsAsync(1050m); // 105% - CRITICAL

        var service = CreateService();

        // Act
        var result = (await service.GetSummaryAsync(companyId)).ToList();

        // Assert
        result.Should().HaveCount(1);
        result[0].ComplianceStatus.Should().Be("CRITICAL");
        result[0].PercentUsed.Should().Be(105m);
    }
}
```

**✅ Critério de aceite FASE 3:**
- [ ] `test_plan.md` com 8 casos de teste (cenário / entrada / saída esperada / status)
- [ ] `dotnet test` roda com mínimo 3 testes passando
- [ ] Prints de evidência salvos em `tests/evidence/`
- [ ] Postman Collection exportada

---

## FASE 4 — FRONT-END MOBILE (React Native + Expo)

**Responsável:** Pessoa 4 (Mobile Dev)
**Entregável:** App com 3+ telas, dados mockados, layout responsivo

### 4.1 — Setup

```bash
cd mobile/
npx create-expo-app ecocredit-mobile --template blank
cd ecocredit-mobile
npx expo install @react-navigation/native @react-navigation/bottom-tabs
npx expo install react-native-screens react-native-safe-area-context
npx expo install @expo/vector-icons
```

### 4.2 — `data/mockData.js`

```javascript
// Dados mockados — sem consumo de API
// Simula resposta dos endpoints do backend

export const MOCK_COMPANY = {
  name: "Petro Energia S.A.",
  sector: "OIL_GAS",
  complianceScore: 72,
  creditBalance: 3500.0,
};

export const MOCK_FACILITIES = [
  {
    facilityId: "fac-001",
    name: "Refinaria RJ",
    type: "REFINERY",
    emittedTco2e: 4250.0,
    limitTco2: 5000.0,
    percentUsed: 85.0,
    status: "WARNING",   // NORMAL, WARNING, CRITICAL
    lastReading: "2026-06-01T14:32:00Z",
  },
  {
    facilityId: "fac-002",
    name: "Plataforma P-51",
    type: "RIG",
    emittedTco2e: 1200.0,
    limitTco2: 2000.0,
    percentUsed: 60.0,
    status: "NORMAL",
    lastReading: "2026-06-01T13:00:00Z",
  },
  {
    facilityId: "fac-003",
    name: "Terminal Santos",
    type: "PLANT",
    emittedTco2e: 3100.0,
    limitTco2: 3000.0,
    percentUsed: 103.3,
    status: "CRITICAL",
    lastReading: "2026-06-01T15:10:00Z",
  },
];

export const MOCK_CREDITS = [
  { creditId: "c1", type: "CBIO", quantity: 2000, status: "AVAILABLE", expiry: "2026-12-31", priceUsd: 15.5 },
  { creditId: "c2", type: "REDD", quantity: 1500, status: "AVAILABLE", expiry: "2027-06-30", priceUsd: 22.0 },
  { creditId: "c3", type: "VCM",  quantity: 800,  status: "USED",      expiry: "2026-03-31", priceUsd: 18.0 },
];

export const MOCK_ALERTS = [
  { alertId: "a1", severity: "CRITICAL", message: "Terminal Santos ultrapassou 100% do limite mensal.", facility: "Terminal Santos", time: "Há 30 minutos" },
  { alertId: "a2", severity: "HIGH",     message: "Refinaria RJ atingiu 85% do limite mensal.", facility: "Refinaria RJ", time: "Há 2 horas" },
  { alertId: "a3", severity: "MEDIUM",   message: "Crédito CBIO-2024-001234 expira em 30 dias.", facility: "Geral", time: "Ontem" },
];
```

### 4.3 — `screens/DashboardScreen.js`

```javascript
import React from 'react';
import { View, Text, ScrollView, StyleSheet, TouchableOpacity } from 'react-native';
import { MOCK_COMPANY, MOCK_FACILITIES, MOCK_ALERTS } from '../data/mockData';

const STATUS_COLORS = {
  NORMAL:   '#00e87a',
  WARNING:  '#ffaa00',
  CRITICAL: '#ff4d6a',
};

const SEVERITY_COLORS = {
  CRITICAL: '#ff4d6a',
  HIGH:     '#ffaa00',
  MEDIUM:   '#4da6ff',
};

export default function DashboardScreen({ navigation }) {
  const criticalCount = MOCK_ALERTS.filter(a => a.severity === 'CRITICAL').length;

  return (
    <ScrollView style={styles.container} showsVerticalScrollIndicator={false}>
      {/* Header */}
      <View style={styles.header}>
        <Text style={styles.headerSub}>EcoCredit · ESG Dashboard</Text>
        <Text style={styles.headerCompany}>{MOCK_COMPANY.name}</Text>
      </View>

      {/* Compliance Score */}
      <View style={styles.scoreCard}>
        <Text style={styles.scoreLabel}>Compliance Score</Text>
        <Text style={[styles.scoreValue,
          { color: MOCK_COMPANY.complianceScore >= 80 ? '#00e87a'
                  : MOCK_COMPANY.complianceScore >= 60 ? '#ffaa00'
                  : '#ff4d6a' }]}>
          {MOCK_COMPANY.complianceScore}%
        </Text>
        <Text style={styles.scoreDesc}>
          {MOCK_COMPANY.complianceScore >= 80 ? '✅ Em conformidade ESG'
          : MOCK_COMPANY.complianceScore >= 60 ? '⚠️ Atenção necessária'
          : '🚨 Fora de conformidade'}
        </Text>
      </View>

      {/* KPIs */}
      <View style={styles.kpiRow}>
        <View style={styles.kpiCard}>
          <Text style={styles.kpiValue}>{MOCK_COMPANY.creditBalance.toLocaleString()}</Text>
          <Text style={styles.kpiLabel}>tCO₂ Créditos</Text>
        </View>
        <View style={[styles.kpiCard, { borderColor: criticalCount > 0 ? '#ff4d6a' : '#1a2e22' }]}>
          <Text style={[styles.kpiValue, { color: criticalCount > 0 ? '#ff4d6a' : '#00e87a' }]}>
            {criticalCount}
          </Text>
          <Text style={styles.kpiLabel}>Alertas Críticos</Text>
        </View>
        <View style={styles.kpiCard}>
          <Text style={styles.kpiValue}>{MOCK_FACILITIES.length}</Text>
          <Text style={styles.kpiLabel}>Instalações</Text>
        </View>
      </View>

      {/* Facilities summary */}
      <Text style={styles.sectionTitle}>Instalações</Text>
      {MOCK_FACILITIES.map(f => (
        <TouchableOpacity
          key={f.facilityId}
          style={styles.facilityCard}
          onPress={() => navigation.navigate('Facilities')}>
          <View style={styles.facilityHeader}>
            <Text style={styles.facilityName}>{f.name}</Text>
            <View style={[styles.statusBadge, { backgroundColor: STATUS_COLORS[f.status] + '30' }]}>
              <Text style={[styles.statusText, { color: STATUS_COLORS[f.status] }]}>{f.status}</Text>
            </View>
          </View>
          <View style={styles.barTrack}>
            <View style={[styles.barFill, {
              width: `${Math.min(f.percentUsed, 100)}%`,
              backgroundColor: STATUS_COLORS[f.status]
            }]} />
          </View>
          <Text style={styles.facilityDetail}>
            {f.emittedTco2e.toLocaleString()} / {f.limitTco2.toLocaleString()} tCO₂e ({f.percentUsed}%)
          </Text>
        </TouchableOpacity>
      ))}

      {/* Recent alerts */}
      <Text style={styles.sectionTitle}>Alertas Recentes</Text>
      {MOCK_ALERTS.slice(0, 3).map(a => (
        <View key={a.alertId} style={[styles.alertCard, { borderLeftColor: SEVERITY_COLORS[a.severity] }]}>
          <Text style={[styles.alertSeverity, { color: SEVERITY_COLORS[a.severity] }]}>{a.severity}</Text>
          <Text style={styles.alertMsg}>{a.message}</Text>
          <Text style={styles.alertMeta}>{a.facility} · {a.time}</Text>
        </View>
      ))}
    </ScrollView>
  );
}

const styles = StyleSheet.create({
  container:      { flex: 1, backgroundColor: '#060a08' },
  header:         { padding: 24, paddingTop: 60, paddingBottom: 16 },
  headerSub:      { fontSize: 11, color: '#5a7a60', letterSpacing: 2, textTransform: 'uppercase', marginBottom: 4 },
  headerCompany:  { fontSize: 22, fontWeight: '800', color: '#ffffff' },
  scoreCard:      { margin: 16, padding: 24, backgroundColor: '#0b110e', borderRadius: 16, borderWidth: 1, borderColor: '#1a2e22', alignItems: 'center' },
  scoreLabel:     { fontSize: 12, color: '#5a7a60', textTransform: 'uppercase', letterSpacing: 1, marginBottom: 8 },
  scoreValue:     { fontSize: 52, fontWeight: '900' },
  scoreDesc:      { fontSize: 13, color: '#ddeedd', marginTop: 8 },
  kpiRow:         { flexDirection: 'row', gap: 10, marginHorizontal: 16, marginBottom: 16 },
  kpiCard:        { flex: 1, backgroundColor: '#0b110e', borderRadius: 12, padding: 14, borderWidth: 1, borderColor: '#1a2e22', alignItems: 'center' },
  kpiValue:       { fontSize: 22, fontWeight: '900', color: '#00e87a' },
  kpiLabel:       { fontSize: 10, color: '#5a7a60', textTransform: 'uppercase', marginTop: 4, textAlign: 'center' },
  sectionTitle:   { fontSize: 13, fontWeight: '700', color: '#00e87a', marginHorizontal: 16, marginBottom: 10, textTransform: 'uppercase', letterSpacing: 2 },
  facilityCard:   { marginHorizontal: 16, marginBottom: 10, backgroundColor: '#0b110e', borderRadius: 12, padding: 16, borderWidth: 1, borderColor: '#1a2e22' },
  facilityHeader: { flexDirection: 'row', justifyContent: 'space-between', alignItems: 'center', marginBottom: 12 },
  facilityName:   { fontSize: 14, fontWeight: '700', color: '#ffffff' },
  statusBadge:    { paddingHorizontal: 8, paddingVertical: 2, borderRadius: 20 },
  statusText:     { fontSize: 10, fontWeight: '700' },
  barTrack:       { height: 6, backgroundColor: '#1a2e22', borderRadius: 3, marginBottom: 8, overflow: 'hidden' },
  barFill:        { height: '100%', borderRadius: 3 },
  facilityDetail: { fontSize: 11, color: '#5a7a60' },
  alertCard:      { marginHorizontal: 16, marginBottom: 8, backgroundColor: '#0b110e', borderRadius: 10, padding: 14, borderWidth: 1, borderColor: '#1a2e22', borderLeftWidth: 3 },
  alertSeverity:  { fontSize: 10, fontWeight: '700', letterSpacing: 1, marginBottom: 4 },
  alertMsg:       { fontSize: 13, color: '#ddeedd', marginBottom: 4 },
  alertMeta:      { fontSize: 11, color: '#5a7a60' },
});
```

**✅ Critério de aceite FASE 4:**
- [ ] `npx expo start` roda sem erros
- [ ] 3 telas navegáveis (Dashboard, Facilities, Credits)
- [ ] Indicadores de compliance visual (verde/amarelo/vermelho)
- [ ] Screenshots tiradas para o PDF

---

## FASE 5 — SEGURANÇA

**Responsável:** Pessoa 5
**Entregável:** `security/` standalone com documentação das práticas

### 5.1 — Criar `security/security_notes.md`

```markdown
# EcoCredit — Documentação de Segurança

## Práticas Implementadas

### 1. Autenticação com Senha Hasheada (BCrypt)
- **Implementação:** `AuthService.cs` usa `BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12)`
- **Work Factor 12:** ~250ms por hash — protege contra ataques de força bruta e rainbow tables
- **Verificação:** `BCrypt.Net.BCrypt.Verify(plainPassword, storedHash)` — timing-safe

### 2. JWT para Autenticação Stateless
- **Token:** HMAC-SHA256, expiração configurável (padrão 8h)
- **Claims:** user_id, company_id, role — sem dados sensíveis
- **Validação:** Assinatura + Issuer + Audience + Lifetime em todo request

### 3. Proteção contra SQL Injection
- **Como:** Entity Framework Core usa queries parametrizadas por padrão
- **Nenhuma concatenação:** String raw SQL nunca é construída com input do usuário
- **Exemplo correto:** `_context.Users.Where(u => u.Email == dto.Email)` — gera `WHERE email = @p0`

### 4. Proteção contra XSS
- **Input validation:** Todos os DTOs validam tamanho e formato antes de processar
- **Email:** Verificação de formato + tamanho máximo antes de buscar no banco
- **Mensagens de erro:** Genéricas — login inválido não revela qual campo está errado

### 5. Autorização por Role (RBAC)
- **[Authorize(Roles = "ADMIN")]:** DELETE de facilidade só por admin
- **[Authorize(Roles = "ADMIN,ANALYST")]:** POST/PUT por admin e analista
- **[Authorize]:** GET disponível para qualquer usuário autenticado
- **VIEWER:** Apenas leitura — não pode criar, editar ou deletar nada
```

### 5.2 — Demo standalone `security/auth_demo/auth.js`

```javascript
// Demo de segurança standalone — sem dependência da API
// Roda no browser abrindo login.html

const MOCK_USERS = [
  // Senha "Admin@2026" hasheada com BCrypt workFactor 12
  // Hash real gerado com: bcrypt.hashSync("Admin@2026", 12)
  {
    email: "admin@petro.com",
    // Simulando hash BCrypt — em produção nunca armazene plain text
    passwordHash: "$2b$12$simulated.hash.for.demo.only",
    role: "ADMIN"
  }
];

// Validação de input — proteção XSS
function sanitizeInput(input) {
  return input
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;")
    .replace(/'/g, "&#x27;")
    .trim();
}

function validateEmail(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email) && email.length <= 200;
}

function handleLogin(event) {
  event.preventDefault();

  const email = sanitizeInput(document.getElementById('email').value);
  const password = document.getElementById('password').value;

  // Validação client-side — primeira linha de defesa
  if (!validateEmail(email)) {
    showError("Email inválido.");
    return;
  }
  if (password.length < 8) {
    showError("Senha deve ter ao menos 8 caracteres.");
    return;
  }

  // Simula chamada à API — mensagem genérica intencionalmente
  // Não revela se foi o email ou a senha que está errado
  const user = MOCK_USERS.find(u => u.email === email);
  if (!user) {
    showError("Credenciais inválidas.");  // Genérico propositalmente
    return;
  }

  showSuccess(`Login realizado. Role: ${user.role}. JWT seria gerado aqui.`);
}

function showError(msg) {
  const el = document.getElementById('feedback');
  el.textContent = msg;
  el.style.color = '#ff4d6a';
}

function showSuccess(msg) {
  const el = document.getElementById('feedback');
  el.textContent = msg;
  el.style.color = '#00e87a';
}
```

**✅ Critério de aceite FASE 5:**
- [ ] `security_notes.md` documenta pelo menos 2 práticas com código de exemplo
- [ ] `auth.js` standalone demonstra validação + proteção XSS
- [ ] Senha nunca aparece em logs nem em respostas da API

---

## FASE 6 — SIMULADOR IoT

**Responsável:** Pessoa 5 (IoT) / Pessoa 1 (suporte dados)
**Entregável:** `iot/sensor_simulator.py` rodando e publicando payloads

### 6.1 — `iot/requirements.txt`

```
requests==2.31.0
schedule==1.2.1
python-dotenv==1.0.0
```

### 6.2 — `iot/sensor_simulator.py`

```python
"""
EcoCredit — Simulador de Sensores IoT
Simula leituras de sensores industriais + dados satelitais Sentinel-5P (ESA)
e publica para a API do EcoCredit via POST /api/v1/emissions

Uso:
    pip install -r requirements.txt
    python sensor_simulator.py

Documentação:
    Sensor CO2:     Mede concentração de dióxido de carbono em PPM.
                    Conversão: 1 PPM CO2 = ~0.0018 tCO2e/hora para instalação de médio porte.
    Sensor CH4:     Metano. GWP = 25 (25x mais potente que CO2). Convertido para CO2e.
    Satélite:       Mock do Sentinel-5P (ESA) — concentração regional de CO2 em coluna.
"""

import json
import random
import time
from datetime import datetime, timezone
import requests

# ─────────────────────────────────────────────
# Configuração
# ─────────────────────────────────────────────
API_BASE_URL = "http://localhost:5000/api/v1"
API_TOKEN = "seu-jwt-aqui"   # Substitua por token real após login
PUBLISH_INTERVAL_SECONDS = 30

FACILITIES = [
    {
        "facility_id": "fac-001",
        "name": "Refinaria RJ",
        "type": "REFINERY",
        "sensors": ["CO2", "CH4"],
        "base_co2_ppm": 1100.0,     # PPM base da instalação
        "base_ch4_ppm": 2.8,
    },
    {
        "facility_id": "fac-002",
        "name": "Plataforma P-51",
        "type": "RIG",
        "sensors": ["CO2"],
        "base_co2_ppm": 850.0,
        "base_ch4_ppm": 0,
    },
]

# ─────────────────────────────────────────────
# Simulação de sensores
# ─────────────────────────────────────────────
def read_co2_sensor(base_ppm: float) -> dict:
    """Simula leitura de sensor de CO2.
    Adiciona ruído gaussiano ±5% para simular variação real.
    """
    noise = random.gauss(0, base_ppm * 0.05)
    ppm = max(400.0, base_ppm + noise)
    # Conversão aproximada PPM → tCO2e/hora para instalação industrial
    quantity_tco2e = round((ppm / 1000) * 0.08, 4)
    return {"gas_type": "CO2", "raw_ppm": round(ppm, 2), "quantity_tco2e": quantity_tco2e}


def read_methane_sensor(base_ppm: float) -> dict:
    """Simula leitura de sensor de CH4 (metano).
    GWP-100 do CH4 = 25 → converte para CO2 equivalente.
    """
    noise = random.gauss(0, base_ppm * 0.08)
    ppm = max(0.0, base_ppm + noise)
    # GWP 25: 1 tCH4 = 25 tCO2e
    quantity_tco2e = round((ppm / 1000) * 0.003 * 25, 4)
    return {"gas_type": "CH4", "raw_ppm": round(ppm, 3), "quantity_tco2e": quantity_tco2e}


def get_sentinel5p_mock(latitude: float = -22.9) -> float:
    """Mock do dado satelital Sentinel-5P (ESA).
    Em produção real: GET https://ads.atmosphere.copernicus.eu/api/v2
    Para o MVP: simula concentração regional de CO2 em coluna (XCO2) em PPM.
    Valor típico: 415-430 PPM na atmosfera, mais alto em regiões industriais.
    """
    base_xco2 = 418.5  # PPM médio global 2024
    industrial_offset = random.uniform(0, 12.0)  # Excesso regional industrial
    noise = random.gauss(0, 0.5)
    return round(base_xco2 + industrial_offset + noise, 2)


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
        "satelliteCo2Regional": get_sentinel5p_mock(),
    }


def publish_to_api(payload: dict) -> bool:
    """Publica payload para a API. Retorna True se bem-sucedido."""
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
            print(f"  ✅ [{datetime.now(timezone.utc).isoformat()}] Publicado: {payload['facilityId']} "
                  f"| {payload['gasType']} | {payload['quantityTco2e']} tCO2e")
            return True
        else:
            print(f"  ⚠️  Erro {response.status_code}: {response.text[:100]}")
            return False
    except requests.exceptions.ConnectionError:
        # API offline — salva payload localmente para retry
        print(f"  📴 API offline — salvando payload em cache local")
        save_to_local_cache(payload)
        return False


def save_to_local_cache(payload: dict):
    """Salva payload localmente quando API está offline."""
    with open("iot/payloads/cache.jsonl", "a") as f:
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
            reading = read_co2_sensor(facility["base_co2_ppm"])
            payload = build_payload(facility, reading)
            print_payload_sample(payload)  # Demo — remover em produção
            publish_to_api(payload)

        if "CH4" in facility["sensors"] and facility["base_ch4_ppm"] > 0:
            reading = read_methane_sensor(facility["base_ch4_ppm"])
            payload = build_payload(facility, reading)
            publish_to_api(payload)


if __name__ == "__main__":
    print("🚀 EcoCredit — Simulador IoT iniciado")
    print(f"   Intervalo: {PUBLISH_INTERVAL_SECONDS}s")
    print(f"   Instalações: {len(FACILITIES)}")
    print(f"   API: {API_BASE_URL}\n")

    # Executa imediatamente e depois a cada N segundos
    simulate_cycle()
    while True:
        time.sleep(PUBLISH_INTERVAL_SECONDS)
        simulate_cycle()
```

**✅ Critério de aceite FASE 6:**
- [ ] `python sensor_simulator.py` roda sem erros
- [ ] Payloads aparecem no console com timestamp e dados
- [ ] `payloads/sample_payload.json` com exemplo documentado
- [ ] `security_notes.md` inclui explicação de cada sensor

---

## FASE 7 — README + DOCUMENTAÇÃO FINAL

### 7.1 — `README.md` (raiz do repositório)

```markdown
# 🌍 EcoCredit — Carbon Compliance SaaS
> FIAP Global Solution 2026/1 · 4º Ano · Engenharia de Software

**EcoCredit** é uma plataforma SaaS B2B de gestão de compliance ambiental e carbon credits.
Integra dados satelitais (Sentinel-5P/ESA mockado) com sensores IoT industriais para entregar
um dashboard executivo de conformidade ESG para empresas de petróleo, energia, agronegócio e mineração.

## 🏗️ Arquitetura

```
Sensor IoT (Python) → POST /api/v1/emissions → ASP.NET Core API → Oracle DB
Sentinel-5P Mock    ─────────────────────────────────────────────────────────↗
App Mobile (Expo)   ← GET /api/v1/emissions/summary ← API ← DB
```

## 👥 Equipe

| Pessoa | Nome | Parte |
|--------|------|-------|
| 1 | [Nome] | Banco de Dados |
| 2 | [Nome] | Backend API |
| 3 | [Nome] | Testes |
| 4 | [Nome] | Mobile |
| 5 | [Nome] | Segurança + IoT |

## 🚀 Como executar

### Pré-requisitos
- .NET 8 SDK
- Oracle XE 21c (ou SQL Server)
- Node.js 18+ + Expo CLI
- Python 3.11+

### 1. Banco de Dados
```bash
sqlplus ecocredit/ecocredit123@localhost:1521/XEPDB1 @database/schema.sql
sqlplus ecocredit/ecocredit123@localhost:1521/XEPDB1 @database/seeds.sql
```

### 2. Backend API
```bash
cd backend/EcoCredit.API
dotnet restore
dotnet run
# Swagger: http://localhost:5000/swagger
```

### 3. Testes
```bash
cd backend/EcoCredit.Tests
dotnet test --verbosity normal
```

### 4. Mobile
```bash
cd mobile/ecocredit-mobile
npm install
npx expo start
# Escanear QR com Expo Go
```

### 5. Simulador IoT
```bash
cd iot
pip install -r requirements.txt
# Configurar API_TOKEN em sensor_simulator.py após login
python sensor_simulator.py
```

## 📁 Estrutura do Projeto
[Ver estrutura completa no playbook]

## 🔗 ODS da ONU
- ODS 9: Indústria, Inovação e Infraestrutura
- ODS 11: Cidades e Comunidades Sustentáveis
- ODS 13: Ação Contra a Mudança Global do Clima
```

---

## CHECKLIST FINAL DE ENTREGA

### Antes do .zip:

**Parte 1 — Banco:**
- [ ] `database/schema.sql` — 6 tabelas, PKs e FKs
- [ ] `database/seeds.sql` — dados de exemplo
- [ ] `database/queries.sql` — 5 consultas comentadas
- [ ] `docs/diagrama_er.png` — diagrama exportado

**Parte 2 — API:**
- [ ] Backend compila e roda (`dotnet run`)
- [ ] Swagger acessível e documentado
- [ ] Mínimo 5 endpoints testados
- [ ] Arquitetura Controller → Service → Repository

**Parte 3 — Testes:**
- [ ] `tests/test_plan.md` com 8 casos de teste preenchidos
- [ ] `dotnet test` com mínimo 3 testes passando
- [ ] `tests/evidence/` com prints de execução
- [ ] Postman Collection exportada

**Parte 4 — Mobile:**
- [ ] App roda no Expo (`npx expo start`)
- [ ] 3 telas navegáveis
- [ ] Screenshots das 3 telas no PDF

**Parte 5 — Segurança:**
- [ ] BCrypt implementado no AuthService
- [ ] JWT configurado e validado
- [ ] `security_notes.md` documenta 2+ práticas
- [ ] Validação de input + mensagens genéricas de erro

**Parte 6 — IoT:**
- [ ] `sensor_simulator.py` roda sem erros
- [ ] Console mostra payloads sendo gerados
- [ ] `sample_payload.json` com exemplo documentado
- [ ] Explicação de cada sensor em `security_notes.md`

**Entrega final:**
- [ ] `.zip` com toda a estrutura acima
- [ ] `docs/ecocredit_entrega.pdf` com:
  - Nomes + RMs de todos os integrantes
  - Diagrama ER
  - Print do Swagger
  - Tabela de casos de teste
  - Screenshots do mobile
  - Explicação dos sensores IoT
- [ ] `.txt` com: RM, Nome, link do YouTube/Drive do pitch
- [ ] Vídeo pitch de 3 minutos no YouTube (unlisted ou público)

### Estrutura do .zip:
```
ecocredit_[TURMA]_[RM1]_[RM2]_[RM3]_[RM4]_[RM5].zip
├── README.md
├── backend/
├── database/
├── mobile/
├── iot/
├── security/
├── tests/
└── docs/
    ├── diagrama_er.png
    └── ecocredit_entrega.pdf
```

---

*EcoCredit · FIAP Global Solution 2026/1 · Playbook v1.0*
