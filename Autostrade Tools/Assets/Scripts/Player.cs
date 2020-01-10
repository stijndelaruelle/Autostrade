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
            //We go the the next from no matter what
            if (m_LevelTimeline != null)
                m_LevelTimeline.NextFrame();

            //Move the player
            Vector2Int offset = UtilityMethods.DirectionToVector2Int(direction);
            Vector3 newPosition = transform.position + new Vector3(offset.x, offset.y, 0.0f);

            //Normally these variables should have a better home. But for now this is fine
            if (newPosition.x < BitsyExporter.s_PlayableZoneStartX)
                return;

            if (newPosition.x > BitsyExporter.s_PlayableZoneEndX)
                return;

            if (newPosition.y > BitsyExporter.s_PlayableZoneStartY * -1)
                return;

            if (newPosition.y < BitsyExporter.s_PlayableZoneEndY * -1)
                return;

            transform.position = newPosition;
        }
    }
}
