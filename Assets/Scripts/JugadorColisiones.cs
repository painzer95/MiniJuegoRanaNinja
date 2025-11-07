using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JugadorColisiones : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textoManzanasGUI;

    JugadorMovimiento jugadorMovimiento;
    Sonidos sonidosScript;

    public int contadorManzanas { get; private set; }

    void Start()
    {
        jugadorMovimiento = GetComponent<JugadorMovimiento>();
        sonidosScript = GetComponent<Sonidos>();

        // Inicializamos el contador de manzanas.
        contadorManzanas = GameObject.FindGameObjectsWithTag("Manzana").Length;
        // Actualizamos el texto en pantalla.
        textoManzanasGUI.text = "" + contadorManzanas;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Manzana"))
        {
            if (sonidosScript != null)
            {
                sonidosScript.ReproducirSonidoRecompensa();
            }

            // Incrementamos el contador de manzanas.
            contadorManzanas--;
            // Actualizamos el texto en pantalla.
            textoManzanasGUI.text = "" + contadorManzanas;

            // Destruimos la manzana al recogerla.
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("CorazonExtra"))
        {
            if (jugadorMovimiento != null)
            {
                jugadorMovimiento.GanarVida(1);

                if (sonidosScript != null)
                {
                    sonidosScript.ReproducirSonidoRecompensa();
                }

                // Destruimos el corazón.
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("BoostCorrer"))
        {
            if (jugadorMovimiento != null)
            {
                jugadorMovimiento.ActivarBoostVelocidad();

                if (sonidosScript != null)
                {
                    sonidosScript.ReproducirSonidoRecompensa();
                }

                // Destruimos el corazón.
                Destroy(collision.gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("BoostSaltar"))
        {
            if (jugadorMovimiento != null)
            {
                jugadorMovimiento.ActivarBoostSalto();

                if (sonidosScript != null)
                {
                    sonidosScript.ReproducirSonidoRecompensa();
                }

                // Destruimos el corazón.
                Destroy(collision.gameObject);
            }
        }


        if (contadorManzanas == 0)
        {
            // Si no quedan manzanas por recoger completamos el nivel.
            Invoke(nameof(NivelCompletado), 1f);
        }
    }

    void NivelCompletado()
    {
        SceneManager.LoadScene("MenuCompletado");
    }
}
