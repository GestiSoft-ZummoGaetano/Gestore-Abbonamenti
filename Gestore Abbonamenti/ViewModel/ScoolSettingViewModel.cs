using GestiSoGestoreAbbonamentift.Common.Enum;
using Gestore_Abbonamenti.View;
using Gestore_Abbonamenti.View.ShowDialog;
using GestoreAbbonamenti.Common.Constant;
using GestoreAbbonamenti.Common.Enum;
using GestoreAbbonamenti.Logic.LogicSetter;
using GestoreAbbonamenti.Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Gestore_Abbonamenti.ViewModel;
public class ScoolSettingViewModel : BaseObservableObject
{
    Scuole? OldSchool = new Scuole();
    public ScoolSettingViewModel()
    {
        SetContext();
    }
    public void SetContext()
    {
        Scuola = new();
        ListScuole = new ObservableCollection<Scuole>(LogicFactory.Instance.Scool.GetScool() ?? new());
        FilterListScuole = ListScuole;
        Citta = new List<string?>(ListScuole.Select(c => c.Citta).Distinct().ToList());
        IndexCitta = 0;
        SelectedCitta = Citta[IndexCitta];

        SearchIstituti();
    }
    void AddOrUpdate()
    {
        try
        {
            if (!string.IsNullOrWhiteSpace(TextCitta))
                Scuola.Citta = TextCitta;
            else
                Scuola.Citta = SelectedCitta;

            if (!string.IsNullOrWhiteSpace(TextIstituto))
                Scuola.Istituto = TextIstituto;
            else
                Scuola.Citta = SelectedIstituto;

            if (Scuola?.Citta == null || Scuola?.Istituto == null || Scuola?.CodiceIstituto == null || Scuola?.DistanzaDalComune == null || Scuola?.PrezzoCedolaSettimanale == null)
            {
                ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, $"COMPILARE TUTTI I CAMPI", ShowDialogImage.ERROR);
                return;
            }
            bool changeAll = false;

            if(Scuola.DistanzaDalComune != OldSchool?.DistanzaDalComune || Scuola.PrezzoCedolaSettimanale != OldSchool.PrezzoCedolaSettimanale)
            {
                var result = ShowDialogView.ShowDialogPage(ShowDialogResult.CAUTION, $"VUOI APPLICARE QUESTA MODIFICA A TUTTI GLI ISTITUTI DI {Scuola.Citta}?", ShowDialogImage.CAUTION, ShowDialogButton.YESNO);

                if (result == ShowDialogResult.YES)
                    changeAll = true;
            }

            var res = LogicFactory.Instance.Scool.AddOrUpdateScool(Scuola, changeAll);
            
            if (res?.Result == ResultType.Success)
            {
                SetContext();
                return;
            }

            ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, $"ERRORE DURANTE IL SALVATAGGIO", ShowDialogImage.ERROR);
        }
        catch (Exception ex)
        {
            ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, ex.ToString(), ShowDialogImage.ERROR);
        }
    }
    void Update()
    {                
        SelectedCitta = RowScuola?.Citta;
        IndexCitta = Citta.IndexOf(RowScuola?.Citta);
        SelectedIstituto = RowScuola?.Istituto;
        IndexIstituto = Istituto.IndexOf(RowScuola?.Istituto);
        Scuola = new Scuole(RowScuola);
        OldSchool = new Scuole(RowScuola);
    }
    void Delete()
    {
        var result = ShowDialogView.ShowDialogPage(ShowDialogResult.CAUTION, "SEI SICURO DI VOLER ELIMINARE QUESTA RIGA?\n " +
            $"{RowScuola?.DisplayMember}", ShowDialogImage.CAUTION, ShowDialogButton.YESNO);

        if (result == ShowDialogResult.NO)
            return;

        var res = LogicFactory.Instance.Scool.DeleteRow(RowScuola);

        if (res.Result == ResultType.Success)
        {
            SetContext();
            return;
        }

        ShowDialogView.ShowDialogPage(ShowDialogResult.ERROR, res.Message, ShowDialogImage.ERROR);
    }
    void Search()
    {
        var viewModel = new IstitutiViewModel();
        var view = new IstitutiView
        {
            DataContext = viewModel
        };

        bool? result = view.ShowDialog();
        if (result == true)
        {
            var row = Scuola;
            row.CodiceIstituto = viewModel.CodiceSelezionato;
            row.Indirizzo = viewModel.Indirizzo;
            Scuola = new Scuole(row);
        }
    }
    void SearchIstituti()
    {
        if (ListScuole != null || ListScuole.Count > 0)
            Istituto = new List<string?>(ListScuole.Where(i => i.Citta == SelectedCitta).Select(c => c.Istituto).Distinct().ToList());

        IndexIstituto = 0;

        if(string.IsNullOrEmpty(TextCitta))
            SelectedIstituto = Istituto[IndexIstituto];
        Scuola = new Scuole
        {
            DistanzaDalComune = ListScuole.Where(c => c.Citta == SelectedCitta).Select(d => d.DistanzaDalComune).FirstOrDefault(),
            PrezzoCedolaSettimanale = ListScuole.Where(c => c.Citta == SelectedCitta).Select(d => d.PrezzoCedolaSettimanale).FirstOrDefault()
        };
    }
    void SearchList()
    {
        if (ListScuole == null || SearchTable == null)
            return;

        if (string.IsNullOrWhiteSpace(SearchTable))
            FilterListScuole = ListScuole;

        string searchLower = SearchTable.ToLower();

        FilterListScuole = new ObservableCollection<Scuole>(ListScuole
            .Where(u => (u.Citta?.ToLower().Contains(searchLower) ?? false) ||
                        (u.Istituto?.ToLower().Contains(searchLower) ?? false) ||
                        (u.Indirizzo?.ToLower().Contains(searchLower) ?? false) ||
                        (u.DistanzaDalComune?.ToString().Contains(searchLower) ?? false) ||
                        (u.PrezzoCedolaSettimanale?.ToString().Contains(searchLower) ?? false) ||
                        (u.CodiceIstituto?.ToString().Contains(searchLower) ?? false)));
    }

    #region Comand
    public ICommand AddCommand => new DelegateCommand(AddOrUpdate);
    public ICommand UpdateCommand => new DelegateCommand(Update);
    public ICommand SearchCommand => new DelegateCommand(Search);
    public ICommand DeleteCommand => new DelegateCommand(Delete);
    public ICommand IstitutiCommand => new DelegateCommand(SearchIstituti);
    public ICommand TextChangedCommand => new DelegateCommand(SearchList);

    #endregion;

    #region Property

    #region List
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
    #endregion

    #region Oblect
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

    Scuole? _rowScuola = new();
    public Scuole? RowScuola
    {
        get => _rowScuola;
        set
        {
            _rowScuola = value;
            OnPropertyChanged(nameof(RowScuola));
        }
    }
    List<string?> _citta;
    public List<string?> Citta
    {
        get => _citta;
        set
        {
            _citta = value;
            OnPropertyChanged(nameof(Citta));
        }
    }
    List<string?> _istituto;
    public List<string?> Istituto
    {
        get => _istituto;
        set
        {
            _istituto = value;
            OnPropertyChanged(nameof(Istituto));
        }
    }
    string _selectedCitta;
    public string SelectedCitta
    {
        get => _selectedCitta;
        set
        {
            _selectedCitta = value;
            OnPropertyChanged(nameof(SelectedCitta));
        }
    }

    string _selectedIstituto;
    public string SelectedIstituto
    {
        get => _selectedIstituto;
        set
        {
            _selectedIstituto = value;
            OnPropertyChanged(nameof(SelectedIstituto));
        }
    }

    string _textCitta;
    public string TextCitta
    {
        get => _textCitta;
        set
        {
            _textCitta = value;
            OnPropertyChanged(nameof(TextCitta));
        }
    }

    string _textIstituto;
    public string TextIstituto
    {
        get => _textIstituto;
        set
        {
            _textIstituto = value;
            OnPropertyChanged(nameof(TextIstituto));
        }
    }

    int _indexCitta;
    public int IndexCitta
    {
        get => _indexCitta;
        set
        {
            _indexCitta = value;
            OnPropertyChanged(nameof(IndexCitta));
        }
    }

    int _indexIstituto;
    public int IndexIstituto
    {
        get => _indexIstituto;
        set
        {
            _indexIstituto = value;
            OnPropertyChanged(nameof(IndexIstituto));
        }
    }

    string _searchTable;
    public string SearchTable
    {
        get => _searchTable;
        set
        {
            _searchTable = value;
            OnPropertyChanged(nameof(SearchTable));
        }
    }
    #endregion SelectedCitta

    #endregion
}
