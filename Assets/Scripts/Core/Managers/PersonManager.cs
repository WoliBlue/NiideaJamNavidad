using System.Collections.Generic;

using Unity.Mathematics;

using UnityEngine;

public class PersonManager : MonoBehaviour {
  #region Variables
  [Header("Person Prefabs per Level")]
  [SerializeField] private List<GameObject> _levelPersonPrefabs;

  [Header("Spawn Settings")]
  [SerializeField] private Vector3 _spawnStartPosition = Vector3.zero;
  [SerializeField] private Vector3 _offsetPerPerson = new Vector3(0f, 10f, 0f);

  [Header("Waiting Queue")]
  private Queue<Person> _waitingQueue;

  [Header("Shop")]
  private Person _currentBuyer;
  #endregion

  #region Events
  void Start() {
  }
  void Update() {

  }
  #endregion

  #region Public Methods
  public void AddPersonToWaitingQueue(Person person) {
    if (_waitingQueue.Contains(person)) {
      Debug.LogWarning(person + " ya está en la cola");
      return;
    }

    _waitingQueue.Enqueue(person);
  }
  public void AdvanceQueue() {
    if (_currentBuyer || _waitingQueue.Count <= 0) return;

    _currentBuyer = _waitingQueue.Dequeue();
    _currentBuyer.ChangeState(new PersonStateWaiting());
  }
  public void HandlePersonFinishedBuying(Person person) {
    if (person != _currentBuyer) {
      Debug.LogWarning("Persona que no es buyer ha terminado de comprar? xd\n");
      return;
    }

    _currentBuyer = null; // Free buyer
    person.DestroySelf();
    AdvanceQueue();
  }
  #region Getters
  public Queue<Person> WaitingQueue => _waitingQueue;
  public Person CurrentBuyer => _currentBuyer;
  public List<GameObject> LevelPersonPrefabs => _levelPersonPrefabs;
  public Vector3 OffsetPerPerson => _offsetPerPerson;
  public Vector3 SpawnStartPosition => _spawnStartPosition;
  #endregion
  #endregion

  #region Private Methods
  private void LoadWaitingQueue() {
    if (_levelPersonPrefabs.Count <= 0) return;

    foreach (GameObject personPrefab in _levelPersonPrefabs) {
      // Intanciate the object in the correct spawn position
      Vector3 spawnPos =
        _spawnStartPosition + _offsetPerPerson * _waitingQueue.Count;
      GameObject personGO =
        Instantiate(personPrefab, spawnPos, Quaternion.identity);

      // Get Person Monovehabiour and insert to the WaitingQueue
      Person person = personPrefab.GetComponent<Person>();
      if (!person) {
        Debug.LogError("Person Prefab no tiene Person Monobehaviour");
        Destroy(personPrefab);
      }
      AddPersonToWaitingQueue(personPrefab.GetComponent<Person>());
    }
  }
  #endregion
}
