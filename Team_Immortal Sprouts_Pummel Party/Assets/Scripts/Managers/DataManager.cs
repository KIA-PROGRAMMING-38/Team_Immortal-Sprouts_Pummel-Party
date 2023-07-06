using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager
{
    public RoomData Room = new RoomData();
    public PlayerData Player = new PlayerData();
    public ItemData Item =new ItemData();
    public MiniGameData MiniGame = new MiniGameData();

    public void InitCSV()
    {
        Player.ReadCSV();
        Item.ReadCSV();
        MiniGame.ReadCSV();
    }
}
