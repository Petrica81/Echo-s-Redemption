using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerInventory",menuName = "ScriptableObjects/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    #region ArmorSlots
    [SerializeField]
    [Tooltip("The item equipped in the head slot.")]
    private ArmorItem head;
    [SerializeField]
    [Tooltip("The item equipped in the chest slot.")]
    private ArmorItem chest;
    [SerializeField]
    [Tooltip("The item equipped in the foot slot.")]
    private ArmorItem foot;
    #endregion

    #region Stats
    [SerializeField]
    [Tooltip("Player's current health.")]
    private int playerHealth;
    [SerializeField]
    [Tooltip("Player's maximum health.")]
    private int playerHealthMax;
    [SerializeField]
    [Tooltip("Player's current mana.")]
    private int playerMana;
    [SerializeField]
    [Tooltip("Player's maximum mana.")]
    private int playerManaMax;
    #endregion

    #region ArmorAccesors
    public ArmorItem Helmet { get => head; set => head = value; }
    public ArmorItem Chestplate { get => chest; set => chest = value; }
    public ArmorItem Pants { get => foot; set => foot = value; }
    #endregion

    #region StatsAccesors
    public int Health {  get => playerHealth; set => playerHealth = value; }
    public int HealthMax { get => playerHealthMax; set => playerHealthMax = value; }
    public int Mana { get => playerMana; set => playerMana = value; }
    public int ManaMax {  get => playerManaMax; set => playerManaMax = value; }
    #endregion
}
