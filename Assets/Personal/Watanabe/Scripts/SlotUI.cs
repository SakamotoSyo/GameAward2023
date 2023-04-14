using UnityEngine;
using UnityEngine.UI;

/// <summary> 戦闘準備の武器スロットで使う </summary>
public class SlotUI : MonoBehaviour
{
    [SerializeField] private Text _param = default;

    private Image[] _weapons = new Image[4];

    private void Start()
    {
        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i] = gameObject.transform.GetChild(i).GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //HomeSceneに戻る
            Fade.Instance.FadeOut();
        }
    }

    public void Unlock(int index)
    {
        foreach (var weapon in _weapons)
        {
            weapon.GetComponent<Image>().color = Color.white;
        }

        if (_weapons[index].TryGetComponent(out WeaponSlot slot))
        {
            _param.text = slot.Parameter;
        }
    }
}
