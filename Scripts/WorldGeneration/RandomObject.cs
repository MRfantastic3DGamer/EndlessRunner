using System.Collections.Generic;
using UnityEngine;

public class RandomObject<T>
{
    private List<T> _list = new List<T>();
    private List<T> _usedList = new List<T>();
    private int Length => _usedList.Count;

    private void Add(T o)
    {
        _list.Add(o);
        _usedList.Add(o);
    }

    /// <summary>
    /// it is meant to be used only once at the start
    /// </summary>
    /// <param name="l">List</param>
    public void AddList(List<T> l)
    {
        foreach (T v in l)
        {
            Add(v);
        }
    }

    private void ResetUsedList()
    {
        foreach (T v in _list)
        {
            _usedList.Add(v);
        }
    }

    public T getRandom()
    {
        
        if (Length == 0) ResetUsedList();
        int i = Random.Range(0, Length);
        T o = _usedList[i];
        _usedList.Remove(o);
        return o;
    }
}