using GestiSoGestoreAbbonamentift.Common.Enum;
using Gestore_Abbonamenti.View.ShowDialog;
using GestoreAbbonamenti.Common.Constant;
using GestoreAbbonamenti.Common.Enum;
using GestoreAbbonamenti.Logic.LogicSetter;
using GestoreAbbonamenti.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Gestore_Abbonamenti.ViewModel;
public class AddOrUpdateArchivioViewModel : BaseObservableObject
{
    long idGenitore;
    public AddOrUpdateArchivioViewModel(int? anno, long id)           // Calcola la differenza in percentuale
    {                                                      //double differenzaPercentuale = ((valoreFinale - valoreIniziale) / valoreIniziale) * 100;
        SetContext(id, anno);
    }
    public void SetContext(long id, int? anno)
    {
        int selectedAnno = anno ?? DateTime.Now.Year;
        GetMesi();
        ListScuole = new ObservableCollection<Scuole>(LogicFactory.Instance.Scool.GetScool() ?? new());
        FilterListScuole = new ObservableCollection<Scuole>(ListScuole);
        Genitore = new Genitori(LogicFactory.Instance.Genitori.GetGenitoreById(id, selectedAnno));
        ListFigli = new ObservableCollection<Figlii>(Genitore?.Figli ?? new ObservableCollection<Figlii>());
        ListComboFigli = new ObservableCollection<Figlii>(ListFigli.Where(f => f.Frequenza == true));
        SelectedTab = 0;

        if (id <= 0)
        {
            EnableAlunno = EnableCedola = false;
            SalvaVisibility = true;
        }

        else
        {
            idGenitore = id;
            EnableAlunno = EnableCedola = true;
            SalvaVisibility = false;
            if (ListFigli.Count > 0)
                ComboFiglio = ListFigli[0];
            SelectionChanged();
        }

    }
    void GetMesi()
    {
        ListMesi = new ObservableCollection<string>
        {
            "GENNAIO","FEBBRAIO","MARZO","APRILE","MAGGIO","GIUGNO","LUGLIO","AGOSTO","SETTEMBRE","OTTOBRE","NOVEMBRE","DICEMBRE"
        };
    }
    public void CalcoloPagamento()
    {
        if (ComboFiglio?.Scuola == null)
            return;

        PercentualePagamento = LogicFactory.Instance.Genitori.GetPercentageCalculation(ComboFiglio.Scuola);

        if (ListCedole?.Count > 0)
        {
            Totale = ListCedole?.Sum(x => x.Importo);
            TotaleRidotto = ListCedole?.Sum(r => r.ImportoRidotto);
            return;
        }

        Totale = TotaleRidotto = 0;

    }
    void AddCedola()
    {
        if (Cedola == null || !Cedola.IsValid() || ComboFiglio == null)
        {
            ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "COMPILARE TUTTI I CAMPI", ShowDialogImage.ERROR);
            return;
        }

        long idFiglio = ListFigli?.FirstOrDefault(z => z.Nome == ComboFiglio?.Nome)?.Id ?? 0;

        if (idFiglio == 0)
            return;

        var nuovaCedola = new CedoleMensili
        {
            Id = 0,
            FiglioId = idFiglio,
            NCedola = Cedola?.NCedola,
            Mese = Cedola?.Mese,
            DataCedola = Cedola?.DataCedola,
            Importo = Cedola?.Importo,
            PercentualeRiduzione = PercentualePagamento,
            ImportoRidotto = Cedola?.Importo - (Cedola?.Importo * (PercentualePagamento / 100))
        };

