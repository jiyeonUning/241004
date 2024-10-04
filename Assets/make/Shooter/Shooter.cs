using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Shooter : MonoBehaviour
{
    // 코드 참고
    // https://christopherhilton88.medium.com/creating-a-power-bar-with-unitys-new-input-system-668a87a1baa7
    // https://christopherhilton88.medium.com/exploring-tap-hold-interactions-hierarchical-precedence-and-errors-unitys-new-input-system-bf9bc2aa0882


    [SerializeField] Slider gauge;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] PlayerAction playerAction;

    [SerializeField] float ShootSpeed;
    [SerializeField] float MaxShootSpeed = 7f;

    private Touch touch;
    private bool PowerBarMoving = false;



    private void Awake()
    {
        playerAction = new PlayerAction();
    }

    private void OnEnable()
    {
        playerAction.Mouse.mouseClick.Enable();
        playerAction.Touch.touchscreen.Enable();
    }

    private void OnDisable()
    {
        playerAction.Mouse.mouseClick.Disable();
        playerAction.Touch.touchscreen.Disable();
    }

    private void Start()
    {
        playerAction.Mouse.mouseClick.started += PowerBar;
        playerAction.Mouse.mouseClick.canceled += Shot;

        playerAction.Touch.touchscreen.started += PowerBar;
        playerAction.Touch.touchscreen.canceled += Shot;
    }



    void PowerBar(InputAction.CallbackContext context)
    {
        if (PowerBarMoving == false)
        {
            PowerBarMoving = true;
            StartCoroutine(PowerBarRoutine());
        }
    }

    void Shot(InputAction.CallbackContext context)
    {
        ShootSpeed = gauge.value;

        GameObject ball = Instantiate(ballPrefab, Camera.main.transform.position, Camera.main.transform.rotation);
        Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
        //rigidbody.velocity = ShootSpeed * Camera.main.transform.forward;
        rigidbody.AddForce(Camera.main.transform.forward * ShootSpeed, ForceMode.Impulse);

        PowerBarMoving = false;
    }


    IEnumerator PowerBarRoutine()
    {
        while (PowerBarMoving)
        {
            gauge.value += Time.deltaTime * MaxShootSpeed;
            yield return null;
        }

        while (PowerBarMoving == false)
        {
            ShootSpeed = 0f;
            gauge.value = 0f;
            yield return null;
        }
    }
}
