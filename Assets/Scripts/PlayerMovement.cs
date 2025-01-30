using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   private Rigidbody2D body;
   [SerializeField] private float speed;
   [SerializeField] private float jumpforce;
   private bool piso;
   private Animator anim;
   private PlayerController playerController; // Referencia al PlayerController
   private bool nivelCompletado = false;


    private void Awake() {
      body = GetComponent<Rigidbody2D>();
      anim =GetComponent<Animator>();
      body.constraints = RigidbodyConstraints2D.FreezeRotation;

      playerController = GetComponent<PlayerController>();
    }

   /// <summary>
   /// Update is called every frame, if the MonoBehaviour is enabled.
   /// </summary>
   void Update()
   {
      float HorizontalInput=Input.GetAxis("Horizontal");
       body.velocity = new Vector2(HorizontalInput*speed,body.velocity.y);
       if(HorizontalInput>0.01f){
         transform.localScale = new Vector3(1.5f,1.5f,1.5f);;
       }
        if(HorizontalInput < -0.01f){
         transform.localScale =new Vector3(-1.5f,1.5f,1.5f);
       }
      if(Input.GetKey(KeyCode.Space) && piso)
      {
       Jump();
      }

        if (!IsVisibleFrom(Camera.main))
        {
            nivelCompletado = true; // Marca el nivel como completado
            Debug.Log("Jugador salió de la cámara. Nivel completado.");

            LevelManager.Instance.SetCompletitud(1.0f); // Completitud = 1 (nivel completado).

            int monedas = playerController.getMonedas(); // Obtener monedas desde PlayerController
            LevelManager.Instance.SetMonedasNivel(monedas);

            TerminarNivel();
            SiguienteNivel(); // Llama al siguiente nivel
        }


        anim.SetBool("Run",HorizontalInput!=0);
      anim.SetBool("piso",piso);
   }


    void TerminarNivel()
    {
        // Calcula el fitness antes de exportar
        LevelManager.Instance.CalcularFitness(2f, 1.7f, 1.5f, 1.2f, 1.0f); // Ajusta los pesos y Eopt

        // Exporta los datos del nivel
        LevelManager.Instance.ExportarDatosNivel();

        Debug.Log("Nivel terminado. Datos exportados.");
    }

    private void Jump()
    {
      body.velocity = new Vector2(body.velocity.x,jumpforce);
      piso = false;
    }

   /// <summary>
   /// Sent when an incoming collider makes contact with this object's
   /// collider (2D physics only).
   /// </summary>
   /// <param name="other">The Collision2D data associated with this collision.</param>
   void OnCollisionEnter2D(Collision2D collision)
   {
      if(collision.gameObject.tag == "Piso")
      {
            piso = true;
      }
   }


   // Método para verificar si el jugador está visible en la cámara
private bool IsVisibleFrom(Camera cam)
{
    // Calcula los planos de visión de la cámara
    var planes = GeometryUtility.CalculateFrustumPlanes(cam);
    // Verifica si el jugador está dentro de los planos de visión
    return GeometryUtility.TestPlanesAABB(planes, GetComponent<Collider2D>().bounds);
}

// Método para manejar el paso al siguiente nivel
private void SiguienteNivel()
{
        Debug.Log("¡Generando el siguiente nivel!");
        transform.position = new Vector3(-12.87f, 0.776f, 0);

        // Incrementar el nivel y cargar el siguiente
        LevelManager.Instance.NextLevel();
        LevelLoader.Instance.LoadLevelFromJson("level"); // Cargar el siguiente nivel
    }
}
