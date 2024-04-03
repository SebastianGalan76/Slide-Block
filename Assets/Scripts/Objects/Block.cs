using UnityEngine;

public class Block : MonoBehaviour
{
    private const float SPEED = 20f;

    [SerializeField] private GameObject[] trailsObjects;

    private MovementSystem movementSystem;
    private FieldType type;
    protected int posX, posY;

    private Vector3 startPosition, newPosition;
    private float time, duration;

    private Animator animator;

    public void Initialize(PlatformManager.ColorBlock colorBlock, int posX, int posY, MovementSystem movementSystem) {
        Initialize(colorBlock.color, posX, posY, movementSystem);

        foreach(GameObject trailObj in trailsObjects) {
            TrailRenderer trail = trailObj.GetComponent<TrailRenderer>();
            trail.colorGradient = colorBlock.trailColor;
        }
    }

    protected void Initialize(FieldType type, int posX, int posY, MovementSystem movementSystem) {
        this.movementSystem = movementSystem;
        this.type = type;
        this.posX = posX;
        this.posY = posY;

        animator = GetComponent<Animator>();

        startPosition = new Vector3(posX, -posY, 1);
        newPosition = startPosition;
        enabled = false;
    }

    private void Update() {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);
        transform.localPosition = Vector3.Lerp(startPosition, newPosition, t);

        if(time >= duration) {
            enabled = false;
            movementSystem.FinishMovement();
        }
    }

    public virtual void Move(DirectionType direction, int value, bool showTrail) {
        startPosition = transform.localPosition;
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
        duration = distance / SPEED;
        time = 0;

        ShowTrail(direction, showTrail);

        enabled = true;
    }

    public void DestroyBlock() {
        HideAllTrails();

        if(animator != null) {
            animator.Play("DestroyBlock");
        }
    }

    public void DestroyGameObject() {
        Destroy(gameObject);
    }

    public FieldType GetBlockType() {
        return type;
    }

    public void HideAllTrails() {
        foreach(GameObject trail in trailsObjects) {
            trail.SetActive(false);
        }
    }

    private void ShowTrail(DirectionType movementDirection, bool show) {
        if(trailsObjects.Length != 4) {
            return;
        }

        HideAllTrails();

        if(!show) {
            return;
        }

        switch (movementDirection) {
            case DirectionType.UP:
                trailsObjects[0].SetActive(true);
                break;
            case DirectionType.DOWN:
                trailsObjects[1].SetActive(true);
                break;
            case DirectionType.LEFT:
                trailsObjects[2].SetActive(true);
                break;
            case DirectionType.RIGHT:
                trailsObjects[3].SetActive(true);
                break;

        }
    }
}
