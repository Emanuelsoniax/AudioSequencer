using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sequencer
{
    [SerializeField]
    public NoteSequence[] sequences;
    [SerializeField]
    private float interval;

    public void OnStart()
    {
        foreach (NoteSequence _noteSequence in sequences)
        {
            _noteSequence.UpdateSequence();
        }
    }

    public IEnumerator PlayTheSequence(NoteSequence _sequence, AudioSource _source)
    {
        for (int i = 0; i < _sequence.notes.Count; i++)
        {
            var note = _sequence.notes[i];
            _source.PlayOneShot(note.CreateClip(note.name, note.sampleRate, note.frequency));
            yield return new WaitForSeconds(note.duration);
            _source.Stop();
            yield return new WaitForSecondsRealtime(interval);
        }
    }

    public void CreateTheSequence()
    {
       
    }
}

[System.Serializable]
public class NoteSequence
{
    [SerializeField]
    private SequenceNode[] nodes;
    [HideInInspector]
    public List<NoteData> notes = new List<NoteData>();
    public InstrumentType sequenceInstrument;

    public void UpdateSequence()
    {
        notes.Clear();
        foreach (SequenceNode _node in nodes)
        {
            _node.ChangeInstrument(sequenceInstrument);
            notes.Add(_node.note);
            Debug.Log("added:" + _node.note.name);
        }
    }
}
