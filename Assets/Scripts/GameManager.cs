using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text talkText;
    public Text questText;
    public GameObject scanObject;
    public GameObject talkPanel;
    public GameObject menuSet;
    public GameObject player;
    public bool is_Action;
    public TalkManager talkManager;
    public int talkIndex;
    public Image portraiting;
    public QuestManager questManager;

    void Start()
    {
        GameLoad();
        questText.text = (questManager.CheckQuest());
    }

    void Update()
    {
        //Sub Menu
        if (Input.GetButtonDown("Cancel"))
        {
            SubMenuActive();
        }
    }
    public void SubMenuActive()
    {
        if (menuSet.activeSelf)
        {
            menuSet.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            menuSet.SetActive(true);
            Time.timeScale = 0;
        }

    }
    public void Action(GameObject scanObj)
    {

        //Get Current Object
        scanObject = scanObj;
        ObjData objData = scanObj.GetComponent<ObjData>();
        Talk(objData.id, objData.is_Npc);

        //Visible Talk for Action
        talkPanel.SetActive(is_Action);
    }

    void Talk(int id, bool is_Npc)
    {
        //Set Talk Data
        int questTalkIndex = questManager.GetQuestTalkIndex(id);
        //퀘스트번호 + NPC id = 퀘스트 대화 데이터 id
        string talkData = talkManager.GetTalk(id + questTalkIndex,
                                                    talkIndex);


        //End Talk
        if (talkData == null)
        {
            is_Action = false;
            talkIndex = 0;
            questText.text = questManager.CheckQuest(id);
            return;
        }

        //Continue Talk
        if (is_Npc)
        {
            talkText.text = talkData.Split(":")[0];

            portraiting.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraiting.color = new Color(1, 1, 1, 1);
        }
        else
        {
            talkText.text = talkData;
            portraiting.color = new Color(1, 1, 1, 0);
        }

        is_Action = true;
        talkIndex++;
    }
    public void GameContinue()
    {
        Time.timeScale = 1;
    }

    public void GameSave()
    {
        //player x
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        //player y
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        //Quest Id
        PlayerPrefs.SetInt("QuestId", questManager.questId);
        //Quest Action Index
        PlayerPrefs.SetInt("QuestActionINdex", questManager.questActionIndex);

        menuSet.SetActive(false);
        Time.timeScale = 1;
    }

    public void GameLoad()
    {
        if (!PlayerPrefs.HasKey("PlayerX"))
        {
            return;
        }
        float x = PlayerPrefs.GetFloat("PlayerX");
        float y = PlayerPrefs.GetFloat("PlayerY");
        int questId = PlayerPrefs.GetInt("QuestId");
        int questActionINdex = PlayerPrefs.GetInt("QuestActionINdex");

        player.transform.position = new Vector3(x, y, 0);
        questManager.questId = questId;
        questManager.questActionIndex = questActionINdex;
        questManager.ControlObject();
    }

    public void GameReSet()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void GameExit()
    {
        //최상위 클래스
        Application.Quit();
    }

}
