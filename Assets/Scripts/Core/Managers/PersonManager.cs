using System.Collections.Generic;

using UnityEngine;

public class PersonManager : MonoBehaviour {
  #region Variables
  [SerializeField] private Queue<Person> _waitingQueue;
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
      Debug.LogWarning(person.Name + " ya está en la cola");
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
  #endregion
  #endregion
}
