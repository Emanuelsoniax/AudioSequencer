using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

public class ApplicationManager : MonoBehaviour
{
    [SerializeField]
    private Sequencer sequencer;

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "Sequencer.seq");
        Sequencer data = new Sequencer();
        data.sequences = sequencer.sequences;
        bf.Serialize(file, data);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/ playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/ playerInfo.dat", FileMode.Open);
            Sequencer data = (Sequencer)bf.Deserialize(file);
            file.Close();
            sequencer.sequences = data.sequences;
        }
    }


    public void Play()
    {
        foreach (NoteSequence _noteSequence in sequencer.sequences)
        {
            _noteSequence.UpdateSequence();
            StartCoroutine(sequencer.PlayTheSequence(_noteSequence));
        }
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

}
