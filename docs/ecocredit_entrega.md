# EcoCredit — Plataforma de Monitoramento Ambiental & ESG
**FIAP Global Solution 2026/1 · 4º Ano · Engenharia de Software**

---

## 1. Integrantes do Grupo

| Nome Completo | RM |
|---|---|
| [NOME INTEGRANTE 1] | RM[XXXXX] |
| [NOME INTEGRANTE 2] | RM[XXXXX] |
| [NOME INTEGRANTE 3] | RM[XXXXX] |


---

## 2. Descrição do Projeto

O **EcoCredit** é uma plataforma inteligente de monitoramento ambiental e ESG voltada para grandes indústrias dos setores de Agricultura, Mineração, Petróleo, Energia e Têxtil.

O problema que a solução resolve é a fragmentação das ferramentas de mercado: hoje as empresas consultam imagens de satélite (escala macro) ou sensores IoT no solo (escala micro) de forma isolada. O diferencial do EcoCredit é **cruzar as duas visões** para gerar laudos automáticos, auditáveis e transparentes — evitando multas milionárias e abrindo caminho para créditos de carbono e green bonds.

### Pilares de dados

| Pilar | Tecnologia | Descrição |
|---|---|---|
| Satélite | Mock Sentinel-5P (ESA) | Cálculo de km² desmatados, ilhas de calor, concentração de CO₂ regional |
| IoT (solo) | Python + simulador de sensores | Temperatura, qualidade do ar (CO₂ PPM, CH₄ PPM) e qualidade da água |
| Visão Computacional | OpenCV (planejado) | Análise local de câmeras industriais |

### Geração de valor

1. **Prevenção de multas** — monitoramento contínuo detecta anomalias antes que virem desastres
2. **Green Bonds** — relatórios auditáveis reduzem custo de capital
3. **Créditos de Carbono** — prova de recuperação de áreas gera ativos monetizáveis
4. **Reputação ESG** — selos verdes com evidências verificáveis

---

## 3. Arquitetura da Solução

```
┌─────────────────────────────────────────────────┐
│                  Mobile App                      │
│          React Native + Expo (iOS/Android)       │
└────────────────────┬────────────────────────────┘
                     │ HTTPS / JWT
┌────────────────────▼────────────────────────────┐
│              Backend API REST                    │
│         ASP.NET Core 8 · Swagger · JWT           │
│                                                  │
│  Controllers → Services → Repositories → EF Core│
└──────────┬──────────────────────┬───────────────┘
           │ In-Memory (dev)      │ Oracle 19c (prod)
           │                      │
┌──────────▼──────────┐  ┌────────▼────────────────┐
│   Simulador IoT     │  │     Banco de Dados       │
│   Python 3.x        │  │  6 tabelas / schema.sql  │
│   co2_sensor.py     │  │  seeds.sql / queries.sql │
│   methane_sensor.py │  └─────────────────────────┘
│   satellite_mock.py │
└─────────────────────┘
```

---

## 4. Banco de Dados

### 4.1 Diagrama Entidade-Relacionamento

> **[INSERIR IMAGEM]** — arquivo `docs/diagrama_er.svg` (abrir no Chrome → Ctrl+P → Salvar como PDF)

O modelo possui **6 entidades** com relacionamentos 1:N entre empresa e suas entidades filhas:

- `TB_COMPANY` → `TB_USER` (1:N)
- `TB_COMPANY` → `TB_FACILITY` (1:N)
- `TB_COMPANY` → `TB_CARBON_CREDIT` (1:N)
- `TB_FACILITY` → `TB_EMISSION_RECORD` (1:N)
- `TB_FACILITY` → `TB_ALERT` (1:N)

### 4.2 Tabelas

| Tabela | Descrição | Campos-chave |
|---|---|---|
| `TB_COMPANY` | Tenant principal — empresa contratante | `company_id`, `cnpj` (UNIQUE), `sector` |
| `TB_USER` | Usuários com roles | `user_id`, `email` (UNIQUE), `role` (ADMIN/ANALYST/VIEWER) |
| `TB_FACILITY` | Instalações industriais (plantas, minas, plataformas) | `facility_id`, `emission_limit_tco2`, `facility_type` |
| `TB_EMISSION_RECORD` | Registro de emissões por instalação | `record_id`, `gas_type`, `quantity_tco2e`, `source` |
| `TB_CARBON_CREDIT` | Créditos de carbono (CBIO, REDD, VCM, GOLD) | `credit_id`, `status`, `expiry_date` |
| `TB_ALERT` | Alertas automáticos de threshold | `alert_id`, `severity`, `resolved` |

### 4.3 Constraints e Índices

