namespace IMH.UI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RefreshFilesButton = new System.Windows.Forms.Button();
            this.FilesListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.AboveOcrCaseRefIsIncorrectButton = new System.Windows.Forms.Button();
            this.UnknownCaseRefButton = new System.Windows.Forms.Button();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.CorrectCaseRefTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AboveOcrCaseRefIsCorrectButton = new System.Windows.Forms.Button();
            this.OcrCaseRefTextBox = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.PDFViewer = new AxAcroPDFLib.AxAcroPDF();
            this.CorrComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PDFViewer)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.RefreshFilesButton);
            this.groupBox1.Controls.Add(this.FilesListBox);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 397);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Files";
            // 
            // RefreshFilesButton
            // 
            this.RefreshFilesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.RefreshFilesButton.BackColor = System.Drawing.SystemColors.ControlDark;
            this.RefreshFilesButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshFilesButton.Location = new System.Drawing.Point(6, 354);
            this.RefreshFilesButton.Name = "RefreshFilesButton";
            this.RefreshFilesButton.Size = new System.Drawing.Size(309, 37);
            this.RefreshFilesButton.TabIndex = 7;
            this.RefreshFilesButton.Text = "Refresh Files";
            this.RefreshFilesButton.UseVisualStyleBackColor = false;
            this.RefreshFilesButton.Click += new System.EventHandler(this.RefreshFilesButton_Click);
            // 
            // FilesListBox
            // 
            this.FilesListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.FilesListBox.FormattingEnabled = true;
            this.FilesListBox.Location = new System.Drawing.Point(6, 19);
            this.FilesListBox.Name = "FilesListBox";
            this.FilesListBox.Size = new System.Drawing.Size(309, 329);
            this.FilesListBox.TabIndex = 0;
            this.FilesListBox.SelectedIndexChanged += new System.EventHandler(this.FilesListBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.CorrComboBox);
            this.groupBox2.Controls.Add(this.AboveOcrCaseRefIsIncorrectButton);
            this.groupBox2.Controls.Add(this.UnknownCaseRefButton);
            this.groupBox2.Controls.Add(this.SubmitButton);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.CorrectCaseRefTextBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.AboveOcrCaseRefIsCorrectButton);
            this.groupBox2.Controls.Add(this.OcrCaseRefTextBox);
            this.groupBox2.Location = new System.Drawing.Point(12, 415);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(321, 302);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Controls";
            // 
            // AboveOcrCaseRefIsIncorrectButton
            // 
            this.AboveOcrCaseRefIsIncorrectButton.BackColor = System.Drawing.Color.LightCoral;
            this.AboveOcrCaseRefIsIncorrectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AboveOcrCaseRefIsIncorrectButton.Location = new System.Drawing.Point(6, 97);
            this.AboveOcrCaseRefIsIncorrectButton.Name = "AboveOcrCaseRefIsIncorrectButton";
            this.AboveOcrCaseRefIsIncorrectButton.Size = new System.Drawing.Size(309, 37);
            this.AboveOcrCaseRefIsIncorrectButton.TabIndex = 8;
            this.AboveOcrCaseRefIsIncorrectButton.Text = "OCR Case Ref is INCORRECT";
            this.AboveOcrCaseRefIsIncorrectButton.UseVisualStyleBackColor = false;
            this.AboveOcrCaseRefIsIncorrectButton.Click += new System.EventHandler(this.AboveOcrCaseRefIsIncorrectButton_Click);
            // 
            // UnknownCaseRefButton
            // 
            this.UnknownCaseRefButton.BackColor = System.Drawing.Color.Gold;
            this.UnknownCaseRefButton.Enabled = false;
            this.UnknownCaseRefButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.UnknownCaseRefButton.Location = new System.Drawing.Point(6, 259);
            this.UnknownCaseRefButton.Name = "UnknownCaseRefButton";
            this.UnknownCaseRefButton.Size = new System.Drawing.Size(309, 37);
            this.UnknownCaseRefButton.TabIndex = 7;
            this.UnknownCaseRefButton.Text = "Unknown Case Ref";
            this.UnknownCaseRefButton.UseVisualStyleBackColor = false;
            this.UnknownCaseRefButton.Click += new System.EventHandler(this.UnknownCaseRefButton_Click);
            // 
            // SubmitButton
            // 
            this.SubmitButton.BackColor = System.Drawing.Color.PaleTurquoise;
            this.SubmitButton.Enabled = false;
            this.SubmitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SubmitButton.Location = new System.Drawing.Point(6, 216);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(309, 37);
            this.SubmitButton.TabIndex = 6;
            this.SubmitButton.Text = "Submit PDF File";
            this.SubmitButton.UseVisualStyleBackColor = false;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Correct Case Ref:";
            // 
            // CorrectCaseRefTextBox
            // 
            this.CorrectCaseRefTextBox.Enabled = false;
            this.CorrectCaseRefTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CorrectCaseRefTextBox.Location = new System.Drawing.Point(103, 140);
            this.CorrectCaseRefTextBox.Name = "CorrectCaseRefTextBox";
            this.CorrectCaseRefTextBox.Size = new System.Drawing.Size(212, 29);
            this.CorrectCaseRefTextBox.TabIndex = 4;
            this.CorrectCaseRefTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "OCR Case Ref:";
            // 
            // AboveOcrCaseRefIsCorrectButton
            // 
            this.AboveOcrCaseRefIsCorrectButton.BackColor = System.Drawing.Color.LightGreen;
            this.AboveOcrCaseRefIsCorrectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AboveOcrCaseRefIsCorrectButton.Location = new System.Drawing.Point(6, 54);
            this.AboveOcrCaseRefIsCorrectButton.Name = "AboveOcrCaseRefIsCorrectButton";
            this.AboveOcrCaseRefIsCorrectButton.Size = new System.Drawing.Size(309, 37);
            this.AboveOcrCaseRefIsCorrectButton.TabIndex = 2;
            this.AboveOcrCaseRefIsCorrectButton.Text = "OCR Case Ref is CORRECT";
            this.AboveOcrCaseRefIsCorrectButton.UseVisualStyleBackColor = false;
            this.AboveOcrCaseRefIsCorrectButton.Click += new System.EventHandler(this.AboveOcrCaseRefIsCorrectButton_Click);
            // 
            // OcrCaseRefTextBox
            // 
            this.OcrCaseRefTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OcrCaseRefTextBox.Location = new System.Drawing.Point(103, 19);
            this.OcrCaseRefTextBox.Name = "OcrCaseRefTextBox";
            this.OcrCaseRefTextBox.ReadOnly = true;
            this.OcrCaseRefTextBox.Size = new System.Drawing.Size(212, 29);
            this.OcrCaseRefTextBox.TabIndex = 0;
            this.OcrCaseRefTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.PDFViewer);
            this.groupBox3.Location = new System.Drawing.Point(339, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1033, 705);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PDF View";
            // 
            // PDFViewer
            // 
            this.PDFViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PDFViewer.Enabled = true;
            this.PDFViewer.Location = new System.Drawing.Point(6, 19);
            this.PDFViewer.Name = "PDFViewer";
            this.PDFViewer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("PDFViewer.OcxState")));
            this.PDFViewer.Size = new System.Drawing.Size(1021, 680);
            this.PDFViewer.TabIndex = 0;
            // 
            // CorrComboBox
            // 
            this.CorrComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.CorrComboBox.FormattingEnabled = true;
            this.CorrComboBox.Location = new System.Drawing.Point(103, 178);
            this.CorrComboBox.Name = "CorrComboBox";
            this.CorrComboBox.Size = new System.Drawing.Size(212, 32);
            this.CorrComboBox.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Correspondent:";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 729);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Incoming Mail Helper";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PDFViewer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox FilesListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CorrectCaseRefTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AboveOcrCaseRefIsCorrectButton;
        private System.Windows.Forms.TextBox OcrCaseRefTextBox;
        private System.Windows.Forms.Button RefreshFilesButton;
        private System.Windows.Forms.Button SubmitButton;
        private System.Windows.Forms.Button UnknownCaseRefButton;
        private AxAcroPDFLib.AxAcroPDF PDFViewer;
        private System.Windows.Forms.Button AboveOcrCaseRefIsIncorrectButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox CorrComboBox;
    }
}

