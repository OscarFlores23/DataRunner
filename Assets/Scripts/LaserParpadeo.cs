using UnityEngine;

public class LaserParpadeo : MonoBehaviour
{
    public float tiempoEncendido = 1f;
    public float tiempoApagado = 1f;
    
    private SpriteRenderer sprite;
    private BoxCollider2D col;
    private Color colorOriginal;
    
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
        colorOriginal = sprite.color;
        
        StartCoroutine(Parpadear());
    }
    
    System.Collections.IEnumerator Parpadear()
    {
        while (true)
        {
            // Encendido
            sprite.color = colorOriginal;
            col.enabled = true;
            yield return new WaitForSeconds(tiempoEncendido);
            
            // Apagado (transparente)
            Color transparente = colorOriginal;
            transparente.a = 0.2f;
            sprite.color = transparente;
            col.enabled = false;
            yield return new WaitForSeconds(tiempoApagado);
        }
    }
}