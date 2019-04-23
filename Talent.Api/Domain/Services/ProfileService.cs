using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Talent.Api.Domain.Contracts;
using Talent.Api.Domain.Models;
using Talent.Common.Aws;
using Talent.Common.Contracts;
using Talent.Common.Models;

namespace Talent.Api.Domain.Services
{
    public class ProfileService : IProfileService
    {
        IRepository<UserLanguage> _userLanguageRepository;
        IRepository<User> _userRepository;
        IFileService _fileService;

        public ProfileService(IRepository<UserLanguage> userLanguageRepository,
                              IRepository<User> userRepository,
                              IFileService fileService)
        {
            _userLanguageRepository = userLanguageRepository;
            _userRepository = userRepository;
            _fileService = fileService;
        }

        public async Task<bool> AddTalentVideo(IFormFile file, string userId)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            List<string> acceptedExtensions = new List<string> { ".mp4", ".mov" };

            if (fileExtension != null && !acceptedExtensions.Contains(fileExtension.ToLower()))
            {
                return false;
            }
            try
            {
                var talent = await _userRepository.GetByIdAsync(userId);
                                
                var fileName = await _fileService.SaveFile(file, FileType.UserVideo);

                talent.VideoName = fileName;
                talent.Videos.Add(new TalentVideo
                {
                    FullVideoName = fileName,
                    DisplayVideoName = file.FileName,
                    Tags = new List<string>()
                });

                await _userRepository.Update(talent);
                return true;

            }
            catch (ApplicationException e)
            {
                return false;
            }
            catch (MongoException e)
            {
                return false;
            }

        }

        public async Task<TalentProfileViewModel> GetTalentProfile(string Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);

            var videoUrl = string.IsNullOrWhiteSpace(user.VideoName)
                           ? ""
                           : await _fileService.GetFileURL(user.VideoName, FileType.UserVideo);

            var result = new TalentProfileViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                MiddleName = user.MiddleName,
                LastName = user.LastName,
                Gender = user.Gender,
                Email = user.Email,
                Phone = user.Phone,
                MobilePhone = user.MobilePhone,
                IsMobilePhoneVerified = user.IsMobilePhoneVerified,
                Summary = user.Summary,
                Description = user.Description,
                Nationality = user.Nationality,
                Address = user.Address,
                VisaStatus = user.VisaStatus,
                VisaExpiryDate = user.VisaExpiryDate,
                ProfilePhoto = user.ProfilePhoto,
                ProfilePhotoUrl = user.ProfilePhotoUrl,
                VideoName = user.VideoName,
                VideoUrl = videoUrl,
                Certifications = user.Certifications,
                Experience = user.Experience,
                Education = user.Education,
                Skills = user.Skills
            };

            return result;
        }

        public async Task<TalentSnapshotMobileViewModel> GetTalentSnapshot(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if(user == null)
            {
                throw new ApplicationException("User does not exist!");
            }

            return BuildTalentSnapshot(user);
        }

        public async Task<IEnumerable<TalentSnapshotMobileViewModel>> GetTalentSnapshotList(string employerId, FeedIncrementModel increment)
        {
            IEnumerable<User> users = (await _userRepository.Get(x => !x.IsDeleted)).Skip(increment.Position).Take(increment.Number);

            List<TalentSnapshotMobileViewModel> result = new List<TalentSnapshotMobileViewModel>();

            foreach(var user in users)
            {
                result.Add(BuildTalentSnapshot(user));
            }
            return result;
        }

        public async Task<IEnumerable<User>> GetSuggestedTalent(string employerId)
        {
            /* *** PROTOTYPE ONLY! *** */

            var result = await _userRepository.Get(x => !x.IsDeleted);
            return result;
        }

        public async Task<IEnumerable<TalentVideoMobileViewModel>> GetTalentVideoFeed(string employerId, FeedIncrementModel increment)
        {
            IEnumerable<User> suggestions = await GetSuggestedTalent(employerId);

            var userChunk = suggestions.Skip(increment.Position).Take(increment.Number);

            // Generating the models is I/O-bound on getting the video URLs from AWS
            // Running them in parallel should improve performance
            var tasks = userChunk.Select(x => BuildTalentVideoModel(x));
            await Task.WhenAll(tasks);

            return tasks.Select(x => x.Result);
        }

        #region Conversion Helpers


        protected TalentSnapshotMobileViewModel BuildTalentSnapshot(User user)
        {
            String name = String.Format("{0} {1}", user.FirstName, user.LastName);
            List<string> skills = user.Skills.Select(x => x.Skill).ToList();
            //string photo = await _documentService.GetFileURL(user.ProfilePhoto, FileType.ProfilePhoto);

            UserExperience latest = user.Experience.OrderByDescending(x => x.End).FirstOrDefault();
            String level, employment;
            if (latest != null)
            {
                level = latest.Position;
                employment = latest.Company;
            }
            else
            {
                level = "Unknown";
                employment = "Unknown";
            }

            var result = new TalentSnapshotMobileViewModel
            {
                CurrentEmployment = employment,
                Id = user.Id,
                Level = level,
                Name = name,
                PhotoId = user.ProfilePhotoUrl,
                Skills = skills,
                Summary = user.Summary,
                Visa = user.VisaStatus
            };

            return result;
        }

        protected async Task<TalentVideoMobileViewModel> BuildTalentVideoModel(User user)
        {
            string videoUrl = await _fileService.GetFileURL(user.VideoName, FileType.UserVideo);
            string name = string.Format("{0} {1}", user.FirstName, user.LastName);
            return new TalentVideoMobileViewModel
            {
                VideoUrl = videoUrl,
                TalentName = name,
                TalentId = user.Id,
                LinkedInUrl = ""
            };
        }

        #endregion
    }
}