        ListCedole?.Add(nuovaCedola);
        CalcoloPagamento();
        Cedola = new();
    }
    void AddFiglio()
    {
        if (Figlio == null || !Figlio.IsValid() || Figlio.Scuola == null)
        {
            ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "COMPILARE TUTTI I CAMPI", ShowDialogImage.ERROR);
            return;
        }

        var nuovoFiglio = new Figlii
        {
            Id = 0,
            GenitoreId = Genitore.Id,
            Nome = Figlio?.Nome,
            Cognome = Figlio?.Cognome,
            DataNascita = Figlio?.DataNascita,
            Sesso = Figlio?.Sesso,
            LuogoNascita = Figlio?.LuogoNascita,
            Indirizzo = Figlio?.Indirizzo,
            CedoleMensili = new List<CedoleMensili>(),
            Scuola = Figlio?.Scuola ?? new()
        };
        ListFigli?.Add(nuovoFiglio);
        Figlio = new();
    }
    void Save(bool? saveAll)
    {
        if (ListFigli?.Count > 0)
        {
            foreach (var figlio in ListFigli ?? new ObservableCollection<Figlii>())
            {
                if (figlio.Scuola == null)
                {
                    ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "COMPILARE TUTTI I CAMPI", ShowDialogImage.ERROR);
                    return;
                }

                figlio.IdScuola = figlio.Scuola.Id;
                // Associa tutte le cedole appartenenti a questo figlio
                figlio.CedoleMensili = ListCedole?
                    .Where(c => c.FiglioId == figlio.Id || c.FiglioId == 0)
                    .ToList() ?? new List<CedoleMensili>();
            }
        }
        Genitore.Figli = new List<Figlii>(ListFigli ?? new());
        var res = LogicFactory.Instance.Genitori.AddOrUpdate(Genitore);


        if (res.Result == ResultType.Success)
        {
            UpdateGenitore((Genitori)res.Data);
            ListComboFigli = new ObservableCollection<Figlii>(ListFigli.Where(f => f.Frequenza == true));
            Cedola = new(); Figlio = new();

            if (saveAll == null)
                ShowDialogView.ShowDialogPage(ShowDialogResult.INFO, res.Message, ShowDialogImage.INFO);

            CalcoloPagamento();

            if (idGenitore <= 0)
            {              
                SelectedTab += 1;
                EnableAlunno = true;
            }

            AddVisibility = true;

            if (ListScuole?.Count == 0)
                ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "NON CI SONO SCUOLE PRESENTI. PER INSERIRLE ANDARE IN IMPOSTAZIONI - SCUOLE", ShowDialogImage.ERROR);

            return;
        }

        ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, res.Message, ShowDialogImage.ERROR);
    }
    void UpdateFiglio()
    {
        if (RowFiglio == null)
            return;

        AddVisibility = false;
        Figlio = RowFiglio;
        indexScuola = ListScuole.ToList().FindIndex(s => s.Id == Figlio.Scuola?.Id);
    }
    void UpdateCedola()
    {
        AddVisibility = false;
        Cedola = RowCedola;
    }
    void UpdateGenitore(Genitori updatedGenitore)
    {
        Genitore = updatedGenitore;

        // Aggiorna le liste visibili
        ListFigli = new ObservableCollection<Figlii>(Genitore.Figli);
        SelectionChanged();
    }
    void SelectionChanged()
    {
        if (Cedola != null)
            Cedola = new();

        var cedole = Genitore?.Figli?
            .Where(f => f.Nome == ComboFiglio?.Nome)
            .SelectMany(f => f.CedoleMensili)
            .OrderBy(f => f.DataCedola)
            ?? Enumerable.Empty<CedoleMensili>();

        ListCedole = new ObservableCollection<CedoleMensili>(cedole);

        CalcoloPagamento();
    }
    void Avanti()
    {
        switch (SelectedTab)
        {
            case 0:

                if (Genitore == null || !Genitore.IsValid())
                {
                    ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "COMPILARE TUTTI I CAMPI", ShowDialogImage.ERROR);
                    return;
                }

                Save(false);

                break;
            case 1:
                Save(false);
                SelectedTab += 1;
                EnableCedola = true;
                SalvaVisibility = false;
                break;
        }
    }
    void Nuovo()
    {
        var result = ShowDialogView.ShowDialogPage(ShowDialogResult.CAUTION, "STAI PER APRIRE UN NUOVO MODULO DI INSERIMENTO " +
           "SEI SICURO DI AVER SALVATO TUTTO CORRETTAMENTE?", ShowDialogImage.CAUTION, ShowDialogButton.YESNO);

        if (result == ShowDialogResult.NO)
            return;
        idGenitore = 0;
        SetContext(0, DateTime.Now.Date.Year);
        // SetContext(idGenitore);
    }
    void Cancel(Window window)
    {
        window.Close();
    }
    void FilterSchool()
    {
        var a = SearchText;
    }
    void SearchIstituti()
    {
        if (ListScuole == null)
            return;

        FilterListScuole = new ObservableCollection<Scuole>(
            ListScuole
            .Where(s => 
                (string.IsNullOrEmpty(SearchTextIstituto) ||
                (s.Citta != null && s.Citta.ToLower().Contains(SearchTextIstituto.ToLower())) ||
                (s.Istituto != null && s.Istituto.ToLower().Contains(SearchTextIstituto.ToLower())) ||
                (s.Indirizzo != null && s.Indirizzo.ToLower().Contains(SearchTextIstituto.ToLower())) ||
                (s.CodiceIstituto != null && s.CodiceIstituto.ToLower().Contains(SearchTextIstituto.ToLower())) ||
                (s.DistanzaDalComune.HasValue && s.DistanzaDalComune.Value.ToString().Contains(SearchTextIstituto.ToLower())) ||
                (s.PrezzoCedolaSettimanale.HasValue && s.PrezzoCedolaSettimanale.Value.ToString().Contains(SearchTextIstituto.ToLower()))
            ))
            .OrderBy(s => s.Istituto));
    }
    void CalcolaCF()
    {
        var genitore = Genitore;
        if (genitore == null || genitore.Nome == string.Empty || genitore.Cognome == string.Empty || genitore.DataNascita == null || genitore.Sesso == string.Empty)
        {
            ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "COMPILARE TUTTI I CAMPI", ShowDialogImage.ERROR);
            return;
        }
   
        var codFisc = LogicFactory.Instance.Genitori.CalcolaCodiceFiscale(genitore.Cognome, genitore.Nome, (DateTime)genitore.DataNascita, genitore.Sesso, genitore.LuogoNascita);

        if (codFisc == string.Empty)
        {
            ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, "COMUNE NON TROVATO", ShowDialogImage.ERROR);
            return;
        }

        genitore.CodiceFiscale = codFisc;
        Genitore = new Genitori(genitore);
    }

    #region ICommand
    public ICommand AddOrUpdateCommand => new DelegateCommand<bool?>(Save);
    public ICommand UpdateFiglioCommand => new DelegateCommand(UpdateFiglio);
    public ICommand UpdateCedolaCommand => new DelegateCommand(UpdateCedola);
    public ICommand AddCedolaCommand => new DelegateCommand(AddCedola);
    public ICommand AddFiglioCommand => new DelegateCommand(AddFiglio);
    public ICommand SelectionChangedCommand => new DelegateCommand(SelectionChanged);
    public ICommand AvantiCommand => new DelegateCommand(Avanti);
    public ICommand NuovoCommand => new DelegateCommand(Nuovo);
    public ICommand CancelCommand => new DelegateCommand<Window>(Cancel);
    public ICommand TextChangedCommand => new DelegateCommand(FilterSchool);
    public ICommand SearchIstitutoCommand => new DelegateCommand(SearchIstituti);
    public ICommand CFCommand => new DelegateCommand(CalcolaCF);
    #endregion

    #region Property

    #region Liste di Oggetti
    ObservableCollection<Figlii>? _ListFigli;
    public ObservableCollection<Figlii>? ListFigli
    {
        get => _ListFigli;
        set
        {
            _ListFigli = value;
            OnPropertyChanged(nameof(ListFigli));
        }
    }

    ObservableCollection<CedoleMensili>? _listCedole;
    public ObservableCollection<CedoleMensili>? ListCedole
    {
        get => _listCedole;
        set
        {
            _listCedole = value;
            OnPropertyChanged(nameof(ListCedole));
        }
    }
    ObservableCollection<string>? _listMesi;
    public ObservableCollection<string>? ListMesi
    {
        get => _listMesi;
        set
        {
            _listMesi = value;
            OnPropertyChanged(nameof(ListMesi));
        }
    }
    ObservableCollection<Figlii>? _listComboFigli;
    public ObservableCollection<Figlii>? ListComboFigli
    {
        get => _listComboFigli;
        set
        {
            _listComboFigli = value;
            OnPropertyChanged(nameof(ListComboFigli));
        }
    }
    ObservableCollection<Figlii>? _filterListComboFigli;
    public ObservableCollection<Figlii>? _FilterListComboFigli
    {
        get => _filterListComboFigli;
        set
        {
            _filterListComboFigli = value;
            OnPropertyChanged(nameof(_FilterListComboFigli));
        }
    }

    ObservableCollection<Scuole>? _listScuole;
    public ObservableCollection<Scuole>? ListScuole
    {
        get => _listScuole;
        set
        {
            _listScuole = value;
            OnPropertyChanged(nameof(ListScuole));
        }
    }

    ObservableCollection<Scuole>? _filterListScuole;
    public ObservableCollection<Scuole>? FilterListScuole
    {
        get => _filterListScuole;
        set
        {
            _filterListScuole = value;
            OnPropertyChanged(nameof(FilterListScuole));
        }
    }
    #endregion

    #region Oggetti
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
    Figlii? _figlio = new();
    public Figlii? Figlio
    {
        get => _figlio;
        set
        {
            _figlio = value;
            OnPropertyChanged(nameof(Figlio));
        }
    }
    CedoleMensili? _cedola = new();
    public CedoleMensili? Cedola
    {
        get => _cedola;
        set
        {
            _cedola = value;
            OnPropertyChanged(nameof(Cedola));
        }
    }
    Figlii _rowFiglio = new();
    public Figlii RowFiglio
    {
        get => _rowFiglio;
        set
        {
            _rowFiglio = value;
            OnPropertyChanged(nameof(RowFiglio));
        }
    }
    CedoleMensili _rowCedola = new();
    public CedoleMensili RowCedola
    {
        get => _rowCedola;
        set
        {
            _rowCedola = value;
            OnPropertyChanged(nameof(RowCedola));
        }
    }
    Scuole? _scuola = new();
    public Scuole? Scuola
    {
        get => _scuola;
        set
        {
            _scuola = value;
            OnPropertyChanged(nameof(Scuola));
        }
    }
    #endregion

    #region Variabili  

    string? _searchText;
    public string? SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged(nameof(SearchText));
        }
    }

    string? _searchTextIstituto;
    public string? SearchTextIstituto
    {
        get => _searchTextIstituto;
        set
        {
            _searchTextIstituto = value;
            OnPropertyChanged(nameof(SearchTextIstituto));
        }
    }

    decimal? _totale = 0;
    public decimal? Totale
    {
        get => _totale;
        set
        {
            _totale = value;
            OnPropertyChanged(nameof(Totale));
        }
    }
    decimal? _percentualePagamento = 0;
    public decimal? PercentualePagamento
    {
        get => _percentualePagamento;
        set
        {
            _percentualePagamento = value;
            OnPropertyChanged(nameof(PercentualePagamento));
        }
    }
    decimal? _totaleRidotto = 0;
    public decimal? TotaleRidotto
    {
        get => _totaleRidotto;
        set
        {
            _totaleRidotto = value;
            OnPropertyChanged(nameof(TotaleRidotto));
        }
    }
    decimal? _importo = 0;
    public decimal? Importo
    {
        get => _importo;
        set
        {
            _importo = value;
            OnPropertyChanged(nameof(Importo));
        }
    }
    int _selectedTab = 0;
    public int SelectedTab
    {
        get => _selectedTab;
        set
        {
            _selectedTab = value;
            OnPropertyChanged(nameof(SelectedTab));
        }
    }
    int _indexScuola = -1;
    public int indexScuola
    {
        get => _indexScuola;
        set
        {
            _indexScuola = value;
            OnPropertyChanged(nameof(indexScuola));
        }
    }
    int _selectedFiglio;
    public int SelectedFiglio
    {
        get => _selectedFiglio;
        set
        {
            _selectedFiglio = value;
            OnPropertyChanged(nameof(SelectedFiglio));
        }
    }
    Figlii? _comboFiglio;
    public Figlii? ComboFiglio
    {
        get => _comboFiglio;
        set
        {
            _comboFiglio = value;
            OnPropertyChanged(nameof(ComboFiglio));
        }
    }
    bool _salvaVisibility;
    public bool SalvaVisibility
    {
        get => _salvaVisibility;
        set
        {
            _salvaVisibility = value; ;
            OnPropertyChanged(nameof(SalvaVisibility));
        }
    }
    bool _enableAlunno;
    public bool EnableAlunno
    {
        get => _enableAlunno;
        set
        {
            _enableAlunno = value; ;
            OnPropertyChanged(nameof(EnableAlunno));
        }
    }
    bool _enableCedola;
    public bool EnableCedola
    {
        get => _enableCedola;
        set
        {
            _enableCedola = value; ;
            OnPropertyChanged(nameof(EnableCedola));
        }
    }
    bool _addVisibility = true;
    public bool AddVisibility
    {
        get => _addVisibility;
        set
        {
            _addVisibility = value; ;
            OnPropertyChanged(nameof(AddVisibility));
        }
    }
    #endregion

    #endregion
}
