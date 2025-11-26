using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Sistema de Vida")]
    public int vidasMaximas = 3;
    private int vidasActuales;
    
    [Header("Daño")]
    public float tiempoInvulnerable = 1f;
    private bool esInvulnerable = false;
    
    [Header("Respawn")]
    public Transform puntoRespawn;
    
    [Header("UI - Corazones")]
    public GameObject[] corazones; // Array de imágenes de corazones
    
    [Header("Referencias")]
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    
    
    void Start()
    {
        vidasActuales = vidasMaximas;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        ActualizarUI();
        Debug.Log("Vidas iniciales: " + vidasActuales);
    }
    
    
    public void RecibirDaño(int cantidad)
    {
        if (!esInvulnerable)
        {
            vidasActuales -= cantidad;
            
            Debug.Log("DAÑO RECIBIDO - Vidas actuales: " + vidasActuales); // DEBUG
            
            ActualizarUI(); // Actualizar UI INMEDIATAMENTE
            
            StartCoroutine(Invulnerabilidad());
            
            if (vidasActuales <= 0)
            {
                Morir();
            }
        }
    }
    
    
    System.Collections.IEnumerator Invulnerabilidad()
    {
        esInvulnerable = true;
        
        for (int i = 0; i < 5; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.1f);
        }
        
        esInvulnerable = false;
        Debug.Log("Invulnerabilidad terminada");
    }
    
    
    void Morir()
    {
        Debug.Log("¡Has muerto! Reiniciando nivel...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    
    void Respawn()
    {
        vidasActuales = vidasMaximas;
        
        if (puntoRespawn != null)
        {
            transform.position = puntoRespawn.position;
        }
        else
        {
            transform.position = new Vector3(-3, 0, 0);
        }
        
        rb.linearVelocity = Vector2.zero;
        ActualizarUI();
        
        Debug.Log("Respawn completado. Vidas restauradas: " + vidasActuales);
    }
    
    
    // FUNCIÓN ACTUALIZAR UI - VERSIÓN CON CORAZONES
    void ActualizarUI()
    {
        Debug.Log("Actualizando UI - Vidas: " + vidasActuales); // DEBUG
        
        if (corazones != null && corazones.Length > 0)
        {
            for (int i = 0; i < corazones.Length; i++)
            {
                if (corazones[i] != null)
                {
                    if (i < vidasActuales)
                    {
                        corazones[i].SetActive(true); // Mostrar corazón
                    }
                    else
                    {
                        corazones[i].SetActive(false); // Ocultar corazón
                    }
                    
                    Debug.Log("Corazón " + i + " estado: " + corazones[i].activeSelf); // DEBUG
                }
            }
        }
        else
        {
            Debug.LogWarning("Array de corazones está vacío o es null!");
        }
    }
    
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstaculo"))
        {
            Debug.Log("Colisión con obstáculo detectada"); // DEBUG
            RecibirDaño(1);
        }
        
        if (other.CompareTag("KillZone"))
        {
            Morir();
        }
    }
}