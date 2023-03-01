using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public Node[] nodes;
    private NoteSequence sequence = new NoteSequence();
    [SerializeField]
    private AudioSource source;
    [SerializeField]
    private float interval;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            UpdateSequence();
        }
    }

    // Update is called once per frame
    void UpdateSequence()
    {
        sequence.notes.Clear();
        foreach(Node _node in nodes)
        {
            sequence.notes.Add(_node.note);
            Debug.Log("added:" + _node.note.name);
        }
    }

    public void Play()
    {
        UpdateSequence();
        StartCoroutine(PlayTheSequence(sequence));
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

public class NoteSequence
{
    public List<Note> notes = new List<Note>();
}
