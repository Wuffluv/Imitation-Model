using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SMO
{
    public partial class Form1 : Form
    {
        // Генератор случайных чисел для СМО
        private Random random = new Random();

        // Данные для графиков
        private List<int> requestsInQueue = new List<int>();
        private List<int> requestsCompleted = new List<int>();
        private List<int> requestsRejected = new List<int>();
        private List<int> time = new List<int>();

        // Параметры СМО
        private const int MaxQueueLength = 10;
        private const int TotalTicks = 1000;
        private Queue<int> queue = new Queue<int>();
        private int completedCount = 0;
        private int rejectedCount = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Сбрасываем данные
            ResetData();

            // Основной цикл имитации
            for (int t = 0; t < TotalTicks; t++)
            {
                time.Add(t);

                // Генерация новой заявки
                if (random.NextDouble() < 0.5) // Вероятность генерации заявки
                {
                    if (queue.Count < MaxQueueLength)
                    {
                        queue.Enqueue(t); // Добавляем заявку в очередь
                    }
                    else
                    {
                        rejectedCount++; // Заявка отклонена
                    }
                }

                // Обработка заявки
                if (queue.Count > 0)
                {
                    queue.Dequeue();
                    completedCount++;
                }

                // Сбор статистики
                requestsInQueue.Add(queue.Count);
                requestsCompleted.Add(completedCount);
                requestsRejected.Add(rejectedCount);
            }

            // Отрисовка графиков
            DrawGraph(pictureBox1, time, requestsInQueue, "Заполненность очереди");
            DrawGraph(pictureBox2, time, requestsCompleted, "Обработанные заявки");
            DrawGraph(pictureBox3, time, requestsRejected, "Отклоненные заявки");
        }

        private void ResetData()
        {
            queue.Clear();
            completedCount = 0;
            rejectedCount = 0;
            requestsInQueue.Clear();
            requestsCompleted.Clear();
            requestsRejected.Clear();
            time.Clear();
        }

        private void DrawGraph(PictureBox pictureBox, List<int> xData, List<int> yData, string title)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);

                // Отрисовка заголовка
                g.DrawString(title, new Font("Arial", 10), Brushes.Black, new PointF(10, 10));

                // Определение масштаба
                float xScale = (float)pictureBox.Width / xData.Count;
                float yScale = (float)pictureBox.Height / (yData.Max() + 1);

                // Отрисовка осей
                g.DrawLine(Pens.Black, 0, pictureBox.Height - 20, pictureBox.Width, pictureBox.Height - 20);
                g.DrawLine(Pens.Black, 20, 0, 20, pictureBox.Height);

                // Отрисовка данных
                for (int i = 1; i < xData.Count; i++)
                {
                    g.DrawLine(Pens.Blue,
                        20 + (i - 1) * xScale, pictureBox.Height - 20 - yData[i - 1] * yScale,
                        20 + i * xScale, pictureBox.Height - 20 - yData[i] * yScale);
                }
            }

            pictureBox.Image = bmp;
        }
    }
}
