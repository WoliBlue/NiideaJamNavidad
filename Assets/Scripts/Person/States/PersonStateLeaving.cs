using UnityEngine;

public class PersonStateLeaving : IPersonState {
  #region Public Methods
  public void Update(Person person) {
    Debug.Log(person.Name + " se está yendo...\n");

    if (!person.CheckReachedTarget()) {
      person.Movement();
      return;
    }

    person.FinishLeaving();
  }
  #endregion
}
