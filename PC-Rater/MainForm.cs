using System;
using System.Drawing;
using System.Windows.Forms;

namespace EmperorPC
{
    public partial class MainForm : Form
    {
        private readonly DataManager dataManager = DataManager.Instance;
        private int prevRow = 0;

        ////////////
        /* 생성자 */
        ////////////
        public MainForm()
        {
            InitializeComponent();

            InitializeTable();

            comboBoxComputerType.SelectedIndex = 0;
            comboBoxPartType.SelectedIndex = 0;
        }

        ///////////////////
        /* 테이블 초기화 */
        ///////////////////
        void InitializeTable()
        {
            foreach (Computer computer in dataManager.ListComputer)
            {
                DataGridViewRow row = (DataGridViewRow)dataGridViewComputer.Rows[0].Clone();

                row.Cells[0].Value = computer.ID;
                row.Cells[1].Value = computer.CPU.Name;
                row.Cells[2].Value = computer.VGAID != 0 ? computer.VGA.Name : "-";
                row.Cells[3].Value = computer.RAM.Name;
                row.Cells[4].Value = computer.RAM.Capacity;
                row.Cells[5].Value = computer.SSD.Name;
                row.Cells[6].Value = computer.SSD.Capacity;
                row.Cells[7].Value = computer.Price;

                dataGridViewComputer.Rows.Add(row);
            }

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

        /////////////////////
        /* 종료시 파일 쓰기*/
        /////////////////////
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dataManager.WriteFile();
        }

        ////////////////////
        /* 현재 상태 출력 */
        ////////////////////
        private void comboBoxComputerType_SelectedValueChanged(object sender, EventArgs e)
        {
            string type = ((ComboBox)sender).Text;

            if (type == "Total" || type == "Office")
                textBoxNumberOfComputer.Text = dataManager.ListComputer.Count.ToString();

            else if (type == "Gaming")
            {
                int count = 0;

                foreach (var computer in dataManager.ListComputer)
                {
                    if (computer.VGA != null)
                        count++;
                }

                textBoxNumberOfComputer.Text = count.ToString();
            }
        }

        ////////////////////
        /* 현재 상태 출력 */
        ////////////////////
        private void comboBoxPartType_SelectedValueChanged(object sender, EventArgs e)
        {
            string type = ((ComboBox)sender).Text;

            int countCPU = dataManager.ListCPU.Count;
            int countVGA = dataManager.ListVGA.Count;
            int countRAM = dataManager.ListRAM.Count;
            int countSSD = dataManager.ListSSD.Count;

            if (type == "Total")
                textBoxNumberOfPart.Text = (countCPU + countVGA + countRAM + countSSD).ToString();
            else if (type == "CPU")
                textBoxNumberOfPart.Text = countCPU.ToString();
            else if (type == "VGA")
                textBoxNumberOfPart.Text = countVGA.ToString();
            else if (type == "RAM")
                textBoxNumberOfPart.Text = countRAM.ToString();
            else if (type == "SSD")
                textBoxNumberOfPart.Text = countSSD.ToString();
        }

        ///////////////////////////////
        /* 목록 선택시 정보 업데이트 */
        ///////////////////////////////
        private void dataGridViewComputer_SelectionChanged(object sender, EventArgs e)
        {
            UpdateResultBox(dataGridViewComputer.CurrentRow.Index);
            prevRow = dataGridViewComputer.CurrentRow.Index;
        }

