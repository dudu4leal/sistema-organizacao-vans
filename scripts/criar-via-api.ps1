# ============================================================
# SCRIPT: Criar 44 torcedores via API
# USO: 1. Inicie a aplicacao (dotnet run)
#       2. Execute este script com a app rodando
# ============================================================

$apiBase = "http://localhost:5000/api"

Write-Host "==============================" -ForegroundColor Cyan
Write-Host " 1. OBTENDO ROTAS" -ForegroundColor Cyan
Write-Host "==============================" -ForegroundColor Cyan

$rotas = Invoke-RestMethod -Uri "$apiBase/rotas" -Method GET
$rotaBingen      = $rotas | Where-Object { $_.nome -eq "Bingen" } | Select-Object -First 1
$rotaQuitandinha = $rotas | Where-Object { $_.nome -eq "Quitandinha" } | Select-Object -First 1

Write-Host "  Bingen: $($rotaBingen.id)" -ForegroundColor Green
Write-Host "  Quitandinha: $($rotaQuitandinha.id)" -ForegroundColor Green

Write-Host "`n==============================" -ForegroundColor Cyan
Write-Host " 2. CRIANDO TORCEDORES" -ForegroundColor Cyan
Write-Host "==============================" -ForegroundColor Cyan

function Criar-Torcedor($Nome, $Telefone, $RotaId) {
    $body = @{
        nome               = $Nome
        telefone           = $Telefone
        rotaPreferencialId = $RotaId
        grupoId            = $null
    } | ConvertTo-Json

    try {
        $result = Invoke-RestMethod -Uri "$apiBase/usuarios" -Method POST -Body $body -ContentType "application/json"
        Write-Host "  $Nome" -ForegroundColor Green
        return $true
    } catch {
        Write-Host "  $Nome - Erro: $($_.Exception.Message)" -ForegroundColor Yellow
        return $false
    }
}

$totalCriados = 0

# --- BINGEN (20 pessoas) ---
Write-Host "`n--- BINGEN (20 pessoas) ---" -ForegroundColor Cyan
$bingen = @(
    @{Nome="Tio Barbinha"; Tel="(24) 99902-0001"},
    @{Nome="Joao Bernardo"; Tel="(24) 99902-0002"},
    @{Nome="Emily"; Tel="(24) 99902-0003"},
    @{Nome="Marcela Diniz"; Tel="(24) 99902-0004"},
    @{Nome="Ester"; Tel="(24) 99902-0005"},
    @{Nome="Gabriella Reis"; Tel="(24) 99902-0006"},
    @{Nome="Daniel Rempto"; Tel="(24) 99902-0007"},
    @{Nome="Renan Costa"; Tel="(24) 99902-0008"},
    @{Nome="Sr(a) Isabelle Costa"; Tel="(24) 99902-0009"},
    @{Nome="Nicolle"; Tel="(24) 99902-0010"},
    @{Nome="Pedrinho Santos"; Tel="(24) 99901-0001"},
    @{Nome="Guaraci Costa"; Tel="(24) 99901-0002"},
    @{Nome="Anne"; Tel="(24) 99901-0003"},
    @{Nome="Ana Luiza"; Tel="(24) 99901-0004"},
    @{Nome="Leo Mussel"; Tel="(24) 99901-0005"},
    @{Nome="Gabi Mussel"; Tel="(24) 99901-0006"},
    @{Nome="Mae Mussel"; Tel="(24) 99901-0007"},
    @{Nome="Guilherme Almeida"; Tel="(24) 99901-0008"},
    @{Nome="Murillo Almeida"; Tel="(24) 99901-0009"},
    @{Nome="Roberta Almeida"; Tel="(24) 99901-0010"}
)
foreach ($p in $bingen) {
    if (Criar-Torcedor $p.Nome $p.Tel $rotaBingen.id) { $totalCriados++ }
}

# --- QUITANDINHA (24 pessoas) ---
Write-Host "`n--- QUITANDINHA (24 pessoas) ---" -ForegroundColor Cyan
$quitandinha = @(
    @{Nome="Dadock"; Tel="(24) 99903-0001"},
    @{Nome="Guilherme Franca"; Tel="(24) 99903-0002"},
    @{Nome="Fonseca"; Tel="(24) 99903-0003"},
    @{Nome="Vitinho Lacerda"; Tel="(24) 99903-0004"},
    @{Nome="Isaac Lacerda"; Tel="(24) 99903-0005"},
    @{Nome="Monique Palma"; Tel="(24) 99903-0006"},
    @{Nome="Aline Palma"; Tel="(24) 99903-0007"},
    @{Nome="Miguel Palma"; Tel="(24) 99903-0008"},
    @{Nome="Tia Valeria Gama"; Tel="(24) 99903-0009"},
    @{Nome="Marcia Gama"; Tel="(24) 99903-0010"},
    @{Nome="Luis Gustavo"; Tel="(24) 99903-0011"},
    @{Nome="Cassiano"; Tel="(24) 99903-0012"},
    @{Nome="Giovani Carvalho"; Tel="(24) 99903-0013"},
    @{Nome="Giovana Nicolay"; Tel="(24) 99903-0014"},
    @{Nome="Isabela Carvalho"; Tel="(24) 99901-0011"},
    @{Nome="Leo Silva"; Tel="(24) 99901-0012"},
    @{Nome="Anna Silva"; Tel="(24) 99901-0013"},
    @{Nome="Lysa Silva"; Tel="(24) 99901-0014"},
    @{Nome="Amanda Soares"; Tel="(24) 99901-0015"},
    @{Nome="Jones"; Tel="(24) 99904-0001"},
    @{Nome="Lucas Barbosa"; Tel="(24) 99904-0002"},
    @{Nome="Luizeto"; Tel="(24) 99904-0003"},
    @{Nome="Cesar Vallejo"; Tel="(24) 99904-0004"},
    @{Nome="Rian Praxedes"; Tel="(24) 99904-0005"}
)
foreach ($p in $quitandinha) {
    if (Criar-Torcedor $p.Nome $p.Tel $rotaQuitandinha.id) { $totalCriados++ }
}

Write-Host "`n==============================" -ForegroundColor Cyan
Write-Host " $totalCriados torcedores criados com sucesso!" -ForegroundColor Green
Write-Host "==============================" -ForegroundColor Cyan
