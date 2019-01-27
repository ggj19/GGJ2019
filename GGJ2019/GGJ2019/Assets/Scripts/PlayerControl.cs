using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{

    public float moveSpeed = 0.2f;
    //public float restartLevelDelay = 1; // 스테이지 넘어갈 때 딜레이 시간 1초

    public GameObject Research;
    public GameObject Nothing;
    public GameObject p_sprite;
    public GameObject Map;
    public GameObject GameOver;
    public GameObject Torchlight;

    // UI
    public GameObject UI_Hungry;
    public GameObject UI_Time;
    public Text timeText;

    float moveX;
    float moveY;
    float angle = 0f;   // 플레이어 시점
    float playtime = 0f;    // 플레이 시간
    float second = 0f;

    //bool isPause = false;

    Animator animator;
    Vector3 objectScale;

    Build build;

    private int torchCount;
    public Text torchCountText;

    public AudioClip moveSoundOnWood1, moveSoundOnWood2;
    public AudioClip moveSoundOnGrass1, moveSoundOnGrass2;
    public AudioClip buildCreateSound, buildDestroySound;
    public AudioClip openMapSound, closeMapSound;
    public AudioClip throwTorchlightSound;
    public AudioClip healSound;
    public AudioClip gameOverSound;

    public AudioClip zombieSound, projectileSound;
    AudioClip CurrentStep;

    bool isStep = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
        build = GetComponent<Build>();
        build.Init(1, 10, 1.28f);
        Global.Status = 0;
        Global.Hungry = 100f;
        Time.timeScale = 1;
        torchCount = 5;
        torchCountText.text = $"{torchCount}";
        CurrentStep = moveSoundOnWood1;
        StartCoroutine(PlayEffectSound());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown("joystick button 3"))
        {
            if (Time.timeScale != 0f)
            {
                AudioManager.PlaySound(openMapSound);
                Map.gameObject.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                AudioManager.PlaySound(closeMapSound);
                Map.gameObject.SetActive(false);
                Time.timeScale = 1;
            }
        }
    }

    void FixedUpdate()
    {
        if (Global.Status == 2)
        {
            AudioManager.PlaySound(gameOverSound);
            Global.Status = 3;
            GameOver.gameObject.SetActive(true);
            GameOver.gameObject.GetComponent<GameOverUI>().SetGameOverTime(playtime);
        }
        else if (Global.Status != 3 && Global.Status != 2)
        {
            playtime += Time.deltaTime;
            // hp 줄어듬
            Global.Hungry -= Time.deltaTime;
            if (Global.Hungry <= 0) Global.Status = 2;
            if (UI_Hungry)
            {
                float hungry = (Global.Hungry / 100f >= 100f) ? 100f : ((Global.Hungry / 100f <= 0) ? 0 : Global.Hungry / 100f);
                UI_Hungry.transform.localScale = new Vector3(hungry, 1f, 1f);
            }

            second += Time.deltaTime;

            UI_Time.gameObject.GetComponent<Text>().text = string.Format("{0:#00}:{1:00}.{2:00}",
            Mathf.Floor(second / 60),           //minutes
            Mathf.Floor(second) % 60,           // seconds
            Mathf.Floor((second * 100) % 100));     // miliseconds

            MoveControl();
        }
    }

    void MoveControl()
    {
        // 건물 짓기
        KeyControl();

        moveX = moveSpeed * Time.deltaTime * Input.GetAxisRaw("Horizontal"); // Raw를 쓰면 가속도 적용 x
        moveY = moveSpeed * Time.deltaTime * Input.GetAxisRaw("Vertical");

        if (moveX != 0 || moveY != 0)
        {
            transform.Translate(moveX, moveY, 0);
            if (!isStep)
            {
                isStep = true;
                StartCoroutine("StepSound");
            }
        }
        else
        {
            isStep = false;
            StopCoroutine("StepSound");
        }

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

    void KeyControl()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown("joystick button 2"))
        {
            build.CreateBuild(angle);
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown("joystick button 1"))
        {
            if (Global.Status != 2)
            {
                if (torchCount > 0)
                {
                    AudioManager.PlaySound(throwTorchlightSound);

                    GameObject Torch = Instantiate(Torchlight, gameObject.transform.position, Quaternion.identity);
                    Torch.gameObject.GetComponent<Torchlight>().Throw(gameObject.transform.position, angle);
                    torchCount--;
                    torchCountText.text = $"{torchCount}";
                    //AudioManager.PlaySound()
                }
            }
        }
    }

    IEnumerator PlayEffectSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1, 5));
            /*if (Random.Range(0, 2) == 0)
                AudioManager.PlaySound(zombieSound);
            else
                AudioManager.PlaySound(projectileSound);*/
        }
    }

    IEnumerator StepSound()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            AudioManager.PlaySound(CurrentStep);
        }
    }


    void OnTriggerEnter2D(Collider2D Col)
    {
        if (Global.Status != 2 && Global.Status != 3)
        {
            if (Col.tag == "Tile")
            {
                if (Col.gameObject.GetComponent<SpriteRenderer>().sprite == Col.gameObject.GetComponent<Tile>().tile[0])
                    CurrentStep = moveSoundOnWood1;
                else CurrentStep = moveSoundOnGrass1;
            }
            if (Col.tag == "Food")
            {
                Global.Hungry = 100f;
                AudioManager.PlaySound(healSound);
                Destroy(Col.gameObject);
            }

            if (Col.tag == "Monster")
            {
                Global.Status = 2;
            }
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
