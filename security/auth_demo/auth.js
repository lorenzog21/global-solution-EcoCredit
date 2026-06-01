// Demo de segurança standalone — sem dependência da API
// Demonstra: sanitização XSS, validação de input, mensagem de erro genérica

const MOCK_USERS = [
  {
    email: "admin@petro.com",
    // Hash BCrypt workFactor 12 de "Admin@2026"
    // Em produção: hash gerado server-side, nunca armazenado em plain text
    passwordPlain: "Admin@2026",
    role: "ADMIN",
    companyId: "comp-001"
  }
];

// Sanitização de input — proteção XSS básica
function sanitizeInput(input) {
  return input
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;")
    .replace(/'/g, "&#x27;")
    .replace(/\//g, "&#x2F;")
    .trim();
}

// Validação de email — primeira linha de defesa
function validateEmail(email) {
  const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
  return emailRegex.test(email) && email.length <= 200;
}

// Validação de senha
function validatePassword(password) {
  return password.length >= 8 && password.length <= 128;
}

function handleLogin(event) {
  event.preventDefault();

  const rawEmail = document.getElementById('email').value;
  const password = document.getElementById('password').value;

  // Sanitiza antes de qualquer operação
  const email = sanitizeInput(rawEmail);

  // Validações client-side — primeira linha de defesa
  if (!validateEmail(email)) {
    showMessage("Email inválido.", false);
    return;
  }

  if (!validatePassword(password)) {
    showMessage("Senha deve ter entre 8 e 128 caracteres.", false);
    return;
  }

  // Simula verificação server-side
  // Em produção: POST /api/v1/auth/login → BCrypt.Verify(password, storedHash)
  const user = MOCK_USERS.find(u => u.email === email);

  // Mensagem genérica intencional — não revela se o email ou a senha está errado
  // Isso previne enumeração de usuários (user enumeration attack)
  if (!user || user.passwordPlain !== password) {
    showMessage("Credenciais inválidas.", false);
    return;
  }

  // Simulação do JWT que seria gerado
  const mockJwt = btoa(JSON.stringify({
    sub: "user-001",
    company_id: user.companyId,
    role: user.role,
    exp: Math.floor(Date.now() / 1000) + (8 * 3600)
  }));

  showMessage(
    `✅ Login realizado com sucesso!\nRole: ${user.role} | Empresa: ${user.companyId}\n` +
    `JWT (simulado): Bearer ${mockJwt.substring(0, 20)}...`,
    true
  );
}

function showMessage(msg, success) {
  const el = document.getElementById('feedback');
  el.style.display = 'block';
  el.textContent = msg;
  el.style.color = success ? '#00e87a' : '#ff4d6a';
  el.style.borderLeft = `2px solid ${success ? '#00e87a' : '#ff4d6a'}`;
}
