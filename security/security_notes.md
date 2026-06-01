# EcoCredit — Documentação de Segurança

## Práticas Implementadas

### 1. Autenticação com Senha Hasheada (BCrypt)
- **Implementação:** `AuthService.cs` usa `BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12)`
- **Work Factor 12:** ~250ms por hash — protege contra ataques de força bruta e rainbow tables
- **Verificação:** `BCrypt.Net.BCrypt.Verify(plainPassword, storedHash)` — timing-safe por design
- **Exemplo no código:**
  ```csharp
  // Hash na criação do usuário
  user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, workFactor: 12);
  
  // Verificação no login — resistente a timing attacks
  bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
  ```

### 2. JWT para Autenticação Stateless
- **Token:** HMAC-SHA256, expiração configurável (padrão 8h)
- **Claims:** user_id, company_id, role — sem dados sensíveis no payload
- **Validação:** Assinatura + Issuer + Audience + Lifetime em todo request autenticado
- **Key:** Mínimo 32 caracteres, armazenada em `appsettings.json` (em produção: Azure Key Vault)

### 3. Proteção contra SQL Injection
- **Como:** Entity Framework Core usa queries parametrizadas por padrão
- **Nenhuma concatenação:** String raw SQL nunca é construída com input do usuário
- **Exemplo correto (EF Core):**
  ```csharp
  // Gera: WHERE email = @p0 — nunca concatena strings
  _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
  ```
- **Exemplo de ataque prevenido:**
  ```
  Input: admin@petro.com' OR '1'='1
  EF Core: parametriza automaticamente → sem efeito
  ```

### 4. Proteção contra XSS
- **Input validation:** Todos os DTOs validam tamanho e formato antes de processar
- **Email:** Verificação de `Contains('@')` + tamanho máximo 200 chars
- **Mensagens de erro:** Genéricas — login inválido não revela qual campo está errado
- **Sanitização no frontend (auth.js):** `replace(/</g, "&lt;")` etc.

### 5. Autorização por Role (RBAC)
- **[Authorize(Roles = "ADMIN")]:** DELETE de facilidade, cadastro de créditos
- **[Authorize(Roles = "ADMIN,ANALYST")]:** POST/PUT de instalações e emissões
- **[Authorize]:** GET disponível para qualquer usuário autenticado
- **VIEWER:** Apenas leitura — não pode criar, editar ou deletar nada

### 6. Sensores IoT — Detalhes Técnicos

#### Sensor CO2 (co2_sensor.py)
- Mede concentração de CO₂ em PPM (partes por milhão) no ar da instalação
- Conversão: `(PPM / 1000) * 0.08` → tCO₂e/hora para instalação industrial de médio porte
- Baseline atmosférico: ~415 PPM (2024); instalações industriais: 800–1500 PPM
- Ruído gaussiano ±5% simula variação real de sensores físicos

#### Sensor CH4 - Metano (methane_sensor.py)
- Metano tem GWP-100 = 25 (25x mais potente que CO₂ no aquecimento global em 100 anos)
- Conversão: `(PPM / 1000) * 0.003 * 25` → tCO₂e (GWP-weighted)
- Instalações de petróleo e gás são maiores emissoras de metano fugitivo

#### Sentinel-5P Mock (satellite_mock.py)
- Simula dados do satélite Sentinel-5P da ESA (Agência Espacial Europeia)
- Sentinel-5P mede concentração de CO₂ em coluna (XCO₂) em PPM na atmosfera
- Em produção real: API do Copernicus Atmosphere Data Store (ads.atmosphere.copernicus.eu)
- Mock: base de 418.5 PPM (média global 2024) + offset regional industrial (0–12 PPM)
- Esses dados enriquecem as leituras locais com contexto regional de poluição
