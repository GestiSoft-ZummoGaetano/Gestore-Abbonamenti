using GestoreAbbonamenti.Data.Database;
using GestoreAbbonamenti.Logic.Interfaces;
using GestoreAbbonamenti.Model;
using GestoreAbbonamenti.Model.cache;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Management;
using System.Windows.Forms;

namespace GestiSoft.Logic.Logic
{
    public class StartUpLogic : IStartUpLogic
    {
        public void Initialize()
        {
            using var context = new DbEntities();
            context.Database.Migrate();
            context.SaveChanges();
        }

        public void OnStarting()
        {
            GestiCache.IdHardware = GetMotherboardId();
            GestiCache.IdHardDisk = GetDiskId();
            GestiCache.IdCpu = GetCpuId();
            Initialize();
            
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

        public string? GetUser()
        {
            try
            {
                using var context = new DbEntities();

                // Controlla se esiste già un comune nel DB
                var comune = context.Comuni
                    .AsNoTracking()
                    .AsSplitQuery()
                    .FirstOrDefault()?.Comune;

                // Se non esiste, prova a leggere da Comune.txt e salvarlo
                if (comune == null)
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Comune.txt");
                    if (File.Exists(filePath))
                    {
                        string contenuto = File.ReadAllText(filePath).Trim();

                        if (!string.IsNullOrWhiteSpace(contenuto))
                        {
                            // Inserisce nel DB
                            context.Comuni.Add(new Comuni { Comune = contenuto });
                            context.SaveChanges();

                            comune = contenuto;
                        }
                    }
                    else
                        System.Windows.MessageBox.Show("File Comune.txt non trovato nella cartella dell'applicazione.");
                }

                return comune;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Errore:\n" + ex.Message);
                return null;
            }
        }

    }
}

