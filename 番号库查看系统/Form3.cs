using Bgimage;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace 番号库查看系统
{
    public partial class Form3 : Form
    {
        public Form1 frm1;
        public Thread thread;
        public Thread thread1;
        string path = Application.StartupPath.ToString() + "\\封面\\";
        public Form3(Form1 form1)
        {
            InitializeComponent();
            frm1 = form1;
            Control.CheckForIllegalCrossThreadCalls = false;
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }
        public DirectoryInfo d;
        public FileInfo[] fs;
        public ImageList il;
        int i;
        void StartWork()
        {

            d = new DirectoryInfo(path);//图片目录
            fs = d.GetFiles("*.png");//图片格式
            il = new ImageList();
            il.ColorDepth = ColorDepth.Depth32Bit;
            il.ImageSize = new Size(200, 134);//显示大小
            this.listView1.LargeImageList = il;
            for (i = 0; i < fs.Length; i++)
            {
                progressBar1.BeginInvoke(new Action(() =>
                {
                    progressBar1.Value++;

                }));
                il.Images.Add(Image.FromFile(fs[i].FullName));
                this.listView1.Items.Add(fs[i].Name, i);
            }

        }

        private void Form3_Load(object sender, System.EventArgs e)
        {
            progressBar1.Maximum = Convert.ToInt32(frm1.textBox12.Text);
            Class1 yy = new Class1();

            thread = new Thread(() => { StartWork(); });
            thread.IsBackground = true;
            thread.Start();

            BackgroundImage = Image.FromStream(yy.ReturnStream());
            BackgroundImageLayout = ImageLayout.Stretch;

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (progressBar1.Value == progressBar1.Minimum || progressBar1.Value == progressBar1.Maximum)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }
    }
}
