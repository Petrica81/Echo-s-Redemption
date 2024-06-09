using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    private int damage;
    public int Damage { get { return damage; } set { damage = value; } }
}
