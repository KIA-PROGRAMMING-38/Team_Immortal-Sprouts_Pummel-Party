using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public static RoomData Room = new RoomData();
    public static PlayerData Player = new PlayerData();
    public static ItemData Item =new ItemData();
    public static MiniGameData MiniGame = new MiniGameData();

    public void InitCSV()
    {
        Player.ReadCSV();
        Item.ReadCSV();
        MiniGame.ReadCSV();
    }
}
