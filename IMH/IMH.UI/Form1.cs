using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace IMH.UI
{
    public partial class Form1 : Form
    {
        private ILogger Logger { get; set; }
        private Settings Settings { get; set; }
        private ILogger Auditor { get; set; }
        private FileDirOperations FileDirOperations { get; set; }
        private readonly Regex _regexOldCaseRef = new Regex("^[1]{1}[0-9]{5}[\\.]{1}[0-9]{3}$", RegexOptions.Compiled);
        private readonly Regex _regexNewCaseRef = new Regex("^[3]{1}[0-9]{5}$", RegexOptions.Compiled);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Logger = new FileLogger();
            Settings = SettingsReader.Read(Logger);
            Auditor = new AuditLogger();
            FileDirOperations = new FileDirOperations(Logger, Settings);
            FileDirOperations.CreateDirsIfNotExist();
            RefreshView();
            Logger.AddInfo(string.Format("{0} Started", this.Text));
            Auditor.AddInfo(string.Format("1,{0} Started", this.Text));
        }

        private void RefreshFiles()
        {
            var files = Directory.GetFiles(Settings.PdfFilesFolderPath);
            var pdfFiles = files.Where(x => x.ToLower().EndsWith("pdf"));
            FilesListBox.Items.Clear();
            FilesListBox.Items.AddRange(pdfFiles.ToArray());
        }

        private void AboveOcrCaseRefIsCorrectButton_Click(object sender, EventArgs e)
        {
            if ((FilesListBox.Items.Count <= 0) || (FilesListBox.SelectedIndex == -1)) return;
            CorrectCaseRefTextBox.Text = OcrCaseRefTextBox.Text;
            SubmitButton.Enabled = true;
            Auditor.AddInfo(string.Format("5,User accepted OCR Case Ref: {0}", OcrCaseRefTextBox.Text));
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logger.AddInfo(string.Format("{0} Ended", this.Text));
            Auditor.AddInfo(string.Format("0,{0} Ended", this.Text));
            PDFViewer.Dispose();
        }

        private void FilesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearFields();
            ClearPdfViewer();

            if (FilesListBox.SelectedIndex > -1)
            {
                var path = (string) FilesListBox.SelectedItem;
                SetPdf(path);
                UpdateOcrCaseRef(path);
            }
        }

        private void ClearFields()
        {
            OcrCaseRefTextBox.Text = string.Empty;
            CorrectCaseRefTextBox.Text = string.Empty;
            CorrectCaseRefTextBox.Enabled = false;
            SubmitButton.Enabled = false;
            UnknownCaseRefButton.Enabled = false;
        }

        private void UpdateOcrCaseRef(string path)
        {
            var fileName = Path.GetFileName(path);
            var ocrCaseRef = string.Empty;
            var strEnded = false;
            for (var i = 0; i < fileName.Length; i++)
            {
                var c = fileName[i];
                if (c == '_') strEnded = true;
                if (i > 1 && !strEnded) ocrCaseRef += c;
            }

            OcrCaseRefTextBox.Text = ocrCaseRef;
        }

        private void SetPdf(string path)
        {
            PDFViewer.LoadFile(path);
            PDFViewer.src = path;
            PDFViewer.setView("fitH");
            PDFViewer.setLayoutMode("SinglePage");
            PDFViewer.setPageMode("none");
            PDFViewer.gotoFirstPage();
            PDFViewer.Show();
        }

        private void ClearPdfViewer()
        {
            PDFViewer.src = string.Empty;
            PDFViewer.Hide();
        }

        private void RefreshFilesButton_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void RefreshView()
        {
            ClearPdfViewer();
            RefreshFiles();
            if (FilesListBox.Items.Count > 0)
            {
                FilesListBox.SelectedIndex = 0;
            }
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if ((FilesListBox.Items.Count <= 0) || (FilesListBox.SelectedIndex == -1)) return;
            var valid = ValidateCaseRef();
            if (!valid)
            {
                MessageBox.Show(@"Incorrect Case Ref. E.g 123456.123, 321321");
            }
            else
            {
                Auditor.AddInfo(string.Format("20,User submitted PDF file. Case Ref: [{0}] - PDF File [{1}]", OcrCaseRefTextBox.Text, (string)FilesListBox.SelectedItem));
                FileDirOperations.MoveToSubmittedFolder((string)FilesListBox.SelectedItem, CorrectCaseRefTextBox.Text);
                RefreshView();
            }
        }

        private bool ValidateCaseRef()
        {
            if (string.IsNullOrEmpty(CorrectCaseRefTextBox.Text)) return false;
            if (_regexOldCaseRef.IsMatch(CorrectCaseRefTextBox.Text)) return true;
            if (_regexNewCaseRef.IsMatch(CorrectCaseRefTextBox.Text)) return true;
            return false;
        }

        private void UnknownCaseRefButton_Click(object sender, EventArgs e)
        {
            if ((FilesListBox.Items.Count <= 0) || (FilesListBox.SelectedIndex == -1)) return;
            Auditor.AddInfo(string.Format("30,User marked PDF file as unknown Case Ref. Case Ref: [{0}] - PDF file [{1}]", OcrCaseRefTextBox.Text, (string)FilesListBox.SelectedItem));
            FileDirOperations.MoveToUnknownCaseRefFolder((string)FilesListBox.SelectedItem);
            RefreshView();
        }

        private void AboveOcrCaseRefIsIncorrectButton_Click(object sender, EventArgs e)
        {
            if ((FilesListBox.Items.Count <= 0) || (FilesListBox.SelectedIndex == -1)) return;
            CorrectCaseRefTextBox.Enabled = true;
            SubmitButton.Enabled = true;
            UnknownCaseRefButton.Enabled = true;
            Auditor.AddInfo(string.Format("10,User discarded OCR Case Ref: {0}", OcrCaseRefTextBox.Text));
            FileDirOperations.CopyException((string)FilesListBox.SelectedItem);
        }
    }
}
