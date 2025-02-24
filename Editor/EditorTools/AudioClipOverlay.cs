using System.Linq;
using External.UnityExtended.Core.Editor;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using UnityExtended.Core.EditorTools;
using UnityExtended.Core.Extensions;
using UnityExtended.Core.Utilities.AudioSourceBetter;
using ObjectField = UnityEditor.Search.ObjectField;

namespace External.UnityExtended.Core.Editor {
    [Overlay(typeof(SceneView), "AudioClip")]
    public class AudioClipOverlay : Overlay {
        private AudioClip currentClip;
        
        private ObjectField audioClipField, audioResourceBetterField;
        private PropertyField volumeField;
        private Button createBetterButton;

        #nullable enable
        public override VisualElement CreatePanelContent() {
            VisualElement root = new();

            audioClipField = new() {
                label = "AudioClip",
                objectType = typeof(AudioClip),
            };

            audioResourceBetterField = new() {
                label = "Associated AudioResourceBetter",
                objectType = typeof(AudioResourceBetter),
                enabledSelf = false
            };

            volumeField = new() {
                label = "AudioResourceBetter volume",
                visible = false,
            };

            createBetterButton = new(ButtonPressed) {
                text = $"Create {nameof(AudioResourceBetter)}",
                visible = false
            };

            if (Selection.activeObject is AudioClip activeAudioClip) {
                ChangeAudioClip(activeAudioClip);
            } else UnbindCurrentAudioClip();

            root.Add(audioClipField, audioResourceBetterField, createBetterButton, volumeField);
            
            root.schedule.Execute(Update).Every(50);

            return root;
        }

        private void Update() {
            if (Selection.activeObject is AudioClip audioClip) {
                if (audioClip == currentClip) return;
                
                ChangeAudioClip(audioClip);
            }
            else UnbindCurrentAudioClip(); 
        }

        private void ButtonPressed() {
            if (audioClipField.value is not AudioClip audioClip) return;

            ToAudioResourceBetter.CreateAudioResourceBetterAsset(audioClip, out var resource);

            ChangeAudioClip(audioClip, resource);
        }

        private void ChangeAudioClip(AudioClip audioClip) {
            if (audioClip == currentClip) return;

            var betterResourceAsset = AssetDatabaseHelper.LoadAllAssetsOfType<AudioResourceBetter>()
                .FirstOrDefault(x => x.resource == audioClip);

            ChangeAudioClip(audioClip, betterResourceAsset);
        }
        
        private void ChangeAudioClip(AudioClip audioClip, AudioResourceBetter? audioResourceBetter) {
            audioClipField.value = audioClip;

            audioResourceBetterField.value = audioResourceBetter;
            
            if (audioResourceBetter != null) {
                volumeField.visible = true;
                createBetterButton.visible = false;
                
                var serProp = new SerializedObject(audioResourceBetter).FindProperty("volume");
                volumeField.BindProperty(serProp);
            }
            else {
                createBetterButton.visible = true;
                volumeField.visible = false;
            }

            currentClip = audioClip;
        }

        private void UnbindCurrentAudioClip() {
            currentClip = null;

            audioClipField.value = null;
            audioResourceBetterField.value = null;

            createBetterButton.visible = false;
            
            volumeField.visible = false;
            volumeField.Unbind();
        }
    }
}
