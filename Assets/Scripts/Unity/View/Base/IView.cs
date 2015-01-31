using UnityEngine;

namespace Unity.View
{
    public interface IView
    {
        Rect Rect{ get; set; }
        void Awake();
        void Start();
        void Update();
        void OnGUI();
        void OnRenderObject();
        void OnAudioFilterRead( float[] aSoundBuffer, int aChannels, int aSample );
        void OnApplicationQuit();
    }
}
