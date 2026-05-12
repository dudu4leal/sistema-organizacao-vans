/* ══════════════════════════════════════════════
   API CLIENT — Carona Alvinegra
   ══════════════════════════════════════════════ */

// API_BASE pode ser sobrescrita via variável global (ex: __API_BASE__)
// ou via window.env.API_BASE para configuração cross-origin no Desktop app.
const API_BASE = (typeof __API_BASE__ !== 'undefined' ? __API_BASE__ :
                  (window.env && window.env.API_BASE ? window.env.API_BASE : ''));

async function apiRequest(method, path, body = null) {
    const options = {
        method,
        headers: { 'Content-Type': 'application/json' }
    };
    if (body) options.body = JSON.stringify(body);

    const response = await fetch(`${API_BASE}${path}`, options);

    if (!response.ok) {
        const data = await response.json().catch(() => ({}));
        throw new Error(data.error || data.title || `Erro ${response.status}`);
    }

    if (response.status === 204) return null;
    try { return await response.json(); }
    catch { return null; }
}

// ── Usuários ──
const UsuarioApi = {
    listar: () => apiRequest('GET', '/api/usuarios'),
    criar: (data) => apiRequest('POST', '/api/usuarios', data),
    obter: (id) => apiRequest('GET', `/api/usuarios/${id}`),
    atualizar: (id, data) => apiRequest('PUT', `/api/usuarios/${id}`, data),
    remover: (id) => apiRequest('DELETE', `/api/usuarios/${id}`),
};

// ── Grupos ──
const GrupoApi = {
    listar: () => apiRequest('GET', '/api/grupos'),
    criar: (data) => apiRequest('POST', '/api/grupos', data),
    adicionarMembro: (grupoId, usuarioId) =>
        apiRequest('POST', `/api/grupos/${grupoId}/membros/${usuarioId}`),
    removerMembro: (grupoId, usuarioId) =>
        apiRequest('DELETE', `/api/grupos/${grupoId}/membros/${usuarioId}`),
    remover: (id) => apiRequest('DELETE', `/api/grupos/${id}`),
};

// ── Rotas ──
const RotaApi = {
    listar: () => apiRequest('GET', '/api/rotas'),
    obter: (id) => apiRequest('GET', `/api/rotas/${id}`),
    criar: (data) => apiRequest('POST', '/api/rotas', data),
    remover: (id) => apiRequest('DELETE', `/api/rotas/${id}`),
};

// ── Jogos ──
const JogoApi = {
    listar: () => apiRequest('GET', '/api/jogos'),
    criar: (data) => apiRequest('POST', '/api/jogos', data),
    obter: (id) => apiRequest('GET', `/api/jogos/${id}`),
    alocar: (id) => apiRequest('POST', `/api/jogos/${id}/alocar`),
    obterAlocacao: (id) => apiRequest('GET', `/api/jogos/${id}/alocar`),
    definirLider: (jogoId, veiculoOrdem, usuarioId) =>
        apiRequest('POST', `/api/jogos/${jogoId}/veiculos/${veiculoOrdem}/lider/${usuarioId}`),
    remover: (id) => apiRequest('DELETE', `/api/jogos/${id}`),
};

// ── Presenças ──
const PresencaApi = {
    listar: (jogoId) => apiRequest('GET', `/api/jogos/${jogoId}/presencas`),
    marcar: (jogoId, data) => apiRequest('POST', `/api/jogos/${jogoId}/presencas`, data),
    remover: (jogoId, presencaId) =>
        apiRequest('DELETE', `/api/jogos/${jogoId}/presencas/${presencaId}`),
};
