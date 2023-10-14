using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CircleTransition : MonoBehaviour
{
    [HideInInspector]
    public Transform target;

    public bool isCoroutineEnd;

    private Canvas _canvas;
    private Image _blackScreen;

    private Vector2 _playerCanvasPos;
    
    private static readonly int RADIUS = Shader.PropertyToID("_Radius");
    private static readonly int CENTER_X = Shader.PropertyToID("_CenterX");
    private static readonly int CENTER_Y = Shader.PropertyToID("_CenterY");

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _blackScreen = GetComponentInChildren<Image>();
    }

    public void SetTransform(Transform target)
    {
        this.target = target;
    }

    public void OpenBlackScreen(float duration, float beginRadius, float endRadius)
    {
        DrawBlackScreen();
        isCoroutineEnd = false;
        StartCoroutine(Transition(duration, beginRadius, endRadius, (result) =>
        {
            if (result)
                isCoroutineEnd = true;
        }));
    }

    public void CloseBlackScreen(float duration, float beginRadius, float endRadius)
    {
        DrawBlackScreen();
        isCoroutineEnd = false;
        StartCoroutine(Transition(duration, beginRadius, endRadius, (result) =>
        {
            if (result)
                isCoroutineEnd = true;
        }));
    }

    public void DrawBlackScreen()
    {
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;
        // Need a target
        var targetScreenPos = Camera.main.WorldToScreenPoint(target.position);

        // To Draw to Image to Full Screen, we get the Canvas Rect size
        var canvasRect = _canvas.GetComponent<RectTransform>().rect;
        var canvasWidth = canvasRect.width;
        var canvasHeight = canvasRect.height;

        // But because the Black Screen is now square (different to Screen). So we much added the different of width/height to it
        // Now we convert Screen Pos to Canvas Pos
        _playerCanvasPos = new Vector2
        {
            x = (targetScreenPos.x / screenWidth) * canvasWidth,
            y = (targetScreenPos.y / screenHeight) * canvasHeight,
        };

        var squareValue = 0f;
        if (canvasWidth > canvasHeight)
        {
            // Landscape
            squareValue = canvasWidth;
            _playerCanvasPos.y += (canvasWidth - canvasHeight) * 0.5f;
        }
        else
        {
            // Portrait            
            squareValue = canvasHeight;
            _playerCanvasPos.x += (canvasHeight - canvasWidth) * 0.5f;
        }

        _playerCanvasPos /= squareValue;
        
        var mat = _blackScreen.material;
        mat.SetFloat(CENTER_X, _playerCanvasPos.x);
        mat.SetFloat(CENTER_Y, _playerCanvasPos.y);

        _blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);

        // Now we want the circle to follow the player position
        // So First, we must get the player world position, convert it to screen position, and normalize it (0 -> 1)
        // And input into the shader
    }

    private IEnumerator Transition(float duration, float beginRadius, float endRadius, Action<bool> callback)
    {
        var mat = _blackScreen.material;
        var time = 0f;
        while (time <= duration)
        {
            time += Time.deltaTime;
            var t = time / duration;
            var radius = Mathf.Lerp(beginRadius, endRadius, t);

            mat.SetFloat(RADIUS, radius);

            yield return null;
        }

        if (callback != null)
        {
            bool result = true;
            callback(result);
        }
    }
}