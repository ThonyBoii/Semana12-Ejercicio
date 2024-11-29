using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviourPun
{
    [SerializeField] private int score = 0; 

    public void AddPoints(int points)
    {
        if (photonView.IsMine) 
        {
            score += points;
            Debug.Log($"Puntos: {score}");
            FindObjectOfType<UIManager>().UpdateScore(score); 
        }
    }
}
