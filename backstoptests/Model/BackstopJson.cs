{
public CompareConfig compareConfig { get; set; }
}
public class CompareConfig
{
public TestPair[] testPairs { get; set; }
}
public class TestPair
{
public string reference { get; set; }
public string test { get; set; }
public string selector { get; set; }
public string fileName { get; set; }
public string label { get; set; }
public bool requireSameDimensions { get; set; }
public double misMatchThreshold { get; set; }
public string url { get; set; }
public string referenceUrl { get; set; }
}