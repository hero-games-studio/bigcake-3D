using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager<T> where T : IUserData
{
    public T data { get; set; }

    private static DataManager<T> instance = null;

    private DataManager() { }

    public static DataManager<T> Instance()
    {
        if (instance == null)
        {
            instance = new DataManager<T>();
        }

        return instance;
    }

    public void Save(T data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file;

        file = File.Open(OtherData.USERDATA_PATH, FileMode.OpenOrCreate);

        formatter.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(OtherData.USERDATA_PATH))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(OtherData.USERDATA_PATH, FileMode.Open);

            data = (T)formatter.Deserialize(file);
            file.Close();
        }
        else
        {
            data.Init();
            Save(data);
            Load();
        }
    }
}