using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    public int CaidasTotales { get; private set; }
    public float Completitud { get; private set; }
    public float DificultadSalto { get; private set; }
    public int monedasNivel { get; set; }
    private int EnemigosTotales;
    public float Fitness { get;  set; }
    public int currentLevel = 1;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistente entre niveles.
        }
        else
        {
            Destroy(gameObject);
        }
    }
   

    public void CalcularFitness(float w1, float w2, float w3, float w4, float w5)
    {
 
      
        float normalizadasCaidas = Mathf.Clamp01(1 - ((float)CaidasTotales / 5));
        float normalizadasMonedas = Mathf.Clamp01((float)monedasNivel / 10);
        Fitness =
            w1 * Completitud +
            w2 * normalizadasCaidas + // Proporción de muertes por huecos
            w3 * (1 - DificultadSalto) +
            w4 * (1 - Mathf.Abs(EnemigosTotales - 3)) +
            w5 * normalizadasMonedas; // Suponiendo 100 como máximo


        Debug.Log($"Fitness calculado: {Fitness}");
    }

    public void ExportarDatosNivel()
    {
        // Ruta del archivo JSON
        string path = Path.Combine(Application.persistentDataPath, "level_results.json");

        // Lista para almacenar los datos de todos los niveles
        List<LevelDataExport> niveles = new List<LevelDataExport>();

        // Leer datos existentes si el archivo ya existe
        if (File.Exists(path))
        {
            string existingJson = File.ReadAllText(path);
            niveles = JsonUtility.FromJson<LevelDataList>(existingJson)?.niveles ?? new List<LevelDataExport>();
        }

        // Crear los datos del nivel actual
        LevelDataExport data = new LevelDataExport
        {
            LevelId = currentLevel, // Número del nivel actual
            CaidasTotales = CaidasTotales,
            Completitud = Completitud,
            DificultadSalto = DificultadSalto,
            EnemigosTotales = EnemigosTotales,
            monedasNivel = monedasNivel,
            Fitness = Fitness
        };

        // Agregar los datos del nivel actual a la lista
        niveles.Add(data);

        // Serializar la lista actualizada
        string json = JsonUtility.ToJson(new LevelDataList { niveles = niveles }, true);
        File.WriteAllText(path, json);

        Debug.Log($"Datos del nivel exportados a: {path}");
    }

    // Clase contenedora para manejar la lista de niveles
    [System.Serializable]
    public class LevelDataList
    {
        public List<LevelDataExport> niveles;
    }

    // Clase de datos del nivel
    [System.Serializable]
    public class LevelDataExport
    {
        public int LevelId; // ID del nivel
        public int CaidasTotales;
        public float Completitud;
        public float DificultadSalto;
        public int EnemigosTotales;
        public int monedasNivel;
        public float Fitness;
    }

    public void SetCompletitud(float value)
    {
        Completitud = value;
        
    }



    public void SetDificultadSalto(float value)
    {
        DificultadSalto = value;

    }
    public void SetEnemigosTotales(int totalEnemigos)
    {
        EnemigosTotales = totalEnemigos;
      
    }

    public int GetEnemigosTotales()
    {
        return EnemigosTotales;
    }

    public void SetMonedasNivel(int value)
    {
        monedasNivel = value;
    }
   
    public void IncrementarCaidas()
    {
        CaidasTotales++;
  
    }

    public void NextLevel()
    {
        currentLevel++;
        ReiniciarDatosNivel();
    }
    public void ReiniciarDatosNivel()
    {
        CaidasTotales = 0;
        Completitud = 0;
        DificultadSalto = 0;
        EnemigosTotales = 0;
        monedasNivel = 0;
        Fitness = 0;

        Debug.Log("Datos del nivel reiniciados.");
    }
}
