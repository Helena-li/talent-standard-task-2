using Talent.Common.Contracts;
using Talent.Common.Models;
using Talent.Services.Profile.Domain.Contracts;
using Talent.Services.Profile.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using Talent.Services.Profile.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Talent.Common.Security;

namespace Talent.Services.Profile.Domain.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserAppContext _userAppContext;
        IRepository<UserLanguage> _userLanguageRepository;
        IRepository<User> _userRepository;
        IRepository<Employer> _employerRepository;
        IRepository<Job> _jobRepository;
        IRepository<Recruiter> _recruiterRepository;
        IFileService _fileService;


        public ProfileService(IUserAppContext userAppContext,
                              IRepository<UserLanguage> userLanguageRepository,
                              IRepository<User> userRepository,
                              IRepository<Employer> employerRepository,
                              IRepository<Job> jobRepository,
                              IRepository<Recruiter> recruiterRepository,
                              IFileService fileService)
        {
            _userAppContext = userAppContext;
            _userLanguageRepository = userLanguageRepository;
            _userRepository = userRepository;
            _employerRepository = employerRepository;
            _jobRepository = jobRepository;
            _recruiterRepository = recruiterRepository;
            _fileService = fileService;
        }


        #region add talent subitems
        public async Task<bool> AddNewLanguage(AddLanguageViewModel language)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }

                var newLanguage = new UserLanguage
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    IsDeleted = false,
                    UserId = userId
                };
                UpdateLanguageFromView(language, newLanguage);
                userProfile.Languages.Add(newLanguage);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateLanguage(AddLanguageViewModel language)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }
                var orginalLanguage = userProfile.Languages.SingleOrDefault(x => x.Id == language.Id);
                if (orginalLanguage == null)
                {
                    return false;
                }
                UpdateLanguageFromView(language, orginalLanguage);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> AddNewSkill(AddSkillViewModel skill)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile==null)
                {
                    return false;
                }
                
                var newSkill = new UserSkill
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    IsDeleted = false,
                    UserId=userId
                };
                UpdateSkillFromView(skill, newSkill);
                userProfile.Skills.Add(newSkill);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateSkill(AddSkillViewModel skill)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }
                var orginalSkill = userProfile.Skills.SingleOrDefault(x => x.Id == skill.Id);
                if (orginalSkill==null)
                {
                    return false;
                }
                UpdateSkillFromView(skill, orginalSkill);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> AddNewExperience(ExperienceViewModel experience)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }

                var newExperience = new UserExperience
                {
                    Id = ObjectId.GenerateNewId().ToString()
                };
                UpdateExperienceFromView(experience, newExperience);
                userProfile.Experience.Add(newExperience);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateExperience(ExperienceViewModel experience)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }
                var orginalExperience = userProfile.Experience.SingleOrDefault(x => x.Id == experience.Id);
                if (orginalExperience == null)
                {
                    return false;
                }
                UpdateExperienceFromView(experience, orginalExperience);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> AddNewEducation(AddEducationViewModel model)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }

                var newEducation = new UserEducation
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    IsDeleted = false,
                    UserId = userId
                };
                UpdateEducationFromView(model, newEducation);
                userProfile.Education.Add(newEducation);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateEducation(AddEducationViewModel model)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }
                var orginalEducation = userProfile.Education.SingleOrDefault(x => x.Id == model.Id);
                if (orginalEducation == null)
                {
                    return false;
                }
                UpdateEducationFromView(model, orginalEducation);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> AddNewCertification(AddCertificationViewModel certification)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }

                var newCertification = new UserCertification
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    IsDeleted = false
                };
                UpdateCertificationFromView(certification, newCertification);
                userProfile.Certifications.Add(newCertification);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> UpdateCertification(AddCertificationViewModel certification)
        {
            try
            {
                var userId = _userAppContext.CurrentUserId;
                User userProfile = await _userRepository.GetByIdAsync(userId);
                if (userProfile == null)
                {
                    return false;
                }
                var orginalCertification = userProfile.Certifications.SingleOrDefault(x => x.Id == certification.Id);
                if (orginalCertification == null)
                {
                    return false;
                }
                UpdateCertificationFromView(certification, orginalCertification);
                await _userRepository.Update(userProfile);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        #endregion
        public async Task<TalentProfileViewModel> GetTalentProfile(string Id)
        {
            User userProfile = (await _userRepository.GetByIdAsync(Id));
            var videoUrl = "";
            var CvUrl = "";

            if (userProfile!=null)
            {
                videoUrl = string.IsNullOrWhiteSpace(userProfile.VideoName)
                          ? ""
                          : await _fileService.GetFileURL(userProfile.VideoName, FileType.UserVideo);
                CvUrl = string.IsNullOrWhiteSpace(userProfile.CvName)
                          ? ""
                          : await _fileService.GetFileURL(userProfile.CvName, FileType.UserCV);
                var skills = userProfile.Skills.Where(x=>x.IsDeleted==false).Select(x => ViewModelFromSkill(x)).ToList();
                var languages = userProfile.Languages.Where(x => x.IsDeleted == false).Select(x => ViewModelFromLanguage(x)).ToList();
                var education= userProfile.Education.Where(x=>x.IsDeleted==false).Select(x => ViewModelFromEducation(x)).ToList();
                var certifications = userProfile.Certifications.Where(x=>x.IsDeleted==false).Select(x => ViewModelFromCertification(x)).ToList();
                var experience = userProfile.Experience.Select(x => ViewModelFromExperience(x)).ToList();

                var results = new TalentProfileViewModel()
                {
                    Id = userProfile.Id,
                    FirstName = userProfile.FirstName,
                    MiddleName = userProfile.MiddleName,
                    LastName = userProfile.LastName,
                    Gender = userProfile.Gender,
                    Email = userProfile.Email,
                    Phone = userProfile.Phone,
                    MobilePhone = userProfile.MobilePhone,
                    IsMobilePhoneVerified = userProfile.IsMobilePhoneVerified,
                    Address = userProfile.Address,
                    Nationality = userProfile.Nationality,
                    VisaStatus = userProfile.VisaStatus,
                    VisaExpiryDate = userProfile.VisaExpiryDate,
                    ProfilePhoto = userProfile.ProfilePhoto,
                    ProfilePhotoUrl = userProfile.ProfilePhotoUrl,
                    VideoName = userProfile.VideoName,
                    VideoUrl = videoUrl,
                    CvName=userProfile.CvName,
                    CvUrl= CvUrl,
                    Summary=userProfile.Summary,
                    Description=userProfile.Description,
                    LinkedAccounts=userProfile.LinkedAccounts,
                    JobSeekingStatus=userProfile.JobSeekingStatus,
                    Languages= languages,
                    Skills=skills,
                    Education=education,
                    Certifications=certifications,
                    Experience=experience
                };

                return results;
            }
            return null;
        }

        public async Task<bool> UpdateTalentProfile(TalentProfileViewModel model, string updaterId)
        {
            try
            {
                if (model.Id!=null)
                {
                    User existingUser = (await _userRepository.GetByIdAsync(model.Id));
                    existingUser.FirstName = model.FirstName;
                    existingUser.MiddleName = model.MiddleName;
                    existingUser.LastName = model.LastName;
                    existingUser.Gender = model.Gender;

                    existingUser.Email = model.Email;
                    existingUser.Phone = model.Phone;
                    //existingUser.MobilePhone = model.MobilePhone;
                    //existingUser.IsMobilePhoneVerified = model.IsMobilePhoneVerified;

                    existingUser.Address = model.Address;
                    existingUser.Nationality = model.Nationality;
                    existingUser.VisaStatus = model.VisaStatus;
                    existingUser.VisaExpiryDate = model.VisaExpiryDate;

                    existingUser.ProfilePhoto = model.ProfilePhoto;
                    existingUser.ProfilePhotoUrl = model.ProfilePhotoUrl;
                    existingUser.VideoName = model.VideoName;
                    //existingUser.VideoUrl = videoUrl;
                    //existingUser.CvName = model.CvName;
                    //existingUser.CvUrl = CvUrl;
                    existingUser.Summary = model.Summary;
                    existingUser.Description = model.Description;
                    existingUser.LinkedAccounts = model.LinkedAccounts;
                    existingUser.JobSeekingStatus = model.JobSeekingStatus;
                    existingUser.UpdatedBy = updaterId;
                    existingUser.UpdatedOn = DateTime.Now;

                    var newSkills = new List<UserSkill>();
                    foreach (var item in model.Skills)
                    {
                        var skill = existingUser.Skills.SingleOrDefault(x => x.Id == item.Id);
                        if (skill == null)
                        {
                            skill = new UserSkill
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                IsDeleted = false
                            };
                        }
                        UpdateSkillFromView(item, skill);
                        newSkills.Add(skill);
                    }
                    existingUser.Skills = newSkills;

                    var newEducation = new List<UserEducation>();
                    foreach (var item in model.Education)
                    {
                        var education = existingUser.Education.SingleOrDefault(x => x.Id == item.Id);
                        if (education==null)
                        {
                            education = new UserEducation
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                IsDeleted = false,
                                CreatedOn= DateTime.Now,
                                CreatedBy= updaterId
                                //Uid and UserId are not given
                            };
                        }
                        UpdateEducationFromView(item, education);
                        education.UpdatedOn = DateTime.Now;
                        education.UpdatedBy = updaterId;
                        
                        newEducation.Add(education);
                    }
                    existingUser.Education = newEducation;

                    var newLanguages = new List<UserLanguage>();
                    foreach (var item in model.Languages)
                    {
                        var language = existingUser.Languages.SingleOrDefault(x => x.Id == item.Id);
                        
                        if (language == null)
                        {
                            language = new UserLanguage
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                IsDeleted = false,
                                UserId= updaterId
                            };
                        }
                        UpdateLanguageFromView(item, language);
                        newLanguages.Add(language);
                    }
                    existingUser.Languages = newLanguages;

                    var newCertifications = new List<UserCertification>();
                    foreach (var item in model.Certifications)
                    {
                        var certification = existingUser.Certifications.SingleOrDefault(x => x.Id == item.Id);
                        if (certification == null)
                        {
                            certification = new UserCertification
                            {
                                Id = ObjectId.GenerateNewId().ToString(),
                                IsDeleted = false,
                                CreatedOn = DateTime.Now,
                                CreatedBy = updaterId
                            };
                        }
                        UpdateCertificationFromView(item, certification);
                        certification.UpdatedOn = DateTime.Now;
                        certification.UpdatedBy = updaterId;
                        newCertifications.Add(certification);
                    }
                    existingUser.Certifications = newCertifications;
                    
                    var newExperience = new List<UserExperience>();
                    foreach (var item in model.Experience)
                    {
                        var experience = existingUser.Experience.SingleOrDefault(x => x.Id == item.Id);
                        if (experience == null)
                        {
                            experience = new UserExperience
                            {
                                Id = ObjectId.GenerateNewId().ToString()
                            };
                        }
                        UpdateExperienceFromView(item, experience);
                        newExperience.Add(experience);
                    }
                    existingUser.Experience = newExperience;
                    await _userRepository.Update(existingUser);
                    return true;
                }
                return false;
            }
            catch (MongoException e)
            {
                return false;
            }
            
        }

        public async Task<EmployerProfileViewModel> GetEmployerProfile(string Id, string role)
        {

            Employer profile = null;
            switch (role)
            {
                case "employer":
                    profile = (await _employerRepository.GetByIdAsync(Id));
                    break;
                case "recruiter":
                    profile = (await _recruiterRepository.GetByIdAsync(Id));
                    break;
            }

            var videoUrl = "";

            if (profile != null)
            {
                videoUrl = string.IsNullOrWhiteSpace(profile.VideoName)
                          ? ""
                          : await _fileService.GetFileURL(profile.VideoName, FileType.UserVideo);

                var skills = profile.Skills.Select(x => ViewModelFromSkill(x)).ToList();

                var result = new EmployerProfileViewModel
                {
                    Id = profile.Id,
                    CompanyContact = profile.CompanyContact,
                    PrimaryContact = profile.PrimaryContact,
                    Skills = skills,
                    ProfilePhoto = profile.ProfilePhoto,
                    ProfilePhotoUrl = profile.ProfilePhotoUrl,
                    VideoName = profile.VideoName,
                    VideoUrl = videoUrl,
                    DisplayProfile = profile.DisplayProfile,
                };
                return result;
            }

            return null;
        }

        public async Task<bool> UpdateEmployerProfile(EmployerProfileViewModel employer, string updaterId, string role)
        {
            try
            {
                if (employer.Id != null)
                {
                    switch (role)
                    {
                        case "employer":
                            Employer existingEmployer = (await _employerRepository.GetByIdAsync(employer.Id));
                            existingEmployer.CompanyContact = employer.CompanyContact;
                            existingEmployer.PrimaryContact = employer.PrimaryContact;
                            existingEmployer.ProfilePhoto = employer.ProfilePhoto;
                            existingEmployer.ProfilePhotoUrl = employer.ProfilePhotoUrl;
                            existingEmployer.DisplayProfile = employer.DisplayProfile;
                            existingEmployer.UpdatedBy = updaterId;
                            existingEmployer.UpdatedOn = DateTime.Now;

                            var newSkills = new List<UserSkill>();
                            foreach (var item in employer.Skills)
                            {
                                var skill = existingEmployer.Skills.SingleOrDefault(x => x.Id == item.Id);
                                if (skill == null)
                                {
                                    skill = new UserSkill
                                    {
                                        Id = ObjectId.GenerateNewId().ToString(),
                                        IsDeleted = false
                                    };
                                }
                                UpdateSkillFromView(item, skill);
                                newSkills.Add(skill);
                            }
                            existingEmployer.Skills = newSkills;

                            await _employerRepository.Update(existingEmployer);
                            break;

                        case "recruiter":
                            Recruiter existingRecruiter = (await _recruiterRepository.GetByIdAsync(employer.Id));
                            existingRecruiter.CompanyContact = employer.CompanyContact;
                            existingRecruiter.PrimaryContact = employer.PrimaryContact;
                            existingRecruiter.ProfilePhoto = employer.ProfilePhoto;
                            existingRecruiter.ProfilePhotoUrl = employer.ProfilePhotoUrl;
                            existingRecruiter.DisplayProfile = employer.DisplayProfile;
                            existingRecruiter.UpdatedBy = updaterId;
                            existingRecruiter.UpdatedOn = DateTime.Now;

                            var newRSkills = new List<UserSkill>();
                            foreach (var item in employer.Skills)
                            {
                                var skill = existingRecruiter.Skills.SingleOrDefault(x => x.Id == item.Id);
                                if (skill == null)
                                {
                                    skill = new UserSkill
                                    {
                                        Id = ObjectId.GenerateNewId().ToString(),
                                        IsDeleted = false
                                    };
                                }
                                UpdateSkillFromView(item, skill);
                                newRSkills.Add(skill);
                            }
                            existingRecruiter.Skills = newRSkills;
                            await _recruiterRepository.Update(existingRecruiter);

                            break;
                    }
                    return true;
                }
                return false;
            }
            catch (MongoException e)
            {
                return false;
            }
        }

        public async Task<bool> UpdateEmployerPhoto(string employerId, IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            List<string> acceptedExtensions = new List<string> { ".jpg", ".png", ".gif", ".jpeg" };

            if (fileExtension != null && !acceptedExtensions.Contains(fileExtension.ToLower()))
            {
                return false;
            }

            var profile = (await _employerRepository.Get(x => x.Id == employerId)).SingleOrDefault();

            if (profile == null)
            {
                return false;
            }

            var newFileName = await _fileService.SaveFile(file, FileType.ProfilePhoto);

            if (!string.IsNullOrWhiteSpace(newFileName))
            {
                var oldFileName = profile.ProfilePhoto;

                if (!string.IsNullOrWhiteSpace(oldFileName))
                {
                    await _fileService.DeleteFile(oldFileName, FileType.ProfilePhoto);
                }

                profile.ProfilePhoto = newFileName;
                profile.ProfilePhotoUrl = await _fileService.GetFileURL(newFileName, FileType.ProfilePhoto);

                await _employerRepository.Update(profile);
                return true;
            }

            return false;

        }

        public async Task<bool> AddEmployerVideo(string employerId, IFormFile file)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateTalentPhoto(string talentId, IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            List<string> acceptedExtensions = new List<string> { ".jpg", ".png", ".gif", ".jpeg" };

            if ((fileExtension != null && !acceptedExtensions.Contains(fileExtension.ToLower()))||(fileExtension == null))
            {
                return false;
            }

            var profile = (await _userRepository.Get(x => x.Id == talentId)).SingleOrDefault();

            if (profile == null)
            {
                return false;
            }

            var newFileName = await _fileService.SaveFile(file, FileType.ProfilePhoto);

            if (!string.IsNullOrWhiteSpace(newFileName))
            {
                var oldFileName = profile.ProfilePhoto;

                if (!string.IsNullOrWhiteSpace(oldFileName))
                {
                    await _fileService.DeleteFile(oldFileName, FileType.ProfilePhoto);
                }

                profile.ProfilePhoto = newFileName;
                profile.ProfilePhotoUrl = await _fileService.GetFileURL(newFileName, FileType.ProfilePhoto);

                await _userRepository.Update(profile);
                return true;
            }

            return false;
        }

        public async Task<bool> AddTalentVideo(string talentId, IFormFile file)
        {
            var fileExtension = Path.GetExtension(file.FileName);
            List<string> acceptedExtensions = new List<string> { ".mp4", ".mov" };

            if ((fileExtension != null && !acceptedExtensions.Contains(fileExtension.ToLower())) || (fileExtension == null))
            {
                return false;
            }
            try
            {
                var profile = (await _userRepository.Get(x => x.Id == talentId)).SingleOrDefault();

                if (profile == null)
                {
                    return false;
                }

                var newFileName = await _fileService.SaveFile(file, FileType.UserVideo);

                if (!string.IsNullOrWhiteSpace(newFileName))
                {
                    profile.VideoName = newFileName;
                    //profile.video = await _fileService.GetFileURL(newFileName, FileType.UserVideo);
                    profile.Videos.Add(new TalentVideo
                    {
                        FullVideoName = newFileName,
                        DisplayVideoName = file.FileName,
                        Tags = new List<string>()
                    });
                    await _userRepository.Update(profile);
                    return true;
                }
                return false;
            }
            catch (MongoException e)
            {

                return false;
            }
            catch (ApplicationException e)
            {
                return false;
            }
        }

        public async Task<bool> RemoveTalentVideo(string talentId, string videoName)
        {
            try
            {
                var deleteFile = await _fileService.DeleteFile(videoName, FileType.UserVideo);
                if (deleteFile)
                {
                    var profile = (await _userRepository.Get(x => x.Id == talentId)).SingleOrDefault();
                    if (profile == null)
                    {
                        return false;
                    }
                    var findItem = profile.Videos.SingleOrDefault(x => x.FullVideoName == videoName);
                    if (findItem!=null)
                    {
                        profile.Videos.Remove(findItem);
                        await _userRepository.Update(profile);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateTalentCV(string talentId, IFormFile file)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<string>> GetTalentSuggestionIds(string employerOrJobId, bool forJob, int position, int increment)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentSnapshotViewModel>> GetTalentSnapshotList(string employerOrJobId, bool forJob, int position, int increment)
        {
            try
            {
                var profile = await _employerRepository.GetByIdAsync(employerOrJobId);
                var talentFeedList = _userRepository.Collection.Skip(position).Take(increment).AsEnumerable();
                if (profile!=null)
                {
                    var result = new List<TalentSnapshotViewModel>();
                    foreach (var item in talentFeedList)
                    {
                        var feedItem = new TalentSnapshotViewModel();
                        feedItem.Id = item.Id;
                        feedItem.Name = item.LastName + ' ' + item.FirstName;
                        feedItem.PhotoId = item.ProfilePhotoUrl;
                        feedItem.VideoUrl = String.IsNullOrEmpty(item.VideoName) ? null:( await _fileService.GetFileURL(item.VideoName, FileType.UserVideo));
                        feedItem.CVUrl = String.IsNullOrEmpty(item.CvName) ? null: (await _fileService.GetFileURL(item.CvName, FileType.UserVideo));
                        feedItem.Summary = item.Summary;
                        var recentExperience = item.Experience.OrderByDescending(x => x.End).FirstOrDefault();
                        feedItem.CurrentEmployment = (recentExperience != null) ? (recentExperience.Company + ' ' + recentExperience.Position)
                            : "Not defined";
                        feedItem.Visa = item.VisaStatus;
                        feedItem.Level = "Not defined";
                        if (item.Skills.Count!=0)
                        {
                            feedItem.Skills = item.Skills.Select(x => x.Skill).ToList();
                        }
                        else
                        {
                            feedItem.Skills =null;
                        }
                        result.Add(feedItem);
                    }
                    return result;
                }
                return null;
            }
            catch (Exception e)
            {

                return null;
            }
        }

        public async Task<IEnumerable<TalentSnapshotViewModel>> GetTalentSnapshotList(IEnumerable<string> ids)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        #region TalentMatching

        public async Task<IEnumerable<TalentSuggestionViewModel>> GetFullTalentList()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public IEnumerable<TalentMatchingEmployerViewModel> GetEmployerList()
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentMatchingEmployerViewModel>> GetEmployerListByFilterAsync(SearchCompanyModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentSuggestionViewModel>> GetTalentListByFilterAsync(SearchTalentModel model)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TalentSuggestion>> GetSuggestionList(string employerOrJobId, bool forJob, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<bool> AddTalentSuggestions(AddTalentSuggestionList selectedTalents)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        #endregion

        #region Conversion Methods

        #region Update from View

        protected void UpdateSkillFromView(AddSkillViewModel model, UserSkill original)
        {
            original.ExperienceLevel = model.Level;
            original.Skill = model.Name;
        }

        protected void UpdateEducationFromView(AddEducationViewModel model, UserEducation original)
        {
            original.Country = model.Country;
            original.InstituteName = model.InstituteName;
            original.Title = model.Title;
            original.Degree = model.Degree;
            original.YearOfGraduation = model.YearOfGraduation;
        }

        protected void UpdateLanguageFromView(AddLanguageViewModel model, UserLanguage original)
        {
            //original.UserId = model.CurrentUserId;
            original.LanguageLevel = model.Level;
            original.Language = model.Name;
        }

        protected void UpdateCertificationFromView(AddCertificationViewModel model, UserCertification original)
        {
            original.CertificationName = model.CertificationName;
            original.CertificationFrom = model.CertificationFrom;
            original.CertificationYear = model.CertificationYear;
        }

        protected void UpdateExperienceFromView(ExperienceViewModel model, UserExperience original)
        {
            original.Company = model.Company;
            original.Position = model.Position;
            original.Responsibilities = model.Responsibilities;
            original.Start = model.Start;
            original.End = model.End;
        }

        #endregion

        #region Build Views from Model

        protected AddSkillViewModel ViewModelFromSkill(UserSkill skill)
        {
            return new AddSkillViewModel
            {
                Id = skill.Id,
                Level = skill.ExperienceLevel,
                Name = skill.Skill
            };
        }

        protected AddLanguageViewModel ViewModelFromLanguage(UserLanguage language)
        {
            return new AddLanguageViewModel
            {
                Id = language.Id,
                CurrentUserId= language.UserId,
                Level = language.LanguageLevel,
                Name = language.Language
            };
        }

        protected AddEducationViewModel ViewModelFromEducation(UserEducation education)
        {
            return new AddEducationViewModel
            {
                Id = education.Id,
                Country = education.Country,
                InstituteName = education.InstituteName,
                Title = education.Title,
                Degree=education.Degree,
                YearOfGraduation=education.YearOfGraduation
            };
        }

        protected AddCertificationViewModel ViewModelFromCertification(UserCertification certification)
        {
            return new AddCertificationViewModel
            {
                Id = certification.Id,
                CertificationName = certification.CertificationName,
                CertificationFrom = certification.CertificationFrom,
                CertificationYear = certification.CertificationYear
            };
        }

        protected ExperienceViewModel ViewModelFromExperience(UserExperience experience)
        {
            return new ExperienceViewModel
            {
                Id = experience.Id,
                Company = experience.Company,
                Position = experience.Position,
                Responsibilities = experience.Responsibilities,
                Start = experience.Start,
                End= experience.End
            };
        }

        #endregion

        #endregion

        #region ManageClients

        public async Task<IEnumerable<ClientViewModel>> GetClientListAsync(string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<ClientViewModel> ConvertToClientsViewAsync(Client client, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();
        }

        public async Task<int> GetTotalTalentsForClient(string clientId, string recruiterId)
        {
            //Your code here;
            throw new NotImplementedException();

        }

        public async Task<Employer> GetEmployer(string employerId)
        {
            return await _employerRepository.GetByIdAsync(employerId);
        }
        #endregion

    }
}
