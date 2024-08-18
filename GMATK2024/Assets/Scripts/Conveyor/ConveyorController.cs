using System.Collections;
using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    [SerializeField] private GameObject[] _objectsToSpawn;

    [SerializeField] private Transform _destinationPoint;
    [SerializeField] private float _spawnDelay = 2f;
    [SerializeField] private float _moveSpeed = 1.5f;

    private WaitForSeconds _delay;

    private void Start()
    {
        _delay = new WaitForSeconds(_spawnDelay);
        Invoke(nameof(StartConveyor), 0f);
    }

    private void StartConveyor()
    {
        StartCoroutine(nameof(ConveyorRoutine));
    }

    private IEnumerator ConveyorRoutine()
    {
        while (true)
        {
            var randomObjectIndex = Random.Range(0, _objectsToSpawn.Length);
            var spawnedObject = Instantiate(_objectsToSpawn[randomObjectIndex], transform.position, Quaternion.identity);

            StartCoroutine(MoveToDestination(spawnedObject));

            yield return _delay;
        }
    }

    private IEnumerator MoveToDestination(GameObject obj)
    {
        while (obj != null && Vector3.Distance(obj.transform.position, _destinationPoint.position) > 0.01f)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, _destinationPoint.position, _moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(obj);
    }
}
