using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void Start()
    {
        if (target == null && PlayerController.instance != null)
            target = PlayerController.instance.transform;
    }

    void LateUpdate()
    {
        if (target == null && PlayerController.instance != null)
            target = PlayerController.instance.transform;

        if (target == null) return;

        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}