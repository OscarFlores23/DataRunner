using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{
    [Header("Botones del Menú")]
    public Button botonJugar;
    public Button botonSalir;
    
    void Start()
    {
        // Conectar funciones a los botones automáticamente
        if (botonJugar != null)
        {
            botonJugar.onClick.AddListener(Jugar);
            Debug.Log("Botón JUGAR conectado");
        }
        else
        {
            Debug.LogError("¡Falta asignar el botón JUGAR!");
        }
        
        if (botonSalir != null)
        {
            botonSalir.onClick.AddListener(Salir);
            Debug.Log("Botón SALIR conectado");
        }
        else
        {
            Debug.LogError("¡Falta asignar el botón SALIR!");
        }
    }
    
    void Jugar()
    {
        Debug.Log("¡Cargando el nivel!");
        SceneManager.LoadScene("Level1");
    }
    
    void Salir()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}