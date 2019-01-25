using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Build : MonoBehaviour
{
    public int wallConunt { get; private set; }
    private int tileSize;
    public GameObject wallPrefab;

    private List<GameObject> wallList;
    private bool bIsBuilding;

    // 상위 모듈에서 초기화 한번 해주세요 (처음 보유한 벽 개수, 타일 크기 = x,y좌표상의 길이)
    public void Init(int startWallCount, int tileSize)
    {
        wallConunt = startWallCount;
        this.tileSize = tileSize;
        wallList = new List<GameObject>();
    }

    // 플레이어가 위치한 타일 위치 반환
    private Vector2 FindTileCenterPos(Vector2 playerPos)
    {
        int x = GetPos((int)Mathf.Abs(playerPos.x));
        int y = GetPos((int)Mathf.Abs(playerPos.y));

        return new Vector2(x, y) * playerPos.normalized;
    }

    private int GetPos(int pos)
    {
        var interval = (float)tileSize / 2;
        for (int i = 0; i < 1000; i++)
        {
            if (interval + i * tileSize >= pos)
            {
                return tileSize * i;
            }
        }
        return 0;
    }

    private Vector2 GetWallOffset(float angle)
    {
        Vector2 offset = Vector2.zero;
        if (angle == 0)
            offset = new Vector2(0, -(float)tileSize / 2);
        else if (angle == 90)
            offset = new Vector2((float)tileSize / 2, 0);
        else if (angle == 180)
            offset = new Vector2(0, (float)tileSize / 2);
        else if (angle == 270)
            offset = new Vector2(-(float)tileSize / 2, 0);

        return offset;
    }

    // 건물 생성
    public void CreateBuild(Vector2 playerPos, float angle)
    {
        if (wallConunt != 0 && !bIsBuilding)
        {
            bIsBuilding = true;
            wallConunt--;
            GameObject wall = Instantiate(wallPrefab, FindTileCenterPos(playerPos), Quaternion.identity);
            wall.transform.Translate(GetWallOffset(angle));
            wall.transform.Rotate(new Vector3(0, 0, angle));
            wallList.Add(wall);
            // 바라보는 방향 따라 +a 해서 설치
            bIsBuilding = false;
        }
    }
    
    // 건물 파괴
    public void DestroyBuild(Vector2 playerPos, float angle)
    {
        Vector3 wallPos = FindTileCenterPos(playerPos) + GetWallOffset(angle);

        GameObject wall = wallList.Where(w => w.transform.position == wallPos).FirstOrDefault();
        if (wall)
        {
            Destroy(wall);
            wallList.Remove(wall);
            wallConunt++;
        }
    }
}
