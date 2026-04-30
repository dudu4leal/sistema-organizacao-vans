/* ══════════════════════════════════════════════
   SPA — Carona Alvinegra
   ══════════════════════════════════════════════ */

// ── State ──
let rotasCache = [];
let usuariosCache = [];
let gruposCache = [];

// ── Helpers ──
const $ = (id) => document.getElementById(id);
const qs = (sel) => document.querySelector(sel);
const qsa = (sel) => document.querySelectorAll(sel);

function toast(msg, type = 'info') {
    const container = $('toast-container');
    const el = document.createElement('div');
    el.className = `toast ${type}`;
    el.textContent = msg;
    container.appendChild(el);
    setTimeout(() => { el.style.opacity = '0'; setTimeout(() => el.remove(), 300); }, 3500);
}

// ── Login ──
$('login-form').addEventListener('submit', (e) => {
    e.preventDefault();
    const nome = $('login-nome').value.trim();
    if (!nome) return;
    $('login-screen').style.display = 'none';
    $('app').style.display = 'flex';
    toast(`Bem-vindo, ${nome}!`, 'success');
    carregarPagina('dashboard');
});

$('btn-logout').addEventListener('click', () => {
    $('app').style.display = 'none';
    $('login-screen').style.display = 'flex';
    $('login-nome').value = '';
    toast('Sessão encerrada', 'info');
});

// ── Navigation ──
qsa('.nav-item').forEach(a => {
    a.addEventListener('click', (e) => {
        e.preventDefault();
        const page = a.dataset.page;
        qsa('.nav-item').forEach(n => n.classList.remove('active'));
        a.classList.add('active');
        qsa('.page').forEach(p => p.classList.remove('active'));
        const el = $(`page-${page}`);
        if (el) el.classList.add('active');
        carregarPagina(page);
    });
});

function carregarPagina(page) {
    switch (page) {
        case 'dashboard': loadDashboard(); break;
        case 'usuarios': loadUsuarios(); break;
        case 'grupos': loadGrupos(); break;
        case 'rotas': loadRotas(); break;
        case 'jogos': loadJogos(); break;
    }
}

// ══════════════════════════════════════════════
//  DASHBOARD
// ══════════════════════════════════════════════

async function loadDashboard() {
    const el = $('dashboard-content');
    el.innerHTML = '<div class="loading"><div class="spinner"></div><p>Carregando...</p></div>';
    try {
        const [usuarios, grupos, rotas, jogos] = await Promise.all([
            UsuarioApi.listar(), GrupoApi.listar(), RotaApi.listar(), JogoApi.listar()
        ]);
        rotasCache = rotas;
        gruposCache = grupos;
        usuariosCache = usuarios;

        el.innerHTML = `
            <div class="stats-grid">
                <div class="stat-card">
                    <div class="stat-icon dark">&#128101;</div>
                    <div class="stat-info"><h4>${usuarios.length}</h4><p>Torcedores</p></div>
                </div>
                <div class="stat-card">
                    <div class="stat-icon gray">&#128101;&#128101;</div>
                    <div class="stat-info"><h4>${grupos.length}</h4><p>Grupos</p></div>
                </div>
                <div class="stat-card">
                    <div class="stat-icon dark">&#128204;</div>
                    <div class="stat-info"><h4>${rotas.length}</h4><p>Rotas</p></div>
                </div>
                <div class="stat-card">
                    <div class="stat-icon gray">&#9917;</div>
                    <div class="stat-info"><h4>${jogos.length}</h4><p>Jogos</p></div>
                </div>
            </div>
            <div class="card">
                <h3>&#128161; Como usar</h3>
                <ol style="padding-left:1.2rem;font-size:0.9rem;color:var(--gray700);line-height:1.8">
                    <li><strong>Rotas</strong> — Cadastre os pontos de embarque disponíveis</li>
                    <li><strong>Torcedores</strong> — Cadastre os passageiros com rota preferencial</li>
                    <li><strong>Grupos</strong> — Organize torcedores em grupos (opcional)</li>
                    <li><strong>Jogos</strong> — Crie um jogo, marque presenças e gere as vans</li>
                </ol>
            </div>
        `;
    } catch (err) {
        el.innerHTML = `<div class="empty-state"><p style="color:var(--error)">Erro: ${err.message}</p></div>`;
    }
}

