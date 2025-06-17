using Gestore_Abbonamenti.View;
using GestoreAbbonamenti.Common.Constant;
using GestoreAbbonamenti.Logic.LogicSetter;
using GestoreAbbonamenti.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gestore_Abbonamenti.ViewModel;
public class IstitutiViewModel : BaseObservableObject
{
    public event EventHandler<bool?>? CloseRequested;
    public string? CodiceSelezionato { get; set; }
    public string? Indirizzo { get; set; }
    public IstitutiViewModel()
    {
        SetContext();
    }
    public void SetContext()
    {
        FilteredListUtenti = ListaIstituti = new ObservableCollection<Istituti>(LogicFactory.Instance.Scool.GetIstituti() ?? new());
    }
    void Select()
    {
        if (RowIstituti == null)
            return;

        CodiceSelezionato = RowIstituti.Codiceindirizzo;
        Indirizzo = RowIstituti.Zero;

        // Chiudi la finestra dialogo con risultato OK
        CloseRequested?.Invoke(this, true); // evento custom
    }
    void SearchList()
    {
         if (ListaIstituti == null || Search == null)
            return;

        if (string.IsNullOrWhiteSpace(Search))
            FilteredListUtenti = ListaIstituti;

        string searchLower = Search.ToLower();

        FilteredListUtenti = new ObservableCollection<Istituti>(ListaIstituti
            .Where(u => (u.Codiceindirizzo?.ToLower().Contains(searchLower) ?? false) ||
                        (u.Percorso?.ToLower().Contains(searchLower) ?? false) ||
                        (u.Zero?.ToLower().Contains(searchLower) ?? false)));
    }

    #region Command
    public ICommand SelectCommand => new DelegateCommand(Select);
    public ICommand TextChangedCommand => new DelegateCommand(SearchList);

    #endregion

    #region Property 
    ObservableCollection<Istituti>? _listaIstituti;
    public ObservableCollection<Istituti>? ListaIstituti
    {
        get => _listaIstituti;
        set
        {
            _listaIstituti = value;
            OnPropertyChanged(nameof(ListaIstituti));
            OnPropertyChanged(nameof(FilteredListUtenti));
        }   
    }

    Istituti? _rowIstituti = new();
    public Istituti? RowIstituti
    {
        get => _rowIstituti;
        set
        {
            _rowIstituti = value;
            OnPropertyChanged(nameof(RowIstituti));
        }
    }

    ObservableCollection<Istituti> _filteredListUtenti = new();
    public ObservableCollection<Istituti> FilteredListUtenti
    {
        get => _filteredListUtenti;
        set
        {
            _filteredListUtenti = value;
            OnPropertyChanged(nameof(FilteredListUtenti));
        }
    }

    ObservableCollection<int>? _years;
    public ObservableCollection<int>? Years
    {
        get => _years;
        set
        {
            _years = value;
            OnPropertyChanged(nameof(Years));
        }
    }

    Genitori? _genitore;
    public Genitori? Genitore
    {
        get => _genitore;
        set
        {
            _genitore = value;
            OnPropertyChanged(nameof(Genitore));
        }
    }
    string? _search;
    public string? Search
    {
        get => _search;
        set
        {
            _search = value;
            OnPropertyChanged(nameof(Search));
        }
    }
    int? _filterForYears;
    public int? FilterForYears
    {
        get => _filterForYears;
        set
        {
            _filterForYears = value;
            OnPropertyChanged(nameof(FilterForYears));
        }
    }
    int? _selectedIndexYear;
    public int? SelectedIndexYear
    {
        get => _selectedIndexYear;
        set
        {
            _selectedIndexYear = value;
            OnPropertyChanged(nameof(SelectedIndexYear));
        }
    }

    #endregion
}
