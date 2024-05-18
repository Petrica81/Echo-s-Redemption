using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField]
    [Tooltip("The sprite which represents the item.")]
    protected Sprite sprite;
    [SerializeField]
    [Tooltip("The price that would appear in shop, if applicable.")]
    protected int price;

    public Sprite Sprite {  get => sprite; set => sprite = value; }
    public int Price { get => price; set => price = value; }
}
