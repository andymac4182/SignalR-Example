using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRExample.Web.Hubs
{
    using SignalRExample.Shared;

    public class ProcessingResultHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }

        public bool FinishedProcessing(ProcessedData processedData)
        {
            try
            {
                var userHub = GlobalHost.ConnectionManager.GetHubContext<UserTracking>();

                userHub.Clients.Client(processedData.ConnectionId).FinishedProcessing(processedData.Result);
            }
            catch (Exception)
            {
                return false;
                throw;
            }

            return true;
        }
    }
}