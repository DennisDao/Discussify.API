using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discussify.API
{   
    public class JWTSettings 
    {
        public string Secret { get; set; }
    }
}
