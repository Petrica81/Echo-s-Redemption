using UnityEngine;

public enum ArmorType
{
    Helmet,
    Chestplate,
    Pants
}
public enum ArmorMaterial
{
    Naked,
    Leather
}
[CreateAssetMenu(fileName = "NewArmorItem", menuName = "ScriptableObjects/Items/Armor")]
public class ArmorItem : Item
{
    [SerializeField]
    [Tooltip("The type of armor.")]
    private ArmorType type;
    [SerializeField]
    [Tooltip("The material of the armor.")]
    private ArmorMaterial material;

    public string Material { get => material.ToString(); }
    public string Type { get => type.ToString(); }

    
}
