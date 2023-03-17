using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exporter
{
    private ApplicationData data;

    public void Export(ApplicationData applicationData)
    {
        data = applicationData;
        SavWav.Save("test", MergeToSingleClip());
    }

    private AudioClip MergeToSingleClip()
    {
        AudioClip mixedclipA = CombineSequences(SequenceAudioClips(GetAudioClips(data.sequences[0])), SequenceAudioClips(GetAudioClips(data.sequences[1])));
        AudioClip mixedclipB = CombineSequences(SequenceAudioClips(GetAudioClips(data.sequences[2])), SequenceAudioClips(GetAudioClips(data.sequences[3])));
        AudioClip mixedclip = CombineSequences(mixedclipA,mixedclipB);
        return mixedclip;
    }

    private List<AudioClip> GetAudioClips(SequenceData _sequence)
    {
        List<AudioClip> clips = new List<AudioClip>();
        for (int i =0; i < _sequence.notes.Count; i++)
        {
            NoteData note = _sequence.notes[i];
            clips.Add(note.CreateClip(note.name, note.sampleRate, note.frequency));
        }

        return clips;
    }

    //source
    //https://answers.unity.com/questions/513408/combine-2-audio-clip-in-one.html

    private AudioClip SequenceAudioClips (List<AudioClip> _clips)
    {
        if (_clips == null || _clips.Count == 0)
            return null;

        int length = 0;
        for (int i = 0; i < _clips.Count; i++)
        {
            if (_clips[i] == null)
                continue;

            length += _clips[i].samples * _clips[i].channels;
        }

        float[] data = new float[length];
        length = 0;
        for (int i = 0; i < _clips.Count; i++)
        {
            if (_clips[i] == null)
                continue;

            float[] buffer = new float[_clips[i].samples * _clips[i].channels];
            _clips[i].GetData(buffer, 0);
            //System.Buffer.BlockCopy(buffer, 0, data, length, buffer.Length);
            buffer.CopyTo(data, length);
            length += buffer.Length;
        }

        if (length == 0)
            return null;

        AudioClip result = AudioClip.Create("CombinedClip", length, 1, 4410, false);
        result.SetData(data, 0);

        return result;
    }

    private AudioClip CombineSequences(AudioClip clipA, AudioClip clipB)
    {
        float[] floatSamplesA = new float[clipA.samples * clipA.channels];
        clipA.GetData(floatSamplesA, 0);

        float[] floatSamplesB = new float[clipB.samples * clipB.channels];
        clipB.GetData(floatSamplesB, 0);

        float[] mixedSamples = MixAndClampFloatBuffers(floatSamplesA, floatSamplesB); 

        AudioClip result = AudioClip.Create("Combine", mixedSamples.Length, clipA.channels, clipA.frequency,
         false);
        result.SetData(mixedSamples, 0);
        return result;
    }

    private float ClampToValidRange(float value)
    {
        float min = -1.0f;
        float max = 1.0f;
        return (value < min) ? min : (value > max) ? max : value;
    }

    private float[] MixAndClampFloatBuffers(float[] bufferA, float[] bufferB)
    {
        int maxLength = Mathf.Min(bufferA.Length, bufferB.Length);
        float[] mixedFloatArray = new float[maxLength];

        for (int i = 0; i < maxLength; i++)
        {
            mixedFloatArray[i] = ClampToValidRange((bufferA[i] + bufferB[i]));
        }
        return mixedFloatArray;
    }

}
