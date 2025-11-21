using System.Linq.Expressions;
using Azure;
using Lab2.Data;
using Lab2.DTO;
using Lab2.DTO.RegisterDTO;
using Lab2.Models;
using Lab2.ViewMode;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIGameController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Lab2.Models.Lab2 lab1 = new Lab2.Models.Lab2()
            {
                CourseName = "Web Programming",
                CourseCode = "WEBD6201",
                Name = "John Doe",
                StudentCode = "123456789",
                Class = "WEBD6201-01"
            };
            int status = 1;
            string message = "Data retrieved successfully";
            var data = new { status, message, lab1 };
            return new JsonResult(data);
        }

        private readonly ApplicationDbContext _db;
        protected ReponseApi _response;
        private readonly UserManager<ApplicationUser> _userManager;
        public APIGameController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _response = new();
            _userManager = userManager;
        }
        [HttpGet("GetAllGameLevel")]
        public async Task<IActionResult> GetAllGameLevel()
        {
            try
            {
                var gameLevel = await _db.GameLevels.ToListAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = gameLevel;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("GetAllQuestionGame")]
        public async Task<IActionResult> GetAllQuestionGame()
        {
            try
            {
                var questionGame = await _db.Questions.ToListAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = questionGame;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("GetAllRegion")]
        public async Task<IActionResult> GetAllRegion()
        {
            try
            {
                var region = await _db.Regions.ToListAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = region;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            try
            {
                var user = new ApplicationUser
                {
                    UserName = registerDTO.Email,
                    Email = registerDTO.Email,
                    Name = registerDTO.Name,
                    RegionId = registerDTO.RegionId,
                    Avatar = registerDTO.LinkAvatar
                };
                var result = await _userManager.CreateAsync(user, registerDTO.Password);
                if (result.Succeeded)
                {
                    _response.IsSuccess = true;
                    _response.Notification = "Đăng ký thành công";
                    _response.Data = user;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Đăng ký thất bại";
                    _response.Data = result.Errors;
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                var email = loginRequest.Email;
                var password = loginRequest.Password;

                var user = await _userManager.FindByEmailAsync(email);
                if (user != null && await _userManager.CheckPasswordAsync(user, password))
                {
                    _response.IsSuccess = true;
                    _response.Notification = "Đăng nhập thành công";
                    _response.Data = user;
                    return Ok(_response);
                }
                else
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Đăng nhập thất bại";
                    _response.Data = "Email hoặc mật khẩu không đúng";
                    return BadRequest(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi" + ex.Message;
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("GetAllQuestionGameByLevel/{levelId}")]
        public async Task<IActionResult> GetAllQuestionGameByLevel(int levelId)
        {
            try
            {
                var questionGame = await _db.Questions.Where(x => x.levelId == levelId).ToListAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = questionGame;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPost("SaveResult")]
        public async Task<IActionResult> SaveResult(LevelResultDTO levelResult)
        {
            try
            {
                var levelResultSave = new LevelResult
                {
                    UserId = levelResult.UserId,
                    LevelId = levelResult.LevelId,
                    Score = levelResult.Score,
                    CompletionDate = DateOnly.FromDateTime(DateTime.Now)
                };
                await _db.LevelResults.AddAsync(levelResultSave);
                await _db.SaveChangesAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lưu kết quả thành công";
                _response.Data = levelResultSave;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("Rating/{idRegion}")]
        public async Task<IActionResult> Rating(int idRegion)
        {
            try
            {
                if (idRegion > 0)
                {
                    var nameRegion = await _db.Regions.Where(x => x.RegionId == idRegion)
                        .Select(x => x.Name).FirstOrDefaultAsync();
                    if (nameRegion == null)
                    {
                        _response.IsSuccess = false;
                        _response.Notification = "Vùng không tồn tại";
                        _response.Data = null;
                        return BadRequest(_response);
                    }
                    var userByRegion = await _db.Users.Where(x => x.RegionId == idRegion).ToListAsync();
                    var listUserId = userByRegion.Select(u => u.Id).ToList();
                    var resultLevelByRegion = await _db.LevelResults
                                .Where(lr => listUserId.Contains(lr.UserId.ToString()))
                                .ToListAsync();
                    RatingVM ratingVM = new();
                    ratingVM.NameRegion = nameRegion;
                    ratingVM.userResultSums = new();
                    foreach (var item in userByRegion)
                    {
                        var sumScore = resultLevelByRegion.Where(x => x.UserId.ToString() == item.Id).Sum(x => x.Score);
                        var sumLevel = resultLevelByRegion.Where(x => x.UserId.ToString() == item.Id).Count();
                        UserResultSum userResultSum = new();
                        userResultSum.NameUser = item.Name;
                        userResultSum.SumScore = sumScore;
                        userResultSum.SumLevel = sumLevel;
                        ratingVM.userResultSums.Add(userResultSum);
                    }
                    _response.IsSuccess = true;
                    _response.Notification = "Lấy dữ liệu thành công";
                    _response.Data = ratingVM;
                    return Ok(_response);
                }
                else
                {
                    var user = await _db.Users.ToListAsync();
                    var resultLevel = await _db.LevelResults.ToListAsync();
                    string nameRegion = "Tất cả";
                    RatingVM ratingVM = new();
                    ratingVM.NameRegion = nameRegion;
                    ratingVM.userResultSums = new();
                    foreach (var item in user)
                    {
                        var sumScore = resultLevel.Where(x => x.UserId.ToString() == item.Id).Sum(x => x.Score);
                        var sumLevel = resultLevel.Where(x => x.UserId.ToString() == item.Id).Count();
                        UserResultSum userResultSum = new();
                        userResultSum.NameUser = item.Name;
                        userResultSum.SumScore = sumScore;
                        userResultSum.SumLevel = sumLevel;
                        ratingVM.userResultSums.Add(userResultSum);
                    }
                    _response.IsSuccess = true;
                    _response.Notification = "Lấy dữ liệu thành công";
                    _response.Data = ratingVM;
                    return Ok(_response);

                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpGet("GetUserInfomation/{userId}")]
        public async Task<IActionResult> GetUserInfomation(string userId)
        {
            try
            {
                var user = await _db.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
                if (user == null)
                {
                    _response.IsSuccess = false;
                    _response.Notification = "Không tìm thấy dữ liệu";
                    _response.Data = null;
                    return BadRequest(_response);
                }
                UserInformationVM userInformationVM = new();
                userInformationVM.Name = user.Name;
                userInformationVM.Email = user.Email;
                userInformationVM.avatar = user.Avatar;
                userInformationVM.Region = await _db.Regions.Where(x => x.RegionId == user.RegionId)
                    .Select(x => x.Name).FirstOrDefaultAsync();
                _response.IsSuccess = true;
                _response.Notification = "Lấy dữ liệu thành công";
                _response.Data = userInformationVM;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Notification = "Lỗi";
                _response.Data = ex.Message;
                return BadRequest(_response);
            }
        }

        [HttpPost("upload-single")]
        public async Task<IActionResult> UploadSingle([FromForm] IFromData file)
        {
            try
            {
                var fileExtension = Path.GetExtension(file.fromFile.FileName);
                var fileName = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.fromFile.CopyToAsync(stream);
                }
                var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                return Ok(new { fileUrl });
            }
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("upload-multiple")]
        public async Task<IActionResult> UploadMultiple([FromForm] ListIFormFileData files)
        {
            try 
            {
                List<string> fileUrls = new();
                foreach (var file in files.formFiles)
                {
                    var fileExtension = Path.GetExtension(file.FileName);   
                    var fileName = Guid.NewGuid().ToString() + fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
                    fileUrls.Add(fileUrl);
                }
                return Ok(new { fileUrls });
            }
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
