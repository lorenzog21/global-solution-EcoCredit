# EcoCredit — Checklist de Entrega
> FIAP Global Solution 2026/1 · 4º Ano · Engenharia de Software

---

## PARTE 1 — Banco de Dados

| # | Entregável | Arquivo | Status |
|---|-----------|---------|--------|
| 1.1 | Schema SQL com 6 tabelas, PKs e FKs | `database/schema.sql` | ✅ Feito |
| 1.2 | Seeds com dados de exemplo | `database/seeds.sql` | ✅ Feito |
| 1.3 | 5 consultas comentadas | `database/queries.sql` | ✅ Feito |
| 1.4 | Diagrama ER exportado em PNG | `docs/diagrama_er.svg` | ✅ Feito — SVG vetorial. Abrir no navegador e exportar como PNG para o PDF |

**Notas:**
- `docs/diagrama_er.svg` — abre em qualquer navegador. Para exportar PNG: abrir no Chrome → botão direito na imagem → "Salvar imagem como" (ou usar a extensão SVG Export).

---

## PARTE 2 — Backend API (C# ASP.NET Core 8)

| # | Entregável | Arquivo | Status |
|---|-----------|---------|--------|
| 2.1 | `Program.cs` com DI, JWT, Swagger, CORS | `backend/EcoCredit.API/Program.cs` | ✅ Feito |
| 2.2 | `appsettings.json` com configuração JWT | `backend/EcoCredit.API/appsettings.json` | ✅ Feito |
| 2.3 | Models (6 entidades) | `backend/EcoCredit.API/Models/` | ✅ Feito |
| 2.4 | DTOs de Auth, Emission, Company, Facility, Credit, Alert | `backend/EcoCredit.API/DTOs/` | ✅ Feito — todos os DTOs implementados |
| 2.5 | Repositories com interfaces | `backend/EcoCredit.API/Repositories/` | ✅ Feito |
| 2.6 | AuthService | `backend/EcoCredit.API/Services/AuthService.cs` | ✅ Feito |
| 2.7 | EmissionService | `backend/EcoCredit.API/Services/EmissionService.cs` | ✅ Feito |
| 2.8 | CompanyService | `backend/EcoCredit.API/Services/CompanyService.cs` | ✅ Feito — camada de serviço com ToDto implementada |
| 2.9 | FacilityService | `backend/EcoCredit.API/Services/FacilityService.cs` | ✅ Feito |
| 2.10 | CreditService | `backend/EcoCredit.API/Services/CreditService.cs` | ✅ Feito — com cálculo de saldo e lógica de uso |
| 2.11 | AlertService | `backend/EcoCredit.API/Services/AlertService.cs` | ✅ Feito |
| 2.12 | AuthController | `backend/EcoCredit.API/Controllers/AuthController.cs` | ✅ Feito |
| 2.13 | CompaniesController | `backend/EcoCredit.API/Controllers/CompaniesController.cs` | ✅ Feito |
| 2.14 | FacilitiesController | `backend/EcoCredit.API/Controllers/FacilitiesController.cs` | ✅ Feito |
| 2.15 | EmissionsController | `backend/EcoCredit.API/Controllers/EmissionsController.cs` | ✅ Feito |
| 2.16 | CreditsController | `backend/EcoCredit.API/Controllers/CreditsController.cs` | ✅ Feito |
| 2.17 | AlertsController | `backend/EcoCredit.API/Controllers/AlertsController.cs` | ✅ Feito |
| 2.18 | JwtMiddleware | — | ✅ Feito — JWT via middleware nativo do ASP.NET (`AddJwtBearer`) |
| 2.19 | ExceptionMiddleware | `backend/EcoCredit.API/Middleware/ExceptionMiddleware.cs` | ✅ Feito — retorna JSON padronizado para 400/403/404/500 |
| 2.20 | `dotnet run` sobe em `http://localhost:5000` | — | ✅ Feito |
| 2.21 | Swagger acessível e documentado | `http://localhost:5000/swagger` | ✅ Feito |

---

## PARTE 3 — Testes

| # | Entregável | Arquivo | Status |
|---|-----------|---------|--------|
| 3.1 | Plano de testes com 8 casos | `tests/test_plan.md` | ✅ Feito |
| 3.2 | EmissionServiceTests (xUnit) | `backend/EcoCredit.Tests/Services/EmissionServiceTests.cs` | ✅ Feito — 5 testes |
| 3.3 | CreditServiceTests (xUnit) | `backend/EcoCredit.Tests/Services/CreditServiceTests.cs` | ✅ Feito — 5 testes (saldo, create, use, já usado, not found) |
| 3.4 | AuthServiceTests (xUnit) | `backend/EcoCredit.Tests/Services/AuthServiceTests.cs` | ✅ Feito — 5 testes (login OK, senha errada, email desconhecido, register OK, email duplicado) |
| 3.5 | Postman Collection exportada | `tests/postman/EcoCredit.postman_collection.json` | ✅ Feito |
| 3.6 | Evidências (prints de execução) | `tests/evidence/` | ❌ Faltando — tirar prints do `dotnet test` e do app rodando |

**Notas:**
- Total de testes: 15 (5 Emission + 5 Credit + 5 Auth). Rodar com `dotnet test` na pasta `backend/`.

---

## PARTE 4 — Mobile (React Native + Expo)

