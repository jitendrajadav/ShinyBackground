using System;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace KegID.UITest
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        static readonly Func<AppQuery, AppQuery> InitialMessage = c => c.Marked("MyLabel").Text("Hello, Xamarin.Forms!");
        static readonly Func<AppQuery, AppQuery> Button = c => c.Marked("loginButton");
        static readonly Func<AppQuery, AppQuery> DoneMessage = c => c.Marked("loginButton").Text("Login");

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }

        [Test]
        public void WelcomeTextIsDisplayed()
        {
            AppResult[] results = app.WaitForElement(c => c.Marked("Welcome to Xamarin.Forms!"));
            app.Screenshot("Welcome screen.");

            Assert.IsTrue(results.Any());
        }

        [Test]
        public void AppLaunches()
        {
            app.Repl();
            // Arrange - Nothing to do because the queries have already been initialized.
            AppResult[] result = app.Query(InitialMessage);
            Assert.IsTrue(result.Any(), "The initial message string isn't correct - maybe the app wasn't re-started?");

            // Act
            app.Tap(Button);

            // Assert
            result = app.Query(DoneMessage);
            Assert.IsTrue(result.Any(), "The 'clicked' message is not being displayed.");
        }
    }

}
