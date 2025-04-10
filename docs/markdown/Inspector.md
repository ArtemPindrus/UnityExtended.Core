# Inspector Features
You can use inspector features by:
- Utilizing Drawers within UnityExtended.Core.Editor.Drawers on custom inspectors.
- Decorating MonoBehaviour with CreateCustomInspectorAttribute described below.

## [CreateCustomInspectorAttribute](/src/Runtime/Generators/Attributes/CreateCustomInspectorAttribute.cs)
Decorate a MonoBehavior with this attribute to instruct source generator to create a custom inspector for this type with all the provided features used.

**Naming:**
For Namespace.MyClass generates Namespace.MyClassInspector

**Implementation:**
Created partial class that inherits UnityEditor.Editor and overrides CreateInspectorGUI method.

**Provides partial methods:**
- partial void PreRearrange(VisualElement root); Define to modify root before elements get rearranged by [SetVisualElementAtDrawer](https://github.com/ArtemPindrus/UnityExtended.Core/blob/main/Editor/Drawers/SetVisualElementAtDrawer.cs)
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
        targetCasted = (SomeClass)target;
        var root = new VisualElement();

        InspectorElement.FillDefaultInspector(root, serializedObject, this);

        DisplayDrawer.FillInFor(root, target);
        ButtonDrawer.FillIn(root, target);

        PreRearrange(root);
        SetVisualElementAtDrawer.Rearrange(root, target);

        root.schedule.Execute(Update).Every(50);

        ModifyRoot(ref root);

        return root;
    }

    partial void ModifyRoot(ref VisualElement root);

    partial void PreRearrange(VisualElement root);
}
```

Inspector:

![CreateCustomInspectorDemo](/docs/images/CreateCustomInspectorDemo.png)
![CreateCustomInspectorDemo2](/docs/images/CreateCustomInspectorDemo2.png)

Without CreateCustomInspector all the inspector features (Display, Button, etc.) will not work (unless you define your own custom inspector that uses Drawers).

## DisplayAttribute and DisplayItemAttribute
Decorate fields of primitive types or custom types with provided DisplayBag to display them in inspector without the need for serialization.

### Real-life example
MonoBehaviour Script using DisplayAttribute on primitives and one type decorated with DisplayItemAttribute with defined IDisplayBag:
```cs
/// <summary>
/// Gets horizontal angle change from the previous frame.
/// </summary>
[field: Display]
public float LastHorizontalAngleChange { get; private set; }

/// <summary>
/// Gets last vertical angle change from the previous frame.
/// </summary>
[field: Display]
public float LastVerticalAngleChange { get; private set; }

/// <summary>
/// Gets horizontal rotation produces by a mouse in the last frame.
/// </summary>
[field: Display]
public float LastMouseHorizontalDelta { get; private set; }

/// <summary>
/// Gets vertical rotation produces by a mouse in the last frame.
/// </summary>
[field: Display]
public float LastMouseVerticalDelta { get; private set; }

// Angles that script tries to achieve.
[field: Display]
public TargetAngles TargetAngles { get; private set; }
```

Inspector:

![DisplayItemDemo](/docs/images/DisplayItemDemo.png)

### DisplayItemAttribute and IDisplayBag
IDisplayBag is used to define appearance of non-primitive types.
Such types should be decorated with DisplayItemAttribute providing the Type of IDisplayBag implementation.

Example above.
MonoBehaviour:
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

DisplayItem:
```cs
#nullable enable
[DisplayItem(typeof(SomethingBag))]
public class Something {
    public string Name { get; set; }
    public int Id { get; set; }

    public Something(string name, int id) {
        Name = name;
        Id = id;
    }
}
```

Its IDisplayBag:
```cs
public class SomethingBag : IDisplayBag<Something> {
    private TextField nameField;
    private IntegerField idField;
    
    public VisualElement CreateVisualElement(string name) {
        var root = new VisualElement();

        Foldout f = new() {
            text = name
        };

        nameField = new("Name");
        idField = new("ID");
        
        f.Add(nameField, idField);
        
        root.Add(f);

        return root;
    }

    public void Update(Something? data) {
        nameField.value = data?.Name;
        idField.value = data?.Id ?? 0;
    }
}
```

Inspector:

![CreateCustomInspectorDemo](/docs/images/CreateCustomInspectorDemo.png)
