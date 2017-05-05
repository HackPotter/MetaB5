using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.IO;

namespace uTest
{
    public class TestDefinition
    {
        public TestResult Result
        {
            get;
            private set;
        }

        public string TestName
        {
            get;
            private set;
        }

        private MethodInfo _test;
        private Type _expectedExceptionType;

        private List<MethodInfo> _postConditions;

        public TestDefinition(string testName, MethodInfo testMethod, List<MethodInfo> postConditions, Type expectedExceptionType = null)
        {
            _test = testMethod;
            TestName = testName;
            _expectedExceptionType = expectedExceptionType;
            _postConditions = postConditions;
            Result = new TestResult();
        }

        public void Run(object target)
        {
            try
            {
                Assert.ResetAssertionCount();
                _test.Invoke(target, null);

                if (_expectedExceptionType != null)
                {
                    Result.TestStatus = TestStatus.Failed;
                    Result.ResultMessage = "Expected exception " + _expectedExceptionType.Name + ", but no exception was thrown.";
                    return;
                }
            }
            catch (TargetInvocationException e)
            {
                Result.TestStatus = TestStatus.Failed;
                if (e.InnerException is uAssertionException)
                {
                    Result.ResultMessage = "Failed at assertion " + Assert.AssertionCount + ": " + e.InnerException.Message + "\n";
                    
                    var stack = new StackTrace(e.InnerException,true);

                    Result.ResultMessage += "Line: " + stack.GetFrame(1).GetFileLineNumber() + "\n";
                    Result.ResultMessage += "File: " + Path.GetFileNameWithoutExtension(stack.GetFrame(1).GetFileName()) + "\n";
                    Result.ResultMessage += "Failed " + stack.GetFrame(0).GetMethod().Name + " assertion";
                    
                }
                else if (e.InnerException.GetType() == _expectedExceptionType)
                {
                    Result.TestStatus = TestStatus.Passed;
                }
                else
                {
                    Result.ResultMessage = "Failed at assertion " + Assert.AssertionCount + "\n\n" + "Unexpected exception: " + e.InnerException.Message + "\n" +
                        e.InnerException.StackTrace;
                }
                return;
            }

            try
            {
                foreach (MethodInfo method in _postConditions)
                {
                    method.Invoke(target, null);
                }
            }
            catch (TargetInvocationException e)
            {
                Result.TestStatus = TestStatus.Failed;
                if (e.InnerException is uAssertionException)
                {
                    Result.ResultMessage = "Post condition failure at assertion " + Assert.AssertionCount + ": " + e.InnerException.Message;
                }
                else
                {
                    Result.ResultMessage = "Failed at assertion " + Assert.AssertionCount + "\n\n" + "Unexpected exception: " + e.InnerException.Message + "\n" + e.InnerException.StackTrace;
                }
                return;
            }
            Result.TestStatus = TestStatus.Passed;
            Result.ResultMessage = "";
        }
    }

    public class TestFixtureDefinition
    {
        public string Name
        {
            get { return TestFixtureType.Name; }
        }
        public Type TestFixtureType
        {
            get;
            private set;
        }

        public TestStatus TestStatus
        {
            get
            {
                if (Tests.Count((td) => td.Result.TestStatus == TestStatus.Failed) != 0)
                {
                    return TestStatus.Failed;
                }

                if (Tests.All((td) => td.Result.TestStatus == TestStatus.Passed))
                {
                    return TestStatus.Passed;
                }


                return TestStatus.Unknown;
            }
        }

        public int PostConditionCount
        {
            get { return _postConditions.Count; }
        }

        private List<MethodInfo> _fixtureSetup = new List<MethodInfo>();
        private List<MethodInfo> _setup = new List<MethodInfo>();
        private List<MethodInfo> _teardown = new List<MethodInfo>();
        private List<MethodInfo> _fixtureTeardown = new List<MethodInfo>();
        private List<MethodInfo> _postConditions = new List<MethodInfo>();

        public List<TestDefinition> Tests
        {
            get;
            private set;
        }

