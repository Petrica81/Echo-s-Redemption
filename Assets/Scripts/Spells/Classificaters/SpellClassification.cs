using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpellClassification : MonoBehaviour
{
    private Dictionary<string,Dictionary<string, int>> spellValue = new Dictionary<string, Dictionary<string, int>> {
        ["fire"] = new Dictionary<string, int>{
                ["Fireball"] = 0,
            },
        ["space"] = new Dictionary<string, int>{
                ["Spaceball"] = 0,
            },
        ["earth"] = new Dictionary<string, int>{
                ["Earthquake"] = 0,
            },
        ["water"] = new Dictionary<string, int>{
                ["WaterCascade"] = 0,
            },
        ["wind"] = new Dictionary<string, int>{
                ["Windball"] = 0,
            }
    };
    private void SpellClassify(string input, string type)
    {
        foreach (string word in input.Split(" "))
        {
            string newWord = TextHelper.FindClosestWord(word, SpellWords.keySpellValue.Keys.ToList());
            if (newWord != null)
            {
                if (SpellWords.keySpellValue[newWord] != null)
                {
                    foreach ((string spell, int value) in SpellWords.keySpellValue[newWord])
                    {
                        Debug.Log($"Spell = {spell}, Value = {value}, Word = {newWord}");
                        spellValue[type][spell] += value;
                    }
                }
            }
        }
    }

    public string GetSpell(string text, string type)
    {
        ResetValues();
        SpellClassify(text, type);
        int maxim = -1;
        string spell = null;
        foreach (string key in spellValue[type].Keys.ToList())
        {
            Debug.Log($"The spell:{key} Has value: {spellValue[type][key]}.");
            if (spellValue[type][key] == maxim)
                spell = null;

            if (spellValue[type][key] > maxim)
            {
                maxim = spellValue[type][key];
                spell = key;
            }
        }
        return (spell != null) ? spell : null;
    }
    private void ResetValues()
    {
        foreach (string type in spellValue.Keys.ToList())
        {
            foreach (string key in spellValue[type].Keys.ToList())
            {
                spellValue[type][key] = 0;
            }
        }
    }
}
