from matplotlib import pyplot
import random
def f(x):
    return 4 / (x ** 2 + 1) ** 0.5

max_point = 5_000
# определяем точки графика функции
x_curve = [0.1 * k for k in range(41)]
y_curve = [f(x_curve[k]) for k in range(41)]
# определяем случайные точки
x_points = [random.uniform(0, 4) for k in range(max_point)]
y_points = [random.uniform(0, 4) for k in range(max_point)]
# построение картинки
pyplot.plot(x_curve, y_curve, color='red', linewidth=5, solid_capstyle='round')
pyplot.scatter(x_points, y_points, linewidth=1, s=7, edgecolors='black')
pyplot.show()
# вычисление доли точек, лежащих ниже графика функции
filtered_y_points = [y_points[k] for k in range(max_point)
                     if f(x_points[k]) >= y_points[k]]
# вывод результата
print('Приближённое значение интеграла:', len(filtered_y_points) / max_point * 16)
