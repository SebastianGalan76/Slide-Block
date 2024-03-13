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
                posX += value;
                transform.position += new Vector3(value, 0, 0);
                break;
            case DirectionType.LEFT:
                posX -= value;
                transform.position -= new Vector3(value, 0, 0);
                break;
            case DirectionType.UP:
                posY -= value;
                transform.position += new Vector3(0, value, 0);
                break;
            case DirectionType.DOWN:
                posY += value;
                transform.position -= new Vector3(0, value, 0);
            break;          
        }
    }

    public FieldType GetBlockType() {
        return type;
    }
}
