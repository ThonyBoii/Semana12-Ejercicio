using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Pun.UtilityScripts;

public class Player : MonoBehaviourPun
{
    private static GameObject localInstance;

    [SerializeField] private TextMeshPro playerNameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    private Rigidbody rb;
    [SerializeField] private float speed;

    private int score = 0; 
    private const int winScore = 5;

    public static GameObject LocalInstance { get { return localInstance;  } }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerNameText.text = GameData.playerName;
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName);
            localInstance = gameObject;
        }
        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();
    }

    [PunRPC]
    private void SetName(string playerName)
    {
        playerNameText.text = GameData.playerName;
    }

    void Update()
    {
        if (!photonView.IsMine || !PhotonNetwork.IsConnected)
        {
            return;
        }
        Move();
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, vertical * speed);

        if(horizontal!=0|| vertical != 0)
        {
            transform.forward = new Vector3(horizontal, 0, vertical);
        }

    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        CheckWinCondition();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    private void CheckWinCondition()
    {
        if (score >= winScore)
        {
            photonView.RPC("LoadVictoryScene", RpcTarget.All); // Sincronizar escena para todos los jugadores
        }
    }

    [PunRPC]
    private void LoadVictoryScene()
    {
        PhotonNetwork.LoadLevel("VictoryScene"); 
    }
}