// ══════════════════════════════════════════════
//  USUÁRIOS (CRUD)
// ══════════════════════════════════════════════

async function loadUsuarios() {
    const el = $('usuarios-list');
    el.innerHTML = '<div class="loading"><div class="spinner"></div><p>Carregando...</p></div>';
    try {
        const [usuarios, grupos, rotas] = await Promise.all([
            UsuarioApi.listar(), GrupoApi.listar(), RotaApi.listar()
        ]);
        usuariosCache = usuarios;
        gruposCache = grupos;
        rotasCache = rotas;

        if (usuarios.length === 0) {
            el.innerHTML = '<div class="empty-state"><div class="empty-icon">&#128101;</div><p>Nenhum torcedor cadastrado</p></div>';
            return;
        }

        let html = '<div class="card"><div class="table-container"><table><thead><tr><th>Nome</th><th>Telefone</th><th>Rota</th><th>Grupo</th><th>Ações</th></tr></thead><tbody>';
        usuarios.forEach(u => {
            const rota = rotas.find(r => r.id === u.rotaPreferencialId);
            const grupo = grupos.find(g => g.id === u.grupoId);
            html += `<tr>
                <td><strong>${u.nome}</strong></td>
                <td>${u.telefone || '<span style="color:var(--gray400)">—</span>'}</td>
                <td><span class="badge badge-black">${rota ? rota.nome : 'N/D'}</span></td>
                <td>${grupo ? `<span class="badge badge-gray">${grupo.nome}</span>` : '<span style="color:var(--gray400)">Avulso</span>'}</td>
                <td class="actions-cell">
                    <button class="btn btn-secondary btn-sm" onclick="editarUsuario('${u.id}')">&#9998;</button>
                    <button class="btn btn-danger btn-sm" onclick="removerUsuario('${u.id}')">&#128465;</button>
                </td>
            </tr>`;
        });
        html += '</tbody></table></div></div>';
        el.innerHTML = html;
    } catch (err) {
        el.innerHTML = `<div class="empty-state"><p style="color:var(--error)">Erro: ${err.message}</p></div>`;
    }
}

