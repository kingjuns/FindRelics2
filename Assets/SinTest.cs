using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SinTest : MonoBehaviour
{
    public float amplitude = 1.0f;
    public float speed = 2.0f;

    private Vector3 initialPosition;
    private float sinOffset = 0f; // Mathf.Sin 값의 현재 값
    
    private Tween moveTweeen; // DoTween 애니메이션 저장 변수

    private void Start()
    {
        initialPosition = transform.position;

        moveTweeen = transform.DOMoveX(initialPosition.x + 5f, 5f).SetLoops(1).OnKill(() =>
        {
            Debug.Log("DoTween animation 종료!");
        });
    }

    private void Update()
    {
        // DoTween 애니메이션이 실행 중이거나 실행 중이 아닐 때 Mathf.Sin 값을 이용한 움직임을 추가해야 함
        if (moveTweeen != null && (moveTweeen.IsActive() || !moveTweeen.IsPlaying()))
        {
            sinOffset = Mathf.Sin(Time.time * speed);
            float newY = initialPosition.y + amplitude * sinOffset;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
