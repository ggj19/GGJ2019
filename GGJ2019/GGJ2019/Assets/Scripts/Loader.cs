// Loader //
using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{
    public Transform Player;
    Transform Camera;

    public GameObject gameManager;
    // Use this for initialization
    void Awake()
    {
        Camera = Player.transform;

        if (GameManager.instance == null) // GameManager가 null이면, 게임 매니저 프리펩을 인스턴스화 함
            Instantiate(gameManager);
    }

    void LateUpdate()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y, transform.position.z);
    }
} // 완료되면 하이라카에서 게임매니저를 지워도 됨.