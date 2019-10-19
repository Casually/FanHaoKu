using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using 番号库查看系统;

namespace 生成
{
    public partial class Form2 : Form
    {
        string path1 = Application.StartupPath.ToString() + "\\AutoRunBat.bat";
        string path = Application.StartupPath.ToString() + "\\视频\\";
        private string Tex;
        private string Tex1;
        int i = 0;
        int n = 0;
        int N = 0;

        double sc = 0;
        int count = 0;

        public Thread thread;
        public Thread thread1;

        public Form1 frm1;
        public Form2(Form1 form1)
        {
            
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            Text = path;
            frm1 = form1;
        }


        private void Button1_Click(object sender, EventArgs e)
        {
            thread1 = new Thread(() => { StartWork1(); });
            thread1.IsBackground = true;
            thread1.Start();

        }

        
        private void Button2_Click(object sender, EventArgs e)
        {
            
            if (textBox1.Text == "")
            {
                MessageBox.Show("请先扫描文件", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                thread = new Thread(() => { StartWork(); });
                thread.IsBackground = true;
                thread.Start();   
            }
            
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                MessageBox.Show("请先生成相关文件", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(path1, true, Encoding.Default))
                {
                    sw.Write(textBox2.Text.ToString());
                    MessageBox.Show("写入文件成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    sw.Close();
                }
                button4.Enabled = true;
                button5.Enabled = true;
                button3.Enabled = false;
                button6.Enabled = true;
            }

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            if (File.Exists(path1))
            {
                System.Diagnostics.Process.Start(path1);
            }
            else
            {
                MessageBox.Show("未发现相关文件", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            
            if (File.Exists(path1))
            {
                File.Delete(path1);
                
                MessageBox.Show("删除文件成功", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("未发现相关文件", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            textBox1.Text = "";
            textBox2.Text = "";

            button1.Enabled = true;
            button4.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;

            label1.Text = "0 文件";
            label2.Text = "0 文件";

            progressBar1.Value = 0;
            progressBar2.Value = 0;
            label3.Text = "0.0 %";
            label4.Text = "0.0 %";
            sc = 0;
            count = 0;
            i = 0;
            n = 0;
            N = 0;
        }

        void StartWork()
        {
            Tex = textBox3.Text;
            Tex1 = textBox4.Text;
            if (string.IsNullOrEmpty(Tex) || string.IsNullOrEmpty(Tex1))
            {
                MessageBox.Show("请选择要保存的文件夹", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            sc = 0;
            count = 0;
            button2.Enabled = false;
            textBox2.Text = "";
            textBox2.AppendText("@echo off" + "\r\n\r\n");
            for (n = 0; n < i; n++)
            {
                textBox2.AppendText("\"C:\\Program Files\\7-Zip\\7z.exe\"" + " a ");
                textBox2.AppendText("\"");
                textBox2.AppendText(textBox4.Text);
                textBox2.AppendText("\\");
                textBox2.AppendText(textBox1.Lines[n]);
                textBox2.AppendText("\\" + textBox1.Lines[n]);
                textBox2.AppendText(".7z" + "\"" + " ");
                textBox2.AppendText("\"");
                textBox2.AppendText(textBox3.Text);
                textBox2.AppendText("\\");
                textBox2.AppendText(textBox1.Lines[n].Substring(0, textBox1.Lines[n].IndexOf(' ')));
                textBox2.AppendText("*" + "." + "*" + "\"" + " -v1000m");

                textBox2.AppendText("\r\n\r\n");
                N++;

                count++;
                sc = count * 1.0 / Convert.ToInt32(frm1.textBox11.Text);

                progressBar2.BeginInvoke(new Action(() =>
                {
                    progressBar2.Value++;

                }));
                label4.Text = sc.ToString("0.0 %");
                label2.Text = N.ToString() + " 文件";
                //Thread.Sleep(500);
            }
            
            button3.Enabled = true;
        }

        void StartWork1()
        {
            button1.Enabled = false;
            textBox1.Text = "";
            textBox2.Text = "";


            if (Directory.Exists(Application.StartupPath.ToString() + "\\视频\\"))
            {

                DirectoryInfo folder = new DirectoryInfo(path);

                foreach (FileInfo file in folder.GetFiles())
                {
                    
                    if (Path.GetExtension(file.Name) == ".mp4" ||
                        Path.GetExtension(file.Name) == ".mkv" ||
                        Path.GetExtension(file.Name) == ".wmv" ||
                        Path.GetExtension(file.Name) == ".avi" ||
                        Path.GetExtension(file.Name) == ".iso")
                    {
                        count++;
                        sc = count * 1.0 / Convert.ToInt32(frm1.textBox11.Text);
                        progressBar1.BeginInvoke(new Action(() =>
                        {
                            progressBar1.Value++;

                        }));
                        label3.Text = sc.ToString("0.0 %");
                        i = textBox1.Lines.Length - 1;

                        label1.Text = i.ToString() + " 文件";
                        textBox1.AppendText(Path.GetFileNameWithoutExtension(file.Name).ToString() + "\r\n");
                        i = textBox1.Lines.Length - 1;
                        label1.Text = i.ToString() + " 文件";
                        //Thread.Sleep(500);
                    }
                }
            }
            else
            {
                MessageBox.Show("视频文件夹不存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }

            if (textBox1.Text == "")
            {
                MessageBox.Show("相关文件不存在", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                button1.Enabled = true;
                button2.Enabled = false;
            }
            else
            {
                i = textBox1.Lines.Length - 1;
                label1.Text = i.ToString() + " 文件";
                button2.Enabled = true;
            }
            
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //button1.Enabled = false;

            progressBar1.Maximum = Convert.ToInt32(frm1.textBox11.Text);
            progressBar2.Maximum = Convert.ToInt32(frm1.textBox11.Text);

            button2.Enabled = false;
            button3.Enabled = false;

            if (File.Exists(path1))
            {
                button1.Enabled = false;
                button4.Enabled = true;
                button5.Enabled = true;
                button6.Enabled = true;
            }
            else
            {
                button1.Enabled = true;
                button4.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }
 
        }

        private void button6_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad.exe", path1);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = dlg.SelectedPath;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = dlg.SelectedPath;
            }
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(label3.Text == "0.0 %" || label3.Text == "100.0 %")
            {
                if (label4.Text == "0.0 %" || label4.Text == "100.0 %")
                {
                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                }
                 
            }
            else
            {
                e.Cancel = true;
            }
            
        }
    }


}
