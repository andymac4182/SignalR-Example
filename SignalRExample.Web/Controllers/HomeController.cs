namespace SignalRExample.Web.Controllers
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        /// <summary>
        /// The sql connection string.
        /// </summary>
        private string SqlConnectionString = "Server=.;Database=SignalRExampleDB;Trusted_Connection=True;";

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            var sqlConnection = new SqlConnection(this.SqlConnectionString);
            var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = "ap_AddDataToBeProcessed";
            var cookieName = ".AspNet.ApplicationCookie";
            sqlCommand.Parameters.AddWithValue("@SessionIdentifier", Request.Cookies[cookieName].Value);
        
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
            sqlConnection.Close();


            return View();
        }
    }
}