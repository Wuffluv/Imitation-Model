import numpy as np
import matplotlib.pyplot as plt
import matplotlib.animation as animation

# Размеры сетки
N = 50

# Функция для инициализации сетки случайным образом
def initialize_grid(size):
    # Создаем случайную матрицу, состоящую из 0 и 1
    return np.random.choice([0, 1], size*size, p=[0.7, 0.3]).reshape(size, size)

# Функция для проверки окончания игры
def check_game_over(grid, previous_states):
    # Проверка на отсутствие живых клеток
    if not grid.any():
        return True

    # Проверка на повторение конфигурации (периодическая конфигурация)
    # конфигурация — это текущее распределение живых и мертвых клеток на поле
    for state in previous_states:
        if np.array_equal(grid, state):
            return True

    # Проверка на стабильную конфигурацию (отсутствие изменений)
    if len(previous_states) > 0 and np.array_equal(grid, previous_states[-1]):
        return True

    return False

# Функция для обновления состояния клеточного автомата
def update(frameNum, img, grid, size, previous_states):
    # Создаем копию текущей сетки для расчета нового поколения
    new_grid = grid.copy()

    # Проходим по всем клеткам сетки
    for i in range(size):
        for j in range(size):
            # Считаем количество "живых" соседей вокруг текущей клетки
            total = int((grid[i, (j-1)%size] + grid[i, (j+1)%size] +
                         grid[(i-1)%size, j] + grid[(i+1)%size, j] +
                         grid[(i-1)%size, (j-1)%size] + grid[(i-1)%size, (j+1)%size] +
                         grid[(i+1)%size, (j-1)%size] + grid[(i+1)%size, (j+1)%size]))

            # Правила "Игры Жизнь"
            # В пустой (мёртвой) клетке, с которой соседствуют три живые клетки, зарождается жизнь
            if grid[i, j] == 0 and total == 3:
                new_grid[i, j] = 1
            # Если у живой клетки есть две или три живые соседки, то эта клетка продолжает жить
            elif grid[i, j] == 1 and (total == 2 or total == 3):
                new_grid[i, j] = 1
            # В противном случае клетка умирает (от одиночества или перенаселенности)
            else:
                new_grid[i, j] = 0

    # Проверяем условия окончания игры
    if check_game_over(new_grid, previous_states):
        plt.close() # Закрываем окно анимации, если игра окончена
        return img

    # Обновляем изображение и сетку
    previous_states.append(grid.copy())
    img.set_data(new_grid)
    grid[:] = new_grid
    return img

# Основная функция, запускающая клеточный автомат
def main():
    # Инициализируем сетку
    grid = initialize_grid(N)
    previous_states = []

    # Создаем фигуру для анимации
    fig, ax = plt.subplots()
    img = ax.imshow(grid, interpolation='nearest', cmap='gray')

    # Анимация
    ani = animation.FuncAnimation(fig, update, fargs=(img, grid, N, previous_states),
                                  frames=200, interval=300, save_count=50,
                                  repeat=False, blit=False)
    plt.show()

if __name__ == '__main__':
    main()
