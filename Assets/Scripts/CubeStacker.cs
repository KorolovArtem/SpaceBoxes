using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class CubeStacker : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject marker;
    [SerializeField] private float cubeSize = 4f;
    [SerializeField] private float markerSpeed = 1f;
    [SerializeField] private float directionChangeInterval = 1f;
    [SerializeField] private float centerVisitProbability = 0.2f;
    [SerializeField] private Material[] cubeMaterials;
    [SerializeField] private GameObject placeEffectPrefab;
    [SerializeField] private float placeDelay = 0.5f;
    [SerializeField] private float minX = -2f;
    [SerializeField] private float maxX = 2f;
    [SerializeField] private float minZ = -2f;
    [SerializeField] private float maxZ = 2f;

    private GameManager gameManager;
    private GameObject currentCube;
    private Vector3 targetPosition;
    private float timeSinceLastDirectionChange;
    private bool canPlaceCube = true;
    private bool isMobileDevice;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        isMobileDevice = Application.isMobilePlatform;

        SpawnFirstCube();
        SetNewTargetPosition();
    }

    void Update()
    {
        if (currentCube == null || EventSystem.current.IsPointerOverGameObject())
            return;

        HandleMarkerMovement();
        HandleInput();
    }

    private void HandleMarkerMovement()
    {
        MoveMarker();
        timeSinceLastDirectionChange += Time.deltaTime;

        if (timeSinceLastDirectionChange >= directionChangeInterval)
        {
            SetNewTargetPosition();
            timeSinceLastDirectionChange = 0f;
        }
    }

    private void HandleInput()
    {
        if (CanPlaceCube() && canPlaceCube)
        {
            StartCoroutine(PlaceCubeWithDelay());
        }
    }

    private bool CanPlaceCube()
    {
        if (isMobileDevice)
        {
            return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
        }
        else
        {
            return Input.GetMouseButtonDown(0);
        }
    }

    private void SpawnFirstCube()
    {
        currentCube = Instantiate(cubePrefab, new Vector3(0, 2, 0), Quaternion.identity);
        AssignRandomMaterial(currentCube);
        gameManager?.IncrementBlockCount();
    }

    private void MoveMarker()
    {
        Vector3 directionToTarget = (targetPosition - marker.transform.position).normalized;
        Vector3 newPosition = marker.transform.position + directionToTarget * markerSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, currentCube.transform.position.x + minX, currentCube.transform.position.x + maxX);
        newPosition.z = Mathf.Clamp(newPosition.z, currentCube.transform.position.z + minZ, currentCube.transform.position.z + maxZ);
        newPosition.y = currentCube.transform.position.y + cubeSize / 2;

        marker.transform.position = newPosition;
    }

    private void SetNewTargetPosition()
    {
        if (Random.value < centerVisitProbability)
        {
            targetPosition = currentCube.transform.position + new Vector3(0, cubeSize / 2, 0);
        }
        else
        {
            float randomX = Random.value < 0.5f ? minX : maxX;
            float randomZ = Random.value < 0.5f ? minZ : maxZ;
            targetPosition = currentCube.transform.TransformPoint(new Vector3(randomX, cubeSize / 2, randomZ));
        }
    }

    private IEnumerator PlaceCubeWithDelay()
    {
        canPlaceCube = false;
        PlaceCube();
        yield return new WaitForSeconds(placeDelay);
        canPlaceCube = true;
    }

    private void PlaceCube()
    {
        if (gameManager != null && gameManager.isGameOver)
            return;

        Vector3 spawnPosition = new Vector3(marker.transform.position.x, currentCube.transform.position.y + cubeSize, marker.transform.position.z);
        currentCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

        AssignRandomMaterial(currentCube);
        TriggerPlaceEffect(spawnPosition);

        gameManager?.IncrementBlockCount();
        AudioManager.instance.PlayPlaceBlockSound();
        SetNewTargetPosition();
        Camera.main.GetComponent<CameraMover>().SetNewTarget(currentCube.transform);
    }

    private void AssignRandomMaterial(GameObject cube)
    {
        if (cubeMaterials.Length > 0)
        {
            Material randomMaterial = cubeMaterials[Random.Range(0, cubeMaterials.Length)];
            cube.GetComponent<Renderer>().material = randomMaterial;
        }
    }

    private void TriggerPlaceEffect(Vector3 position)
    {
        if (placeEffectPrefab != null)
        {
            Instantiate(placeEffectPrefab, position - new Vector3(0, cubeSize / 2, 0), Quaternion.identity);
        }
    }

    public void DisableMarker()
    {
        if (marker != null)
        {
            marker.SetActive(false);
        }
    }
}