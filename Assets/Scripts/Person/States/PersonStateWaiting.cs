using UnityEngine;

public class PersonStateWaiting : IPersonState {
  #region Public Methods
  public void Enter(Person person) {
    Debug.Log(person.Name + " espera\n");
  }
  public void Exit(Person person) {
    Debug.Log(person.Name + " está esperando...\n");
  }
  public void Update(Person person) {
    Debug.Log(person.Name + " deja de esperar\n");
  }
  #endregion
}
