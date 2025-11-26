using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Seguimiento del Jugador")]
    public Transform jugador;              // Referencia al jugador
    public float suavizado = 0.125f;       // Qué tan suave sigue la cámara
    public Vector3 offset;                 // Distancia entre cámara y jugador
    
    void Start()
    {
        // Si no se asigna offset manualmente, usar la distancia inicial
        if (offset == Vector3.zero)
        {
            offset = transform.position - jugador.position;
        }
    }
    
    void LateUpdate()
    {
        // Calcular posición deseada
        Vector3 posicionDeseada = jugador.position + offset;
        
        // Interpolar suavemente entre posición actual y deseada
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, suavizado);
        
        // Aplicar la nueva posición
        transform.position = posicionSuavizada;
    }
}
