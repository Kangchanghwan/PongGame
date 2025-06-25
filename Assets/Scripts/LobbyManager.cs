using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{
    // 플레이어 목록과 준비 여부를 표시할 UI 텍스트
    public Text lobbyText;

    // 게임을 시작하기 위해 필요한 최소한의 준비된 플레이어 수
    private const int MinimumReadyCountToStartGame = 2;

    // 플레이어 준비 상태를 저장하는 딕셔너리
    private Dictionary<ulong, bool> _clientReadyStates
        = new Dictionary<ulong, bool>();

    // OnNetworkSpawn은 NetworkBehaviour가 생성될 때 호출됨
    public override void OnNetworkSpawn()
    {

    }

    private void OnDisable()
    {

    }

    // 클라이언트가 연결되었을때 실행할 콜백
    private void OnClientConnected(ulong clientId)
    {

    }

    // 클라이언트가 씬 로드를 완료했을 때 실행할 콜백
    private void OnClientSceneLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
    {

    }

    // 클라이언트 접속이 끊겼을 때 실행할 콜백
    private void OnClientDisconnected(ulong clientId)
    {

    }

    // 서버가 다른 클라이언트들에게 어떤 클라이언트의 준비 상태가 변경됨을 알리는 RPC 메서드
    // 서버에서 요청되어 각 클라이언트들에서 실행됨
    [ClientRpc]
    private void SetClientIsReadyClientRpc(ulong clientId, bool isReady)
    {

    }

    // 서버가 다른 클라이언트들에게 어떤 클라이언트의 준비 상태가 변경됨을 알리는 RPC 메서드
    // 서버에서 요청되어 각 클라이언트들에서 실행됨
    [ClientRpc]
    private void RemovePlayerClientRpc(ulong clientId)
    {

    }

    // 로비 텍스트를 갱신
    private void UpdateLobbyText()
    {

    }

    // 게임을 시작할 수 있는지 확인
    private bool CheckIsReadyToStart()
    {
        return true;
    }

    // 게임 시작
    private void StartGame()
    {

    }

    // 클라이언트가 준비 버튼을 눌렀을때 실행하는 메서드
    public void SetPlayerIsReady()
    {

    }

    // 클라이언트가 준비 상태가 변경됬음을 서버에게 알리기 위한 RPC 메서드
    // 클라이언트에서 요청되어 서버에서 실행됨
    [ServerRpc(RequireOwnership = false)]
    private void SetClientIsReadyServerRpc(ulong clientId, bool isReady)
    {

    }
}