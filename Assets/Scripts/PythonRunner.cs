using System.Diagnostics;
using System.IO;
using UnityEngine;

public class PythonRunner : MonoBehaviour
{
    public string pythonExecutable = "python"; // Ruta del ejecutable de Python
    public string scriptPath = "Assets/Scripts/Python/algorithm.py"; // Ruta del script

    public void RunPythonScript()
    {
        // Verificar si el archivo existe
        if (!File.Exists(scriptPath))
        {
            UnityEngine.Debug.LogError($"No se encontró el script de Python en la ruta: {scriptPath}");
            return;
        }

        // Crear un proceso para ejecutar el script
        ProcessStartInfo processInfo = new ProcessStartInfo
        {
            FileName = pythonExecutable,
            Arguments = $"\"{scriptPath}\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        try
        {
            using (Process process = Process.Start(processInfo))
            {
                // Leer la salida del script
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                // Mostrar resultados
                if (!string.IsNullOrEmpty(output))
                {
                    UnityEngine.Debug.Log($"Python Output: {output}");
                }

                if (!string.IsNullOrEmpty(error))
                {
                    UnityEngine.Debug.LogError($"Python Error: {error}");
                }
            }
        }
        catch (System.Exception ex)
        {
            UnityEngine. Debug.LogError($"Error al ejecutar el script de Python: {ex.Message}");
        }
    }
}
