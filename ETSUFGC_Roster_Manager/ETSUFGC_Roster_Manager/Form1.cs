using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ETSUFGC_Roster_Manager
{

    public partial class Form1 : Form
    {
        public string newFileName;
        public string subjectFile;
        public DataColumn col;
        public DataRow row;
        DataGridViewCheckBoxCell cell = new DataGridViewCheckBoxCell();

        public Form1()
        {
            InitializeComponent();
        }
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(36/2, 36/2, 372/2, 13/2);
            textBox.SetBounds(36/2, 86/2, 700/2, 20/2);
            buttonOk.SetBounds(228/2, 160/2, 160/2, 60/2);
            buttonCancel.SetBounds(400 / 2, 160 / 2, 160 / 2, 60 / 2);
            label.AutoSize = true;
            form.ClientSize = new Size(796/2, 307/2);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;

            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Open file dialog, allows you to select a csv file
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "CSV|*.csv", InitialDirectory = $"../ETSUFGC_Roster_Manager" })
                {

                    if (ofd.ShowDialog() == DialogResult.OK)
                    {

                        dataGridView1.Rows.Clear();
                        ActiveForm.Text = ofd.FileName;
                        subjectFile = ofd.FileName;
                        string[] data = File.ReadAllLines(ofd.FileName);
                        for (int i = 0; i < data.Length; i++)
                        {
                            string[] temp = data[i].Split(',');
                            if (temp[1] == "P")
                            {
                                dataGridView1.Rows.Add(temp[0], cell.Value = true) ;
                            }
                            else
                                dataGridView1.Rows.Add(temp[0], cell.Selected = false);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string value = "";
            int numRows;
            if (InputBox("Name the file", "Name the file", ref value) == DialogResult.OK)
            {
                newFileName = value;
                ActiveForm.Text = newFileName;
            }
            else
            {
                return;
            }
            if (InputBox("How many Rows?", "Rows", ref value) == DialogResult.OK)
            {
                numRows = Int32.Parse(value);
                for (int i = 0; i < numRows; i++)
                {
                    dataGridView1.Rows.Add(" ", cell.Selected = false);
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog() {InitialDirectory = $"../ETSUFGC_Roster_Manager" };
            if(save.ShowDialog() == DialogResult.OK)
            {
                string path = save.FileName + ".csv";
                ActiveForm.Text = save.FileName;
                using (StreamWriter bw = new StreamWriter(File.Open(path, FileMode.Create)))
                {
                    foreach (DataGridViewRow dgvR in dataGridView1.Rows)
                    {
                        for (int j = 0; j < dataGridView1.Columns.Count; ++j)
                        {
                            object val = dgvR.Cells[j].Value;
                            if (val == null)
                            {
                            }
                            else
                            {
                                if (j % 2 == 0)
                                    bw.Write(val.ToString() + ",");
                                else
                                if (val.ToString() == "True")
                                {
                                    bw.WriteLine("P");
                                }
                                else
                                    bw.WriteLine("A");
                            }
                        }
                    }
                }
            }

        }
    }
}