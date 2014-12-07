using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRExample.Web.Hubs
{
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Web.Configuration;
    using System.Web.Security;

    using Microsoft.AspNet.Identity;

    /// <summary>
    /// The user tracking.
    /// </summary>
    public class UserTracking : Hub
    {
        /// <summary>
        /// The sql connection string.
        /// </summary>
        private string SqlConnectionString = "Server=.;Database=SignalRExampleDB;Trusted_Connection=True;";

        public override Task OnConnected()
        {
            // TODO: Explain code

            var user = Context.User;
            var cookies = Context.RequestCookies;

            var sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");

            string cookieName = sessionStateSection.CookieName;

            // Overriding Cookie Name for new MVC Template
            cookieName = ".AspNet.ApplicationCookie";

            if (!user.Identity.IsAuthenticated || !cookies.ContainsKey(cookieName))
            {
                return base.OnConnected();
            }

            var sqlConnection = new SqlConnection(this.SqlConnectionString);
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "ap_SessionConnect";

            sqlCommand.Parameters.AddWithValue("@SessionIdentifier", cookies[cookieName].Value);

            sqlCommand.Parameters.AddWithValue("@ConnectionId", this.Context.ConnectionId);
            sqlCommand.Parameters.AddWithValue("@ConnectionServer", Environment.MachineName);
            sqlCommand.Parameters.AddWithValue("@CustomerId", user.Identity.GetUserName());

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            // TODO: Explain code

            var user = Context.User;
            var cookies = Context.RequestCookies;
            var sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");

            string cookieName = sessionStateSection.CookieName;

            // Overriding Cookie Name for new MVC Template
            cookieName = ".AspNet.ApplicationCookie";

            if (!user.Identity.IsAuthenticated || !cookies.ContainsKey(cookieName))
            {
                return base.OnConnected();
            }

            var sqlConnection = new SqlConnection(this.SqlConnectionString);
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "ap_SessionDisconnect";

            sqlCommand.Parameters.AddWithValue("@SessionIdentifier", cookies[cookieName].Value);
            sqlCommand.Parameters.AddWithValue("@ConnectionId", this.Context.ConnectionId);
            sqlCommand.Parameters.AddWithValue("@ConnectionServer", Environment.MachineName);
            sqlCommand.Parameters.AddWithValue("@CustomerId", user.Identity.GetUserName());

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            // TODO: Explain code

            var user = Context.User;
            var cookies = Context.RequestCookies;

            var sessionStateSection = (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");

            string cookieName = sessionStateSection.CookieName;

            // Overriding Cookie Name for new MVC Template
            cookieName = ".AspNet.ApplicationCookie";

            if (!user.Identity.IsAuthenticated || !cookies.ContainsKey(cookieName))
            {
                return base.OnConnected();
            }

            var sqlConnection = new SqlConnection(this.SqlConnectionString);
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "ap_SessionConnect";

            sqlCommand.Parameters.AddWithValue("@SessionIdentifier", cookies[cookieName].Value);

            sqlCommand.Parameters.AddWithValue("@ConnectionId", this.Context.ConnectionId);
            sqlCommand.Parameters.AddWithValue("@ConnectionServer", Environment.MachineName);
            sqlCommand.Parameters.AddWithValue("@CustomerId", user.Identity.GetUserName());

            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();

            return base.OnReconnected();
        }

        public void Hello()
        {
            string name;
            var user = Context.User;

            if (user != null && user.Identity.IsAuthenticated)
            {
                name = user.Identity.Name;
            }
            else
            {
                name = "anonymous";
            }

            Clients.All.hello("Welcome " + name);
        }
    }
}