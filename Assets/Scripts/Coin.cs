using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que toca la moneda es el jugador
        if (other.CompareTag("Player"))
        {
            // Busca el componente Player en el jugador
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.AddScore(1); // Suma 1 al puntaje del jugador
                Destroy(gameObject); // Destruye la moneda
            }
        }
    }
}
