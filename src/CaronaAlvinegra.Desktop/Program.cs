using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace CaronaAlvinegra.Desktop;

static class Program
{
    private static Process? _apiProcess;
    private static readonly HttpClient _httpClient = new() { Timeout = TimeSpan.FromSeconds(5) };

    private const string ApiUrl = "http://localhost:5000";
    private const int MaxRetries = 30;
    private const int RetryDelayMs = 1000;

    [STAThread]
    static void Main()
    {
        // Localiza o diretório da solução a partir do local do executável
        var apiProjectPath = FindApiProjectPath()
            ?? throw new InvalidOperationException(
                "Não foi possível localizar o projeto CaronaAlvinegra.Api. " +
                "Certifique-se de que a estrutura de diretórios está intacta.");

        // Inicia a API como processo filho
        StartApiProcess(apiProjectPath);

        // Aguarda a API ficar pronta
        if (!WaitForApi().GetAwaiter().GetResult())
        {
            KillApiProcess();
            System.Windows.Forms.MessageBox.Show(
                $"A API não iniciou após {MaxRetries * RetryDelayMs / 1000} segundos.\n" +
                $"Verifique se o .NET SDK 9.0 está instalado e tente novamente.",
                "Erro de inicialização",
                System.Windows.Forms.MessageBoxButtons.OK,
                System.Windows.Forms.MessageBoxIcon.Error);
            return;
        }

        // Inicia o formulário WinForms
        ApplicationConfiguration.Initialize();
        var form = new Form1(ApiUrl);
        form.FormClosed += (_, _) => KillApiProcess();
        System.Windows.Forms.Application.Run(form);
    }

    /// <summary>
    /// Sobe na árvore de diretórios a partir do diretório de saída
    /// até encontrar o arquivo .sln, então localiza o projeto Api.
    /// </summary>
    private static string? FindApiProjectPath()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir != null)
        {
            if (dir.GetFiles("CaronaAlvinegra.sln").Length > 0)
            {
                var apiDir = Path.Combine(dir.FullName, "src", "CaronaAlvinegra.Api");
                if (Directory.Exists(apiDir))
                    return apiDir;
            }
            dir = dir.Parent;
        }

        return null;
    }

    /// <summary>
    /// Inicia o projeto Api com 'dotnet run' em segundo plano.
    /// </summary>
    private static void StartApiProcess(string apiProjectPath)
    {
        _apiProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"run --project \"{apiProjectPath}\" --urls \"{ApiUrl}\"",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = apiProjectPath
            },
            EnableRaisingEvents = true
        };

        // Log da saída da API para depuração
        _apiProcess.OutputDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                Debug.WriteLine($"[API OUT] {e.Data}");
        };
        _apiProcess.ErrorDataReceived += (_, e) =>
        {
            if (!string.IsNullOrEmpty(e.Data))
                Debug.WriteLine($"[API ERR] {e.Data}");
        };

        _apiProcess.Start();
        _apiProcess.BeginOutputReadLine();
        _apiProcess.BeginErrorReadLine();
    }

    /// <summary>
    /// Faz polling do endpoint /health até a API responder ou estourar o tempo limite.
    /// </summary>
    private static async Task<bool> WaitForApi()
    {
        for (var i = 0; i < MaxRetries; i++)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiUrl}/health");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var body = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[API] Saudável — {body}");
                    return true;
                }
            }
            catch (HttpRequestException)
            {
                // API ainda não está pronta
            }
            catch (TaskCanceledException)
            {
                // Timeout na requisição
            }

            await Task.Delay(RetryDelayMs);
        }

        return false;
    }

    /// <summary>
    /// Finaliza o processo da API ao fechar o Desktop.
    /// </summary>
    private static void KillApiProcess()
    {
        if (_apiProcess is { HasExited: false })
        {
            try
            {
                _apiProcess.Kill(entireProcessTree: true);
                _apiProcess.WaitForExit(5000);
            }
            catch (InvalidOperationException)
            {
                // Processo já foi finalizado
            }
            finally
            {
                _apiProcess.Dispose();
                _apiProcess = null;
            }
        }
    }
}
