using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook2.Domain;

namespace TweetBook2.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync();

        Task<Post> GetPostsByIdAsync(Guid postId);

        Task<bool> UpdatePostAsync(Post postToUpdate);

        Task<bool> DeletePostAsync(Guid postId);

        Task<bool> CreatePostAsync(Post post);
    }
}
