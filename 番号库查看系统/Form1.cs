using Bgimage;
using INIfiles;
using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using 生成;

namespace 番号库查看系统
{
    public partial class Form1 : Form
    {

        public string pathname = string.Empty;
        public string path1 = string.Empty;
        public string path2 = string.Empty;
        public Thread thread1;
        public Thread thread;
        System.Windows.Forms.Timer time1;
        ToolTip tip = new ToolTip();
        IniFiles ini = new IniFiles(Application.StartupPath + @"\config.ini");

        [DllImport("user32", EntryPoint = "HideCaret")]
        //禁止焦点
        private static extern bool HideCaret(IntPtr hWnd);
        [DllImport("user32.dll")]
        protected static extern bool AnimateWindow(IntPtr hWnd, int dwTime, int dwFlags);

        //dwflag的取值如下
        public const int AW_HOR_POSITIVE = 0x00000001;        //从左到右显示
        public const int AW_HOR_NEGATIVE = 0x00000002;        //从右到左显示
        public const int AW_VER_POSITIVE = 0x00000004;        //从上到下显示
        public const int AW_VER_NEGATIVE = 0x00000008;        //从下到上显示

        //若使用了AW_HIDE标志，则使窗口向内重叠，即收缩窗口；否则使窗口向外扩展，即展开窗口
        public const int AW_CENTER = 0x00000010;
        public const int AW_HIDE = 0x00010000;        //隐藏窗口，缺省则显示窗口
        public const int AW_ACTIVATE = 0x00020000;        //激活窗口。在使用了AW_HIDE标志后不能使用这个标志

        //使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        public const int AW_SLIDE = 0x00040000;
        public const int AW_BLEND = 0x00080000;        //透明度从高到低
        public Form1()
        {
            InitializeComponent();
            time1 = new System.Windows.Forms.Timer() { Interval = 5 };
            time1.Tick += new EventHandler(timer1_Tick);
            base.Opacity = 0;
            time1.Enabled = true;

            Control.CheckForIllegalCrossThreadCalls = false;
            this.番号库TableAdapter.Fill(this.本地番号数据库DataSet.番号库);
        }

