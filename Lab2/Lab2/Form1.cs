using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting; // Необходимо для работы с Chart

namespace Lab2
{
    public partial class Form1 : Form
    {
        private Queue queue;
        private Random random;
        private List<int> waitingTimes;
        private int currentProcessingTime;
        private Request currentRequest;
        private int requestsProcessed;
        private int requestsRejected;

        private void lblAverageWait_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
            InitSimulation();
        }

        // Инициализация полей для симуляции
        private void InitSimulation()
        {
            queue = new Queue(50); // Максимальная длина очереди
            random = new Random();
            waitingTimes = new List<int>();
            currentProcessingTime = 0;
            currentRequest = null;
            requestsProcessed = 0;
            requestsRejected = 0;
        }

        // Метод для запуска симуляции
        private void StartSimulation(int totalTicks)
        {
            for (int tick = 0; tick < totalTicks; tick++)
            {
                // Обработка текущей заявки
                if (currentProcessingTime > 0)
                {
                    currentProcessingTime--;
                }
                else if (currentRequest != null)
                {
                    // Ограничение максимального времени ожидания
                    if (currentRequest.WaitingTime > 30000)
                    {
                        currentRequest.WaitingTime = 30000; // Установим максимальное время ожидания
                    }

                    waitingTimes.Add(currentRequest.WaitingTime);
                    currentRequest = null;
                    requestsProcessed++;
                }

                // Генерация новой заявки с измененной вероятностью
                if (random.NextDouble() < 0.1) // Уменьшим вероятность появления новой заявки
                {
                    var newRequest = new Request(tick, random.Next(1, 500)); // Уменьшим максимальное время обработки до 500
                    if (!queue.AddRequest(newRequest))
                    {
                        requestsRejected++;
                    }
                }

                // Если процессор свободен, берем новую заявку из очереди
                if (currentRequest == null)
                {
                    currentRequest = queue.PopRequest();
                    if (currentRequest != null)
                    {
                        currentProcessingTime = currentRequest.TreatmentTime;
                    }
                }

                // Увеличиваем время ожидания всех заявок в очереди
                queue.IncrementWaitingTimes();
            }
        }


        // Метод для отображения гистограммы с улучшенной визуализацией
        private void DisplayHistogram()
        {
            chartHistogram.Series.Clear();
            var series = new Series("Время ожидания");
            series.ChartType = SeriesChartType.Column;

            // Ограничиваем гистограмму для лучшей визуализации
            var histogram = new int[10];
            foreach (var time in waitingTimes)
            {
                int bin = Math.Min(time / 3000, histogram.Length - 1); // Разделяем на интервалы по 3000
                histogram[bin]++;
            }

            for (int i = 0; i < histogram.Length; i++)
            {
                series.Points.AddXY($"{i * 3000} - {(i + 1) * 3000}", histogram[i]);
            }

            chartHistogram.Series.Add(series);
        }

        private void btnStartSimulation_Click_1(object sender, EventArgs e)
        {
            int totalTicks = 100000; // Количество тиков времени
            StartSimulation(totalTicks);

            // Выводим результаты на форму
            lblProcessed.Text = $"Обработано заявок: {requestsProcessed}";
            lblRejected.Text = $"Отклонено заявок: {requestsRejected}";
            lblAverageWait.Text = $"Среднее время ожидания: {waitingTimes.Average()}";

            // Отображаем гистограмму
            DisplayHistogram();
        }
    }

    // Класс для представления заявки
    public class Request
    {
        public int Id { get; set; }
        public int TreatmentTime { get; set; }
        public int WaitingTime { get; set; }

        public Request(int id, int treatmentTime)
        {
            Id = id;
            TreatmentTime = treatmentTime;
            WaitingTime = 0;
        }
    }

    // Класс для очереди заявок
    public class Queue
    {
        private List<Request> requests = new List<Request>();
        private int maxLength;

        public Queue(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public bool AddRequest(Request request)
        {
            if (requests.Count < maxLength)
            {
                requests.Add(request);
                return true;
            }
            return false;
        }

        public Request PopRequest()
        {
            if (requests.Count > 0)
            {
                var request = requests[0];
                requests.RemoveAt(0);
                return request;
            }
            return null;
        }

        public void IncrementWaitingTimes()
        {
            foreach (var request in requests)
            {
                request.WaitingTime++;
            }
        }

        public int Count => requests.Count;
    }
}
