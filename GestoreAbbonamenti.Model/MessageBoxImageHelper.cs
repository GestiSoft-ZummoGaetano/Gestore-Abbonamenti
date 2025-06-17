using GestoreAbbonamenti.Common.Enum;

public static class MessageBoxImageHelper
{
    public static string GetImagePath(ShowDialogImage imageType)
    {
        return imageType switch
        {
            ShowDialogImage.ERROR => "/Resource/Icons/Error.png",
            ShowDialogImage.INFO => "/Resource/Icons/Info.png",
            ShowDialogImage.CAUTION => "/Resource/Icons/Caution.png",
            _ => null // Nessuna immagine
        };
    }

    public static string GetButton(ShowDialogButton buttonType)
    {
        return buttonType switch
        {
            ShowDialogButton.OK => "OK",
            ShowDialogButton.OKCANCEL => "OKCANCEL",
            ShowDialogButton.YESNO => "YESNO",
            _ => null // Nessuna immagine
        };
    }
}