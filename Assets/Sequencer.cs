using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sequencer
{
    [SerializeField]
    public NoteSequence[] sequences;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private float interval;

    public void OnStart()
    {
        foreach (NoteSequence _noteSequence in sequences)
        {
            _noteSequence.UpdateSequence();
        }
    }

    public IEnumerator PlayTheSequence(NoteSequence sequence)
    {
        for (int i = 0; i < sequence.notes.Count; i++)
        {
            var note = sequence.notes[i];
            source.PlayOneShot(note.CreateClip(note.name, note.sampleRate, note.frequency));
            yield return new WaitForSeconds(note.duration);
            source.Stop();
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
    public List<Note> notes = new List<Note>();
    public Instrument sequenceInstrument;

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
