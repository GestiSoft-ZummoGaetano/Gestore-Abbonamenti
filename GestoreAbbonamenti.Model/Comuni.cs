using System.ComponentModel.DataAnnotations;

namespace GestoreAbbonamenti.Model;
public class Comuni
{
    [Key]
    public int Id { get; set; }
    public string? Comune {  get; set; }
}
