using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ForsakenNet.Tasks
{
    class HTTPClientTask
    {

        static HttpClientHandler _ClientHandler = new HttpClientHandler();
        //setting a Stopwatch to see how long it takes.
        public Util.TimerUtil taskTimer;

        public HTTPClientTask()
        {
            //Declaring here so it can run Multiple instances from where it was laoded.
            taskTimer = new Util.TimerUtil();
        }


        public async Task<string> getClient(string URL)
        {
            taskTimer.Reset();
            taskTimer.Start();
            using (var httpClient = new HttpClient(_ClientHandler, false))
            {
                //Sets a Timeout if it takes to long
                httpClient.Timeout = TimeSpan.FromSeconds(15);
                using(var response  = await httpClient.GetAsync(URL))
                {
                    //If not accepted log bad Result
                    if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                    {
                        await Logging.Log.WriteLog("Bad Request", Logging.LogType.Error);
                        return null;
                    }
                    //Read the response as string async so it waits for full response
                    string webData = await response.Content.ReadAsStringAsync();
                    if(String.IsNullOrEmpty(webData))
                    {
                        //No data recieved so need to log it so no NullPointer
                        await Logging.Log.WriteLog("Data is Null or Emtpy", Logging.LogType.Error);
                        return null;
                    } else
                    {

                        Stream webStream = await response.Content.ReadAsStreamAsync();
                        StreamReader readStream = null;

                        readStream = new StreamReader(webStream);
                        await Logging.Log.WriteLog($"HTTPClient Done in: {taskTimer.getMS()}MS", Logging.LogType.Log);
                        return readStream.ReadToEnd();
                    }
                }
                
            }
        }


    }
}
