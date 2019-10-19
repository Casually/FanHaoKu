using HtmlAgilityPack;
using INIfiles;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace 番号库查看系统
{
    public partial class DownloadImageWindow : Form
    {
        public Form1 frm1;
        public Thread threadurl;
        public string strold = string.Empty;
        public string strnew = string.Empty;
        public string str1 = string.Empty;
        public string stradd0 = string.Empty;
        IniFiles ini = new IniFiles(Application.StartupPath + @"\config.ini");
        public DownloadImageWindow(Form1 form1)
        {
            InitializeComponent();
            frm1 = form1;
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        public string GetNumber(string p_str)
        {
            string strReturn = string.Empty;

            if (p_str == null || p_str.Trim() == "")
            {
                strReturn = "";

            }

            foreach (char chrTemp in p_str)
            {
                if (char.IsNumber(chrTemp))
                {
                    strReturn += chrTemp.ToString();
                }

            }
            return strReturn;
        }

        public string GetChar(string p_str)
        {
            string strReturn = string.Empty;
            if (p_str == null || p_str.Trim() == "")
            {
                strReturn = "";

            }

            foreach (char chrTemp in p_str)
            {
                if (!char.IsNumber(chrTemp))
                {
                    strReturn += chrTemp.ToString();
                }
            }
            return strReturn;
        }
        public void ImgSave(string aValueimg)
        {
            string pathnameimg = Application.StartupPath.ToString() + "\\封面\\";
            WebRequest imgRequest = WebRequest.Create(aValueimg);
            
            this.textBox1.AppendText("正在请求WebRequest..........\r\n");
            HttpWebResponse res;
            
            try
            {
             
                res = (HttpWebResponse)imgRequest.GetResponse();
                this.textBox1.AppendText("请求WebRequest成功..........\r\n");
            }
            catch (WebException ex)
            {

                res = (HttpWebResponse)ex.Response;
                this.textBox1.AppendText("请求WebRequest失败..........\r\n");
                this.textBox1.AppendText("响应值"+ res+ "..........\r\n");

            }

            if (res.StatusCode.ToString() == "OK")
            {
                this.textBox1.AppendText("响应状态OK..........\r\n");
                this.textBox1.AppendText("正在获取图片流信息..........\r\n");
                Image downImage = Image.FromStream(imgRequest.GetResponse().GetResponseStream());

                this.textBox1.AppendText("获取图片流信息成功..........\r\n");
                string deerory = string.Format(pathnameimg);

                string fileName = string.Format("{0}.jpg", frm1.番号TextBox.Text);

                downImage.Save(deerory + fileName);
                this.textBox1.AppendText("保存图片至本地..........\r\n");
                downImage.Dispose();
                this.textBox1.AppendText("释放资源..........\r\n");
            }
            this.textBox1.AppendText(frm1.番号TextBox.Text + ".jpg" + " 下载完成..........\r\n");
            
            using (Image jpgtopng = new Bitmap(pathnameimg + frm1.番号TextBox.Text + ".jpg"))
            {
                this.textBox1.AppendText("正在进行图片格式转换..........\r\n");
                jpgtopng.Save(pathnameimg + frm1.封面TextBox.Text, ImageFormat.Png);
            }
            this.textBox1.AppendText(frm1.封面TextBox.Text + " 格式转换完成..........\r\n");
          
            File.Delete(pathnameimg + frm1.番号TextBox.Text + ".jpg");
            this.textBox1.AppendText("删除"+ frm1.番号TextBox.Text +".jpg" +" 成功..........\r\n");

            

            for (int i = 5; i > 0; i--)
            {
                this.textBox1.AppendText(i+" 秒钟后自动关闭下载窗口..........\r\n");
                this.Text = "DownloadImageWindow 倒计时 " + i;
                Thread.Sleep(1000);
            }
            this.Close(); ;
            
        }


        void StartWorkurl()
        {
              
            strold = frm1.番号TextBox.Text;
            strnew = strold.Replace("-", "");
            str1 = GetChar(strnew).ToLower() + GetNumber(strnew);
            string url = ini.IniReadValue("URL", "Default") + str1;
            this.textBox1.AppendText("文件名 " + url + " 获取成功..........\r\n");
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            HtmlWeb web = new HtmlWeb();
            
            //从url中加载
            HtmlAgilityPack.HtmlDocument doc = web.Load(url);
            this.textBox1.AppendText("加载 url 成功..........\r\n");
            //获得title标签节点，其子标签下的所有节点也在其中
            HtmlNode headNode = doc.DocumentNode.SelectSingleNode("//body");
            this.textBox1.AppendText("解析 body 成功..........\r\n");
            //获得id选择器为u1标签（是u1非ul（L）)节点
            HtmlNode aNode = doc.DocumentNode.SelectSingleNode("//a[@class='movie-box']");
            this.textBox1.AppendText("解析 movie-box 成功..........\r\n");
            //获得ul标签下的所有子节点
            // HtmlNodeCollection aCollection = aNode.ChildNodes;
            string aValue = aNode.Attributes["href"].Value;
            this.textBox1.AppendText("解析 href 成功..........\r\n");
            //获得标签内的内容
            //MessageBox.Show("属性值：" + aValue);
            this.textBox1.AppendText("第一层解析完毕  " + aValue + "\r\n");

            string imgurl = aValue;
            HtmlWeb webimg = new HtmlWeb();

            //从url中加载
            HtmlAgilityPack.HtmlDocument docimg = webimg.Load(imgurl);
            this.textBox1.AppendText("加载第二层 url 成功..........\r\n");
            //获得title标签节点，其子标签下的所有节点也在其中
            HtmlNode headNodeimg = docimg.DocumentNode.SelectSingleNode("//body");
            this.textBox1.AppendText("解析第二层 body 成功..........\r\n");
            //获得id选择器为u1标签（是u1非ul（L）)节点
            HtmlNode aNodeimg = docimg.DocumentNode.SelectSingleNode("//a[@class='bigImage']");
            this.textBox1.AppendText("解析第二层 movie-box 成功..........\r\n");
            //获得ul标签下的所有子节点
            // HtmlNodeCollection aCollection = aNode.ChildNodes;
            string aValueimg = aNodeimg.Attributes["href"].Value;
            this.textBox1.AppendText("解析第二层 href 成功..........\r\n");
            //MessageBox.Show("第二层解析完毕  " + aValueimg);
            this.textBox1.AppendText("第二层解析完毕  " + aValueimg + "\r\n");
            this.textBox1.AppendText("开始获取第二层大图..........\r\n");
            ImgSave(aValueimg);
            
            
        }
        private void DownloadImageWindow_Shown(object sender, System.EventArgs e)
        {

            this.textBox1.AppendText("开始下载..........\r\n");
            frm1.button3.Enabled = false;
            this.textBox1.AppendText("禁用Button..........\r\n");
            this.textBox1.AppendText("开启下载线程..........\r\n");
            threadurl = new Thread(() => { StartWorkurl(); });
            threadurl.IsBackground = true;
            threadurl.Start();
        }

        private void DownloadImageWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            frm1.button3.Enabled = true;

            int index = frm1.dataGridView1.CurrentRow.Index;    //取得选中行的索引


            frm1.flush_view();
            frm1.dataGridView1.Rows[index].Selected = true;
            frm1.dataGridView1.CurrentCell = frm1.dataGridView1.Rows[index].Cells[0];

        }

        private void DownloadImageWindow_Load(object sender, System.EventArgs e)
        {
            if (ini.IniReadValue("URL", "Default") == "")
            {

                MessageBox.Show(this, "配置文件错误", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Dispose();
                this.Close();
                frm1.Dispose();
                frm1.Close();
            }
            else
            {
                ;
            }
        }
    }
}
