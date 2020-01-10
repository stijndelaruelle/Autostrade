using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private LevelTimeline m_LevelTimeline = null;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Direction direction = Direction.None;

        if (Input.GetKeyDown(KeyCode.UpArrow))    { direction = Direction.North; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { direction = Direction.East;  }
        if (Input.GetKeyDown(KeyCode.DownArrow))  { direction = Direction.South; }
        if (Input.GetKeyDown(KeyCode.LeftArrow))  { direction = Direction.West;  }

        if (direction != Direction.None)
        {
            Vector2Int offset = UtilityMethods.DirectionToVector2Int(direction);
            transform.position += new Vector3(offset.x, offset.y, 0.0f);

            if (m_LevelTimeline != null)
                m_LevelTimeline.NextFrame();
        }
    }
}