function abrirModal(tipo, data = null) {
    const overlay = $('modal-overlay');
    const title = $('modal-title');
    const body = $('modal-body');

    if (tipo === 'usuario') {
        title.textContent = data ? 'Editar Torcedor' : 'Novo Torcedor';
        const rotasOpts = rotasCache.map(r => `<option value="${r.id}" ${data?.rotaPreferencialId === r.id ? 'selected' : ''}>${r.nome}</option>`).join('');
        const gruposOpts = gruposCache.map(g => `<option value="${g.id}" ${data?.grupoId === g.id ? 'selected' : ''}>${g.nome}</option>`).join('');
        body.innerHTML = `
            <form id="modal-form">
                <div class="form-group">
                    <label for="f-nome">Nome completo *</label>
                    <input type="text" id="f-nome" value="${data?.nome || ''}" required>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label for="f-telefone">Telefone</label>
                        <input type="text" id="f-telefone" value="${data?.telefone || ''}" placeholder="(21) 99999-9999">
                    </div>
                    <div class="form-group">
                        <label for="f-rota">Rota preferencial *</label>
                        <select id="f-rota" required><option value="">Selecione...</option>${rotasOpts}</select>
                    </div>
                </div>
                <div class="form-group">
                    <label for="f-grupo">Grupo</label>
                    <select id="f-grupo"><option value="">— Sem grupo —</option>${gruposOpts}</select>
                </div>
            </form>
            <div class="modal-footer">
                <button class="btn btn-secondary" onclick="fecharModal()">Cancelar</button>
                <button class="btn btn-primary" onclick="salvarUsuario('${data?.id || ''}')">Salvar</button>
            </div>`;
    } else if (tipo === 'grupo') {
        title.textContent = 'Novo Grupo';
        body.innerHTML = `
            <form id="modal-form">
                <div class="form-group">
                    <label for="f-grupo-nome">Nome do grupo *</label>
                    <input type="text" id="f-grupo-nome" placeholder="Ex: Amigos do Bairro" required>
                </div>
            </form>
            <div class="modal-footer">
                <button class="btn btn-secondary" onclick="fecharModal()">Cancelar</button>
                <button class="btn btn-primary" onclick="salvarGrupo()">Criar Grupo</button>
            </div>`;
    } else if (tipo === 'rota') {
        title.textContent = data ? 'Editar Rota' : 'Nova Rota';
        body.innerHTML = `
            <form id="modal-form">
                <div class="form-group">
                    <label for="f-rota-nome">Nome da rota *</label>
                    <input type="text" id="f-rota-nome" value="${data?.nome || ''}" placeholder="Ex: Cascatinha" required>
                </div>
                <div class="form-group">
                    <label for="f-rota-local">Local de embarque *</label>
                    <input type="text" id="f-rota-local" value="${data?.localEmbarque || ''}" placeholder="Ex: Praça da Cascatinha" required>
                </div>
            </form>
            <div class="modal-footer">
                <button class="btn btn-secondary" onclick="fecharModal()">Cancelar</button>
                <button class="btn btn-primary" onclick="salvarRota('${data?.id || ''}')">Salvar</button>
            </div>`;
    } else if (tipo === 'jogo') {
        title.textContent = 'Novo Jogo';
        body.innerHTML = `
            <form id="modal-form">
                <div class="form-group">
                    <label for="f-jogo-adversario">Adversário *</label>
                    <input type="text" id="f-jogo-adversario" placeholder="Ex: Flamengo" required>
                </div>
                <div class="form-row">
                    <div class="form-group">
                        <label for="f-jogo-data">Data e hora *</label>
                        <input type="datetime-local" id="f-jogo-data" required>
                    </div>
                    <div class="form-group">
                        <label for="f-jogo-local">Local *</label>
                        <input type="text" id="f-jogo-local" placeholder="Ex: Maracanã" required>
                    </div>
                </div>
            </form>
            <div class="modal-footer">
                <button class="btn btn-secondary" onclick="fecharModal()">Cancelar</button>
                <button class="btn btn-primary" onclick="salvarJogo()">Criar Jogo</button>
            </div>`;
    }
    overlay.style.display = 'flex';
}

function fecharModal() {
    $('modal-overlay').style.display = 'none';
}

