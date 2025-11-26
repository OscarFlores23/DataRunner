using UnityEngine;

public class EnemigoPatrulla : MonoBehaviour
{
    [Header("Patrulla")]
    public float velocidad = 2f;
    public float distanciaPatrulla = 5f;
    
    private Vector3 puntoInicio;
    private bool moviendoDerecha = true;
    private SpriteRenderer sprite;
    
    void Start()
    {
        puntoInicio = transform.position;
        sprite = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        if (moviendoDerecha)
        {
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);
            
            if (transform.position.x >= puntoInicio.x + distanciaPatrulla)
            {
                moviendoDerecha = false;
                if (sprite != null) sprite.flipX = true;
            }
        }
        else
        {
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);
            
            if (transform.position.x <= puntoInicio.x - distanciaPatrulla)
            {
                moviendoDerecha = true;
                if (sprite != null) sprite.flipX = false;
            }
        }
    }
}