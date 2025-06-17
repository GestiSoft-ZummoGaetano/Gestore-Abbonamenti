public static class SelectionManager
{
    private static string _selectedTag;

    public static string SelectedTag
    {
        get => _selectedTag;
        set => _selectedTag = value;
    }
}