        /////////////////////////////
        /* 점수 및 적합성 업데이트 */
        /////////////////////////////
        private void UpdateResultBox(int index)
        {
            if (index >= dataManager.ListComputer.Count)
            {
                textBoxOffice.Clear();
                textBoxGaming.Clear();
                textBoxWorkstation.Clear();
                textBoxOverall.Clear();

                return;
            }

            Computer computer = dataManager.ListComputer[index];

            computer.CalculateScore();

            textBoxPhysics.Text = ((int)computer.Score.Physics).ToString();
            textBoxGraphics.Text = ((int)computer.Score.Graphics).ToString();
            textBoxMemory.Text = ((int)computer.Score.Memory).ToString();
            textBoxStorage.Text = ((int)computer.Score.Storage).ToString();

            int officeScore = (int)computer.Score.Office;
            int gamingScore = (int)computer.Score.Gaming;
            int workstationScore = (int)computer.Score.Workstation;
            int overallScore = (int)computer.Score.Overall;

            if (officeScore < 10000)
                textBoxOffice.ForeColor = Color.Red;
            else if (officeScore < 20000)
                textBoxOffice.ForeColor = Color.Green;
            else
                textBoxOffice.ForeColor = Color.Blue;

            if (gamingScore < 20000)
                textBoxGaming.ForeColor = Color.Red;
            else if (gamingScore < 50000)
                textBoxGaming.ForeColor = Color.Green;
            else
                textBoxGaming.ForeColor = Color.Blue;

            if (workstationScore < 15000)
                textBoxWorkstation.ForeColor = Color.Red;
            else if (workstationScore < 30000)
                textBoxWorkstation.ForeColor = Color.Green;
            else
                textBoxWorkstation.ForeColor = Color.Blue;

            if (overallScore < 20000)
                textBoxOverall.ForeColor = Color.Red;
            else if (overallScore < 50000)
                textBoxOverall.ForeColor = Color.Green;
            else
                textBoxOverall.ForeColor = Color.Blue;

            textBoxOffice.Text = officeScore.ToString();
            textBoxGaming.Text = gamingScore.ToString();
            textBoxWorkstation.Text = workstationScore.ToString();
            textBoxOverall.Text = overallScore.ToString();
        }

        ////////////////////////
        /* 새로운 컴퓨터 생성 */
        ////////////////////////
        private void MenuNewComputer_Click(object sender, EventArgs e)
        {
            NewComputer form = new();

            if (form.ShowDialog() != DialogResult.OK)
                return;

            int newID;
            CPU CPU = form.RowCPU.HasValue ? dataManager.ListCPU[form.RowCPU.Value] : null;
            VGA VGA = form.RowVGA.HasValue ? dataManager.ListVGA[form.RowVGA.Value] : null;
            RAM RAM = form.RowRAM.HasValue ? dataManager.ListRAM[form.RowRAM.Value] : null;
            SSD SSD = form.RowSSD.HasValue ? dataManager.ListSSD[form.RowSSD.Value] : null;

            for (Random random = new(); ;)
            {
                bool isUnique = true;

                newID = random.Next(1000, 9999);

                foreach (var item in dataManager.ListComputer)
                {
                    if (item.ID == newID)
                    {
                        isUnique = false;
                        break;
                    }
                }

                if (isUnique == true)
                    break;
            }

            Computer computer = new(newID, CPU, VGA, RAM, SSD);

            dataManager.ListComputer.Add(computer);

            DataGridViewRow row = (DataGridViewRow)dataGridViewComputer.Rows[0].Clone();

            row.Cells[0].Value = computer.ID;
            row.Cells[1].Value = computer.CPU.Name;
            row.Cells[2].Value = computer.VGAID != 0 ? computer.VGA.Name : "-";
            row.Cells[3].Value = computer.RAM.Name;
            row.Cells[4].Value = computer.RAM.Capacity;
            row.Cells[5].Value = computer.SSD.Name;
            row.Cells[6].Value = computer.SSD.Capacity;
            row.Cells[7].Value = computer.Price;

            dataGridViewComputer.Rows.Add(row);
            dataGridViewComputer.Rows[prevRow].Selected = false;
            dataGridViewComputer.Rows[dataManager.ListComputer.Count - 1].Selected = true;

            prevRow = dataManager.ListComputer.Count - 1;

            UpdateResultBox(dataManager.ListComputer.Count - 1);

            statusBar.Text = "Add Computer.";
        }

