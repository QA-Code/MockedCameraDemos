# Mocked Camera Demo Tests
Example tests to demonstrate the browser capabilities required to mock webcams for automated tests

## General ##
- You must have the browser installed locally in order to run these tests. 
- The NuGet webdriver packages must match the version of the browser you are running against. If the tests report driver version errors, update the associated NuGet package (ChromeDriver, FirefoxDriver etc...) in the test project and update your browser to the latest (non-beta) version.

## Edge Chromium ##
Note - Older (none-Chromium based) versions of Edge do not have capabilities to mock the camera. 

Edge Chromium requires 2 things for the tests to work:
1. The version of the msedgedriver.exe must match your local version of your browser. That can be downloaded here https://developer.microsoft.com/en-us/microsoft-edge/tools/webdriver/

2. The 'EdgeBrowserExePath' variable must match the location of your Edge exe

Webdriver tests on Edge require the EdgeDriverService to be running. 

## Firefox ##
The Firefox test uses a local Firefox driver service to speed up the test. Without this the test can run very slowly.

## Safari ##
Make sure you have enable remote automation in Safari https://developer.apple.com/documentation/webkit/testing_with_webdriver_in_safari

Change the SafariDriverPath, SafariVersion and MacPlatform variables to match your local setup.

