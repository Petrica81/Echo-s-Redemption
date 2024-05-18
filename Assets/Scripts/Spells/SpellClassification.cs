using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellClassification : MonoBehaviour
{
    private Dictionary<string, int> spellValue = new Dictionary<string, int> {
        { "fire", 0 },
        { "space", 0 },
        { "earth", 0 },
        { "water", 0 },
        { "wind", 0 }
    };
    private void SpellClassify(string _input)
    {
        foreach (string _word in _input.Split(" "))
        {
            string _newWord = TextHelper.FindClosestWord(_word, SpellWords.keySpellValue.Keys.ToList());
            if (_newWord != null)
            {
                if (SpellWords.keySpellValue[_newWord] != null)
                {
                    foreach ((string _spell, int _value) in SpellWords.keySpellValue[_newWord])
                    {
                        Debug.Log($"!!!!!!!! spell = {_spell} and value = {_value}, word = {_newWord}");
                        spellValue[_spell] += _value;
                    }
                }
            }
        }
    }

    public string GetSpell(string _text, string _type)
    {
        ResetValues();
        SpellClassify(_text);
        int _maxim = 0;
        string _spell = null;
        foreach (string _key in spellValue.Keys.ToList())
        {
            Debug.Log($"The spell:{_key} Has value: {spellValue[_key]}.");
            if (spellValue[_key] == _maxim)
                _spell = null;

            if (spellValue[_key] > _maxim)
            {
                _maxim = spellValue[_key];
                _spell = _key;
            }
        }
        return (_spell != null) ? _spell : null;
    }
    private void ResetValues()
    {
        foreach (string _key in spellValue.Keys.ToList())
        {
            spellValue[_key] = 0;
        }
    }
}
