using UnityEngine;

public static class ArrayExtensions
{
    public static void Shuffle<T>(this T[] vecs)
    {
        for (int t = 0; t < vecs.Length; t++)
        {
            var tmp = vecs[t];
            int r = Random.Range(t, vecs.Length);
            vecs[t] = vecs[r];
            vecs[r] = tmp;
        }
    }
}