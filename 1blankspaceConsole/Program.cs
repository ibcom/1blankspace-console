using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

namespace _1blankspaceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string sLogonName = "";

            if (args.Length > 0)
            {
                sLogonName = args[0];
            }

            Console.WriteLine("Welcome to the 1blankspace Console.");

            if (sLogonName == "")
            {
                Console.WriteLine("");
                Console.WriteLine("Logon name:");
                sLogonName = Console.ReadLine();
            }

            if (sLogonName == "")
            {
                Console.WriteLine("No logon name entered.  Press any key to exit.");
                Console.Read();
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("Password:");
                string sPassword = Console.ReadLine();

                Console.WriteLine("");
                Console.WriteLine("Logging on...");
                
                WebRequest request = WebRequest.Create("https://secure.mydigitalspacelive.com/directory/ondemand/logon.asp");
                request.Method = "POST";

                string postData = "logon=" + sLogonName + "&password=" + sPassword;
                byte[] byteArray = Encoding.ASCII.GetBytes(postData);

                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;

                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);

                string sResponse = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                //Console.WriteLine(sResponse);

                string[] aResponse = sResponse.Split('|');

                if (aResponse[1] == "NO")
                {
                    Console.WriteLine("");
                    Console.WriteLine("Incorrect logon name or password, press any key to exit...");
                    Console.Read();
                }
                else
                {
                    string sSID = aResponse[2];

                    request = WebRequest.Create("https://secure.mydigitalspacelive.com/directory/ondemand/object.asp");
                    request.Method = "POST";

                    postData = "method=CORE_GET_USER_DETAILS&sid=" + sSID;
                    byteArray = Encoding.ASCII.GetBytes(postData);

                    request.ContentType = "application/x-www-form-urlencoded";
                    request.ContentLength = byteArray.Length;

                    dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    response = request.GetResponse();
                    dataStream = response.GetResponseStream();
                    reader = new StreamReader(dataStream);

                    sResponse = reader.ReadToEnd();

                    //Console.WriteLine(sResponse);

                    reader.Close();
                    dataStream.Close();
                    response.Close();

                    aResponse = sResponse.Split('|');

                    if (aResponse[0] == "OK")
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Hi " + aResponse[1] + ", a short but sweet time together.");
                        Console.WriteLine("");
                        Console.WriteLine("Press any key to part ways. I'll be thinking of you until next time...");
                        Console.Read();
                    }
                }
            }
        }
    }
}
