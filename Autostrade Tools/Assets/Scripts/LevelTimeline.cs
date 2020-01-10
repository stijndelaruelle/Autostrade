using UnityEngine;

[ExecuteAlways]
public class LevelTimeline : MonoBehaviour
{
    public delegate void StepDelegate(int currentStep, int previousStep);

    [SerializeField]
    private int m_CurrentFrame = 0;
    public int CurrentFrame
    {
        get { return m_CurrentFrame; }
    }

    private static int s_TimelineMinRange = 0;
    public int TimelineMinRange
    {
        get { return s_TimelineMinRange; }
    }

    private static int s_TimelineMaxRange = 25;
    public int TimelineMaxRange
    {
        get { return s_TimelineMaxRange; }
    }

    //Events
    public event StepDelegate FrameChangedEvent = null;

    public void SetFrame(int step)
    {
        if (step < 0)
            return;

        if (FrameChangedEvent != null)
            FrameChangedEvent(step, m_CurrentFrame);

        m_CurrentFrame = step;
    }

    public void PreviousFrame()
    {
        SetFrame(m_CurrentFrame - 1);
    }

    public void NextFrame()
    {
        SetFrame(m_CurrentFrame + 1);
    }
}
