using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to attach to objects to make them moveable with the laser pointer
public class Moveable : MonoBehaviour
{
    public Material MaterialDefault;

    public Material MaterialHovered;

    public Material MaterialDragging;

    private Renderer _renderer;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (MaterialDefault == null) MaterialDefault = _renderer.material;
        else _renderer.material = MaterialDefault;
        if (MaterialHovered == null) MaterialHovered = MaterialDefault;
        if (MaterialDragging == null) MaterialDragging = MaterialDefault;

        LaserPointer _laserPointer = FindObjectOfType<LaserPointer>();

        if (_laserPointer != null)
        {
            _laserPointer.PointerDragged += (sender, e) =>
            {
                if (e.target.gameObject == gameObject)
                {
                    Vector3 origin = new Vector3(0.0f, 0.0f, -10.0f);
                    Vector3 xAxis = new Vector3(1.0f, 0.0f, 0.0f);
                    Vector3 yAxis = new Vector3(0.0f, 1.0f, 0.0f);


                    Quaternion orientation = e.target.transform.rotation;
                    e.target.transform.RotateAround(origin, yAxis, e.dx);
                    e.target.transform.RotateAround(origin, xAxis, -e.dy);
                    e.target.transform.rotation = orientation;

                    Rigidbody _rb = e.target.gameObject.GetComponent<Rigidbody>();
                    if (_rb != null)
                    {
                        _rb.velocity = Vector3.zero;
                        _rb.useGravity = false;
                        _rb.isKinematic = false;
                        _rb.freezeRotation = true;
                    }

                    _renderer.material = MaterialDragging;
                }
            };

            _laserPointer.PointerOut += (sender, e) =>
            {
                if (e.target.gameObject == gameObject)
                {
                    Rigidbody _rb = e.target.gameObject.GetComponent<Rigidbody>();
                    if (_rb != null)
                    {
                        _rb.useGravity = true;
                        _rb.isKinematic = false;
                        _rb.freezeRotation = false;
                    }

                    _renderer.material = MaterialDefault;
                }
            };

            _laserPointer.PointerIn += (sender, e) =>
            {
                if (e.target.gameObject == gameObject)
                {
                    Rigidbody _rb = e.target.gameObject.GetComponent<Rigidbody>();
                    if (_rb != null)
                    {
                        //_rb.useGravity = false;
                        //_rb.isKinematic = false;
                        //_rb.freezeRotation = true;
                    }

                    _renderer.material = MaterialHovered;
                }
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
