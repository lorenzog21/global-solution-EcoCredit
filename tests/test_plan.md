# EcoCredit — Plano de Testes
**Projeto:** EcoCredit · FIAP Global Solution 2026/1
**Data:** 2026-06-01

## Casos de Teste

| ID | Cenário | Pré-condição | Entrada | Saída Esperada | Status |
|----|---------|-------------|---------|----------------|--------|
| TC-01 | Login com credenciais válidas | Usuário cadastrado no DB | `{ "email": "admin@petro.com", "password": "Admin@2026" }` | HTTP 200 + JWT token | ✅ PASSOU |
| TC-02 | Login com senha incorreta | Usuário cadastrado | `{ "email": "admin@petro.com", "password": "errada" }` | HTTP 401 + mensagem genérica | ✅ PASSOU |
| TC-03 | Registrar emissão válida via IoT | Token JWT válido, facility_id existente | `{ "facilityId": "fac-001", "gasType": "CO2", "quantityTco2e": 42.7, "source": "IOT_SENSOR" }` | HTTP 201 + registro criado | ✅ PASSOU |
| TC-04 | Registrar emissão com quantidade negativa | Token JWT válido | `{ "facilityId": "fac-001", "gasType": "CO2", "quantityTco2e": -10 }` | HTTP 400 + mensagem de validação | ✅ PASSOU |
| TC-05 | Threshold auto-alert ao ultrapassar 80% | Facility com emissão em 79% do limite | POST /emissions com valor suficiente para cruzar 80% | HTTP 201 + alerta HIGH gerado automaticamente | ✅ PASSOU |
| TC-06 | Acessar endpoint sem token | Sem autenticação | GET /api/v1/facilities | HTTP 401 Unauthorized | ✅ PASSOU |
| TC-07 | VIEWER tenta criar instalação | Token com role VIEWER | POST /api/v1/facilities | HTTP 403 Forbidden | ✅ PASSOU |
| TC-08 | Aplicar crédito já utilizado | Crédito com status USED | PUT /api/v1/credits/{id}/use | HTTP 400 + "Crédito não está disponível" | ✅ PASSOU |

## Testes Automatizados (xUnit)

Executar com: `dotnet test backend/EcoCredit.Tests`

| Teste | Cenário | Resultado |
|-------|---------|-----------|
| RegisterAsync_ValidEmission_ShouldCreateRecord | Emissão válida persiste no repositório | ✅ PASSOU |
| CheckThreshold_WhenAbove80Pct_ShouldCreateHighAlert | Alerta HIGH gerado a 85% do limite | ✅ PASSOU |
| GetSummaryAsync_ShouldReturnCorrectComplianceStatus | Status CRITICAL quando acima de 100% | ✅ PASSOU |
| RegisterAsync_WhenAbove100Pct_ShouldCreateCriticalAlert | Alerta CRITICAL quando acima de 100% | ✅ PASSOU |
| GetSummaryAsync_WhenBelow80Pct_ShouldReturnNormalStatus | Status NORMAL quando abaixo de 80% | ✅ PASSOU |

## Cobertura de Requisitos

- **Autenticação:** TC-01, TC-02 — Login + JWT
- **Autorização:** TC-06, TC-07 — RBAC
- **Emissões IoT:** TC-03, TC-04, TC-05 — Registro e alertas automáticos
- **Créditos:** TC-08 — Regras de negócio de créditos
