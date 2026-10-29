using System;
using System.Collections.Generic;

class Program
{
    public static void SolveJosephus(int n, int k)
    {
        Queue<int> que = new Queue<int>();

        for (int i = 1; i <= n; i++)
        {
            que.Enqueue(i);
        }

        List<int> Loai = new List<int>();

        while (que.Count > 1)
        {
        
            for (int i = 1; i < k; i++)
            {
                int person = que.Dequeue();
                que.Enqueue(person);
            }

      
            Loai.Add(que.Dequeue());
        }


        int survivor = que.Dequeue();


        Console.WriteLine("Thu tu bi loai: " + string.Join(" ", Loai));
        Console.WriteLine("Nguoi con lai: " + survivor);
    }

   static void Main()
    {
        int[] a = { 73, 74, 75, 71, 69, 72, 76, 73 };
        int n = a.Length;
        int[] result = new int[n];
        Stack<int> st = new Stack<int>();
        for(int i = 0; i < n; i++)
        {
            while (st.Count > 0 && a[i] >= a[st.Peek()])
            {
                int Ngay = st.Pop();
                result[Ngay] = i - Ngay;
            }
            st.Push(i);
        }
        Console.WriteLine(string.Join(" ", result));

        SolveJosephus(7, 4);
    }
}
