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

#if UNITY_EDITOR
    private void Start()
    {
        if (_meshManager._isGS)
        {
            _meshManager._weaponType = WeaponType.GreatSword;
        }
        if (_meshManager._isDB)
        {
            _meshManager._weaponType = WeaponType.DualBlades;
        }
        if (_meshManager._isH)
        {
            _meshManager._weaponType = WeaponType.Hammer;
        }
        if (_meshManager._isS)
        {
            _meshManager._weaponType = WeaponType.Spear;
        }
    }
#endif
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
#if UNITY_EDITOR
        if (_meshManager._isGS || _meshManager._isDB || _meshManager._isH || _meshManager._isS)
        {
            switch (_meshManager._weaponType)
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
                        Debug.Log("指定された武器の名前 : " + _meshManager._weaponType + " は存在しません");
                    }
                    return;
            }
        }
#endif

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
        Vector3 pos = new Vector3(0, 0, 0);
        List<Vector3> posList = new List<Vector3>();
        for (int i = 0; i < weaponList.Count; i++)
        {
            float x = weaponList[i].x + _meshManager.FirstCenterPos.x;

            float y = weaponList[i].y;

            pos = new Vector3(x, y);

            _meshManager.MyVertices[i] = pos;

            posList.Add(pos);
        }

        _meshManager.MyMesh.SetVertices(posList);
    }
}
