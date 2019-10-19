using Bgimage;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace 番号库查看系统
{
    public partial class Form5 : Form
    {
        public Form1 frm1;
        public Form5(Form1 form1)
        {
            InitializeComponent();
            frm1 = form1;
        }

        private void Form5_Load(object sender, System.EventArgs e)
        {
            textBox2.Text = frm1.演员详情DataGridView.Rows[0].Cells["dataGridViewTextBoxColumn2"].Value.ToString();
            textBox1.Text = frm1.演员详情DataGridView.Rows[0].Cells["ID"].Value.ToString();
            textBox3.Text = frm1.演员详情DataGridView.Rows[0].Cells["日文名"].Value.ToString();
            textBox4.Text = frm1.演员详情DataGridView.Rows[0].Cells["英文名"].Value.ToString();
            textBox5.Text = frm1.演员详情DataGridView.Rows[0].Cells["身高"].Value.ToString();
            textBox6.Text = frm1.演员详情DataGridView.Rows[0].Cells["出生日期"].Value.ToString();
            textBox7.Text = frm1.演员详情DataGridView.Rows[0].Cells["出生地"].Value.ToString();
            textBox8.Text = frm1.演员详情DataGridView.Rows[0].Cells["国籍"].Value.ToString();
            textBox9.Text = frm1.演员详情DataGridView.Rows[0].Cells["出道时间"].Value.ToString();
            textBox10.Text = frm1.演员详情DataGridView.Rows[0].Cells["星座"].Value.ToString();
            textBox11.Text = frm1.演员详情DataGridView.Rows[0].Cells["头像本地路径"].Value.ToString();
            string pathname = Application.StartupPath.ToString() + "\\演员头像\\" + textBox11.Text;
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

    }
}
