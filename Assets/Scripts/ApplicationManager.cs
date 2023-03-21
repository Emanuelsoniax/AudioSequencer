using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using SFB;

[RequireComponent (typeof(AudioSource))]
public class ApplicationManager : MonoBehaviour
{
    [SerializeField]
    private Sequencer sequencer;
    private Exporter exporter = new Exporter();

    public void Start()
    {
        sequencer.OnStart(this);
    }

    private void InitializeWindowSettings()
    {
       
    }

    public void Export()
    {
        ApplicationData data = new ApplicationData(sequencer.sequences[0].GetData(), sequencer.sequences[1].GetData(), sequencer.sequences[2].GetData(), sequencer.sequences[3].GetData());
        exporter.Export(data);
    }

    public void Save()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(ApplicationData));

        ApplicationData data = new ApplicationData(sequencer.sequences[0].GetData(), sequencer.sequences[1].GetData(), sequencer.sequences[2].GetData(), sequencer.sequences[3].GetData());

        // Save file with filter
        var extensionList = new[] {
        new ExtensionFilter("SEQUENCER", "seq"),
        };
        var path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "MySaveFile", extensionList);

        if(path == null)
        {
            Debug.Log("empty");
            return;
        }

        FileStream stream = new FileStream(path, FileMode.Create);
        using (stream)
        {
            serializer.Serialize(stream, data);
        }
    }

    public void Load()
    {
       XmlSerializer serializer = new XmlSerializer(typeof(ApplicationData));

        var extensionList = new[] {
        new ExtensionFilter("SEQUENCER", "seq"),
       };
        
       var path = StandaloneFileBrowser.OpenFilePanel("Open File", "", extensionList, false);

        if (path.Length == 0)
        {
            Debug.Log("empty");
            return;
        }

        FileStream stream = new FileStream(path[0], FileMode.Open);

       ApplicationData data = serializer.Deserialize(stream) as ApplicationData;
 

        for (int i = 0; i < sequencer.sequences.Length; i++)
        {
           sequencer.sequences[i].ApplyData(data.sequences[i]);
        }
    }
    



    public void Play()
    {
        Stop();
        foreach (NoteSequence _noteSequence in sequencer.sequences)
        {
            _noteSequence.UpdateSequence();
            StartCoroutine(sequencer.PlayTheSequence(_noteSequence, GetComponent<AudioSource>()));
        }
    }

    public void Stop()
    {
        StopAllCoroutines();
        GetComponent<AudioSource>().Stop();
    }

    public void Loop()
    {
        if (!sequencer.islooping) sequencer.islooping = true;
        else sequencer.islooping = false;
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

}

[System.Serializable]
public class ApplicationData
{
    public List<SequenceData> sequences;

    public ApplicationData() { }

    public ApplicationData(SequenceData sequenceA, SequenceData sequenceB, SequenceData sequenceC, SequenceData sequenceD)
    {
        sequences = new List<SequenceData>() { sequenceA, sequenceB, sequenceC, sequenceD };
    }
}


