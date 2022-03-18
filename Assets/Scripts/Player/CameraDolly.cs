using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraDolly : MonoBehaviour
{

    [Tooltip("The rail system that the camera will follow by default. Will be changed by gates eventually.")][SerializeField] private GameObject _currentRail = null;
    [Tooltip("The target the camera will best try and follor. Should be player or any other focal point. Could be changed by cutscene.")] public GameObject FocalPoint = null;

    //internals
    private List<Transform> nodes = new List<Transform>();
    private Transform _sectionEnd;
    private Transform _sectionStart;
    public void SetRail(GameObject Rail)
    {
        nodes = new List<Transform>();
        if (Rail)
        {
            foreach (Transform node in Rail.GetComponentsInChildren<Transform>().OrderBy(o => o.gameObject.name))
            {
                if (node != Rail.transform)
                {
                    nodes.Add(node);
                }
            }
        }
        _sectionStart = null;
        _sectionEnd = null;
    }

    void OnDrawGizmosSelected()
    {
        if (_currentRail)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < _currentRail.GetComponentsInChildren<Transform>().Length-1; i++)
            {
                if (_currentRail.GetComponentsInChildren<Transform>()[i] != _currentRail.transform)
                {
                    Gizmos.DrawLine(_currentRail.GetComponentsInChildren<Transform>()[i].position, _currentRail.GetComponentsInChildren<Transform>()[i + 1].position);
                }
            }
        }
    }

    private Vector3 getClosestPointOnSegment(Transform start, Transform end, Transform point)
    {
        Vector3 dir = (end.position - start.position).normalized;
        Vector3 closestPointOnLine = ((start.position - point.position) - Vector3.Dot((start.position - point.position), dir) * dir)+point.position;
        if (Vector2.Distance(closestPointOnLine, end.position) > Vector2.Distance(end.position, start.position))
        {
            closestPointOnLine = start.position;
        }
        else if (Vector2.Distance(closestPointOnLine, start.position) > Vector2.Distance(end.position, start.position))
        {
            closestPointOnLine = end.position;
        }
        return closestPointOnLine;
    }
    // Start is called before the first frame update
    void Start()
    {
        this.SetRail(_currentRail);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FocalPoint == null) return;
        if (_currentRail == null) return;
        if (nodes.Count < 2) return;
        if (_sectionStart == null || _sectionEnd == null || Vector3.Distance(this.transform.position,_sectionStart.position + new Vector3(0,0,this.transform.position.z)) <= 0.089f || Vector3.Distance(this.transform.position, _sectionEnd.position + new Vector3(0, 0, this.transform.position.z)) <= 0.089f)
        {
            _sectionStart = nodes.OrderBy(o => Vector2.Distance(o.position, FocalPoint.transform.position)).ToArray<Transform>()[0]; //sort by distance.
            int index = nodes.IndexOf(_sectionStart);
            if (index == 0)
            {
                _sectionEnd = nodes[index + 1];
            }
            else if (index == nodes.Count - 1)
            {
                _sectionEnd = nodes[index - 1];
            }
            else
            {
                Transform[] possiblilies = { nodes[nodes.IndexOf(_sectionStart) - 1], nodes[nodes.IndexOf(_sectionStart) + 1] }; //Get the two nodes that are linked to this one
                _sectionEnd = possiblilies.OrderBy(o => Vector2.Distance(getClosestPointOnSegment(_sectionStart,o,FocalPoint.transform), FocalPoint.transform.position)).ToArray<Transform>()[0]; //sort those by distance.
            }
        }
        Vector2 dir = (_sectionEnd.position-_sectionStart.position).normalized;
        Vector2 pos = getClosestPointOnSegment(_sectionStart, _sectionEnd, FocalPoint.transform);
        
        if (Vector2.Distance(this.transform.position,pos) > 0.0625f)
        {
            this.transform.position = new Vector3(pos.x - (pos.x % 0.0625f), pos.y - (pos.y % 0.0625f), this.transform.position.z);
        }

    }
}