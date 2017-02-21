using UnityEngine;

public class RailCrawler : MonoBehaviour
{
    [NonNull]
    public RailNode StartNode;
    public float Tau = 0.5f;

    private float _railProgress = 0;

    private RailNode _previousNode;
    private RailNode _currentNode;

    private Vector3 _lastRailPosition = Vector3.zero;

    private bool _reachedEnd = false;

    // units/s^2
    public float Acceleration = 0.25f;

    public float Speed
    {
        get;
        set;
    }

    public Vector3 Position
    {
        get;
        private set;
    }

    public Vector3 Direction
    {
        get;
        private set;
    }

    private float _realSpeed;

    // Use this for initialization
    void Start()
    {
        Asserter.NotNull(StartNode, "RailCrawler.Start:StartNode is null");
        Asserter.NotNull(StartNode.NextNode, "RailCrawler.Start:StartNode.NextNode is null");
        Asserter.NotNull(StartNode.NextNode.NextNode, "RailCrawler.Start:StartNode.NextNode.NextNode is null");
        Asserter.NotNull(StartNode.NextNode.NextNode.NextNode, "RailCrawler.StartNode.NextNode.NextNode.NextNode is null");

        _previousNode = StartNode;
        _currentNode = StartNode.NextNode;

        _lastRailPosition = StartNode.transform.position;

        Vector3 startingPoint = GetPosition(_previousNode, 0, Tau);
        Vector3 nextPoint = GetPosition(_previousNode, 0.0166667f, Tau);

        Direction = (nextPoint - startingPoint).normalized;
        Position = startingPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (_reachedEnd)
        {
            return;
        }

        _realSpeed = Mathf.MoveTowards(_realSpeed, Speed, Acceleration * Time.deltaTime);
        if (_realSpeed != 0)
        {
            _railProgress += Time.deltaTime * _realSpeed;

            while (_railProgress > 1.0f)
            {
                _railProgress -= 1;

                _previousNode = _currentNode;
                _currentNode = _currentNode.NextNode;
                if (_currentNode == null || _currentNode.NextNode == null || _currentNode.NextNode.NextNode == null)
                {
                    _reachedEnd = true;
                    return;
                }
            }

            _lastRailPosition = Position;
            Position = GetPosition(_previousNode, _railProgress, Tau);
            Direction = (Position - _lastRailPosition).normalized;

        }
    }

    private Vector3 GetPosition(RailNode startNode, float time, float tau)
    {
        // Start here:
        //  Need to check all nodes from startNode.NextNode... 3 nodes out are all non-null.
        // If any of them is null, set position to....current position? That's the hacky way of doing it.
        // It should be the position of startNode.NextNode.

        return MathExt.CatmullRom2(startNode.transform.position, startNode.NextNode.transform.position, startNode.NextNode.NextNode.transform.position, startNode.NextNode.NextNode.NextNode.transform.position, time, Tau);
    }
}
