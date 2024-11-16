using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SMO
{
    public partial class Form1 : Form
    {
        private Random random = new Random();

        // Данные для графиков
        private List<int> requestsInQueue = new List<int>();  // Заполненность очереди по тикам времени
        private List<int> requestsCompleted = new List<int>();  // Количество обработанных заявок по тикам времени
        private List<int> requestsRejected = new List<int>();  // Количество отклонённых заявок по тикам времени
        private List<int> time = new List<int>();  // Массив времени (тики)

        // Общие параметры
        private const int TotalTicks = 1000;  // Количество тиков времени
        private const int MaxQueueLength = 3; // Максимальная длина очереди

        public Form1()
        {
            InitializeComponent();
        }

        // Запуск моделирования при нажатии кнопки
        private void button1_Click(object sender, EventArgs e)
        {
            RunMultipleProcessors(); // СМО с несколькими процессорами
        }

        // Моделирование СМО с несколькими процессорами
        private void RunMultipleProcessors()
        {
            // Очередь заявок
            Queue<int> queue = new Queue<int>();
            int completedCount = 0;  // Количество обработанных заявок
            int rejectedCount = 0;  // Количество отклонённых заявок

            // Процессоры (массив)
            const int processorCount = 3;  // Количество процессоров
            int[] processors = new int[processorCount];  // Время оставшейся обработки для каждого процессора

            ResetData();  // Сброс данных перед моделированием

            // Основной цикл моделирования
            for (int t = 0; t < TotalTicks; t++)
            {
                time.Add(t);

                // Генерация заявки с вероятностью 0.7
                if (random.NextDouble() < 0.7)
                {
                    if (queue.Count < MaxQueueLength) // Если есть место в очереди
                    {
                        queue.Enqueue(t);  // Добавляем заявку в очередь
                    }
                    else
                    {
                        rejectedCount++;  // Заявка отклонена, так как очередь полная
                    }
                }

                // Обработка заявок процессорами
                for (int i = 0; i < processorCount; i++)
                {
                    if (processors[i] == 0 && queue.Count > 0) // Если процессор свободен и очередь не пуста
                    {
                        queue.Dequeue();  // Берём заявку из очереди
                        processors[i] = random.Next(1, 5); // Случайное время обработки заявки
                        completedCount++;  // Увеличиваем количество обработанных заявок
                    }
                    else if (processors[i] > 0)
                    {
                        processors[i]--; // Уменьшаем оставшееся время обработки
                    }
                }

                // Сбор статистики
                requestsInQueue.Add(queue.Count);  // Количество заявок в очереди
                requestsCompleted.Add(completedCount);  // Общее количество обработанных заявок
                requestsRejected.Add(rejectedCount);  // Общее количество отклонённых заявок
            }

            DrawGraphs();  // Построение графиков
        }

        // Сброс данных перед новым моделированием
        private void ResetData()
        {
            requestsInQueue.Clear();
            requestsCompleted.Clear();
            requestsRejected.Clear();
            time.Clear();
        }

        // Отрисовка всех графиков
        private void DrawGraphs()
        {
            DrawGraph(pictureBox1, time, requestsInQueue, "Заполненность очереди", "Время (тики)", "Число заявок");
            DrawGraph(pictureBox2, time, requestsCompleted, "Обработанные заявки", "Время (тики)", "Число заявок");
            DrawGraph(pictureBox3, time, requestsRejected, "Отклонённые заявки", "Время (тики)", "Число заявок");
        }

        // Метод для отрисовки одного графика
        private void DrawGraph(PictureBox pictureBox, List<int> xData, List<int> yData, string title, string xLabel, string yLabel)
        {
            Bitmap bmp = new Bitmap(pictureBox.Width, pictureBox.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);  // Очищаем фон

                // Отрисовка заголовка графика
                g.DrawString(title, new Font("Arial", 12, FontStyle.Bold), Brushes.Black, new PointF(10, 10));

                // Проверка на пустые данные
                if (xData.Count == 0 || yData.Count == 0)
                {
                    g.DrawString("Нет данных", new Font("Arial", 10), Brushes.Red, new PointF(10, pictureBox.Height / 2));
                    pictureBox.Image = bmp;
                    return;
                }

                // Определение масштабов для отрисовки
                float xScale = (float)(pictureBox.Width - 80) / xData.Count; // Учитываем отступы для оси X
                float yScale = (float)(pictureBox.Height - 80) / (yData.Max() > 0 ? yData.Max() : 1); // Учитываем отступы для оси Y

                // Отрисовка осей координат
                g.DrawLine(Pens.Black, 60, pictureBox.Height - 40, pictureBox.Width - 20, pictureBox.Height - 40); // X-ось
                g.DrawLine(Pens.Black, 60, 20, 60, pictureBox.Height - 40); // Y-ось

                // Отрисовка делений на оси X
                for (int i = 0; i <= xData.Count; i += xData.Count / 10)
                {
                    float xPos = 60 + i * xScale;
                    g.DrawLine(Pens.Gray, xPos, pictureBox.Height - 40, xPos, 20); // Линии сетки для оси X
                    g.DrawString((i).ToString(), new Font("Arial", 8), Brushes.Black, new PointF(xPos - 10, pictureBox.Height - 35)); // Подписи оси X
                }

                // Отрисовка делений на оси Y
                for (int i = 0; i <= yData.Max(); i += Math.Max(1, yData.Max() / 10))
                {
                    float yPos = pictureBox.Height - 40 - i * yScale;
                    g.DrawLine(Pens.Gray, 60, yPos, pictureBox.Width - 20, yPos); // Линии сетки для оси Y
                    g.DrawString(i.ToString(), new Font("Arial", 8), Brushes.Black, new PointF(35, yPos - 7)); // Подписи оси Y
                }

                // Отрисовка линий графика
                for (int i = 1; i < xData.Count; i++)
                {
                    float x1 = 60 + (i - 1) * xScale;
                    float y1 = pictureBox.Height - 40 - yData[i - 1] * yScale;
                    float x2 = 60 + i * xScale;
                    float y2 = pictureBox.Height - 40 - yData[i] * yScale;

                    g.DrawLine(new Pen(Color.Blue, 2), x1, y1, x2, y2); // Линия графика
                }

                // Подписи осей
                g.DrawString(xLabel, new Font("Arial", 10), Brushes.Black, new PointF(pictureBox.Width / 2, pictureBox.Height - 20));
                g.DrawString(yLabel, new Font("Arial", 10), Brushes.Black, new PointF(10, pictureBox.Height / 2), new StringFormat { FormatFlags = StringFormatFlags.DirectionVertical });
            }

            pictureBox.Image = bmp;  // Устанавливаем изображение для pictureBox
        }
    }
}
