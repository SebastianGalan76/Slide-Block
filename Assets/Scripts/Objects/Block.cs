using UnityEngine;

public class Block : MonoBehaviour
{
    private FieldType type;
    private int posX, posY;

    public void Initialize(FieldType type, int posX, int posY) {
        this.type = type;
        this.posX = posX;
        this.posY = posY;
    }

    public void MoveRight(int value) {
        transform.position += new Vector3(value, 0, 0);
    }

    public void MoveLeft(int value) {
        transform.position -= new Vector3(value, 0, 0);
    }

    public void MoveUp(int value) {
        transform.position += new Vector3(0, value, 0);
    }

    public void MoveDown(int value) {
        transform.position -= new Vector3(0, value, 0);
    }

    public FieldType GetBlockType() {
        return type;
    }
}
