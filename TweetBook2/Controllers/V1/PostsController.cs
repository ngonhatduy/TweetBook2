using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TweetBook2.Contracts.V1;
using TweetBook2.Contracts.V1.Requests;
using TweetBook2.Contracts.V1.Responses;
using TweetBook2.Domain;
using TweetBook2.Services;

namespace TweetBook2.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet(ApiRoutes.Posts.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _postService.GetPostsAsync());
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid postId)
        {
            var post = await _postService.GetPostsByIdAsync(postId);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public async Task<IActionResult> Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post { Name = postRequest.Name };

            await _postService.CreatePostAsync(post);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/";
            var locationUrl = baseUrl + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());
            var response = new PostResponse { Id = post.Id };
            return Created(locationUrl, response);
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid postId, [FromBody] UpdatePostRequest request)
        {
            var post = new Post
            {
                Id = postId,
                Name = request.Name,
            };

            var updated = await _postService.UpdatePostAsync(post);
            if (updated)
                return Ok(post);
            return NotFound();
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid postId)
        {
            var deleted = await _postService.DeletePostAsync(postId);
            if (deleted)
                return NoContent();
            return NotFound();
        }
    }
}