- **CHECK constraints** em todos os campos de domínio (`sector`, `role`, `gas_type`, `status`, `severity`)
- **Índice composto** `idx_emission_facility_date` em `(facility_id, recorded_at)` — consultas de séries temporais
- **Índices** em `idx_alert_facility`, `idx_credit_company`, `idx_user_company`

---

## 5. Backend API (ASP.NET Core)

### 5.1 Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| ASP.NET Core | 9.0 | Framework principal |
| Entity Framework Core | 8.0 | ORM + In-Memory para dev |
| BCrypt.Net-Next | 4.2 | Hash de senhas (workFactor 12) |
| JWT Bearer | 8.0 | Autenticação stateless |
| Swashbuckle (Swagger) | 6.6 | Documentação interativa da API |

### 5.2 Endpoints

#### Auth — `/api/v1/auth`
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| POST | `/login` | Login — retorna JWT | Público |
| POST | `/register` | Cadastra usuário | Público |

#### Companies — `/api/v1/companies`
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/` | Lista empresas | JWT |
| GET | `/{id}` | Busca por ID | JWT |
| POST | `/` | Cria empresa | ADMIN |

#### Facilities — `/api/v1/facilities`
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/` | Lista instalações da empresa | JWT |
| POST | `/` | Cria instalação | ADMIN, ANALYST |
| PUT | `/{id}` | Atualiza instalação | ADMIN, ANALYST |
| DELETE | `/{id}` | Desativa (soft delete) | ADMIN |

#### Emissions — `/api/v1/emissions`
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| POST | `/` | Registra emissão | JWT |
| GET | `/summary` | Sumário por instalação + compliance | JWT |

#### Credits — `/api/v1/credits`
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/` | Lista créditos + saldo disponível | JWT |
| POST | `/` | Registra novo crédito | ADMIN |
| PUT | `/{id}/use` | Aplica crédito para compensar emissão | ADMIN, ANALYST |

#### Alerts — `/api/v1/alerts`
| Método | Rota | Descrição | Auth |
|---|---|---|---|
| GET | `/` | Lista alertas (ativos ou histórico) | JWT |
| PUT | `/{id}/resolve` | Resolve alerta | JWT |

### 5.3 Lógica de Alertas Automáticos

O `EmissionService` dispara alertas automaticamente ao registrar emissões:

| Threshold | Severidade | Tipo |
|---|---|---|
| ≥ 80% do limite mensal | HIGH | THRESHOLD |
| ≥ 100% do limite mensal | CRITICAL | THRESHOLD |

### 5.4 Print — API rodando / Swagger

> **[INSERIR PRINT]** — tirar screenshot do Swagger em `http://localhost:5000/swagger` com os endpoints expandidos

---

## 6. Testes

### 6.1 Plano de Testes — Casos Manuais

| ID | Cenário | Entrada | Saída Esperada | Resultado |
|---|---|---|---|---|
| TC-01 | Login com credenciais válidas | `admin@petro.com / Admin@2026` | HTTP 200 + JWT token | ✅ PASSOU |
| TC-02 | Login com senha incorreta | `admin@petro.com / errada` | HTTP 401 + mensagem genérica | ✅ PASSOU |
| TC-03 | Registrar emissão válida via IoT | `facilityId, gasType: CO2, qty: 42.7` | HTTP 201 + registro criado | ✅ PASSOU |
| TC-04 | Emissão com quantidade negativa | `quantityTco2e: -10` | HTTP 400 + validação | ✅ PASSOU |
| TC-05 | Threshold auto-alert ao cruzar 80% | Emissão que leva facility para 85% | HTTP 201 + alerta HIGH gerado | ✅ PASSOU |
| TC-06 | Endpoint sem token | GET /facilities sem Bearer | HTTP 401 Unauthorized | ✅ PASSOU |
| TC-07 | VIEWER tenta criar instalação | Token role VIEWER + POST /facilities | HTTP 403 Forbidden | ✅ PASSOU |
| TC-08 | Aplicar crédito já utilizado | PUT /credits/{id}/use (status=USED) | HTTP 400 + "Crédito não disponível" | ✅ PASSOU |

### 6.2 Testes Automatizados (xUnit) — 15/15 passando

Executados com: `dotnet test backend/EcoCredit.Tests`

#### AuthServiceTests (5 testes)

| Teste | Cenário | Resultado |
|---|---|---|
| `LoginAsync_WithValidCredentials_ShouldReturnToken` | Login OK retorna JWT com claims corretos | ✅ PASSOU |
| `LoginAsync_WithWrongPassword_ShouldReturnNull` | Senha errada retorna null | ✅ PASSOU |
| `LoginAsync_WithUnknownEmail_ShouldReturnNull` | Email inexistente retorna null | ✅ PASSOU |
| `RegisterAsync_WithNewEmail_ShouldReturnTrue` | Novo email cadastra e chama AddAsync | ✅ PASSOU |
| `RegisterAsync_WithDuplicateEmail_ShouldReturnFalse` | Email duplicado retorna false, não chama AddAsync | ✅ PASSOU |

