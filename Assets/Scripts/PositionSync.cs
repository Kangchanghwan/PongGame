using Unity.Netcode;
using UnityEngine;

// 플레이어의 위치를 동기화하기 위한 컴포넌트
public class PositionSync : NetworkBehaviour
{
    private Vector2 _lastPosition; // 마지막으로 동기화된 위치
    // 위치값 동기화를 위한 네트워크 변수
    public NetworkVariable<Vector2> networkPosition 
        = new NetworkVariable<Vector2>(
            readPerm: NetworkVariableReadPermission.Everyone,
            writePerm: NetworkVariableWritePermission.Owner);

    private void FixedUpdate()
    {
        if (IsOwner)
        {
            if (Vector2.Distance(_lastPosition, transform.position) > 0.001f)
            {
                _lastPosition = transform.position;
                networkPosition.Value =  _lastPosition;
            }
        }
        else
        {
            transform.position = networkPosition.Value;
        }
    }
}