using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace MockedCameraDemos
{
    public class DemoTests
    {
        private const string DemoUrl = "https://qa-code.com/webcam-demo/";
        private const string EdgeBrowserExePath = @"C:\Program Files (x86)\Microsoft\Edge\Application\msedge.exe";
        private const string SafariDriverPath = "/usr/bin/";
        private const string SafariVersion = "13.1.1";
        private const string MacPlatform = "macOS 10.15";
        private readonly string _buildPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private IWebDriver _driver;
        private FirefoxDriverService _firefoxService;
        private EdgeDriverService _edgeService;

        [Test]
        public void Chrome()
        {
            var options = new ChromeOptions();
            options.AddArgument("ignore-certificate-errors");
            options.AddArgument("use-fake-ui-for-media-stream");
            options.AddArgument("use-fake-device-for-media-stream");
            _driver = new ChromeDriver(_buildPath, options);
            
            RunTest();
        }

        [Test]
        public void Firefox()
        {
            _firefoxService = FirefoxDriverService.CreateDefaultService(_buildPath);
            _firefoxService.Host = "::1";

            var options = new FirefoxOptions() { AcceptInsecureCertificates = true };
            options.SetPreference("media.navigator.streams.fake", true);
            options.SetPreference("media.navigator.permission.disabled", true);
            _driver = new FirefoxDriver(_firefoxService, options);

            RunTest();
        }

        [Test]
        public void EdgeChromium()
        {
            _edgeService = EdgeDriverService.CreateDefaultService(_buildPath, @"msedgedriver.exe");
            _edgeService.UseVerboseLogging = true;
            _edgeService.UseSpecCompliantProtocol = true;
            _edgeService.Start();

            var argsList = new List<string> { "use-fake-ui-for-media-stream", "use-fake-device-for-media-stream", "log-level=1" };

            #pragma warning disable 618
            var capabilities = new DesiredCapabilities(new Dictionary<string, object>()
            #pragma warning restore 618
            {
                { "ms:edgeOptions", new Dictionary<string, object>() {
                    {  "binary", EdgeBrowserExePath },
                    {  "args", argsList }
                }}
            });

            _driver = new RemoteWebDriver(_edgeService.ServiceUrl, capabilities);
            
            RunTest();
        }

        [Test]
        public void Safari()
        {

            var options = new SafariOptions()
            {
                PlatformName = MacPlatform,
                BrowserVersion = SafariVersion,
                UnhandledPromptBehavior = UnhandledPromptBehavior.Accept
            };

            _driver = new SafariDriver(SafariDriverPath, options);

            RunTest();
        }

        private void RunTest()
        {
            _driver.Manage().Window.Maximize();
            _driver.Navigate().GoToUrl(DemoUrl);
            _driver.FindElement(DemoPage.StartButton).Click();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            DemoHelpers.VerifyVideoIsBeingDisplayed(_driver, DemoPage.Video);
        }

        [TearDown]
        public void TearDown()
        {
            _driver?.Quit();
            _driver?.Dispose();
            _edgeService?.Dispose();
            _firefoxService?.Dispose();
        }
    }
}