using Microsoft.AspNetCore.Mvc;
using TASKHIVE.Data;

namespace TASKHIVE.Controllers.PManagerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PmUserListController : ControllerBase
    {
        public readonly DataContext _context;

        public PmUserListController(DataContext _context)
        {
            this._context = _context;
        }

         [HttpGet("list")]
        public ActionResult<IEnumerable<ViewUserListDto>> GetAll()
        {
            var users = _context.Users
                .Include(u => u.UserCategory)
                .ToList();

            // Map each user to ViewUserListDto
            var viewUserListDtos = users.Select(user => new ViewUserListDto
            {

                ProfileImageUrl = user.ProfileImageUrl,
                UserId = user.UserId,
                FirstName = user.FirstName,
                UserName = user.UserName,
                Email = user.Email,
                UserCategoryType = user.UserCategory != null ? user.UserCategory.UserCategoryType : null,
                IsActive = user.IsActive
            }).ToList();

            return Ok(viewUserListDtos);
        }
    }
}
