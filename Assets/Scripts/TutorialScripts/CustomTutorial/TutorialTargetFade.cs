using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTargetFade : TutorialBase
{
    CircleTransition circleTransition;

    [SerializeField]
    List<Tile> tiles;

    public PublicLibrary.TileType type;

    public bool isOpen;

    [Tooltip("버튼 중에서 가져올 인덱스 값")]
    [SerializeField]
    int targetNum = 1;

    [SerializeField]
    float duration = 1;

    [SerializeField]
    float beginRadius = 1;

    [SerializeField]
    float endRadius = 0.15f;

    public override void Enter()
    {
        circleTransition = GameManager.Instance.circleTransition;

        List<Tile> tempTiles = GameManager.Instance.SpawnedTiles;
        foreach (Tile tile in tempTiles)
        {
            if (type == tile.Type)
            {
                tiles.Add(tile);
            }
        }

        if (type == PublicLibrary.TileType.Button)
        {
            circleTransition.SetTransform(tiles[targetNum].transform);
        }
        else if (type == PublicLibrary.TileType.Straight ||
                type == PublicLibrary.TileType.Up_Left ||
                type == PublicLibrary.TileType.Up_Left_Down ||
                type == PublicLibrary.TileType.Up_Left_Down_Right)
        {
            // int result = Mathf.RoundToInt(tiles.Count / 2);
            // circleTransition.SetTransform(tiles[result].transform);
            circleTransition.SetTransform(tiles[0].transform);
        }
        else
        {
            circleTransition.SetTransform(tiles[0].transform);
        }

        circleTransition.OpenBlackScreen(duration, beginRadius, endRadius);
    }

    public override void Execute(TutorialController controller)
    {
        if (circleTransition.isCoroutineEnd)
            controller.SetNextTutorial();
    }

    public override void Exit()
    {
    }
}
