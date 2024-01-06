using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TileButton : MonoBehaviour
{
    [Header("Animator Settings")]
    [SerializeField] Animator anim;

    [HideInInspector] public Tile Tile;

    GameManager gameManager;

    void Awake() => gameManager = GameManager.Instance;

    // 초기 위치 저장
    [SerializeField] List<Vector3> tempVec;

    GameObject moveSound;
    public GameObject StarButton;
    GameObject touchSound;


    public bool canDoMove;

    void Start()
    {
        anim.GetComponent<Animator>();
        Tile = GetComponent<Tile>();
        
        tempVec = new List<Vector3>();

        moveSound = Resources.Load<GameObject>("Sound/TileMoveSound");
        touchSound = Resources.Load<GameObject>("Sound/TouchSound");
        canDoMove = true;
    }

    public void Initialize()
    {
        // 초기 위치 저장하기
        List<Tile> tileTransforms = TilesButtonLine();
        foreach (Tile tile in tileTransforms)
        {
            tempVec.Add(tile.transform.position);
        }
    }

    void Update()
    {
        if (tempVec.Count == 0)
        {
            tempVec.Clear();
            List<Tile> tileTransforms = TilesButtonLine();
            foreach (Tile tile in tileTransforms)
            {
                tempVec.Add(tile.transform.position);
            }
        }
    }

    // 마우스 가까이 가져다 대었을 때
    void OnMouseEnter()
    {
        if (gameManager.hasGameEnded)
            return;

        if (canDoMove == false)
            return;

        anim.SetBool("isEntered", true);
        Instantiate(touchSound);
    }

    void OnMouseExit()
    {
        if (gameManager.hasGameEnded)
            return;

        if (canDoMove == false)
            return;

        anim.SetBool("isEntered", false);
    }

    void OnMouseDown()
    {
        if (gameManager.hasGameEnded)
            return;

        if (canDoMove == false)
            return;

        anim.SetTrigger("Clicked");

        // 만약 애니메이션이 실행되고 있는 중 이라면, 리턴 시킨다.
        int totalPlayingTweens = gameManager.tweenQueue.Count;
        if (totalPlayingTweens > 0)
            return;

        // 이펙트
        GameObject go = Instantiate(StarButton);
        go.transform.position = transform.position;
        Destroy(go, 0.8f);
        Instantiate(moveSound);

        // 타일 스왑 시킨다.
        ObjectSwap(TilesButtonLine());

        // 버튼 누른 횟수 1 증가
        gameManager.PressButtonCount++;
    }

    public List<Tile> TilesButtonLine()
    {
        // 해당되는 타일들을 전부 담기 위한 타일 리스트
        List<Tile> result = new List<Tile>();
        // 버튼 타일을 기준으로 정면에 레이캐스트를 쏘았을 때
        // "Tile" 레이어를 가진 Collider 오브젝트들 만 리스트에 넣기
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Tile");
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Mathf.Infinity, layerMask);
        // 거리에 따라 결과를 정렬
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance));

        result = hits.Select((hit) => hit.collider.transform.GetComponent<Tile>()).ToList();

        return result;
    }

    public void ObjectSwap(List<Tile> newList)
    {
        for (int i = 0; i < newList.Count - 1; i++)
        {
            MoveObjectToTarget(newList[i], tempVec[i + 1], gameManager.PlayingAnimSpeed);
        }
        MoveObjectWithCurve(newList[newList.Count - 1], tempVec[0], 0.1f, gameManager.PlayingAnimSpeed);
    }

    private void MoveObjectToTarget(Tile tile, Vector3 targetPosition, float duration)
    {
        targetPosition.y = tile.newY;
        tile.isPlaying = true;
        // 트윈 실행
        Tween move = tile.transform.DOMove(targetPosition, duration)
            .OnUpdate(() =>
            {
                // tile.newY = tile.CalculatePosY();
                // tile.transform.position = tile.SetVector(tile.newY);
            })
            .OnComplete(() =>
            {
                tile.isPlaying = false;
                tile.LoadSaveTime();
                gameManager.tweenQueue.Dequeue();
            }
        );

        gameManager.tweenQueue.Enqueue(move);
    }

    private void MoveObjectWithCurve(Tile tile, Vector3 targetPos, float curveStrength, float duration)
    {
        // targetPos.y = tile.newY;
        tile.isPlaying = true;
        Vector3 middlePoint = (tile.transform.position + targetPos) * 0.5f;
        // 아래로 -1만큼 위치 수정
        middlePoint.y = -1f;
        Vector3 direction = (middlePoint - tile.transform.position).normalized;
        Vector3 controlPoint = middlePoint + direction * curveStrength;

        //Tween move = tile.transform.DOLocalPath(new Vector3[] { tile.transform.localPosition, controlPoint, targetPos }, duration)
        //    .OnUpdate(() =>
        //    {
        //        tile.newY = tile.CalculatePosY();
        //        targetPos.y = tile.newY;
        //    })
        //    .OnComplete(() =>
        //    {
        //        tile.isPlaying = false;
        //        tile.SetVector(tile.newY);
        //        gameManager.tweenQueue.Dequeue();
        //        // 가장 오래 실행 될 것 같은 트윈에 길찾기 시작 함수 넣기
        //        gameManager.BFSStart();
        //    });
        Vector3[] pathWithMidpoint = new Vector3[] { tile.transform.localPosition, middlePoint, targetPos };
        Tween move = tile.transform.DOPath(pathWithMidpoint, duration)
            .OnUpdate(() =>
            {
                // 위치 B를 업데이트 (예를 들어, 다른 오브젝트나 변수로부터 위치 B를 얻어옴)
                targetPos = new Vector3(targetPos.x, tile.CalculatePosY(), targetPos.z);
            })
            .OnComplete(() =>
            {
                Debug.Log("이동 완료");
                tile.isPlaying = false;
                tile.LoadSaveTime();
                gameManager.tweenQueue.Dequeue();
                // 가장 오래 실행 될 것 같은 트윈에 길찾기 시작 함수 넣기
                gameManager.BFSStart();
            });

        gameManager.tweenQueue.Enqueue(move);
    }
}
