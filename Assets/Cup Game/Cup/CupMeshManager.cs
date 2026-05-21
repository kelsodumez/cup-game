using UnityEngine;
using System;

static public class CupMeshManager
{
    static public event Action<MeshRenderer> OnOutlineAltered;

    public static void OutlineAltered(MeshRenderer _mrAltered)
    {
        OnOutlineAltered?.Invoke(_mrAltered);
    }
}