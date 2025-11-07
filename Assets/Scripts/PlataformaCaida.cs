using UnityEngine;
using System.Collections;

public class PlataformaCaida : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] float tiempoEspera = 2f;
    [SerializeField] float tiempoDestruccion = 3f; 

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private bool haSidoPisada = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();

        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si ya la hemos pisado, o no es el jugador, no hacemos nada
        if (haSidoPisada || collision.gameObject.name != "Jugador")
        {
            return;
        }

        foreach (ContactPoint2D contact in collision.contacts)
        {
            // 'contact.normal' apunta DESDE el jugador HACIA la plataforma.
            // Si el jugador está ENCIMA, la normal apuntará HACIA ABAJO (Y = -1).
            // Usamos -0.5f como un margen seguro.
            if (contact.normal.y < -0.5f)
            {
                StartCoroutine(SecuenciaCaida());
                break;
            }
        }
    }

    private IEnumerator SecuenciaCaida()
    {
        // Marcar como pisada para que no se active de nuevo
        haSidoPisada = true;

        yield return new WaitForSeconds(tiempoEspera);

        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;
        }

        Destroy(gameObject, tiempoDestruccion);
    }
}