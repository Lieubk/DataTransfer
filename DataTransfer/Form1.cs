using DataTransfer.Protocol;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DataTransfer
{

    public partial class Demo : Form
    {

        private DataTrans handler = new DataTrans();
        private bool TXIntrFlag = false;

        private int P_TX_Val = 0;
        private int P_RX_Val = 0;

        public Demo()
        {
            InitializeComponent();
            handler.ProgressUpdateHandler += new DataTransEvent(UpdateProgressPoll);
            handler.UpdateSMSHandler += new DataTransEvent(UpdateSMSPool);

            timer1.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
            timer1.Interval = (200) * (1);              // Timer will tick event 
            timer1.Start();

        }

        private void UpdateSMSPool(object param)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(MainSMSBOXUpdate), param);
        }

        private void MainSMSBOXUpdate(object state)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    richTextBox1.Text += state as string;
                    richTextBox1.Focus();
                    richTextBox1.Select(richTextBox1.Text.Length, 0);
                });
            }
            else
            {
                richTextBox1.Text += state as string;
                richTextBox1.Focus();
                richTextBox1.Select(richTextBox1.Text.Length, 0);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Update view

            progressBarTX.Value = P_TX_Val;
            progressBarRX.Value = P_RX_Val;
        }

        private void UpdateProgressPoll(object param)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(MainProgressUpdate), param);
        }

        private void MainProgressUpdate(object state)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate
                {
                    if (handler.IsSendingData)
                    {
                        if ((int)state >= 0 && (int)state <=100)
                            P_TX_Val = (int)state;
                    }
                    else
                    {
                        if ((int)state >= 0 && (int)state <= 100)
                            P_RX_Val = (int)state;
                    }


                    if((int)state == 100)
                    {
                        MessageBox.Show(this, "Done!", "Notice");
                    }
                });
            }
            else
            {
                if (handler.IsSendingData)
                {
                    if ((int)state >= 0 && (int)state <= 100)
                        progressBarTX.Value = (int)state;
                }
                else
                {
                    if ((int)state >= 0 && (int)state <= 100)
                        progressBarRX.Value = (int)state;
                }

                if ((int)state == 100)
                {
                    MessageBox.Show(this, "Done!", "Notice");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TextBoxSendFilePath.Text = dlg.FileName;
            }
        }

        private void ButConnect_Click(object sender, EventArgs e)
        {
            if(ButConnect.Text == "&Connect")
            {
                ButConnect.Text = "&Disconnect";
                SendBox.Enabled = false;
                RecvBox.Enabled = false;
                // Connect to UDP port
                try
                {
                    handler.ConnectToUDPPort(RecvBox.Text, SendBox.Text);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(this, ex.Message, "Notice");
                }
            }
            else
            {
                ButConnect.Text = "&Connect";
                SendBox.Enabled = true;
                RecvBox.Enabled = true;
                //DisConnect from UDP port
                handler.Disconnect();
            }
        }

        private void TextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        private void butSelFolderToSave_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox2.Text = dlg.SelectedPath;
                handler.SaveFolderPath = dlg.SelectedPath; 
            }
        }

        private void butSendFile_Click(object sender, EventArgs e)
        {
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += delegate
            {
                byte[] buf = null;
                try
                {
	                buf = File.ReadAllBytes(TextBoxSendFilePath.Text);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("File corrupt!", "Alert");
                }
                if (handler.IsConnect)
                {
                    if(buf != null)
                        handler.SendDataPackage(buf, TextBoxSendFilePath.Text, ref TXIntrFlag);

                }
                else
                    MessageBox.Show("UDP Port has not connect yet!","Alert");
            };
            worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(FuncRunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        private void FuncRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TXIntrFlag = false;
        }

        private void Demo_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Stop();
        }

        private void butSendSMS_Click(object sender, EventArgs e)
        {
            //Send SMS
            System.ComponentModel.BackgroundWorker worker = new System.ComponentModel.BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += delegate
            {
                byte[] buf = null;
                try
                {
                    buf = Encoding.ASCII.GetBytes(textBox5.Text);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Alert");
                }
                if (handler.IsConnect)
                {
                    if (buf != null)
                        handler.SendSMS(buf);

                }
                else
                    MessageBox.Show("UDP Port has not connect yet!", "Alert");
            };
//             worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(FuncRunWorkerCompleted);
            worker.RunWorkerAsync();
        }
    }
}
