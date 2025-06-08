    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public string Name { get; private set; }
    public int Level { get; private set; }
    public int Experience { get; private set; }
    public int Attack {get; private set;}
    public int Defense {get; private set;}
    public int Hp {get; private set;}
    public int Critical {get; private set;}
    public int Gold {get; private set;}
}
