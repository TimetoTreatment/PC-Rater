using System;
using System.Windows.Forms;

namespace EmperorPC
{
    public partial class NewComputer : Form
    {
        private readonly DataManager dataManager = DataManager.Instance;

        public int? RowCPU { get; set; } = null;

        public int? RowVGA { get; set; } = null;

        public int? RowRAM { get; set; } = null;

        public int? RowSSD { get; set; } = null;

        public NewComputer()
        {
            InitializeComponent();

            foreach (var item in dataManager.ListCPU)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewCPU.Rows[0].Clone();

                row.Cells[0].Value = item.ID;
                row.Cells[1].Value = item.Manufacturer;
                row.Cells[2].Value = item.Name;
                row.Cells[3].Value = item.Price;
                row.Cells[4].Value = item.CoreCount;
                row.Cells[5].Value = item.Frequency;

                dataGridViewCPU.Rows.Add(row);
            }

            foreach (var item in dataManager.ListVGA)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewVGA.Rows[0].Clone();

                row.Cells[0].Value = item.ID;
                row.Cells[1].Value = item.Manufacturer;
                row.Cells[2].Value = item.Name;
                row.Cells[3].Value = item.Price;
                row.Cells[4].Value = item.StreamProcessor;
                row.Cells[5].Value = item.VRAM;

                dataGridViewVGA.Rows.Add(row);
            }

            foreach (var item in dataManager.ListRAM)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewRAM.Rows[0].Clone();

                row.Cells[0].Value = item.ID;
                row.Cells[1].Value = item.Manufacturer;
                row.Cells[2].Value = item.Name;
                row.Cells[3].Value = item.Price;
                row.Cells[4].Value = item.Capacity;
                row.Cells[5].Value = item.Frequency;

                dataGridViewRAM.Rows.Add(row);
            }

            foreach (var item in dataManager.ListSSD)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewSSD.Rows[0].Clone();

                row.Cells[0].Value = item.ID;
                row.Cells[1].Value = item.Manufacturer;
                row.Cells[2].Value = item.Name;
                row.Cells[3].Value = item.Price;
                row.Cells[4].Value = item.Capacity;
                row.Cells[5].Value = item.Performance;

                dataGridViewSSD.Rows.Add(row);
            }
        }

        private void dataGridViewCPU_SelectionChanged(object sender, EventArgs e)
        {
            RowCPU = dataGridViewCPU.CurrentRow.Index;

            if (RowCPU != null && (checkBoxNoVGA.Checked || RowVGA != null) && RowRAM != null && RowSSD != null)
                buttonOK.Enabled = true;
        }

        private void dataGridViewVGA_SelectionChanged(object sender, EventArgs e)
        {
            RowVGA = dataGridViewVGA.CurrentRow.Index;
            checkBoxNoVGA.Checked = false;

            if (RowCPU != null && (checkBoxNoVGA.Checked || RowVGA != null) && RowRAM != null && RowSSD != null)
                buttonOK.Enabled = true;
        }

        private void dataGridViewRAM_SelectionChanged(object sender, EventArgs e)
        {
            RowRAM = dataGridViewRAM.CurrentRow.Index;

            if (RowCPU != null && (checkBoxNoVGA.Checked || RowVGA != null) && RowRAM != null && RowSSD != null)
                buttonOK.Enabled = true;
        }

        private void dataGridViewSSD_SelectionChanged(object sender, EventArgs e)
        {
            RowSSD = dataGridViewSSD.CurrentRow.Index;

            if (RowCPU != null && (checkBoxNoVGA.Checked || RowVGA != null) && RowRAM != null && RowSSD != null)
                buttonOK.Enabled = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNoVGA.Checked == true)
                RowVGA = null;

            if (RowCPU != null && (checkBoxNoVGA.Checked || RowVGA != null) && RowRAM != null && RowSSD != null)
                buttonOK.Enabled = true;
        }
    }
}
