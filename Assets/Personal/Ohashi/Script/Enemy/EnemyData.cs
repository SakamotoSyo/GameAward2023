using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public Sprite EnemySprite;

    public Animator EnemyAnim;

    public int RankPoint;

    public WeaponData[] WeaponDates;

}
