using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TweetBook2.Contracts.V1;
using TweetBook2.Domain;

namespace TweetBook2.Controllers.V1
{
    public class PostsController : Controller
    {
        private List<Post> _posts;

        public PostsController()
        {
            _posts = new List<Post>();
            for (var i = 1; i < 5; i++)
            {
                _posts.Add(new Post { Id = Guid.NewGuid().ToString()});
            }
        }

        [HttpGet(ApiRoutes.Posts.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_posts);
        }
    }
}
