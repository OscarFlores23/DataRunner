using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Sistema de Vida")]
    public int vidasMaximas = 3;           // Vidas máximas
    private int vidasActuales;             // Vidas actuales
    
    [Header("Daño")]
    public float tiempoInvulnerable = 1f;  // Tiempo invulnerable después de recibir daño
    private bool esInvulnerable = false;   // ¿Está invulnerable ahora?
     
    [Header("Respawn")]
    public Transform puntoRespawn;         // Dónde reaparece al morir
    
    [Header("Referencias")]
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    
    [Header("UI")]
    public UnityEngine.UI.Text textoVidas;  // Referencia al texto de vidas
    
    
    void Start()
    {
        vidasActuales = vidasMaximas;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        // Actualizar UI al inicio
        ActualizarUI();
        
        Debug.Log("Vidas iniciales: " + vidasActuales);
    }
    
    
    // Función para recibir daño
    public void RecibirDaño(int cantidad)
    {
        // Solo recibe daño si NO es invulnerable
        if (!esInvulnerable)
        {
            vidasActuales -= cantidad;
            
            ActualizarUI();
            Debug.Log("¡Auch! Vidas restantes: " + vidasActuales);
            
            // Activar invulnerabilidad temporal
            StartCoroutine(Invulnerabilidad());
            
            // Verificar si murió
            if (vidasActuales <= 0)
            {
                Morir();
            }
        }
    }
    
    
    // Corrutina de invulnerabilidad (parpadeo)
    System.Collections.IEnumerator Invulnerabilidad()
    {
        esInvulnerable = true;
        
        // Parpadear 5 veces
        for (int i = 0; i < 5; i++)
        {
            // Hacer invisible
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds(0.1f);
            
            // Hacer visible
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }
        
        esInvulnerable = false;
        Debug.Log("Invulnerabilidad terminada");
    }
    
    
    // Función cuando muere
    void Morir()
    {
        Debug.Log("¡Has muerto! Reiniciando nivel...");
        
        // Opción 1: Reiniciar la escena completa
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        // Opción 2: Respawn en punto específico (comentado por ahora)
        // Respawn();
    }
    
    
    // Función para respawn (reaparición)
    void Respawn()
    {
        // Restaurar vidas
        vidasActuales = vidasMaximas;
        
        ActualizarUI();
        
        // Mover al punto de respawn
        if (puntoRespawn != null)
        {
            transform.position = puntoRespawn.position;
        }
        else
        {
            // Si no hay punto, volver al origen
            transform.position = new Vector3(-3, 0, 0);
        }
        
        // Resetear velocidad
        rb.linearVelocity = Vector2.zero;
        
        Debug.Log("Respawn completado. Vidas restauradas: " + vidasActuales);
    }
    
    
    // Detectar colisión con obstáculos
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si toca un obstáculo
        if (other.CompareTag("Obstaculo"))
        {
            RecibirDaño(1);
        }
        
        // Si cae al vacío
        if (other.CompareTag("KillZone"))
        {
            Morir();
        }
    }
    // Actualizar la UI de vidas
    void ActualizarUI()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidasActuales;
        }
    }
}