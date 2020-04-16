using UnityEngine;
using System.Collections;

public class PlayerInputs : MonoBehaviour
{
    JoyStickModule joystick;
    private void Awake()
    {
        joystick = GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem._model._JoyStickModule;
    }

    // Update is called once per frame
    void Update()
    {
        joystick.Ljoystick = GameObject.Find("Canvas/MoveStickUI").GetComponent<VirtualJoystick>().GetVirtualJoystickInput();
        joystick.Rjoystick = GameObject.Find("Canvas/AttackStickUI").GetComponent<VirtualJoystick>().GetVirtualJoystickInput();

    }
}
