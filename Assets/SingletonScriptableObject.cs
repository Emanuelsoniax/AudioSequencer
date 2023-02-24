using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T> {

    private static T instance;
    public static T Instance {
        get {
            if (instance == null) {
                T[] assets = Resources.LoadAll<T>("");
                if(assets == null || assets.Length < 1) {
                    throw new System.Exception("No objects of type " + typeof(T) + " are present in the Resources folder.");
                }
                else if(assets.Length > 1) {
                    Debug.LogWarning("Multiple instances of type " + typeof(T) + " were present in the Resources folder.");
                }
                instance = assets[0];
            }
            return instance;
        }
    }

}