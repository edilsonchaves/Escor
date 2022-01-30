using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GamaManager : MonoBehaviour
{
    public static GamaManager gm;
    public int life;
    public int poder;

    // private conferirMenu _conferir;

    

    void Awake()
    {
        if(gm == null)
        {
            gm = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(gameObject);
        }

        //_conferir = GameObject.FindObjectOfType<conferirMenu>();
        
    }

   

    /*public void Save()
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.OpenOrCreate);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();

            formatter.Serialize(file, _conferir.myStats);
        }
        catch (SerializationException exc)
        {
            Debug.LogError(" There was an issue serializing this data: " + exc.Message);
        }
        finally
        {
            file.Close();
        }        
    }

    public void Load()
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.Open);
       
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            _conferir.myStats = formatter.Deserialize(file) as Stats;
        }
        catch(SerializationException exc )
        {
            Debug.LogError("Error deserializing data" + exc.Message);
        }
        finally
        {
            file.Close();
        }      
        
    }*/

    public void SetLife(int life)
    {
        life += life;
    }

    public int GetLife()
    {
        return life;
    }


}