async function salvarUsuario(id) {
    const nome = $('f-nome').value.trim();
    const telefone = $('f-telefone').value.trim();
    const rotaId = $('f-rota').value;
    const grupoId = $('f-grupo').value;

    if (!nome || !rotaId) { toast('Preencha nome e rota!', 'warning'); return; }

    try {
        if (id) {
            await UsuarioApi.atualizar(id, { nome, telefone: telefone || null, rotaPreferencialId: rotaId, grupoId: grupoId || null });
            toast('Torcedor atualizado!', 'success');
        } else {
            await UsuarioApi.criar({ nome, telefone: telefone || null, rotaPreferencialId: rotaId, grupoId: grupoId || null });
            toast('Torcedor cadastrado!', 'success');
        }
        fecharModal();
        loadUsuarios();
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

async function editarUsuario(id) {
    try {
        const u = await UsuarioApi.obter(id);
        if (u) abrirModal('usuario', u);
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

async function removerUsuario(id) {
    if (!confirm('Remover este torcedor?')) return;
    try {
        await UsuarioApi.remover(id);
        toast('Torcedor removido', 'info');
        loadUsuarios();
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

// ══════════════════════════════════════════════
//  GRUPOS (com Drag & Drop)
// ══════════════════════════════════════════════

let draggedUser = null;

async function loadGrupos() {
    const el = $('grupos-container');
    el.innerHTML = '<div class="loading"><div class="spinner"></div><p>Carregando...</p></div>';
    try {
        const [grupos, usuarios] = await Promise.all([
            GrupoApi.listar(), UsuarioApi.listar()
        ]);
        gruposCache = grupos;
        usuariosCache = usuarios;

        // Usuários sem grupo (avulsos)
        const avulsos = usuarios.filter(u => !u.grupoId);

        let html = '<div class="grupos-grid">';
        grupos.forEach(g => {
            const membros = usuarios.filter(u => u.grupoId === g.id);
            html += `
                <div class="grupo-card" data-grupo-id="${g.id}">
                    <div class="grupo-card-header">
                        <h3>${g.nome}</h3>
                        <span class="badge badge-gray">${g.totalMembros}</span>
                    </div>
                    <div class="grupo-card-body drop-zone" data-grupo-id="${g.id}">
                        ${membros.length === 0 ? '<div class="grupo-empty">Arraste usuários para cá</div>' :
                            membros.map(m => `
                                <div class="grupo-membro" draggable="true" data-usuario-id="${m.id}">
                                    <span>&#128100; ${m.nome}</span>
                                    <button class="grupo-membro-remove" onclick="removerMembroGrupo('${g.id}','${m.id}')" title="Remover do grupo">&times;</button>
                                </div>`).join('')}
                    </div>
                </div>`;
        });
        html += '</div>';

        // Seção de avulsos
        html += `
            <div class="grupo-avulsos">
                <div class="grupo-avulsos-header">&#128101; Torcedores Avulsos (${avulsos.length})</div>
                <div class="grupo-avulsos-body drop-zone" data-grupo-id="">
                    ${avulsos.length === 0 ? '<div class="grupo-empty">Todos os torcedores estão em grupos</div>' :
                        avulsos.map(u => `
                            <div class="grupo-membro" draggable="true" data-usuario-id="${u.id}" style="display:inline-flex;margin:0.25rem">
                                &#128100; ${u.nome}
                            </div>`).join('')}
                </div>
            </div>`;

        el.innerHTML = html;
        initDragDrop();
    } catch (err) {
        el.innerHTML = `<div class="empty-state"><p style="color:var(--error)">Erro: ${err.message}</p></div>`;
    }
}

function initDragDrop() {
    qsa('.grupo-membro[draggable]').forEach(el => {
        el.addEventListener('dragstart', (e) => {
            draggedUser = el.dataset.usuarioId;
            e.dataTransfer.effectAllowed = 'move';
            el.style.opacity = '0.4';
        });
        el.addEventListener('dragend', (e) => {
            el.style.opacity = '1';
        });
    });

    qsa('.drop-zone').forEach(zone => {
        zone.addEventListener('dragover', (e) => {
            e.preventDefault();
            e.dataTransfer.dropEffect = 'move';
            zone.style.background = 'var(--gray100)';
        });
        zone.addEventListener('dragleave', () => {
            zone.style.background = '';
        });
        zone.addEventListener('drop', async (e) => {
            e.preventDefault();
            zone.style.background = '';
            if (!draggedUser) return;
            const novoGrupoId = zone.dataset.grupoId || null;
            await moverUsuarioGrupo(draggedUser, novoGrupoId);
            draggedUser = null;
        });
    });
}

async function moverUsuarioGrupo(usuarioId, novoGrupoId) {
    try {
        // Se está movendo para avulsos (novoGrupoId vazio), precisamos remover do grupo atual
        if (!novoGrupoId) {
            // Encontrar usuario e atualizar grupoId para null
            const usuario = usuariosCache.find(u => u.id === usuarioId);
            if (usuario) {
                await UsuarioApi.atualizar(usuarioId, {
                    nome: usuario.nome,
                    telefone: usuario.telefone,
                    rotaPreferencialId: usuario.rotaPreferencialId,
                    grupoId: null
                });
            }
        } else {
            await GrupoApi.adicionarMembro(novoGrupoId, usuarioId);
        }
        toast('Membro movido!', 'success');
        loadGrupos();
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

async function removerMembroGrupo(grupoId, usuarioId) {
    try {
        const usuario = usuariosCache.find(u => u.id === usuarioId);
        if (usuario) {
            await UsuarioApi.atualizar(usuarioId, {
                nome: usuario.nome,
                telefone: usuario.telefone,
                rotaPreferencialId: usuario.rotaPreferencialId,
                grupoId: null
            });
        }
        toast('Membro removido do grupo', 'info');
        loadGrupos();
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

async function salvarGrupo() {
    const nome = $('f-grupo-nome').value.trim();
    if (!nome) { toast('Informe o nome do grupo!', 'warning'); return; }
    try {
        await GrupoApi.criar({ nome });
        toast('Grupo criado!', 'success');
        fecharModal();
        loadGrupos();
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

// ══════════════════════════════════════════════
//  ROTAS (CRUD)
// ══════════════════════════════════════════════

async function loadRotas() {
    const el = $('rotas-list');
    el.innerHTML = '<div class="loading"><div class="spinner"></div><p>Carregando...</p></div>';
    try {
        const rotas = await RotaApi.listar();
        rotasCache = rotas;

        if (rotas.length === 0) {
            el.innerHTML = '<div class="empty-state"><div class="empty-icon">&#128204;</div><p>Nenhuma rota cadastrada</p></div>';
            return;
        }

        let html = '<div class="card"><div class="table-container"><table><thead><tr><th>Rota</th><th>Local de Embarque</th><th>Ações</th></tr></thead><tbody>';
        rotas.forEach(r => {
            html += `<tr>
                <td><strong>${r.nome}</strong></td>
                <td>${r.localEmbarque}</td>
                <td class="actions-cell">
                    <button class="btn btn-danger btn-sm" onclick="removerRota('${r.id}')">&#128465;</button>
                </td>
            </tr>`;
        });
        html += '</tbody></table></div></div>';
        el.innerHTML = html;
    } catch (err) {
        el.innerHTML = `<div class="empty-state"><p style="color:var(--error)">Erro: ${err.message}</p></div>`;
    }
}

async function salvarRota(id) {
    const nome = $('f-rota-nome').value.trim();
    const local = $('f-rota-local').value.trim();
    if (!nome || !local) { toast('Preencha todos os campos!', 'warning'); return; }
    try {
        await RotaApi.criar({ nome, localEmbarque: local });
        toast('Rota criada!', 'success');
        fecharModal();
        loadRotas();
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

async function removerRota(id) {
    if (!confirm('Remover esta rota?')) return;
    try {
        await RotaApi.remover(id);
        toast('Rota removida', 'info');
        loadRotas();
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

// ══════════════════════════════════════════════
//  JOGOS
// ══════════════════════════════════════════════

async function loadJogos() {
    const el = $('jogos-list');
    el.innerHTML = '<div class="loading"><div class="spinner"></div><p>Carregando...</p></div>';
    try {
        const jogos = await JogoApi.listar();

        if (jogos.length === 0) {
            el.innerHTML = '<div class="empty-state"><div class="empty-icon">&#9917;</div><p>Nenhum jogo cadastrado</p></div>';
            return;
        }

        let html = '<div class="jogos-grid">';
        jogos.forEach(j => {
            const data = new Date(j.data);
            const dataStr = data.toLocaleDateString('pt-BR', { day: '2-digit', month: 'long', year: 'numeric', hour: '2-digit', minute: '2-digit' });
            html += `
                <div class="jogo-card">
                    <div class="jogo-card-header">
                        <div>
                            <h3>Botafogo &times; ${j.adversario}</h3>
                            <div class="jogo-data">&#128197; ${dataStr} &bull; ${j.local}</div>
                        </div>
                        <span class="badge ${j.alocacaoRealizada ? 'badge-success' : 'badge-warning'}">
                            ${j.alocacaoRealizada ? 'Alocado' : 'Pendente'}
                        </span>
                    </div>
                    <div class="jogo-card-body">
                        <span class="badge badge-black">${j.totalPresentes} presenças</span>
                    </div>
                    <div class="jogo-card-footer">
                        <button class="btn btn-primary btn-sm" onclick="gerenciarPresencas('${j.id}')">&#128203; Presenças</button>
                        ${j.totalPresentes > 0 ? `<button class="btn btn-success btn-sm" onclick="alocarJogo('${j.id}')">&#128666; Gerar Vans</button>` : ''}
                        <button class="btn btn-secondary btn-sm" onclick="verAlocacao('${j.id}')">&#128269; Ver Resultado</button>
                        <button class="btn btn-danger btn-sm" onclick="removerJogo('${j.id}')">&#128465;</button>
                    </div>
                </div>`;
        });
        html += '</div>';
        el.innerHTML = html;
    } catch (err) {
        el.innerHTML = `<div class="empty-state"><p style="color:var(--error)">Erro: ${err.message}</p></div>`;
    }
}

async function salvarJogo() {
    const adv = $('f-jogo-adversario').value.trim();
    const data = $('f-jogo-data').value;
    const local = $('f-jogo-local').value.trim();
    if (!adv || !data || !local) { toast('Preencha todos os campos!', 'warning'); return; }
    try {
        await JogoApi.criar({ adversario: adv, data: new Date(data).toISOString(), local });
        toast('Jogo criado!', 'success');
        fecharModal();
        loadJogos();
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

async function removerJogo(id) {
    if (!confirm('Remover este jogo?')) return;
    try {
        await JogoApi.remover(id);
        toast('Jogo removido', 'info');
        loadJogos();
        $('alocacao-resultado').innerHTML = '';
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

// ══════════════════════════════════════════════
//  PRESENÇAS
// ══════════════════════════════════════════════

async function gerenciarPresencas(jogoId) {
    try {
        const [presencas, usuarios] = await Promise.all([
            PresencaApi.listar(jogoId), UsuarioApi.listar()
        ]);

        const presenteIds = presencas.map(p => p.usuarioId);

        let html = `
            <div style="margin-bottom:0.75rem;display:flex;justify-content:space-between;align-items:center">
                <span style="font-weight:600;font-size:0.9rem">${presenteIds.length} de ${usuarios.length} confirmados</span>
            </div>
            <div class="presenca-list">`;

        usuarios.forEach(u => {
            const isPresente = presenteIds.includes(u.id);
            html += `
                <div class="presenca-item">
                    <input type="checkbox" id="pres-${u.id}" ${isPresente ? 'checked' : ''}
                        onchange="togglePresenca('${jogoId}', '${u.id}', this.checked, '${u.rotaPreferencialId}')">
                    <label for="pres-${u.id}">&#128100; ${u.nome}</label>
                </div>`;
        });

        html += '</div>';

        $('modal-title').textContent = 'Gerenciar Presenças';
        $('modal-body').innerHTML = html + `
            <div class="modal-footer">
                <button class="btn btn-secondary" onclick="fecharModal()">Fechar</button>
            </div>`;
        $('modal-overlay').style.display = 'flex';
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

async function togglePresenca(jogoId, usuarioId, checked, rotaId) {
    try {
        if (checked) {
            await PresencaApi.marcar(jogoId, { usuarioId, rotaEfetivaId: rotaId });
            toast('Presença marcada!', 'success');
        } else {
            // Find presenca by usuarioId to remove it
            const presencas = await PresencaApi.listar(jogoId);
            const presenca = presencas.find(p => p.usuarioId === usuarioId);
            if (presenca) {
                await PresencaApi.remover(jogoId, presenca.id);
                toast('Presença removida', 'info');
            }
        }
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

// ══════════════════════════════════════════════
//  ALOCAÇÃO (Gerar Vans)
// ══════════════════════════════════════════════

async function alocarJogo(jogoId) {
    try {
        const resultado = await JogoApi.alocar(jogoId);
        exibirResultado(resultado);
        toast('Alocação realizada com sucesso!', 'success');
        loadJogos();
    } catch (err) { toast(`Erro na alocação: ${err.message}`, 'error'); }
}

async function verAlocacao(jogoId) {
    try {
        const resultado = await JogoApi.alocar(jogoId);
        exibirResultado(resultado);
    } catch (err) { toast(`Erro: ${err.message}`, 'error'); }
}

function exibirResultado(r) {
    const container = $('alocacao-resultado');
    if (!r.sucesso && r.erros?.length > 0) {
        container.innerHTML = `
            <div class="card">
                <h3>&#9888; Erros na alocação</h3>
                <ul style="padding-left:1.2rem;color:var(--error)">
                    ${r.erros.map(e => `<li>${e}</li>`).join('')}
                </ul>
            </div>`;
        return;
    }

    let html = `<div class="card">
        <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:0.75rem">
            <h3 style="margin:0">&#128666; Resultado — ${r.jogoDescricao || ''}</h3>
            <button class="btn btn-success btn-sm" onclick="copiarWhatsApp('${r.jogoId}')">&#128172; Copiar para WhatsApp</button>
        </div>`;

    if (r.veiculos?.length > 0) {
        html += '<div class="veiculos-grid">';
        r.veiculos.forEach(v => {
            const tipoClass = v.classificacao === 0 ? 'van' : v.classificacao === 1 ? 'doblo' : 'espera';
            const tipoBadge = v.classificacao === 0 ? 'Van' : v.classificacao === 1 ? 'Doblo/Spin' : 'Lista de Espera';
            html += `
                <div class="veiculo-card tipo-${tipoClass}">
                    <div class="veiculo-header">
                        <span class="veiculo-nome">${v.tipoDescricao || `Veículo ${v.ordem + 1}`}</span>
                        <span class="veiculo-tipo ${tipoClass}">${tipoBadge}</span>
                    </div>
                    <div class="veiculo-lotacao">${v.lotacao} vagas &bull; ${v.vagasRestantes} livres</div>
                    <ol class="passageiros-list" start="1">
                        ${v.passageiros.map(p => `
                            <li>
                                ${p.isLider ? '<span class="lider-badge">Líder</span>' : ''}
                                ${p.nome} ${p.telefone ? `&bull; ${p.telefone}` : ''}
                            </li>`).join('')}
                    </ol>
                </div>`;
        });
        html += '</div>';
    }

    if (r.listaEspera?.length > 0) {
        html += `
            <div style="margin-top:1rem;padding:0.75rem;background:#fef3c7;border-radius:var(--radius)">
                <strong>&#9888; Lista de Espera (${r.listaEspera.length})</strong>
                <div style="margin-top:0.35rem;font-size:0.85rem">
                    ${r.listaEspera.map(p => `&#10003; ${p.nome}`).join('<br>')}
                </div>
            </div>`;
    }

    html += '</div>';
    container.innerHTML = html;

    // Scroll to result
    container.scrollIntoView({ behavior: 'smooth', block: 'start' });
}

async function copiarWhatsApp(jogoId) {
    try {
        const resultado = await JogoApi.alocar(jogoId);
        let texto = `*🚐 CARONA ALVINEGRA*\n*${resultado.jogoDescricao || 'Jogo'}*\n\n`;

        if (resultado.veiculos) {
            resultado.veiculos.forEach(v => {
                const tipo = v.classificacao === 0 ? '🚐 Van' : v.classificacao === 1 ? '🚗 Doblo/Spin' : '⏳ Espera';
                texto += `*${tipo}* (${v.lotacao} vagas)\n`;
                v.passageiros.forEach(p => {
                    const lider = p.isLider ? ' 👑' : '';
                    texto += `  ${p.numero}. ${p.nome}${lider}\n`;
                });
                texto += '\n';
            });
        }

        if (resultado.listaEspera?.length > 0) {
            texto += `*⏳ Lista de Espera (${resultado.listaEspera.length})*\n`;
            resultado.listaEspera.forEach(p => { texto += `  ${p.nome}\n`; });
        }

        await navigator.clipboard.writeText(texto);
        toast('Lista copiada para a área de transferência!', 'success');
    } catch (err) { toast(`Erro ao copiar: ${err.message}`, 'error'); }
}

// ── Load dashboard on startup ──
