using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FireSaverApi.Contracts;
using FireSaverApi.DataContext;
using FireSaverApi.Dtos;
using FireSaverApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FireSaverApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RouteBuilderController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IRoutebuilderService routeBuilder;

        public RouteBuilderController(IMapper mapper,
                                        IRoutebuilderService routeBuilder)
        {
            this.mapper = mapper;
            this.routeBuilder = routeBuilder;
        }

       
    }
}