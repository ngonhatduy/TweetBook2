using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using TweetBook2.Contracts.V1;
using TweetBook2.Contracts.V1.Requests;
using TweetBook2.Contracts.V1.Responses;
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

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post { Id = postRequest.Id };

            if(string.IsNullOrEmpty(post.Id))
                post.Id = Guid.NewGuid().ToString();

            _posts.Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
            var locationUrl = baseUrl + ApiRoutes.Posts.Get.Replace("{postId}", post.Id);
            var response = new PostResponse { Id = post.Id };
            return Created(locationUrl, response);
        }
    }
}
