using UnityEngine;

public class Block : MonoBehaviour
{
    private FieldType type;
    protected int posX, posY;

    public void Initialize(FieldType type, int posX, int posY) {
        this.type = type;
        this.posX = posX;
        this.posY = posY;
    }

    public virtual void Move(DirectionType direction, int value) {
        switch (direction) {
            case DirectionType.RIGHT:
                MoveRight(value); break;
            case DirectionType.LEFT:
                MoveLeft(value); break;
            case DirectionType.UP:
                MoveUp(value); break;
            case DirectionType.DOWN:
                MoveDown(value); break;
        }
    }

    private void MoveRight(int value) {
        posX += value;
        transform.position += new Vector3(value, 0, 0);
    }

    private void MoveLeft(int value) {
        posX -= value;
        transform.position -= new Vector3(value, 0, 0);
    }

    private void MoveUp(int value) {
        posY -= value;
        transform.position += new Vector3(0, value, 0);
    }

    private void MoveDown(int value) {
        posY += value;
        transform.position -= new Vector3(0, value, 0);
    }

    public FieldType GetBlockType() {
        return type;
    }
}
