using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TypeClassification: MonoBehaviour
{
    private Dictionary<string,int> typeValue = new Dictionary<string,int> {
        { "fire", 0 },
        { "space", 0 },
        { "earth", 0 },
        { "water", 0 },
        { "wind", 0 }
    };
    [SerializeField]
    private MasteryClassification masteryClassification;
    private void TypeClassify(string _input)
    {
        foreach(string _word in _input.Split(" "))
        {
            string _newWord = TextHelper.FindClosestWord(_word, SpellWords.keyTypeValue.Keys.ToList());
            if (_newWord != null)
            {
                if (SpellWords.keyTypeValue[_newWord] != null)
                {
                    foreach ((string _type, int _value) in SpellWords.keyTypeValue[_newWord])
                    {
                        Debug.Log($"!!!!!!!! type = {_type} and value = {_value}, word = {_newWord}");
                        typeValue[_type] += _value;
                    }
                }
            }
        }
    }

    public string GetType(string _text)
    {
        ResetValues();
        TypeClassify(_text);
        int _maxim = 0;
        string _type = null;
        foreach(string _key in typeValue.Keys.ToList())
        {
            Debug.Log($"The type:{_key} Has value: {typeValue[_key]}.");
            if (typeValue[_key] == _maxim)
                _type = null;

            if (typeValue[_key] > _maxim)
            {
                _maxim = typeValue[_key];
                _type = _key;
            }
        }
        Debug.Log($"Mastery level: {masteryClassification.GetMastery(_text, _maxim)}");
        return (_type!=null)?_type:null;
    }
    private void ResetValues()
    {
        foreach(string _key in typeValue.Keys.ToList())
        {
            typeValue[_key] = 0;
        }
    }
}