        private void 番号库BindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.番号库BindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.本地番号数据库DataSet);
        }

        void StartWork1()
        {        
            Class1 yy = new Class1();
            BackgroundImage = Image.FromStream(yy.ReturnStream());
            BackgroundImageLayout = ImageLayout.Stretch;
        }
        void StartWork()
        {
            Loadpict();
            FileCount();
            FileCount1();
            space();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            g_formHeight = this.Height;
            g_formWidth = this.Width;
            WritenIn_Tags(this);
            // TODO: 这行代码将数据加载到表“本地番号数据库DataSet.演员详情”中。您可以根据需要移动或删除它。
            this.演员详情TableAdapter.Fill(this.本地番号数据库DataSet.演员详情);
            thread1 = new Thread(() => { StartWork1(); });
            thread = new Thread(() => { StartWork(); });
            thread1.IsBackground = true;
            thread.IsBackground = true;
            thread1.Start();          
            thread.Start();

        }

        private void 序号TextBox_TextChanged(object sender, EventArgs e)
        {
            thread = new Thread(() => { StartWork(); });
            thread.IsBackground = true;
            thread.Start();
        }

        private void Loadpict()
        {
            if (封面TextBox.Text != string.Empty)
            {
                pathname = Application.StartupPath.ToString() + "\\封面\\" + 封面TextBox.Text;
                if (File.Exists(pathname))
                {
                    pictureBox1.LoadAsync(pathname);
                }
                else
                {
                    Class1 yy = new Class1();
                    pictureBox1.Image = Image.FromStream(yy.ReturnStream());
                }

            }
            else
            {
                ;//MessageBox.Show("封面不存在");
            }
        }

        private void 打开文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            path1 = Application.StartupPath.ToString() + "\\视频\\" + 本地链接TextBox.Text;
            if (本地链接TextBox.Text != string.Empty)
            {
                if (File.Exists(path1))
                {
                    System.Diagnostics.Process.Start(Application.StartupPath.ToString() + "\\视频\\" + 本地链接TextBox.Text);

                }
                else
                {
                    MessageBox.Show(本地链接TextBox.Text ,"文件不存在");

                }

            }
        }

        private void 打开文件所在位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            path1 = Application.StartupPath.ToString() + "\\视频\\" + 本地链接TextBox.Text;
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo("Explorer.exe")
            {
                Arguments = "/e,/select," + path1
            };
            System.Diagnostics.Process.Start(psi);
        }

        public void flush_view()
        {
            textBox1.Text = "";
            dataGridView1.DataSource = 番号库BindingSource;

            本地链接TextBox.DataBindings.Clear();
            类别TextBox.DataBindings.Clear();
            影片长度_分钟_TextBox.DataBindings.Clear();
            发行日期DateTimePicker.DataBindings.Clear();
            番号名TextBox.DataBindings.Clear();
            番号TextBox.DataBindings.Clear();
            封面TextBox.DataBindings.Clear();
            演员名称TextBox.DataBindings.Clear();
            序号TextBox.DataBindings.Clear();

            this.番号TextBox.DataBindings.Add(new Binding("Text", this.番号库BindingSource, "番号", true));
            this.演员名称TextBox.DataBindings.Add(new Binding("Text", this.番号库BindingSource, "演员名称", true));
            this.番号名TextBox.DataBindings.Add(new Binding("Text", this.番号库BindingSource, "番号名", true));
            this.发行日期DateTimePicker.DataBindings.Add(new Binding("Value", this.番号库BindingSource, "发行日期", true));
            this.影片长度_分钟_TextBox.DataBindings.Add(new Binding("Text", this.番号库BindingSource, "影片长度(分钟)", true));
            this.封面TextBox.DataBindings.Add(new Binding("Text", this.番号库BindingSource, "封面", true));
            this.本地链接TextBox.DataBindings.Add(new Binding("Text", this.番号库BindingSource, "本地链接", true));
            this.类别TextBox.DataBindings.Add(new Binding("Text", this.番号库BindingSource, "类别", true));
            this.序号TextBox.DataBindings.Add(new Binding("Text", this.番号库BindingSource, "序号", true));

            番号库BindingNavigator.BindingSource = 番号库BindingSource;
            this.番号库TableAdapter.Fill(this.本地番号数据库DataSet.番号库);
            dataGridView1.Refresh();
            this.tableAdapterManager.UpdateAll(this.本地番号数据库DataSet);
        }
        public void button1_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;    //取得选中行的索引


            flush_view();
            dataGridView1.Rows[index].Selected = true;
            dataGridView1.CurrentCell = dataGridView1.Rows[index].Cells[0];
        }

        public void space()
        {
            path2 = Application.StartupPath.ToString() + "\\视频\\" + 本地链接TextBox.Text;
            if (File.Exists(path2))
            {
                FileInfo f = new FileInfo(path2);
                float f1 = Convert.ToSingle(f.Length.ToString());
                float F1 = f1 / 1024 / 1024 / 1024;
                textBox13.Text = F1.ToString("0.00" + "  GB");          
            }
            else
            {
                textBox13.Text = "无文件";
            }
        }
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;
        //        return cp;
        //    }
        //}
        public void FileCount1()
        {
            int FileCount1 = 0;
            if (Directory.Exists(Application.StartupPath.ToString() + "\\封面\\"))
            {
                DirectoryInfo Dir = new DirectoryInfo(Application.StartupPath.ToString() + "\\封面\\");
                foreach (FileInfo FI1 in Dir.GetFiles())
                {
                    //这里写文件格式
                    if (Path.GetExtension(FI1.Name) == ".png")
                    {
                        FileCount1++;
                    }
                }
                textBox12.Text = Convert.ToString(FileCount1);
            }
            else
            {
                Directory.CreateDirectory(Application.StartupPath.ToString() + "\\封面\\");
                textBox12.Text = "0";
            }
        }

        public void FileCount()
        {
            int FileCount = 0;
            
            // 这里写你的目录

            if (Directory.Exists(Application.StartupPath.ToString() + "\\视频\\"))
            {
                DirectoryInfo Dir = new DirectoryInfo(Application.StartupPath.ToString() + "\\视频\\");
                foreach (FileInfo FI in Dir.GetFiles())
                {
                    //这里写文件格式
                    if (Path.GetExtension(FI.Name) == ".mp4" ||
                        Path.GetExtension(FI.Name) == ".mkv" ||
                        Path.GetExtension(FI.Name) == ".wmv" ||
                        Path.GetExtension(FI.Name) == ".avi" ||
                        Path.GetExtension(FI.Name) == ".iso")
                    {
                        FileCount++;
                    }
                }
                textBox11.Text = Convert.ToString(FileCount);
            }
            else
            {
                Directory.CreateDirectory(Application.StartupPath.ToString() + "\\视频\\");
                textBox11.Text = "0";
            }

        }
        private delegate void myDelegate();//声明委托
        //private Thread thread4;
        private void ThreadProcSafe()
        {
            setRich();
        }
        private void setRich()
        {
            
            //lbLog 控件名
            if (this.InvokeRequired)
            {
                myDelegate md = new myDelegate(setRich);
                this.Invoke(md, new object[] { });
            }
            else
            {
                Form2 f2 = new Form2(this);
                f2.ShowDialog();
            }
               
        }
        private void textBox11_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(ThreadProcSafe));
            thread.Start();
            
        }

        private void textBox11_MouseEnter(object sender, EventArgs e)
        {
            HideCaret(this.textBox11.Handle);
        }

        private void textBox11_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret(this.textBox11.Handle);
        }

        private void textBox11_MouseUp(object sender, MouseEventArgs e)
        {
            HideCaret(this.textBox11.Handle);
        }

        private void textBox12_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret(this.textBox12.Handle);
        }

        private void textBox12_MouseEnter(object sender, EventArgs e)
        {
            HideCaret(this.textBox12.Handle);
        }

        private void textBox12_MouseUp(object sender, MouseEventArgs e)
        {
            HideCaret(this.textBox12.Handle);
        }


        private void textBox12_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(this);
            form3.ShowDialog();
        }

        private void textBox12_MouseHover(object sender, EventArgs e)
        {
            tip.Show("点击查看封面文件列表", textBox11, 0);
        }

        private void textBox11_MouseLeave(object sender, EventArgs e)
        {
            tip.Hide(textBox11);
        }

        private void textBox11_MouseHover_1(object sender, EventArgs e)
        {
            tip.Show("显示一键生成组件", textBox11, 0);
        }

        private void textBox12_MouseLeave(object sender, EventArgs e)
        {
            tip.Hide(textBox12);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体显示    
                WindowState = FormWindowState.Normal;
                //激活窗体并给予它焦点
                this.Activate();
                //任务栏区显示图标
                this.ShowInTaskbar = true;
                //托盘区图标隐藏
                notifyIcon1.Visible = false;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //判断是否选择的是最小化按钮
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                this.ShowInTaskbar = false;
                //图标显示在托盘区
                notifyIcon1.Visible = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                AnimateWindow(this.Handle, 2000, AW_CENTER | AW_BLEND | AW_HIDE);
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
            this.Activate();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("是否确认退出程序？", "退出", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                // 关闭所有的线程
                this.Dispose();
                this.Close();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            wherecom wherecomm = new wherecom(this);
            if (this.Opacity >= 1)
            {
                time1.Enabled = false;
                
                if (ini.ExistINIFile())
                {
                    if (ini.IniReadValue("Check", "Default") == "false")
                    {
                        
                        wherecomm.ShowDialog();
                    }        
                    else if(ini.IniReadValue("Check", "Default") == "true")
                    {
                        ;
                    }
                    else
                    {
                        MessageBox.Show("配置文件错误", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Dispose();
                        this.Close();
                    }

                }
                else
                {
                    ini.IniWriteValue("Check", "Default", "false");
                    ini.IniWriteValue("URL", "Default", "https://avmask.com/cn/search/");
                    wherecomm.ShowDialog();
                }
                             
            }
            else
            {
                Opacity += 0.02;
                
            }

        }

        private void textBox13_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret(this.textBox13.Handle);
        }

        private void textBox13_MouseEnter(object sender, EventArgs e)
        {
            HideCaret(this.textBox13.Handle);
        }

        private void textBox13_MouseUp(object sender, MouseEventArgs e)
        {
            HideCaret(this.textBox13.Handle);
        }

        private void textBox13_MouseLeave(object sender, EventArgs e)
        {
            tip.Hide(textBox13);
        }

        private void textBox13_MouseHover(object sender, EventArgs e)
        {
            tip.Show("显示MD5校验组件", textBox13, 0);
        }


        private void 西野翔toolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.番号库TableAdapter.西野翔(this.本地番号数据库DataSet.番号库);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 椎名空ToolStripButton_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.番号库TableAdapter.椎名空(this.本地番号数据库DataSet.番号库);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void 水野朝阳toolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.番号库TableAdapter.水野朝阳(this.本地番号数据库DataSet.番号库);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 松下纱荣子toolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.番号库TableAdapter.松下纱荣子(this.本地番号数据库DataSet.番号库);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 饭冈加奈子toolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.番号库TableAdapter.饭冈佳奈子(this.本地番号数据库DataSet.番号库);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 八乃翼toolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.番号库TableAdapter.八乃翼(this.本地番号数据库DataSet.番号库);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 天使萌toolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.番号库TableAdapter.天使萌(this.本地番号数据库DataSet.番号库);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                flush_view();
            }
            else
            {

                using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\\本地番号数据库.accdb"))
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = string.Format("select * from 番号库 where 演员名称 like '" + "%"+textBox1.Text+"%" + "'OR 序号 like '" + "%" + textBox1.Text + "%" + "'OR 类别 like '" + "%" + textBox1.Text + "%" + "'OR [影片长度(分钟)] like '" + "%" + textBox1.Text + "%" + "'OR 发行日期 like '" + "%" + textBox1.Text + "%" + "'");//要显示的数据

                        int rows = cmd.ExecuteNonQuery();
                        OleDbDataAdapter da = new OleDbDataAdapter(cmd.CommandText, con);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        //con.Close();
                        DataTable dtbl = ds.Tables[0];
                        dataGridView1.AutoGenerateColumns = false;
                        this.dataGridView1.DataSource = dtbl;

                        //将DatagridView的数据通过BindingSource与BindingNavigator连接起来
                        BindingSource bs = new BindingSource();
                        
                        bs.DataSource = dtbl;
                        番号库BindingNavigator.BindingSource = bs;
                        dataGridView1.DataSource = bs;

                        序号TextBox.DataBindings.Clear();
                        序号TextBox.DataBindings.Add(new Binding("Text", bs, "序号", true));
                        演员名称TextBox.DataBindings.Clear();
                        演员名称TextBox.DataBindings.Add(new Binding("Text", bs, "演员名称", true));
                        封面TextBox.DataBindings.Clear();
                        封面TextBox.DataBindings.Add(new Binding("Text", bs, "封面", true));
                        番号TextBox.DataBindings.Clear();
                        番号TextBox.DataBindings.Add(new Binding("Text", bs, "番号", true));
                        番号名TextBox.DataBindings.Clear();
                        番号名TextBox.DataBindings.Add(new Binding("Text", bs, "番号名", true));
                        发行日期DateTimePicker.DataBindings.Clear();
                        发行日期DateTimePicker.DataBindings.Add(new Binding("Text", bs, "发行日期", true));
                        影片长度_分钟_TextBox.DataBindings.Clear();
                        影片长度_分钟_TextBox.DataBindings.Add(new Binding("Text", bs, "影片长度(分钟)", true));
                        类别TextBox.DataBindings.Clear();
                        类别TextBox.DataBindings.Add(new Binding("Text", bs, "类别", true));
                        本地链接TextBox.DataBindings.Clear();
                        本地链接TextBox.DataBindings.Add(new Binding("Text", bs, "本地链接", true));

                        if(番号库BindingNavigator.PositionItem.Text=="0")
                        {
                            pictureBox1.Image = null;
                            序号TextBox.Text = "";
                            演员名称TextBox.Text = "";
                            封面TextBox.Text = "";
                            番号TextBox.Text = "";
                            番号名TextBox.Text = "";
                            发行日期DateTimePicker.Text = "";
                            影片长度_分钟_TextBox.Text = "";
                            类别TextBox.Text = "";
                            本地链接TextBox.Text = "";

                            MessageBox.Show("当前无搜索结果", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            flush_view();

                            番号库BindingNavigator.BindingSource = 番号库BindingSource;
                            this.番号库TableAdapter.Fill(this.本地番号数据库DataSet.番号库);
                            dataGridView1.Refresh();
                            this.tableAdapterManager.UpdateAll(this.本地番号数据库DataSet);
                        }
                    }
                }

            }
        }
        private void setRichimg()
        {

            //lbLog 控件名
            if (this.InvokeRequired)
            {
                myDelegate md = new myDelegate(setRichimg);
                this.Invoke(md, new object[] { });
            }
            else
            {
                DownloadImageWindow downloadImageWindow = new DownloadImageWindow(this);
                downloadImageWindow.ShowDialog();
            }

        }
        private void ThreadProcSafeImg()
        {
            setRichimg();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            thread = new Thread(new ThreadStart(ThreadProcSafeImg));
            thread.Start();
        }      

        private void 本地链接TextBox_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4(this);
            f4.ShowDialog();
        }

        private void 本地链接TextBox_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret(this.本地链接TextBox.Handle);
        }

        private void 本地链接TextBox_MouseEnter(object sender, EventArgs e)
        {
            HideCaret(this.本地链接TextBox.Handle);
        }

        private void 本地链接TextBox_MouseHover(object sender, EventArgs e)
        {
            tip.Show("显示内置播放器组件", 本地链接TextBox, 0);
        }

        private void 本地链接TextBox_MouseLeave(object sender, EventArgs e)
        {
            tip.Hide(本地链接TextBox);
        }


        private void textBox11_MouseUp_1(object sender, MouseEventArgs e)
        {
            HideCaret(this.本地链接TextBox.Handle);
        }


        private void 演员详情DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0)
            {
                ;
            }
            else
            {
                Form5 f5 = new Form5(this);
                f5.ShowDialog();

            }
            
        }
        void Resize_AllControls(Control cons, double scaleX, double scaleY)
        {
            foreach (Control con in cons.Controls)
            {
                var tags = con.Tag.ToString().Split(new char[] { '?' });
                int widthOld = Convert.ToInt32(tags[0]);
                int heightOld = Convert.ToInt32(tags[1]);
                int leftOld = Convert.ToInt32(tags[2]);
                int topOld = Convert.ToInt32(tags[3]);
                int fontSizeOld = Convert.ToInt32(tags[4]);

                con.Width = Convert.ToInt32(widthOld * scaleX);
                con.Height = Convert.ToInt32(heightOld * scaleY);
                con.Left = Convert.ToInt32(leftOld * scaleY);
                con.Top = Convert.ToInt32(topOld * scaleY);
                int fontsizeNew = Convert.ToInt32(fontSizeOld * scaleX);
                con.Font = new System.Drawing.Font(con.Font.Name, fontsizeNew, Font.Style);
                if (con.Controls.Count > 0)
                {
                    Resize_AllControls(con, scaleX, scaleY);
                }
            }
        }
        int g_formWidth, g_formHeight; 

        private void Form1_Resize(object sender, EventArgs e)
        {
            Form fm = (Form)sender;
            double scaleX = fm.Width * 1.0 / g_formWidth;
            double scaleY = fm.Height * 1.0 / g_formHeight;
            Resize_AllControls(fm, scaleX, scaleY);
        }

        void WritenIn_Tags(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                string str = con.Width.ToString() + "?"
                    + con.Height.ToString() + "?"
                    + con.Left.ToString() + "?"
                    + con.Top.ToString() + "?"
                    + con.Font.Size.ToString();
                con.Tag = str;
                if (con.Controls.Count > 0)
                {
                    WritenIn_Tags(con);
                }
            }
        }
    }
    

}
