using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que toca la moneda es el jugador
        if (other.CompareTag("Player"))
        {
            // Verifica si el jugador es el local
            Player player = other.GetComponent<Player>();
            if (player != null && player.photonView.IsMine)
            {
                player.AddScore(1); // Suma 1 al puntaje del jugador
                Destroy(gameObject); // Destruye la moneda
            }
        }
    }
}
