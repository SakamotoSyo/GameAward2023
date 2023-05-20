using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public Sprite EnemySprite;

    public EnemyColor EnemyColor;

    public Animator EnemyAnim;

    public int RankPoint;

    public GameObject EnemyPrefab;

    public WeaponData[] WeaponDates;

    public bool IsBoss;

}
