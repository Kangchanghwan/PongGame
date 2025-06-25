using Unity.Netcode;
using UnityEngine;

// 플레이어로서 공을 받아내는 패들(판때기) 구현
public class PlayerPaddle : NetworkBehaviour
{
    // 패들 색상을 변경하기 위한 컴포넌트
    private SpriteRenderer _spriteRenderer;
    public float speed = 10f; // 이동 속도

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 패들 색상을 변경
    [ClientRpc]
    public void SetRendererColorClientRpc(Color color)
    {
        _spriteRenderer.color = color;
    }

    // 패들 위치를 변경
    [ClientRpc]
    public void SpawnToPositionClientRpc(Vector3 position)
    {
        transform.position = position;
    }

    private void Update()
    {
        // 게임이 활성화 안된 상태에서는 이동 처리를 하지 않음
        if (GameManager.Instance != null 
            && !GameManager.Instance.IsGameActive)
        {
            return;
        }
    }
}
