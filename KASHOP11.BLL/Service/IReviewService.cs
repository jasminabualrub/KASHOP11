using KASHOP11.DAL.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP11.BLL.Service
{
    public interface IReviewService
    {
        public Task<bool>AddReview(string userId,AddReviewRequest request);
    }
}
