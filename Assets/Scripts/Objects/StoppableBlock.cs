using UnityEngine;

public class StoppableBlock : Block
{
    [SerializeField] private Sprite stoppedBlockSprite;
    [SerializeField] private Sprite unstoppedBlockSprite;
    
    private SpriteRenderer spriteRenderer;

    public bool isStopped;

    private float startPositionX, deltaX, deltaAbsX;
    private float startPositionY, deltaY, deltaAbsY;
    private float moveSensitivity = 0.2f;
    private bool performedMovement;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown() {
        startPositionX = Input.mousePosition.x;
        startPositionY = Input.mousePosition.y;
        performedMovement = false;
    }

    private void OnMouseUp() {
        if(performedMovement) {
            return;
        }

        deltaX = startPositionX - Input.mousePosition.x;
        deltaY = startPositionY - Input.mousePosition.y;

        deltaX /= Screen.dpi;
        deltaY /= Screen.dpi;

        deltaAbsX = Mathf.Abs(deltaX);
        deltaAbsY = Mathf.Abs(deltaY);

        if(deltaAbsX < moveSensitivity && deltaAbsY < moveSensitivity) {
            OnClick();
        }
    }

    public override void Move(DirectionType direction, int value, bool showTrail) {
        base.Move(direction, value, showTrail);

        performedMovement = true;
    }

    public void OnClick() {
        isStopped = !isStopped;

        if(isStopped) {
            spriteRenderer.sprite = stoppedBlockSprite;
        } else {
            spriteRenderer.sprite = unstoppedBlockSprite;
        }
    }
}
