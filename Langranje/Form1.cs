using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Langranje
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public double LangrF(int n, double x_main)
        {
            double a = -1;
            double b = 1;
            double h = (b - a) / n;
            double[] x = new double[n + 1];
            if (radioButton1.Checked)                   // Рахуємо масив інтерполяційних вузлів
            {
                for (int i = 0; i <= n; i++)
                {
                    x[i] = a + i * h;
                }
            }
            if (radioButton2.Checked)
            {
                for (int i = 0; i <= n; i++)
                {
                    x[i] = ((a + b) / 2.0) + ((b - a) / 2.0 )* Math.Cos(((2.0 * i + 1) * 3.14) / (2.0 * (n + 1)));
                }
            }
            double[] f = new double[n + 1];
            for (int i = 0; i <= n; i++)            // Рахуємо масив значень функцій 
            {
                f[i] = 1 / (1 + 25 * x[i] * x[i]);
            }
            double l;
            double lagr = 0;
            for (int i = 0; i <= n; i++)                // Ланграж 
            {
                l = 1;
                for (int j = 0; j <= n; j++)
                {
                    if (j != i)
                        l *= (x_main - x[j]) / (x[i] - x[j]);
                }
                lagr += l * f[i];
            }

            for (int i = 0; i < n + 1; i++)
            {
                f[i] = f[i] - f[i] % 0.001;
                chart1.Series["points"].Points.AddXY(x[i], f[i]);
            }
            double result = Math.Abs(1 / (1 + 25 * x_main * x_main) - lagr);
            textBox2.Text = Convert.ToString(result);

            return lagr;
        }

        public double LagranjeG(int n, double x_main)
        {
            double a = -1;
            double b = 1;
            double h = (b - a) / n;
            double[] x = new double[n + 1];


            if (radioButton1.Checked)                     // Рахуємо масив інтерполяційних вузлів
            {
                for (int i = 0; i <= n; i++)
                {
                    x[i] = a + i * h;
                }
            }
            if (radioButton2.Checked)
            {
                for (int i = 0; i <= n; i++)
                {
                    x[i] = ((a + b) / 2.0) + ((b - a) / 2.0) * Math.Cos(((2.0 * i + 1) * 3.14) / (2.0 * (n + 1)));
                }
            }

            double[] g = new double[n + 1];                  // Рахуємо масив значень функцій 
            for (int i = 0; i <= n; i++)
            {
                g[i] = Math.Log(x[i] + 2);
            }

     

            double l;
            double lagr = 0;
            for (int i = 0; i <= n; i++)                     // Ланграж 
            {
                l = 1;
                for (int j = 0; j <= n; j++)
                {
                    if (j != i)
                        l *= (x_main - x[j]) / (x[i] - x[j]);
                }
                lagr += l * g[i];
            }


            for (int i = 0; i < n + 1; i++)
            {
                g[i] = g[i] - g[i] % 0.001;
                chart1.Series["points"].Points.AddXY(x[i], g[i]);
            }

            double result = Math.Abs(Math.Log(x_main + 2) - lagr);
            textBox2.Text = Convert.ToString(result);
            return lagr;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            chart1.Series["Main function"].Points.Clear();
            chart1.Series["Lagranje polinom"].Points.Clear();
            chart1.Series["points"].Points.Clear();


            int n1 = Convert.ToInt32(nTextbox.Text);
            int n2 = 100;
            double x_main = Convert.ToDouble(xTextbox.Text);
            double a = -1;
            double b = 1;
            double h = (b - a) / n2;
            double[] x = new double[n2 + 1];
            double[] f = new double[n2 + 1];
            if (radioButton1.Checked)
                for (int i = 0; i <= n2; i++)
                {
                    x[i] = a + i * h;

                }
            if (radioButton2.Checked)
            {
                for (int i = 0; i <= n2; i++)
                {
                    x[i] = ((a + b) / 2.0) + ((b - a) / 2.0) * Math.Cos(((2.0 * i + 1) * 3.14) / (2.0 * (n2 + 1)));
                }
            }

            for (int i = 0; i <= n2; i++)
            {
                f[i] = 1 / (1 + 25 * x[i] * x[i]);
                chart1.Series["Main function"].Points.AddXY(x[i], f[i]);
            }
      

            double lag = LangrF(n1, x_main);
            textBox1.Text = Convert.ToString(lag);

            double[] lag1 = new double[n2 + 1];
            for (int i = 0; i <= n2; i++)
            {
                lag1[i] = LangrF(n1, x[i]);
                chart1.Series["Lagranje polinom"].Points.AddXY(x[i], lag1[i]);
            }
            for (int i = 0; i <= n2; i++)
            {
                dataGridView1.Rows.Add(x[i], f[i], lag1[i]);
            }

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            chart1.Series["Main function"].Points.Clear();
            chart1.Series["Lagranje polinom"].Points.Clear();
            chart1.Series["points"].Points.Clear();

            int n1 = Convert.ToInt32(nTextbox.Text);
            int n2 = 100;
            double x_main = Convert.ToDouble(xTextbox.Text);
            double a = -1;
            double b = 1;
            double h = (b - a) / n2;
            double[] x = new double[n2 + 1];
            double[] f = new double[n2 + 1];
            if (radioButton1.Checked)
                for (int i = 0; i <= n2; i++)
                {
                    x[i] = a + i * h;

                }
            if (radioButton2.Checked)
            {
                for (int i = 0; i <= n2; i++)
                {
                    x[i] = (a + b) / 2.0 + (b - a) / 2.0 * Math.Cos(((2.0 * i + 1) * 3.14) / (2.0 * (n2 + 1)));
                }
            }
            for (int i = 0; i <= n2; i++)
            {
                f[i] = Math.Log(x[i] + 2);
                chart1.Series["Main function"].Points.AddXY(x[i], f[i]);
            }
            double lag = LagranjeG(n1, x_main);
            textBox1.Text = Convert.ToString(lag);

            double[] lag1 = new double[n2 + 1];
            for (int i = 0; i <= n2; i++)
            {
                lag1[i] = LagranjeG(n1, x[i]);
                chart1.Series["Lagranje polinom"].Points.AddXY(x[i], lag1[i]);
            }
            for (int i = 0; i <= n2; i++)
            {
                dataGridView1.Rows.Add(x[i], f[i], lag1[i]);
            }
        }
    }


}
