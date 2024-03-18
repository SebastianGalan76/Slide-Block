using UnityEngine;

public class Block : MonoBehaviour
{
    private const float SPEED = 30f;

    private MovementSystem movementSystem;
    private FieldType type;
    protected int posX, posY;

    private Vector3 startPosition, newPosition;
    private float time, duration;

    public void Initialize(FieldType type, int posX, int posY, MovementSystem movementSystem) {
        this.movementSystem = movementSystem;
        this.type = type;
        this.posX = posX;
        this.posY = posY;

        startPosition = new Vector3(posX, -posY, 1);
        newPosition = startPosition;
        enabled = false;
    }

    private void Update() {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        transform.position = Vector3.Lerp(startPosition, newPosition, t);

        if(time >= duration) {
            enabled = false;
            movementSystem.FinishMovement();
        }
    }

    public virtual void Move(DirectionType direction, int value) {
        startPosition = transform.position;
        switch (direction) {
            case DirectionType.RIGHT:
                posX += value;
                newPosition += new Vector3(value, 0, 0);
                break;
            case DirectionType.LEFT:
                posX -= value;
                newPosition -= new Vector3(value, 0, 0);
                break;
            case DirectionType.UP:
                posY -= value;
                newPosition += new Vector3(0, value, 0);
                break;
            case DirectionType.DOWN:
                posY += value;
                newPosition -= new Vector3(0, value, 0);
            break;          
        }

        float distance = Vector3.Distance(startPosition, newPosition);
        duration = distance /SPEED;
        time = 0;
        
        enabled = true;
    }

    public FieldType GetBlockType() {
        return type;
    }
}
