namespace DataTransfer
{
    partial class Demo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Demo));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ButConnect = new System.Windows.Forms.Button();
            this.RecvBox = new System.Windows.Forms.TextBox();
            this.SendBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TextBoxSendFilePath = new System.Windows.Forms.TextBox();
            this.butSelFileToSend = new System.Windows.Forms.Button();
            this.butSendFile = new System.Windows.Forms.Button();
            this.progressBarTX = new System.Windows.Forms.ProgressBar();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.butSelFolderToSave = new System.Windows.Forms.Button();
            this.butOpenFolder = new System.Windows.Forms.Button();
            this.progressBarRX = new System.Windows.Forms.ProgressBar();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.butSendSMS = new System.Windows.Forms.Button();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ButConnect);
            this.groupBox1.Controls.Add(this.RecvBox);
            this.groupBox1.Controls.Add(this.SendBox);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(301, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Setting";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Receiver Port";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sender Port";
            // 
            // ButConnect
            // 
            this.ButConnect.Location = new System.Drawing.Point(193, 17);
            this.ButConnect.Name = "ButConnect";
            this.ButConnect.Size = new System.Drawing.Size(101, 48);
            this.ButConnect.TabIndex = 1;
            this.ButConnect.Text = "&Connect";
            this.ButConnect.UseVisualStyleBackColor = true;
            this.ButConnect.Click += new System.EventHandler(this.ButConnect_Click);
            // 
            // RecvBox
            // 
            this.RecvBox.Location = new System.Drawing.Point(87, 45);
            this.RecvBox.MaxLength = 5;
            this.RecvBox.Name = "RecvBox";
            this.RecvBox.Size = new System.Drawing.Size(100, 20);
            this.RecvBox.TabIndex = 0;
            this.RecvBox.Text = "63537";
            this.RecvBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // SendBox
            // 
            this.SendBox.Location = new System.Drawing.Point(87, 19);
            this.SendBox.MaxLength = 5;
            this.SendBox.Name = "SendBox";
            this.SendBox.Size = new System.Drawing.Size(100, 20);
            this.SendBox.TabIndex = 0;
            this.SendBox.Text = "63536";
            this.SendBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TextBoxSendFilePath);
            this.groupBox2.Controls.Add(this.butSelFileToSend);
            this.groupBox2.Controls.Add(this.butSendFile);
            this.groupBox2.Controls.Add(this.progressBarTX);
            this.groupBox2.Location = new System.Drawing.Point(13, 102);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(301, 107);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Send data";
            // 
            // TextBoxSendFilePath
            // 
            this.TextBoxSendFilePath.Location = new System.Drawing.Point(6, 19);
            this.TextBoxSendFilePath.Name = "TextBoxSendFilePath";
            this.TextBoxSendFilePath.ReadOnly = true;
            this.TextBoxSendFilePath.Size = new System.Drawing.Size(288, 20);
            this.TextBoxSendFilePath.TabIndex = 2;
            // 
            // butSelFileToSend
            // 
            this.butSelFileToSend.Location = new System.Drawing.Point(6, 48);
            this.butSelFileToSend.Name = "butSelFileToSend";
            this.butSelFileToSend.Size = new System.Drawing.Size(58, 23);
            this.butSelFileToSend.TabIndex = 1;
            this.butSelFileToSend.Text = "Se&lect";
            this.butSelFileToSend.UseVisualStyleBackColor = true;
            this.butSelFileToSend.Click += new System.EventHandler(this.button2_Click);
            // 
            // butSendFile
            // 
            this.butSendFile.Location = new System.Drawing.Point(236, 77);
            this.butSendFile.Name = "butSendFile";
            this.butSendFile.Size = new System.Drawing.Size(58, 23);
            this.butSendFile.TabIndex = 1;
            this.butSendFile.Text = "&Send";
            this.butSendFile.UseVisualStyleBackColor = true;
            this.butSendFile.Click += new System.EventHandler(this.butSendFile_Click);
            // 
            // progressBarTX
            // 
            this.progressBarTX.Location = new System.Drawing.Point(6, 77);
            this.progressBarTX.Name = "progressBarTX";
            this.progressBarTX.Size = new System.Drawing.Size(223, 23);
            this.progressBarTX.Step = 1;
            this.progressBarTX.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarTX.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox2);
            this.groupBox3.Controls.Add(this.butSelFolderToSave);
            this.groupBox3.Controls.Add(this.butOpenFolder);
            this.groupBox3.Controls.Add(this.progressBarRX);
            this.groupBox3.Location = new System.Drawing.Point(13, 215);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(301, 106);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Receive data";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(6, 19);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(288, 20);
            this.textBox2.TabIndex = 2;
            // 
            // butSelFolderToSave
            // 
            this.butSelFolderToSave.Location = new System.Drawing.Point(6, 45);
            this.butSelFolderToSave.Name = "butSelFolderToSave";
            this.butSelFolderToSave.Size = new System.Drawing.Size(58, 23);
            this.butSelFolderToSave.TabIndex = 1;
            this.butSelFolderToSave.Text = "&Folder";
            this.butSelFolderToSave.UseVisualStyleBackColor = true;
            this.butSelFolderToSave.Click += new System.EventHandler(this.butSelFolderToSave_Click);
            // 
            // butOpenFolder
            // 
            this.butOpenFolder.Location = new System.Drawing.Point(236, 74);
            this.butOpenFolder.Name = "butOpenFolder";
            this.butOpenFolder.Size = new System.Drawing.Size(58, 23);
            this.butOpenFolder.TabIndex = 1;
            this.butOpenFolder.Text = "&Open";
            this.butOpenFolder.UseVisualStyleBackColor = true;
            // 
            // progressBarRX
            // 
            this.progressBarRX.Location = new System.Drawing.Point(6, 74);
            this.progressBarRX.Name = "progressBarRX";
            this.progressBarRX.Size = new System.Drawing.Size(223, 23);
            this.progressBarRX.Step = 1;
            this.progressBarRX.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBarRX.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.butSendSMS);
            this.groupBox4.Controls.Add(this.textBox5);
            this.groupBox4.Controls.Add(this.richTextBox1);
            this.groupBox4.Location = new System.Drawing.Point(321, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 308);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "SMS";
            // 
            // butSendSMS
            // 
            this.butSendSMS.Location = new System.Drawing.Point(152, 247);
            this.butSendSMS.Name = "butSendSMS";
            this.butSendSMS.Size = new System.Drawing.Size(42, 52);
            this.butSendSMS.TabIndex = 2;
            this.butSendSMS.Text = "&Send";
            this.butSendSMS.UseVisualStyleBackColor = true;
            this.butSendSMS.Click += new System.EventHandler(this.butSendSMS_Click);
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(7, 247);
            this.textBox5.MaxLength = 5120;
            this.textBox5.Multiline = true;
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(138, 52);
            this.textBox5.TabIndex = 1;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 19);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(188, 222);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // Demo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 333);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Demo";
            this.Text = "Demo";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Demo_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox TextBoxSendFilePath;
        private System.Windows.Forms.Button butSelFileToSend;
        private System.Windows.Forms.Button butSendFile;
        private System.Windows.Forms.ProgressBar progressBarTX;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButConnect;
        private System.Windows.Forms.TextBox RecvBox;
        private System.Windows.Forms.TextBox SendBox;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button butSelFolderToSave;
        private System.Windows.Forms.Button butOpenFolder;
        private System.Windows.Forms.ProgressBar progressBarRX;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button butSendSMS;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Timer timer1;
    }
}

