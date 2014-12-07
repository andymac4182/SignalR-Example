namespace SignalRExample.OfflineProcessor
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Net;
    using System.Reflection;
    using System.Threading;
    using System.Timers;

    using Microsoft.AspNet.SignalR.Client;

    using SignalRExample.Shared;

    using ConnectionState = Microsoft.AspNet.SignalR.Client.ConnectionState;
    using Timer = System.Timers.Timer;

    class Program
    {
        /// <summary>
        /// TThis timer is used to process data items.
        /// </summary>
        private static readonly Timer ProcessDataTimer = new Timer();

        /// <summary>
        /// TThis timer is used to process data items.
        /// </summary>
        private static readonly Timer NotifyTimer = new Timer();

        /// <summary>
        /// The sql connection string.
        /// </summary>
        private const string SqlConnectionString = "Server=.;Database=SignalRExampleDB;Trusted_Connection=True;";

        private static Random rnd = new Random();

        /// <summary>
        /// The signal r servers.
        /// </summary>
        private static Dictionary<string, HubConnection> signalRServers = new Dictionary<string, HubConnection>();

        /// <summary>
        /// The signal r hub proxies.
        /// </summary>
        private static Dictionary<string, IHubProxy> signalRHubProxies = new Dictionary<string, IHubProxy>(); 

        static void Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = 10;
            
            
            ProcessDataTimer.Enabled = true;
            ProcessDataTimer.Elapsed += ProcessDataTimerOnElapsed;
            ProcessDataTimer.Interval = 10000;
            ProcessDataTimer.Start();


            NotifyTimer.Enabled = true;
            NotifyTimer.Elapsed += NotifyTimer_Elapsed;
            NotifyTimer.Interval = 5000;
            NotifyTimer.Start();

            Console.ReadLine();


            
        }

        static void NotifyTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var sqlConnection = new SqlConnection(SqlConnectionString);
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "ap_GetDataToNotify";

            sqlConnection.Open();
            var dataTable = new DataTable();
            var data = new SqlDataAdapter(sqlCommand);

            data.Fill(dataTable);
            sqlConnection.Close();

            foreach (DataRow row in dataTable.Rows)
            {
                if (int.Parse(row["ConnectionState"].ToString()) != 1)
                {
                    continue;
                }

                var signalRServerName = row["ConnectionServer"].ToString();

                // For testing
                if (Environment.MachineName == signalRServerName)
                {
                    signalRServerName = "localhost";
                }
                

                if (!signalRServers.ContainsKey(signalRServerName))
                {
                    var hubConnection = new HubConnection("http://" + signalRServerName + ":58381/");
                    signalRServers.Add(signalRServerName, hubConnection);

                    signalRHubProxies.Add(signalRServerName, signalRServers[signalRServerName].CreateHubProxy("ProcessingResultHub"));
                    
                    signalRServers[signalRServerName].Start().Wait();
                }

                var notifyResult = false;

                try
                {
                    if (signalRServers[signalRServerName].State != ConnectionState.Connected)
                    {
                        signalRServers[signalRServerName].Start().Wait();
                    }

                    notifyResult =
                    signalRHubProxies[signalRServerName].Invoke<bool>(
                        "FinishedProcessing",
                        new ProcessedData
                        {
                            ConnectionId = row["ConnectionId"].ToString(),
                            Result = int.Parse(row["Result"].ToString())
                        }).Result;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error talking to signalR");
                }
                

                if (!notifyResult)
                {
                    continue;
                }

                var processedSqlConnection = new SqlConnection(SqlConnectionString);
                var processedSqlCommand = processedSqlConnection.CreateCommand();
                processedSqlCommand.CommandType = CommandType.StoredProcedure;
                processedSqlCommand.CommandText = "ap_ClientNotified";

                processedSqlCommand.Parameters.AddWithValue("@DataId", int.Parse(row["DataId"].ToString()));

                processedSqlConnection.Open();
                processedSqlCommand.ExecuteNonQuery();
                processedSqlConnection.Close();
            }
        }

        private static void ProcessDataTimerOnElapsed(object sender, ElapsedEventArgs elapsedEventArgs)
        {
            ProcessDataTimer.Stop();

            var sqlConnection = new SqlConnection(SqlConnectionString);
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "ap_GetDataToProcess";

            sqlConnection.Open();
            var dataTable = new DataTable();
            var data = new SqlDataAdapter(sqlCommand);

            data.Fill(dataTable);
            sqlConnection.Close();

            foreach (DataRow row in dataTable.Rows)
            {
                var dataId = int.Parse(row["Id"].ToString());
                int result = 0;
                if (rnd.Next(0, 100) > 25)
                {
                    result = 1;
                }

                var processedSqlConnection = new SqlConnection(SqlConnectionString);
                var processedSqlCommand = processedSqlConnection.CreateCommand();
                processedSqlCommand.CommandType = CommandType.StoredProcedure;
                processedSqlCommand.CommandText = "ap_ProcessData";

                processedSqlCommand.Parameters.AddWithValue("@DataId", dataId);
                processedSqlCommand.Parameters.AddWithValue("@Result", result);

                processedSqlConnection.Open();
                
                var processedDataTable = new DataTable();
                var processedData = new SqlDataAdapter(processedSqlCommand);

                processedData.Fill(processedDataTable);

                processedSqlConnection.Close();
                
            }


            ProcessDataTimer.Start();
        }
    }

}
