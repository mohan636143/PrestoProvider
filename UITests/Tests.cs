using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.UITest.Android;
using System.Collections.Generic;

namespace Provider.UITests
{
    [TestFixture(Platform.Android)]
    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
            //app = ConfigureApp.iOS.StartApp();
        }

        [Test]
        public async void LoginWithFaceBook()
        {
            try
            {
                //app.Screenshot("Login Page");
                bool passwordHandled = false;
                app.Tap(btn => btn.Marked("Am_LoginPage_LoginButton"));
                AppResult[] webView = app.WaitForElement(web => web.Marked("Am_FbLogin_WebView"));

                await Task.Delay(5000);
                //AppResult[] allElements = app.Query("WebView css:'*' {textContent CONTAINS 'Log In'}");
                //allElements = app.Query("webView xpath:'//body'");
                //allElements = app.Query("webView xpath:'//div[@id=\"element_id\"]'");
                //allElements = app.Query("webView xpath:'//span/a[contains(@class,\"element_class\")]'");
                //allElements = app.Query("WebView css:'*' {textContent CONTAINS 'Log In'}");
                //allElements = app.Query(web => web.WebView(0).Class("Button").All());
                //var tmp = app.Query((web => web.WebView()));
                //var tmp = app.Query((web => web.WebView().InvokeJS("document.getElementById('m_login_email')")));
                //allElements = app.Query((web => web.WebView().Class("input")));
                AppWebResult[] res = app.Query((web => web.WebView().Css("input")));
                int coount = 0;
                while (res != null && res.Count() < 1)
                {
                    res = app.Query((web => web.WebView().Css("input")));
                    coount++;
                }
                app.Tap(c => c.Css("input#" + res[0].Id));
                app.EnterText("mohan636143");
                if (res.Count() == 2)//In case there is only login textbox present.
                {
                    passwordHandled = true;
                    app.Tap(c => c.Css("input#" + res[1].Id));
                    app.EnterText("AP31BP4914$");
                }
                res = app.Query((web => web.WebView().Css("button")));
                app.Tap(c => c.Css("button#" + res[0].Id));
                await Task.Delay(5000);
                res = null;
                while (res != null && res.Count() < 1)
                {
                    res = app.Query((web => web.WebView().Css("button")));

                }
                if (!passwordHandled)
                {
                    //Checking if we have any textboxes. In case of password not given in earlier
                    res = app.Query((web => web.WebView().Css("input")));//Checking for text box.
                    if (res.Count() == 1)
                    {
                        app.Tap(c => c.Css("input#" + res[1].Id));
                        app.EnterText("AP31BP4914$");
                    }
                    res = app.Query((web => web.WebView().Css("button")));
                    app.Tap(c => c.Css("button#" + res[0].Id));//This completes entering id and pass.
                    await Task.Delay(5000);
                }

                //Next accept the permissions
                res = app.Query((web => web.WebView().Css("button")));
                AppWebResult continueBtn = res.FirstOrDefault(btn => !string.IsNullOrEmpty(btn.Id));
                app.Tap(c => c.Css("button#" + continueBtn.Id));//This completes entering id and pass.


            }
            catch (Exception ex)
            {

            }
        }

        [Test]
        public void EnterDataAfterLogin()
        {
            try
            {
                LoginWithFaceBook();
                //Checking for if view is loaded.
                AppResult[] elements = app.WaitForElement("Am_UserSignUp_txtFirstName");
                if (elements != null && elements.Count() < 1)
                    elements = app.WaitForElement("Am_UserSignUp_txtFirstName");
                //Tap the FirstName and Enter Text
                app.Tap(entry => entry.Marked("Am_UserSignUp_txtFirstName"));
                app.ClearText(entry => entry.Marked("Am_UserSignUp_txtFirstName"));
                app.EnterText(entry => entry.Marked("Am_UserSignUp_txtFirstName"), "First");

                //Tap the FirstName and Enter Text
                app.Tap(entry => entry.Marked("Am_UserSignUp_txtLastName"));
                app.ClearText(entry => entry.Marked("Am_UserSignUp_txtLastName"));
                app.EnterText(entry => entry.Marked("Am_UserSignUp_txtLastName"), "Last");

                //Tap the FirstName and Enter Text
                app.Tap(entry => entry.Marked("Am_UserSignUp_txtEmail"));
                app.ClearText(entry => entry.Marked("Am_UserSignUp_txtEmail"));
                app.EnterText(entry => entry.Marked("Am_UserSignUp_txtEmail"), "test@xyz.com");

            }
            catch (Exception ex)
            {

            }
        }

        [Test]
        public void TestListView()
        {
            try
            {
                // Func<AppQuery, AppQuery> mayank = c => c.Marked("Am_TestPage_ListView");



                AppResult[] lstView = app.WaitForElement(c => c.Marked("Am_TestPage_ListView"));
                AppResult[] listViewItems = app.Query(lst => lst.Marked("Am_TestPage_ListView").Child());
                int count = listViewItems.Count();


                //Func<AppQuery, AppQuery> firstCellInList = null;
                for (int i = 0; i < count; i++)
                {
                    //  if(listViewItems[i)
                    app.Tap(c => c.Marked(listViewItems[i].Label));


                    ////if (platform == Platform.Android)
                    ////    firstCellInList = x => x.Class("ViewCellRenderer_ViewCellContainer").Index(0);
                    ////else if (platform == Platform.iOS)
                    ////firstCellInList = x => x.Marked("{AutomationId of ViewCell}").Index(0);
                    //if (platform == Platform.Android)
                    //{
                    //    //var tmp = app.Query()
                    //}
                    //else if (platform == Platform.iOS)
                    //{

                    //}
                }
                //if (platform == Platform.Android)
                //    firstCellInList = x => x.Class("WebView").;
                //else if (platform == Platform.iOS)
                //    firstCellInList = x => x.Marked("{AutomationId of ViewCell}").Index(0);

                //app.WaitForElement(firstCellInList);
                //app.Tap(firstCellInList);
            }
            catch (Exception ex)
            {

            }
        }


    }
}
