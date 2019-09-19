using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using test_mongo_auth.Models;
using test_mongo_auth.Services;

namespace test_mongo_auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        public ActionResult<List<Post>> Get() =>
            _postService.Get();

        [HttpGet("{id:length(24)}", Name = "GetPost")]
        public ActionResult<Post> Get(string id)
        {
            var post = _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpPost]
        public ActionResult<Post> Create(Post post)
        {
            _postService.Create(post);

            return CreatedAtRoute("GetPost", new { id = post.Id.ToString() }, post);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Post postIn)
        {
            var post = _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            _postService.Update(id, postIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var post = _postService.Get(id);

            if (post == null)
            {
                return NotFound();
            }

            _postService.Remove(post.Id);

            return NoContent();
        }
    }
}