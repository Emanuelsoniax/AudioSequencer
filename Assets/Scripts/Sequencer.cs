using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Sequencer
{
    [SerializeField]
    public NoteSequence[] sequences;
    [SerializeField]
    private float interval;
    public ApplicationManager manager;
    public bool islooping;
    private float noteDuration;
    public float NoteDuration
    {
        get { return noteDuration; }
        set { noteDuration = value;
            if(noteDuration < 0)
            {
                noteDuration = 0;
            }
            foreach(NoteSequence noteSequence in sequences)
            {
                noteSequence.UpdateNoteDuration(noteDuration);
            }
            UpdateText();
        }
    }
    [SerializeField]
    public TextMeshProUGUI noteDurationText;
    public void OnStart(ApplicationManager _manager)
    {
        manager = _manager;
        NoteDuration = 0.14f;
        foreach (NoteSequence _noteSequence in sequences)
        {
            _noteSequence.SetSprites(_manager.instrumentSprites);
            _noteSequence.UpdateSequence();
        }
    }

    public void UpdateText()
    {
        noteDurationText.text = noteDuration.ToString("0.00");
    }

    public IEnumerator PlayTheSequence(NoteSequence _sequence, AudioSource _source)
    {
        manager.sequenceBar.SetActive(true);
        for (int i = 0; i < _sequence.notes.Count; i++)
        {
            var note = _sequence.notes[i];
            manager.sequenceBar.GetComponent<RectTransform>().position = manager.barPositions[i].position;
            _source.PlayOneShot(note.CreateClip(note.name, note.sampleRate, note.frequency));
            yield return new WaitForSeconds(note.duration);
            _source.Stop();
            yield return new WaitForSecondsRealtime(interval);
        }
        if (islooping)
        {
            yield return PlayTheSequence(_sequence, _source);
        }
        else
        {
            manager.sequenceBar.SetActive(false);
            yield return null;
        }
    }
}

[System.Serializable]
public class NoteSequence
{
    [SerializeField]
    private SequenceNode[] nodes;
    [SerializeField]
    private Image instrumentImage;
    [HideInInspector]
    public List<NoteData> notes = new List<NoteData>();
    public InstrumentType sequenceInstrument;
    private Dictionary<InstrumentType, Sprite> sprite;

    public void UpdateSequence()
    {
        notes.Clear();
        foreach (SequenceNode _node in nodes)
        {
            _node.UpdateInstrument(sequenceInstrument);
            notes.Add(_node.note);
        }

        instrumentImage.sprite = sprite.GetValueOrDefault(sequenceInstrument);
    }

    public void UpdateNoteDuration(float _duration)
    {
        foreach (SequenceNode _node in nodes)
        {
            _node.note.duration = _duration;
        }
    }

    public void SetSprites(InstrumentSprites sprites)
    {
        sprite = new Dictionary<InstrumentType, Sprite>()
        {
            [InstrumentType.Square] = sprites.squareSprite,
            [InstrumentType.Sawtooth] = sprites.sawtoothSprite,
            [InstrumentType.Sine] = sprites.sineSprite,
            [InstrumentType.Noise] = sprites.noiseSprite
        };
    }

    public void ChangeInstrument()
    {
        if (sequenceInstrument == InstrumentType.Noise)
        {
            sequenceInstrument = 0;
        }
        else
        {
            sequenceInstrument = (sequenceInstrument + 1);
        }

        UpdateSequence();
    }


    public void ApplySequenceToNode(List<NoteData> _notes)
    {
        for (int i = 0; i < _notes.Count; i++)
        {
            nodes[i].note = _notes[i];
            nodes[i].note.SetValues();
            nodes[i].ChangeSprite();
        }
    }

    public SequenceData GetData()
    {
        SequenceData data = new SequenceData();

        data.notes = notes;
        data.sequenceInstrument = sequenceInstrument;

        return data;
    }

    public void ApplyData(SequenceData data)
    {
        sequenceInstrument = data.sequenceInstrument;
        notes = data.notes;

        ApplySequenceToNode(notes);
    }
}

public class SequenceData
{
    public List<NoteData> notes;
    public InstrumentType sequenceInstrument;
}
