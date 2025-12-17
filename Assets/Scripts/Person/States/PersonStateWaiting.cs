using UnityEngine;

public class PersonStateWaiting : IPersonState {
  #region Public Methods
  public void Update(Person person) {
    Debug.Log(person.Name + " está ESPERANDO...\n");
    person.Movement();
  }
  #endregion
}