        public TestFixtureDefinition(Type type)
        {
            TestFixtureType = type;
            Tests = new List<TestDefinition>();
            foreach (MethodInfo methodInfo in type.GetAllMethods().Reverse())
            {
                if (methodInfo.GetParameters().Length != 0)
                {
                    continue;
                }
                if (methodInfo.IsDefined(typeof(uTestFixtureSetupAttribute), false))
                {
                    _fixtureSetup.Add(methodInfo);
                }
                if (methodInfo.IsDefined(typeof(uTestFixtureTeardownAttribute), false))
                {
                    _fixtureTeardown.Add(methodInfo);
                }
                if (methodInfo.IsDefined(typeof(uSetupAttribute), false))
                {
                    _setup.Add(methodInfo);
                }
                if (methodInfo.IsDefined(typeof(uTearDownAttribute), false))
                {
                    _teardown.Add(methodInfo);
                }
                if (methodInfo.IsDefined(typeof(uPostConditionTest), false))
                {
                    _postConditions.Add(methodInfo);
                }
                if (methodInfo.IsDefined(typeof(uTestAttribute), false))
                {
                    Type expectedExceptionType = null;
                    List<MethodInfo> testPostConditions = null;

                    if (methodInfo.IsDefined(typeof(uTestExpectedExceptionAttribute), false))
                    {
                        uTestExpectedExceptionAttribute expectedExceptionAttribute = methodInfo.GetCustomAttributes(typeof(uTestExpectedExceptionAttribute), false)[0] as uTestExpectedExceptionAttribute;
                        expectedExceptionType = expectedExceptionAttribute.ExceptionType;
                    }
                    if (!methodInfo.IsDefined(typeof(uIgnorePostConditionsAttribute), false))
                    {
                        testPostConditions = _postConditions;
                    }

                    Tests.Add(new TestDefinition(methodInfo.Name, methodInfo, testPostConditions ?? new List<MethodInfo>(), expectedExceptionType));
                }
            }
        }

        public void Run()
        {
            object instance = Activator.CreateInstance(TestFixtureType);

            foreach (MethodInfo fixtureSetup in _fixtureSetup)
            {
                fixtureSetup.Invoke(instance, null);
            }
            foreach (TestDefinition test in Tests)
            {
                foreach (MethodInfo setup in _setup)
                {
                    setup.Invoke(instance, null);
                }

                test.Run(instance);

                foreach (MethodInfo teardown in _teardown)
                {
                    teardown.Invoke(instance, null);
                }
            }
            foreach (MethodInfo fixtureTeardown in _fixtureTeardown)
            {
                fixtureTeardown.Invoke(instance, null);
            }
        }

        public void RunSingle(int index)
        {
            TestDefinition testDefinition = Tests[index];
            object instance = Activator.CreateInstance(TestFixtureType);
            foreach (MethodInfo fixtureSetup in _fixtureSetup)
            {
                fixtureSetup.Invoke(instance, null);
            }
            foreach (MethodInfo setup in _setup)
            {
                setup.Invoke(instance, null);
            }

            testDefinition.Run(instance);

            foreach (MethodInfo teardown in _teardown)
            {
                teardown.Invoke(instance, null);
            }
            foreach (MethodInfo fixtureTeardown in _fixtureTeardown)
            {
                fixtureTeardown.Invoke(instance, null);
            }
        }

        public void ClearResults()
        {
            foreach (TestDefinition test in Tests)
            {
                test.Result.Clear();
            }
        }
    }

    public class TestRunner
    {
        private List<TestFixtureDefinition> _testFixtures = new List<TestFixtureDefinition>();

        public List<TestFixtureDefinition> TestFixtures
        {
            get { return _testFixtures; }
        }

        public TestRunner()
        {
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type testFixtureType in asm.GetTypes().Where((t) => t.IsDefined(typeof(uTestFixtureAttribute), true) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null))
                {
                    _testFixtures.Add(new TestFixtureDefinition(testFixtureType));
                }
            }
        }

        public void Run()
        {
            foreach (TestFixtureDefinition testFixture in _testFixtures)
            {
                testFixture.Run();
            }
        }
    }
}