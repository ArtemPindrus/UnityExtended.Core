using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtended.Core.Extensions;
using Toggle = UnityEngine.UIElements.Toggle;

namespace UnityExtended.Core.Editor.EditorTools {
    public class ToAudioResourceBetterWindow : EditorWindow {
        private class Item {
            public readonly AudioClip AudioClip;
            public bool Operate;

            public Item(AudioClip audioClip, bool operate = true) {
                AudioClip = audioClip;
                Operate = operate;
            }
        }
    
        private List<Item> audioClips;
        private Foldout clipsFoldout;

        public static void Create() {
            GetWindow<ToAudioResourceBetterWindow>();
        }
    
        public static void Create(List<AudioClip> audioClips) {
            var clips = audioClips.Select(x => new Item(x)).ToList();
            var window = GetWindow<ToAudioResourceBetterWindow>();
            window.audioClips.AddRange(clips);

            foreach (var t in clips) {
                var clip = t;
                Toggle operateToggle = new() {
                    label = clip.AudioClip.name,
                    value = clip.Operate
                };
                operateToggle.RegisterValueChangedCallback(x => clip.Operate = x.newValue);
            
                window.clipsFoldout.Add(operateToggle);
            }
        }
    
        public void CreateGUI() {
            audioClips ??= new();
        
            VisualElement root = rootVisualElement;

            Label label = new("New AudioClips imported. For which ones to generate AudioResourceBetter?");

            clipsFoldout = new() {
                text = "Imported Clips"
            };

            Button createBetter = new(CreateButtonPressed) {
                text = "Create AudioSourceBetter for AudioClips in the list."
            };
        
            root.Add(label, clipsFoldout, createBetter);
        }

        private void CreateButtonPressed() {
            foreach (var clip in audioClips.Where(x => x.Operate)) {
                ToAudioResourceBetter.CreateAudioResourceBetterAsset(clip.AudioClip, out _);
            }
        }
    }
}
