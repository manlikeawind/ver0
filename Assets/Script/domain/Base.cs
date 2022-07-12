using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

abstract public class BaseObj : MonoBehaviour
{
    public virtual void construct(SqliteDataReader reader) { }

    public virtual void fired() { }
    public virtual void heat() { }

    public virtual void freeze() { }
    public virtual void cooling() { }
    public virtual void wind(float dirx, float diry) { }

    public virtual void attacked(int weaponType, float damage) { }

    public virtual void magnetism() { }
}
