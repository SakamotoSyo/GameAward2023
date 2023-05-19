using UnityEngine;
using UnityEngine.UI;

public class StatusView : MonoBehaviour
{
    [SerializeField] private Text _attackValue = default;
    [SerializeField] private Text _testValue = default;

    private WeaponStatus _status = default;

    private void Start()
    {
        _status = GetComponent<WeaponStatus>();
    }

    private void Update()
    {
        _attackValue.text = $"Attack : {_status.AttackValue}";
        _testValue.text = $"Test : {_status.TestValue}";
    }
}
