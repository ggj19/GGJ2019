using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Build : MonoBehaviour
{
    public int wallCount { get; private set; }
    public int fireCount { get; private set; }
    private float tileSize;
    public GameObject wallPrefab;
    public Text UI_WallText;
    public Text UI_FireText;

    private List<GameObject> wallList;
    private bool bIsBuilding;
    private Vector3 WallPos;

    public AudioClip createBuildSound, destroyBuildSound;

    // 상위 모듈에서 초기화 한번 해주세요 (처음 보유한 횃불, 벽 개수, 타일 크기 = x,y좌표상의 길이)
    public void Init(int startFireCount, int startWallCount, float tileSize)
    {
        wallCount = startWallCount;
        this.tileSize = tileSize;
        wallList = new List<GameObject>();
        UI_WallText.text = $"{wallCount}";

        fireCount = startFireCount;
        this.tileSize = tileSize;
        UI_FireText.text = $"{wallCount}";
    }

    private Vector3 GetWallOffset(float angle)
    {
        Vector3 offset = Vector3.zero;
        if (angle == 0)
            offset = new Vector3(0, -(float)tileSize / 2, 0);
        else if (angle == 90)
            offset = new Vector3((float)tileSize / 2, 0, 0);
        else if (angle == 180)
            offset = new Vector3(0, (float)tileSize / 2, 0);
        else if (angle == 270)
            offset = new Vector3(-(float)tileSize / 2, 0);

        return offset;
    }

    // 건물 생성
    public void CreateBuild(float angle)
    {
        if (wallCount != 0 && !bIsBuilding)
        {
            bIsBuilding = true;
            
            WallPos += GetWallOffset(angle);
            for (int i=0; i<wallList.Count; i++)
            {
                if (wallList[i].gameObject.transform.position == WallPos)
                {
                    bIsBuilding = false;
                    return;
                }
            }
            GameObject wall = Instantiate(wallPrefab, WallPos, Quaternion.identity);
            float tmp_angle = 180 + angle;

            wall.transform.Rotate(new Vector3(0, 0, tmp_angle));
            wallList.Add(wall);
            // 바라보는 방향 따라 +a 해서 설치
            wallCount--;
            UI_WallText.text = $"{wallCount}";
            bIsBuilding = false;
            AudioManager.PlaySound(createBuildSound);
        }
    }

    

    private void OnTriggerStay2D(Collider2D Col)
    {
        //Debug.Log(wallConunt);
        if (Col.tag == "Tile"/* && Col.gameObject.GetComponent<SpriteRenderer>().sprite == Col.gameObject.GetComponent<Tile>().tile[0]*/)
        {
            WallPos = new Vector3(Col.transform.position.x, Col.transform.position.y, 0f);
        }

        if (Col.tag == "Wall")
        {
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown("joystick button 0"))
            {
                AudioManager.PlaySound(destroyBuildSound);

                wallList.Remove(Col.gameObject);
                Destroy(Col.gameObject);
                wallCount++;
                UI_WallText.text = $"{wallCount}";
            }
        }
    }
}
