# ============================================================
# SCRIPT: Criar torcedores via API do Carona Alvinegra
# USO: Execute com a aplicação rodando (dotnet run)
# ============================================================

$apiBase = "http://localhost:5000/api"

# ============================================================
# 1. Primeiro, obter os IDs das rotas
# ============================================================
Write-Host "Buscando rotas..." -ForegroundColor Cyan
$rotas = Invoke-RestMethod -Uri "$apiBase/rotas" -Method GET

$rotaCascatinha  = $rotas | Where-Object { $_.nome -eq "Cascatinha" } | Select-Object -First 1
$rotaBingen      = $rotas | Where-Object { $_.nome -eq "Bingen" } | Select-Object -First 1
$rotaQuitandinha = $rotas | Where-Object { $_.nome -eq "Quitandinha" } | Select-Object -First 1
$rotaCentro      = $rotas | Where-Object { $_.nome -eq "Centro" } | Select-Object -First 1

# Se alguma rota não existir, criar
if (-not $rotaBingen) {
    Write-Host "Rota Bingen não encontrada. Criando..." -ForegroundColor Yellow
    $rotaBingen = Invoke-RestMethod -Uri "$apiBase/rotas" -Method POST -Body (@{ nome = "Bingen"; localEmbarque = "Bingen – Petrópolis" } | ConvertTo-Json) -ContentType "application/json"
}

Write-Host "Rotas carregadas!" -ForegroundColor Green

# ============================================================
# 2. Criar torcedores
# ============================================================

$totalCriados = 0
$totalExistentes = 0

function Criar-Torcedor($nome, $telefone, $rotaId) {
    param([string]$Nome, [string]$Telefone, [string]$RotaId)
    
    $body = @{
        nome               = $Nome
        telefone           = $Telefone
        rotaPreferencialId = $RotaId
        grupoId            = $null
    } | ConvertTo-Json
    
    try {
        $result = Invoke-RestMethod -Uri "$apiBase/usuarios" -Method POST -Body $body -ContentType "application/json"
        Write-Host "  ✅ $Nome" -ForegroundColor Green
        return $true
    } catch {
        if ($_.Exception.Response.StatusCode -eq 400) {
            # Pode ser duplicado
            Write-Host "  ⚠️  $Nome — já existe (ou erro de validação)" -ForegroundColor Yellow
            return $false
        }
        Write-Host "  ❌ $Nome — Erro: $_" -ForegroundColor Red
        return $false
    }
}

# =====================
# VAN 1 — Rota: Cascatinha
# =====================
Write-Host "`n🚐 VAN 1 — Cascatinha" -ForegroundColor Cyan
$van1 = @(
    "Pedrinho Santos", "Guaraci Costa", "Anne", "Ana Luiza",
    "Léo Mussel", "Gabi Mussel", "Mãe Mussel", "Guilherme Almeida",
    "Murillo Almeida", "Roberta Almeida", "Isabela Carvalho",
    "Léo Silva", "Anna Silva", "Lysa Silva", "Amanda Soares"
)
$tel = 1
foreach ($nome in $van1) {
    $telefone = "(24) 99901-{0:D4}" -f $tel
    if (Criar-Torcedor $nome $telefone $rotaCascatinha.id) { $totalCriados++ } else { $totalExistentes++ }
    $tel++
}

# =====================
# VAN 2 — Rota: Bingen
# =====================
Write-Host "`n🚐 VAN 2 — Bingen" -ForegroundColor Cyan
$van2 = @(
    "Tio Barbinha", "João Bernardo", "Emily", "Marcela Diniz",
    "Ester", "Gabriella Reis", "Daniel Rempto", "Renan Costa",
    "Sr(a) Isabelle Costa", "Nicolle"
)
$tel = 1
foreach ($nome in $van2) {
    $telefone = "(24) 99902-{0:D4}" -f $tel
    if (Criar-Torcedor $nome $telefone $rotaBingen.id) { $totalCriados++ } else { $totalExistentes++ }
    $tel++
}

# =====================
# VAN 3 — Rota: Quitandinha
# =====================
Write-Host "`n🚐 VAN 3 — Quitandinha" -ForegroundColor Cyan
$van3 = @(
    "Dadock", "Guilherme França", "Fonseca", "Vitinho Lacerda",
    "Isaac Lacerda", "Monique Palma", "Aline Palma", "Miguel Palma",
    "Tia Valéria Gama", "Márcia Gama", "Luís Gustavo", "Cassiano",
    "Giovani Carvalho", "Giovana Nicolay"
)
$tel = 1
foreach ($nome in $van3) {
    $telefone = "(24) 99903-{0:D4}" -f $tel
    if (Criar-Torcedor $nome $telefone $rotaQuitandinha.id) { $totalCriados++ } else { $totalExistentes++ }
    $tel++
}

# =====================
# SPIN — Rota: Centro
# =====================
Write-Host "`n🚗 SPIN — Centro" -ForegroundColor Cyan
$spin = @(
    "Jones", "Lucas Barbosa", "Luizeto", "César Vallejo", "Rian Praxedes"
)
$tel = 1
foreach ($nome in $spin) {
    $telefone = "(24) 99904-{0:D4}" -f $tel
    if (Criar-Torcedor $nome $telefone $rotaCentro.id) { $totalCriados++ } else { $totalExistentes++ }
    $tel++
}

# ============================================================
# 3. Resumo
# ============================================================
Write-Host "`n========================================" -ForegroundColor Cyan
Write-Host "RESUMO" -ForegroundColor Cyan
Write-Host "  ✅ Criados: $totalCriados" -ForegroundColor Green
Write-Host "  ⚠️  Já existentes / erros: $totalExistentes" -ForegroundColor Yellow
Write-Host "  📁 Total: $($totalCriados + $totalExistentes)" -ForegroundColor White
Write-Host "========================================" -ForegroundColor Cyan
