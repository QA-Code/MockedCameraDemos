using System;
using System.Threading;
using FluentAssertions;
using OpenQA.Selenium;

namespace MockedCameraDemos
{
    public static class DemoHelpers
    {
        public static void VerifyVideoIsBeingDisplayed(IWebDriver driver, By element)
        {
            const int maxRetries = 5;
            var playing = false;

            for (var i = 1; i <= maxRetries; i++)
            {
                var currentTime = Convert.ToDouble(driver.FindElement(element).GetAttribute("currentTime"));
                if (currentTime > 0)
                {
                    playing = true;
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            playing.Should().BeTrue("video is playing");
        }

    }
}
