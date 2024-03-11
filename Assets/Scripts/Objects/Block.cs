using System.Collections;
using System.Collections.Generic;
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
}
