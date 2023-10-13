using DG.Tweening;
using UnityEngine;

public class SinTest : MonoBehaviour
{

    public float amptiltude = 1.0f;
    public float speed = 2.0f;

    Vector3 initalPos;

    //public bool isAction;
    private float sinOffset = 0f;

    private Tween moveTeen;

    void Start()
    {
        initalPos = transform.position;

        //moveTeen = transform.DOMoveX(initalPos.x + )
    }

    // Update is called once per frame
    void Update()
    {
        if (moveTeen != null && (moveTeen.IsActive()) || !moveTeen.IsPlaying())
        {
            sinOffset = Mathf.Sin(Time.time * speed);
            float yPos = initalPos.y + amptiltude + sinOffset;

            transform.position = new Vector3(initalPos.y, yPos, initalPos.z);
        }
    }
}
