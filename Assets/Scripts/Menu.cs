using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // Botón iniciar el juego.
    public void IniciarJuego()
    {
        SceneManager.LoadScene("Nivel_01");
    }

    public void SiguienteNivel()
    {
        if ((JugadorMovimiento.NivelActualIndex + 1) < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(JugadorMovimiento.NivelActualIndex + 1);
        }
        else
        {
            SceneManager.LoadScene("MenuPrincipal");
        }
    }

    // Botón reiniciar el nivel.
    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(JugadorMovimiento.NivelActualNombre);
    }

    // Botón volver al menú principal.
    public void VolverAlMenuPrincipal()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    // Botón añadir un botón para salir
    public void SalirDelJuego()
    {
        Debug.Log("SALIENDO DEL JUEGO...");
        Application.Quit();
    }
}