#### CreditServiceTests (5 testes)

| Teste | Cenário | Resultado |
|---|---|---|
| `GetBalanceAsync_ShouldSumAvailableCredits` | Soma apenas créditos AVAILABLE | ✅ PASSOU |
| `CreateAsync_ShouldSetStatusAvailable` | Novo crédito nasce com status AVAILABLE | ✅ PASSOU |
| `UseCreditAsync_WhenAvailable_ShouldMarkUsed` | Crédito disponível é marcado USED | ✅ PASSOU |
| `UseCreditAsync_WhenAlreadyUsed_ShouldReturnNull` | Crédito já usado retorna null | ✅ PASSOU |
| `UseCreditAsync_WhenNotFound_ShouldReturnNull` | ID inexistente retorna null | ✅ PASSOU |

#### EmissionServiceTests (5 testes)

| Teste | Cenário | Resultado |
|---|---|---|
| `RegisterAsync_ValidEmission_ShouldCreateRecord` | Emissão válida persiste no repositório | ✅ PASSOU |
| `CheckThreshold_WhenAbove80Pct_ShouldCreateHighAlert` | Alerta HIGH gerado a 85% do limite | ✅ PASSOU |
| `GetSummaryAsync_ShouldReturnCorrectComplianceStatus` | Status CRITICAL quando acima de 100% | ✅ PASSOU |
| `RegisterAsync_WhenAbove100Pct_ShouldCreateCriticalAlert` | Alerta CRITICAL quando acima de 100% | ✅ PASSOU |
| `GetSummaryAsync_WhenBelow80Pct_ShouldReturnNormalStatus` | Status NORMAL quando abaixo de 80% | ✅ PASSOU |

### 6.3 Print — Testes passando

> **[INSERIR PRINT]** — tirar screenshot do terminal com o output de `dotnet test` mostrando `Test Run Successful. Total tests: 15, Passed: 15`

---

## 7. Aplicativo Mobile

### 7.1 Tecnologias

| Tecnologia | Versão | Uso |
|---|---|---|
| React Native | 0.85.3 | Framework mobile |
| Expo SDK | 56 | Toolchain e deploy |
| React Navigation | 7.x | Bottom tabs + stack |
| @expo/vector-icons | 15 | Ícones |

### 7.2 Telas

#### Login Screen
- Validação de email e senha
- Credenciais demo: `admin@petro.com / Admin@2026`
- Ao autenticar, navega para o TabNavigator principal

> **[INSERIR PRINT]** — screenshot da tela de Login no simulador iOS

#### Dashboard Screen
- Cards de resumo: total de emissões, créditos disponíveis, alertas ativos
- Lista de alertas recentes com badge de severidade (HIGH/CRITICAL)
- Componente `AlertBadge` em modo compacto

> **[INSERIR PRINT]** — screenshot do Dashboard com os cards e alertas

#### Facilities Screen
- Lista de instalações industriais com indicador de compliance
- Componente `EmissionCard` com barra de progresso e status colorido (NORMAL / HIGH / CRITICAL)
- Componente `ComplianceIndicator` em tamanho `lg`

> **[INSERIR PRINT]** — screenshot da tela de Instalações com as barras de progresso

#### Credits Screen
- Saldo total de créditos de carbono disponíveis (tCO₂)
- Lista detalhada por tipo (CBIO, REDD, VCM, GOLD)
- Status visual por crédito (AVAILABLE / USED)

> **[INSERIR PRINT]** — screenshot da tela de Créditos de Carbono

### 7.3 Componentes Reutilizáveis

| Componente | Descrição |
|---|---|
| `ComplianceIndicator` | Indicador visual de conformidade, suporta `size="md"` e `size="lg"` |
| `EmissionCard` | Card de emissão com barra de progresso e status colorido |
| `AlertBadge` | Badge de alerta, modo completo e modo `compact` |

---

## 8. Segurança

### 8.1 Autenticação e Hash de Senhas

- **BCrypt** com `workFactor: 12` (~250ms por hash) — proteção contra força bruta e rainbow tables
- Verificação timing-safe: `BCrypt.Verify()` — resistente a timing attacks
- Senhas **nunca armazenadas em texto puro** — apenas o hash

### 8.2 JWT (JSON Web Token)

- Algoritmo: **HMAC-SHA256**
- Expiração: **8 horas** (configurável em `appsettings.json`)
- Claims: `user_id`, `company_id`, `role` — sem dados sensíveis no payload
- Validação completa: assinatura + issuer + audience + lifetime em cada request

### 8.3 Proteção contra SQL Injection

