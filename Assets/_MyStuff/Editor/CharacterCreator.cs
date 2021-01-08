using UnityEditor;
using UnityEngine;
using garagekitgames;
using SO;

public class CharacterCreator : EditorWindow
{

    Color color;
    garagekitgames.BodyPart[] bodyParts; 

    [MenuItem("Window/CharacterCreator")]
    public static void ShowWindow()
    {
        GetWindow<CharacterCreator>("Character Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Color the selected objects!", EditorStyles.boldLabel);

        color = EditorGUILayout.ColorField("Color", color);

        var serializedObject = new SerializedObject(bodyParts);
       // bodyParts = EditorGUILayout.PropertyField(serializedObject.FindProperty(), true);
        if (GUILayout.Button("Create Character!"))
        {
            Colorize();
        }
    }

    void Colorize()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial.color = color;
            }

            CharacterJoint[] charJoints = obj.GetComponentsInChildren<CharacterJoint>();
            int i = 0;
            foreach (CharacterJoint charJoint in charJoints)
            {
                ConfigurableJoint confJoint;
                if (!charJoint.transform.GetComponent<ConfigurableJoint>())
                {
                    i++;
                    confJoint = charJoint.gameObject.AddComponent<ConfigurableJoint>() as ConfigurableJoint;
                    //				confJoint.autoConfigureConnectedAnchor = false;
                    confJoint.connectedBody = charJoint.connectedBody;
                    confJoint.anchor = charJoint.anchor;
                    confJoint.axis = charJoint.axis;
                    //				confJoint.connectedAnchor = charJoint.connectedAnchor;
                    confJoint.secondaryAxis = charJoint.swingAxis;
                    confJoint.xMotion = ConfigurableJointMotion.Locked;
                    confJoint.yMotion = ConfigurableJointMotion.Locked;
                    confJoint.zMotion = ConfigurableJointMotion.Locked;
                    confJoint.angularXMotion = ConfigurableJointMotion.Limited;
                    confJoint.angularYMotion = ConfigurableJointMotion.Limited;
                    confJoint.angularZMotion = ConfigurableJointMotion.Limited;
                    confJoint.lowAngularXLimit = charJoint.lowTwistLimit;
                    confJoint.highAngularXLimit = charJoint.highTwistLimit;
                    confJoint.angularYLimit = charJoint.swing1Limit;
                    confJoint.angularZLimit = charJoint.swing2Limit;
                    confJoint.rotationDriveMode = RotationDriveMode.Slerp;

                    confJoint.enablePreprocessing = false;
                    confJoint.projectionDistance = 0.1f;
                    confJoint.projectionAngle = 180f;
                    confJoint.projectionMode = JointProjectionMode.PositionAndRotation;
                    //				JointDrive temp = confJoint.slerpDrive; // These are left here to remind us how to set the drive
                    //				temp.mode = JointDriveMode.Position;
                    //				temp.positionSpring = 0f;
                    //				confJoint.slerpDrive = temp;
                    //				confJoint.targetRotation = Quaternion.identity;
                }
                DestroyImmediate(charJoint);
            }
            Debug.Log("Replaced " + i + " CharacterJoints with ConfigurableJoints on " + this.name);
        }
    }
}
