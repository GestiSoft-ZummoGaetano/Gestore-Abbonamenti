using Newtonsoft.Json.Linq;
using System.Management;
using System.Text;


namespace GestisoftInstallerAbbonamenti
{
    public partial class GetVredencial : Form
    {
        private static readonly HttpClient client = new();
        private static bool IsAuthenticated = false;
        private static string message = string.Empty;
        private static bool quit = false;
        private string currentPath = string.Empty;
        public GetVredencial(string path)
        {
            InitializeComponent();
            currentPath = path;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {

            await VerificaCredenziali();

            if (!IsAuthenticated)
            {
                MessageBox.Show("Errore di connessione\n" + message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string directoryPath = Path.Combine(currentPath, "Gestione Abbonamenti");

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = Path.Combine(directoryPath, "Comune.txt");
            File.WriteAllText(filePath, cmdComune.Text.ToUpper());
            quit = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private string GetCpuId()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT ProcessorId FROM Win32_Processor"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["ProcessorId"].ToString();
                    }
                }
            }
            catch
            {
                return "UNKNOWN_CPU";
            }
            return "UNKNOWN_CPU";
        }

        private string GetDiskId()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_DiskDrive"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["SerialNumber"].ToString();
                    }
                }
            }
            catch
            {
                return "UNKNOWN_DISK";
            }
            return "UNKNOWN_DISK";
        }

        private string GetMotherboardId()
        {
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT SerialNumber FROM Win32_BaseBoard"))
                {
                    foreach (ManagementObject obj in searcher.Get())
                    {
                        return obj["SerialNumber"].ToString();
                    }
                }
            }
            catch
            {
                return "UNKNOWN_MB";
            }
            return "UNKNOWN_MB";
        }

        private async Task VerificaCredenziali()
        {
            var url = "https://www.villachifeciscopello.com/_functions/verificaAdmin";
            var json = $"{{" +
                $"\"utente\": \"{cmbUser.Text}\", " +
                $"\"password\": \"{cmbPwd.Text}\", " +
                $"\"comune\": \"{cmdComune.Text.ToUpper()}\", " +
                $"\"idHardware\": \"{GetMotherboardId()}\", " +
                $"\"idHardDisk\": \"{GetDiskId()}\", " +
                $"\"idCpu\": \"{GetCpuId()}\"}}";

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(url, content);
                string responseString = await response.Content.ReadAsStringAsync();
                var jsonResponse = JObject.Parse(responseString);
                IsAuthenticated = jsonResponse["success"]?.ToObject<bool>() ?? false;
                message = jsonResponse["message"]?.ToString() ?? "";

            }
            catch (Exception ex)
            {
                MessageBox.Show("Errore di connessione: " + ex.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetVredencial_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
