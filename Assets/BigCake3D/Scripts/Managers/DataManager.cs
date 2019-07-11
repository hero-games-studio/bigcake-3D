using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager
{
    public void Save(UserData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream file = File.Create(OtherData.USERDATA_PATH);

        formatter.Serialize(file, data);
        file.Close();
    }

    public UserData Load()
    {
        UserData data = null;
        if (File.Exists(OtherData.USERDATA_PATH))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream file = File.Open(OtherData.USERDATA_PATH, FileMode.Open);

            data = (UserData) formatter.Deserialize(file);
        }
        return data;
    }

}
