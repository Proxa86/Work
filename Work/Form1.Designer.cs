namespace Work
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
            this.buttonOpenFileCreated = new System.Windows.Forms.Button();
            this.buttonOpenFileInput = new System.Windows.Forms.Button();
            this.buttonFindFilesJava = new System.Windows.Forms.Button();
            this.buttonOpenFileSrc = new System.Windows.Forms.Button();
            this.buttonOpenFileBin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonDeleteFileFilter = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBoxForFindFilesJava = new System.Windows.Forms.TextBox();
            this.buttonOpenReport = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.lbPersent = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonFindFilesJava2 = new System.Windows.Forms.Button();
            this.buttonOpenReport2 = new System.Windows.Forms.Button();
            this.textBoxFilter = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageJava = new System.Windows.Forms.TabPage();
            this.tabPageC = new System.Windows.Forms.TabPage();
            this.tabPageDif = new System.Windows.Forms.TabPage();
            this.buttonChoiceFileText = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonChoiceFolder = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageJava.SuspendLayout();
            this.tabPageC.SuspendLayout();
            this.tabPageDif.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOpenFileCreated
            // 
            this.buttonOpenFileCreated.Location = new System.Drawing.Point(20, 30);
            this.buttonOpenFileCreated.Name = "buttonOpenFileCreated";
            this.buttonOpenFileCreated.Size = new System.Drawing.Size(95, 23);
            this.buttonOpenFileCreated.TabIndex = 0;
            this.buttonOpenFileCreated.Text = "OpenCreatedFile";
            this.buttonOpenFileCreated.UseVisualStyleBackColor = true;
            this.buttonOpenFileCreated.Click += new System.EventHandler(this.buttonOpenFile_Click);
            // 
            // buttonOpenFileInput
            // 
            this.buttonOpenFileInput.Location = new System.Drawing.Point(141, 30);
            this.buttonOpenFileInput.Name = "buttonOpenFileInput";
            this.buttonOpenFileInput.Size = new System.Drawing.Size(95, 23);
            this.buttonOpenFileInput.TabIndex = 1;
            this.buttonOpenFileInput.Text = "OpenInputFile";
            this.buttonOpenFileInput.UseVisualStyleBackColor = true;
            this.buttonOpenFileInput.Click += new System.EventHandler(this.buttonOpenFileInput_Click);
            // 
            // buttonFindFilesJava
            // 
            this.buttonFindFilesJava.Location = new System.Drawing.Point(20, 88);
            this.buttonFindFilesJava.Name = "buttonFindFilesJava";
            this.buttonFindFilesJava.Size = new System.Drawing.Size(95, 23);
            this.buttonFindFilesJava.TabIndex = 2;
            this.buttonFindFilesJava.Text = "FindFilesJava";
            this.buttonFindFilesJava.UseVisualStyleBackColor = true;
            this.buttonFindFilesJava.Click += new System.EventHandler(this.buttonFindFiles_Click);
            // 
            // buttonOpenFileSrc
            // 
            this.buttonOpenFileSrc.Location = new System.Drawing.Point(14, 34);
            this.buttonOpenFileSrc.Name = "buttonOpenFileSrc";
            this.buttonOpenFileSrc.Size = new System.Drawing.Size(95, 23);
            this.buttonOpenFileSrc.TabIndex = 7;
            this.buttonOpenFileSrc.Text = "OpenFileSrc";
            this.buttonOpenFileSrc.UseVisualStyleBackColor = true;
            this.buttonOpenFileSrc.Click += new System.EventHandler(this.buttonOpenFileSrc_Click);
            // 
            // buttonOpenFileBin
            // 
            this.buttonOpenFileBin.Location = new System.Drawing.Point(148, 34);
            this.buttonOpenFileBin.Name = "buttonOpenFileBin";
            this.buttonOpenFileBin.Size = new System.Drawing.Size(95, 23);
            this.buttonOpenFileBin.TabIndex = 8;
            this.buttonOpenFileBin.Text = "OpenFileBin";
            this.buttonOpenFileBin.UseVisualStyleBackColor = true;
            this.buttonOpenFileBin.Click += new System.EventHandler(this.buttonOpenFileBin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(67, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Работа с NOP С/С++";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(178, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Работа с Java, Class, Jar файлами";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(228, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Поиск файлов Java из файлов MavenStatus";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(153, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Удаление все файлы кроме:";
            // 
            // buttonDeleteFileFilter
            // 
            this.buttonDeleteFileFilter.Location = new System.Drawing.Point(13, 35);
            this.buttonDeleteFileFilter.Name = "buttonDeleteFileFilter";
            this.buttonDeleteFileFilter.Size = new System.Drawing.Size(95, 23);
            this.buttonDeleteFileFilter.TabIndex = 13;
            this.buttonDeleteFileFilter.Text = "DeleteFile";
            this.buttonDeleteFileFilter.UseVisualStyleBackColor = true;
            this.buttonDeleteFileFilter.UseWaitCursor = true;
            this.buttonDeleteFileFilter.Click += new System.EventHandler(this.buttonDeleteFileFilter_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 358);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(265, 23);
            this.progressBar1.TabIndex = 14;
            // 
            // textBoxForFindFilesJava
            // 
            this.textBoxForFindFilesJava.Location = new System.Drawing.Point(20, 128);
            this.textBoxForFindFilesJava.Name = "textBoxForFindFilesJava";
            this.textBoxForFindFilesJava.Size = new System.Drawing.Size(232, 20);
            this.textBoxForFindFilesJava.TabIndex = 15;
            // 
            // buttonOpenReport
            // 
            this.buttonOpenReport.Location = new System.Drawing.Point(154, 88);
            this.buttonOpenReport.Name = "buttonOpenReport";
            this.buttonOpenReport.Size = new System.Drawing.Size(95, 23);
            this.buttonOpenReport.TabIndex = 16;
            this.buttonOpenReport.Text = "OpenReport";
            this.buttonOpenReport.UseVisualStyleBackColor = true;
            this.buttonOpenReport.Click += new System.EventHandler(this.buttonOpenReport_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // lbPersent
            // 
            this.lbPersent.AutoSize = true;
            this.lbPersent.Location = new System.Drawing.Point(16, 392);
            this.lbPersent.Name = "lbPersent";
            this.lbPersent.Size = new System.Drawing.Size(88, 13);
            this.lbPersent.TabIndex = 17;
            this.lbPersent.Text = "Processing ... 0%";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 188);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Работа с Java и Jar файлами";
            // 
            // buttonFindFilesJava2
            // 
            this.buttonFindFilesJava2.Location = new System.Drawing.Point(14, 212);
            this.buttonFindFilesJava2.Name = "buttonFindFilesJava2";
            this.buttonFindFilesJava2.Size = new System.Drawing.Size(91, 23);
            this.buttonFindFilesJava2.TabIndex = 19;
            this.buttonFindFilesJava2.Text = "FindFiles";
            this.buttonFindFilesJava2.UseVisualStyleBackColor = true;
            this.buttonFindFilesJava2.Click += new System.EventHandler(this.buttonFindFilesJava2_Click);
            // 
            // buttonOpenReport2
            // 
            this.buttonOpenReport2.Location = new System.Drawing.Point(144, 212);
            this.buttonOpenReport2.Name = "buttonOpenReport2";
            this.buttonOpenReport2.Size = new System.Drawing.Size(95, 23);
            this.buttonOpenReport2.TabIndex = 20;
            this.buttonOpenReport2.Text = "OpenReport";
            this.buttonOpenReport2.UseVisualStyleBackColor = true;
            this.buttonOpenReport2.Click += new System.EventHandler(this.buttonOpenReport2_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.Location = new System.Drawing.Point(147, 35);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(100, 20);
            this.textBoxFilter.TabIndex = 21;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageJava);
            this.tabControl1.Controls.Add(this.tabPageC);
            this.tabControl1.Controls.Add(this.tabPageDif);
            this.tabControl1.Location = new System.Drawing.Point(3, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(269, 351);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPageJava
            // 
            this.tabPageJava.Controls.Add(this.label3);
            this.tabPageJava.Controls.Add(this.buttonOpenFileCreated);
            this.tabPageJava.Controls.Add(this.buttonOpenReport2);
            this.tabPageJava.Controls.Add(this.buttonOpenFileInput);
            this.tabPageJava.Controls.Add(this.buttonFindFilesJava2);
            this.tabPageJava.Controls.Add(this.label2);
            this.tabPageJava.Controls.Add(this.label5);
            this.tabPageJava.Controls.Add(this.textBoxForFindFilesJava);
            this.tabPageJava.Controls.Add(this.buttonFindFilesJava);
            this.tabPageJava.Controls.Add(this.buttonOpenReport);
            this.tabPageJava.Location = new System.Drawing.Point(4, 22);
            this.tabPageJava.Name = "tabPageJava";
            this.tabPageJava.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageJava.Size = new System.Drawing.Size(261, 325);
            this.tabPageJava.TabIndex = 0;
            this.tabPageJava.Text = "Java";
            this.tabPageJava.UseVisualStyleBackColor = true;
            // 
            // tabPageC
            // 
            this.tabPageC.Controls.Add(this.buttonOpenFileBin);
            this.tabPageC.Controls.Add(this.buttonOpenFileSrc);
            this.tabPageC.Controls.Add(this.label1);
            this.tabPageC.Location = new System.Drawing.Point(4, 22);
            this.tabPageC.Name = "tabPageC";
            this.tabPageC.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageC.Size = new System.Drawing.Size(261, 325);
            this.tabPageC.TabIndex = 1;
            this.tabPageC.Text = "C++";
            this.tabPageC.UseVisualStyleBackColor = true;
            // 
            // tabPageDif
            // 
            this.tabPageDif.Controls.Add(this.buttonChoiceFolder);
            this.tabPageDif.Controls.Add(this.label6);
            this.tabPageDif.Controls.Add(this.buttonChoiceFileText);
            this.tabPageDif.Controls.Add(this.textBoxFilter);
            this.tabPageDif.Controls.Add(this.label4);
            this.tabPageDif.Controls.Add(this.buttonDeleteFileFilter);
            this.tabPageDif.Location = new System.Drawing.Point(4, 22);
            this.tabPageDif.Name = "tabPageDif";
            this.tabPageDif.Size = new System.Drawing.Size(261, 325);
            this.tabPageDif.TabIndex = 2;
            this.tabPageDif.Text = "Different";
            this.tabPageDif.UseVisualStyleBackColor = true;
            // 
            // buttonChoiceFileText
            // 
            this.buttonChoiceFileText.Location = new System.Drawing.Point(12, 101);
            this.buttonChoiceFileText.Name = "buttonChoiceFileText";
            this.buttonChoiceFileText.Size = new System.Drawing.Size(96, 23);
            this.buttonChoiceFileText.TabIndex = 22;
            this.buttonChoiceFileText.Text = "ChoiceFile";
            this.buttonChoiceFileText.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(151, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Удаление файлов по списку";
            // 
            // buttonChoiceFolder
            // 
            this.buttonChoiceFolder.Location = new System.Drawing.Point(147, 101);
            this.buttonChoiceFolder.Name = "buttonChoiceFolder";
            this.buttonChoiceFolder.Size = new System.Drawing.Size(83, 23);
            this.buttonChoiceFolder.TabIndex = 24;
            this.buttonChoiceFolder.Text = "ChoiceFolder";
            this.buttonChoiceFolder.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 409);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lbPersent);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPageJava.ResumeLayout(false);
            this.tabPageJava.PerformLayout();
            this.tabPageC.ResumeLayout(false);
            this.tabPageC.PerformLayout();
            this.tabPageDif.ResumeLayout(false);
            this.tabPageDif.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenFileCreated;
        private System.Windows.Forms.Button buttonOpenFileInput;
        private System.Windows.Forms.Button buttonFindFilesJava;
        private System.Windows.Forms.Button buttonOpenFileSrc;
        private System.Windows.Forms.Button buttonOpenFileBin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonDeleteFileFilter;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBoxForFindFilesJava;
        private System.Windows.Forms.Button buttonOpenReport;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Label lbPersent;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonFindFilesJava2;
        private System.Windows.Forms.Button buttonOpenReport2;
        private System.Windows.Forms.TextBox textBoxFilter;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageJava;
        private System.Windows.Forms.TabPage tabPageC;
        private System.Windows.Forms.TabPage tabPageDif;
        private System.Windows.Forms.Button buttonChoiceFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonChoiceFileText;
    }
}

