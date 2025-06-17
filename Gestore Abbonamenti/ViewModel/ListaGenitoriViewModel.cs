using Gestore_Abbonamenti.View;
using GestoreAbbonamenti.Common.Constant;
using GestoreAbbonamenti.Logic.LogicSetter;
using GestoreAbbonamenti.Model;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gestore_Abbonamenti.ViewModel;
public class ListaGenitoriViewModel : BaseObservableObject
{
    public ListaGenitoriViewModel()
    {
        SetContext();
    }
    public void SetContext()
    {
        FilteredListUtenti = ListGenitori = new ObservableCollection<Genitori>(LogicFactory.Instance.Genitori.GetGenitori() ?? new());
    }
    void Update()
    {
        if (Genitore == null)
            return;

        AddOrUpdateArchivioView window = new AddOrUpdateArchivioView(DateTime.Now.Year, Genitore.Id);
        window.ShowDialog();
        SetContext();
    }
    void AddOrUpdate()
    {
        AddOrUpdateArchivioView window = new AddOrUpdateArchivioView();
        var result = window.ShowDialog();
        SetContext();
    }
    void SearchList()
    {
         if (ListGenitori == null || Search == null)
            return;

        if (string.IsNullOrWhiteSpace(Search))
            FilteredListUtenti = ListGenitori;

        string searchLower = Search.ToLower();

        FilteredListUtenti = new ObservableCollection<Genitori>(ListGenitori
            .Where(u => (u.Nome?.ToLower().Contains(searchLower) ?? false) ||
                        (u.Cognome?.ToLower().Contains(searchLower) ?? false) ||
                        (u.Indirizzo?.ToLower().Contains(searchLower) ?? false) ||
                        (u.CodiceFiscale?.ToLower().Contains(searchLower) ?? false) ||
                        (u.NCivico?.ToLower().Contains(searchLower) ?? false)));
    }

    #region Command
    public ICommand UpdateCedolaCommand => new DelegateCommand(Update);
    public ICommand AddOrUpdateCommand => new DelegateCommand(AddOrUpdate); 
    public ICommand TextChangedCommand => new DelegateCommand(SearchList);

    #endregion

    #region Property
    ObservableCollection<Genitori>? _listGenitori;
    public ObservableCollection<Genitori>? ListGenitori
    {
        get => _listGenitori;
        set
        {
            _listGenitori = value;
            OnPropertyChanged(nameof(ListGenitori));
            OnPropertyChanged(nameof(FilteredListUtenti));
        }   
    }

    ObservableCollection<Genitori> _filteredListUtenti = new();
    public ObservableCollection<Genitori> FilteredListUtenti
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
