using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using System.Xml.Serialization;

public class Save_Load : MonoBehaviour
{
    public Tilemap tilemap;
    public TilemapCollider2D tilemapCollider;

    public GameObject test;

    public bool save = false;

    void Update(){
        if(save){
            save = false;
            Save();
        }
    }

    public void Save(){
        XmlSerializer serializer = new XmlSerializer(test.GetType());
        StreamWriter writer = new StreamWriter(Application.dataPath + "/LevelData/test/maze/test.xml");
        serializer.Serialize(writer.BaseStream, test);
        writer.Close();
    }

    public void Load(){

    }
}
