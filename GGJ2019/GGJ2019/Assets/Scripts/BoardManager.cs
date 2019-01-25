﻿// BoardManager 1 //
using UnityEngine;
using System.Collections;
using System; // 직렬화 (Serializable) 속성들을 사용할 수 있다.
using System.Collections.Generic;  // List를 사용할 수 있다.
using Random = UnityEngine.Random; // System과 Unity Engine에 모두 랜덤이 있기 때문

public class BoardManager : MonoBehaviour {
    [Serializable]   // Count라는 Serializable(직렬화) public class 선언
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)  // 값 할당을 위한 생성자
        {
            minimum = min;
            maximum = max;
        }
    }
    public int columns = 8;     // 열을 위한 정수 (우리가 원하는 공간을 그림(가로 크기조정))
    public int rows = 8;         // 행을 위한 정수 (세로 크기조정)
    public int Rand_Max = 0;
    //public Count TileCount = new Count(1, 7); // Count를 사용해 레벨마다 얼마나 많은 벽을 랜덤하게 생성할지 범위를 특정

    // 오브젝트를 불러옴
    public GameObject[] Tile;
    //public GameObject[] wallTiles;  // 인스펙터에서 선택하게 될 여러 프리팹들로 각각의 배열을 채움

    private Transform boardHolder;  // Hierarchy를 깨끗이 하기위해 사용(죄다 boardHolder 자식으로 집어넣음)
    private List<Vector3> gridPositions = new List<Vector3>();
    // 3차원 배열리스트, 게임판 위에 가능한 모든 다른 위치들을 추적하기 위해 사용하며, 
    // 해당 장소에 있는지 없는지 추적하는데 사용


    // UI
    public GameObject UI_Hungry;

/* 리스트 초기화 */

    void InitializeList()  
    {
        //gridPositions.Clear();  // 리스트 내부의 clear 함수를 써서, 모든 리스트된 gridPosition을 초기화함
        for (int x = 1; x < columns - 1; x++) 
        { // 게임상에서 벽이나 적, 아이템들이 있을 수 있는 가능한 모든 위치를 만듬
            for (int y = 1; y < rows - 1; y++)  // -1한 이유는 가장자리 때문
            { 
                gridPositions.Add(new Vector3(x, y, 1f));  // 벡터 3를 격자형 위치 리스트에 더함
            }
        }
    }

    /* void를 리턴하는 private 함수인 BoardSetup()선언, 바깥 벽과 게임 보드의 바닥을 짓기 위해 사용 */
    void BoardSetup() 
    { 
        boardHolder = new GameObject("Board").transform; 
         // boardHolder를 Boad라는 새로운 게임 오브젝트의 Transform과 동일하게 하고 시작
        for (int x = -1*columns/2; x < columns/2 + 1; x++) // -1과 +1은 바닥과 바깥벽을 구분하기 위함
        {
            for (int y = -1* rows/2; y < rows/2 + 1; y++)
            {
                int rand = Random.Range(0, Rand_Max);
                GameObject instance = Instantiate(Tile[rand], new Vector3(x*1.28f, y*1.28f, 10f), Quaternion.identity) as GameObject; // 바닥 타일 소환
                  // instance라 불리는 게임 오브젝트 타입을 선언하고, 인스턴스화 하려는 오브젝트를 할당,
                  //   따라서 Instatiate함수를 불러오고, 우리가 고른 프리펩인 toInstantiate를 넣어 현재 루프의 
                  //   x와 y 위치 값이 있는 new Vector3를 더함
                instance.transform.SetParent(boardHolder);
                  // 새로 생성된 인스턴스의 부모 오브젝트를 boardHolder로 해줌
            }
        }
    }

    /* Vector3를 리턴하는 새로운 함수 */
    Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);  // randomIndex를 선언하고, Range 내에 0부터 gridPositions 리스트의 위치 값 개수만큼 랜덤 값을 설정
        Vector3 randomPosition = gridPositions[randomIndex];     // gridPositions 리스트에서 랜덤하게 선택된 인덱스에 있는 격자 위치 값과 동일하게
        gridPositions.RemoveAt(randomIndex);                     // 같은 장소에 두 개 이상의 오브젝트를 만드는 것을 방지, 사용한 격자 위치를 리스트에서 제거
        return randomPosition;
    }

    /* 선택한 장소에 실제로 타일을 소환 */
    void LayoutObjectAtRandom(GameObject tileArray, int minimum, int maximum)   // 세가지 파라미트 [게임 오브젝트 배열 tileArray, minimum, maximum]
    {
        int objectCount = Random.Range(minimum, maximum + 1);                   // 주어진 오브젝트를 얼마나 스폰할지 조정 ex)한 레벨 벽 개수
        
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();  // 랜덤 위치를 선택
            Instantiate(tileArray, randomPosition, Quaternion.identity);        // 우리가 선택한 랜덤위치에 인스턴스화 함
        }
    }

    // void Start()와 void Update()를 지우고 public void SetupScene 선언
    /* 유일한 public 함수, 실제로 게임보드가 만들어질 때 게임매니저에 의해 호출*/
    public void SetupScene(int level)
    {
        BoardSetup();  // 신을 구성하기 위해 호출
        InitializeList();


        /*Instantiate(Refrigerator, new Vector3(-1, rows, 0f), Quaternion.identity);
        LayoutObjectAtRandom(WasteBasket, 1, 1);*/
        //LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
        //int enemyCount = (int)Mathf.Log(level, 2f);
        // 레벨에 따라 적 생성, 로그함수를 사용 ex) 1스테이지 : 1마리, 4스테이지 : 2마리, 8스테이지 : 3마리
    }

    void Update()
    {
        UI_Hungry.transform.localScale = new Vector3(Global.Hungry/100f, 1f, 1f);
    }
}