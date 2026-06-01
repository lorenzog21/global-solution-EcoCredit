-- EcoCredit — Exemplos de Prevenção de SQL Injection
-- Demonstração educacional: vulnerável vs. seguro

-- =============================================
-- VULNERÁVEL (NUNCA FAZER — apenas demonstração)
-- =============================================

-- Input malicioso: ' OR '1'='1
-- Query resultante: SELECT * FROM TB_USER WHERE email = '' OR '1'='1'
-- Efeito: retorna TODOS os usuários — bypass de autenticação

-- Em SQL dinâmico (vulnerável):
-- EXECUTE IMMEDIATE 'SELECT * FROM TB_USER WHERE email = ''' || :user_input || '''';

-- =============================================
-- SEGURO: Query parametrizada
-- =============================================

-- Abordagem correta com bind variables (Oracle):
SELECT user_id, email, role, company_id
FROM TB_USER
WHERE email = :p_email        -- :p_email = bind variable — nunca concatenado
  AND active = 1;

-- O motor de banco trata :p_email como dado, nunca como SQL
-- Input ' OR '1'='1 vira literalmente a string ' OR '1'='1, sem efeito na lógica

-- =============================================
-- SEGURO: Entity Framework Core (C# — produção)
-- =============================================

-- O EF Core gera automaticamente:
-- SELECT * FROM "TB_USER" WHERE "email" = @p0
-- Equivalente a:
SELECT user_id, email, role, company_id
FROM TB_USER
WHERE email = :p0   -- p0 é parametrizado, nunca concatenado
  AND active = 1;

-- =============================================
-- SEGURO: Consulta de emissões com parâmetros
-- =============================================

-- Uso correto de parâmetros para consultar emissões de uma facility:
SELECT record_id, gas_type, quantity_tco2e, source, recorded_at
FROM TB_EMISSION_RECORD
WHERE facility_id = :p_facility_id          -- bind variable
  AND TRUNC(recorded_at, 'MM') = TRUNC(SYSTIMESTAMP, 'MM')
ORDER BY recorded_at DESC;

-- Uso correto de parâmetros para buscar créditos:
SELECT credit_id, quantity_tco2, credit_type, status, expiry_date
FROM TB_CARBON_CREDIT
WHERE company_id = :p_company_id            -- bind variable
  AND status = 'AVAILABLE'
  AND expiry_date > SYSDATE
ORDER BY expiry_date ASC;
