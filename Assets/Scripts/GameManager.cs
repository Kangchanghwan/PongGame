using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    // 싱글톤 구현
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    
    private static GameManager instance;
    
    // 게임이 진행 중인지 여부
    public bool IsGameActive { get; private set; }
    
    // 점수 표시할 UI 텍스트
    public Text scoreText;
    
    // 플레이어 색상, 스폰 위치
    public Color[] playerColors = new Color[2];
    public Transform[] spawnPositionTransforms = new Transform[2];

    // 공 프리팹
    public GameObject ballPrefab;

    // 게임 오버 텍스트와 게임 오버시 표시할 패널
    public Text gameoverText;
    public GameObject gameoverPanel;

    // 플레이어 번호와 클라이언트 ID를 맵핑하는 딕셔너리
    private Dictionary<int, ulong> playerNumberClientIdMap 
        = new Dictionary<int, ulong>();
    
    // 플레이어 점수를 저장하는 배열
    private int[] playerScores = new int[2];
    
    // 승리 도달 점수
    private const int WinScore = 11;

    // 처음 활성화시 게임을 시작하는 처리를 실행
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayer();
            SpawnBall();
        }
        IsGameActive = true;
        gameoverPanel.SetActive(false);
        UpdateScoreTextClientRpc(0, 0);
        NetworkManager.OnClientConnectedCallback += OnClientDisconnected;
    }
    
    public override void OnNetworkDespawn()
    {
        NetworkManager.OnClientConnectedCallback -= OnClientDisconnected;
    }

    // 게임 도중 클라이언트가 나갔을때 실행
    private void OnClientDisconnected(ulong clinetId)
    {
        if (IsGameActive)
        {
            ExitGame();
        }
    }

    // 플레이어들을 스폰
    private void SpawnPlayer()
    {
        if (NetworkManager.ConnectedClientsList.Count != 2)
        {
            Debug.LogError("Network clients can only be spawned once.");
            return;
        }

        var playerPrefab = NetworkManager.NetworkConfig.PlayerPrefab;

        for (int i = 0; i < 2; i++)
        {
            NetworkClient client = NetworkManager.ConnectedClientsList[i];
            playerNumberClientIdMap[i] = client.ClientId;

            var spawnPosition = spawnPositionTransforms[i].position;
            var playerColor = playerColors[i];
            
            var playerGameObject = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            var playerPaddle = playerGameObject.GetComponent<PlayerPaddle>();
            playerPaddle.NetworkObject.SpawnAsPlayerObject(client.ClientId);
            playerPaddle.SpawnToPositionClientRpc(spawnPosition);
            playerPaddle.SetRendererColorClientRpc(playerColor);
        }
    }

    // 공 생성
    private void SpawnBall()
    {
        var ballGameObject = Instantiate(ballPrefab, Vector2.zero, Quaternion.identity);
        var ball = ballGameObject.GetComponent<Ball>();
        ball.NetworkObject.Spawn();
    }

    // 점수 추가
    public void AddScore(int playerNumber, int score)
    {
      playerScores[playerNumber] += score;
      UpdateScoreTextClientRpc(playerScores[0], playerScores[1]);

      if (playerScores[playerNumber] >= WinScore)
      {
          var winnerId = playerNumberClientIdMap[playerNumber];
          EndGame(winnerId);
      }
    }

    // 점수 텍스트 갱신
    [ClientRpc]
    private void UpdateScoreTextClientRpc(int player0Score, int player1Score)
    {
        scoreText.text = $"{player0Score} : {player1Score}";
    }
    
    // 서버에서 실행할 게임 종료
    public void EndGame(ulong winnerId)
    {
        if (!IsServer)
        {
            return;
        }
        
        var ball =FindObjectOfType<Ball>();
        ball.NetworkObject.Despawn();
        
        EndGameClientRpc(winnerId);
    }
    
    // 클라이언트에서 실행할 게임 종료 처리
    [ClientRpc]
    public void EndGameClientRpc(ulong winnerId)
    {
        IsGameActive = false;

        if (winnerId == NetworkManager.LocalClientId)
        {
            gameoverText.text = "You won!";
        }
        else
        {
            gameoverText.text = "You lost!";
        }
        
        gameoverPanel.SetActive(true);
    }
    
    // 게임과 네트워크 연결을 종료하고 메뉴 화면으로 돌아감
    public void ExitGame()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Menu");
    }
}