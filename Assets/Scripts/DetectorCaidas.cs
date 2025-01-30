using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectorCaidas : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colision");
        // Verifica si el objeto que cayó es el jugador
        if (collision.tag == "Jugador")
        {
            Debug.Log("El jugador ha caído");
            LevelManager.Instance.IncrementarCaidas();
            // Obtén el componente del jugador y maneja la lógica de muerte o reinicio
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Muerte(); // Reduce una vida o maneja la lógica de reinicio
            }
        }
    }
}

