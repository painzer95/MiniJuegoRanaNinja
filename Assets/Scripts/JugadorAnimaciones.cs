using UnityEngine;

public class JugadorAnimaciones : MonoBehaviour
{
    Animator animator;
    SpriteRenderer sprite;
    JugadorMovimiento jugadorMovimiento;

    private bool estaMuerto = false;
    private bool estaSaltando = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        jugadorMovimiento = GetComponent<JugadorMovimiento>();
    }

    void Update()
    {
        Animaciones();
    }

    void Animaciones()
    {
        bool estaEnElSuelo = jugadorMovimiento.estaEnElSuelo();
        float movimiento = jugadorMovimiento.InputHorizontal;
        float velocidadVertical = jugadorMovimiento.VelocidadVertical;
        int saltosRestantes = jugadorMovimiento.SaltosRestantes;
        int vidasRestantes = jugadorMovimiento.VidasRestantes;

        if (vidasRestantes <= 0 && jugadorMovimiento.AcabaDeRecibirDano)
        {
            if (!estaMuerto)
            {
                animator.Play("JugadorMuere");
                estaMuerto = true;

                return;
            }
        }
        else if (vidasRestantes > 0 && jugadorMovimiento.AcabaDeRecibirDano)
        {
            animator.Play("JugadorDanho", 0, 0f);

            jugadorMovimiento.ResetearDanho();
        }
        
        if (!estaMuerto &&
            !animator.GetCurrentAnimatorStateInfo(0).IsName("JugadorDanho"))
        {
            if (!estaEnElSuelo)
            {
                if (velocidadVertical < -0.1f)
                {
                    animator.Play("JugadorCae");
                    estaSaltando = false;
                }
                else if (velocidadVertical > 0.1f)
                {
                        estaSaltando = true;

                        // Si quedan 1 salto, es el 1er salto.
                        if (saltosRestantes == 1)
                        {
                            animator.Play("JugadorSalta");
                            //Debug.Log("PRIMER SALTO. Quedan: " + (saltosRestantes));
                        }
                        // Si quedan 0 saltos, es el 2do salto.
                        else if (saltosRestantes == 0)
                        {
                            animator.Play("JugadorSaltaDoble");
                            //Debug.Log("SEGUNDO SALTO. Quedan: " + (saltosRestantes));
                        }

                    if (movimiento > 0)
                    {
                        sprite.flipX = false;
                    }
                    else if (movimiento < 0)
                    {
                        sprite.flipX = true;
                    }
                }
            }
            else
            {
                estaSaltando = false;

                if (movimiento > 0)
                {
                    animator.Play("JugadorCorre");
                    sprite.flipX = false;
                }
                else if (movimiento < 0)
                {
                    animator.Play("JugadorCorre");
                    sprite.flipX = true;
                }
                else
                {
                    animator.Play("JugadorQuieto");
                }
            }
        }
    }
}
