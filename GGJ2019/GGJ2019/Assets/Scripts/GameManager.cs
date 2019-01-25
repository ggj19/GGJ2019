// GameManager //
using UnityEngine;
using System.Collections;
using System.Collections.Generic; // 적을 계속 추적하기 위해 사용할 리스트 자료 구조를 사용할 수 있게함

public class GameManager : MonoBehaviour {
    public float turnDelay = .1f; // 턴 사이에 게임이 얼마동안 대기할지 나타냄
    public static GameManager instance = null; 
      // 클래스 바깥에서도 접근가능하며, static은 변수가 인스턴스가 아니라 클래스 그 자체에 속해있다는 것
      //   => 전역 변수, 어떤 스크립트에서도 접근가능
    public BoardManager boardScript;
    [HideInInspector] // 변수가 public이지만 에디터에서는 숨김

    private int level = 1; // 적이 2레벨부터 등장하기 때문에 3

	// Use this for initialization
    void Awake () { // Start()를 Awake()로 바꿈
        if (instance == null) 
            instance = this; // 자기 자신의 위치 반환
        else if (instance != this)  // null도 this도 아닐 경우
            Destroy(gameObject); // 실수로 2개가 안생기게 방지
        DontDestroyOnLoad(gameObject); // 새로운 신을 로드할 때 점수가 사라지지 않게 함
        boardScript = GetComponent<BoardManager>();
          // 콜바이레퍼런스 : 값 복사가 아닌 실제 오브젝트 자체를 들고 옴. 
          //   보드매니저 스크립트로 컴포넌트들을 레퍼런스로 들고와 저장
        InitGame();
    }

 /* boardScript의 SetupScene 함수를 불러옴 */
    void InitGame () { 
        boardScript.SetupScene(level);
    }
}