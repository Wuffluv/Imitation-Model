using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Markov_Chains
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ConfigureChart();
        }

        private void ConfigureChart()
        {
            // Настройка области графика
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.Titles.Clear();

            // Добавление области графика
            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Title = "Шаги (t)";
            chartArea.AxisY.Title = "Состояние";
            chartArea.AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chartArea.AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisX.Interval = 1;
            chart1.ChartAreas.Add(chartArea);

            // Настройка серии
            Series series = new Series("Состояния");
            series.ChartType = SeriesChartType.Line;
            series.Color = Color.Blue;
            series.BorderWidth = 2;
            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 6;
            series.MarkerColor = Color.Red;
            chart1.Series.Add(series);

            // Добавление заголовка
            chart1.Titles.Add("График изменения состояний системы");
            chart1.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
            chart1.Titles[0].Alignment = ContentAlignment.TopCenter;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Чтение данных
            int t_max = int.Parse(textBox1.Text);
            int n = int.Parse(textBox2.Text);
            if (n <= 1)
            {
                MessageBox.Show("Количество состояний должно быть больше 1.");
                return;
            }

            // Инициализация данных
            Random random = new Random();
            double[] p_vector = GenerateDistribution(n, random);
            double[,] p_matrix = GenerateTransitionMatrix(n, random);
            List<int> states = new List<int>();
            int absorbingState = n - 1; // Поглощающее состояние

            int totalTimeToAbsorption = 0;
            int absorptionCount = 0;

            for (int t = 0; t < t_max; t++)
            {
                // Генерация нового состояния
                int currentState = RandomState(p_vector, random);
                states.Add(currentState);

                // Проверка на поглощение
                if (currentState == absorbingState)
                {
                    totalTimeToAbsorption += t;
                    absorptionCount++;
                    break;
                }

                // Обновление распределения
                p_vector = UpdateDistribution(p_vector, p_matrix);
            }

            // Обновление данных графика
            chart1.Series["Состояния"].Points.Clear();
            for (int i = 0; i < states.Count; i++)
            {
                chart1.Series["Состояния"].Points.AddXY(i, states[i]);
            }

            // Вывод результатов
            listBox1.Items.Clear();
            listBox1.Items.Add("Среднее время до поглощения: " +
                               (absorptionCount > 0 ? (totalTimeToAbsorption / absorptionCount).ToString() : "Не достигнуто"));
            listBox1.Items.Add("Частота состояний:");
            foreach (var group in states.GroupBy(x => x))
            {
                listBox1.Items.Add($"Состояние {group.Key}: {group.Count()}");
            }
        }

        private double[] GenerateDistribution(int size, Random random)
        {
            double[] distribution = new double[size];
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                distribution[i] = random.NextDouble();
                sum += distribution[i];
            }
            for (int i = 0; i < size; i++)
            {
                distribution[i] /= sum;
            }
            return distribution;
        }

        private double[,] GenerateTransitionMatrix(int size, Random random)
        {
            double[,] matrix = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                double sum = 0;
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = random.NextDouble();
                    sum += matrix[i, j];
                }
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] /= sum;
                }
            }
            return matrix;
        }

        private int RandomState(double[] probabilities, Random random)
        {
            double r = random.NextDouble();
            double cumulative = 0;
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i];
                if (r < cumulative)
                {
                    return i;
                }
            }
            return probabilities.Length - 1;
        }

        private double[] UpdateDistribution(double[] current, double[,] matrix)
        {
            double[] newDistribution = new double[current.Length];
            for (int j = 0; j < current.Length; j++)
            {
                double sum = 0;
                for (int i = 0; i < current.Length; i++)
                {
                    sum += current[i] * matrix[i, j];
                }
                newDistribution[j] = sum;
            }
            return newDistribution;
        }
    }
}
