using UnityEngine;
using Kinemic.Gesture;
using UnityEngine.Events;
using System;

// Manager for the Band Tab
public class BandTabManager : MonoBehaviour
{
    public Engine Engine => Engine.Instance;

    [Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    // events to toggle tab content
    public BoolEvent HasBand;
    public BoolEvent NoBand;

    public UnityEngine.UI.Text NoBandLabel;
    public UnityEngine.UI.Text DeviceName;
    public UnityEngine.UI.Text ConnectionStateLabel;
    public UnityEngine.UI.Text BatteryLabel;
    public UnityEngine.UI.Text StreamQualityLabel;
    public UnityEngine.UI.Dropdown HandednessDropdown;

    private bool _hasBand = false;

    // current band
    private string _band = null;

    // events to toggle tab content
    private void SetHasBand(bool hasBand)
    {
        if (hasBand != _hasBand)
        {
            _hasBand = hasBand;
            HasBand.Invoke(_hasBand);
            NoBand.Invoke(!_hasBand);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (HasBand == null) HasBand = new BoolEvent();
        if (NoBand == null) NoBand = new BoolEvent();

        Engine.ConnectionStateChanged += (sender, e) =>
        {
            _band = e.State != ConnectionState.DISCONNECTED ? e.Band : null;

            // update info on device panel
            if (DeviceName != null && _band != null) DeviceName.text = "Band " + _band.Substring(0, 5);
            if (ConnectionStateLabel != null && _band != null) ConnectionStateLabel.text = "" + e.State;

            SetHasBand(e.State != ConnectionState.DISCONNECTED);

            if (e.State == ConnectionState.CONNECTED)
            {
                UpdateBandInfo();
            }
        };

        Engine.SearchStarted += (sender) =>
        {
            if (NoBandLabel != null) NoBandLabel.text = "Searching...";
        };

        Engine.SearchStopped += (sender) =>
        {
            if (NoBandLabel != null) NoBandLabel.text = "No Band connected";
        };

        Engine.BatteryChanged += (sender, e) =>
        {
            SetBattery(e.Battery);
        };

        Engine.StreamQualityChanged += (sender, e) =>
        {
            SetStreamQuality(e.Quality);
        };

        HasBand.Invoke(_hasBand);
        NoBand.Invoke(!_hasBand);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ConnectStrongest() => Engine.ConnectStrongest();

    public void Disconnect()
    {
        if (_band != null)
        {
            Engine.Disconnect(_band);
        }
    }

    private void UpdateBandInfo()
    {
        if (_band != null)
        {
            int battery = Engine.GetBattery(_band);
            SetBattery(battery);

            int streamQuality = Engine.GetStreamQuality(_band);
            SetStreamQuality(streamQuality);
        }
    }

    private void SetBattery(int battery)
    {
        if (BatteryLabel != null) BatteryLabel.text = "" + battery + "%";
    }

    private void SetStreamQuality(int streamQuality)
    {
        if (StreamQualityLabel != null) StreamQualityLabel.text = "" + streamQuality + "%";
    }
}
