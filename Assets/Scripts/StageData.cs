using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

[System.Serializable]
public class ScoreData
{
    public int stageNum;
    public List<string> name;
    public List<float> score;
}

[System.Serializable]
public class GameData
{
    public List<ScoreData> data;
}

public class StageData : MonoBehaviour
{
    public static StageData instance;

    public GameData gameData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        // 데이터를 초기화하거나 로드하는 로직
        LoadData();

        // JSON 데이터를 읽어와서 파싱
        string jsonText = File.ReadAllText(Application.dataPath + "/GameData.json");
        gameData = JsonUtility.FromJson<GameData>(jsonText);

        // 데이터를 사용하거나 수정하는 로직
    }

    [ContextMenu("From Json Data")]
    private void LoadData()
    {
        // JSON 데이터를 읽어와서 파싱
        string jsonText = File.ReadAllText(Application.dataPath + "/GameData.json");
        gameData = JsonUtility.FromJson<GameData>(jsonText);

        // Refresh Text
        // RankManager.instance.
    }

    [ContextMenu("To Json Data")]
    private void SaveData()
    {
        // 데이터를 저장하는 로직

        // JSON으로 직렬화
        string jsonText = JsonUtility.ToJson(gameData, true);

        // JSON 데이터를 파일로 저장
        File.WriteAllText(Application.dataPath + "/GameData.json", jsonText);
    }

    public ScoreData GetStageModify(int stageNum)
    {
        return gameData.data.Find(data => data.stageNum == stageNum);
    }

    public void AddNewScore(int stageNum, string playerName, float newScore)
    {
        // 해당 스테이지의 데이터를 찾아옴
        ScoreData stageToModify = gameData.data.Find(data => data.stageNum == stageNum);

        if (stageToModify != null)
        {
            int insertIndex = -1;

            // 순위에 따라 데이터 정렬 (높은 점수 순서)
            for (int i = 0; i < stageToModify.score.Count; i++)
            {
                if (newScore > stageToModify.score[i])
                {
                    insertIndex = i;
                    break;
                }
            }

            // 새 데이터를 추가하거나 기존 데이터를 미뤄놓음
            if (insertIndex >= 0)
            {
                stageToModify.name.Insert(insertIndex, playerName);
                stageToModify.score.Insert(insertIndex, newScore);
            }
            else
            {
                stageToModify.name.Add(playerName);
                stageToModify.score.Add(newScore);
            }

            // 상위 5개 데이터만 유지
            int numToKeep = Mathf.Min(stageToModify.score.Count, 5);
            stageToModify.name = stageToModify.name.GetRange(0, numToKeep);
            stageToModify.score = stageToModify.score.GetRange(0, numToKeep);

            // 순위 데이터 로드
            for (int i = 0; i < numToKeep; i++)
            {
                RankManager.instance.LoadScoreDataText(stageNum, i);
                // print($"{stageToModify}, {i}, {stageToModify.name[i]}, {stageToModify.score[i]}");
            }

            SaveData();
        }
    }


    public bool CheckNewScore(int stageNum, float newScore)
    {
        // 스테이지 데이터를 찾습니다.
        ScoreData stageToModify = GetStageModify(stageNum);

        if (stageToModify != null)
        {
            // 저장된 점수 목록을 순회하며 newScore와 비교합니다.
            foreach (float score in stageToModify.score)
            {
                if (newScore > score)
                {
                    // newScore가 현재 비교 중인 점수보다 높으면 true를 반환합니다.
                    return true;
                }
            }
        }

        // 모든 비교가 끝난 후에도 높은 점수가 없다면 false를 반환합니다.
        return false;
    }
}
