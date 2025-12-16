using UnityEngine;

public class PersonStateDialoging : IPersonState {
  #region Public Methods
  public void Enter(Person person) {
    Debug.Log(person.Name + " dialoga\n");
  }
  public void Exit(Person person) {
    Debug.Log(person.Name + " está dialogando...\n");
  }
  public void Update(Person person) {
    Debug.Log(person.Name + " deja de dialogar\n");
  }
  #endregion
}
