/*
 * NGCompiler.cs
 * Copyright (c) 2012 Nick Gravelyn
*/

using System.CodeDom.Compiler;
using System.Reflection;
using Microsoft.CSharp;
using UnityEditor;
using UnityEngine;
using System;

/// <summary>
/// Provides a simple interface for dynamically compiling C# code.
/// </summary>
public static class NGCompiler
{
    /// <summary>
    /// Compiles a C# script as if it were a file in your project.
    /// </summary>
    /// <param name="scriptText">The text of the script.</param>
    /// <param name="errors">The compiler errors and warnings from compilation.</param>
    /// <param name="assemblyIfSucceeded">The compiled assembly if compilation succeeded.</param>
    /// <returns>True if compilation was a success, false otherwise.</returns>
    public static bool CompileCSharpScript(string scriptText, out CompilerErrorCollection errors, out Assembly assemblyIfSucceeded)
    {
        var codeProvider = new CSharpCodeProvider();
        var compilerOptions = new CompilerParameters();

        // we want a DLL and we want it in memory
        compilerOptions.GenerateExecutable = false;
        compilerOptions.GenerateInMemory = true;

        // add references to all currently loaded assemblies
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            compilerOptions.ReferencedAssemblies.Add(assembly.Location);
        }

        // default to null output parameters
        errors = null;
        assemblyIfSucceeded = null;

        // compile the assembly from the source script text
        CompilerResults result = codeProvider.CompileAssemblyFromSource(compilerOptions, scriptText);

        // store the errors for the caller. even on successful compilation, we may have warnings.
        errors = result.Errors;

        // see if any errors are actually errors. if so return false
        foreach (CompilerError e in errors)
        {
            if (!e.IsWarning)
            {
                return false;
            }
        }

        // otherwise we pass back the compiled assembly and return true
        assemblyIfSucceeded = result.CompiledAssembly;
        return true;
    }

    /// <summary>
    /// Compiles a method body of C# script, wrapped in a basic void-returning method.
    /// </summary>
    /// <param name="methodText">The text of the script to place inside a method.</param>
    /// <param name="errors">The compiler errors and warnings from compilation.</param>
    /// <param name="methodIfSucceeded">The compiled method if compilation succeeded.</param>
    /// <returns>True if compilation was a success, false otherwise.</returns>
    public static bool CompileCSharpImmediateSnippet(string methodText, out CompilerErrorCollection errors, out MethodInfo methodIfSucceeded)
    {
        // wrapper text so we can compile a full type when given just the body of a method
        string methodScriptWrapper = @"
using UnityEngine; 
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
public static class CodeSnippetWrapper
{{
    public static void PerformAction()
    {{
        {0};
    }}
}}";

        // default method to null
        methodIfSucceeded = null;

        // compile the full script
        Assembly assembly;
        if (CompileCSharpScript(string.Format(methodScriptWrapper, methodText), out errors, out assembly))
        {
            // if compilation succeeded, we can use reflection to get the method and pass that back to the user
            methodIfSucceeded = assembly.GetType("CodeSnippetWrapper").GetMethod("PerformAction", BindingFlags.Static | BindingFlags.Public);
            return true;
        }

        // compilation failed, caller has the errors, return false
        return false;
    }
}