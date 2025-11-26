using UnityEngine;

public class MetaTrigger : MonoBehaviour
{
    // Efecto visual al llegar
    public Color colorVictoria = Color.yellow;
    private SpriteRenderer sprite;
    private bool yaGano = false;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si el jugador toca la meta
        if (other.CompareTag("Player") && !yaGano)
        {
            yaGano = true;
            
            // Efecto visual
            sprite.color = colorVictoria;
            
            // Llamar al GameManager
            if (GameManager.instance != null)
            {
                GameManager.instance.Victoria();
            }
            
            Debug.Log("¡Jugador llegó a la meta!");
        }
    }
}