        //////////////////////
        /* 새로운 부품 생성 */
        //////////////////////
        private void MenuNewCPU_Click(object sender, EventArgs e)
        {
            NewPart form = new("CPU");

            if (form.ShowDialog() != DialogResult.OK)
                return;

            int newID;
            string manufacturer = form.CurrentDataGridViewRow.Cells[1].Value.ToString();
            string name = form.CurrentDataGridViewRow.Cells[2].Value.ToString();
            int price = int.Parse(form.CurrentDataGridViewRow.Cells[3].Value.ToString());
            int coreCount = int.Parse(form.CurrentDataGridViewRow.Cells[4].Value.ToString());
            double frequency = double.Parse(form.CurrentDataGridViewRow.Cells[5].Value.ToString());

            if (form.RandomID)
            {
                for (Random random = new(); ;)
                {
                    bool isUnique = true;

                    newID = random.Next(1000, 9999);

                    foreach (var item in dataManager.ListComputer)
                    {
                        if (item.ID == newID)
                        {
                            isUnique = false;
                            break;
                        }
                    }

                    if (isUnique == true)
                        break;
                }
            }
            else
                newID = int.Parse(form.CurrentDataGridViewRow.Cells[0].Value.ToString());

            DataGridViewRow row = (DataGridViewRow)dataGridViewCPU.Rows[0].Clone();

            row.Cells[0].Value = newID;
            row.Cells[1].Value = manufacturer;
            row.Cells[2].Value = name;
            row.Cells[3].Value = price;
            row.Cells[4].Value = coreCount;
            row.Cells[5].Value = frequency;

            dataManager.ListCPU.Add(new CPU(newID, manufacturer, name, price, coreCount, frequency));

            tabControlPart.SelectedIndex = 0;

            dataGridViewCPU.Rows.Add(row);
            dataGridViewCPU.Rows[dataGridViewCPU.CurrentRow.Index].Selected = false;
            dataGridViewCPU.Rows[dataManager.ListCPU.Count - 1].Selected = true;

            statusBar.Text = "CPU was Successfully added.";
        }

        private void MenuNewVGA_Click(object sender, EventArgs e)
        {
            NewPart form = new("VGA");

            if (form.ShowDialog() != DialogResult.OK)
                return;

            int newID;
            string manufacturer = form.CurrentDataGridViewRow.Cells[1].Value.ToString();
            string name = form.CurrentDataGridViewRow.Cells[2].Value.ToString();
            int price = int.Parse(form.CurrentDataGridViewRow.Cells[3].Value.ToString());
            int streamProcessor = int.Parse(form.CurrentDataGridViewRow.Cells[4].Value.ToString());
            int VRAM = int.Parse(form.CurrentDataGridViewRow.Cells[5].Value.ToString());

            if (form.RandomID)
            {
                for (Random random = new(); ;)
                {
                    bool isUnique = true;

                    newID = random.Next(1000, 9999);

                    foreach (var item in dataManager.ListComputer)
                    {
                        if (item.ID == newID)
                        {
                            isUnique = false;
                            break;
                        }
                    }

                    if (isUnique == true)
                        break;
                }
            }
            else
                newID = int.Parse(form.CurrentDataGridViewRow.Cells[0].Value.ToString());

            DataGridViewRow row = (DataGridViewRow)dataGridViewVGA.Rows[0].Clone();

            row.Cells[0].Value = newID;
            row.Cells[1].Value = manufacturer;
            row.Cells[2].Value = name;
            row.Cells[3].Value = price;
            row.Cells[4].Value = streamProcessor;
            row.Cells[5].Value = VRAM;

            dataManager.ListVGA.Add(new VGA(newID, manufacturer, name, price, streamProcessor, VRAM));

            tabControlPart.SelectedIndex = 1;

            dataGridViewVGA.Rows.Add(row);
            dataGridViewVGA.Rows[dataGridViewVGA.CurrentRow.Index].Selected = false;
            dataGridViewVGA.Rows[dataManager.ListVGA.Count - 1].Selected = true;

            statusBar.Text = "VGA was Successfully added.";
        }

