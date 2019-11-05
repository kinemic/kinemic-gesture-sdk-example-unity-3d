using UnityEngine;
using Kinemic.Gesture;

public class GestureTabManager : MonoBehaviour
{
    public UnityEngine.Video.VideoPlayer VideoPlayer;
    public UnityEngine.UI.Text NameLabel;

    private Gesture _currentGesture;

    private int _currentIndex = 0;

    private Gesture[] _gestureSequence = { Gesture.SWIPE_R, Gesture.SWIPE_L, Gesture.CHECK_MARK, Gesture.CROSS_MARK };

    // set video clip for gesture
    private void SetGesture(Gesture gesture)
    {
        _currentGesture = gesture;

        switch(gesture)
        {
            case Gesture.SWIPE_R:
                if (NameLabel != null) NameLabel.text = "Swipe Right";
                if (VideoPlayer != null) VideoPlayer.clip = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Video.VideoClip>("Assets/Videos/Swipe R.mp4");
                break;
            case Gesture.SWIPE_L:
                if (NameLabel != null) NameLabel.text = "Swipe Left";
                if (VideoPlayer != null) VideoPlayer.clip = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Video.VideoClip>("Assets/Videos/Swipe L.mp4");
                break;
            case Gesture.CHECK_MARK:
                if (NameLabel != null) NameLabel.text = "Check Mark";
                if (VideoPlayer != null) VideoPlayer.clip = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Video.VideoClip>("Assets/Videos/Check Mark.mp4");
                break;
            case Gesture.CROSS_MARK:
                if (NameLabel != null) NameLabel.text = "Cross Mark";
                if (VideoPlayer != null) VideoPlayer.clip = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Video.VideoClip>("Assets/Videos/X Mark.mp4");
                break;
        }
    }

    private void SetGestureIndex(int index)
    {
        _currentIndex = index;
        SetGesture(_gestureSequence[index]);
    }

    public void NextGesture()
    {
        SetGestureIndex((_currentIndex + 1) % _gestureSequence.Length);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGestureIndex(0);

        // listen for gestures and check if the detected gesture was the one shown on screen
        Engine.Instance.GestureDetected += (sender, e) =>
        {
            if (e.Gesture == _currentGesture)
            {
                // TODO success animation
                NextGesture();
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
