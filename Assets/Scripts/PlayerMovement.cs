using UnityEngine;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    // ========== VARIABLES ==========
    // Variables de movimiento horizontal
    [Header("Movimiento Horizontal")]
    public float velocidadMovimiento = 7f;  // Qué tan rápido camina
    
    // Variables de salto
    [Header("Salto")]
    public float fuerzaSalto = 15f;         // Qué tan alto salta
    public int saltosMaximos = 1;           // Cantidad de saltos (doble salto)
    private int saltosRestantes;             // Cuántos saltos le quedan
    
    // Detección de suelo
    [Header("Detección de Suelo")]
    public Transform piesDelJugador;         // Punto donde están los pies
    public float radioDeteccion = 0.2f;      // Tamaño del área de detección
    public LayerMask capaSuelo;              // Qué objetos son "suelo"
    
    // Variables de Dash
    [Header("Dash Cibernético")]
    public float fuerzaDash = 20f;           // Qué tan lejos se teletransporta
    public float duracionDash = 0.2f;        // Cuánto dura el dash (en segundos)
    public float tiempoCooldown = 2f;        // Tiempo de espera entre dashes
    
    private bool estaDasheando = false;      // ¿Está haciendo dash ahora?
    private bool puedeHacerDash = true;      // ¿Puede hacer dash?
    private float direccionDash;             // Dirección del dash (-1 izquierda, 1 derecha)

    // Efecto visual del Dash
    [Header("Efecto Visual Dash")]
    public Color colorNormal = Color.cyan;      // Color normal
    public Color colorDash = Color.yellow;      // Color durante dash
    private SpriteRenderer spriteRenderer;      // Componente que controla el color

    // Referencias a componentes
    private Rigidbody2D rb;                  // El componente de física
    private bool estaEnSuelo;                // ¿Está tocando el suelo?
    
    // Referencias
    private Animator animator;
    
    
    // ========== INICIO ==========
    void Start()
    {
        // Obtener el Rigidbody2D del personaje
        rb = GetComponent<Rigidbody2D>();
        
        // Al inicio, tiene todos los saltos disponibles
        saltosRestantes = saltosMaximos;
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        //FUNCIÓN PARA LA ANIMACION DEL JUGADOR
        animator = GetComponent<Animator>();

    }
    
    
    // ========== ACTUALIZACIÓN CADA FRAME ==========
    void Update()
    {
        // Verificar si está en el suelo
        DetectarSuelo();
        
        // Si NO está dasheando, puede moverse normalmente
        if (!estaDasheando)
        {
            // Leer input del jugador y mover
            Mover();
        }
        
        // Detectar si presiona tecla de salto
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            Saltar();
        }
        
        // Detectar si presiona Q para hacer Dash
        if (Input.GetKeyDown(KeyCode.Q) && puedeHacerDash)
        {
            IniciarDash();
        }
    }
    
    // ========== FUNCIÓN: DETECTAR SUELO ==========
    void DetectarSuelo()
    {
        // Crear un círculo invisible en los pies del jugador
        // Si ese círculo toca algo de la capa "suelo", entonces está en el suelo
        estaEnSuelo = Physics2D.OverlapCircle(piesDelJugador.position, radioDeteccion, capaSuelo);
        
        // Si está en el suelo, recuperar todos los saltos
        if (estaEnSuelo)
        {
            saltosRestantes = saltosMaximos;
        }
    }
    
    
    // ========== FUNCIÓN: MOVER ==========
    void Mover()
    {
        // Leer input horizontal (A/D o Flechas)
        // Esto da: -1 si presiona izquierda, +1 si presiona derecha, 0 si no presiona nada
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        
        // Aplicar movimiento al Rigidbody
        // rb.velocity.y = mantiene la velocidad vertical (gravedad)
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.linearVelocity.y);
        
        if (animator != null)
        {
            animator.SetFloat("Velocidad", Mathf.Abs(rb.linearVelocity.x));
        }
    }
    
    
    // ========== FUNCIÓN: SALTAR ==========
    void Saltar()
    {
        // Solo puede saltar si le quedan saltos disponibles
        if (saltosRestantes > 0)
        {
            // Resetear la velocidad vertical (para saltos consistentes)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            
            // Aplicar fuerza hacia arriba
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            
            // Reducir un salto
            saltosRestantes--;
            
            // Mensaje en consola (para debugging)
            Debug.Log("¡Salto! Saltos restantes: " + saltosRestantes);
        }
    }
    
    // ========== FUNCIÓN: INICIAR DASH ==========
    void IniciarDash()
    {
        // Determinar dirección del dash
        // Si está presionando derecha (D), dash a la derecha
        // Si está presionando izquierda (A), dash a la izquierda
        // Si no presiona nada, dash hacia donde está mirando
        float inputHorizontal = Input.GetAxis("Horizontal");
        
        if (inputHorizontal != 0)
        {
            // Dash en la dirección que está presionando
            direccionDash = Mathf.Sign(inputHorizontal); // -1 o 1
        }
        else
        {
            // Si no presiona nada, dash a la derecha por defecto
            direccionDash = 1f;
        }
        
        // Activar el dash
        estaDasheando = true;
        puedeHacerDash = false;
        spriteRenderer.color = colorDash;

        // Aplicar la velocidad del dash
        rb.linearVelocity = new Vector2(direccionDash * fuerzaDash, 0f);
        
        // Iniciar las corrutinas (funciones con tiempo)
        StartCoroutine(TerminarDash());
        StartCoroutine(ReiniciarCooldownDash());
        
        // Mensaje de debug
        Debug.Log("¡DASH CIBERNÉTICO ACTIVADO! Dirección: " + direccionDash);
    }
    
    
    // ========== CORRUTINA: TERMINAR DASH ==========
    // Esta función espera un tiempo y luego termina el dash
    IEnumerator TerminarDash()
    {
        // Esperar el tiempo de duración del dash
        yield return new WaitForSeconds(duracionDash);
        
        // Terminar el dash
        estaDasheando = false;
        
        // Reducir la velocidad al terminar (para que no siga volando)
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
        spriteRenderer.color = colorNormal;
        Debug.Log("Dash terminado");
    }
    
    
    // ========== CORRUTINA: REINICIAR COOLDOWN ==========
    // Esta función espera el tiempo de cooldown antes de permitir otro dash
    IEnumerator ReiniciarCooldownDash()
    {
        // Esperar el tiempo de cooldown
        yield return new WaitForSeconds(tiempoCooldown);
        
        // Permitir hacer dash nuevamente
        puedeHacerDash = true;
        
        Debug.Log("Dash disponible de nuevo");
    }
    
    // ========== VISUALIZACIÓN EN EDITOR ==========
    // Esto dibuja el círculo de detección en el editor (solo para que lo veas)
    void OnDrawGizmosSelected()
    {
        if (piesDelJugador != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(piesDelJugador.position, radioDeteccion);
        }
    }
}