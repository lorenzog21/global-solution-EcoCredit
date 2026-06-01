# EcoCredit — Carbon Compliance SaaS
> FIAP Global Solution 2026/1 · 4º Ano · Engenharia de Software

**EcoCredit** é uma plataforma SaaS B2B de gestão de compliance ambiental e carbon credits.
Integra dados satelitais (Sentinel-5P/ESA mockado) com sensores IoT industriais para entregar
um dashboard executivo de conformidade ESG para empresas de petróleo, energia, agronegócio e mineração.

## Arquitetura

```
Sensor IoT (Python) → POST /api/v1/emissions → ASP.NET Core 8 API → Oracle DB (InMemory dev)
Sentinel-5P Mock    ─────────────────────────────────────────────────────────────────────────↗
App Mobile (Expo)   ← GET /api/v1/emissions/summary ← API
```

## ODS da ONU

- **ODS 9** — Indústria, Inovação e Infraestrutura
- **ODS 11** — Cidades e Comunidades Sustentáveis
- **ODS 13** — Ação Contra a Mudança Global do Clima

## Equipe

| Pessoa | Parte |
|--------|-------|
| 1 | Banco de Dados (Oracle SQL) |
| 2 | Backend API (C# ASP.NET Core 8) |
| 3 | Testes (xUnit + Postman) |
| 4 | Mobile (React Native + Expo) |
| 5 | Segurança + IoT (Python) |

## Como executar

### Pré-requisitos
- .NET 8 SDK
- Node.js 18+ + Expo CLI (`npm i -g expo-cli`)
- Python 3.11+

### 1. Banco de Dados (Oracle)
```bash
sqlplus ecocredit/ecocredit123@localhost:1521/XEPDB1 @database/schema.sql
sqlplus ecocredit/ecocredit123@localhost:1521/XEPDB1 @database/seeds.sql
```
> Em modo de desenvolvimento, a API usa InMemory database com seed automático.

### 2. Backend API
```bash
cd backend/EcoCredit.API
dotnet restore
dotnet run
# Swagger: http://localhost:5000/swagger
# Login: POST /api/v1/auth/login
#   { "email": "admin@petro.com", "password": "Admin@2026" }
```

### 3. Testes
```bash
cd backend/EcoCredit.Tests
dotnet test --verbosity normal
# 6 testes — todos devem passar
```

### 4. Mobile
```bash
cd mobile/ecocredit-mobile
npm install
npx expo start
# Escanear QR com Expo Go ou pressionar 'w' para abrir no browser
```

### 5. Simulador IoT
```bash
cd iot
pip install -r requirements.txt
# Configurar API_TOKEN em sensor_simulator.py com token obtido em /auth/login
python sensor_simulator.py
```

### 6. Demo de Segurança
```bash
# Abrir no browser:
open security/auth_demo/login.html
```

## Estrutura do Projeto

```
ecocredit/
├── backend/
│   ├── EcoCredit.API/         # C# ASP.NET Core 8 — Controller/Service/Repository
│   └── EcoCredit.Tests/       # xUnit — 6 testes automatizados
├── database/
│   ├── schema.sql             # 6 tabelas Oracle
│   ├── seeds.sql              # Dados de exemplo
│   └── queries.sql            # 5 consultas de relatório
├── mobile/
│   └── ecocredit-mobile/      # React Native + Expo — 3 telas
├── iot/
│   ├── sensor_simulator.py    # Loop de publicação IoT
│   └── sensors/               # CO2, CH4, Sentinel-5P mock
├── security/
│   ├── auth_demo/             # Demo standalone BCrypt + XSS
│   └── sql_injection_demo/    # Exemplos de prevenção
└── tests/
    ├── test_plan.md           # 8 casos de teste
    └── postman/               # Collection Postman
```

## Endpoints da API

| Método | Rota | Descrição |
|--------|------|-----------|
| POST | `/api/v1/auth/login` | Login — retorna JWT |
| POST | `/api/v1/auth/register` | Cadastra usuário |
| GET  | `/api/v1/facilities` | Lista instalações da empresa |
| POST | `/api/v1/facilities` | Cadastra instalação (ADMIN/ANALYST) |
| POST | `/api/v1/emissions` | Registra emissão (IoT ou manual) |
| GET  | `/api/v1/emissions/summary` | Sumário por instalação com compliance |
| GET  | `/api/v1/credits` | Lista créditos de carbono |
| POST | `/api/v1/credits` | Adquire créditos (ADMIN) |
| PUT  | `/api/v1/credits/{id}/use` | Aplica crédito para compensar emissões |
| GET  | `/api/v1/alerts` | Lista alertas ativos |
| PUT  | `/api/v1/alerts/{id}/resolve` | Resolve alerta |
| GET  | `/api/v1/companies` | Lista empresas |
