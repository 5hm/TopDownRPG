using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    //대화 데이터를 저장
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;
    // Start is called before the first frame update
    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    // Update is called once per frame
    void GenerateData()
    {
        //Talk Data
        //NPC1 : 1000 , NPC2 : 2000
        //box : 3000 , table : 4000
        //water : 5000 , house : 6000
        talkData.Add(1000, new string[] {"안녕?:0", "여긴 처음이지?:1"});
        talkData.Add(100, new string[] { "평범한 나무상자다." });
        talkData.Add(200, new string[] { "뭐지 이건" });
        talkData.Add(300, new string[] { "여긴 물이야 못들어가" });
        talkData.Add(400, new string[] { "우리집인가?" });
        talkData.Add(2000, new string[] { "던전이얌:2", "들어갈래?:3" });

        //Quest Talk
        talkData.Add(10 + 1000, new string[] {"어서 와 :2" ,
                                              "이 마을에 혼걸 환영해! :3",
                                              "좀 둘러봐 : 2"});
        talkData.Add(11 + 2000, new string[] {"ㅎㅇ :3" ,
                                              "여기 들어가려고? :3",
                                              "안돼 ㅡㅡ : 0",
                                              "내가 잃어버린 물건을 찾아줘 :2",
                                              "그럼 비켜줄게 :1"});

        talkData.Add(20 + 1000, new string[] { "루도의 동전? :3",
                                               "내가 알겠니? :0"});
        talkData.Add(20 + 2000, new string[] { "어디서 잃어버린거지? :0" });

        talkData.Add(20 + 5000, new string[] { "루도의 동전을 찾았다!" });

        talkData.Add(21 + 2000, new string[] { "오 :2",
                                               "ㄳ :2"});

        //Portrait Data
        //0 -> Angry , 1 -> Idle , 2 -> Smile , 3 -> Talk
        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1000 + 1, portraitArr[1]);
        portraitData.Add(1000 + 2, portraitArr[2]);
        portraitData.Add(1000 + 3, portraitArr[3]);

        portraitData.Add(2000 + 0, portraitArr[4]);
        portraitData.Add(2000 + 1, portraitArr[5]);
        portraitData.Add(2000 + 2, portraitArr[6]);
        portraitData.Add(2000 + 3, portraitArr[7]);

    }

   
    public string GetTalk(int id, int talkIndex)
    {
        //id가 없으면 퀘스트 대화순서 제거 후 재탐색
        if (!talkData.ContainsKey(id))
        {
           if(!talkData.ContainsKey(id - id % 10))
            {
                return GetTalk(id - id%100, talkIndex); // Get First Talk
            }
            else
            {
                return GetTalk(id - id%10, talkIndex); // Get First Quest Talk
            }
        }

        if (talkIndex == talkData[id].Length)
         return null; 
        
        else
         return talkData[id][talkIndex]; 
        
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
