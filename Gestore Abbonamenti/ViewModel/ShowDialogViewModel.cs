using GestiSoGestoreAbbonamentift.Common.Enum;
using GestoreAbbonamenti.Common.Constant;
using GestoreAbbonamenti.Common.Enum;
using System.Windows;

namespace GestoreAbbonamenti.ViewModel
{
    public class ShowDialogViewModel : BaseObservableObject
    {
        #region Property
        string? _title;
        public string? Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }
        string? _textMessage;
        public string? TextMessage
        {
            get => _textMessage;
            set
            {
                _textMessage = value;
                OnPropertyChanged(nameof(TextMessage));
            }
        }
        string? _imageSource;
        public string? ImageSource
        {
            get => _imageSource;
            set
            {
                _imageSource = value;
                OnPropertyChanged(nameof(ImageSource));
            }
        }
        bool _okVisibility = false;
        public bool OkVisibility
        {
            get => _okVisibility;
            set
            {
                _okVisibility = value;
                OnPropertyChanged(nameof(OkVisibility));
            }
        }
        bool _cancelVisibility = false;
        public bool CancelVisibility
        {
            get => _cancelVisibility;
            set
            {
                _cancelVisibility = value;
                OnPropertyChanged(nameof(CancelVisibility));
            }
        }
        bool _yesVisibility = false;
        public bool YesVisibility
        {
            get => _yesVisibility;
            set
            {
                _yesVisibility = value;
                OnPropertyChanged(nameof(YesVisibility));
            }
        }
        bool _noVisibility = false;
        public bool NoVisibility
        {
            get => _noVisibility;
            set
            {
                _noVisibility = value;
                OnPropertyChanged(nameof(NoVisibility));
            }
        }
        #endregion
        public ShowDialogViewModel()
        {
        }

        public void SetContext(ShowDialogResult title, string textMessage, ShowDialogImage image, ShowDialogButton button = ShowDialogButton.OK)
        {
            Title = title.ToString();
            TextMessage = textMessage;
            ImageSource = MessageBoxImageHelper.GetImagePath(image);
            //ImageSource = "/Resource/Icons/Error.png";

            switch (button)
            {
                case ShowDialogButton.OK:
                    OkVisibility = true;                  
                    break;
                case ShowDialogButton.OKCANCEL:
                    OkVisibility = CancelVisibility = true;
                    break;
                case ShowDialogButton.YESNO:
                    YesVisibility = NoVisibility = true;
                    break;
            }           
        }
    }
}
