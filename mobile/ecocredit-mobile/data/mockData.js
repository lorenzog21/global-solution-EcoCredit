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
    status: "WARNING",
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
