using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PriorityQueue<T>
{
    private T[] heap;

    private uint elementCount;

    private Func<T, T, int> comparator;

    public PriorityQueue(uint size, Func<T, T, int> _comparator)
    {
        heap = new T[size + 1];
        comparator = _comparator;
        elementCount = 0;
    }

    public  PriorityQueue(IList<T> list, uint size, Func<T,T,int> _comparator)
    {
        if (size < list.Count)
            throw new Exception("Can't add all elements of the list to the Priority Queue. Size: " + size + " Number of elements to add: " + list.Count);

        comparator = _comparator;

        elementCount = size;
        heap = new T[elementCount + 1];

        for(int i = 0; i < list.Count; i++)
        {
            heap[i + 1] = list[i];
        }

        for(uint i = (uint)list.Count / 2 + 1; i > 0; i --)
        {
            PercolateDown(i);
        }
    }

    public PriorityQueue(IList<T> list, Func<T, T, int> _comparator) : this(list, (uint) list.Count, _comparator)
    {
    }

    private uint GetParent(uint child)
    {
        return child / 2;
    }

    private uint GetLeftChild(uint parent)
    {
        return parent * 2;
    }

    private uint GetRightChild(uint parent)
    {
        return parent * 2 + 1;
    }

    private void PercolateDown(uint index)
    {
        T parent = heap[index];

        //Gets the children of the parent while ensuring they exist.
        T leftChild = default(T);
        if (GetLeftChild(index) <= elementCount)
            leftChild = heap[GetLeftChild(index)];
        T rightChild = default(T);
        if (GetRightChild(index) <= elementCount)
            rightChild = heap[GetRightChild(index)];

        //Runs while there is still a child less than the node to percolate down.
        while (((! leftChild.Equals(default(T))) && comparator(parent, leftChild) < 0) ||
                ((! rightChild.Equals(default(T))) && comparator(parent, rightChild) < 0))
        {
            T biggerChild;
            uint biggerIndex;
            //Finds which existing child is the greatest.
            if (rightChild.Equals(default(T)) || (!leftChild.Equals(default(T)) && comparator(leftChild, rightChild) > 0))
            {
                biggerChild = leftChild;
                biggerIndex = GetLeftChild(index);
            }
            else
            {
                biggerChild = rightChild;
                biggerIndex = GetRightChild(index);
            }

            //Moves the greatest child up.
            heap[index] = biggerChild;
            index = biggerIndex;

            //Look for new children.
            leftChild = default(T);
            if (GetLeftChild(index) <= elementCount)
                leftChild = heap[GetLeftChild(index)];
            rightChild = default(T);
            if (GetRightChild(index) <= elementCount)
                rightChild = heap[GetRightChild(index)];
        }

        //Put the node to percolate down in its lowest possible spot.
        heap[index] = parent;
    }

    private void PercolateUp(uint index)
    {
        T current = heap[index];
        T parent = heap[GetParent(index)];

        while(! parent.Equals(default(T)) && comparator(current, parent) > 0)
        {
            heap[index] = parent;
            index = GetParent(index);
            parent = heap[GetParent(index)];
        }

        heap[index] = current;
    }

    public void Add(T item)
    {
        Debug.Log("Heap length: " + heap.Length + " number of elements " + elementCount);

        if (heap.Length <= elementCount + 1)
        {
            throw new Exception("PriorityQueue doesn't have enough room for another item. Current size is " + elementCount + " and " + heap.Length + " slots are available.");
        }

        heap[elementCount++] = item;

        PercolateUp(elementCount);
    }

    public T Dequeue()
    {
        if (elementCount == 0)
        {
            throw new Exception("No elements in Priority Queue.");
        }

        T item = heap[1];

        //Sucessor
        heap[1] = heap[elementCount];
        heap[elementCount--] = default(T);
        PercolateDown(1);

        return item;
    }

    public uint Count()
    {
        return elementCount;
    }

    public bool HasRoom()
    {
        return elementCount + 1 < heap.Length;
    }

    public bool IsEmpty()
    {
        return elementCount == 0;
    }

    public override string ToString()
    {
        return heap.ToString();
    }

    public IList<T> ToList()
    {
        List<T> output = new List<T>();

        for(int i = 1; i < heap.Length && heap[i] != null && !heap[i].Equals(default(T)); i++)
        {
            output.Add(heap[i]);
        }

        return output;
    }
}
