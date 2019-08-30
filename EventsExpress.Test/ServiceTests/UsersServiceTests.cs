﻿using System;
using System.Collections.Generic;
using System.Text;
using EventsExpress.Core.Services;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using AutoMapper;
using EventsExpress.Core.IServices;
using MediatR;
using EventsExpress.Core.Infrastructure;
using EventsExpress.Db.Entities;
using System.Linq;
using EventsExpress.Core.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EventsExpress.Test.ServiceTests
{
    [TestFixture]
    class UsersServiceTests: TestInitializer
    {
        private UserService service;
        

        private static Mock<IPhotoService> mockPhotoService;
        private static Mock<IMediator> mockMediator;
        private static Mock<IEmailService> mockEmailService;
        private static Mock<CacheHelper> mockCacheHelper;
        private static Mock<IEventService> mockEventService;

        private UserDTO existingUserDTO;
        private User existingUser;

        [SetUp]
        protected override void Initialize()
        {
            base.Initialize();
            mockMediator = new Mock<IMediator>();
            mockPhotoService = new Mock<IPhotoService>();
            mockEmailService = new Mock<IEmailService>();
            mockCacheHelper = new Mock<CacheHelper>();
            mockEventService = new Mock<IEventService>();
            
            service = new UserService(mockUnitOfWork.Object, mockMapper.Object, mockPhotoService.Object, mockMediator.Object, mockCacheHelper.Object, mockEmailService.Object);

            const string existingEmail = "existingEmail@gmail.com";
            var id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D");
            var name = "existingName";

            existingUser = new User { Id = id, Name = name, Email = existingEmail};
            existingUserDTO = new UserDTO { Id = id, Name = name, Email = existingEmail };

            mockUnitOfWork.Setup(u => u.UserRepository.Get("Role,Categories.Category,Photo"))
                .Returns(new List<User> { existingUser }
                    .AsQueryable());

            mockMapper.Setup(m => m.Map<User>(existingUserDTO))
                .Returns(existingUser);
            mockMapper.Setup(m => m.Map<UserDTO>(existingUser))
                .Returns(existingUserDTO);
        }

        [Test]
        public void Create_RepeatEmail_ReturnFalse()
        {
            var newUser = new UserDTO { Email = "existingEmail@gmail.com" };

            var result = service.Create(newUser);

            Assert.IsFalse(result.Result.Successed);
        }

        [Test]
        public void Verificate_CacheDtoNull_ReturnFalse()
        {
            CacheDTO cache = new CacheDTO() { };
            var result = service.ConfirmEmail(cache);

            Assert.IsFalse(result.Result.Successed);
        }

        [Test]
        public void Verificate_CacheDtoEmpty_ReturnFalse()
        {
            CacheDTO cache = new CacheDTO() { Token=""};
            var result = service.ConfirmEmail(cache);

            Assert.IsFalse(result.Result.Successed);
        }

        [Test]
        public void PasswordRecovery_UserdDTONull_ReturnFalse()
        {
            UserDTO newUser = new UserDTO() { };
            var result = service.PasswordRecover(newUser);

            Assert.IsFalse(result.Result.Successed);
        }

        [Test]
        public void Update_EmailIsNull_ReturnFalse()
        {
            UserDTO newUser = new UserDTO() { };
            var result = service.Update(newUser);

            Assert.IsFalse(result.Result.Successed);
        }

        [Test]
        public void Update_EmailIsEmpty_ReturnFalse()
        {
            UserDTO newUser = new UserDTO() { Email=""};
            var result = service.Update(newUser);

            Assert.IsFalse(result.Result.Successed);
        }

        [Test]
        public void Update_Success_ReturnAnyException()
        {
            mockUnitOfWork.Setup(u => u.UserRepository.Insert(It.IsAny<User>()));
            mockUnitOfWork.Setup(u => u.SaveAsync());

            Assert.DoesNotThrowAsync(async()=>await service.Update(existingUserDTO));
        }

        [Test]
        public void Update_InvalidId_ReturnFalse()
        {
            var newUser = new UserDTO() { Id = new Guid("62FA647C-AD54-4BCC-A860-E5A2664B019D") };

            var result = service.Update(newUser);

            Assert.IsFalse(result.Result.Successed);
        }

        [Test]
        public void Verificat_UserIsNull_ReturnFalse()
        {
            mockUnitOfWork.Setup(u => u.UserRepository.Get(It.IsAny<Guid>()))
                .Returns((User)null);

            var result = service.ConfirmEmail(new CacheDTO());

            Assert.IsFalse(result.Result.Successed);
        }

    }

}
