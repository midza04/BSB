using Dashboard.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using System.Web.Mvc;
using System.Web.Http;

namespace Dashboard.Controllers
{
    public class ChartsController : ApiController
    {
        UploadsRepository _repo;
        
        public ChartsController()
        {
             _repo = new UploadsRepository();
        }
        // GET api/default2
        //public Dictionary<string, int> Get()
        //{
        //     var x = _repo.GetUploadCount();
        //     return x;
        //}


        public string  Get()
        {            
            var x = _repo.GetUploadCount(7);
            return JsonConvert.SerializeObject(new { dates = x.Keys.ToList(), recordCount =  x.Values.ToList() });
        }

        
    }
}
