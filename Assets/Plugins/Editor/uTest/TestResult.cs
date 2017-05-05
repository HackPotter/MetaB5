
namespace uTest
{
    public enum TestStatus
    {
        Passed,
        Failed,
        Unknown,
    }

    public class TestResult
    {
        public TestStatus TestStatus
        {
            get;
            set;
        }

        public string ResultMessage
        {
            get;
            set;
        }

        public TestResult()
        {
            ResultMessage = "";
            TestStatus = TestStatus.Unknown;
        }

        public void Clear()
        {
            ResultMessage = "";
            TestStatus = TestStatus.Unknown;
        }
    }
}

