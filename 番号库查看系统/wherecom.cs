using Bgimage;
using INIfiles;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace 番号库查看系统
{
    public partial class wherecom : Form
    {
        [DllImport("user32", EntryPoint = "HideCaret")]
        //禁止焦点
        private static extern bool HideCaret(IntPtr hWnd);
        IniFiles ini = new IniFiles(Application.StartupPath + @"\config.ini");
        public wherecom(Form1 form1)
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            Class1 yy = new Class1();
            BackgroundImage = Image.FromStream(yy.ReturnStream());

            //BackgroundImageLayout = ImageLayout.Stretch;
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

        private void wherecom_MouseDown(object sender, MouseEventArgs e)
        {
            HideCaret(this.textBox1.Handle);
        }

        private void wherecom_Load(object sender, EventArgs e)
        {
            if (ini.ExistINIFile())
            {
                if(ini.IniReadValue("Check", "Default")=="false")
                {
                    checkBox1.Checked = false;
                }
                else if (ini.IniReadValue("Check", "Default") == "true")
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    ;
                }
                 
            }
            else
            {
                ;
            }
        }

        private void wherecom_MouseUp(object sender, MouseEventArgs e)
        {
            HideCaret(this.textBox1.Handle);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            HideCaret(this.textBox1.Handle);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                ini.IniWriteValue("Check", "Default", "true");
            }
            else
            {
                ini.IniWriteValue("Check", "Default", "false");

            }
        }
    }
}
