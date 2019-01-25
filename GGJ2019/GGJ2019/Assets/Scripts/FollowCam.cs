using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

    public Transform Player;
    Transform Camera;

    void Awake()
    {
        Camera = Player.transform;
    }

    void LateUpdate()
    {
        transform.position = new Vector3(Player.position.x, Player.position.y, transform.position.z);
    }
}
