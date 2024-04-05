using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if(IsPointerOverUIObject()) {
            return;
        }

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

    public void Initialize(int posX, int posY, MovementSystem movementSystem) {
        Initialize(FieldType.STOPPABLE, posX, posY, movementSystem);
    }

    public override void Move(DirectionType direction, int value, bool showTrail) {
        base.Move(direction, value, showTrail);

        performedMovement = true;
    }

    public void OnClick() {
        isStopped = !isStopped;
        AudioManager.Instance.PlaySound(SoundType.STOPPABLE_BLOCK);

        if(isStopped) {
            spriteRenderer.sprite = stoppedBlockSprite;
        } else {
            spriteRenderer.sprite = unstoppedBlockSprite;
        }
    }

    private bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
