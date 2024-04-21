using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deque<T>
{
    static int MAX = 100;
    T[] arr;
    public int front;
    public int rear;
    public int size;

    public Deque()
    {
        this.arr = new T[MAX];
        front = -1;
        rear = 0;
        this.size = 3;
    }

    public bool IsFull()
    {
        return ((this.front == 0 && rear == size - 1) || this.front == rear + 1);
    }

    public bool IsEmpty()
    {
        return (front == -1);
    }

    public void PushFront( T val )
    {
        if (this.IsFull())
        {
            Console.WriteLine( "Full" );
            return;
        }

        if (this.front == -1)
        {
            this.front = 0;
            this.rear = 0;
        }
        else if (this.front == 0)
        {
            this.front = this.size - 1;
        }
        else
        {
            this.front = this.front - 1;
        }

        this.arr[this.front] = val;
    }

    public void PushBack( T val )
    {
        if (this.IsFull())
        {
            Console.WriteLine( "Full" );
            return;
        }

        if (this.front == -1)
        {
            this.front = 0;
            this.rear = 0;
        }
        else if (this.rear == this.size - 1)
        {
            this.rear = 0;
        }
        else
        {
            this.rear = this.rear + 1;
        }

        this.arr[this.rear] = val;
    }

    public void PopFront()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine( "Empty" );
            return;
        }

        if (this.front == this.rear)
        {
            this.front = -1;
            this.rear = -1;
        }
        else if (this.front == this.size - 1)
        {
            this.front = 0;
        }
        else
        {
            this.front = this.front + 1;
        }
    }

    public void PopBack()
    {
        if (this.IsEmpty())
        {
            Console.WriteLine( "Empty" );
            return;
        }

        if (this.front == this.rear)
        {
            this.front = -1;
            this.rear = -1;
        }
        else if (this.rear == 0)
        {
            this.rear = this.size - 1;
        }
        else
        {
            this.rear = this.rear - 1;
        }
    }

    public T GetFront()
    {
        if (this.IsEmpty())
        {
            throw new Exception( "empty" );
        }
        return this.arr[this.front];
    }

    public T GetBack()
    {
        if (this.IsEmpty())
        {
            throw new Exception( "empty" );
        }
        return this.arr[this.rear];
    }

    public void Clear()
    {
        this.arr = new T[MAX];
        front = -1;
        rear = 0;
        this.size = 100;
    }
}
