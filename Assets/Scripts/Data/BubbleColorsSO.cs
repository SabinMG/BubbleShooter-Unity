using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BubbleColor
{
    public int number;
    public Color value;
}

[CreateAssetMenu(menuName = "Bubble Pop/Bubble Colors")]
public class BubbleColorsSO : ScriptableObject
{
    [SerializeField] BubbleColor[] _bubbleColors;
    private Dictionary<int, Color> _colorDictionary;

    public void OnDestroy()
    {
        _colorDictionary = null;
    }

    public Color GetColorFortheValue(int color)
    {
        if (_colorDictionary == null) InitializeDictionary();
        return _colorDictionary[color];
    }

    private void InitializeDictionary()
    {
        _colorDictionary = new Dictionary<int, Color>();
        for (int i = 0; i < _bubbleColors.Length; i++)
        {
            BubbleColor color = _bubbleColors[i];
            _colorDictionary.Add(color.number, color.value);
        }
    }
}
