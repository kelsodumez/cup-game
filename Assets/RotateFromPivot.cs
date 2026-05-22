using UnityEngine;

public class RotateFromPivot : MonoBehaviour
{
    [SerializeField] private Transform _centrePoint;
    private float _centreScale;
    private ArmController.hand_target _currentHand;
    private Vector3 _pivotPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _currentHand = GetComponent<Hand>().HandType;
    }

    void Start()
    {
        _centreScale = _centrePoint.transform.lossyScale.x;
        switch (_currentHand)
        {
            case ArmController.hand_target.left:
                _pivotPoint = new Vector3(_centrePoint.transform.position.x + _centreScale, _centrePoint.transform.position.y, _centrePoint.transform.position.z);
                break;
            case ArmController.hand_target.right:
                _pivotPoint = new Vector3(_centrePoint.transform.position.x - _centreScale, _centrePoint.transform.position.y, _centrePoint.transform.position.z);
                break;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateToPivot();
    }

    private void RotateToPivot()
    {
        transform.rotation = Quaternion.LookRotation(_pivotPoint - transform.position);
    }
}
