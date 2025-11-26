using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;  // Singleton (solo una instancia)
    
    void Awake()
    {
        // Asegurar que solo haya un GameManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Función para ganar el nivel
    public void Victoria()
    {
        Debug.Log("¡¡¡VICTORIA!!! ¡Has completado el nivel!");
        
        // Detener el juego por 2 segundos para que vean el mensaje
        Time.timeScale = 0f;
        
        // Aquí más adelante pondremos pantalla de victoria
    }
    
    // Función para reiniciar el nivel
    public void ReiniciarNivel()
    {
        Time.timeScale = 1f;  // Reanudar tiempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}