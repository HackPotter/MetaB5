using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class CircularBuffer
{
    private float[] _array;
    private int _index = 0;

    public CircularBuffer(int capacity)
    {
        _array = new float[capacity];
    }

    public void Push(float value)
    {
        _array[_index] = value;
        _index++;
        _index %= _array.Length;
    }

    public float GetAverage()
    {
        float val = 0;
        foreach (float v in _array)
        {
            val += v;
        }
        val /= _array.Length;
        return val;
    }
}

