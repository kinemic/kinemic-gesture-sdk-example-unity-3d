using UnityEngine;
using Kinemic.Gesture;

// Kinemic Band Laser Pointer
// Orientation of the Kinemic Band is mapped to a laser pointer.
// Rotating the wrist toggles the grab mode.
public class LaserPointer : MonoBehaviour
{
    private bool interactWithUI = false;
    private bool drag = false;
    private bool select = false;

    public bool active = true;
    public Color color;
    public float thickness = 0.002f;
    public Color clickColor = Color.green;
    public GameObject holder;
    public GameObject pointer;
    bool isActive = false;
    public bool addRigidBody = false;
    public Transform reference;
    public event PointerEventHandler PointerIn;
    public event PointerEventHandler PointerOut;
    public event PointerEventHandler PointerClick;
    public event PointerEventHandler PointerDragged;

    private float dx;
    private float dy;

    private AirmouseMovedEventArgs lastEvent;
    private AirmouseMovedEventArgs lastDrag;
    private AirmouseMovedEventArgs firstDrag;
    private AirmouseMovedEventArgs lastPoint;

    private float lastAdjustedX;
    private float lastAdjustedY;

    Transform previousContact = null;

    private void AirmouseMoved(object sender, AirmouseMovedEventArgs e)
    {
        select = false;
        drag = false;
        if (lastEvent != null)
        {
            // do not move laser when rotating wrist
            if (lastEvent.PalmFacing == e.PalmFacing && Mathf.Abs(e.WristAngle - lastEvent.WristAngle) < 2.5)
            {
                interactWithUI = e.PalmFacing == AirMousePalmFacing.PALM_FACING_SIDEWAYS;
                if (e.PalmFacing == AirMousePalmFacing.PALM_FACING_SIDEWAYS) // drag mode
                {
                    if (firstDrag == null)
                    {
                        firstDrag = e;
                    } else
                    {
                        drag = true;

                        // adjust x,y for offset from orientation change
                        float adjustedX = lastPoint.X + e.X - firstDrag.X;
                        float adjustedY = lastPoint.Y + e.Y - firstDrag.Y;

                        if (lastDrag != null)
                        {
                            dx = adjustedX - lastAdjustedX;
                            dy = adjustedY - lastAdjustedY;
                        }


                        transform.rotation = Quaternion.Euler(-adjustedY, adjustedX, 0);
                        lastDrag = e;

                        lastAdjustedX = adjustedX;
                        lastAdjustedY = adjustedY;
                    }
                } else
                {
                    firstDrag = null;
                    lastDrag = null;
                }


                if (e.PalmFacing == AirMousePalmFacing.PALM_FACING_DOWNWARDS) // pointer mode
                {
                    select = true;
                    transform.rotation = Quaternion.Euler(-e.Y, e.X, 0);
                    lastPoint = e;
                    firstDrag = null;
                }

            }
            else
            {
                // orientation changed
            }
        }
        lastEvent = e;
    }

    

    private void Start()
    {
        Engine.Instance.AirmouseMoved += AirmouseMoved;

        holder = new GameObject();
        holder.transform.parent = this.transform;
        holder.transform.localPosition = Vector3.zero;
        holder.transform.localRotation = Quaternion.identity;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.transform.localScale = new Vector3(thickness, thickness, 100f);
        pointer.transform.localPosition = new Vector3(0f, 0f, 50f);
        pointer.transform.localRotation = Quaternion.identity;
        BoxCollider collider = pointer.GetComponent<BoxCollider>();
        if (addRigidBody)
        {
            if (collider)
            {
                collider.isTrigger = true;
            }
            Rigidbody rigidBody = pointer.AddComponent<Rigidbody>();
            rigidBody.isKinematic = true;
        }
        else
        {
            if (collider)
            {
                Object.Destroy(collider);
            }
        }
        //Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        //newMaterial.SetColor("_Color", color);
        //pointer.GetComponent<MeshRenderer>().material = newMaterial;
    }

    public virtual void OnPointerIn(PointerEventArgs e)
    {
        PointerIn?.Invoke(this, e);
    }

    public virtual void OnPointerClick(PointerEventArgs e)
    {
        PointerClick?.Invoke(this, e);
    }

    public virtual void OnPointerOut(PointerEventArgs e)
    {
        PointerOut?.Invoke(this, e);
    }

    public virtual void OnPointerDragged(PointerEventArgs e)
    {
        PointerDragged?.Invoke(this, e);
    }


    private void Update()
    {
        if (!isActive)
        {
            isActive = true;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }

        float dist = 100f;

        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit);

        if (select)
        {
            if (previousContact && previousContact != hit.transform)
            {
                PointerEventArgs args = new PointerEventArgs();
                //args.fromInputSource = pose.inputSource;
                args.distance = 0f;
                args.flags = 0;
                args.target = previousContact;
                OnPointerOut(args);
                previousContact = null;
            }
            if (bHit && previousContact != hit.transform)
            {
                PointerEventArgs argsIn = new PointerEventArgs();
                //argsIn.fromInputSource = pose.inputSource;
                argsIn.distance = hit.distance;
                argsIn.flags = 0;
                argsIn.target = hit.transform;
                OnPointerIn(argsIn);
                previousContact = hit.transform;
            }
            if (!bHit)
            {
                previousContact = null;
            }
        }

        if (drag && previousContact != null)
        {
            PointerEventArgs argsIn = new PointerEventArgs();
            //argsIn.fromInputSource = pose.inputSource;
            argsIn.distance = hit.distance;
            argsIn.flags = 0;
            argsIn.target = previousContact;
            argsIn.dx = dx;
            argsIn.dy = dy;
            dx = 0.0f;
            dy = 0.0f;
            OnPointerDragged(argsIn);
        }

        if (bHit && hit.distance < 100f)
        {
            dist = hit.distance;
        }

        if (bHit && interactWithUI)
        {
            PointerEventArgs argsClick = new PointerEventArgs();
            //argsClick.fromInputSource = pose.inputSource;
            argsClick.distance = hit.distance;
            argsClick.flags = 0;
            argsClick.target = hit.transform;
            OnPointerClick(argsClick);
        }

        if (drag && previousContact != null)
        {
            pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);
            pointer.GetComponent<MeshRenderer>().material.color = clickColor;
        }
        else
        {
            pointer.transform.localScale = new Vector3(thickness, thickness, dist);
            pointer.GetComponent<MeshRenderer>().material.color = color;
        }
        pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
    }
}

public struct PointerEventArgs
{
    public string fromInput;
    public uint flags;
    public float distance;
    public Transform target;
    public float dx;
    public float dy;
}

public delegate void PointerEventHandler(object sender, PointerEventArgs e);