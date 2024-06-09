using System.Collections.Generic;
using UnityEngine;

public class Grimoire : MonoBehaviour
{
    [SerializeField]
    private List<Spell> _spellList;

    private Dictionary<string, GameObject> _spells;

    private void Awake()
    {
        _spells = new Dictionary<string, GameObject>();
        foreach (var spell in _spellList)
        {
            _spells.Add(spell.Name, spell.Prefab);
        }
    }

    public void CastSpell(string spellName)
    {
        if (_spells.TryGetValue(spellName, out GameObject spellPrefab))
        {
            Instantiate(spellPrefab, transform.parent.position + new Vector3(0,0.25f,0), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Spell {spellName} not found in grimoire.");
        }
    }
}
