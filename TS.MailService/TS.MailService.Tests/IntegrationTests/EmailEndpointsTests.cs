﻿//using System.Net.Http.Json;
//using System.Net;
//using FluentAssertions;
//using TS.MailService.Application.Models;
//using TS.MailService.Infrastructure.Entities;
//using TS.MailService.Tests.InitializeModels;
//using System;
//using TS.MailService.Application;
//using Microsoft.AspNetCore.Mvc.Testing;

//namespace TS.MailService.Tests.IntegrationTests
//{
//    public class MessageControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
//    {
//        private readonly HttpClient _httpClient;
//        private readonly Guid _emailId;

//        public MessageControllerIntegrationTests(WebApplicationFactory<Program> factory)
//        {
//            _httpClient = factory.CreateClient();
//            _emailId = InitializeData.GetEmailMessageEntityFromUser().Id;
//        }

//        [Fact]
//        public async Task Post_ValidMessage_ReturnsMessageViewModel()
//        {
//            // Arrange
//            var email = InitializeData.GetShortEmailMessageFromUser();

//            // Act
//            var response = await _httpClient.PostAsJsonAsync("/api/email", email);

//            // Assert
//            response.Should().BeSuccessful();
//        }

//        [Fact]
//        public async Task Post_InvalidMessage_ReturnsBadRequest()
//        {
//            // Arrange
//            var email = new ShortEmailMessageDto();

//            // Act
//            var response = await _httpClient.PostAsJsonAsync("/api/email", email);

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
//        }

//        [Fact]
//        public async Task GetAllEmails_ReturnIEnumerableEmailMessageDTO()
//        {
//            // Act
//            var response = await _httpClient.GetAsync("/api/email");

//            // Assert
//            response.EnsureSuccessStatusCode();
//            var emails = await response.Content.ReadFromJsonAsync<IEnumerable<EmailMessageDto>>();
//            emails.Should().NotBeNull();
//            emails.Should().NotContainNulls(x => x.Body);
//            emails.Should().NotContainNulls(x => x.Recipients);
//            emails.Should().NotContainNulls(x => x.Subject);
//        }

//        [Fact]
//        public async Task GetUserById_CorrectId_ReturnUserViewModel()
//        {
//            // Act
//            var response = await _httpClient.GetAsync($"/api/email/{_emailId}");

//            // Assert
//            response.EnsureSuccessStatusCode();
//            var emailMessageDto = await response.Content.ReadFromJsonAsync<EmailMessageEntity>();

//            emailMessageDto.Should().NotBeNull();
//            emailMessageDto.Body.Should().NotBeNull();
//            emailMessageDto.Recipients.Should().NotContainNulls();
//            emailMessageDto.Subject.Should().NotBeNull();
//        }

//        [Fact]
//        public async Task GetUserById_IncorrectId_ReturnNoContent()
//        {
//            // Act
//            var response = await _httpClient.GetAsync($"/api/user/{Guid.NewGuid()}");

//            // Assert
//            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
//        }
//    }
//}