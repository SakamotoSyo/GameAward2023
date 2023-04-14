using UnityEngine;

public class AttackStatus : MonoBehaviour
{
    [Multiline(5)]
    [SerializeField] private string _status = "";

    public string Status { get => _status; protected set => _status = value; }
}
