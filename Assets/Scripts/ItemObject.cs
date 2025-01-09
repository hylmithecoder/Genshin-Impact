using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public static ItemObject Instance {get; private set;}
    public List<GameObject> theItem;
    
    void Awake()
    {
        Instance = this;
    }
}