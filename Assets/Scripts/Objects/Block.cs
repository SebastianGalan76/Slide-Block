using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private BlockType type;

    private int posX, posY;

    public void Initialize(BlockType type, int posX, int posY) {
        this.type = type;
        this.posX = posX;
        this.posY = posY;
    }
}
