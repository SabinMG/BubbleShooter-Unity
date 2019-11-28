using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Bubble Pop/Game Config")]
public class GameConfigSO : ScriptableObject, IGameConfig
{
    [SerializeField] HorizontalRowLayout _hexGridInitLayout;
    [SerializeField] Vector2Int _hexGridSize;
    [SerializeField] float _hexCellRadius;
    [SerializeField] int _bubbleInitRows;
   
    public HorizontalRowLayout hexGridInitLayout => _hexGridInitLayout;
    public Vector2Int hexGridSize => _hexGridSize;
    public float hexCellRadius => _hexCellRadius;
    public int bubbleInitRows => _bubbleInitRows;
}
