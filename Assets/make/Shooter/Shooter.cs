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
    [SerializeField] PlayerInput input;

    [SerializeField] float ShootSpeed;
    [SerializeField] float MaxShootSpeed = 15f;

    private bool PowerBarMoving = false;


    private void Update()
    {
        if (input.actions["touchscreen"].WasPressedThisFrame()) { PowerBar(); }
        if (input.actions["touchscreen"].WasReleasedThisFrame()) { Shot(); }
    }


    void PowerBar()
    {
        if (PowerBarMoving == false)
        {
            PowerBarMoving = true;
            StartCoroutine(PowerBarRoutine());
        }
    }

    void Shot()
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
            gauge.value = 0f;
            yield return null;
        }
    }
}
