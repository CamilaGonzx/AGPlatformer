using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;


public class LevelLoader : MonoBehaviour
{
    [SerializeField] private GameObject[] blockPrefabs; // Array de prefabs
    [SerializeField] private float blockSize = 3.5f; // Tamaño de cada bloque en unidades
    [SerializeField] private string jsonFileName = "level"; // Nombre del archivo JSON sin extensión
    public int enemigosTotales { get; private set; }
    private List<GameObject> instanciados = new List<GameObject>(); // Lista para rastrear los objetos instanciados
    public static LevelLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }

    void Start()
    {
        // Cargar y generar el nivel al inicio
        LoadLevelFromJson(jsonFileName);
    }

    public int getEnemigosTotales(){ return enemigosTotales; }
    void LimpiarNivel()
    {
        foreach (GameObject obj in instanciados)
        {
            Destroy(obj); // Destruir cada objeto instanciado
        }
        instanciados.Clear(); // Limpiar la lista
        enemigosTotales = 0; // Reiniciar el contador de enemigos
    }


    public void LoadLevelFromJson(string fileName)
    {
        string filePath = Path.Combine(Application.dataPath, "Scripts/Python/new_level.json");
        string jsonFile = File.ReadAllText(filePath);

        // Deserializar el archivo JSON en un diccionario
        var niveles = JsonConvert.DeserializeObject<Dictionary<string, List<List<int>>>>(jsonFile);

        if (niveles == null)
        {
            Debug.LogError("Error: El archivo JSON no contiene datos válidos.");
            return;
        }

        // Determinar la clave del nivel actual
        string currentLevelKey = $"level{LevelManager.Instance.currentLevel}";
        if (!niveles.ContainsKey(currentLevelKey))
        {
            Debug.LogError($"Error: No se encontró el nivel '{currentLevelKey}' en el archivo JSON.");
            return;
        }

        // Obtener los datos del nivel actual
        List<List<int>> levelData = niveles[currentLevelKey];



        if (levelData == null || levelData == null || levelData.Count == 0)

        {
            Debug.LogError($"Error: El nivel '{currentLevelKey}' está vacío o mal formateado.");
            return;
        }

        Debug.Log($"Cargando nivel: {currentLevelKey}");

        // Limpiar el nivel anterior antes de cargar el nuevo
        LimpiarNivel();

        CalculoGaps(levelData);

        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 spawnOffset = new Vector3(cameraPosition.x - 5.3f, cameraPosition.y + .7f, 0);

        float gridSize = 0.559f; // Tamaño uniforme para la cuadrícula

        for (int y = 0; y < levelData.Count; y++)
        {
            if (levelData[y] == null)

            {
                Debug.LogError($"Error: La fila {y} está vacía en el JSON.");
                continue;
            }

            for (int x = 0; x < levelData[y].Count; x++)
            {
                int blockId = levelData[y][x];


                if (blockId >= blockPrefabs.Length || blockPrefabs[blockId] == null)
                {
                    Debug.LogError($"Error: No hay prefab asignado para el bloque con ID {blockId}.");
                    continue;
                }
                if (blockId == 0)
                {
                    continue;
                }

                if (blockId == 2)
                {
                    enemigosTotales++;
                }


                // Obtener prefab y dimensiones reales
                GameObject prefab = blockPrefabs[blockId];
                float platformOffset = (blockId == 4) ? -.36f : 0.0f; // Desplazar plataformas hacia abajo
                SpriteRenderer renderer = prefab.GetComponent<SpriteRenderer>();
                if (renderer == null)
                {
                    Debug.LogError($"Error: El prefab con ID {blockId} no tiene un SpriteRenderer.");
                    continue;
                }

                // Tamaño real del prefab (incluyendo escala)
                float prefabWidth = renderer.bounds.size.x * prefab.transform.localScale.x;
                float prefabHeight = renderer.bounds.size.y * prefab.transform.localScale.y;

                // Ajustar el tamaño para eliminar separaciones
                float adjustmentFactor = 0.70f; // Reducir ligeramente para compensar márgenes
                prefabWidth *= adjustmentFactor;
                prefabHeight *= adjustmentFactor;

                // Calcular posición ajustada
                Vector3 position = new Vector3(x * gridSize, -y * gridSize + platformOffset, 0) + spawnOffset;
                // Instanciar el prefab en la posición calculada
                GameObject instancia = Instantiate(prefab, position, Quaternion.identity);
                instanciados.Add(instancia);
            }
            
        }

        LevelManager.Instance.SetEnemigosTotales(enemigosTotales);

    }

    void CalculoGaps(List<List<int>> levelData)
    {
        int maxSalto = 3; // Máximo número de bloques que el jugador puede saltar
        List<int> gaps = new List<int>(); // Lista para guardar el tamaño de los gaps
        int contador = 0; // Contador para el tamaño del gap actual
        bool enGap = false; // Indica si estamos en un gap

        for (int x = 0; x < levelData[4].Count; x++) // Recorre la fila 4
        {
            int blockId = levelData[4][x];

            if (blockId == 0) // Detecta un espacio vacío
            {
                if (!enGap) enGap = true;
                contador++;
            }
            else // Detecta una plataforma
            {
                if (enGap)
                {
                    enGap = false;
                    gaps.Add(contador); // Guarda el tamaño del gap
                    contador = 0; // Reinicia el contador
                }
            }
        }

        // Si termina en un gap, agrega el último
        if (enGap) gaps.Add(contador);

        // Analiza los gaps
        foreach (int gap in gaps)
        {
            if (gap > maxSalto)
            {
                Debug.Log($"Gap largo detectado: {gap} bloques (excede el máximo de {maxSalto}).");
            }
           
        }

        // Calcula el promedio o aplica penalización si es necesario
        float dificultadTotal = 0f;
        foreach (int gap in gaps)
        {
            if (gap > maxSalto)
            {
                dificultadTotal += gap * 2; // Penalización para gaps largos
            }
            else
            {
                dificultadTotal += gap; // Gaps dentro del límite
            }
        }

        float dificultadMaxima = gaps.Count * maxSalto * 2;

        // Normaliza Jd
        float Jd = dificultadTotal / dificultadMaxima;

        Debug.Log($"DIFICULTAD DE SAALTO: {Jd} ");
        LevelManager.Instance.SetDificultadSalto(Jd);
       
    }



}
