using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static PublicLibrary;

public class Tile : MonoBehaviour
{
    public TileType Type;

    private int rotation;
    private int rotationMultipler = 90;

    public int X { get; private set; }
    public int Y { get; private set; }

    LevelData levelData;

    [Header("DoTween && Sin Animation Values")]
    float amplitude = 0.1f;
    float speed = 1.0f;
    float duration = 1.0f;
    Vector3 initialPosition;
    Sequence sinMoveSequence;
    float timeOffset;
    public bool isPlaying;
    public float newY;

    public float startTime;
    public float elapsedTime = 0f;
    public float savedTime = 0f;

    // 콜라이더 오브젝트를 저장하는 리스트
    public List<Transform> colls = new List<Transform>();
    // 평지 위치 값을 저장하는 변수
    public Vector3 floor = Vector3.zero;

    // 방문 했는지 체크하는 변수
    public bool isVisited;

    // 마지막 타일인지 확인하는 변수
    public bool isDestination;

    // 움직임 시작 여부를 결정하는 변수
    public bool shouldMove = true;

    public void Initialize(TileType type, int rot, int x, int y, LevelData level)
    {
        Type = type;
        rotation = rot;
        X = x;
        Y = y;
        // 타일 방문 여부
        isVisited = false;

        // 자식 오브젝트 가져오기
        if (Type >= TileType.Start)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Floor"))
                {
                    floor = child.position;
                }
                else if (child.CompareTag("Connect"))
                {
                    colls.Add(child);
                }
            }
        }

        transform.eulerAngles = new Vector3(0, rotation * rotationMultipler, 0);

        if (Type == TileType.Button)
        {
            if (y == level.Column - 1)
            {
                transform.eulerAngles = new Vector3(0, -1 * rotationMultipler, 0);
            }
        }

        if (Type == TileType.End)
            isDestination = true;

        if (Type >= TileType.Straight)
        {
            isPlaying = false;
            initialPosition = transform.position;
            timeOffset = Random.Range(0f, speed * Mathf.PI);
            PlaySequence();
        }
    }

    public void PlaySequence()
    {
        // shouldMove가 false면 함수를 빠져나옴
        if (!shouldMove) return;

        sinMoveSequence = DOTween.Sequence();
        // Tween 시작
        sinMoveSequence.Append(transform.DOMoveY(initialPosition.y + amplitude, duration)
            .SetLoops(1).OnUpdate(() =>
            {
                // Sin 함수 움직임 계산하기
                if (isPlaying == false)
                {
                    elapsedTime = Time.time - startTime;
                    newY = Mathf.Sin((elapsedTime + savedTime) * timeOffset) * amplitude;
                    transform.position = new Vector3(transform.position.x, newY, transform.position.z);
                }
            }));

        sinMoveSequence.OnComplete(() =>
        {
            // Tween 완료되면 Sin 움직임 다시 시작
            PlaySequence();
        });
    }

    //움직임 멈추기
    public void StopMovement()
    {
        shouldMove = false; // 움직임을 시작하지 않도록 설정
        if (sinMoveSequence != null) // sinMoveSequence가 null이 아닐 때만 호출
            sinMoveSequence.Pause();
    }

    public float CalculatePosY()
    {
        return Mathf.Sin((elapsedTime + savedTime) * timeOffset) * amplitude;
    }

    public Vector3 SetVector(float newY)
    {
        return new Vector3(transform.position.x, newY, transform.position.z);
    }

    public void LoadSaveTime()
    {
        // 한 번만 실행
        if (isPlaying == true)
        {
            savedTime = elapsedTime - savedTime; // 이동을 멈추고 현재 경과한 시간을 저장
        }
        else
        {
            startTime = Time.time; // 이동을 재개하고 시작 시간을 업데이트
        }
    }
}
