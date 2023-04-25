namespace SVG_editor_finalproject.Handlers
{
    public class FileHandler
    {
        public string fileName = "";
        private bool _modified;

        public OpenFileDialog OpenDialog { get; } = new();
        public SaveFileDialog SaveDialog { get; } = new();
        public Action<string> onOpen { get; }
        public Action<string> onSave { get; }
        public string fileExtension { get; }
        public event EventHandler ModifiedChanged;
        public bool Modified
        {
            get => _modified;
            set
            {
                _modified = value;
                ModifiedChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public FileHandler(string ext, Action<string> onSave, Action<string> onOpen)
        {
            fileExtension = ext;
            OpenDialog.Filter = $"{fileExtension.ToUpper()} files (*.{fileExtension}))|*.{fileExtension}|All files (*.*)|*.*";
            SaveDialog.Filter = $"{fileExtension.ToUpper()} files (*.{fileExtension}))|*.{fileExtension}|All files (*.*)|*.*";
            this.onSave = onSave;
            this.onOpen = onOpen;
        }
        public bool SaveWithDialog()
        {
            if (SaveDialog.ShowDialog() != DialogResult.OK)
                return false;
            fileName = SaveDialog.FileName;
            onSave(fileName);
            Modified = false;
            return true;
        }
        public bool SaveWithCurrentName()
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return SaveWithDialog();
            }
            onSave(fileName);
            Modified = false;
            return true;
        }
        public bool Open()
        {
            if (!CheckModifiedAndCanContinue())
                return false;
            if (OpenDialog.ShowDialog() != DialogResult.OK)
                return false;
            fileName = OpenDialog.FileName;
            onOpen(fileName);
            Modified = false;
            return true;
        }
        public static bool YesNo(string msg)
        {
            return MessageBox.Show(msg, "Question", MessageBoxButtons.YesNo)
                   == DialogResult.Yes;
        }
        public bool CheckModifiedAndCanContinue()
        {
            if (!Modified)
                return true;
            if (!YesNo("Document modified, do you want to save?"))
                return true;
            return SaveWithCurrentName();
        }

        public bool NewWithCheck()
        {
            if (!CheckModifiedAndCanContinue())
                return false;
            Modified = false;
            fileName = "";
            return true;
        }
    }
}
