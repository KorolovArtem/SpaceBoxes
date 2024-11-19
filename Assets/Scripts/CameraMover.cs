using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 25f, -30f);

    private Transform currentTarget;
    private Transform newTarget;
    private Vector3 desiredPosition;

    void Start()
    {
        currentTarget = null;
        desiredPosition = transform.position;
    }

    void Update()
    {
        if (newTarget != null && currentTarget != newTarget)
        {
            currentTarget = newTarget;

            desiredPosition = currentTarget.position + offset;
        }

        if (currentTarget != null)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, step);

            desiredPosition = currentTarget.position + offset;

            transform.LookAt(currentTarget);
        }
    }

    public void SetNewTarget(Transform newTargetTransform)
    {
        newTarget = newTargetTransform;
    }
}