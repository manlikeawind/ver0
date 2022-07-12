using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;

public class Teleport : BaseObj
{
    public string id;
    public string scene;
    public float posX;
    public float posY;
    public string to;

    public override void construct(SqliteDataReader reader) {
        reader.Read();
        this.id = reader.GetString(reader.GetOrdinal("id"));
        this.scene = reader.GetString(reader.GetOrdinal("scene"));
        this.posX = reader.GetFloat(reader.GetOrdinal("posX"));
        this.posY = reader.GetFloat(reader.GetOrdinal("posY"));
        this.to = reader.GetString(reader.GetOrdinal("to"));
    }
}
