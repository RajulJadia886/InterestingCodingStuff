using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using myapplication.models;
using System.Data;
using System.Data.SqlClient;

namespace myapplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _configuration;
        public EmployeeController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public IActionResult Get()
        {
            string query = @"select * from employee order by orderid";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using(SqlConnection myConnection = new SqlConnection(sqlDataSource))
            {
                myConnection.Open();
                using(SqlCommand myCommand = new SqlCommand(query,myConnection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConnection.Close();
                }
            }
            return Json(table);
        }

        [HttpPut("update")]
        public IActionResult Put([FromBody] updateDto updateValues)
        {
            string query = @"update employee set orderid = '" + updateValues.secondOrderId + @"'where employeeId = " + updateValues.empId1 + @"";
            string query2 = @"update employee set orderid = '" + updateValues.orderId1 + @"'where employeeId = " + updateValues.empId2 + @"";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("Default");
            SqlDataReader myReader;
            using (SqlConnection myConnection = new SqlConnection(sqlDataSource))
            {
                myConnection.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConnection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                }
                using (SqlCommand myCommand = new SqlCommand(query2, myConnection))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConnection.Close();
                }
            }
            return Json(table);
        }
    }
}
