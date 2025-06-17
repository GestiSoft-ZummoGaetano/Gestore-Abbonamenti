using System;
using System.Text;

public static class CodiceFiscaleGenerator
{
    public static string CalcolaCodiceFiscale(string cognome, string nome, DateTime dataNascita, string sesso, string codiceCatastale)
    {
        if (dataNascita == null)
            return string.Empty;

        var cf = new StringBuilder();

        cf.Append(CodificaCognome(cognome));
        cf.Append(CodificaNome(nome));
        cf.Append(CodificaDataSesso(dataNascita, sesso));
        cf.Append(codiceCatastale.ToUpper());
        cf.Append(CalcolaCarattereControllo(cf.ToString()));

        return cf.ToString();
    }

    private static string CodificaCognome(string cognome) => CodificaNomeOCognome(cognome, false);

    private static string CodificaNome(string nome) => CodificaNomeOCognome(nome, true);

    private static string CodificaNomeOCognome(string input, bool isNome)
    {
        input = RimuoviCaratteriSpeciali(input.ToUpper());
        var consonanti = EstraiConsonanti(input);
        var vocali = EstraiVocali(input);

        if (isNome && consonanti.Length >= 4)
            // Prende la 1a, 3a e 4a consonante per il nome (regola Codice Fiscale)
            return $"{consonanti[0]}{consonanti[2]}{consonanti[3]}";

        // Prende le prime 3 lettere (consonanti + vocali) o aggiunge X se mancano
        return (consonanti + vocali + "XXX").Substring(0, 3);
    }

    private static string CodificaDataSesso(DateTime data, string sesso)
    {
        string[] mesi = { "A", "B", "C", "D", "E", "H", "L", "M", "P", "R", "S", "T" };
        string anno = data.Year.ToString("D4").Substring(2, 2);
        string mese = mesi[data.Month - 1];
        int giorno = data.Day;

        // Se sesso è Femminile aggiunge 40 al giorno
        if (sesso.ToUpper() == "F" || sesso.ToUpper() == "FEMMINA")
            giorno += 40;

        return anno + mese + giorno.ToString("D2");
    }

    private static string CalcolaCarattereControllo(string codice)
    {
        int somma = 0;
        for (int i = 0; i < codice.Length; i++)
        {
            char c = codice[i];
            // Posizione dispari (0-based): valore Dispari, pari: valore Pari
            somma += (i % 2 == 0) ? ValoreDispari(c) : ValorePari(c);
        }
        int resto = somma % 26;
        return ((char)('A' + resto)).ToString();
    }

    private static int ValorePari(char c)
    {
        // Valori per caratteri in posizione pari (indice 1,3,5,...)
        if (char.IsDigit(c))
            return c - '0';

        switch (c)
        {
            case 'A': return 0;
            case 'B': return 1;
            case 'C': return 2;
            case 'D': return 3;
            case 'E': return 4;
            case 'F': return 5;
            case 'G': return 6;
            case 'H': return 7;
            case 'I': return 8;
            case 'J': return 9;
            case 'K': return 10;
            case 'L': return 11;
            case 'M': return 12;
            case 'N': return 13;
            case 'O': return 14;
            case 'P': return 15;
            case 'Q': return 16;
            case 'R': return 17;
            case 'S': return 18;
            case 'T': return 19;
            case 'U': return 20;
            case 'V': return 21;
            case 'W': return 22;
            case 'X': return 23;
            case 'Y': return 24;
            case 'Z': return 25;
            default: return 0;
        }
    }

    private static int ValoreDispari(char c)
    {
        // Valori per caratteri in posizione dispari (indice 0,2,4,...)
        if (char.IsDigit(c))
        {
            switch (c)
            {
                case '0': return 1;
                case '1': return 0;
                case '2': return 5;
                case '3': return 7;
                case '4': return 9;
                case '5': return 13;
                case '6': return 15;
                case '7': return 17;
                case '8': return 19;
                case '9': return 21;
            }
        }

        switch (c)
        {
            case 'A': return 1;
            case 'B': return 0;
            case 'C': return 5;
            case 'D': return 7;
            case 'E': return 9;
            case 'F': return 13;
            case 'G': return 15;
            case 'H': return 17;
            case 'I': return 19;
            case 'J': return 21;
            case 'K': return 2;
            case 'L': return 4;
            case 'M': return 18;
            case 'N': return 20;
            case 'O': return 11;
            case 'P': return 3;
            case 'Q': return 6;
            case 'R': return 8;
            case 'S': return 12;
            case 'T': return 14;
            case 'U': return 16;
            case 'V': return 10;
            case 'W': return 22;
            case 'X': return 25;
            case 'Y': return 24;
            case 'Z': return 23;
            default: return 0;
        }
    }

    private static string EstraiConsonanti(string s)
    {
        var sb = new StringBuilder();
        foreach (char c in s)
            if ("BCDFGHJKLMNPQRSTVWXYZ".Contains(c))
                sb.Append(c);
        return sb.ToString();
    }

    private static string EstraiVocali(string s)
    {
        var sb = new StringBuilder();
        foreach (char c in s)
            if ("AEIOU".Contains(c))
                sb.Append(c);
        return sb.ToString();
    }

    private static string RimuoviCaratteriSpeciali(string s)
    {
        return s.Replace(" ", "").Replace("'", "").Replace("-", "").Replace("_", "").Replace(".", "");
    }
}
