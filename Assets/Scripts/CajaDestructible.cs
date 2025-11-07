using UnityEngine;
using System.Collections;

public class CajaDestructible : MonoBehaviour
{
    [Header("Efectos de Destrucción")]
    [SerializeField] GameObject prefabPiezaRota;
    [SerializeField] Sprite[] spritesDePiezas = new Sprite[4];
    [SerializeField] AudioClip sonidoRomper;
    [SerializeField] float fuerzaExplosion = 5f;
    
    private Animator animator;
    private AudioSource audioSource;
    private Collider2D colliderPrincipal;
    private bool estaRota = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        colliderPrincipal = GetComponent<Collider2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name != "Jugador" && estaRota)
        {
            return;
        }

        ContactPoint2D contact = collision.contacts[0];

        // 'contact.normal.y' > 0.5f significa que el golpe fue
        // principalmente en la superficie inferior de la caja.
        if (contact.normal.y > 0.5f)
        {
            // ¡Iniciamos la secuencia!
            StartCoroutine(RomperCaja());
        }
    }

    private IEnumerator RomperCaja()
    {
        estaRota = true;
        colliderPrincipal.enabled = false;

        if (animator != null)
        {
            animator.Play("CajaGolpear");
        }

        yield return new WaitForSeconds(1f);

        if (sonidoRomper != null)
        {
            audioSource.PlayOneShot(sonidoRomper);
        }

        for (int i = 0; i < spritesDePiezas.Length; i++)
        {
            // Creamos una instancia de la pieza
            GameObject pieza = Instantiate(prefabPiezaRota, transform.position, Quaternion.identity);

            // Le asignamos el sprite correspondiente (0, 1, 2, 3)
            pieza.GetComponent<SpriteRenderer>().sprite = spritesDePiezas[i];

            // Le damos la fuerza de explosión
            Rigidbody2D rbPieza = pieza.GetComponent<Rigidbody2D>();
            if (rbPieza != null)
            {
                // Dirección aleatoria hacia arriba y los lados
                Vector2 direccion = new Vector2(Random.Range(-1f, 1f), Random.Range(0.5f, 1f)).normalized;

                rbPieza.bodyType = RigidbodyType2D.Dynamic;
                rbPieza.AddForce(direccion * fuerzaExplosion, ForceMode2D.Impulse);
            }
        }

        Destroy(gameObject);
    }
}