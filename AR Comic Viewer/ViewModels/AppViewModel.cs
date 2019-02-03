using AR_Comic_Viewer.Models;
using AR_Comic_Viewer.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace AR_Comic_Viewer.ViewModels
{
    public class AppViewModel : BaseViewModel
    {
        private readonly decimal scaleStep = 0.05M;
        private IDialogService _defaultDialog;
        private IImageArchiveService _archive;
        private IEnvironmentService _environment;
        private ObservableCollection<ImagePreview> _imagePreviewList;

        private string _title = "AR Comic Viewer";



        private Visibility _startPanelVisibility = Visibility.Visible; //Visibility="Hidden"

        public Visibility StartPanelVisibility
        {
            get => _startPanelVisibility;
            set => SetField(ref _startPanelVisibility, value, "StartPanelVisibility");
        }
      
        public AppViewModel(IDialogService defaultDialog, IImageArchiveService archive, IEnvironmentService environment)
        {
            _imagePreviewList = new ObservableCollection<ImagePreview>();
            _defaultDialog = defaultDialog;
            _archive = archive;
            _environment = environment;

            var args = _environment.GetCommandLineArguments().ToList();
            if (args.Count > 1 && args[1] != null)
            {
                try
                {
                    OpenFileByPath(args[1]);
                }
                catch (System.Exception e)
                {
                    _defaultDialog.ShowMessage(e.Message);
                }
            }

        }

        private bool _reset;
        public bool Reset
        {
            get => _reset;
            set => SetField(ref _reset, value, "Reset");

        }

        private decimal _scale = 1;
        public decimal Scale
        {
            get => _scale;
            set
            {
                if (value <= 0.05M) value = 0.05M;
                SetField(ref _scale, value, "Scale");
            }
        }

        public ObservableCollection<ImagePreview> ImagePreviewList
        {
            get => _imagePreviewList;
            set => SetField(ref _imagePreviewList, value, "ImagePreviewList");
        }
        public string Title
        {
            get => _title;
            set => SetField(ref _title, value, "Title");
        }

        private RelayCommand _closeFile;
        public RelayCommand CloseFile
        {
            get => _closeFile ?? (_closeFile = new RelayCommand(CloseFileExecute));
        }


        private RelayCommand _scalePlus;
        public RelayCommand ScalePlus
        {
            get => _scalePlus ?? (_scalePlus = new RelayCommand(ScalePlusExecute));
        }

        private void ScalePlusExecute(object o)
        {
            Scale += scaleStep;
        }

        private RelayCommand _scaleMinus;
        public RelayCommand ScaleMinus
        {
            get => _scaleMinus ?? (_scaleMinus = new RelayCommand(ScaleMinusExecute));
        }

        private void ScaleMinusExecute(object o)
        {
            Scale -= scaleStep;
        }

        private RelayCommand _scaleFull;
        public RelayCommand ScaleFull
        {
            get => _scaleFull ?? (_scaleFull = new RelayCommand(ScaleFullExecute));
        }

        private void ScaleFullExecute(object o)
        {
            Scale = 1;
        }

        private void CloseFileExecute(object o)
        {
            ImagePreviewList.Clear();
            Title = "AR Comic Viewer";
            StartPanelVisibility = Visibility.Visible;
            Reset = true;
            _archive.Dispose();
        }


        private RelayCommand _openFile;
        public RelayCommand OpenFile
        {
            get => _openFile ?? (_openFile = new RelayCommand(OpenFileExecute));
        }

        private void OpenFileExecute(object o)
        {
            var filter = "Comic book zip (*.cbz)|*.cbz|Zip archive (*.zip)|*.zip";
            if (!_defaultDialog.OpenFileDialog(filter)) return;

            OpenFileByPath(_defaultDialog.FilePath);
        }

        private void OpenFileByPath(string path)
        {
            Title = path + " | AR Comic Viewer";
            _archive.OpenArchive(path);
            var list = _archive.GetArchiveEntries();


            ImagePreviewList.Clear();

            foreach (var item in list)
            {
                var bar = _archive.ConvertEntryToBitmapSource(item);
                if (bar == null) continue;

                ImagePreviewList.Add(new ImagePreview()
                {
                    Title = item.Name,
                    Image = bar
                });
            }
            // ImagePreviewList = result;
            StartPanelVisibility = Visibility.Hidden;
            Reset = true;
        }




        #region The RelayCommand that implements ICommand
        public RelayCommand PreviewDropCommand
        {
            get { return _PreviewDropCommand ?? (_PreviewDropCommand = new RelayCommand(HandlePreviewDrop)); }
            set
            {
                SetField(ref _PreviewDropCommand, value, "PreviewDropCommand");
            }
        }
        private RelayCommand _PreviewDropCommand;

        #endregion

        #region The method encapsulated in the relay command
        private void HandlePreviewDrop(object inObject)
        {
            IDataObject ido = inObject as IDataObject;
            if (null == ido) return;

            string[] dropedFiles = null;

            if (ido.GetDataPresent(DataFormats.FileDrop))
            {
                dropedFiles = ido.GetData(DataFormats.FileDrop, true) as string[];
            }

            OpenFileByPath(dropedFiles[0]);

            // Do what you need here based on the format passed in.
            // You will probably have a few options and you need to
            // decide an order of preference.
        }
        #endregion







    }
}