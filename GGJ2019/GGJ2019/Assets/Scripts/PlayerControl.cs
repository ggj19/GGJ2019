using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public float moveSpeed = 0.2f;
	//public float restartLevelDelay = 1; // 스테이지 넘어갈 때 딜레이 시간 1초

    public GameObject Research;
    public GameObject Nothing;
    public GameObject p_sprite;

    float moveX;
    float moveY;
    float angle = 0f;   // 플레이어 시점

    Animator animator;
    Vector3 objectScale;

    Build build;

    void Awake() {
        animator = GetComponent<Animator>();
        build = GetComponent<Build>();
        build.Init(10, 1.28f);
    }

    void FixedUpdate() {
        MoveControl();
    }

    void MoveControl()
    {
        // 건물 짓기
        BuildControl();
        
        // hp 줄어듬
        Global.Hungry -= 1 * Time.deltaTime;
        if (Global.Hungry <= 0) Global.Status = 2; // GameEnd

        moveX = moveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal"); // Raw를 쓰면 가속도 적용 x
        moveY = moveSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

		if (moveX != 0 || moveY != 0)

        transform.Translate(moveX,moveY,0);

        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        viewPos.x = Mathf.Clamp01(viewPos.x);
        viewPos.y = Mathf.Clamp01(viewPos.y);

        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewPos);

        transform.position = worldPos;
        

        if (moveY > 0 && moveX == 0) angle = 180f;
        if (moveY < 0 && moveX == 0) angle = 0f;
        if (moveX < 0) angle = 270f;
        if (moveX > 0) angle = 90f;
    
        //animator.SetTrigger("Player_Left");
        //p_sprite.flipX = true;

        p_sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void BuildControl()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            build.CreateBuild(angle);
        }
    }

    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Col.tag == "Food")
        {
            Global.Hungry = 100f;
            Col.gameObject.SetActive(false);
        }
    }

    void OnTriggerStay2D(Collider2D Col)
    {
        /*if (Col.tag == "Furniture")
            Research.SetActive(true);
		if (Input.GetButtonDown ("Fire1")) {
            if (Col.name == "Blanket" || Col.name == "Blanket(Clone)")
	            Nothing.SetActive(true);
		}*/
    }

    void OnTriggerExit2D(Collider2D Col)
    {
        Research.SetActive(false);
        Nothing.SetActive(false);
    }
}
