using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Linq;

public class RankManager : MonoBehaviour
{
    public static RankManager instance;

    public RectTransform rectTr;
    public Vector2 min;
    public Vector2 max;

    [Header("Anim Speed")]
    public float moveWidthSpeed = 0.25f;
    public float moveHeightSpeed = 0.25f;

    [Header("Rank parent object")]
    public GameObject rankObj;

    [Header("Rank child objects")]
    public GameObject[] rankObjs;
    public float showObjTimer = 0.25f;

    [Header("Scene Build Index")]
    [Tooltip("You must enter an index indicating \"Stage1 Build index number\"")]
    public int minBuildNum = 3;
    [Tooltip("You must enter an index indicating \"Stage9 Build index number\"")]
    public int maxBuildNum = 11;

    [HideInInspector]
    public Scene currentScene;
    // [HideInInspector]
    public int currentSceneNum;
    int stageNum;

    public List<TextMeshProUGUI> nameText;
    public List<TextMeshProUGUI> scoreText;

    public GameObject rankInsertObj;
    public TextMeshProUGUI rankInsertNameText;
    public TextMeshProUGUI rankShowScoreText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            rankObj.SetActive(false);
            rankInsertObj.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnEnable()
    {
        InitializeSceneData();
    }

    public void CallEnable()
    {
        InitializeSceneData();

        SetActiveChilds(false);

        rankObj.SetActive(true);

        StartMoveDelta();
    }

    private void OnDisable()
    {
        rectTr.DOSizeDelta(min, 0, true);
    }

    public void InitializeSceneData()
    {
        currentScene = SceneManager.GetActiveScene();
        currentSceneNum = currentScene.buildIndex;
        stageNum = currentSceneNum - (minBuildNum - 1);
    }

    private void LateUpdate()
    {
        int stageBuildNum = SceneManager.GetActiveScene().buildIndex - (minBuildNum - 1);
        if (currentSceneNum != stageBuildNum)
        {
            print("현재 스테이지와 다릅니다. 변경합니다.");

            currentSceneNum = SceneManager.GetActiveScene().buildIndex - (minBuildNum - 1);
            stageNum = currentSceneNum;
        }
    }

    #region UI Animation

    private void StartMoveDelta()
    {
        rectTr.DOSizeDelta(min, 0, true);
        rectTr.DOSizeDelta(new Vector2(max.x, min.y), moveWidthSpeed, true).OnComplete(() => { CallBackMoveDelta1(); });
    }

    private void CallBackMoveDelta1()
    {
        float currentTime = 0f;
        rectTr.DOSizeDelta(new Vector2(max.x, max.y), moveHeightSpeed, true).OnUpdate(() => {
            if (showObjTimer <= currentTime)
            {
                currentTime = 0;
                SetActiveChilds(true);
            }

            currentTime += Time.deltaTime;
        });
    }

    private void SetActiveChilds(bool active)
    {
        foreach (var child in rankObjs)
            child.SetActive(active);
    }

    #endregion

    #region UI Content

    public void ChangeToJsonData(ScoreData scoreData, string nameText, string scoreText)
    {

    }

    // 텍스트 변경 메서드
    public void LoadScoreDataText(int stageNum, int index)
    {
        ScoreData stageToModify = StageData.instance.GetStageModify(stageNum);

        if (stageToModify != null)
        {
            nameText[index].text = stageToModify.name[index];
            scoreText[index].text = stageToModify.score[index].ToString();
        }
    }

    // 랭크 순위가 변동되었습니까잉?
    public void IsRankChange()
    {
        // 목적지에 도착하면 값 넣고 처리하기
        int currentSecond = GameManager.Instance.timerController.currentSeconds;
        // 비교하기
        if (StageData.instance.CheckNewScore(stageNum, currentSecond))
        {
            // 해당 되면 값 변경 후
            rankInsertNameText.text = "";
            rankShowScoreText.text = currentSecond.ToString();
            // 랭크 변경 UI 띄우기
            rankInsertObj.SetActive(true);
        }
    }

    public void ButtonInteraction()
    {
        // 일단 랭크 올리고 씬 넘기기
        if (SceneManager.GetActiveScene().name == "Stage9")
        {
            SceneManager.LoadScene("EndScene");
        }

        StageData.instance.AddNewScore(stageNum, rankInsertNameText.text.ToString(), float.Parse(rankShowScoreText.text));
        UpdateTexts();
        rankInsertObj.gameObject.SetActive(false);
        rankObj.gameObject.SetActive(true);
    }

    private void UpdateTexts()
    {
        for (int i = 0; i < 5; i++)
        {
            LoadScoreDataText(stageNum, i);
        }
    }

    #endregion
}