| # | Entregável | Arquivo | Status |
|---|-----------|---------|--------|
| 4.1 | Setup Expo com navegação bottom-tabs | `mobile/ecocredit-mobile/` | ✅ Feito |
| 4.2 | Mock data | `mobile/ecocredit-mobile/data/mockData.js` | ✅ Feito |
| 4.3 | DashboardScreen | `mobile/ecocredit-mobile/screens/DashboardScreen.js` | ✅ Feito |
| 4.4 | FacilitiesScreen | `mobile/ecocredit-mobile/screens/FacilitiesScreen.js` | ✅ Feito |
| 4.5 | CreditsScreen | `mobile/ecocredit-mobile/screens/CreditsScreen.js` | ✅ Feito |
| 4.6 | LoginScreen | `mobile/ecocredit-mobile/screens/LoginScreen.js` | ✅ Feito — tela de login com validação e credenciais demo |
| 4.7 | Componente ComplianceIndicator | `mobile/ecocredit-mobile/components/ComplianceIndicator.js` | ✅ Feito — suporta `size="md"` e `size="lg"` |
| 4.8 | Componente EmissionCard | `mobile/ecocredit-mobile/components/EmissionCard.js` | ✅ Feito — barra de progresso + status colorido |
| 4.9 | Componente AlertBadge | `mobile/ecocredit-mobile/components/AlertBadge.js` | ✅ Feito — modo completo e `compact` |
| 4.10 | `npx expo start` roda sem erros | — | ✅ Feito |
| 4.11 | Screenshots das telas para o PDF | — | ❌ Faltando — tirar com `npx expo start --web` |

**Notas:**
- App.js atualizado: fluxo Login → MainTabs (Bottom Tabs).
- Credenciais demo: `admin@petro.com / Admin@2026` e `analyst@petro.com / Analyst@2026`.

---

## PARTE 5 — Segurança

| # | Entregável | Arquivo | Status |
|---|-----------|---------|--------|
| 5.1 | BCrypt implementado no AuthService | `backend/EcoCredit.API/Services/AuthService.cs` | ✅ Feito — workFactor 12 |
| 5.2 | JWT configurado e validado | `backend/EcoCredit.API/Program.cs` | ✅ Feito — HMAC-SHA256, 8h |
| 5.3 | Documentação de segurança | `security/security_notes.md` | ✅ Feito |
| 5.4 | Demo standalone de login + XSS | `security/auth_demo/login.html` + `auth.js` | ✅ Feito |
| 5.5 | Exemplos de prevenção SQL Injection | `security/sql_injection_demo/prevention_examples.sql` | ✅ Feito |
| 5.6 | Validação de input + mensagens genéricas de erro | `backend/EcoCredit.API/Controllers/AuthController.cs` | ✅ Feito |
| 5.7 | RBAC por roles (ADMIN / ANALYST / VIEWER) | controllers com `[Authorize(Roles)]` | ✅ Feito |

---

## PARTE 6 — Simulador IoT

| # | Entregável | Arquivo | Status |
|---|-----------|---------|--------|
| 6.1 | `requirements.txt` | `iot/requirements.txt` | ✅ Feito |
| 6.2 | Sensor CO2 | `iot/sensors/co2_sensor.py` | ✅ Feito |
| 6.3 | Sensor CH4 (metano) | `iot/sensors/methane_sensor.py` | ✅ Feito |
| 6.4 | Mock Sentinel-5P | `iot/sensors/satellite_mock.py` | ✅ Feito |
| 6.5 | Script principal do simulador | `iot/sensor_simulator.py` | ✅ Feito |
| 6.6 | Exemplo de payload documentado | `iot/payloads/sample_payload.json` | ✅ Feito |
| 6.7 | `python sensor_simulator.py` roda sem erros | — | ✅ Feito (modo offline quando API está down) |

---

## ENTREGA FINAL

| # | Entregável | Status |
|---|-----------|--------|
| E.1 | `docs/diagrama_er.svg` — diagrama vetorial completo | ✅ Feito — exportar para PNG para o PDF |
| E.2 | `docs/ecocredit_entrega.pdf` — documento final com nomes, RMs, prints, tabela de testes | ❌ Faltando |
| E.3 | `tests/evidence/` — prints de execução dos testes e do app | ❌ Faltando |
| E.4 | `.txt` com RM, Nome, link do YouTube/Drive do pitch | ❌ Faltando |
| E.5 | Vídeo pitch de 3 minutos (YouTube unlisted ou público) | ❌ Faltando |
| E.6 | `.zip` com estrutura completa | ❌ Faltando |

---

## RESUMO EXECUTIVO

| Parte | Status | O que falta |
|-------|--------|------------|
| 1 — Banco de Dados | 🟢 100% | — Diagrama ER gerado em SVG (exportar PNG para o PDF) |
| 2 — Backend API | 🟢 100% | — Tudo implementado |
| 3 — Testes | 🟢 95% | Evidências (`tests/evidence/`) |
| 4 — Mobile | 🟢 95% | Screenshots para o PDF |
| 5 — Segurança | 🟢 100% | — |
| 6 — IoT | 🟢 100% | — |
| Entrega Final | 🔴 15% | PDF, evidências, pitch, .zip |

**Pendências críticas para entrega (sem isso não entrega):**
1. `tests/evidence/` — rodar `dotnet test` e tirar screenshot, rodar `npx expo start --web` e tirar screenshots das 3 telas + tela de login
2. `docs/ecocredit_entrega.pdf` — documento final com nomes, RMs e evidências
3. Vídeo pitch de 3 minutos no YouTube
4. `.txt` com dados do grupo
5. `.zip` final com toda a estrutura