        private void MenuNewRAM_Click(object sender, EventArgs e)
        {
            NewPart form = new("RAM");

            if (form.ShowDialog() != DialogResult.OK)
                return;

            int newID;
            string manufacturer = form.CurrentDataGridViewRow.Cells[1].Value.ToString();
            string name = form.CurrentDataGridViewRow.Cells[2].Value.ToString();
            int price = int.Parse(form.CurrentDataGridViewRow.Cells[3].Value.ToString());
            int capacity = int.Parse(form.CurrentDataGridViewRow.Cells[4].Value.ToString());
            int frequency = int.Parse(form.CurrentDataGridViewRow.Cells[5].Value.ToString());

            if (form.RandomID)
            {
                for (Random random = new(); ;)
                {
                    bool isUnique = true;

                    newID = random.Next(1000, 9999);

                    foreach (var item in dataManager.ListComputer)
                    {
                        if (item.ID == newID)
                        {
                            isUnique = false;
                            break;
                        }
                    }

                    if (isUnique == true)
                        break;
                }
            }
            else
                newID = int.Parse(form.CurrentDataGridViewRow.Cells[0].Value.ToString());

            DataGridViewRow row = (DataGridViewRow)dataGridViewRAM.Rows[0].Clone();

            row.Cells[0].Value = newID;
            row.Cells[1].Value = manufacturer;
            row.Cells[2].Value = name;
            row.Cells[3].Value = price;
            row.Cells[4].Value = capacity;
            row.Cells[5].Value = frequency;

            dataManager.ListRAM.Add(new RAM(newID, manufacturer, name, price, capacity, frequency));

            tabControlPart.SelectedIndex = 2;

            dataGridViewRAM.Rows.Add(row);
            dataGridViewRAM.Rows[dataGridViewRAM.CurrentRow.Index].Selected = false;
            dataGridViewRAM.Rows[dataManager.ListRAM.Count - 1].Selected = true;

            statusBar.Text = "RAM was Successfully added.";
        }

        private void MenuNewSSD_Click(object sender, EventArgs e)
        {
            NewPart form = new("SSD");

            if (form.ShowDialog() != DialogResult.OK)
                return;

            int newID;
            string manufacturer = form.CurrentDataGridViewRow.Cells[1].Value.ToString();
            string name = form.CurrentDataGridViewRow.Cells[2].Value.ToString();
            int price = int.Parse(form.CurrentDataGridViewRow.Cells[3].Value.ToString());
            int capacity = int.Parse(form.CurrentDataGridViewRow.Cells[4].Value.ToString());
            int performance = int.Parse(form.CurrentDataGridViewRow.Cells[5].Value.ToString());

            if (form.RandomID)
            {
                for (Random random = new(); ;)
                {
                    bool isUnique = true;

                    newID = random.Next(1000, 9999);

                    foreach (var item in dataManager.ListComputer)
                    {
                        if (item.ID == newID)
                        {
                            isUnique = false;
                            break;
                        }
                    }

                    if (isUnique == true)
                        break;
                }
            }
            else
                newID = int.Parse(form.CurrentDataGridViewRow.Cells[0].Value.ToString());

            DataGridViewRow row = (DataGridViewRow)dataGridViewSSD.Rows[0].Clone();

            row.Cells[0].Value = newID;
            row.Cells[1].Value = manufacturer;
            row.Cells[2].Value = name;
            row.Cells[3].Value = price;
            row.Cells[4].Value = capacity;
            row.Cells[5].Value = performance;

            dataManager.ListSSD.Add(new SSD(newID, manufacturer, name, price, capacity, performance));

            tabControlPart.SelectedIndex = 3;

            dataGridViewSSD.Rows.Add(row);
            dataGridViewSSD.Rows[dataGridViewSSD.CurrentRow.Index].Selected = false;
            dataGridViewSSD.Rows[dataManager.ListSSD.Count - 1].Selected = true;

            statusBar.Text = "SSD was Successfully added.";
        }

        ////////////////////////////////
        /* 모든 튜플 초기 상태로 복구 */
        ////////////////////////////////
        private void MenuResetDatabase_Click(object sender, EventArgs e)
        {
            dataManager.ResetDatabase();

            dataGridViewComputer.Rows.Clear();
            dataGridViewComputer.Refresh();
            dataGridViewCPU.Rows.Clear();
            dataGridViewCPU.Refresh();
            dataGridViewVGA.Rows.Clear();
            dataGridViewVGA.Refresh();
            dataGridViewRAM.Rows.Clear();
            dataGridViewRAM.Refresh();
            dataGridViewSSD.Rows.Clear();
            dataGridViewSSD.Refresh();

            InitializeTable();

            statusBar.Text = "Restore database to initial state.";
        }

        private void dataGridViewCPU_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewCPU.ClearSelection();
        }

        private void dataGridViewVGA_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewVGA.ClearSelection();
        }

        private void dataGridViewRAM_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewRAM.ClearSelection();
        }

        private void dataGridViewSSD_SelectionChanged(object sender, EventArgs e)
        {
            dataGridViewSSD.ClearSelection();
        }
    }
}
