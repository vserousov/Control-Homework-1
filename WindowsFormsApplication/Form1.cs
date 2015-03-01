using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathFunctions;
using System.IO;

namespace WindowsFormsApplication
{
    public partial class Form1 : Form
    {
        FunctionManager manager;

        const int numTypes = 3;
        const int defaultIndex = 0;
        const string defaultFirstK = "k =";
        const string defaultSecondK = "b = ";
        const string captionError = "Ошибка!";
        const string messageError = "Заполнены не все поля или заполнены неверно!";
        const string title = "Менеджер математических функций";

        private string currentFile = "./data.txt";

        public Form1()
        {
            InitializeComponent();
            string defaultFile = Path.GetFileName(currentFile);

            manager = new FunctionManager();

            try
            {
                if (!File.Exists(currentFile))
                {
                    manager.CreateFile(currentFile);
                }
                else
                {
                    manager.OpenFile(currentFile);
                }
            }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, captionError);
            }

            this.Text = defaultFile + " - " + title;
            comboBox1.SelectedIndex = defaultIndex;
            label3.Text = defaultFirstK;
            label4.Text = defaultSecondK;
            label5.Text = "";
            textBox3.Visible = false;
            UpdateListView();
        }

        /// <summary>
        /// Очищаем форму
        /// </summary>
        private void resetForm()
        {
            int index = comboBox1.SelectedIndex;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";

            if (index == 0)
            {
                label4.Visible = true;
                label5.Visible = false;
                label3.Text = "k =";
                label4.Text = "b =";
                label5.Text = "";
                textBox2.Visible = true;
                textBox3.Visible = false;
            }

            if (index == 1)
            {
                label4.Visible = true;
                label5.Visible = true;
                label3.Text = "a =";
                label4.Text = "b =";
                label5.Text = "c = ";
                textBox2.Visible = true;
                textBox3.Visible = true;
            }

            if (index == 2)
            {
                label4.Visible = false;
                label5.Visible = false;
                label3.Text = "k =";
                label4.Text = "";
                label5.Text = "";
                textBox2.Visible = false;
                textBox3.Visible = false;
            }
        }


        /// <summary>
        /// Обновляем ListView
        /// </summary>
        private void UpdateListView()
        {
            bool linear = checkBox1.Checked;
            bool parabola = checkBox2.Checked;
            bool hyperbola = checkBox3.Checked;

            int count = manager.MathFunctions.Count;
            
            listView1.Items.Clear();
            
            for (int i = 0; i < count; i++)
            {
                OneArgument function = manager.MathFunctions[i];
                if (linear && function is Linear ||
                    parabola && function is Parabola ||
                    hyperbola && function is Hyperbola)
                {
                    string[] row = { i.ToString(), function.name, function.Show(), function.argument.ToString(), String.Format("{0:f2}", function.value) };
                    listView1.Items.Add(new ListViewItem(row));
                }
            }

            button3.Enabled = listView1.SelectedItems.Count > 0 && listView1.Items.Count > 0;

            //Выравнивание ListView в случае появления скроллбара
            if (listView1.Items.Count > 11)
            {
                columnHeader4.Width = 90;
            }
            else
            {
                columnHeader4.Width = 115;
            }
        }

        /// <summary>
        /// Отключение изменения ширины заголовков
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.Cancel = true;
            e.NewWidth = listView1.Columns[e.ColumnIndex].Width;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetForm();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex;
            int num = manager.getNumParams(index);
            double argument;
            double[] param = new double[num];
            string[] text = new string[numTypes];

            text[0] = textBox1.Text.Replace('.', ',');
            text[1] = textBox2.Text.Replace('.', ',');
            text[2] = textBox3.Text.Replace('.', ',');

            for (int i = 0; i < num; i++)
            {
                if (!double.TryParse(text[i], out param[i]))
                {
                    MessageBox.Show(messageError, captionError, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            string arg = textBox4.Text.Replace('.',',');
            
            if(!double.TryParse(arg, out argument))
            {
                 MessageBox.Show("Вы не ввели аргумент!", captionError, MessageBoxButtons.OK, MessageBoxIcon.Information);
                 return;
            }

            OneArgument function;

            if (index == 0)
            {
                function = new Linear();
            }
            else if (index == 1)
            {
                function = new Parabola();
            }
            else
            {
                function = new Hyperbola();
            }
            
            try
            {
                function.Parameters(param);
                function.Calculate(argument);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, captionError, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            manager.MathFunctions.Add(function);
            UpdateListView();
            resetForm();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            resetForm();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            button3.Enabled = listView1.SelectedItems.Count > 0 && listView1.Items.Count > 0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int id = 0;
            int ord = 0;
            foreach (ListViewItem eachItem in listView1.SelectedItems)
            {
                id = int.Parse(eachItem.SubItems[0].Text);
                ord = eachItem.Index;
                manager.MathFunctions.RemoveAt(id);
                UpdateListView();
            }

            int count = listView1.Items.Count;

            if (count > 0)
            {
                listView1.Items[(count > ord ? ord : ord - 1)].Selected = true;
                listView1.Select();
            }
        }

        private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (listView1.Items.Count == 0)
                return;

            if(e.Column == 4)
            {
                manager.SortByValue();
                UpdateListView();
            } 
/*            else
            {
                MessageBox.Show("Доступна только сортировка по значению!", captionError, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
 */
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListView();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListView();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            UpdateListView();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Создать";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFile = saveFileDialog1.FileName;
                manager.CreateFile(currentFile);
                string defaultFile = Path.GetFileName(currentFile);
                this.Text = defaultFile + " - " + title;
                UpdateListView();
                resetForm();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            manager.SaveFile(currentFile);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Multiselect = false;
            openFileDialog1.Title = "Открыть";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {              
                try
                {
                    manager.OpenFile(openFileDialog1.FileName);
                    currentFile = openFileDialog1.FileName;
                    UpdateListView();
                    string defaultFile = Path.GetFileName(currentFile);
                    this.Text = defaultFile + " - " + title;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, captionError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    manager.OpenFile(currentFile);
                }
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "txt files (*.txt)|*.txt";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Сохранить как...";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFile = saveFileDialog1.FileName;
                manager.SaveFileAs(currentFile);
                string defaultFile = Path.GetFileName(currentFile);
                this.Text = defaultFile + " - " + title;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Программа \"Менеджер математических функций\". \r\nРазработал студент группы 171ПИ Сероусов Виталий.", "О программе",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void newToolStripButton_Click(object sender, EventArgs e)
        {
            newToolStripMenuItem_Click(sender, e);
        }

        private void openToolStripButton_Click(object sender, EventArgs e)
        {
            openToolStripMenuItem_Click(sender, e);
        }

        private void saveToolStripButton_Click(object sender, EventArgs e)
        {
            saveToolStripMenuItem_Click(sender, e);
        }

        private void helpToolStripButton_Click(object sender, EventArgs e)
        {
            aboutToolStripMenuItem_Click(sender, e);
        }
    }
}
