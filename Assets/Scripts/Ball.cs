using Unity.Netcode;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    private Vector2 direction; // 이동 방향
    private const float StartSpeed = 3f; // 최초 이동 속도
    private const float MaxSpeed = 15f; // 최대 이동 속도
    private const float AdditionalSpeedPerHit = 0.2f; // 충돌시 추가되는 속도
    private float currentSpeed = StartSpeed; // 현재 속도

    // 최초 공의 방향을 결정
    public override void OnNetworkSpawn()
    {
        if (!IsServer)
        {
            return;
        }

        // 게임 시작시 공의 이동 방향은
        // 무작위성이 조금 추가된 왼쪽 방향
        direction =
            (Vector2.left + Random.insideUnitCircle).normalized;
    }

    private void FixedUpdate()
    {
        // 서버가 아니거나 게임이 종료된 경우 이동 처리를 하지 않음
        if (!IsServer || !GameManager.Instance.IsGameActive)
        {
            return;
        }

        var distance = currentSpeed * Time.deltaTime;
        var hit = Physics2D.Raycast(transform.position, direction, distance);

        if (hit.collider == null)
        {
            transform.position += (Vector3)direction * distance;
        }
        else if (hit.collider.tag == "ScoringZone")
        {
            if (hit.point.x < 0f)
            {
                GameManager.Instance.AddScore(1, 1);
                direction = (Vector2.left + Random.insideUnitCircle).normalized;
            }
            else
            {
                GameManager.Instance.AddScore(0, 1);
                direction = (Vector2.right + Random.insideUnitCircle).normalized;
            }

            transform.position = new Vector3(0f, Random.Range(-3f, 3f), 0f);
            currentSpeed = StartSpeed;
        }
        else
        {
            transform.position = hit.point;
            distance -= hit.distance;
            direction = Vector2.Reflect(direction, hit.normal);
            direction = (direction + Random.insideUnitCircle * 0.05f).normalized;
            transform.position += (Vector3)direction * distance;
            currentSpeed = Mathf.Min(currentSpeed + AdditionalSpeedPerHit, MaxSpeed);
        }
    }
}