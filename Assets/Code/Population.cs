using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{
    private List<GameObject> _rockets;
    private List<Rocket> _matingPool;

    public GameObject _rocket;
    public Transform Target;
    public Transform StartPoint;
    public int RocketCount = 100;
    public bool Evaluating = false;

    private void Start()
    {
        InitPopulation();
    }

    private void Update()
    {
        if (!Evaluating)
        {
            Manager.Count++;
            if (Manager.Count >= Manager.Lifespan)
            {
                Manager.Count = 0;
                Evaluate();
                Selection();
                Manager.Generation++;
            }
        }
    }

    public void RePopulate()
    {
        InitPopulation();
    }

    private void Evaluate()
    {
        Evaluating = true;
        float maxFitness = 0.0f;

        for (var i = 0; i < _rockets.Count; i++)
        {
            Rocket r = _rockets[i].GetComponent<Rocket>();
            r.CalculateFitness();
            if (r.Fitness > maxFitness)
            {
                maxFitness = r.Fitness;
            }
        }
        if (Manager.MaxFitness < maxFitness)
        {
            Manager.MaxFitness = maxFitness;
        }

        for (var i = 0; i < _rockets.Count; i++)

        {
            Rocket r = _rockets[i].GetComponent<Rocket>();
            if (r)
            {
                r.Fitness /= maxFitness;
            }
        }

        _matingPool = new List<Rocket>();


        bool logged = false;
        float largestFitness = 0.0f;
        for (var i = 0; i < _rockets.Count; i++)
        {
            Rocket r = _rockets[i].GetComponent<Rocket>();
            var n = r.Fitness * 100;
            for (var j = 0; j < n; j++)
            {
                _matingPool.Add(r);
            }
        }
    }

    private void Selection()
    {
        List<GameObject> newRockets = new List<GameObject>();
        for (var i = 0; i < _rockets.Count; i++)
        {
            DNA parentA = _matingPool[Random.Range(0, _matingPool.Count)].DNA;
            DNA parentB = _matingPool[Random.Range(0, _matingPool.Count)].DNA;
            DNA child = parentA.Crossover(parentB);
            child.Mutate();

            _rockets[i].GetComponent<Rocket>().Init(child);
        }

        Evaluating = false;
    }

    private void InitPopulation()
    {
        if (this.transform.childCount > 0)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.parent = null;
                Destroy(child);
            }
        }

        _rockets = new List<GameObject>();
        _matingPool = new List<Rocket>();

        for (var i = 0; i < RocketCount; i++)
        {
            _rockets.Add(Instantiate(_rocket, this.transform));
        }
    }
}