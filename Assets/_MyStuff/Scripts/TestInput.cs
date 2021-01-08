using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TestInput : MonoBehaviour {

    public CharacterInput input;

    // Use this for initialization
    void Start () {

        input = GetComponent<CharacterInput>();

    }

    public static bool InvokeboolMethod(Type calledType, string methodName)
    {
        // Get the Type for the class
         //calledType = Type.GetType(typeName);

        // Invoke the method itself. The string returned by the method winds up in s
        bool s = (bool)calledType.InvokeMember(
                        methodName,
                        BindingFlags.InvokeMethod | BindingFlags.Public |
                            BindingFlags.Static,
                        null,
                        null,
                        null);

        // Return the string that was returned by the called method.
        return s;
    }

    // Update is called once per frame
    void Update () {

       /* Type thisType = input.GetType();
        MethodInfo theMethod = thisType.GetMethod(TheCommandString);
        theMethod.Invoke(this, userParameters);
        */
       // print("TRYHEHEHEHEHEHEHEHE : "+InvokeboolMethod(input.GetType(), "PressLeftPunch"));

        Type thisType = input.GetType();
        MethodInfo theMethod = thisType.GetMethod("HoldLeftPunch");
        //if (theMethod != null)
            //print("trylolololololololololol : " + theMethod.Invoke(input, new object[] { }));


    }
}
