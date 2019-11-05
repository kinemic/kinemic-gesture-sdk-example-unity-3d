using UnityEngine;
using Kinemic.Gesture;

// Manager for the Airmouse Tab
public class AirmouseTabManager : MonoBehaviour
{
    public UnityEngine.GameObject AirmouseCanvasGroup;
    public UnityEngine.GameObject AirmouseGroup;
    public UnityEngine.GameObject TabPanel;

    public UnityEngine.UI.Slider HorizontalIndicator;
    public UnityEngine.UI.Slider VerticalIndicator;

    // Start is called before the first frame update
    void Start()
    {
        string[] bands = Engine.Instance.GetBands();
        if (bands.Length == 1)
        {
            Engine.Instance.StartAirmouse(bands[0]);
        } 
        

        // make sure to start airmouse for connected bands
        Engine.Instance.ConnectionStateChanged += (sender, e) =>
        {
            if (e.State == ConnectionState.CONNECTED)
            {
                Engine.Instance.StartAirmouse(e.Band);
            }
        };

        // animate slider valued for x,y
        Engine.Instance.AirmouseMoved += (sender, e) =>
        {
            if (HorizontalIndicator) HorizontalIndicator.value = e.X;
            if (VerticalIndicator) VerticalIndicator.value = e.Y;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideDialog()
    {
        if (TabPanel != null) TabPanel.SetActive(false);
        if (AirmouseGroup != null) AirmouseGroup.gameObject.SetActive(true);
        if (AirmouseCanvasGroup != null) AirmouseCanvasGroup.gameObject.SetActive(true);
    }

    public void ShowDialog()
    {
        if (TabPanel != null) TabPanel.SetActive(true);
        if (AirmouseGroup != null) AirmouseGroup.gameObject.SetActive(false);
        if (AirmouseCanvasGroup != null) AirmouseCanvasGroup.gameObject.SetActive(false);
    }

    
}
