using System;

namespace uTest.Examples
{
    // The uTestFixture attribute indicates that this class is a test fixture.
    // The uTestRunner will look for any class with this attribute and search it for tests
    // that it can run
    [uTestFixture]
    public class ExampleFixture
    {
        // Any method with the attribute uTestFixtureSetup will
        // be run exactly once before any tests in the fixture are run.
        // The method cannot have any parameters.
        // Methods in base classes are called first.
        [uTestFixtureSetup]
        void Setup()
        {
        }
        
        // Any method with the attribute uTestFixtureTeardown will
        // be run exactly once after all tests in the fixture have completed.
        // The method cannot have any parameters.
        // Methods in base classes are called first.
        [uTestFixtureTeardown]
        void Teardown()
        {
        }

        // Methods marked with uTest are tests.
        // They are run each time the fixture is run.
        [uTest]
        void TestOne()
        {
        }
        
        // Tests with the uTestExpectedException attribute will fail
        // unless an exception of the given type is thrown.
        [uTest]
        [uTestExpectedException(typeof(InvalidOperationException))]
        void TestTwo()
        {
        }


        // Tests with the uSetup attribute will be invoked once before
        // each test is run.
        // If you have 10 tests, uSetup will be run 10 times.
        [uSetup]
        void TestSetup()
        {
        }

        // Tests with the uTearDown attribute will be invoked once after
        // each test is run.
        // If you have 10 tests, it will be run 10 times: once after each test.
        [uTearDown]
        void TestTeardown()
        {
        }
    }
}