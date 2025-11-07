using UnityEngine;
using UnityEngine.SceneManagement;

public class JugadorColisionTrampa : MonoBehaviour
{
    JugadorMovimiento jugadorMovimiento;
    Rigidbody2D rigidbody2D;
    CapsuleCollider2D capsuleCollider2D;

    float fuerza = 20f;
    private bool estaInmune = false;

    void Start()
    {
        jugadorMovimiento = GetComponent<JugadorMovimiento>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int vidasRestantes = jugadorMovimiento.VidasRestantes;
        Debug.Log(
            "Tocando algo = " +
            collision.gameObject.name + " y " +
            collision.gameObject.tag + " y " +
            collision.gameObject.layer
            );

        // Si el jugador no tiene el script de movimiento activo o está inmune, no hacemos nada.
        if (jugadorMovimiento == null || !jugadorMovimiento.enabled || estaInmune)
        {
            return;
        }

        if (
            collision.gameObject.tag == "Trampa" ||
            collision.gameObject.tag == "Enemigo"
            )
        {
            // Hacemos al jugador inmune temporalmente.
            estaInmune = true;

            // Restamos una vida al jugador.
            jugadorMovimiento.RecibirDano(1);

            vidasRestantes = jugadorMovimiento.VidasRestantes;

            if (vidasRestantes > 0)
            {
                // Aplicamos un empujón hacia arriba
                rigidbody2D.AddForce(transform.up * fuerza, ForceMode2D.Impulse);

                // Reseteamos la inmunidad después de 0.5 segundos.
                Invoke(nameof(ResetearInmunidad), 0.5f);
            }
            else if (vidasRestantes <= 0)
            {
                // Desactivamos el movimiento del jugador y el collider.
                jugadorMovimiento.enabled = false;
                capsuleCollider2D.enabled = false;

                // Desactivamos el script de colisión.
                this.enabled = false;

                // Esperamos 1 segundo y cargamos el menú de muerte.
                Invoke(nameof(MenuMuerte), 1f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "LimiteAbismo")
        {
            // Desactivamos el movimiento del jugador y el collider.
            jugadorMovimiento.enabled = false;
            capsuleCollider2D.enabled = false;

            // Desactivamos el script de colisión.
            this.enabled = false;

            // Esperamos 1 segundo y cargamos el menú de muerte.
            Invoke(nameof(MenuMuerte), 0.5f);
        }
    }

    void ResetearInmunidad()
    {
        estaInmune = false;
    }

    public void MenuMuerte()
    {
        SceneManager.LoadScene("MenuMuerte");
    }
}
