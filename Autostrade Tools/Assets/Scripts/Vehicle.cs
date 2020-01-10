using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Vehicle : MonoBehaviour
{
    [SerializeField]
    private LevelTimeline m_LevelTimeline = null; //Could turn it into a Singleton

    [Header("Behaviour")]
    [Space(5)]
    [SerializeField]
    private int m_Speed = 0;
    public int Speed
    {
        get { return m_Speed; }
    }

    [SerializeField]
    private int m_StartFrame = 0;
    public int StartFrame
    {
        get { return m_StartFrame; }
    }

    [SerializeField]
    private List<Direction> m_Moves = null;

    [Space(5)]
    [Header("Bitsy Visuals")]
    [SerializeField]
    private int m_Width = 0;
    public int Width
    {
        get { return m_Width; }
    }

    [SerializeField]
    private int m_Height = 0;
    public int Height
    {
        get { return m_Height; }
    }

    //Note, if this were a real project this would be structured more neatly
    [Header("Frame 1")]
    [SerializeField]
    private List<string> m_TileReferencesF1Pos1 = null;

    [SerializeField]
    private List<string> m_TileReferencesF1Pos2 = null;

    [SerializeField]
    private List<string> m_TileReferencesF1Pos3 = null;

    [SerializeField]
    private List<string> m_TileReferencesF1Pos4 = null;

    [Header("Frame 2")]
    [SerializeField]
    private List<string> m_TileReferencesF2Pos1 = null;

    [SerializeField]
    private List<string> m_TileReferencesF2Pos2 = null;

    [SerializeField]
    private List<string> m_TileReferencesF2Pos3 = null;

    [SerializeField]
    private List<string> m_TileReferencesF2Pos4 = null;

    private void Start()
    {
        Debug.Log("Vehicle Start");
        if (m_LevelTimeline != null)
        {
            m_LevelTimeline.FrameChangedEvent += OnStepChanged;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Vehicle OnDestroy");
        if (m_LevelTimeline != null)
        {
            m_LevelTimeline.FrameChangedEvent -= OnStepChanged;
        }
    }

    public void SetPosition(int newFrame, int oldFrame)
    {
        Vector2Int offset = CalculateOffset(newFrame, oldFrame);
        transform.position += new Vector3(offset.x, offset.y, 0.0f);
    }

    public Vector2Int GetPosition(int newFrame, int oldFrame)
    {
        Vector2Int offset = CalculateOffset(newFrame, oldFrame);
        return new Vector2Int((int)transform.position.x, (int)transform.position.y) + offset;
    }

    private Vector2Int CalculateOffset(int newFrame, int oldFrame)
    {
        if (newFrame == oldFrame)
            return Vector2Int.zero;

        if (newFrame < 0 || oldFrame < 0)
            return Vector2Int.zero;

        if (newFrame < m_StartFrame && oldFrame < m_StartFrame)
            return Vector2Int.zero;

        int diff = newFrame - oldFrame;
        int sign = (int)Mathf.Sign(diff);

        //Calculate the offset from the current position
        //Note: Could also have calculated from the start, but this should be more efficient
        Vector2Int cummulativeOffset = new Vector2Int();

        for (int i = 0; i < Mathf.Abs(diff); ++i)
        {
            //Get direction
            Direction direction = Direction.North;

            if (sign > 0)
            {
                if (oldFrame + i >= m_Moves.Count + m_StartFrame) { direction = m_Moves[m_Moves.Count - 1]; }
                else if (oldFrame + i < m_StartFrame)             { direction = Direction.None; }
                else                                              { direction = m_Moves[oldFrame + i - m_StartFrame]; }
            }
            else
            {
                if (oldFrame - (i + 1) >= m_Moves.Count + m_StartFrame) { direction = m_Moves[m_Moves.Count - 1]; }
                else if (oldFrame - (i + 1) < m_StartFrame)             { direction = Direction.None; }
                else                                                    { direction = m_Moves[oldFrame - (i + 1) - m_StartFrame]; }

                direction = UtilityMethods.InvertDirection(direction);
            }

            //Add to offset
            cummulativeOffset += UtilityMethods.DirectionToVector2Int(direction) * m_Speed;
        }

        return cummulativeOffset;
    }

    public List<string> GetBitsyTileReferences()
    {
        //Even = Frame 1
        int pos = (int)transform.position.x - 6;

        //Extra offset if it's a truck (cheaty hardcode)
        if (m_Width == 3)
            pos += 1;

        //Super dirty, should be structured way neater. But I didn't want to write a custom inspector
        if ((int)transform.position.y % 2 == 0)
        {
            switch (pos)
            {
                case 0: return m_TileReferencesF1Pos1;
                case 1: return m_TileReferencesF1Pos2;
                case 2: return m_TileReferencesF1Pos3;
                case 3: return m_TileReferencesF1Pos4;

                default:
                    return m_TileReferencesF1Pos1;
            }
        }
        else
        {
            switch (pos)
            {
                case 0: return m_TileReferencesF2Pos1;
                case 1: return m_TileReferencesF2Pos2;
                case 2: return m_TileReferencesF2Pos3;
                case 3: return m_TileReferencesF2Pos4;

                default:
                    return m_TileReferencesF1Pos1;
            }
        }
    }

    public List<Vector2Int> GetHitboxes()
    {
        Direction direction = Direction.North;

        if (m_LevelTimeline.CurrentFrame >= m_Moves.Count + m_StartFrame) { direction = m_Moves[m_Moves.Count - 1]; }
        else if (m_LevelTimeline.CurrentFrame < m_StartFrame)             { direction = Direction.None; }
        else                                                              { direction = m_Moves[m_LevelTimeline.CurrentFrame - m_StartFrame]; }

        Vector2Int offset = UtilityMethods.DirectionToVector2Int(direction);
        List<Vector2Int> hitboxes = new List<Vector2Int>();

        int posX = (int)transform.position.x;
        int posY = (int)transform.position.y * -1; //Bitsy has a different axis system

        for (int step = 0; step <= m_Speed; ++step) //0 = current position
        {
            for (int heightY = 0; heightY < m_Height; ++heightY)
            {
                for (int widthX = 0; widthX < m_Width; ++widthX)
                {
                    Vector2Int hitbox = new Vector2Int(posX + widthX + (offset.x * step), posY + heightY - (offset.y * step));

                    if (hitboxes.Contains(hitbox) == false)
                    {
                        //Add an end point (hitbox)
                        hitboxes.Add(hitbox);
                    }   
                }
            }
        }

        return hitboxes;
    }

    //Callback
    private void OnStepChanged(int newFrame, int oldFrame)
    {
        SetPosition(newFrame, oldFrame);
    }
}
