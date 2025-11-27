using UnityEngine;

public class EnemigoMirarJugador : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;
    
    private SpriteRenderer sprite;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        
        // Buscar jugador automáticamente
        if (jugador == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                jugador = playerObj.transform;
            }
        }
    }
    
    void Update()
    {
        if (jugador != null)
        {
            MirarAlJugador();
        }
    }
    
    void MirarAlJugador()
    {
        // Si el jugador está a la derecha
        if (jugador.position.x > transform.position.x)
        {
            sprite.flipX = false;  // Mirar derecha
        }
        else
        {
            sprite.flipX = true;   // Mirar izquierda
        }
    }
}