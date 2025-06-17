using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace GestoreAbbonamenti.Model;
public class Istituti
{
    public int Id { get; set; }
    public string? Percorso { get; set; } = string.Empty;
    public string? Settore { get; set; } = string.Empty;
    public string? Codiceindirizzo { get; set; } = string.Empty;
    public string? Zero { get; set; } = string.Empty;
    public string? Tipo { get; set; } = string.Empty;
    public string? Indirizzo { get; set; } = string.Empty;
    public string? Descrizione_Diploma { get; set; } = string.Empty;
    public string? N_lingue_Straniere { get; set; } = string.Empty;
    public string? FLG_MNS { get; set; } = string.Empty;
    public string? NUM_ORD_IND_ESA { get; set; } = string.Empty;

}
