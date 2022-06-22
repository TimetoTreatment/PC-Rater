using System;
using System.Windows.Forms;

namespace EmperorPC
{
    public partial class NewPart : Form
    {
        public DataGridViewRow CurrentDataGridViewRow { get; set; } = null;

        public bool RandomID { get; set; } = false;

        public NewPart(string type)
        {
            InitializeComponent();

            switch (type)
            {
                case "CPU":
                    CurrentDataGridViewRow = dataGridViewCPU.Rows[0];
                    tabControl.SelectedIndex = 0;
                    tabControl.TabPages.RemoveAt(3);
                    tabControl.TabPages.RemoveAt(2);
                    tabControl.TabPages.RemoveAt(1);
                    break;

                case "VGA":
                    CurrentDataGridViewRow = dataGridViewVGA.Rows[0];
                    tabControl.SelectedIndex = 1;
                    tabControl.TabPages.RemoveAt(3);
                    tabControl.TabPages.RemoveAt(2);
                    tabControl.TabPages.RemoveAt(0);
                    break;

                case "RAM":
                    CurrentDataGridViewRow = dataGridViewRAM.Rows[0];
                    tabControl.SelectedIndex = 2;
                    tabControl.TabPages.RemoveAt(3);
                    tabControl.TabPages.RemoveAt(1);
                    tabControl.TabPages.RemoveAt(0);
                    break;

                case "SSD":
                    CurrentDataGridViewRow = dataGridViewSSD.Rows[0];
                    tabControl.SelectedIndex = 3;
                    tabControl.TabPages.RemoveAt(2);
                    tabControl.TabPages.RemoveAt(1);
                    tabControl.TabPages.RemoveAt(0);
                    break;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                if (CurrentDataGridViewRow.Cells[i].Value == null || CurrentDataGridViewRow.Cells[i].Value.ToString().Length <= 0)
                {
                    if (i == 0 && checkBox1.Checked == true)
                        continue;

                    MessageBox.Show("All fields must be filled.");
                    return;
                }
            }

            if (CurrentDataGridViewRow.Cells[0].Value == null || CurrentDataGridViewRow.Cells[0].Value.ToString().Length <= 0)
                RandomID = true;

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                RandomID = true;
            else
                RandomID = false;
        }
    }
}
