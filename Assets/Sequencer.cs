using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequencer : MonoBehaviour
{
    public Node[] nodes;
    private NoteSequence sequence = new NoteSequence();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            UpdateSequence();
        }
    }

    // Update is called once per frame
    void UpdateSequence()
    {
        sequence.notes = new List<Note>();
        foreach(Node _node in nodes)
        {
            sequence.notes.Add(_node.note);
        }
    }

    public void Play()
    {
        StartCoroutine(PlayTheSequence(sequence));
    }

    public IEnumerator PlayTheSequence(NoteSequence sequence)
    {
        for (int i = 0; i < sequence.notes.Count; i++)
        {
            var note = sequence.notes[i];
            nodes[i].PlayClip();
            yield return new WaitForSeconds(note.duration);
            nodes[i].audioSource.Stop();
        }
    }

}

public class NoteSequence
{
    public List<Note> notes = new List<Note>();
}
