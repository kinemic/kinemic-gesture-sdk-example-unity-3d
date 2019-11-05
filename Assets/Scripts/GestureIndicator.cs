using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kinemic.Gesture;

// Small badge which listens for gestures and shows the icon of the gesture
public class GestureIndicator : MonoBehaviour
{
    public float Durarion = 2.0f;
    public float FadeDuration = 0.15f;

    public bool VibrateOnGesture = true;
    public float VibrateDuration = 0.3f;

    public Sprite SwipeR;
    public Sprite SwipeL;
    public Sprite SwipeUp;
    public Sprite SwipeDown;
    public Sprite RotateRL;
    public Sprite RotateLR;
    public Sprite CheckMark;
    public Sprite CrossMark;
    public Sprite CircleR;
    public Sprite CircleL;

    public SVGImage GestureImage;

    private CanvasGroup _canvasGroup;
    private bool _fadingIn;
    private bool _fadingOut;
    private float _changeRate;
    private string _band;

    private IEnumerator _currentCoroutine;
    private IEnumerator _currentCoroutineStarter;

    // Start is called before the first frame update
    void Start()
    {
        _canvasGroup = this.GetComponent<CanvasGroup>();
        SetAlpha(0);

        Engine.Instance.GestureDetected += (sender, e) =>
        {
            if (gameObject.activeInHierarchy)
            {
                _band = e.Band;
                ShowGesture(e.Gesture);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGesture(Gesture gesture)
    {
        if(VibrateOnGesture)
        {
            Engine.Instance.Vibrate(_band, (short)(VibrateDuration * 1000.0f));
        }

        if (_currentCoroutineStarter != null)
        {
            StopCoroutine(_currentCoroutineStarter);
        }

        _currentCoroutineStarter = DismissAndShowGestureCoroutine(gesture);

        StartCoroutine(_currentCoroutineStarter);
    }

    IEnumerator FadeIn()
    {
        _fadingIn = true;
        _changeRate = (1.0f - 0.0f) / FadeDuration;
        SetAlpha(0.0f);

        while (_fadingIn)
        {
            if (_canvasGroup.alpha > 0.99f)
            {
                _fadingIn = false;
                SetAlpha(1.0f);
                yield break;
            }
            else
            {
                SetAlpha(_canvasGroup.alpha + (_changeRate * Time.deltaTime));
            }

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        _fadingOut = true;
        _changeRate = (1.0f - 0.0f) / FadeDuration;

        while (_fadingOut)
        {
            if (_canvasGroup.alpha < 0.01f)
            {
                _fadingOut = false;
                SetAlpha(0);
                yield break;
            }
            else
            {
                SetAlpha(_canvasGroup.alpha - (_changeRate * Time.deltaTime));
            }

            yield return null;
        }
    }

    IEnumerator DismissAndShowGestureCoroutine(Gesture gesture)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;

            yield return FadeOut();
            yield return new WaitForSeconds(0.1f);
        }

        _currentCoroutine = ShowGestureCoroutine(gesture);

        yield return _currentCoroutine;

        _currentCoroutineStarter = null;
    }

    IEnumerator ShowGestureCoroutine(Gesture gesture)
    {
        if (SetGestureIcon(gesture))
        {
            yield return FadeIn();
            yield return new WaitForSeconds(Durarion);
            yield return FadeOut();
        }

        _currentCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        _canvasGroup.alpha = Mathf.Clamp(alpha, 0, 1);
    }

    private bool SetGestureIcon(Gesture gesture)
    {
        if (GestureImage == null) return false;

        switch(gesture)
        {
            case Gesture.CHECK_MARK:
                GestureImage.sprite = CheckMark;
                break;
            case Gesture.CROSS_MARK:
                GestureImage.sprite = CrossMark;
                break;
            case Gesture.SWIPE_R:
                GestureImage.sprite = SwipeR;
                break;
            case Gesture.SWIPE_L:
                GestureImage.sprite = SwipeL;
                break;
            case Gesture.SWIPE_UP:
                GestureImage.sprite = SwipeUp;
                break;
            case Gesture.SWIPE_DOWN:
                GestureImage.sprite = SwipeDown;
                break;
            case Gesture.CIRCLE_L:
                GestureImage.sprite = CircleL;
                break;
            case Gesture.CIRCLE_R:
                GestureImage.sprite = CircleR;
                break;
            case Gesture.ROTATE_RL:
                GestureImage.sprite = RotateRL;
                break;
            case Gesture.ROTATE_LR:
                GestureImage.sprite = RotateLR;
                break;
            default:
                return false;
        }

        return true;
    }
}
