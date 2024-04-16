using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    private enum Direction
    {
        Forward_BlueI,
        Backward_BlueD,
        Right_RedI,
        Left_RedD,
        Up_GreenI,
        Down_GreenD
    }

    // Editor fields
    [SerializeField]
    private bool isBeltOn = true;
    public bool IsBeltOn
    {
        get { return isBeltOn; }
    }

    [SerializeField]
    private float acceleration = 30f;

    [SerializeField]
    [Range(0, 50)]
    private float maxSpeed = 1f;
    public float MaxSpeed
    {
        get { return maxSpeed; }
    }

    [SerializeField]
    [Tooltip("For selecting movement direction. Using the direction of the belt object.")]
    private Direction direction = Direction.Forward_BlueI;

    // Private variables
    private Vector3 _direction;

    void Start()
    {
        switch (direction) // Transforming enum to vector3
        {
            case Direction.Forward_BlueI:
            {
                _direction = gameObject.transform.forward;
                break;
            }
            case Direction.Right_RedI:
            {
                _direction = gameObject.transform.right;
                break;
            }
            case Direction.Backward_BlueD:
            {
                _direction = -gameObject.transform.forward;
                break;
            }
            case Direction.Left_RedD:
            {
                _direction = -gameObject.transform.right;
                break;
            }
            case Direction.Up_GreenI:
            {
                _direction = gameObject.transform.up;
                break;
            }
            case Direction.Down_GreenD:
            {
                _direction = -gameObject.transform.up;
                break;
            }
        }
    }

    void Update()
    {
        isBeltOn = GameManager.instance.IsTaskOn;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!IsBeltOn)
            return;

        Rigidbody obj = collision.gameObject.GetComponent<Rigidbody>();
        if (obj)
        {
            if (obj.velocity.magnitude > maxSpeed)
                return;

            obj.AddForce(_direction * acceleration, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Method <c>ToggleBelt</c> toggles belt on and off.
    /// </summary>
    public void ToggleBelt()
    {
        isBeltOn = !isBeltOn;
    }
}
