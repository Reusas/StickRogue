using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class Flag : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] GameObject boss;
    [SerializeField] Transform bossSpawn;
    public static bool bossIsAlive;
    [SerializeField] Transform flag;
    [SerializeField] Transform flag2;
    [SerializeField] TMP_Text flagInfoTxt;
    Animator textAnim;
    
    [SerializeField] float timeToTakeDown = 60f;
    bool showInfo = false;
    bool canTakeDown = false;
    bool isTakingDown = false;
    bool isRising = false;
    bool complete;
    Vector2 startFlagPos;
    Vector2 finalFlagPos;
    public float t;
    float progress;
    
    void Awake()
    {
        textAnim = flagInfoTxt.GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.transform.tag == "Player")
        {
            Debug.Log("Test");
            if(!isRising && !isTakingDown && !complete)
            {
                FlagInfo();
            }
            canTakeDown = true;
            if(isRising && !complete|| isTakingDown&& !complete)
            {
                textAnim.SetBool("Flash",false);
            }
            
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.transform.tag == "Player")
        {
            if(!isRising && !isTakingDown && !complete)
            {
                FlagInfo();
            }

            if(isRising && !complete|| isTakingDown&& !complete)
            {
                textAnim.SetBool("Flash",true);
            }

            canTakeDown = false;

        }
    }

    void FlagInfo()
    {
        showInfo = !showInfo;
        flagInfoTxt.gameObject.SetActive(showInfo);
    }

    void Update()
    {
        if(isTakingDown && canTakeDown)
        {
            t+= 1/timeToTakeDown * Time.deltaTime;

            flag.localPosition = Vector2.Lerp(startFlagPos,finalFlagPos,t);
            if(t>=1)
            {
                isTakingDown = false;
                t=0;
                flag.gameObject.SetActive(false);
                flag2.gameObject.SetActive(true);
                startFlagPos = flag2.localPosition;
                finalFlagPos = new Vector2(startFlagPos.x,startFlagPos.y + 1);
                isRising = true;
            }
            progress = Mathf.FloorToInt(t * 50);
            flagInfoTxt.text = "Raising flag " + progress + "%";
        }

        if(isRising && canTakeDown)
        {
            if(bossIsAlive)
            {
                flagInfoTxt.text = "Deafeat boss to finish raising flag!";
                return;
            }
            t+= 1/timeToTakeDown * Time.deltaTime;
            flag2.localPosition = Vector2.Lerp(startFlagPos,finalFlagPos,t);
            progress = 50 + Mathf.FloorToInt(t * 50);
            flagInfoTxt.text = "Raising flag " + progress + "%";
            if(t>=1)
            {
                isRising = false;
                canTakeDown = true;
                Debug.Log("Completed");
                complete = true;
                flagInfoTxt.text = "Area complete! Tap to leave";

            }

        }

        


    }


    public void OnPointerDown(PointerEventData eventData)
    {
        if(canTakeDown && !isTakingDown && !isRising && !complete)
        {
            isTakingDown = true;
            startFlagPos = flag.localPosition;
            finalFlagPos = new Vector2(startFlagPos.x,startFlagPos.y - 1);
            SpawnBoss();

        }

        if(complete && canTakeDown)
        {
            GameManager.gameManagerSingleton.loadNextLevel();
            Debug.Log("Leave stage kek");
        }
    }

    void SpawnBoss()
    {
        bossIsAlive = true;
        Instantiate(boss,bossSpawn.position,boss.transform.rotation);
    }
}
