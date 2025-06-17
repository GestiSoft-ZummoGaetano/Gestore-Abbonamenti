using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using Microsoft.Win32;


namespace GestisoftInstallerAbbonamenti
{
    public partial class Form1 : Form
    {
        string path = "C:\\Gestione Abbonamenti";
        string zipFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Gestione Abbonamenti.zip");

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ControlBox = false;
            textInfo.TabStop = false;
            textInfo.Multiline = true;
            textInfo.ScrollBars = ScrollBars.Vertical;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textInfo.Text += $"\r\nPercorso installazione: {path}";
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Visible = false;
            btnCacella.Visible = true;
            progressBar1.Value = 0;
            await InstallDependencies();
            ExtractApplication();

        }

        private async Task InstallDependencies()
        {
            await InstallAspNetCore();
            progressBar1.Value += 10;
            await InstallDotNet();
            progressBar1.Value += 10;
            await InstallWindowsDesktopRuntime();
        }

        private async Task InstallAspNetCore()
        {
            if (IsRuntimeInstalled("Microsoft ASP.NET Core 6.0.11 Shared Framework"))
            {
                textInfo.AppendText("\r\nASP.NET Core Runtime già installato. Operazione saltata.");            
                return;
            }

            string installerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Binaries", "aspnetcore-runtime-6.0.11-win-x64.exe");
            await RunInstaller(installerPath, "/quiet /norestart");
        }

        private async Task InstallDotNet()
        {
            if (IsRuntimeInstalled("Microsoft .NET Runtime - 6.0.11 (x64)"))
            {
                textInfo.AppendText("\r\n.NET Runtime già installato. Operazione saltata.");
                return;
            }

            string installerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Binaries", "dotnet-runtime-6.0.11-win-x64.exe");
            await RunInstaller(installerPath, "/quiet /norestart");
        }

        private async Task InstallWindowsDesktopRuntime()
        {
            if (IsRuntimeInstalled("Microsoft Windows Desktop Runtime - 6.0.15 (x64)"))
            {
                textInfo.AppendText("\r\nWindows Desktop Runtime già installato. Operazione saltata.");
                return;
            }

            string installerPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Binaries", "windowsdesktop-runtime-6.0.15-win-x64.exe");
            await RunInstaller(installerPath, "/quiet /norestart");
        }

