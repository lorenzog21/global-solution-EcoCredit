# Evidências de Execução — EcoCredit

Esta pasta contém os prints de execução exigidos na entrega.

---

## Evidências necessárias

### 1. Testes automatizados passando

**Arquivo:** `dotnet_test_passed.png`

Como tirar o print:
```bash
cd backend
dotnet test EcoCredit.sln --logger "console;verbosity=normal"
```
Tirar screenshot do terminal mostrando:
```
Test Run Successful.
Total tests: 15
     Passed: 15
 Total time: X,XXX Seconds
```

**Status:** `[INSERIR PRINT]`

---

### 2. Swagger UI com endpoints

**Arquivo:** `swagger_ui.png`

Como tirar o print:
```bash
cd backend
dotnet run --project EcoCredit.API/EcoCredit.API.csproj
# Abrir http://localhost:5000/swagger no Chrome
```
Tirar screenshot com todos os grupos de endpoints visíveis (Auth, Companies, Credits, Emissions, Facilities, Alerts).

**Status:** `[INSERIR PRINT]`

---

### 3. Login no Swagger / Postman

**Arquivo:** `swagger_login_200.png`

Executar POST `/api/v1/auth/login` com:
```json
{
  "email": "admin@petro.com",
  "password": "Admin@2026"
}
```
Tirar screenshot do response HTTP 200 com o JWT token.

**Status:** `[INSERIR PRINT]`

---

### 4. App Mobile — Tela de Login

**Arquivo:** `mobile_login.png`

```bash
cd mobile/ecocredit-mobile
npx expo start --ios
```
Tirar screenshot do simulador com a tela de Login aberta.

**Status:** `[INSERIR PRINT]`

---

### 5. App Mobile — Dashboard

**Arquivo:** `mobile_dashboard.png`

Após fazer login no app, tirar screenshot do Dashboard com os cards de emissões, créditos e alertas.

**Status:** `[INSERIR PRINT]`

---

### 6. App Mobile — Facilities (Instalações)

**Arquivo:** `mobile_facilities.png`

Tirar screenshot da tela de Instalações com os `EmissionCard` e barras de progresso de compliance.

**Status:** `[INSERIR PRINT]`

---

### 7. App Mobile — Credits (Créditos de Carbono)

**Arquivo:** `mobile_credits.png`

Tirar screenshot da tela de Créditos com o saldo disponível em tCO₂.

**Status:** `[INSERIR PRINT]`

---

### 8. Simulador IoT rodando

**Arquivo:** `iot_simulator_running.png`

```bash
cd iot
python sensor_simulator.py
```
Tirar screenshot do terminal com os logs de envio dos sensores.

**Status:** `[INSERIR PRINT]`
