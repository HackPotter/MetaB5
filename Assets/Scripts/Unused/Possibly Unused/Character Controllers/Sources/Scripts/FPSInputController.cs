using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
[AddComponentMenu("Character/FPS Input Controller")]
public class FPSInputController : MonoBehaviour
{
    
#pragma warning disable 0067, 0649
    [SerializeField]
    private float _keyboardTurningRate;
#pragma warning restore 0067, 0649

    private CharacterMotor motor;

    // Use this for initialization
    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.Instance.PauseLevel != PauseLevel.Unpaused)
        {
			motor.inputMoveDirection = Vector3.zero;
            return;
        }

        // Get the input vector from kayboard or analog stick
        float forward = 0;
        float right = 0;
        float turning = 0;

        if (Input.GetKey(KeyCode.W))
        {
            forward += 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            forward -= 1;
        }

        if (Input.GetKey(KeyCode.E))
        {
            right += 1;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            right -= 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            turning -= _keyboardTurningRate * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            turning += _keyboardTurningRate * Time.deltaTime;
        }



        Vector3 directionVector = new Vector3(right, 0, forward);

        if (directionVector != Vector3.zero)
        {
            // Get the length of the directon vector and then normalize it
            // Dividing by the length is cheaper than normalizing when we already have the length anyway
            float directionLength = directionVector.magnitude;
            directionVector = directionVector / directionLength;

            // Make sure the length is no bigger than 1
            directionLength = Mathf.Min(1, directionLength);

            // Make the input vector more sensitive towards the extremes and less sensitive in the middle
            // This makes it easier to control slow speeds when using analog sticks
            directionLength = directionLength * directionLength;

            // Multiply the normalized direction vector by the modified length
            directionVector = directionVector * directionLength;
        }

        // Apply the direction to the CharacterMotor
        motor.inputMoveDirection = transform.rotation * directionVector;
        transform.Rotate(Vector3.up, turning);
        //motor.inputJump = Input.GetButton("Jump");
    }
}