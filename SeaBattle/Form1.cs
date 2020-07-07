using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sea_battle
{
    public partial class Form1 : Form
    {
        Random rnd=new Random();
        int k = 0, l = 4, count_user = 0, count_pc=0,column, row, last=0, length=0, direct=0, l_column, l_row;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.Size = new Size(500, 360);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Create_table(dataGridView1);
            Create_table(dataGridView2);
            Create_table(dataGridView3);
            Random_ship(dataGridView2);
            dataGridView1.ClearSelection();
            dataGridView3.ClearSelection();

            textBox1.Text = "Для початку гри ти повинен розташувати кораблі на своєму ігровому полі";
        }

        private void button1_Click(object sender, EventArgs e) // OWN
        {
            buttonSTART.Visible = false;
            k = 0;
            l = 4;
            Clear_choice();
            buttonOwn.Visible = false;
            buttonRand.Visible = false;
            buttonSelect.Visible = true;
            buttonSelect.Location = buttonOwn.Location;
            dataGridView1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e) // RANDOM
        {
            Clear_choice();
            Random_ship(dataGridView1);
            buttonSTART.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e) // SELECT
        {
            if (check_bound(dataGridView1) == true && check_build(dataGridView1, l) == true && Build_Ship(dataGridView1, l) == true)
            {
                k++;
            }
            if (k + l == 5)
            {
                k = 0;
                l--;
            }
            if (l == 0)
            {
                buttonOwn.Visible = true;
                buttonRand.Visible = true;
                buttonSelect.Visible = false;
                buttonSTART.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e) //START
        {
            Start_Game();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e) //Стріляємо у поле супротивника
        {
            Bang_User();
        }

        private bool Build_Ship(DataGridView MyGrid, int count)  // контроль довжини корабля
        {
            if (MyGrid.SelectedCells.Count != count)
            {
                textBox1.Text = "Вибач, але потрібно вибрати " + count + "-х палубний корабель";
                return false;
            }
            else
            {
                textBox1.Text="";
                for (int i = 0; i < MyGrid.SelectedCells.Count; i++)
                {
                    MyGrid.SelectedCells[i].Style.BackColor = Color.Coral;
                }
                return true;
            }
        }

        private bool check_build(DataGridView MyGrid, int len)   // перевірка положення корабля
        {
            DataGridViewSelectedCellCollection A = MyGrid.SelectedCells;

            if (A.Count > 2)
            {
                if (A[0].RowIndex != A[1].RowIndex) // перевірка чи корабель не горизонтальний 
                {
                    for (int i = 0; i < A.Count; i++)
                    {
                        if (A[0].ColumnIndex != A[i].ColumnIndex || A[0].RowIndex - A[i].RowIndex > len) // контроль положення і довжини
                        {
                            textBox1.Text = "Вибач, але таке розташування корабля заборонене";
                            return false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < A.Count; i++)
                    {
                        if (A[0].RowIndex != A[i].RowIndex || A[0].ColumnIndex - A[i].ColumnIndex > len)
                        {
                            textBox1.Text = "Вибач, але таке розташування корабля заборонене";
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool check_bound(DataGridView MyGrid)    // перевірка положення корабля відносно інших кораблів
        {
            DataGridViewSelectedCellCollection A = MyGrid.SelectedCells;

            int column = 0, row = 0;
            for (int i = 0; i < A.Count; i++)
            {
                column = A[i].ColumnIndex;
                row = A[i].RowIndex;
                if (column == 0)
                {
                    return false;
                }
                if (MyGrid.Rows[row].Cells[column].Style.BackColor == Color.Coral ||
                    column != 0 && row != 0 && MyGrid.Rows[row - 1].Cells[column - 1].Style.BackColor == Color.Coral ||
                    row != 0 && MyGrid.Rows[row - 1].Cells[column].Style.BackColor == Color.Coral ||
                    column != 10 && row != 0 && MyGrid.Rows[row - 1].Cells[column + 1].Style.BackColor == Color.Coral ||
                    column != 0 && MyGrid.Rows[row].Cells[column - 1].Style.BackColor == Color.Coral ||
                    column != 10 && MyGrid.Rows[row].Cells[column + 1].Style.BackColor == Color.Coral ||
                    column != 0 && row != 9 && MyGrid.Rows[row + 1].Cells[column - 1].Style.BackColor == Color.Coral ||
                    row != 9 && MyGrid.Rows[row + 1].Cells[column].Style.BackColor == Color.Coral ||
                    column != 10 && row != 9 && MyGrid.Rows[row + 1].Cells[column + 1].Style.BackColor == Color.Coral)
                {
                    textBox1.Text = "Вибач, але кораблі не повинні дотикатись один до одного";
                    return false;
                }
            }
            return true;
        }

        private void Random_ship(DataGridView MyGrid)
        {
            Random rnd = new Random();
            int column;
            int row;
            int direct;

            MyGrid.ClearSelection();

            column = rnd.Next(1, 11);
            if (column > 7)
            {
                row = rnd.Next(7);
                MyGrid.Rows[row].Cells[column].Selected = true;
                MyGrid.Rows[row + 1].Cells[column].Selected = true;
                MyGrid.Rows[row + 2].Cells[column].Selected = true;
                MyGrid.Rows[row + 3].Cells[column].Selected = true;
            }
            else
            {
                row = rnd.Next(10);
                direct = rnd.Next(2);
                if (direct == 1 && row > 6)
                {
                    row = 6;
                    MyGrid.Rows[row].Cells[column].Selected = true;
                    MyGrid.Rows[row + 1].Cells[column].Selected = true;
                    MyGrid.Rows[row + 2].Cells[column].Selected = true;
                    MyGrid.Rows[row + 3].Cells[column].Selected = true;
                }
                else if (direct == 1)
                {
                    MyGrid.Rows[row].Cells[column].Selected = true;
                    MyGrid.Rows[row + 1].Cells[column].Selected = true;
                    MyGrid.Rows[row + 2].Cells[column].Selected = true;
                    MyGrid.Rows[row + 3].Cells[column].Selected = true;
                }
                else
                {
                    MyGrid.Rows[row].Cells[column].Selected = true;
                    MyGrid.Rows[row].Cells[column + 1].Selected = true;
                    MyGrid.Rows[row].Cells[column + 2].Selected = true;
                    MyGrid.Rows[row].Cells[column + 3].Selected = true;
                }
            }
            Build_Ship(MyGrid, 4);

            for (int i = 0; i < 2; i++)
            {
                MyGrid.ClearSelection();

                column = rnd.Next(1, 11);
                if (column > 8)
                {
                    row = rnd.Next(8);
                    MyGrid.Rows[row].Cells[column].Selected = true;
                    MyGrid.Rows[row + 1].Cells[column].Selected = true;
                    MyGrid.Rows[row + 2].Cells[column].Selected = true;
                }
                else
                {
                    row = rnd.Next(10);
                    direct = rnd.Next(2);
                    if (direct == 1 && row > 7)
                    {
                        row = 7;
                        MyGrid.Rows[row].Cells[column].Selected = true;
                        MyGrid.Rows[row + 1].Cells[column].Selected = true;
                        MyGrid.Rows[row + 2].Cells[column].Selected = true;
                    }
                    else if (direct == 1)
                    {
                        MyGrid.Rows[row].Cells[column].Selected = true;
                        MyGrid.Rows[row + 1].Cells[column].Selected = true;
                        MyGrid.Rows[row + 2].Cells[column].Selected = true;
                    }
                    else
                    {
                        MyGrid.Rows[row].Cells[column].Selected = true;
                        MyGrid.Rows[row].Cells[column + 1].Selected = true;
                        MyGrid.Rows[row].Cells[column + 2].Selected = true;
                    }
                }
                if (check_bound(MyGrid) != true)
                {
                    i--;
                }
                else
                {
                    Build_Ship(MyGrid, 3);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                MyGrid.ClearSelection();

                column = rnd.Next(1, 11);
                if (column > 9)
                {
                    row = rnd.Next(9);
                    MyGrid.Rows[row].Cells[column].Selected = true;
                    MyGrid.Rows[row + 1].Cells[column].Selected = true;
                }
                else
                {
                    row = rnd.Next(10);
                    direct = rnd.Next(2);
                    if (direct == 1 && row > 8)
                    {
                        row = 8;
                        MyGrid.Rows[row].Cells[column].Selected = true;
                        MyGrid.Rows[row + 1].Cells[column].Selected = true;
                    }
                    else if (direct == 1)
                    {
                        MyGrid.Rows[row].Cells[column].Selected = true;
                        MyGrid.Rows[row + 1].Cells[column].Selected = true;
                    }
                    else
                    {
                        MyGrid.Rows[row].Cells[column].Selected = true;
                        MyGrid.Rows[row].Cells[column + 1].Selected = true;
                    }
                }
                if (check_bound(MyGrid) != true)
                {
                    i--;
                }
                else
                {
                    Build_Ship(MyGrid, 2);
                }
            }
            for (int i = 0; i < 4; i++)
            {
                MyGrid.ClearSelection();

                column = rnd.Next(1, 11);
                row = rnd.Next(10);
                MyGrid.Rows[row].Cells[column].Selected = true;
                if (check_bound(MyGrid) != true)
                {
                    i--;
                }
                else
                {
                    Build_Ship(MyGrid, 1);
                }
            }
            MyGrid.ClearSelection();
        }

        private void Clear_choice()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 1; j < 11; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Style.BackColor = Color.GhostWhite;
                }
            }
        }

        private void Create_table(DataGridView MyGrid)
        {
            MyGrid.RowCount = 10;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                MyGrid.Rows[i].Height = 27;
                MyGrid.Rows[i].Cells[0].Value = (i + 1).ToString();
                MyGrid.Rows[i].Cells[0].Style = dataGridView1.ColumnHeadersDefaultCellStyle;
            }
            MyGrid.RowHeadersVisible = false;
        }

        private void Bang_User() // Ми стріляємо - змінює поле супротивника
        {
            int row = dataGridView3.CurrentRow.Index;
            int column = dataGridView3.CurrentCell.ColumnIndex;
            if (dataGridView2.Rows[row].Cells[column].Style.BackColor == Control.DefaultBackColor) // Якщо такого кольору, то не можна стріляти
            {
                textBox1.Text = "Ви не можете здійснити вистріл в цю клітинку, оскільки у ній по правилах не може бути корабля";
            }
            else if (dataGridView2.Rows[row].Cells[column].Style.BackColor != Color.Coral) // Якщо не у корабель попали
                                                                                        //ставить крапочку, якщо промахнулися
            {
                dataGridView2.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                dataGridView3.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                dataGridView3.Rows[row].Cells[column].Value = "⋅";
                dataGridView2.Rows[row].Cells[column].Value = "⋅";
                while(Bangs_PC())
                {
                    textBox1.Text = "Суперник влучив";
                }
            }
            else
            {     //Якщо попали у корабель - Х і міняється кольор (ПОРАНЕНИЙ) і +1 до балів 
                dataGridView3.Rows[row].Cells[column].Style = dataGridView1.ColumnHeadersDefaultCellStyle;
                dataGridView3.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                dataGridView3.Rows[row].Cells[column].Value = "X";

                dataGridView2.Rows[row].Cells[column].Style = dataGridView1.ColumnHeadersDefaultCellStyle;
                dataGridView2.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                dataGridView2.Rows[row].Cells[column].Value = "X";
                block_cells_u(dataGridView2);
                block_cells_u(dataGridView3);
                count_user++;
                if(check_kill_user(dataGridView2))   //Перевірка, чи вбили корабель
                {
                    block_cell_for_user(dataGridView2);
                    block_cell_for_user(dataGridView3);
                }
            }

            if (count_user==20 || count_pc==20)
            {
                Stop_Game();
            }
        }

        private bool Bangs_PC() // Стріляє Пк
        {
            if (count_pc==20) // Чи не закінчилася гра
            {
                return false;
            }

            if (column==11 || row ==10 || column ==0 || row < 0)
            {
                direct += 2;
                if (direct > 3)
                {
                    direct -= 4;
                }
                change_direct(direct);
                change_direct(direct);
            }
            if (dataGridView1.Rows[row].Cells[column].Style.BackColor==Control.DefaultBackColor && length==0 ||
                dataGridView1.Rows[row].Cells[column].Style.BackColor == Color.Black && length == 0)
            {
                Random rnd = new Random();
                column = rnd.Next(1, 11);
                row = rnd.Next(10);
                return true;
            }
            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor==Control.DefaultBackColor && length==1 ||
                dataGridView1.Rows[row].Cells[column].Style.BackColor == Color.Black && length == 1)
            {
                column = l_column;
                row=l_row;
                direct = rnd.Next(4);
                change_direct(direct);

                return true;
            }

            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor == Control.DefaultBackColor && length > 1 ||
                     dataGridView1.Rows[row].Cells[column].Style.BackColor == Color.Black && length >1)
            {
                row = l_row;
                column = l_column;
                direct += 2;
                if (direct>3)
                {
                    direct -= 4;
                }

                change_direct(direct);
                
                return true;
            }


            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor == Control.DefaultBackColor && last == 0 ||
                     dataGridView1.Rows[row].Cells[column].Style.BackColor == Color.Black && last == 0)
            {
                return_direct(direct);

                direct += 1;
                if (direct > 3)
                {
                    direct -= 4;
                }
                change_direct(direct);
                return true;
            }

            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor==Color.GhostWhite && length==0)
            {
                dataGridView1.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                Random rnd = new Random();
                column = rnd.Next(1, 11);
                row = rnd.Next(10);
                return false;
            }

            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor== Color.Coral && length==0)
            {
                dataGridView1.Rows[row].Cells[column].Style.BackColor = Color.Black;
                block_cells();
                if (check_kill(dataGridView1) != true)
                {
                    Random rnd = new Random();
                    length = 1;
                    last = 1;
                    l_column = column;
                    l_row = row;
                    direct = rnd.Next(4);
                    change_direct(direct);
                }
                else
                {
                    block_cells_kill(dataGridView1);
                    Random rnd = new Random();
                    column = rnd.Next(1, 11);
                    row = rnd.Next(10);
                }
                return true;
            }

            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor != Color.Coral && length == 1 && last == 0)
            {
                dataGridView1.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                column = l_column;
                row=l_row;
                direct = rnd.Next(4);
                change_direct(direct);
                return false;
            }

            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor != Color.Coral && length > 1 && last == 0)
            {

                dataGridView1.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                if (direct == 0)
                {
                    row+=length;
                    direct=2;
                }
                else if (direct == 1)
                {
                    column-=length;
                    direct=3;
                }
                else if (direct == 2)
                {
                    row-=length;
                    direct=0;
                }
                else
                {
                    column+=length;
                    direct = 1;
                }
                return false;
            }

            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor == Color.Coral && length > 0)
            {
                last = 1;
                dataGridView1.Rows[row].Cells[column].Style.BackColor = Color.Black;
                block_cells();
                if (check_kill(dataGridView1) == true)
                {
                    block_cell_kill();
                    length = 0;
                    last = 0;
                    Random rnd = new Random();
                    column = rnd.Next(1, 11);
                    row = rnd.Next(10);
                }
                else
                {
                    change_direct(direct);
                    l_column = column;
                    l_row = row;
                    length++;
                }
                return true;
            }
            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor != Color.Coral && length ==1 && last == 1)
            {
                dataGridView1.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                last = 1;
                column = l_column;
                row = l_row;
                direct = rnd.Next(4);
                change_direct(direct);
                return false;
            }

            else if (dataGridView1.Rows[row].Cells[column].Style.BackColor != Color.Coral && length > 1 && last == 1)
            {
                dataGridView1.Rows[row].Cells[column].Style.BackColor = Control.DefaultBackColor;
                last = 0;
                column = l_column;
                row = l_row;

                if (direct==0)
                {
                    row += length;
                    direct = 2;
                    row ++;
                }
                else if (direct==1)
                {
                    column -= length;
                    direct = 3;
                    column--;
                }
                else if (direct == 2)
                {
                    row -= length;
                    direct = 0;
                    row--;
                }
                else
                {
                    column += length;
                    direct = 1;
                    column++;
                }
                return false;
            }

            else
            {
                return true;
            }

        }

        private bool check_kill(DataGridView MyGrid) // Перевіряє чи вбито
        {
            count_pc++;
         
            int k_column = column;
            int k_row = row;

            if (k_row!=0 && (MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor== Color.GhostWhite || MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor==Control.DefaultBackColor) &&
                k_column != 10 && (MyGrid.Rows[k_row].Cells[k_column+1].Style.BackColor == Color.GhostWhite || MyGrid.Rows[k_row].Cells[k_column+1].Style.BackColor == Control.DefaultBackColor) &&
                k_row != 9 && (MyGrid.Rows[k_row + 1].Cells[k_column].Style.BackColor == Color.GhostWhite || MyGrid.Rows[k_row +1 ].Cells[k_column].Style.BackColor == Control.DefaultBackColor) &&
                k_column != 1 && (MyGrid.Rows[k_row].Cells[k_column-1].Style.BackColor == Color.GhostWhite || MyGrid.Rows[k_row].Cells[k_column-1].Style.BackColor == Control.DefaultBackColor))
            {
                MessageBox.Show("Корабель вбито");
                return true;
            }

            if(k_row!=0 && MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor != Color.GhostWhite
                     && MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor != Control.DefaultBackColor)
            {
                while (k_row!=0 && MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor==Color.Black)
                {
                    k_row--;
                }

                if (k_row != 0 && MyGrid.Rows[k_row - 1].Cells[k_column].Style.BackColor == Color.Coral)
                {
                    return false;
                }
            }

            if (k_column != 10 && MyGrid.Rows[k_row].Cells[k_column+1].Style.BackColor != Color.GhostWhite
                     && MyGrid.Rows[k_row].Cells[k_column + 1].Style.BackColor != Control.DefaultBackColor)
            {
                while (k_column != 10 && MyGrid.Rows[k_row].Cells[k_column+1].Style.BackColor == Color.Black)
                {
                    k_column++;
                }
                if (k_column != 10 && MyGrid.Rows[k_row].Cells[k_column + 1].Style.BackColor == Color.Coral)
                {
                    return false;
                }
            }

            if (k_row != 9 && MyGrid.Rows[k_row +1].Cells[k_column].Style.BackColor != Color.GhostWhite
                     && MyGrid.Rows[k_row+1].Cells[k_column].Style.BackColor != Control.DefaultBackColor)
            {
                while (k_row != 9 && MyGrid.Rows[k_row + 1].Cells[k_column].Style.BackColor == Color.Black)
                {
                    k_row++;
                }
                if (k_row != 9 && MyGrid.Rows[k_row + 1].Cells[k_column].Style.BackColor == Color.Coral)
                {
                    return false;
                }
            }

            if (k_column != 1 && MyGrid.Rows[k_row].Cells[k_column-1].Style.BackColor != Color.GhostWhite
                     && MyGrid.Rows[k_row].Cells[k_column -1].Style.BackColor != Control.DefaultBackColor)
            {
                while (k_column != 1 && MyGrid.Rows[k_row].Cells[k_column - 1].Style.BackColor == Color.Black)
                {
                    k_column--;
                }
                if (k_column != 10 && MyGrid.Rows[k_row].Cells[k_column - 1].Style.BackColor == Color.Coral)
                {
                    return false;
                }
            }
            MessageBox.Show("Корабель вбито");
            return true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Це вікно буде закрито. Чи дійсно ви хочете зробити це?",
               "Exit", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.No)
            {
                e.Cancel = true;

            }
        }

        private void change_direct(int d)
        {
            if (direct == 0)
            {
                row--;
            }
            else if (direct == 1)
            {
                column++;
            }
            else if (direct == 2)
            {
                row++;
            }
            else
            {
                column--;
            }
        }

        private void return_direct(int d)
        {
            if (direct == 0)
            {
                row++;
            }
            else if (direct == 1)
            {
                column--;
            }
            else if (direct == 2)
            {
                row--;
            }
            else
            {
                column++;
            }
        }

        private void block_cells_kill(DataGridView MyGrid)
        {
            if (row!=0)
            {
                MyGrid.Rows[row - 1].Cells[column].Style.BackColor = Control.DefaultBackColor;
            }
            if (row!=9)
            {
                MyGrid.Rows[row + 1].Cells[column].Style.BackColor = Control.DefaultBackColor;
            }
            if (column!=1)
            {
                MyGrid.Rows[row].Cells[column - 1].Style.BackColor = Control.DefaultBackColor;
            }
            if(column!=10)
            {
                MyGrid.Rows[row].Cells[column + 1].Style.BackColor = Control.DefaultBackColor;
            }
        }

        private void block_cell_kill()
        {
            int k_row = row;
            int k_column = column;
            if (direct==0 && row!=0)
            {
                dataGridView1.Rows[row - 1].Cells[column].Style.BackColor = Control.DefaultBackColor;
                while (k_row != 10 && dataGridView1.Rows[k_row].Cells[k_column].Style.BackColor == Color.Black)
                {
                    k_row++;
                }
                if (k_row != 10)
                {
                    dataGridView1.Rows[k_row].Cells[k_column].Style.BackColor = Control.DefaultBackColor;
                }
            }
            else if (direct==1 && column!=10)
            {
                dataGridView1.Rows[row].Cells[column + 1].Style.BackColor = Control.DefaultBackColor;
                while (k_column !=1  && dataGridView1.Rows[k_row].Cells[k_column].Style.BackColor == Color.Black)
                {
                    k_column--;
                }
                if (k_column!=1)
                {
                    dataGridView1.Rows[k_row].Cells[k_column].Style.BackColor = Control.DefaultBackColor;
                }
            }
            else if (direct==2 && row!=9)
            {
                dataGridView1.Rows[row + 1].Cells[column].Style.BackColor = Control.DefaultBackColor;
                while (k_row != 0 && dataGridView1.Rows[k_row].Cells[k_column].Style.BackColor == Color.Black)
                {
                    k_row--;
                }
                if (k_row != 0)
                {
                    dataGridView1.Rows[k_row].Cells[k_column].Style.BackColor = Control.DefaultBackColor;
                }
            }
            else if (direct==3 && column!=1)
            {
                dataGridView1.Rows[row].Cells[column - 1].Style.BackColor = Control.DefaultBackColor;
                while (k_column != 11 && dataGridView1.Rows[k_row].Cells[k_column].Style.BackColor == Color.Black)
                {
                    k_column++;
                }
                if (k_column != 11)
                {
                    dataGridView1.Rows[k_row].Cells[k_column].Style.BackColor = Control.DefaultBackColor;
                }
            }
        }

        private bool check_kill_user(DataGridView MyGrid)
        {
            int k_row = dataGridView3.CurrentCell.RowIndex;
            int k_column = dataGridView3.CurrentCell.ColumnIndex;

            if (k_row!=0 && (MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor== Color.GhostWhite || MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor==Control.DefaultBackColor) &&
                k_column != 10 && (MyGrid.Rows[k_row].Cells[k_column+1].Style.BackColor == Color.GhostWhite || MyGrid.Rows[k_row].Cells[k_column+1].Style.BackColor == Control.DefaultBackColor) &&
                k_row != 9 && (MyGrid.Rows[k_row + 1].Cells[k_column].Style.BackColor == Color.GhostWhite || MyGrid.Rows[k_row +1 ].Cells[k_column].Style.BackColor == Control.DefaultBackColor) &&
                k_column != 1 && (MyGrid.Rows[k_row].Cells[k_column-1].Style.BackColor == Color.GhostWhite || MyGrid.Rows[k_row].Cells[k_column-1].Style.BackColor == Control.DefaultBackColor))
            {
                MessageBox.Show("Корабель вбито користувачем");
                return true;
            }

            if(k_row!=0 && MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor != Color.GhostWhite
                     && MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor != Control.DefaultBackColor)
            {
                while (k_row!=0 && MyGrid.Rows[k_row-1].Cells[k_column].Style.BackColor==Color.Black)
                {
                    k_row--;
                }

                if (k_row != 0 && MyGrid.Rows[k_row - 1].Cells[k_column].Style.BackColor == Color.Coral)
                {
                    return false;
                }
            }

            if (k_column != 10 && MyGrid.Rows[k_row].Cells[k_column+1].Style.BackColor != Color.GhostWhite
                     && MyGrid.Rows[k_row].Cells[k_column + 1].Style.BackColor != Control.DefaultBackColor)
            {
                while (k_column != 10 && MyGrid.Rows[k_row].Cells[k_column+1].Style.BackColor == Color.Black)
                {
                    k_column++;
                }
                if (k_column != 10 && MyGrid.Rows[k_row].Cells[k_column + 1].Style.BackColor == Color.Coral)
                {
                    return false;
                }
            }

            if (k_row != 9 && MyGrid.Rows[k_row +1].Cells[k_column].Style.BackColor != Color.GhostWhite
                     && MyGrid.Rows[k_row+1].Cells[k_column].Style.BackColor != Control.DefaultBackColor)
            {
                while (k_row != 9 && MyGrid.Rows[k_row + 1].Cells[k_column].Style.BackColor == Color.Black)
                {
                    k_row++;
                }
                if (k_row != 9 && MyGrid.Rows[k_row + 1].Cells[k_column].Style.BackColor == Color.Coral)
                {
                    return false;
                }
            }

            if (k_column != 1 && MyGrid.Rows[k_row].Cells[k_column-1].Style.BackColor != Color.GhostWhite
                     && MyGrid.Rows[k_row].Cells[k_column -1].Style.BackColor != Control.DefaultBackColor)
            {
                while (k_column != 1 && MyGrid.Rows[k_row].Cells[k_column - 1].Style.BackColor == Color.Black)
                {
                    k_column--;
                }
                if (k_column != 10 && MyGrid.Rows[k_row].Cells[k_column - 1].Style.BackColor == Color.Coral)
                {
                    return false;
                }
            }
            MessageBox.Show("Корабель вбито користувачем");
            return true;
        }

        private void block_cells()
        {
            if (column != 1 && row != 0)
            {
                dataGridView1.Rows[row - 1].Cells[column - 1].Style.BackColor = Control.DefaultBackColor;
            }
            if (column != 10 && row != 0)
            {
                dataGridView1.Rows[row - 1].Cells[column + 1].Style.BackColor = Control.DefaultBackColor;
            }
            if (column != 10 && row != 9)
            {
                dataGridView1.Rows[row + 1].Cells[column + 1].Style.BackColor = Control.DefaultBackColor;
            }
            if (column != 0 && row != 9)
            {
                dataGridView1.Rows[row + 1].Cells[column - 1].Style.BackColor = Control.DefaultBackColor;
            }
        }

        private void block_cell_for_user(DataGridView MyGrid)
        {
            int k_row = dataGridView3.CurrentCell.RowIndex;
            int k_column = dataGridView3.CurrentCell.ColumnIndex;

            while (k_row!=-1 && 
                dataGridView2.Rows[k_row].Cells[k_column].Value!=null &&  
                dataGridView2.Rows[k_row].Cells[k_column].Value.ToString()=="X")
            {
                k_row--;
            }

            if (k_row!=-1)
            {
                MyGrid.Rows[k_row].Cells[k_column].Style.BackColor = Control.DefaultBackColor;
            }

            k_row = dataGridView3.CurrentCell.RowIndex;

            while (k_row !=10 &&
                dataGridView2.Rows[k_row].Cells[k_column].Value!=null &&  
                dataGridView2.Rows[k_row].Cells[k_column].Value.ToString()=="X")
            {
                k_row++;
            }
            if (k_row != 10)
            {
                dataGridView3.Rows[k_row].Cells[k_column].Style.BackColor = Control.DefaultBackColor;
            }

             k_row = dataGridView3.CurrentCell.RowIndex;

             while (k_column != 0 &&
                 dataGridView2.Rows[k_row].Cells[k_column].Value != null &&
                 dataGridView2.Rows[k_row].Cells[k_column].Value.ToString() == "X")
             {
                 k_column--;
             }

             if (k_column != 0)
             {
                 dataGridView3.Rows[k_row].Cells[k_column].Style.BackColor = Control.DefaultBackColor;
             }

             k_column = dataGridView3.CurrentCell.ColumnIndex;

             while (k_column != 11 &&
                  dataGridView2.Rows[k_row].Cells[k_column].Value != null &&
                  dataGridView2.Rows[k_row].Cells[k_column].Value.ToString() == "X")
             {
                 k_column++;
             }
             if (k_column != 11)
             {
                 dataGridView3.Rows[k_row].Cells[k_column].Style.BackColor = Control.DefaultBackColor;
             }
        }

        private void block_cells_u(DataGridView MyGrid)
        {
            int k_row = dataGridView3.CurrentCell.RowIndex;
            int k_column = dataGridView3.CurrentCell.ColumnIndex;
            if (k_column != 1 && k_row != 0)
            {
                MyGrid.Rows[k_row - 1].Cells[k_column - 1].Style.BackColor = Control.DefaultBackColor;
            }
            if (k_column != 10 && k_row != 0)
            {
                MyGrid.Rows[k_row - 1].Cells[k_column + 1].Style.BackColor = Control.DefaultBackColor;
            }
            if (k_column != 10 && k_row != 9)
            {
                MyGrid.Rows[k_row + 1].Cells[k_column + 1].Style.BackColor = Control.DefaultBackColor;
            }
            if (k_column != 0 && k_row != 9)
            {
                MyGrid.Rows[k_row + 1].Cells[k_column - 1].Style.BackColor = Control.DefaultBackColor;
            }
        }

        private void Start_Game()
        {
            dataGridView1.ClearSelection();
            dataGridView3.ClearSelection();
            buttonOwn.Visible = false;
            buttonRand.Visible = false;
            buttonSelect.Visible = false;
            buttonSTART.Visible = false;

            dataGridView3.Visible = true;
            this.Size=new Size(800, 360);
            Random rnd = new Random();
            column = rnd.Next(1, 11);
            row = rnd.Next(10);
            textBox1.Text="Починай гру, можеш зробити перший вистріл";
        }

        private void Stop_Game()
        {
            dataGridView3.Visible = false;
            dataGridView2.Visible = true;
            dataGridView2.Location = dataGridView3.Location;
            if (count_pc==20)
            {
                MessageBox.Show("Ви програли");
            }
            else
            {
                MessageBox.Show("Вітаю з перемогою");
            }
        }
    }
}