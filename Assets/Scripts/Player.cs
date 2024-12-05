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

    public static GameObject LocalInstance { get { return localInstance; } }

    private void Awake()
    {
        if (photonView.IsMine)
        {
            playerNameText.text = GameData.playerName;
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName);
            localInstance = gameObject;
        }
        else
        {
            photonView.RPC("SetName", RpcTarget.AllBuffered, GameData.playerName);  // Asegura que otros jugadores reciban el nombre
        }

        DontDestroyOnLoad(gameObject);
        rb = GetComponent<Rigidbody>();

        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();

        if (scoreText != null)
        {
            scoreText.text = "Score: 0"; 
        }
    }

    [PunRPC]
    private void SetName(string playerName)
    {
        playerNameText.text = playerName;
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

        float rotationInput = Input.GetAxisRaw("Horizontal"); 
        float movementInput = Input.GetAxisRaw("Vertical"); 

        
        transform.Rotate(0, rotationInput * speed * Time.deltaTime, 0);

        
        Vector3 movement = transform.forward * movementInput * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);

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
            photonView.RPC("LoadVictoryScene", RpcTarget.All); 
        }
    }

    [PunRPC]
    private void LoadVictoryScene()
    {
        PhotonNetwork.LoadLevel("VictoryScene"); 
    }
}
