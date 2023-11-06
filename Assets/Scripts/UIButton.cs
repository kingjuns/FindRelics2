using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    public void ShowRankUI()
    {
        RankManager.instance.CallEnable();
    }
}
