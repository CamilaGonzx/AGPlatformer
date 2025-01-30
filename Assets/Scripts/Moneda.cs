using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.CompareTag("Jugador"))
        { // Asegúrate de que no haya un espacio adicional en la etiqueta
            

            PlayerController player = collision.GetComponent<PlayerController>();
           
            player.RecogerMonedas(); // Llama al método en PlayerController
            
            Destroy(gameObject); // Destruye al enemigo


        }
    }
}
