using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
  

    /// <summary>
    /// Sent when an incoming collider makes contact with this object's
    /// collider (2D physics only).
    /// </summary>
    /// <param name="other">The Collision2D data associated with this collision.</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        

        if (collision.collider.tag == "Jugador")
        {
       
            PlayerController player = collision.collider.GetComponent<PlayerController>();

            if (player != null) // Verifica que el jugador tiene el componente PlayerController
            {
                // Verifica si el jugador está por encima del enemigo
                if (collision.GetContact(0).point.y > transform.position.y)
                {
                 
                    Destroy(gameObject); // Destruye al enemigo
                }
                else
                {
               
                    player.Muerte(); // Reduce la vida del jugador
                }
            }
        }
    }

    
}
