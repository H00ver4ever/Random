using UnityEngine;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;


public class LoadSavemanager : MonoBehaviour
{
    [XmlRoot("GameData")]
    public class LevelStateData
    {
        public struct DataTransform
        {
            public float posX;
            public float posY;
            public float posZ;
            public float rotX;
            public float rotY;
            public float rotZ;
            public float scaleX;
            public float scaleY;
            public float scaleZ;
        }

        // Data for enemy
        public class DataEnemy
        {
            //Enemy Transform Data
            public DataTransform posRotScale;

            //Enemy ID
            public int enemyID;

            //Health
            public int health;
        }

        // Data for player
        public class DataPlayer
        {
            //Transform Data
            public DataTransform posRotScale;

            //Has Collected sword
            public bool collectedWeapon;

            //Health
            public int health;
        }
        public List<DataEnemy> enemies = new List<DataEnemy>();
        public DataPlayer player = new DataPlayer();
    }

    public LevelStateData levelState = new LevelStateData();
    // Saves game data to XML file
    public void Save(string fileName = "LevelDesign.xml")
    {
        // Clear existing enemy data


        // Save game data
        XmlSerializer serializer = new XmlSerializer(typeof(LevelStateData));
        FileStream stream = new FileStream(fileName, FileMode.Create);
        serializer.Serialize(stream, levelState);
        stream.Flush();
        stream.Dispose();
        stream.Close();
    }

    // Load game data from XML file
    public void Load(string fileName = "LevelDesign.xml")
    {
        XmlSerializer serializer = new XmlSerializer(typeof(LevelStateData));
        FileStream stream = new FileStream(fileName, FileMode.Open);
        levelState = serializer.Deserialize(stream) as LevelStateData;
        stream.Flush();
        stream.Dispose();
        stream.Close();
    }
}
