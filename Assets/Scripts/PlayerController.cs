using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private int vidasIniciales = 3;
    [SerializeField] private int monedasIniciales = 0; // Número inicial de vidas
    private int vidasActuales;
    private int monedasRecogidas;
    private int monedasNivel;
    private bool dead;
    private Rigidbody2D body;
    private Animator anim;
    public Text vidasText;
    public Text monedasText;
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        vidasActuales = vidasIniciales;
        monedasRecogidas = monedasIniciales;// Establece las vidas iniciales
        vidasText.text = "Vidas Actuales: "+vidasActuales;
        monedasText.text = "Monedas: " + monedasRecogidas;
    }
    
    

    public void Muerte()
    {
        // Reduce una vida
        vidasActuales = Mathf.Clamp(vidasActuales - 1, 0, 99);
        vidasText.text = "Vidas Actuales: "+vidasActuales;
            
        // Verifica si el jugador se queda sin vidas
        if (vidasActuales <= 0)
        {
            Debug.Log("Game Over");
            // Detén al jugador y ejecuta lógica de Game Over
            GameOver();
             dead = true;
        }else{
            ReiniciarPosicion();
        }
      
    }

    public void RecogerMonedas()
    {
        monedasRecogidas++;
        monedasNivel++;

        if (monedasRecogidas == 25)
        {
            monedasRecogidas = 0;
            
            AgregarVida();
        }
        monedasText.text = "Monedas: " + monedasRecogidas;// Reinicia el contador
    }


    private void AgregarVida()
    {
        vidasActuales = Mathf.Clamp(vidasActuales + 1, 0, 99);
        vidasText.text = "Vidas Actuales: " + vidasActuales;
    }

    private void ReiniciarPosicion()
    {
        // Reinicia al jugador en un punto seguro (puedes personalizar esto)
        transform.position = new Vector3(-12.87f,0.776f, 0); // Cambia (0, 0, 0) por tu punto inicial
    }
    private void GameOver()
    {
        // Detén al jugador y carga la lógica de fin del juego
        body.velocity = Vector3.zero; // Detiene el movimiento
        Debug.Log("Mostrar pantalla de Game Over o reiniciar nivel.");
        // Aquí puedes reiniciar el nivel o cargar la pantalla de Game Over
    }

    public int getMonedas()
    {
        return monedasNivel;
    }
    
}
