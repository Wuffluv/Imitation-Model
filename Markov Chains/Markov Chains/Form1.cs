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
            ConfigureChart(); // Настройка графика при инициализации формы
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Чтение данных, введённых пользователем
            int t_max = int.Parse(textBox1.Text); // Максимальное количество шагов
            int n = int.Parse(textBox2.Text); // Количество состояний
            int runs = 100; // Количество прогонов модели

            // Проверка валидности количества состояний
            if (n <= 1)
            {
                MessageBox.Show("Количество состояний должно быть больше 1.");
                return;
            }

            // Инициализация данных
            Random random = new Random(); // Генератор случайных чисел
            double[,] p_matrix = GenerateTransitionMatrix(n, random); // Генерация матрицы переходных вероятностей
            int absorbingState = n - 1; // Установка последнего состояния как поглощающего

            int totalTimeToAbsorption = 0; // Общая сумма времени до поглощения
            int absorptionCount = 0; // Количество прогонов, достигших поглощающего состояния
            Dictionary<int, int> stateFrequency = new Dictionary<int, int>(); // Частота нахождения в состояниях

            // Прогон модели runs раз
            for (int run = 0; run < runs; run++)
            {
                // Генерация начального распределения вероятностей
                double[] p_vector = GenerateDistribution(n, random);
                List<int> states = new List<int>();

                // Эмуляция шагов для текущего прогона
                for (int t = 0; t < t_max; t++)
                {
                    // Генерация нового состояния
                    int currentState = RandomState(p_vector, random);
                    states.Add(currentState);

                    // Увеличение частоты нахождения в текущем состоянии
                    if (!stateFrequency.ContainsKey(currentState))
                        stateFrequency[currentState] = 0;
                    stateFrequency[currentState]++;

                    // Проверка, достигнуто ли поглощающее состояние
                    if (currentState == absorbingState)
                    {
                        totalTimeToAbsorption += t; // Сохранение времени до поглощения
                        absorptionCount++; // Увеличение счётчика прогонов с поглощением
                        break; // Выход из цикла шагов
                    }

                    // Обновление распределения вероятностей
                    p_vector = UpdateDistribution(p_vector, p_matrix);
                }
            }

            // Обновление данных графика
            chart1.Series["Состояния"].Points.Clear();
            foreach (var state in stateFrequency)
            {
                chart1.Series["Состояния"].Points.AddXY(state.Key, state.Value);
            }

            // Вывод результатов в listBox1
            listBox1.Items.Clear();
            listBox1.Items.Add("Среднее время до поглощения: " +
                               (absorptionCount > 0 ? (totalTimeToAbsorption / (double)absorptionCount).ToString("F2") : "Не достигнуто"));
            listBox1.Items.Add("Распределение вероятностей состояний:");
            foreach (var state in stateFrequency.OrderBy(x => x.Key))
            {
                double probability = state.Value / (double)(runs * t_max); // Расчёт вероятности для каждого состояния
                listBox1.Items.Add($"Состояние {state.Key}: {probability:P2}");
            }
        }

        // Генерация начального распределения вероятностей
        private double[] GenerateDistribution(int size, Random random)
        {
            double[] distribution = new double[size];
            double sum = 0;
            for (int i = 0; i < size; i++)
            {
                distribution[i] = random.NextDouble(); // Случайное значение вероятности
                sum += distribution[i];
            }
            for (int i = 0; i < size; i++)
            {
                distribution[i] /= sum; // Нормализация вероятностей
            }
            return distribution;
        }

        // Генерация матрицы переходных вероятностей
        private double[,] GenerateTransitionMatrix(int size, Random random)
        {
            double[,] matrix = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                double sum = 0;
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] = random.NextDouble(); // Случайное значение вероятности перехода
                    sum += matrix[i, j];
                }
                for (int j = 0; j < size; j++)
                {
                    matrix[i, j] /= sum; // Нормализация вероятностей
                }
            }
            return matrix;
        }

        // Генерация следующего состояния на основе текущего распределения вероятностей
        private int RandomState(double[] probabilities, Random random)
        {
            double r = random.NextDouble(); // Случайное число от 0 до 1
            double cumulative = 0;
            for (int i = 0; i < probabilities.Length; i++)
            {
                cumulative += probabilities[i]; // Накопление вероятностей
                if (r < cumulative)
                {
                    return i; // Возврат состояния, соответствующего вероятности
                }
            }
            return probabilities.Length - 1; // Возврат последнего состояния, если случайное число больше всех
        }

        // Обновление распределения вероятностей на основе матрицы переходов
        private double[] UpdateDistribution(double[] current, double[,] matrix)
        {
            double[] newDistribution = new double[current.Length];
            for (int j = 0; j < current.Length; j++)
            {
                double sum = 0;
                for (int i = 0; i < current.Length; i++)
                {
                    sum += current[i] * matrix[i, j]; // Умножение вектора на матрицу
                }
                newDistribution[j] = sum;
            }
            return newDistribution;
        }

        // Настройка графика
        private void ConfigureChart()
        {
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.Titles.Clear();

            // Добавление области графика
            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.AxisX.Title = "Состояния";
            chartArea.AxisY.Title = "Частота";
            chartArea.AxisX.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chartArea.AxisY.TitleFont = new Font("Arial", 12, FontStyle.Bold);
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas.Add(chartArea);

            // Настройка серии
            Series series = new Series("Состояния");
            series.ChartType = SeriesChartType.Column;
            series.Color = Color.Blue;
            chart1.Series.Add(series);

            // Добавление заголовка
            chart1.Titles.Add("Распределение состояний");
            chart1.Titles[0].Font = new Font("Arial", 14, FontStyle.Bold);
            chart1.Titles[0].Alignment = ContentAlignment.TopCenter;
        }
    }
}
