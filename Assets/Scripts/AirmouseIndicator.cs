using UnityEngine;
using Kinemic.Gesture;

// Indicator for airmouse state and actions
public class AirmouseIndicator : MonoBehaviour
{

    public SVGImage AngleIndicator;
    public SVGImage AirmouseState;
    public SVGImage AirmouseActionDownwards;
    public SVGImage AirmouseActionSideways;
    public SVGImage AirmouseActionUpwards;
    public Sprite AirmouseStateUpwards;
    public Sprite AirmouseStateDownwards;
    public Sprite AirmouseStateSideways;
    public Sprite AirmouseStateInconclusive;
    public Sprite DownwardsAction;
    public Sprite DownwardsActionSelected;
    public Sprite SidewaysAction;
    public Sprite SidewaysActionSelected;
    public Sprite UpwardsAction;
    public Sprite UpwardsActionSelected;

    private AirMousePalmFacing _facing;

    // Start is called before the first frame update
    void Start()
    {
        SetFacing(AirMousePalmFacing.PALM_FACING_INCONCLUSIVE);
        if (AngleIndicator != null) AngleIndicator.gameObject.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -90.0f);

        // listen for airmouse moved events
        Engine.Instance.AirmouseMoved += (sender, e) =>
        {
            // Rotate the angle indicator to match the wrist angle
            if (AngleIndicator != null) AngleIndicator.transform.rotation = Quaternion.Euler(0, 0, -e.WristAngle);

            // set the indicator image for the current palm facing
            SetFacing(e.PalmFacing);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void SetFacing(AirMousePalmFacing facing)
    {
        if (facing != _facing)
        {
            _facing = facing;

            if (AirmouseActionDownwards != null) AirmouseActionDownwards.setSpriteOrHide(DownwardsAction);
            if (AirmouseActionSideways != null) AirmouseActionSideways.setSpriteOrHide(SidewaysAction);
            if (AirmouseActionUpwards != null) AirmouseActionUpwards.setSpriteOrHide(UpwardsAction);

            switch (facing)
            {
                case AirMousePalmFacing.PALM_FACING_DOWNWARDS:
                    if (AirmouseActionDownwards != null) AirmouseActionDownwards.setSpriteOrHide(DownwardsActionSelected);
                    if (AirmouseState != null) AirmouseState.sprite = AirmouseStateDownwards;
                    break;
                case AirMousePalmFacing.PALM_FACING_SIDEWAYS:
                    if (AirmouseActionSideways != null) AirmouseActionSideways.setSpriteOrHide(SidewaysActionSelected);
                    if (AirmouseState != null) AirmouseState.sprite = AirmouseStateSideways;
                    break;
                case AirMousePalmFacing.PALM_FACING_UPWARDS:
                    if (AirmouseActionUpwards != null) AirmouseActionUpwards.setSpriteOrHide(UpwardsActionSelected);
                    if (AirmouseState != null) AirmouseState.sprite = AirmouseStateUpwards;
                    break;
                case AirMousePalmFacing.PALM_FACING_INCONCLUSIVE:
                    if (AirmouseState != null) AirmouseState.sprite = AirmouseStateInconclusive;
                    break;
            }
        }
    }
}

public static class SVGExtensions
{
    public static void setSpriteOrHide(this SVGImage image, Sprite sprite)
    {
        if (sprite == null)
        {
            image.sprite = sprite;
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
            image.sprite = sprite;
        }
    }
}


