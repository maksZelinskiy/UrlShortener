using System.Reflection;
using Moq;
using BusinessLogic.DAL;
using DataAccess.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Controllers;
using Xunit;

namespace UrlShortener.Tests
{
    public class AboutControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly AboutController _aboutController;

        public AboutControllerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _aboutController = new AboutController(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Get_ReturnsAboutDataAndCanEditFlag()
        {
            var aboutData = new AboutModel { Content = "About content", Topic = "About topic" };
            _unitOfWorkMock.Setup(uow => uow.AboutRepository.GetAboutAsync()).ReturnsAsync(aboutData);

            var result = await _aboutController.Get();

            var jsonResult = Assert.IsType<JsonResult>(result);
            var about = jsonResult.Value!.GetType()
                .GetProperty("about")!;
            var canEdit = jsonResult.Value!.GetType()
                .GetProperty("canEdit")!;

            Assert.Equal(aboutData, about.GetValue(jsonResult.Value, null));
            Assert.Null(canEdit.GetValue(jsonResult.Value, null));
        }

        [Fact]
        public async Task Put_WithValidData_ReturnsOkResult()
        {
            var aboutModel = new AboutModel { Content = "New about content", Topic = "New about topic" };
            _unitOfWorkMock.Setup(u => u.AboutRepository.UpdateAboutAsync(aboutModel)).Verifiable();
            _unitOfWorkMock.Setup(u => u.SaveAsync()).Returns(Task.CompletedTask);

            var result = await _aboutController.Put(aboutModel);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var message = okResult.Value!.GetType().GetProperty("message")!;

            Assert.Equal("About page updated successfully", message.GetValue(okResult.Value, null));

            _unitOfWorkMock.Verify(u => u.AboutRepository.UpdateAboutAsync(aboutModel), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveAsync(), Times.Once);
        }
    }
}
