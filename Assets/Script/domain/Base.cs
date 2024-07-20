using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class BaseObj : MonoBehaviour
{
    public string oid { get; set; }
    public string type { get; set; }
    public int lastUpdate { get; set; }
    public bool valid;
    public virtual void construct(string info) { }
    public virtual void unconstruct() { }
    public virtual void validate() { }
    public virtual void invalid() { }
    public virtual void fired() { }
    public virtual void freeze() { }
    public virtual void cooling() { }
    public virtual void wind(float dirx, float diry) { }
    public virtual void attacked(Weapon weapon, float damage) { }
    public virtual void magnetism() { }
    public virtual void pickUp() { }
    public virtual void addChild(string id, GameObject child) { }
    public virtual void removeChild(string id) { }
}
