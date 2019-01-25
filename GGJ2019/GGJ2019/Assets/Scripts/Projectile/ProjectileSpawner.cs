using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어에게 부착해주세요
public class ProjectileSpawner : MonoBehaviour
{
    public GameObject projectilePrefab;
    // 한 변 길이
    [SerializeField] private int length;
    [SerializeField] private int minPower, maxPower;
    [SerializeField] private float spawnCycleTime;

    private void Awake()
    {
        Init(Vector3.zero);
    }

    public void Init(Vector3 playerPos)
    {
        StartCoroutine(SpawnProjectiles(playerPos));
    }

    private int GetRandomLength()
    {
        return Random.Range(-length, length) / 2;
    }
    private Vector2 GetRandomPos()
    {
        if (Random.Range(0, 2) == 0)
        {
            if (Random.Range(0, 2) == 0)
            {
                return new Vector2(transform.position.x + length / 2, transform.position.y + GetRandomLength());
            }
            return new Vector2(transform.position.x - length / 2, transform.position.y + GetRandomLength());
        }
        else
        {
            if (Random.Range(0, 2) == 0)
            {
                return new Vector2(transform.position.x + GetRandomLength(), transform.position.y + length / 2);
            }
            return new Vector2(transform.position.x + GetRandomLength(), transform.position.y - length / 2);
        }
    }

    private void Launch()
    {
        GameObject projecile = Instantiate(projectilePrefab, GetRandomPos(), Quaternion.identity);
        int random = Random.Range(minPower, maxPower);
        var direction = transform.position - projecile.transform.position;
        projecile.GetComponent<Rigidbody2D>()?.AddForce(direction * random);
    }

    private IEnumerator SpawnProjectiles(Vector2 playerPos)
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnCycleTime);
            Launch();
        }
    }
}
