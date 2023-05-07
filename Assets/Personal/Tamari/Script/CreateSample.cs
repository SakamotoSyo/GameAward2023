using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSample : MonoBehaviour
{
    [SerializeField] MeshManager _meshManager = default;

    [SerializeField, Tooltip("大剣のサンプル")]
    private List<Vector3> _taikenSample = default;

    [SerializeField, Tooltip("双剣のサンプル")]
    private List<Vector3> _soukenSample = default;

    [SerializeField, Tooltip("ハンマーのサンプル")]
    private List<Vector3> _hammerSample = default;

    [SerializeField, Tooltip("やりのサンプル")]
    private List<Vector3> _yariSample = default;


    public void WeaponSamples()
    {
        switch (GameManager.BlacksmithType)
        {
            case WeaponType.GreatSword:
                {
                    SampleTaiken();
                }
                break;
            case WeaponType.DualBlades:
                {
                    SampleSouken();
                }
                break;
            case WeaponType.Hammer:
                {
                    SampleHammer();
                }
                break;
            case WeaponType.Spear:
                {
                    SampleYari();
                }
                break;
            default:
                {
                    Debug.Log("指定された武器の名前 : " + GameManager.BlacksmithType + " は存在しません");
                }
                return;
        }

        Debug.Log(GameManager.BlacksmithType + "のさんぷる");
    }

    public void SampleTaiken()
    {
        BaseSampleCreate(_taikenSample);
    }

    public void SampleSouken()
    {
        BaseSampleCreate(_soukenSample);
    }

    public void SampleHammer()
    {
        BaseSampleCreate(_hammerSample);
    }

    public void SampleYari()
    {
        BaseSampleCreate(_yariSample);
    }

    private void BaseSampleCreate(List<Vector3> weaponList)
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            _meshManager.MyVertices[i] = weaponList[i];
            _meshManager.MyMesh.SetVertices(weaponList);
        }
    }
}
