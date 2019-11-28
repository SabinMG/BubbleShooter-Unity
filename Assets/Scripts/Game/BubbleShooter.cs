using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Entitas.Unity;
using DG.Tweening;
using Entitas;

public class BubbleShooter : MonoBehaviour
{
    #region public varaibles
    public Transform _bubbleShooterHint;
    public Transform[] _shootBubblesPos;
    #endregion

    #region private variables
    private bool _mouseDown = false;
    private LineRenderer _lineRenderer;
    private List<Vector2> _rayHitPoints;
    private Vector3[] _linePositions = new Vector3[3];
    private Contexts _contexts;
    private HexOffsetCoord _lastHintCoordinate;
    private GameEntity[] _shootBubbles = new GameEntity[2];
    #endregion

    #region unity callbacks
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;
        _rayHitPoints = new List<Vector2>();
        _bubbleShooterHint.gameObject.SetActive(false);
        _contexts = Contexts.sharedInstance;
        for (int i = 0; i < 2; i++ ) // two shoot bubbles
        {
            _shootBubbles[i] = CreatNewShootBubble();
        }
        Invoke("ShowTwoShootBubbles", .5f);
    }
    #endregion

    #region public methods
    public void Execute()
    {
        if (Input.touches.Length > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                HandleTouchDown(touch.position);
            }
            else if (touch.phase == TouchPhase.Canceled || touch.phase == TouchPhase.Ended)
            {
                HandleTouchUp(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                HandleTouchMove(touch.position);
            }
            HandleTouchMove(touch.position);
            return;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            _mouseDown = true;
            HandleTouchDown(Input.mousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _mouseDown = false;
            HandleTouchUp(Input.mousePosition);
        }
        else if (_mouseDown)
        {
            HandleTouchMove(Input.mousePosition);
        }
    }
    #endregion

    #region 
    private void ShowTwoShootBubbles()
    {
        for (int i = 0; i < 2; i++) // two shoot bubbles
        {
            BubbleView bubbleView = (BubbleView)_shootBubbles[i].view.value;
            bubbleView.GetComponent<CircleCollider2D>().enabled = false;
        }

        Vector3 myVector = _shootBubblesPos[1].position;
        DOTween.To(() => myVector, x => myVector = x, _shootBubblesPos[0].position, .3f).OnUpdate(() =>
        {
            _shootBubbles[0].ReplacePosition(myVector);
        });
    }

    private void HandleTouchDown(Vector2 touch)
    {
       
    }

    private void HandleTouchUp(Vector2 touch)
    {
        _lineRenderer.enabled = false;
        _bubbleShooterHint.gameObject.SetActive(false);
        ShootBubble();
    }

    private void ShootBubble()
    {
        _shootBubbles[0].AddShooter(false);
        int hitPointsCount = _rayHitPoints.Count;
        Vector3 bubblePos = GexGridService.HexOffsetCoordToPixel(_contexts, _lastHintCoordinate);

        Vector3 shootEndPoint = new Vector3(bubblePos.x, -bubblePos.y, 0);
        Vector3 shootMidPoint = hitPointsCount == 2?(Vector3)_rayHitPoints[0]:shootEndPoint;
        Vector3 myVector = _shootBubbles[0].position.value; // start point

        bool shakeNeighbours = false;

        // should rewrite the folllowing lines, or move to any new system
        DOTween.To(() => myVector, x => myVector = x, shootMidPoint, .14f).OnUpdate(() =>
        {
            _shootBubbles[0].ReplacePosition(new Vector2(myVector.x, myVector.y));
        }).OnComplete(()=> 
        {
            myVector = _shootBubbles[0].position.value;
            DOTween.To(() => myVector, x => myVector = x, shootEndPoint, .14f).OnUpdate(() =>
            {
                _shootBubbles[0].ReplacePosition(new Vector2(myVector.x, myVector.y));

                float beforEns = Vector2.SqrMagnitude(shootEndPoint - myVector);
                if (beforEns < .01f && !shakeNeighbours)
                {
                    _shootBubbles[0].AddHexCell(new Vector2Int(_lastHintCoordinate.row, _lastHintCoordinate.col));
                    _shootBubbles[0].ReplaceShooter(true);
                    shakeNeighbours = true;
                    _shootBubbles[0].AddMerge(true,false);
                }

            }).OnComplete(() =>
            {
               
                BubbleView bubbleView = (BubbleView)_shootBubbles[0].view.value;
                bubbleView.GetComponent<CircleCollider2D>().enabled = true;
                if(!_shootBubbles[0].hasHexCell) _shootBubbles[0].AddHexCell(new Vector2Int(_lastHintCoordinate.row, _lastHintCoordinate.col));

                _shootBubbles[0] = _shootBubbles[1];
                _shootBubbles[1] = CreatNewShootBubble();

                Invoke("ShowTwoShootBubbles", .1f);
                // create new shooter buuble
            });
        });
    }

    private void HandleTouchMove(Vector2 touch)
    {
        _rayHitPoints.Clear();
        Vector2 point = Camera.main.ScreenToWorldPoint(touch);

        _lineRenderer.enabled = true;
        var direction = new Vector2(point.x - transform.position.x, point.y - transform.position.y);

        Vector3 endPoint = transform.position + new Vector3(direction.x, direction.y,0);
        Debug.DrawLine(transform.position, endPoint, Color.yellow);

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Boundary")
            {
                _rayHitPoints.Add(hit.point);
                DoRayCast(hit, direction);
            }
            else if(hit.collider.tag == "Bubble")
            {
                ShowShootHintOnBubbleHit(hit); 
                _rayHitPoints.Add(hit.point);
                DrawPaths();
            }
        }
    }

    private void DoRayCast(RaycastHit2D previousHit, Vector2 directionIn)
    {
        var normal = Mathf.Atan2(previousHit.normal.y, previousHit.normal.x);
        var newDirection = normal + (normal - Mathf.Atan2(directionIn.y, directionIn.x));
        var reflection = new Vector2(-Mathf.Cos(newDirection), -Mathf.Sin(newDirection));
        var newCastPoint = previousHit.point+ (.2f * reflection);

        Vector3 endPoint = newCastPoint + new Vector2(reflection.x, reflection.y);
        Debug.DrawLine(newCastPoint, endPoint, Color.yellow);

        var hit2 = Physics2D.Raycast(newCastPoint, reflection);
        if (hit2.collider != null)
        {
            if (hit2.collider.tag == "Boundary")
            {
               _rayHitPoints.Clear();
               _lineRenderer.enabled = false;
               _bubbleShooterHint.gameObject.SetActive(false);
            }
            else if (hit2.collider.tag == "Bubble")
            {
                ShowShootHintOnBubbleHit(hit2);
                _rayHitPoints.Add(hit2.point);
                DrawPaths();
            }
        }
    }

    private void DrawPaths()
    {
        _linePositions[0] = transform.position;
        int hitPointsCount = _rayHitPoints.Count;

        if (hitPointsCount == 1)
        {
            Vector3 endPoint = new Vector3(_rayHitPoints[0].x, _rayHitPoints[0].y, 0);
            Vector3 midPosint = (transform.position + endPoint) / 2.0f;
            _linePositions[1] = midPosint;
            _linePositions[2] = _rayHitPoints[0];
        }
        else if (hitPointsCount == 2)
        {
            _linePositions[1] = _rayHitPoints[0];
            _linePositions[2] = _rayHitPoints[1];
        }
        _lineRenderer.SetPositions(_linePositions);
        _lineRenderer.enabled = true;
    }

    private void ShowShootHintOnBubbleHit(RaycastHit2D bubbleHit)
    {
        EntityLink entityLink = bubbleHit.collider.gameObject.GetEntityLink();
        if (entityLink == null) return;

        GameEntity entity = (GameEntity)entityLink.entity;
        float angle = Vector2.Angle(bubbleHit.normal, Vector2.up);
        bool sameSide = angle > 120 ? false : true;
        float dotValue = Vector2.Dot(bubbleHit.normal,Vector2.right);
        int neighbourDirection = 0;

        if (dotValue > 0) neighbourDirection = sameSide ? 0 : 5;
        else neighbourDirection = sameSide ? 3 : 4;

        HexOffsetCoord neighbour = GexGridService.GetNeightbourCoord(_contexts, new HexOffsetCoord(entity.hexCell.coordinate.x, entity.hexCell.coordinate.y), neighbourDirection);
        if (!IsNeighbourValid(neighbour)) return;
  
        if (neighbour.row == _lastHintCoordinate.row && neighbour.col == _lastHintCoordinate.col) return;
        _lastHintCoordinate = neighbour;

        Vector2 bubblePos = GexGridService.HexOffsetCoordToPixel(_contexts, neighbour);
        Vector3 newBubbleHintPosition = new Vector3(bubblePos.x, -bubblePos.y, 0);
        _bubbleShooterHint.position = newBubbleHintPosition;
        _bubbleShooterHint.DOKill();
        _bubbleShooterHint.localScale = Vector3.zero;
        _bubbleShooterHint.transform.DOScale(Vector3.one, .2f);
        _bubbleShooterHint.gameObject.SetActive(true);
    }

    private bool IsNeighbourValid(HexOffsetCoord neighBour)
    {
        if (neighBour.row < 0 || neighBour.col < 0) return false;

        Vector2Int neighBourCoord = new Vector2Int(neighBour.row, neighBour.col);
        GameEntity neighbourCell = _contexts.game.GetNeighbourWithCellValue(neighBourCoord);

        if (neighbourCell != null) return false;
       return true;
    }

    private GameEntity CreatNewShootBubble()
    {
        GameEntity bubbleEntity = _contexts.game.CreateEntity();
        int[] primitiveBubles = BubbleService.primitiveBubbles;
        int random = Random.Range(0, primitiveBubles.Length);
        bubbleEntity.AddBubble(primitiveBubles[random]);
        bubbleEntity.AddAsset("Bubble");
        bubbleEntity.AddPosition(new Vector2(_shootBubblesPos[1].position.x, _shootBubblesPos[1].position.y)); // we create always at the second position
        return bubbleEntity;
    }
    #endregion
}
