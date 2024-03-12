using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSystem : MonoBehaviour
{
    [SerializeField] private PlatformManager platform;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.D)) {
            platform.MoveRight();
        }
        if(Input.GetKeyDown(KeyCode.A)) {
            platform.MoveLeft();
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            platform.MoveUp();
        }
        if(Input.GetKeyDown(KeyCode.S)) {
            platform.MoveDown();
        }
    }
}
