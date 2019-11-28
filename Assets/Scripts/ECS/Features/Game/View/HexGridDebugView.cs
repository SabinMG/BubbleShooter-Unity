using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas;

public class HexGridDebugView : MonoBehaviour, IHexGridLayoutListener, IHexGridSizeListener, IHexCellRadiusComponetListener
{
    #region public varibles
    public Vector2 _offsetCoord;
    [Range(0, 6)]
    public int neighbourDirection;
    #endregion

    #region private variables
    private float _radius;
    private HorizontalRowLayout _layoutType;
    private int _rows;
    private int _columns;
    private Contexts _context;
    private List<Vector2> _hexGridPosList;
    private GameObject _currentOffsetSphere;
    private GameObject _neighbourOffsetSphere;
    #endregion

    #region unity callbacks
    // Start is called before the first frame update
    void Start()
    {
        _context = Contexts.sharedInstance;
        _context.game.hexGridLayoutEntity.AddHexGridLayoutListener(this);
        _context.game.hexGridSizeEntity.AddHexGridSizeListener(this);
        _context.game.hexCellRadiusComponetEntity.AddHexCellRadiusComponetListener(this);
        _currentOffsetSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        _currentOffsetSphere.transform.localScale = new Vector3(.7f,.7f,.7f);
        _neighbourOffsetSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    void Update()
    {
        if (_hexGridPosList == null) return;
        HexOffsetCoord newOffCoord = new HexOffsetCoord((int)_offsetCoord.x, (int)_offsetCoord.y);

        Vector2 posCurent = GexGridService.HexOffsetCoordToPixel(_context, newOffCoord);
        _currentOffsetSphere.transform.position = new Vector3((float)posCurent.x, -(float)posCurent.y, 0);

        HexOffsetCoord neighBour = GexGridService.GetNeightbourCoord(_context, newOffCoord, neighbourDirection);
        //Debug.Log(" Row:" + neighBour.row + " column :" + neighBour.col);

        Vector2 pos = GexGridService.HexOffsetCoordToPixel(_context, neighBour);
        _neighbourOffsetSphere.transform.position = new Vector3((float)pos.x, -(float)pos.y, 0);
    }

    private void OnDrawGizmos()
    {
        if (_hexGridPosList == null) return;

        for (int i = 0; i < _hexGridPosList.Count; i++)
        {
            Vector2 pos = _hexGridPosList[i];
            Vector3 posVec3 = new Vector3((float)pos.x, -(float)pos.y, 0);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(posVec3, _radius);
        }
    }
    #endregion

    #region events
    public void OnHexGridLayout(GameEntity entity, HorizontalRowLayout layoutType)
    {
        _layoutType = layoutType;
        UpdateGrid();
    }

    public void OnHexGridSize(GameEntity entity, int rows, int columns)
    {
        _rows = rows;
        _columns = columns;
        UpdateGrid();
    }

    public void OnHexCellRadiusComponet(GameEntity entity, float value)
    {
        _radius = value;
        UpdateGrid();
    }
    #endregion

    #region private methods
    private void UpdateGrid()
    {
        _hexGridPosList = new List<Vector2>();
        for (int i = 0; i < _rows; i++)
        {
            for (int j = 0; j < _columns; j++)
            {
                HexOffsetCoord newOffCoord = new HexOffsetCoord(i, j);
                Vector2 pos = GexGridService.HexOffsetCoordToPixel(_context, newOffCoord);
                _hexGridPosList.Add(pos);
            }
        }
    }
    #endregion
}
