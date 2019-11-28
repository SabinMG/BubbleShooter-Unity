using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class CreateInitialBubblesSystem : IInitializeSystem
{
    #region private variables
    private readonly Contexts m_contexts;
    private readonly IGameConfig _gameConfig;
    #endregion

    #region constructor
    public CreateInitialBubblesSystem(Contexts contexts, IGameConfig gameConfig)
    {
        m_contexts = contexts;
        _gameConfig = gameConfig;
    }

    public void Initialize()
    {
        int columns = _gameConfig.hexGridSize.y;
        int rows = _gameConfig.bubbleInitRows;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                GameEntity bubbleEntity = m_contexts.game.CreateEntity();
                int[] primitiveBubles =  BubbleService.primitiveBubbles;
                int random = Random.Range(0, primitiveBubles.Length);
                bubbleEntity.AddBubble(primitiveBubles[random]);
                bubbleEntity.AddAsset("Bubble");
                bubbleEntity.AddHexCell(new Vector2Int(i, j));
                Vector2 bubblePos = GexGridService.HexOffsetCoordToPixel(m_contexts, new HexOffsetCoord(bubbleEntity.hexCell.coordinate.x, bubbleEntity.hexCell.coordinate.y));
                bubbleEntity.AddPosition(new Vector2(bubblePos.x, -bubblePos.y));
            }
        }
    }
    #endregion
}
