using UnityEngine;
using System.Collections;

public abstract class Buildings:MonoBehaviour
{
    public Vector2 buildSpace;
    public Vector3 startPos;

    public Buildings()
    {
    }

    public void BuildingsConstructor(Vector2 _buildSpace,Vector3 _startPos)
    {
        buildSpace = _buildSpace;
        startPos = _startPos;
    }


}
