using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace 番号库查看系统
{
    public partial class Form4 : Form
    {
        public Form1 frm1;
        public string path1;// = string.Empty;
        public Thread thread;
        public Form4(Form1 form1)
        {         
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            frm1 = form1;            
        }

        void StartWork()
        {

            //path1 = "F:\\我的视频\\玉生烟练习室版.flv";
            path1 = Application.StartupPath.ToString() + "\\视频\\" + frm1.本地链接TextBox.Text;
            if (frm1.本地链接TextBox.Text != string.Empty)
            {
                if (File.Exists(path1))
                {
                    colorSlider1.Visible = true;
                    axPlayer1.Visible = true;
                    axPlayer1.Open(path1);
                    toolStripButton1.Enabled = false;
                    toolStripButton6.Enabled = false;
                }
                else
                {
                    MessageBox.Show(frm1.本地链接TextBox.Text + "文件不存在", "提示");

                }

            }

        }
        private void Form4_Load(object sender, System.EventArgs e)
        {
            thread = new Thread(() => { StartWork(); });
            thread.IsBackground = true;
            thread.Start();

        }

        private void toolStripButton2_Click(object sender, System.EventArgs e)
        {
            axPlayer1.Pause();
            toolStripButton1.Enabled = true;
            toolStripButton2.Enabled = false;
        }

        private void toolStripButton4_Click(object sender, System.EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, System.EventArgs e)
        {
            axPlayer1.Play();
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = true;
        }

        private void toolStripButton3_Click(object sender, System.EventArgs e)
        {
            axPlayer1.Close();
            timer1.Stop();
            toolStripButton3.Enabled = false;
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripButton6.Enabled = true;
            label2.Text = "00:00:00";
            label1.Text = "00:00:00";
            colorSlider1.Value = 0;
        }

        private void toolStripButton5_Click(object sender, System.EventArgs e)
        {

        }

        private void toolStripButton6_Click(object sender, System.EventArgs e)
        {
            string fileName;// = string.Empty; //文件名
                                            //打开文件
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.DefaultExt = ".mp4";
            dlg.Filter = "(*.mp4,*.mkv,*.flv,*.rmvb,*.mov,*.wmv  )|*.mp4;*.mkv;*.flv;*.rmvb;*.mov;*.wmv";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fileName = dlg.FileName;
                axPlayer1.Open(fileName);
                toolStripButton6.Enabled = false;
                toolStripButton3.Enabled = true;
                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = true;
                
            }
            else
            {
                ;
            }

        }
        string TimeToString(TimeSpan span)
        {
            return span.Hours.ToString("00") + ":" +
            span.Minutes.ToString("00") + ":" +
            span.Seconds.ToString("00");
        }
        private void colorSlider1_Scroll(object sender, ScrollEventArgs e)
        {
            axPlayer1.SetPosition(colorSlider1.Value);
            label1.Text = TimeToString(TimeSpan.FromSeconds(colorSlider1.Value));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = TimeToString(TimeSpan.FromMilliseconds(axPlayer1.GetPosition()));
            colorSlider1.Value = axPlayer1.GetPosition() <= 0 ? 0 : axPlayer1.GetPosition();
        }

        private void axPlayer1_OnOpenSucceeded(object sender, EventArgs e)
        {
            label1.Text = "00:00:00";
            label2.Text = TimeToString(TimeSpan.FromMilliseconds(axPlayer1.GetDuration()));
            colorSlider1.Maximum = axPlayer1.GetDuration();
            axPlayer1.SetConfig(201, "4");
            axPlayer1.SetConfig(207, "1");
            timer1.Start();
        }

        private void axPlayer1_OnDownloadCodec(object sender, AxAPlayer3Lib._IPlayerEvents_OnDownloadCodecEvent e)
        {

        }

        private void axPlayer1_OnStateChanged(object sender, AxAPlayer3Lib._IPlayerEvents_OnStateChangedEvent e)
        {
            if (e.nNewState == 0)  //就绪
            {
                timer1.Stop();
                toolStripButton3.Enabled = false;
                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = false;
                toolStripButton6.Enabled = true;
                label2.Text = "00:00:00";
                label1.Text = "00:00:00";
                colorSlider1.Value = 0;
            }

        }

    }
}