        private void ExtractApplication()
        {
            if (!File.Exists(zipFilePath))
            {
                textInfo.AppendText($"\r\nERRORE: Il file {zipFilePath} non esiste!");
                return;
            }

            using (var getVredencial = new GetVredencial(path))
            {
                var result = getVredencial.ShowDialog();

                if (result != DialogResult.OK)
                {
                    textInfo.AppendText("\r\nInstallazione annullata dall’utente.");
                    btnCacella.Visible = false;
                    btnStart.Visible = false;
                    btnFine.Visible = true;
                    progressBar1.Value = 0;
                    return;
                }
            }

            try
            {
                textInfo.AppendText("\r\nEstrazione del pacchetto in corso...");

                using (FileStream zipToOpen = new FileStream(zipFilePath, FileMode.Open, FileAccess.Read))
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Read))
                {
                    int totalFiles = archive.Entries.Count;
                    int currentFile = 0;

                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string destinationPath = Path.Combine(path, entry.FullName);
                        string directoryPath = Path.GetDirectoryName(destinationPath);

                        if (!string.IsNullOrEmpty(directoryPath))
                            Directory.CreateDirectory(directoryPath);

                        if (!string.IsNullOrEmpty(entry.Name))
                            entry.ExtractToFile(destinationPath, overwrite: true);

                        currentFile++;

                        textInfo.AppendText($"\r\nEstrazione: {entry.FullName}");
                        textInfo.SelectionStart = textInfo.Text.Length;
                        textInfo.ScrollToCaret();

                        progressBar1.Value = 30 + (int)((double)currentFile / totalFiles * 40);

                        Application.DoEvents();
                    }
                }

                if (CreateShortcut())
                {
                    textInfo.AppendText("\r\nApplicazione installata in " + path);
                    btnCacella.Visible = false;
                    btnStart.Visible = false;
                    btnFine.Visible = true;
                    progressBar1.Value = 100;
                }
                else
                {
                    textInfo.AppendText("\r\nErrore nella creazione del collegamento.");
                }
            }
            catch (Exception ex)
            {
                textInfo.AppendText($"\r\nERRORE Generale durante l'estrazione: {ex.Message}");
            }
        }


        private async Task RunInstaller(string fileName, string arguments)
        {
            if (!File.Exists(fileName))
            {
                textInfo.AppendText($"\r\nERRORE: Il file {fileName} non esiste!");
                return;
            }

            try
            {
                Process process = new Process();
                process.StartInfo.FileName = fileName;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true;

                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        textInfo.AppendText($"\r\n{e.Data}");
                        textInfo.SelectionStart = textInfo.Text.Length;
                        textInfo.ScrollToCaret();
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        textInfo.AppendText($"\r\nERRORE: {e.Data}");
                        textInfo.SelectionStart = textInfo.Text.Length;
                        textInfo.ScrollToCaret();
                    }
                };

                textInfo.AppendText($"\r\nInstallazione in corso di {fileName}...");
                textInfo.SelectionStart = textInfo.Text.Length;
                textInfo.ScrollToCaret();

                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                await Task.Run(() => process.WaitForExit());

                if (process.ExitCode == 0)
                {
                    textInfo.AppendText($"\r\n{fileName} installato con successo!");
                }
                else
                {
                    textInfo.AppendText($"\r\nERRORE: {fileName} ha restituito un errore (ExitCode {process.ExitCode}).");
                }
                progressBar1.Value += 20;
            }
            catch (Exception ex)
            {
                textInfo.AppendText($"\r\nERRORE Generale: {ex.Message}");
            }
        }

        private bool CreateShortcut()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string shortcutPath = Path.Combine(desktopPath, "Gestione Abbonamenti.lnk");
                string targetPath = Path.Combine(path, "Gestione Abbonamenti", "Gestore Abbonamenti.exe");

                if (!File.Exists(targetPath))
                {
                    textInfo.AppendText($"\r\nERRORE: Il file {targetPath} non esiste, impossibile creare la scorciatoia.");
                    return false;
                }

                Type shellType = Type.GetTypeFromProgID("WScript.Shell");
                object shell = Activator.CreateInstance(shellType);
                object shortcut = shellType.InvokeMember("CreateShortcut", System.Reflection.BindingFlags.InvokeMethod, null, shell, new object[] { shortcutPath });

                Type shortcutType = shortcut.GetType();
                shortcutType.InvokeMember("TargetPath", System.Reflection.BindingFlags.SetProperty, null, shortcut, new object[] { targetPath });
                shortcutType.InvokeMember("WorkingDirectory", System.Reflection.BindingFlags.SetProperty, null, shortcut, new object[] { Path.GetDirectoryName(targetPath) });
                shortcutType.InvokeMember("Description", System.Reflection.BindingFlags.SetProperty, null, shortcut, new object[] { "Gestione Abbonamenti" });
                shortcutType.InvokeMember("Save", System.Reflection.BindingFlags.InvokeMethod, null, shortcut, null);

                textInfo.AppendText("\r\nScorciatoia creata con successo sul Desktop!");
                progressBar1.Value += 10;
                textInfo.AppendText("\r\nFine!");
                return true;
            }
            catch (Exception ex)
            {
                textInfo.AppendText($"\r\nERRORE durante la creazione della scorciatoia: " + ex.Message);
                return false;
            }
        }

        private void btnCacella_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                    textInfo.Text = "Installazione Annullata";
                    btnCacella.Visible = false;
                    btnStart.Visible = false;
                    btnFine.Visible = true;
                }

            }
            catch (Exception ex)
            {
                textInfo.Text = "Errore durante l'eliminazione: \r\n" + ex.Message;
            }
        }

        private void btnFine_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool IsRuntimeInstalled(string displayNameContains)
        {
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    foreach (string subkeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                        {
                            var displayName = subkey?.GetValue("DisplayName") as string;
                            if (!string.IsNullOrEmpty(displayName) && displayName.Contains(displayNameContains))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            // Su sistemi a 64 bit controlliamo anche WOW6432Node
            registryKey = @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(registryKey))
            {
                if (key != null)
                {
                    foreach (string subkeyName in key.GetSubKeyNames())
                    {
                        using (RegistryKey subkey = key.OpenSubKey(subkeyName))
                        {
                            var displayName = subkey?.GetValue("DisplayName") as string;
                            if (!string.IsNullOrEmpty(displayName) && displayName.Contains(displayNameContains))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

    }
}
