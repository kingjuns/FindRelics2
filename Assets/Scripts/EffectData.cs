using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

[CreateAssetMenu(fileName = "Effect", menuName = "Effect/effectData")]
public class EffectData : ScriptableObject
{
    public ParticleSystem Effect_Prefab; // 이펙트
    public Vector3 EffectPos; // 이펙트 위치값
}
