using UnityEngine;

public class PersonStateComing : IPersonState {
  #region Public Methods
  public void Enter(Person person) {
    Debug.Log(person.Name + " viene\n");
  }
  public void Exit(Person person) {
    Debug.Log(person.Name + " está viniendo...\n");
  }
  public void Update(Person person) {
    Debug.Log(person.Name + " deja de venir\n");
  }
  #endregion
}
