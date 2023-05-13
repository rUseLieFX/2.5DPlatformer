using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataCollector : MonoBehaviour
{

    static DataCollector instance;
    public static DataCollector Instance { get { return instance; } }
    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogError("T�bb DataCollector van!");
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        string destination = Application.persistentDataPath + "/save.dat";
        FileStream file;

        if (File.Exists(destination)) file = File.OpenRead(destination);
        else
        {
            Debug.Log("Nem tal�ltam f�jlt!");
            return;
        }


        BinaryFormatter bf = new BinaryFormatter();
        int[] data = (int[])bf.Deserialize(file);
        file.Close();

        GameData.maps = data;
    }

    public void MapCompleted(GameData.Episodes ep,int map)
    {
        bool changed = false;
        // Ha az adott episode-n�l j�r� sz�m kisebb, mint a p�lya sz�ma, akkor ezt el�sz�r csin�lja.
        // Ha eddig a 3-t a p�ly�t csin�lta meg, �s ez a 4., akkor friss�tsuk az adatot.
        if (GameData.maps[(int)ep] < map)
        {
            GameData.maps[(int)ep] = map;
            changed = true;
        }

        //Ez a sz�m azt jelenti, hogy 0-t�l kezdve a sz�m�t�st, hanyadik p�ly�ra kap enged�lyt.
        //Ha ez most a 0. p�lya, akkor az 1-es sz�m� p�ly�ra kell enged�lyt adni.
        
        // Ment�s
        if (changed)
        {
            string destination = Application.persistentDataPath + "/save.dat";
            FileStream file;

            if (File.Exists(destination)) file = File.OpenWrite(destination);
            else file = File.Create(destination);

            int[] data = GameData.maps;
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(file, data);
            file.Close();
        }
    }


}
