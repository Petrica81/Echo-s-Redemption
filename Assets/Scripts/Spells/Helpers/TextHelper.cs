using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class TextHelper
{
    public static string FormatText(string _text)
    {
        string _newText = "";

        foreach(string _word in _text.Split(' '))
        {
            if (!SpellWords.unnecessaryWords.Contains(_word.ToLower()))
            {
                _newText += _word.ToLower() + " ";
            }
        }
        _newText = _newText.TrimEnd(' ');
        Debug.Log($"Text formated to: {_newText}");
        return _newText;
    }
    public static int LevenshteinDistance(string _s, string _t)
    {
        int _sLen = _s.Length;
        int _tLen = _t.Length;
        int[,] _dist = new int[_sLen + 1, _tLen + 1];

        for (int _i = 0; _i <= _sLen; _i++)
            _dist[_i, 0] = _i;

        for (int _j = 0; _j <= _tLen; _j++)
            _dist[0, _j] = _j;

        for (int _i = 1; _i <= _sLen; _i++)
        {
            for (int _j = 1; _j <= _tLen; _j++)
            {
                int _cost = (_t[_j - 1] == _s[_i - 1]) ? 0 : 1;
                _dist[_i, _j] = Mathf.Min(
                    Mathf.Min(_dist[_i - 1, _j] + 1, _dist[_i, _j - 1] + 1),
                    _dist[_i - 1, _j - 1] + _cost);
            }
        }
        return _dist[_sLen, _tLen];
    }

    public static string FindClosestWord(string _input, List<string> _words)
    {
        int _minDistance = int.MaxValue;
        string _closestWord = null;

        foreach (string _word in _words)
        {
            int distance = _minDistance;
            if (SpellWords.levenshteinDistances.Keys.Contains((_input, _word)))
            {
                distance = SpellWords.levenshteinDistances[(_input, _word)];
            }
            else
            {
                distance = LevenshteinDistance(_input, _word);
                SpellWords.levenshteinDistances.Add((_input, _word), distance);
            }
            if (distance < _minDistance && distance <= 1)
            {
                _minDistance = distance;
                _closestWord = _word;
            }
            else if (_word.Contains(_input) && distance <= 3)
            {
                _closestWord = _word;
            }
        }
        if(_closestWord != null)
            Debug.Log($"Most appropiate word for {_input} is {_closestWord}");
        return _closestWord;
    }
}
