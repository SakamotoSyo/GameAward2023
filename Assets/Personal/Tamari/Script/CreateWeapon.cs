using UnityEngine;

public class CreateWeapon : BaseCreateWeapon
{
    public void CreateWeapons(WeaponType weapon)
    {
        switch (weapon)
        {
            case WeaponType.GreatSword:
                {
                    _data = SaveManager.Load(SaveManager.TAIKENFILEPATH);
                }
                break;
            case WeaponType.DualBlades:
                {
                    _data = SaveManager.Load(SaveManager.SOUKENFILEPATH);
                }
                break;
            case WeaponType.Hammer:
                {
                    _data = SaveManager.Load(SaveManager.HAMMERFILEPATH);
                }
                break;
            case WeaponType.Spear:
                {
                    _data = SaveManager.Load(SaveManager.YARIFILEPATH);
                }
                break;
            default:
                {
                    Debug.Log("指定された武器の名前 : " + weapon + " は存在しません");
                }
                return;
        }
        Create();
    }


    /// <summary>
    /// デバッグ用。ボタンでデバッグするときはこれ使おう。
    /// </summary>
    /// <param name="weaponName"></param>
    public void DebugCreate(string weaponName)
    {
        switch (weaponName)
        {
            case "Taiken":
                {
                    _data = SaveManager.Load(SaveManager.TAIKENFILEPATH);
                }
                break;
            case "Souken":
                {
                    _data = SaveManager.Load(SaveManager.SOUKENFILEPATH);
                }
                break;
            case "Hammer":
                {
                    _data = SaveManager.Load(SaveManager.HAMMERFILEPATH);
                }
                break;
            case "Yari":
                {
                    _data = SaveManager.Load(SaveManager.YARIFILEPATH);
                }
                break;
            default:
                {
                    Debug.Log("指定された武器の名前 : " + weaponName + " は存在しません");
                }
                return;
        }
        Create();
    }
}
