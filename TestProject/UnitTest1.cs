using NUnit.Framework;
using System;
using System.IO;
using System.Reflection;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Drawing;
namespace TestProject
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        private static object locker = new object();
        [Test]
        public void TestMethod()
        {
            ChromeDriverService chservice = ChromeDriverService.CreateDefaultService(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            var newDriver = new ChromeDriver(chservice, new ChromeOptions(), TimeSpan.FromSeconds(180));
            int width = 375;
            int height = 667; //Or set this to 1000 to capture a long screen
            newDriver.Manage().Window.Size = new Size(width, height); //We can either open browser to a viewport size or maximum
            try
            {
                newDriver.Navigate().GoToUrl("https://www.google.com/");
                Thread.Sleep(1000);
                //Take Screenshot and save to bitmaps_test/currentdate/
                //Backstop requires a folder inside bitmaps_test for test image, other wise the report will fail.
                //Backstop requires a folder inside bitmaps_test for test image, other wise the report will fail.
                string fileName = "HomePage_Mobile.png";
                string testFilePath = "C:\\Users\\ranjeet.rattanpal\\source\\repos\\TestProject\\backstop_data\\bitmaps_test\\20180525\\";
                string refFilePath = "C:\\Users\\ranjeet.rattanpal\\source\\repos\\TestProject\\backstop_data\\bitmaps_reference";
                var screenshot = ((ITakesScreenshot)newDriver).GetScreenshot();
                screenshot.SaveAsFile(Path.Combine(testFilePath, fileName), ScreenshotImageFormat.Png);
                BackstopDetails bsDet = new BackstopDetails();
                bsDet.ReferenceFile = Path.Combine(refFilePath, fileName);
                bsDet.TestFile = Path.Combine(testFilePath, fileName);
                bsDet.FileName = fileName;
                bsDet.Label = "Home Page";
                bsDet.RequireSameDimensions = true;
                bsDet.MisMatchThreshold = 0.1;
                bsDet.Url = newDriver.Url;
                CreateJsonConfig(bsDet); //This creates a new config file BSCompareConfig in backstop_data folder
            }
            finally
            {
                newDriver.Close();
                newDriver.Quit();
            }
        }
        private static void CreateJsonConfig(BackstopDetails bsDet)
        {
            var testPair = new TestPair();
            testPair.reference = bsDet.ReferenceFile;
            testPair.test = bsDet.TestFile;
            testPair.selector = "document";
            testPair.fileName = bsDet.FileName;
            testPair.label = bsDet.Label;
            testPair.requireSameDimensions = bsDet.RequireSameDimensions;
            testPair.misMatchThreshold = bsDet.MisMatchThreshold;
            testPair.url = bsDet.Url;
            testPair.referenceUrl = string.Empty;
            string compareConfigFile = "C:\\Users\\ranjeet.rattanpal\\source\\repos\\TestProject\\backstop_data\\BSCompareConfig.json";
            BackstopJson root = null;
            //when running a test Suite in parallel, multiple tests will be accessing the same file. So
            //using locker and FileStream to lock the file
            //If the file exists, it adds a new test pair,and if nothing exists, it creates a new file
            lock (locker)
            {
                string data = null;
                using (FileStream fileStart = new FileStream(compareConfigFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    StreamReader reader = new StreamReader(fileStart);
                    data = reader.ReadLine();
                    reader.Dispose();
                    if (String.IsNullOrEmpty(data))
                    {
                        root = new BackstopJson();
                        //CompareConfig
                        var compareConfig = new CompareConfig();
                        var testPairs = new TestPair[1];
                        testPairs[0] = testPair;
                        compareConfig.testPairs = testPairs;
                        root.compareConfig = compareConfig;
                    }
                    else
                    {
                        JToken jsonObj = JObject.Parse(data);
                        root = JsonConvert.DeserializeObject<BackstopJson>(jsonObj.ToString());
                        var compareConfig = root.compareConfig;
                        var existingTestPairs = compareConfig.testPairs;
                        var newTestPairs = new List<TestPair>();
                        newTestPairs.AddRange(existingTestPairs);
                        newTestPairs.Add(testPair);
                        compareConfig.testPairs = (TestPair[])newTestPairs.ToArray();
                        root.compareConfig = compareConfig;
                    }
                }
                using (FileStream file = new FileStream(compareConfigFile, FileMode.Truncate, FileAccess.ReadWrite, FileShare.None))
                {
                    string json = JsonConvert.SerializeObject(root);
                    StreamWriter writer = new StreamWriter(file);
                    writer.Write(json);
                    writer.Flush();
                }
            }
            Console.WriteLine($"[file:///{compareConfigFile} ]");
        }
    }
}
    