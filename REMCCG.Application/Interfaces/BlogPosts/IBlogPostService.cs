using REMCCG.Application.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REMCCG.Application.Interfaces.BlogPosts
{
    public interface IBlogPostService : ICRUD<BlogPostDTO, BlogPostModel>
    {
    }
}
