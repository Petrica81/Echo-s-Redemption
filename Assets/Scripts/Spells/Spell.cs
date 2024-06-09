using UnityEngine;

public abstract class Spell : MonoBehaviour
{
    [SerializeField]
    protected string _name;
    [SerializeField]
    protected GameObject _prefab;

    public string Name { get { return _name; } }
    public GameObject Prefab {  get { return _prefab; } }
}
