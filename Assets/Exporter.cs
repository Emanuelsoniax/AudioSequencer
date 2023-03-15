using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exporter: MonoBehaviour
{
   
    public void Export(ApplicationData applicationData)
    {
        FindObjectOfType<AudioSource>().clip.SetData(CombineSequenceToSingleClip(applicationData.sequences[0].notes), 0);
        FindObjectOfType<AudioSource>().Play();
    }

    private float[] CombineSequenceToSingleClip(List<NoteData> _notes)
    {
        float[] singleClipData = new float[_notes.Count];

        for (int i = 0; i < _notes.Count;)
        {
            var note = _notes[i];

            singleClipData = note.CreateData(note.name, note.sampleRate, note.frequency);
        }

        return singleClipData;
    }

}
