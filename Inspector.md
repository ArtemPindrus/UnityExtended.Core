# Inspector Features
You can use inspector features by:
- Utilizing Drawers within UnityExtended.Core.Editor.Drawers on custom inspectors.
- Decorating MonoBehaviour with CreateCustomInspectorAttribute described below.

## [CreateCustomInspectorAttribute](https://github.com/ArtemPindrus/UnityExtended.Core/blob/main/Generators/Attributes/CreateCustomInspectorAttribute.cs)
Decorate a MonoBehavior with this attribute to instruct source generator to create a custom inspector for this type with all the provided features used.

**Naming:**
For Namespace.MyClass generates Namespace.MyClassInspector

**Implementation:**
Created partial class that inherits UnityEditor.Editor and overrides CreateInspectorGUI method.

**Provides partial methods:**
- partial void Update2(); Define to execute statements on root's schedule every x milliseconds.
- partial void PreRearrange(ref VisualElement root); Define to modify root before elements get rearranged by [SetVisualElementAtDrawer](https://github.com/ArtemPindrus/UnityExtended.Core/blob/main/Editor/Drawers/SetVisualElementAtDrawer.cs)
- partial void ModifyRoot(ref VisualElement root); Define to modify root after all the other operations. Root is guaranteed to NOT be modified by generator provided partial definition after this method.

### Example
Client Code:
```cs
[CreateCustomInspector]
public class SomeClass : MonoBehaviour {
    [Display]
    private float someField;

    [Display] private Something something;
    
    [Button]
    private void M() {
        something = new("MyName", 1);
        Debug.Log("Was called!");
    }
}
```

Source Generated Inspector:
```cs
[UnityEditor.CustomEditor(typeof(SomeClass))]
partial class SomeClassInspector : UnityEditor.Editor{
    private SomeClass targetCasted;

    public override VisualElement CreateInspectorGUI() {
        // Reservation Main
        targetCasted = (SomeClass)target;
        var root = new VisualElement();

        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        // Reservation CustomInspectorMainRes
        DisplayDrawer.FillInFor(root, target);
        ButtonDrawer.FillIn(root, target);

        PreRearrange(ref root);
        SetVisualElementAtDrawer.Rearrange(root, target);

        // Reservation FinishRes
        root.schedule.Execute(Update).Every(50);

        ModifyRoot(ref root); // modify before returning;

        return root;
    }

    private void Update() {
        // Reservation Main
        if (!Application.isPlaying) return;


        // Reservation CustomInspectorMainRes

        // Reservation FinishRes
         Update2();
    }

    partial void ModifyRoot(ref VisualElement root);

    partial void Update2();

    partial void PreRearrange(ref VisualElement root);
}
```

Inspector:

![](docs/CreateCustomInspectorDemo.png)
![](docs/CreateCustomInspectorDemo2.png)

Without CreateCustomInspector all the inspector features (Display, Button, etc.) will not work (unless you define your own custom inspector that uses Drawers).
