import random
import matplotlib.pyplot as plt

def generate_sequence():
    sequence = []
    n = 1
    max_attempts = 1000  # Максимальное количество попыток для поиска нового числа
    attempts = 0

    while attempts < max_attempts:
        # Генерируем случайное число в интервале (0, 1)
        x = random.uniform(0, 1)

        # Проверяем, можно ли добавить число в последовательность
        interval_size = 1 / n
        intervals = [i * interval_size for i in range(n + 1)]

        # Определяем, в каком интервале находится число
        interval_index = None
        for i in range(n):
            if intervals[i] < x < intervals[i + 1]:
                interval_index = i
                break

        # Проверяем, находится ли число в новом интервале
        if interval_index is not None and interval_index not in [s[1] for s in sequence]:
            sequence.append((x, interval_index))
            n += 1
            attempts = 0  # Сбрасываем счетчик попыток при успешном добавлении
        else:
            attempts += 1
            if attempts >= max_attempts // 2:  # Прерываем, если долго не удается найти новое число
                break

    # Возвращаем последовательность без информации об интервалах
    return [s[0] for s in sequence]

# Генерируем наиболее длинную конечную последовательность
sequence = generate_sequence()

# Выводим результат
print("Наиболее длинная последовательность:", sequence)
print("Длина последовательности:", len(sequence))

# Построение графика последовательности
if len(sequence) > 0:
    plt.figure(figsize=(10, 6))
    plt.plot(range(1, len(sequence) + 1), sequence, 'o-', label='Последовательность {xn}')
    plt.xlabel('Индекс n')
    plt.ylabel('Значение xn')
    plt.title('Наиболее длинная последовательность, удовлетворяющая условиям из задачи')
    plt.grid(True)
    plt.legend()
    plt.show()
else:
    print("Ошибка")
