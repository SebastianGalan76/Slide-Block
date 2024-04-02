using UnityEngine;

public class AdValueTimer : MonoBehaviour
{
    private void Start() {
        InvokeRepeating("IncreaseAdValue", 0, 60);
    }

    public void IncreaseAdValue() {
        AdSystem.ChangeAdValue(1);
    }
}
