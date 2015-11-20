using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using File = Google.Apis.Drive.v2.Data.File;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            string[] scopes = new string[] { DriveService.Scope.Drive };
            InitializeComponent();
            var keyFilePath = @"c:\file.p12";    // Downloaded from https://console.developers.google.com
            var serviceAccountEmail = "85836446253-kt9eavc7jo8sc93gi30b5jh1vg3htgfu@developer.gserviceaccount.com";  // found https://console.developers.google.com

            //loading the Key file
            var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scopes
            }.FromCertificate(certificate));
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Drive API Sample",
            });
            FilesResource.ListRequest request = service.Files.List();
            FileList files = request.Execute();
            foreach (var file in files.Items)
            {
                Console.WriteLine(file.Title);
            }
        }


        /*
            var clientId = "85836446253-popehkln526pi2q3e8knugina12m795n.apps.googleusercontent.com";
                // From https://console.developers.google.com
            var clientSecret = "Tg-6QhgysVf9i-GYhEEszUau"; // From https://console.developers.google.com


            InitializeComponent();
            SpreadsheetsService service = AuthenticateOauth(clientId, clientSecret, Environment.UserName);

            //Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
            SpreadsheetQuery query = new SpreadsheetQuery("https://drive.google.com/");
            //Make a request to the API and get all spreadsheets.
            SpreadsheetFeed feed = service.Query(query);
            if (feed.Entries != null) { Console.WriteLine("feed = 0"); }


            //Iterate through all of the spreadsheets returned
            foreach (SpreadsheetEntry entry in feed.Entries)
            {
                // Print the title of this spreadsheet to the screen
                Console.WriteLine(entry.Title.Text);
            }
        }

        /// <summary>
            /// Authenticate to Google Using Oauth2
            /// Documentation https://developers.google.com/accounts/docs/OAuth2
            /// </summary>
            /// <param name="clientId">From Google Developer console https://console.developers.google.com</param>
            /// <param name="clientSecret">From Google Developer console https://console.developers.google.com</param>
            /// <param name="userName">A string used to identify a user.</param>
            /// <returns></returns>
        public static SpreadsheetsService AuthenticateOauth(string clientId, string clientSecret, string userName)
        {

            //Google Drive scopes Documentation:   https://developers.google.com/drive/web/scopes
            string[] scopes = new string[] { DriveService.Scope.Drive,  // view and manage your files and documents
                                             "https://spreadsheets.google.com/feeds",
                                              "https://docs.google.com/feeds"
        };  


            try
            {
                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
                UserCredential credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                                                                                             , scopes
                                                                                             , userName
                                                                                             , CancellationToken.None
                                                                                             , new FileDataStore("Welcome.Dinner.Helper.Auth.Store")).Result;

                SpreadsheetsService service = new SpreadsheetsService("Testname");
                /*(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Daimto Drive API Sample",
                });
                service.RequestFactory =new GOAuth2RequestFactory(null, "Testname", credential); 
                Console.WriteLine(service.ToString());
                return service;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.InnerException);
                return null;

            }

        }

        




        /*

        string[] scopes = new string[] { "https://spreadsheets.google.com/feeds", "https://docs.google.com/feeds",
        "https://www.googleapis.com/auth/drive", "https://docs.googleusercontent.com/"};
        var clientId = "85836446253-popehkln526pi2q3e8knugina12m795n.apps.googleusercontent.com";      // From https://console.developers.google.com
        var clientSecret = "Tg-6QhgysVf9i-GYhEEszUau";          // From https://console.developers.google.com
                                                                // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%

        UserCredential credential;
        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
        {
            ClientId = clientId,
            ClientSecret = clientSecret
        },
                                                                   scopes,
                                                                   Environment.UserName,
                                                                    CancellationToken.None,
                                                                    new FileDataStore("GoogleDrive.Auth.Store")).Result;
        OAuth2Parameters parameters = new OAuth2Parameters()
        {
            RefreshToken = credential.Token.RefreshToken,
            AccessToken = credential.Token.AccessToken,
            ClientId = clientId,
            ClientSecret = clientSecret,
            Scope = "https://www.googleapis.com/auth/drive https://spreadsheets.google.com/feeds",
            AccessType = "offline",
            TokenType = "refresh",
        };
        string authUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);



        ////////////////////////////////////////////////////////////////////////////
        // STEP 5: Make an OAuth authorized request to Google
        ////////////////////////////////////////////////////////////////////////////


        // Initialize the variables needed to make the request
        GOAuth2RequestFactory requestFactory = new GOAuth2RequestFactory(null, "WelcomeDinnerHelper", parameters);
        SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
        // service.Credentials.ClientToken = credential.Token.AccessToken;
        //service.Credentials.AccountType = 
        //service.SetAuthenticationToken(credential.Token.RefreshToken);




        //Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
        SpreadsheetQuery query = new SpreadsheetQuery("https://drive.google.com/folderview?id=0B1bCZuk1TqIdfjBPZkFsYXQ5a19aUHFaTVZLVUljMHRaeTlCWnZYeWFVRy1YTUVrU3Jack0&usp=sharing/private/full");
        //Make a request to the API and get all spreadsheets.
       SpreadsheetFeed feed = service.Query(query);
        if (feed.Entries != null ) {Console.WriteLine("feed = 0");}


         //Iterate through all of the spreadsheets returned
        foreach (SpreadsheetEntry entry in feed.Entries)
        {
            // Print the title of this spreadsheet to the screen
           Console.WriteLine(entry.Title.Text);
        }
    }





    /* ////////////////////////////////////////////////////////////////////////////
        // STEP 1: Configure how to perform OAuth 2.0
        ////////////////////////////////////////////////////////////////////////////


        string CLIENT_ID = "85836446253-popehkln526pi2q3e8knugina12m795n.apps.googleusercontent.com";

        // This is the OAuth 2.0 Client Secret retrieved
        // above.  Be sure to store this value securely.  Leaking this
        // value would enable others to act on behalf of your application!
        string CLIENT_SECRET = "Tg-6QhgysVf9i-GYhEEszUau";

        // Space separated list of scopes for which to request access.
        string SCOPE = "https://spreadsheets.google.com/feeds https://docs.google.com/feeds";

        // This is the Redirect URI for installed applications.
        // If you are building a web application, you have to set your
        // Redirect URI at https://code.google.com/apis/console.
        string REDIRECT_URI = "urn:ietf:wg:oauth:2.0:oob";

        ////////////////////////////////////////////////////////////////////////////
        // STEP 2: Set up the OAuth 2.0 object
        ////////////////////////////////////////////////////////////////////////////

        // OAuth2Parameters holds all the _parameters related to OAuth 2.0.
        OAuth2Parameters parameters = new OAuth2Parameters();

        // Set your OAuth 2.0 Client Id (which you can register at
        // https://code.google.com/apis/console).
        parameters.ClientId = CLIENT_ID;

        // Set your OAuth 2.0 Client Secret, which can be obtained at
        // https://code.google.com/apis/console.
        parameters.ClientSecret = CLIENT_SECRET;

        // Set your Redirect URI, which can be registered at
        // https://code.google.com/apis/console
        parameters.RedirectUri = REDIRECT_URI;



        ////////////////////////////////////////////////////////////////////////////
        // STEP 3: Get the Authorization URL
        ////////////////////////////////////////////////////////////////////////////

        // Set the scope for this particular service.
        parameters.Scope = SCOPE;

        // Get the authorization url.  The user of your application must visit
        // this url in order to authorize with Google.  If you are building a
        // browser-based application, you can redirect the user to the authorization
        // url.
        string authorizationUrl = OAuthUtil.CreateOAuth2AuthorizationUrl(parameters);


        parameters.AccessCode = Console.ReadLine();



        ////////////////////////////////////////////////////////////////////////////
        // STEP 4: Get the Access Token
        ////////////////////////////////////////////////////////////////////////////

        // Once the user authorizes with Google, the request token can be exchanged
        // for a long-lived access token.  If you are building a browser-based
        // application, you should parse the incoming request token from the url and
        // set it in OAuthParameters before calling GetAccessToken().
        //if (parameters!= null)
        //{
        OAuthUtil.GetAccessToken(parameters);
        string accessToken = parameters.AccessToken;
        //    Console.WriteLine("OAuth Access Token: " + accessToken);
        //}


        ////////////////////////////////////////////////////////////////////////////
        // STEP 5: Make an OAuth authorized request to Google
        ////////////////////////////////////////////////////////////////////////////

        // Initialize the variables needed to make the request
        GOAuth2RequestFactory requestFactory =
            new GOAuth2RequestFactory(null, "MySpreadsheetIntegration-v1", parameters);
        SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
        service.RequestFactory = requestFactory;


        // Instantiate a SpreadsheetQuery object to retrieve spreadsheets.
        SpreadsheetQuery query = new SpreadsheetQuery();

        // Make a request to the API and get all spreadsheets.
        SpreadsheetFeed feed = service.Query(query);

        // Iterate through all of the spreadsheets returned
        foreach (SpreadsheetEntry entry in feed.Entries)
        {
            // Print the title of this spreadsheet to the screen
            Console.WriteLine(entry.Title.Text);
        }

/*

        //string[] scopes = new string[] { "https://spreadsheets.google.com/feeds", "https://docs.google.com/feeds" };
        //var clientId = "85836446253-popehkln526pi2q3e8knugina12m795n.apps.googleusercontent.com";      // From https://console.developers.google.com
        //var clientSecret = "Tg-6QhgysVf9i-GYhEEszUau";          // From https://console.developers.google.com
        //                                                        // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
        //FileDataStore StorageInfo = new FileDataStore("GoogleDrive.Auth.Store");
        //var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
        //{
        //    ClientId = clientId,
        //    ClientSecret = clientSecret
        //},
        //                                                            scopes,
        //                                                            Environment.UserName,
        //                                                            CancellationToken.None,
        //                                                            StorageInfo).Result;
        //SpreadsheetsService service = new SpreadsheetsService("MySpreadsheetIntegration-v1");
    }
    */



    }
}


