using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ControladorPausa : MonoBehaviour
{
    [SerializeField] GameObject panelDePausa;

    InputAction actionPausa;

    private bool estaPausado = false;

    void Start()
    {
        actionPausa = InputSystem.actions.FindAction("Escape");
        if (actionPausa != null)
        {
            actionPausa.Enable();
        }
        else
        {
            Debug.LogError("No se encontró la acción 'Pause'. Revisa tu Input Action Asset.");
        }

        ContinuarJuego();
    }

    void Update()
    {
        // Si pulsamos Escape.
        if (actionPausa != null && actionPausa.WasPressedThisFrame())
        {
            if (estaPausado)
            {
                ContinuarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void ReiniciarNivel()
    {
        SceneManager.LoadScene(JugadorMovimiento.NivelActualNombre);
    }

    public void ContinuarJuego()
    {
        panelDePausa.SetActive(false); // Oculta el menú.
        Time.timeScale = 1f; // Reanuda el tiempo.
        estaPausado = false;
    }

    public void PausarJuego()
    {
        panelDePausa.SetActive(true); // Muestra el menú.
        Time.timeScale = 0f; // Congela el juego.
        estaPausado = true;
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
