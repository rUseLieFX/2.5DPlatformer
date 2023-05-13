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
            Debug.LogError("Több DataCollector van!");
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MapCompleted(GameData.Episodes ep,int map)
    {
        // Ha az adott episode-nál járó szám kisebb, mint a pálya száma, akkor ezt elõször csinálja.
        // Ha eddig a 3-t a pályát csinálta meg, és ez a 4., akkor frissítsuk az adatot.
        if (GameData.maps[(int)ep] < map) GameData.maps[(int)ep] = map;

        //Ez a szám azt jelenti, hogy 0-tól kezdve a számítást, hanyadik pályára kap engedélyt.
        //Ha ez most a 0. pálya, akkor az 1-es számú pályára kell engedélyt adni.

        //TODO: Ha a következõ pálya száma már nem létezõ szám, akkor a következõ EP-re kapjunk engedélyt.
        // Ez még átfontolás tárgya, mivel lehet jobb lenne a játékosnak, ha az összes EP fel lenne oldva, és nem lenne *annyira* korlátozva.

        // Mentés

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