- **Entity Framework Core** usa queries parametrizadas por padrão
- Nenhuma concatenação de string SQL com input do usuário
- Exemplo: `FirstOrDefaultAsync(u => u.Email == dto.Email)` gera `WHERE email = @p0`

### 8.4 Proteção contra XSS

- Validação de input nos DTOs (tamanho, formato de email)
- Mensagens de erro genéricas — não revelam qual campo está errado
- Sanitização no frontend HTML demo (`auth.js`)

### 8.5 RBAC — Controle de Acesso por Role

| Role | Permissões |
|---|---|
| `ADMIN` | CRUD completo, gerenciamento de usuários e créditos |
| `ANALYST` | Criar/editar instalações e emissões, sem acesso administrativo |
| `VIEWER` | Somente leitura — não pode criar, editar ou deletar |

---

## 9. Simulador IoT

O simulador em Python (`iot/sensor_simulator.py`) envia dados continuamente para a API, simulando sensores físicos reais.

### 9.1 Sensores Implementados

| Sensor | Arquivo | Gás medido | Conversão |
|---|---|---|---|
| CO₂ | `co2_sensor.py` | CO₂ (PPM) | `(PPM/1000) × 0.08` → tCO₂e/hora |
| CH₄ (Metano) | `methane_sensor.py` | CH₄ (PPM) | `(PPM/1000) × 0.003 × 25` → tCO₂e (GWP-25) |
| Sentinel-5P | `satellite_mock.py` | XCO₂ regional | Base 418.5 PPM + offset industrial regional |

### 9.2 Execução

```bash
cd iot
pip install -r requirements.txt
python sensor_simulator.py
```

> O simulador opera em **modo offline** automaticamente quando a API está indisponível, registrando os payloads localmente para envio posterior.

### 9.3 Exemplo de Payload IoT

```json
{
  "facilityId": "fac-001",
  "gasType": "CO2",
  "quantityTco2e": 42.7,
  "source": "IOT_SENSOR",
  "sensorId": "sensor-co2-001",
  "rawPpm": 1240.5,
  "satelliteCo2Regional": 421.3
}
```

---

## 10. Como Executar o Projeto

### Backend

```bash
cd backend
dotnet run --project EcoCredit.API/EcoCredit.API.csproj
# API: http://localhost:5000
# Swagger: http://localhost:5000/swagger
```

### Testes

```bash
cd backend
dotnet test EcoCredit.sln --logger "console;verbosity=normal"
```

### Mobile

```bash
cd mobile/ecocredit-mobile
npm install --legacy-peer-deps
npx expo start --ios       # simulador iOS
npx expo start --android   # emulador Android
npx expo start --web       # navegador
```

### Simulador IoT

```bash
cd iot
pip install -r requirements.txt
python sensor_simulator.py
```

### Credenciais Demo

| Email | Senha | Role |
|---|---|---|
| `admin@petro.com` | `Admin@2026` | ADMIN |
| `analyst@petro.com` | `Analyst@2026` | ANALYST |

---

## 11. Estrutura do Repositório

```
global-solution-EcoCredit/
├── backend/
│   ├── EcoCredit.API/
│   │   ├── Controllers/      # 6 controllers REST
│   │   ├── DTOs/             # Auth, Company, Facility, Emission, Credit, Alert
│   │   ├── Middleware/       # ExceptionMiddleware
│   │   ├── Models/           # 6 entidades
│   │   ├── Repositories/     # Interfaces + implementações EF Core
│   │   ├── Services/         # Auth, Emission, Credit, Facility, Company, Alert
│   │   ├── Program.cs        # DI, JWT, Swagger, CORS, seed
│   │   └── appsettings.json
│   └── EcoCredit.Tests/
│       └── Services/         # 15 testes xUnit
├── mobile/ecocredit-mobile/
│   ├── screens/              # Login, Dashboard, Facilities, Credits
│   ├── components/           # ComplianceIndicator, EmissionCard, AlertBadge
│   ├── data/mockData.js
│   └── App.js
├── database/
│   ├── schema.sql            # 6 tabelas Oracle 19c+
│   ├── seeds.sql             # Dados de exemplo
│   └── queries.sql           # 5 consultas comentadas
├── iot/
│   ├── sensors/              # co2_sensor.py, methane_sensor.py, satellite_mock.py
│   ├── sensor_simulator.py
│   └── requirements.txt
├── security/
│   ├── security_notes.md
│   ├── auth_demo/            # login.html + auth.js
│   └── sql_injection_demo/
├── tests/
│   ├── test_plan.md
│   ├── evidence/             # Prints de execução (ver pasta)
│   └── postman/              # EcoCredit.postman_collection.json
└── docs/
    ├── diagrama_er.svg
    └── ecocredit_entrega.md  # Este documento
```

---

*Documento gerado em 05/06/2026 · FIAP Global Solution 2026/1*
