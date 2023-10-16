using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool IsWalkable { get; set; }
    public Vector2 NodePosition { get; set; }

    public Node(bool isWalkable, Vector2 nodePosition)
    {
        IsWalkable = isWalkable;
        NodePosition = nodePosition;
    }
}
