using UnityEngine;

public class DeactivateObject : MonoBehaviour
{
    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
