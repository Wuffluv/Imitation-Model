import numpy as np
import matplotlib.pyplot as plt

# Начальные параметры модели
x_init = 40       # Начальная численность жертв (например, кроликов)
y_init = 9        # Начальная численность хищников (например, лисиц)
alpha = 0.1       # Скорость размножения жертв (рождаемость)
beta = 0.02       # Коэффициент взаимодействия (эффект охоты хищников на жертв)
delta = 0.01      # Эффективность преобразования добычи в потомство хищников
gamma = 0.1       # Скорость смертности хищников
r_x = 5           # Пополнение популяции жертв (внешние факторы)
x_max = 100       # Максимально допустимая численность популяции жертв
y_max = 50        # Максимально допустимая численность популяции хищников
t_max = 200       # Время моделирования
dt = 0.1          # Шаг времени для численного расчёта

# Функция моделирования системы "жертва-хищник"
def simulate_predator_prey(x_init, y_init, alpha, beta, delta, gamma, r_x, x_max, y_max, t_max, dt):
    t = np.arange(0, t_max, dt)  # Временной массив
    x = np.zeros_like(t)         # Массив для численности жертв
    y = np.zeros_like(t)         # Массив для численности хищников
    x[0] = x_init                # Инициализация начальной численности жертв
    y[0] = y_init                # Инициализация начальной численности хищников

    for i in range(1, len(t)):
        # Вычисление изменения численности жертв и хищников за шаг времени
        dx = alpha * x[i-1] - beta * x[i-1] * y[i-1] + r_x
        dy = delta * x[i-1] * y[i-1] - gamma * y[i-1]

        # Обновление численности популяций с учетом максимальных ограничений
        x[i] = min(x[i-1] + dx * dt, x_max)
        y[i] = min(y[i-1] + dy * dt, y_max)

        # Предотвращение отрицательных значений численности
        if x[i] < 0: x[i] = 0
        if y[i] < 0: y[i] = 0

    return t, x, y

# Запуск симуляции
t, x, y = simulate_predator_prey(x_init, y_init, alpha, beta, delta, gamma, r_x, x_max, y_max, t_max, dt)

# Построение графиков численности жертв и хищников
plt.figure(figsize=(10, 6))
plt.plot(t, x, label='Prey Population (x)', color='green')  # График численности жертв
plt.plot(t, y, label='Predator Population (y)', color='red')  # График численности хищников
plt.xlabel('Time')                            # Подпись оси времени
plt.ylabel('Population Size')                 # Подпись оси численности
plt.legend()                                  # Легенда графика
plt.title('Volterra-Lotka Model')             # Заголовок графика
plt.grid()                                    # Сетка
plt.show()

# Построение фазовой траектории
plt.figure(figsize=(8, 8))
plt.plot(x, y, color='purple')                # Фазовая траектория (жертвы против хищников)
plt.xlabel('Prey Population (x)')            # Подпись оси жертв
plt.ylabel('Predator Population (y)')        # Подпись оси хищников
plt.title('Phase Trajectory of Predator-Prey Dynamics')  # Заголовок графика
plt.grid()                                    # Сетка
plt.show()
