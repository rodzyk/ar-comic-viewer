namespace AR_Comic_Viewer.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message);
        string FilePath { get; set; }
        bool OpenFileDialog(string filter);
        bool SaveFileDialog();
    }
}
