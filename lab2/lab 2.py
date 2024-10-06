import math
import random
import matplotlib.pyplot as plt
from scipy import stats

class ExponentialGenerator:
    def __init__(self, rate, tics_per_second):
        # Инициализируем генератор с заданной интенсивностью и количеством тиков в секунду
        self.rate = rate
        self.tics_per_second = tics_per_second
        self.time_to_next_request = 0

    def generate_next(self):
        # Генерируем время до следующей заявки с использованием экспоненциального распределения
        uniform_random_value = random.random()
        self.time_to_next_request = math.log(1 - uniform_random_value) * (-1 / self.rate)
        # Преобразуем во временные единицы (тики)
        self.time_to_next_request = round(self.time_to_next_request * self.tics_per_second)


class Request:
    def __init__(self, request_id, treatment_time):
        # Инициализируем заявку с идентификатором и временем обработки
        self.request_id = request_id
        self.treatment_time = treatment_time
        self.waiting_time = 0  # Инициализируем время ожидания нулем


class Queue:
    def __init__(self, max_length):
        # Инициализируем очередь с максимальной длиной
        self.container = []
        self.max_length = max_length

    def add_request(self, request):
        # Добавляем заявку в очередь, если есть место
        if len(self.container) < self.max_length:
            self.container.append(request)
            return True
        return False  # Возвращаем False, если очередь заполнена

    def pop_request(self):
        # Удаляем и возвращаем первую заявку из очереди
        if len(self.container) > 0:
            return self.container.pop(0)
        return None  # Возвращаем None, если очередь пуста


class Processor:
    def __init__(self):
        # Инициализируем процессор без текущей заявки
        self.current_request = None
        self.remaining_time = 0

    def add_request(self, request):
        # Назначаем заявку процессору, если он свободен
        if not self.current_request:
            self.current_request = request
            self.remaining_time = request.treatment_time
            return True
        return False  # Возвращаем False, если процессор занят

    def process(self):
        # Обрабатываем текущую заявку, если она есть
        if self.current_request:
            # Уменьшаем оставшееся время обработки
            if self.remaining_time > 0:
                self.remaining_time -= 1
            # Если обработка завершена, возвращаем выполненную заявку
            if self.remaining_time == 0:
                completed_request = self.current_request
                self.current_request = None
                return completed_request
        return None  # Возвращаем None, если заявка не завершена


def simulate_queue_system(total_tics, rate, max_queue_length, max_treatment_time, tics_per_second):
    # Инициализируем компоненты системы массового обслуживания
    generator = ExponentialGenerator(rate, tics_per_second)
    queue = Queue(max_queue_length)
    processor = Processor()

    # Списки для хранения результатов и статистики
    requests_completed = []
    requests_rejected = []
    requests_in_queue = []
    time_ticks = []

    request_id = 0

    # Основной цикл симуляции
    for tik in range(total_tics):
        # Обрабатываем текущую заявку в процессоре
        completed_request = processor.process()
        if completed_request:
            # Добавляем выполненную заявку в список завершенных заявок
            requests_completed.append(completed_request)

        # Перемещаем заявку из очереди в процессор, если процессор свободен
        if not processor.current_request:
            request = queue.pop_request()
            if request:
                processor.add_request(request)

        # Генерируем новую заявку, если время до следующей заявки равно нулю
        if generator.time_to_next_request == 0:
            request_id += 1
            # Генерируем случайное время обработки для новой заявки
            treatment_time = random.randint(1, max_treatment_time)
            new_request = Request(request_id, treatment_time)
            # Генерируем время до следующей заявки
            generator.generate_next()
            # Пытаемся добавить новую заявку в очередь
            if not queue.add_request(new_request):
                # Если очередь заполнена, добавляем заявку в список отклоненных
                requests_rejected.append(new_request)
        else:
            # Уменьшаем время до следующей заявки на один тик
            generator.time_to_next_request -= 1

        # Обновляем время ожидания для каждой заявки в очереди
        for request in queue.container:
            request.waiting_time += 1

        # Сохраняем статистику для построения графиков
        time_ticks.append(tik)
        requests_in_queue.append(len(queue.container))

    # Выводим результаты симуляции
    print('Количество обработанных заявок:', len(requests_completed))
    print('Количество отброшенных заявок:', len(requests_rejected))
    print('Количество заявок в очереди:', len(queue.container))

    # Проводим статистический анализ времени ожидания выполненных заявок
    waiting_times = [req.waiting_time for req in requests_completed]
    if waiting_times:
        print('Описание числовых характеристик времени ожидания:', stats.describe(waiting_times))

    # Построение гистограммы времени ожидания
    plt.figure()
    plt.hist(waiting_times, bins=30)
    plt.title("Гистограмма времени ожидания (в тиках)")
    plt.show()

    # Построение графика количества заявок в очереди по времени
    plt.figure()
    plt.plot(time_ticks, requests_in_queue)
    plt.title("График заполненности очереди")
    plt.show()

    return requests_completed, requests_rejected


if __name__ == "__main__":
    # Параметры симуляции
    total_tics = 10000  # Общее количество тиков для симуляции
    rate = 0.25  # Интенсивность генерации заявок (лямбда)
    max_queue_length = 50  # Максимальная длина очереди
    max_treatment_time = 1650  # Максимальное время обработки заявки
    tics_per_second = 100  # Количество тиков в секунду

    # Запуск симуляции
    simulate_queue_system(total_tics, rate, max_queue_length, max_treatment_time, tics_per_second)
