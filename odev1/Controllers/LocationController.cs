using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace odev1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
       NpgsqlConnection bağlanti = new NpgsqlConnection("server=localHost;port=5432,Database=dblocation; user Id=postgres; password=12345");


        [HttpPost]
        public void Add(Location location)
        {
            using (var conn = new NpgsqlConnection())
            {
                conn.Open();
                var cmd =new NpgsqlCommand("İNSERT İNTO locations (name,x,y) VALUES (@name,@x,@y)",conn);
                cmd.Parameters.AddWithValue("name", location.name);
                cmd.Parameters.AddWithValue("x", location.x);
                cmd.Parameters.AddWithValue("y", location.y);
                cmd.ExecuteNonQuery();

            }
        }
     
        [HttpGet]
        public List<Location> GetAll()
        {
            List<Location> locations = new List<Location>();
            using(var conn = new NpgsqlConnection())
            {
                conn.Open();
                var sql = "SELECT * FROM locations";
                var cmd = new NpgsqlCommand(sql,conn);
                using  (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Location location = new Location(); 
                        location.id = reader.GetInt32("id");
                        location.name = reader.GetString("name");
                        location.x = reader.GetDouble("x");
                        location.y = reader.GetDouble("y");
                        locations.Add(location);
                        
                    }
                }
                conn.Close();
            }
            return locations;
        }

        [HttpGet("{id}")]
        public Location Get(int id)
        {
            var conn = new NpgsqlConnection();
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM loctions WHERE id=@id",conn); 
            cmd.Parameters.AddWithValue("id",id);   
            NpgsqlDataReader reader = cmd.ExecuteReader();  
            while (reader.Read())
            {
                Location location = new Location
                {
                    id = reader.GetInt32("id"),
                    name = reader.GetString("name"),
                    x = reader.GetDouble("x"),
                    y = reader.GetDouble("y"),
                };
                conn.Close();
                return location;
            }
            return null;
        }

        [HttpDelete("{id}")]
        
        public void Delete(int id)
        {
            NpgsqlDataReader dataReader;
            using(NpgsqlConnection conn = new NpgsqlConnection())
            {
                conn.Open();    
                using( var cmd = new NpgsqlCommand("DELETE FROM locations WHERE id=@id",conn) )
                {
                    cmd.Parameters.AddWithValue("@id",id);   
                    dataReader = cmd.ExecuteReader();
                     dataReader.Close();
                    conn.Close();   
                    dataReader = null;
                }
            }
        }
        [HttpPut]
        public void Update (Location location)
        {
            NpgsqlDataReader dataReader;
            using(var conn = new NpgsqlConnection())
            {
                conn.Open();
                using (var cmd = new NpgsqlCommand("UPDATE locations SET name=@name, x=@x, y=@y WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("id",location.id);
                    cmd.Parameters.AddWithValue("name",location.name);          
                    cmd.Parameters.AddWithValue("x",location.x);    
                    cmd.Parameters.AddWithValue("y",location.y);

                    dataReader=cmd.ExecuteReader();
                    dataReader.Close();
                    conn.Close ();  
                }
            }
        }
    }    
    
   
}
