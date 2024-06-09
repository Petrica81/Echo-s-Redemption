using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MasteryClassification : MonoBehaviour
{
    private Dictionary<string, List<(string, int)>> keyMasteryValue = new Dictionary<string, List<(string, int)>>
    {
        { "god",new List<(string,int)>{("High",500)} },
        { "godess",new List<(string,int)>{("High",500)} }
    };
    private Dictionary<string, int> masteryValue = new Dictionary<string, int> {
        { "High", 0 },
        { "Medium", 0 },
        { "Low", 0 }
    };
    private void MasteryClassify(string _input)
    {
        foreach (string _word in _input.Split(" "))
        {
            string _newWord = null;
            if(_word.Count() > 1)
                _newWord = TextHelper.FindClosestWord(_word, keyMasteryValue.Keys.ToList());
            if (_newWord != null)
            {
                if (keyMasteryValue[_newWord] != null)
                {
                    foreach ((string _mastery, int _value) in keyMasteryValue[_newWord])
                    {
                        Debug.Log($"Mastery = {_mastery}, Value = {_value}, Word = {_newWord}");
                        masteryValue[_mastery] += _value;
                    }
                }
            }
        }
    }

    public string GetMastery(string _text, int _value)
    {
        ResetValues();
        MasteryClassify(_text);
        if (_value > 40)
            masteryValue["Medium"] = _value;
        else
            masteryValue["Low"] = 100 - _value;
        int _maxim = -1;
        string _mastery = null;
        foreach (string _key in masteryValue.Keys.ToList())
        {
            Debug.Log($"The mastery:{_key} Has value: {masteryValue[_key]}.");
            if (masteryValue[_key] == _maxim)
                _mastery = null;

            if (masteryValue[_key] > _maxim)
            {
                _maxim = masteryValue[_key];
                _mastery = _key;
            }
        }
        return (_mastery != null) ? _mastery : null;
    }
    private void ResetValues()
    {
        foreach (string _key in masteryValue.Keys.ToList())
        {
            masteryValue[_key] = 0;
        }
    }
}
