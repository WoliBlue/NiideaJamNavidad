using System;
using System.Collections.Generic;
using System.Linq;

using Unity.Mathematics;

using UnityEngine;

public class PersonManager : MonoBehaviour
{
    private const int DAYS = 4;

    #region Variables
    [Header("Person Prefabs Lists")]
    [SerializeField]
    private GameObjectList[] _levelPersonPrefabsPerDay
                                               = new GameObjectList[DAYS];

    [Header("Reference Points")]
    [SerializeField] private Transform _shopPoint;
    [SerializeField] private Transform _leavingPoint;

    [Header("Spawn Settings")]
    [SerializeField] private Transform _spawnStart;
    [SerializeField] private Vector3 _offsetPerPerson = new Vector3(0f, 0f, 1f);

    [Header("Waiting Queue")]
    private Queue<Person> _waitingQueue = new();

    [Header("Shop")]
    private Person _currentBuyer;
    #endregion

    #region Events
    void Start()
    {
        LoadWaitingQueue(1);
    }
    void Update()
    {
        if (!CheckAnyPerson()) return;

        if (!_currentBuyer)
        {
            AdvanceQueue();
        }
    }
    #endregion

    #region Public Methods
    public void AddPersonToWaitingQueue(Person person)
    {
        if (_waitingQueue == null) return;

        if (_waitingQueue.Contains(person))
        {
            Debug.LogWarning(person + " ya está en la cola");
            Destroy(person.gameObject);
            return;
        }

        _waitingQueue.Enqueue(person);
        person.ChangeState(new PersonStateWaiting());
        person.OnFinishedBuying = HandlePersonFinishedBuying;
        person.OnFinishedLeaving = HandlePersonFinishedLeaving;
    }
    public void AdvanceQueue()
    {
        if (_currentBuyer || _waitingQueue.Count <= 0) return;

        _currentBuyer = _waitingQueue.Dequeue();

        // Buyer go to the shop
        _currentBuyer.SetTargetPosition(_shopPoint.position);
        _currentBuyer.ChangeState(new PersonStateBuying());

        // People of the waiting queue, go to the next position
        var arrayTemporal = _waitingQueue.ToArray();
        for (int i = 0; i < arrayTemporal.Length; i++)
        {
            Person person = arrayTemporal[i];
            person.SetTargetPosition(
              _spawnStart.position + _offsetPerPerson * i
            );
        }
    }
    public void HandlePersonFinishedBuying(Person person)
    {
        if (person != _currentBuyer)
        {
            Debug.LogWarning("Persona que no es buyer ha terminado de comprar? xd\n");
            return;
        }

        person.SetTargetPosition(_leavingPoint.position);
        person.ChangeState(new PersonStateLeaving());

        // Al irse el cliente, reseteamos el estado de compra del GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.willBuy = false;
        }
    }
    public void HandlePersonFinishedLeaving(Person person)
    {
        if (person != _currentBuyer) return;

        _currentBuyer = null;
        person.DestroySelf();
        AdvanceQueue();
    }
    public bool CheckPeopleWaiting()
    {
        if (_waitingQueue == null) return false;

        return _waitingQueue.Count > 0;
    }
    public bool CheckAnyPerson()
    {
        return CheckPeopleWaiting() || _currentBuyer != null;
    }
    #region Getters
    public Queue<Person> WaitingQueue => _waitingQueue;
    public Person CurrentBuyer => _currentBuyer;
    public GameObjectList[] LevelPersonPrefabsPerDay => _levelPersonPrefabsPerDay;
    public Vector3 OffsetPerPerson => _offsetPerPerson;
    public Transform SpawnStart => _spawnStart;
    public Transform ShopPoint => _shopPoint;
    public Transform LeavingPoint => _leavingPoint;
    #endregion
    #endregion

    #region Private Methods
    private void LoadWaitingQueue(int day)
    {
        if (day <= 0 || day > DAYS)
        {
            Debug.LogError(
              "Person manager intenta cargar personas de un día inexistente!\n"
            );
        }

        var listItems = _levelPersonPrefabsPerDay[day - 1].items;
        if (listItems.Count <= 0) return;

        for (int i = 0; i < listItems.Count; i++)
        {
            GameObject personPrefab = listItems.ElementAt(i);
            // Intanciate the object in the correct spawn position
            Vector3 spawnPos =
            _spawnStart.position + _offsetPerPerson * i;
            GameObject personGO =
              Instantiate(personPrefab, spawnPos, Quaternion.identity);

            // Get Person Monovehabiour
            Person person = personGO.GetComponent<Person>();
            if (!person)
            {
                Debug.LogError("Person Prefab no tiene Person Monobehaviour");
                Destroy(personPrefab);
            }

            // Insert target position
            person.SetTargetPosition(spawnPos);

            AddPersonToWaitingQueue(person);
        }
    }
    private void OnValidate()
    {
        if (_levelPersonPrefabsPerDay == null ||
            _levelPersonPrefabsPerDay.Length != DAYS)
        {
            _levelPersonPrefabsPerDay = new GameObjectList[DAYS];
        }

        for (int i = 0; i < _levelPersonPrefabsPerDay.Length; i++)
        {
            if (_levelPersonPrefabsPerDay[i] == null)
                _levelPersonPrefabsPerDay[i] = new GameObjectList();
        }
    }
    #endregion
}
