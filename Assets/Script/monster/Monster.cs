using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Monster : MonoBehaviour
{
    public string oid { get; set; }
    public string type { get; set; }
    public int lastUpdate { get; set; }
    public float hp { get; set; }
    public virtual void construct(string info) { }
    public virtual void attacked(Weapon weapon, float damage) { }
}
