using System;
using System.Data;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using UnityEngine;

public class DBHelper
{

    private const string DB_NAME = "test_db.db";
    private const string TABLE_NAME = "test_table";
    private string connectionPath = "URI=file:" + Application.persistentDataPath + "/" + DB_NAME;

    IDbConnection dbConnection;
    IDbCommand dbCommand;
    IDataReader dbReader;

    public DBHelper()
    {
        
        dbConnection = new SqliteConnection(connectionPath);
        dbConnection.Open();

        string queryCreateTable = "CREATE TABLE IF NOT EXISTS " + TABLE_NAME + " (id INTEGER NOT NULL UNIQUE, val TEXT(255), PRIMARY KEY(id AUTOINCREMENT))";
        
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryCreateTable;
        dbCommand.ExecuteNonQuery();
    }

    public List<ItemModel> addItem(ItemModel item)
    {
        string queryInsertValue = "INSERT INTO " + TABLE_NAME + " (val) VALUES ('" + item.val + "')";
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryInsertValue;
        dbCommand.ExecuteNonQuery();

        return getAllItems();
    }

    public List<ItemModel> getAllItems()
    {
        List<ItemModel> outList = new List<ItemModel>();

        string querySelectFrom = "SELECT * FROM " + TABLE_NAME;
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = querySelectFrom;
        dbReader = dbCommand.ExecuteReader();
        while (dbReader.Read())
        {
            outList.Add(
                new ItemModel(
                    id: Int32.Parse(dbReader[0].ToString()),
                    val: dbReader[1].ToString()
                )
            );
        }

        return outList;
    }

    public ItemModel? getItemById(int id)
    {
        string querySelectFrom = "SELECT * FROM " + TABLE_NAME + " WHERE id = " + id;
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = querySelectFrom;
        dbReader = dbCommand.ExecuteReader();

        if (dbReader.FieldCount == 0) return null;

        return new ItemModel(
            id: Int32.Parse(dbReader[0].ToString()),
            val: dbReader[1].ToString()
        );
    }

    public List<ItemModel> updateItem(ItemModel item)
    {
        string queryInsertValue = "UPDATE " + TABLE_NAME + " SET val = '" + item.val + "' WHERE id = " + item.id;
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryInsertValue;
        dbCommand.ExecuteNonQuery();

        return getAllItems();
    }

    public List<ItemModel> deleteItem(int id)
    {
        string queryInsertValue = "DELETE FROM " + TABLE_NAME + " WHERE id = " + id;
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = queryInsertValue;
        dbCommand.ExecuteNonQuery();

        return getAllItems();
    }

    private void closeDB()
    {
        dbConnection.Close();
    }
}