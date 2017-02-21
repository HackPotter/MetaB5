using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

// Entry point for testing login logic.
public class Test : MonoBehaviour
{
    private bool loggedin = false;

    void Update()
    {
        // If user presses Space, then perform the login operation.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Starting login process...");

            LoginOperation.Create(LoginCriteria.Create("greg7", "asdf2")).ExecuteAsync(Callback);

            // Alternatively, executes the operation synchronously.
            //  This WILL block until the web operation is completed (sometimes it takes as long as ~250 frames), so asynchronous is preferred to prevent gameplay hiccups.
            //LoginOperationResult result = operation.Execute();

            Debug.Log("Finished login operation!");
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            RegisterOperationCriteria criteria = RegisterOperationCriteria.Builder.Create()
                .Username("greg900563")
                .Password("")
                .Email("gregory.hanes256236@gmail.com")
                .DateOfBirth(new DateTime(1989, 11, 25))
                .Gender(RegistrationCriteriaGender.Male)
                .ZipCode("52333").Build();

            RegisterOperation registerOperation = RegisterOperation.Create(criteria);
            registerOperation.ExecuteAsync(RegisterCallback);
        }
    }

    void Callback(LoginOperationResult result)
    {
        Debug.Log("Got response of login!");
        Debug.Log(result.AuthenticationToken);
        loggedin = true;
    }

    void RegisterCallback(RegisterOperationResult result)
    {
        Debug.Log("Got response of registration!");
        //Debug.Log(result.Text);
        
    }

    void OnGUI()
    {
        if (loggedin)
        {
            GUILayout.Label("LOGGED IN!!!!!!!!!!!!!!!!!!!!!!!!");
        }
    }
}

