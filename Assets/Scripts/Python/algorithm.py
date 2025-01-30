import json
import random
import numpy as np


# Leer archivo JSON
def leer_json(ruta):
    with open(ruta, "r") as archivo:
        datos = json.load(archivo)
    return datos


# Guardar archivo JSON
def guardar_json(ruta, datos):
    with open(ruta, "w") as archivo:
        json.dump(datos, archivo, indent=4)
    print(f"Datos guardados en {ruta}")


# Selección por elitismo
def seleccion_elitismo(poblacion, aptitudes, num_mejores=2):
    mejores_indices = sorted(range(len(aptitudes)), key=lambda i: aptitudes[i], reverse=True)[:num_mejores]
    elite = [poblacion[i] for i in mejores_indices]
    return elite


# Selección por torneo
def seleccion_torneo(poblacion, aptitudes, k=2):
    competidores = random.sample(range(len(poblacion)), k)
    mejor = max(competidores, key=lambda i: aptitudes[i])
    return poblacion[mejor]


# Cruce en dos puntos
def cruce_dos_puntos(padre1, padre2):
    hijo1 = [fila[:] for fila in padre1]
    hijo2 = [fila[:] for fila in padre2]
    punto1, punto2 = sorted(random.sample(range(len(padre1[0])), 2))

    for fila in range(len(padre1)):
        hijo1[fila][punto1:punto2] = padre2[fila][punto1:punto2]
        hijo2[fila][punto1:punto2] = padre1[fila][punto1:punto2]

    return hijo1, hijo2


# Mutación controlada
def mutacion_controlada(matriz,tasa_mutacion= 0.2):
    hijo_mutado = [fila[:] for fila in matriz]
    if random.random() < tasa_mutacion:
        fila, col = random.randint(0, len(matriz) - 1), random.randint(0, len(matriz[0]) - 1)
        hijo_mutado[fila][col] = random.choice([0, 1, 2, 3, 4])
    return hijo_mutado


# Algoritmo genético principal
def algoritmo_genetico():
    # Leer datos
    level_results = leer_json("C:/Users/katty/AppData/LocalLow/DefaultCompany/pl\level_results.json")
    levels = leer_json("new_  level.json")

    # Convertir poblaciones y aptitudes
    poblacion = list(levels.values())
    aptitudes = [nivel["Fitness"] for nivel in level_results["niveles"]]

    # Selección por elitismo
    num_elite = 2
    elite = seleccion_elitismo(poblacion, aptitudes, num_mejores=num_elite)

    # Selección por torneo
    nueva_poblacion = elite[:]

    while len(nueva_poblacion) < len(poblacion):
        padre1 = seleccion_torneo(poblacion, aptitudes)
        padre2 = seleccion_torneo(poblacion, aptitudes)
        hijo1, hijo2 = cruce_dos_puntos(padre1, padre2)
        hijo1 = mutacion_controlada(hijo1,0.5)
        hijo2 = mutacion_controlada(hijo2,0.5)
        nueva_poblacion.append(hijo1)
        if len(nueva_poblacion) < len(poblacion):
            nueva_poblacion.append(hijo2)

    # Crear nuevo archivo JSON
    nuevos_levels = {f"level{i+1}": nueva_poblacion[i] for i in range(len(nueva_poblacion))}
    guardar_json("new_level.json", nuevos_levels)


if __name__ == "__main__":
    algoritmo_genetico()