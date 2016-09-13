using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Drive.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GoogleDriveSample
{
    //2016.09.13
    //Google Driveを操作するクラス
    //何度も認証する必要はないのでsingletonクラスで作成
    public class GoogleApi
    {
        private static string[] SCOPES = { DriveService.Scope.DriveReadonly };
        private static string APPLICATION_NAME = "GoogleDriveSample";

        private static GoogleApi mInstance;

        private DriveService mService;

        public static GoogleApi GetInstance
        {
            get
            {
                if(mInstance == null)
                {
                    mInstance = new GoogleApi();
                }
                return mInstance;
            }
        }

        private GoogleApi()
        {
            UserCredential credential;

            using (var stream = new FileStream("client_secret_sp2.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/google-drive-sample-sp2.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    SCOPES,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Debug.WriteLine(credPath, "Credential file saved to");
            }

            //Drive API serviceの作成
            mService = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME,
            });
        }

        /// <summary>
        /// ファイルのリストを取得
        /// </summary>
        /// <returns></returns>
        public IList<Google.Apis.Drive.v3.Data.File> GetFiles()
        {
            //リクエストパラメータの定義
            FilesResource.ListRequest listRequest = mService.Files.List();
            //ファイル取得数
            listRequest.PageSize = 30;
            listRequest.Fields = "nextPageToken, files(id, name)";

            //ファイルのリスト
            return listRequest.Execute().Files;
        }
    }
}
