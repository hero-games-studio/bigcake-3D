using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager
{ 
    public UserData data
    {
        get;
        set;
    }

    private static DataManager instance = null;

    private DataManager() { }

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }

            return instance;
        }
    }

    public void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file
            = File.Open(OtherData.USERDATA_PATH, FileMode.OpenOrCreate);

        formatter.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(OtherData.USERDATA_PATH))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(OtherData.USERDATA_PATH, FileMode.Open);

            data = (UserData)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            data = new UserData();
            data.Init();
            Save();
            Load();
        }
    }
}