using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node : MonoBehaviour
{
    public Note note;
    public AudioSource audioSource;
    [SerializeField]
    private NoteSprites sprites;
    public Dictionary<string, Sprite> sprite = new Dictionary<string, Sprite>();
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; //force 2D sound
        audioSource.Stop(); //avoids audiosource from starting to play automatically
        SetSprites();
        note.SetValues();
    }

    public void ClickButton()
    {
        note.ChangeNote();
        GetComponent<Image>().sprite = sprite.GetValueOrDefault(note.name);
        StartCoroutine(PlayClip());
    }

    public void SetSprites()
    {
        sprite = new Dictionary<string, Sprite>()
        {
            ["A"] = sprites.A,
            ["B"] = sprites.B,
            ["C"] = sprites.C,
            ["D"] = sprites.D,
            ["E"] = sprites.E,
            ["F"] = sprites.F,
            ["G"] = sprites.G,
            ["none"] = sprites.none
        };
    }

    public IEnumerator PlayClip()
    {
        audioSource.PlayOneShot(note.CreateClip(note.name, note.sampleRate, note.frequency));
        yield return new WaitForSeconds(note.duration);
        audioSource.Stop();
    }

}

public enum NoteType { none = 0, A = 1, B = 2, C = 3, D = 4, E = 5, F = 6, G = 7 }

[System.Serializable]
public class Note
{
    public string name;
    public int sampleRate;
    public float frequency;
    public float duration;
    public NoteType note;

    public void SetValues()
    {
        switch (note)
        {
            case NoteType.none:
                name = "none";
                frequency = 0;
                return;
            case NoteType.A:
                name = "A";
                frequency = 440;
                return;
            case NoteType.B:
                name = "B";
                frequency = 494;
                return;
            case NoteType.C:
                name = "C";
                frequency = 523;
                return;
            case NoteType.D:
                name = "D";
                frequency = 587;
                return;
            case NoteType.E:
                name = "E";
                frequency = 659;
                return;
            case NoteType.F:
                name = "F";
                frequency = 698;
                return;
            case NoteType.G:
                name = "G";
                frequency = 784;
                return;
        }
    }

    public void ChangeNote()
    {
        if (note == NoteType.G)
        {
            note = 0;
        }
        else
        {
            note = (note + 1);
        }

        SetValues();
    }

    public AudioClip CreateClip(string _name, int _samplerate, float _frequency)
    {
        AudioClip clip = AudioClip.Create(_name, _samplerate, 1, _samplerate, false);

        var size = clip.frequency * (int)Mathf.Ceil(clip.length);
        float[] data = new float[size];

        int count = 0;
        while (count < data.Length)
        {
            data[count] = Mathf.Sin(2 * Mathf.PI * _frequency * count / _samplerate);
            count++;
        }

        clip.SetData(data, 0);
        return clip;
    }
}
