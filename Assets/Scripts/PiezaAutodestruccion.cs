using UnityEngine;

public class PiezaAutodestruccion : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 4f);
    }
}