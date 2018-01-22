using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA
{
    private List<Vector2> _genes;

    public DNA()
    {
        _genes = new List<Vector2>();
        Init();
    }

    public DNA(List<Vector2> genes)
    {
        _genes = genes;
    }

    private void Init()
    {
        for (var i = 0; i < 200; i++)
        {
            _genes.Add(Util.RandomVector());
        }
    }

    public void Mutate()
    {
        for (var i = 0; i < _genes.Count; i++)
        {
            if (Random.Range(0.0f, 1.0f) < 0.01f)
            {
                _genes[i] = Util.RandomVector();
            }
        }
    }

    public DNA Crossover(DNA d)
    {
        List<Vector2> newDNA = new List<Vector2>();
        int mid = Mathf.FloorToInt(Random.Range(0, _genes.Count));
        for (var i = 0; i < _genes.Count; i++)
        {
            if (i > mid)
            {
                newDNA.Add(_genes[i]);
            }
            else
            {
                newDNA.Add(d.Genes[i]);
            }
        }

        return new DNA(newDNA);
    }

    public List<Vector2> Genes { get { return _genes; } }
}