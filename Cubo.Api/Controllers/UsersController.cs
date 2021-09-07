using System;
using System.Linq;
using Cubo.Api.Filters;
using Cubo.Api.Models;
using Cubo.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cubo.Api.Controllers
{
    [Route("[controller]")]
    [ExceptionHandler]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;
        private readonly ILogger _logger;

        public UsersController(IUserRepository userRepository,
            IMemoryCache cache,
            ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        => Json(_userRepository.GetAll().Select(x => x.Name));

        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {

            var key = $"user-{name}";
            _logger.LogInformation($"Fetch user by name: {name}.");
            var user = _cache.Get<User>(key);
            if (user == null)
            {
                _logger.LogInformation($"User {name} was not found in Redis cache.");
                user = _userRepository.Get(name);
                _cache.Set(key, user, TimeSpan.FromSeconds(10)); 

                }
            else
                {

                    _logger.LogInformation($"User {name} was found in Redis cache.");
                }
                if (user == null)
                {
                    return NotFound();
                }

                return Json(user);
            }
        }
    }


