using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    [SerializeField]
    private NoteSequence[] sequences;
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private float interval;

    public void Play()
    {
        foreach (NoteSequence _noteSequence in sequences)
        {
            _noteSequence.UpdateSequence();
            StartCoroutine(PlayTheSequence(_noteSequence));
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

}

[System.Serializable]
public class NoteSequence
{
    [SerializeField]
    private SequenceNode[] nodes;
    [HideInInspector]
    public List<Note> notes = new List<Note>();

    public void UpdateSequence()
    {
        notes.Clear();
        foreach (SequenceNode _node in nodes)
        {
            notes.Add(_node.note);
            Debug.Log("added:" + _node.note.name);
        }
    }
}
