using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public int Lifespan = 200;
    public float ForceScale = 0.01f;
    public Transform Target;
    public Transform StartPoint;

    private Rigidbody2D _body;

    private Vector2 _velocity;
    private Vector2 _acceleration;

    private Population _population;
    private DNA _dna;
    private int _count;
    private int _timeTaken;
    private float _fitness;
    private bool _completed = false;
    private bool _dead = false;

    public float Fitness
    {
        get { return _fitness; }
        set { _fitness = value; }
    }

    public DNA DNA { get { return _dna; } }

    private void Start()
    {
        _population = transform.parent.GetComponent<Population>();
        _body = GetComponent<Rigidbody2D>();
        Target = _population.Target;
        StartPoint = _population.StartPoint;

        Reset();
    }

    public void Init(DNA dna)
    {
        Reset();
        _dna = dna;
    }

    private void ApplyForce(Vector2 force)
    {
        _acceleration += force * ForceScale;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            _dead = true;
        }
    }

    private void Update()
    {
        if (Vector2.Distance(_body.position, Target.position) < 1.0f)
        {
            _completed = true;
            _body.position = Target.position;
        }

        if (_completed == false && !_dead)
        {
            _timeTaken = Manager.Count;
            if (_count < _dna.Genes.Count)
            {
                ApplyForce(_dna.Genes[_count]);
                _count++;
            }

            _body.velocity = _body.velocity += _acceleration;
            _body.position = _body.position += _body.velocity;

            _acceleration = Vector2.zero;

            Vector2 vel = _body.velocity;
            float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Deg2Rad;
            _body.rotation = angle * 360;
        }
    }

    public void CalculateFitness()
    {
        float dist = Vector2.Distance(_body.position, Target.position);

        _fitness -= _timeTaken * 2.0f;
        _fitness = dist.Map(0.0f, Screen.width, Screen.width, 0.0f);
        if (_completed)
        {
            _fitness *= 10.0f;
        }
        else if (_dead)
        {
            _fitness /= 10.0f;
        }
        
    }

    private void Reset()
    {
        _dna = new DNA();
        _count = 0;
        _body.velocity = Vector2.zero;
        _body.position = StartPoint.position;
        _acceleration = Vector2.zero;
        _completed = false;
        _dead = false;
    }
}

public static class Util
{
    public static Vector2 RandomVector()
    {
        return new Vector2(
            Random.Range(-0.1f, 0.1f),
            Random.Range(-0.1f, 0.1f)
            );
    }
}

public static class ExtensionMethods
{
    public static float Map(this float value, float fromSource, float toSource, float targetForm, float targetTo)
    {
        return (value - fromSource) / (toSource - fromSource) * (targetTo - targetForm) + targetForm;
    }
}
