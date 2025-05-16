using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using ToDoList.Controllers;
using ToDoList.Helpers;
using ToDoList.Models;
using ToDoList.Requests.TodoRequest;
using ToDoList.Services.Interfaces;
using static ToDoList.Helpers.ResponseHelper;
using NUnit.Framework.Legacy;

namespace ToDoList.Controllers.Tests
{
    [TestFixture]
    public class TodoControllerTests
    {
        private Mock<ITodoService> _serviceMock;
        private TodoController _controller;

        [SetUp]
        public void Setup()
        {
            _serviceMock = new Mock<ITodoService>();
            _controller = new TodoController(null, _serviceMock.Object);
        }

        [Test]
        public async Task Index_ReturnsTodosSuccessfully()
        {
            var todos = new List<Todo>
            {
                new Todo { Id = 1, Title = "Test Task 1" },
                new Todo { Id = 2, Title = "Test Task 2" }
            };

            _serviceMock.Setup(s => s.GetTodosByDateAsync(null)).ReturnsAsync(todos);

            var result = await _controller.Index();

            var response = AssertResponse(result, 200, "Fetched all tasks", "Success");
            ClassicAssert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Show_ReturnsTodo_WhenFound()
        {
            var todo = new Todo { Id = 1, Title = "Test Task" };
            _serviceMock.Setup(s => s.GetTodoAsync(1)).ReturnsAsync(todo);

            var result = await _controller.Show(1);

            var response = AssertResponse(result, 200, "Fetched task", "Success");
            ClassicAssert.AreEqual(todo.Title, JsonSerializer.Deserialize<Todo>(response.Data!.ToString()!).Title);
        }

        [Test]
        public async Task Show_ReturnsNotFound_WhenMissing()
        {
            _serviceMock.Setup(s => s.GetTodoAsync(99)).ReturnsAsync((Todo)null);

            var result = await _controller.Show(99);

            var response = AssertResponse(result, 404, "Task not found", "Error");
            ClassicAssert.IsNull(response.Data);
        }

        [Test]
        public async Task Create_ReturnsCreatedTodo_WhenValid()
        {
            var request = new CreateTodoRequest
            {
                Title = "New Task",
                Date = DateTime.Today
            };

            var createdTodo = new Todo { Id = 1, Title = "New Task" };

            _serviceMock.Setup(s => s.CreateTodoAsync(request)).ReturnsAsync(createdTodo);

            var result = await _controller.Create(request);

            var response = AssertResponse(result, 200, "Task created", "Success");
            ClassicAssert.AreEqual("New Task", JsonSerializer.Deserialize<Todo>(response.Data!.ToString()!)!.Title);
        }

        [Test]
        public async Task Create_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            var request = new CreateTodoRequest();
            _controller.ModelState.AddModelError("Title", "Title is required");

            var result = await _controller.Create(request);

            var response = AssertResponse(result, 400, "Validation failed", "Error");
            ClassicAssert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Update_ReturnsOk_WhenTodoIsUpdated()
        {
            int id = 1;
            var request = new EditTodoRequest { Title = "Updated Task" };
            var updatedTodo = new Todo { Id = id, Title = "Updated Task" };

            _serviceMock.Setup(s => s.UpdateTodoAsync(id, request))
                        .ReturnsAsync(updatedTodo);

            var result = await _controller.Update(id, request);

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            ClassicAssert.AreEqual(200, okResult!.StatusCode);

            var json = okResult.Value as string;
            var response = JsonSerializer.Deserialize<Response>(json!);
            ClassicAssert.AreEqual("Success", response!.Status);
            ClassicAssert.AreEqual("Task updated", response.Message);
            ClassicAssert.IsNotNull(response.Data);
        }

        [Test]
        public async Task Update_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            int id = 999;
            var request = new EditTodoRequest { Title = "Doesn't Matter" };

            _serviceMock.Setup(s => s.UpdateTodoAsync(id, request))
                        .ReturnsAsync((Todo)null);

            var result = await _controller.Update(id, request);

            ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFound = result as NotFoundObjectResult;
            ClassicAssert.AreEqual(404, notFound!.StatusCode);

            var json = notFound.Value as string;
            var response = JsonSerializer.Deserialize<Response>(json!);
            ClassicAssert.AreEqual("Error", response!.Status);
            ClassicAssert.AreEqual("Task not found", response.Message);
        }

        [Test]
        public async Task UpdateStatus_ReturnsOk_WhenSetToCompleted_CompletedAtIsNotNull()
        {
            int id = 1;
            var request = new UpdateStatusTodoRequest { IsCompleted = true };
            var updatedTodo = new Todo
            {
                Id = id,
                IsCompleted = true,
                CompletedAt = DateTime.UtcNow
            };

            _serviceMock.Setup(s => s.UpdateStatusAsync(id, request))
                        .ReturnsAsync(updatedTodo);

            var result = await _controller.UpdateStatus(id, request);

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            ClassicAssert.AreEqual(200, okResult.StatusCode);

            var json = okResult.Value as string;
            var response = JsonSerializer.Deserialize<Response>(json!);
            ClassicAssert.AreEqual("Success", response!.Status);
            ClassicAssert.AreEqual("Status updated", response.Message);

            var todoData = response.Data as JsonElement?;
            ClassicAssert.IsTrue(todoData.HasValue);

            if (todoData!.Value.TryGetProperty("CompletedAt", out var completedAtProp))
            {
                ClassicAssert.IsTrue(completedAtProp.GetDateTime() != default);
            }
            else
            {
                ClassicAssert.Fail("CompletedAt property not found in response data");
            }
        }

        [Test]
        public async Task UpdateStatus_ReturnsOk_WhenSetToNotCompleted_CompletedAtIsNull()
        {
            int id = 1;
            var request = new UpdateStatusTodoRequest { IsCompleted = false };
            var updatedTodo = new Todo
            {
                Id = id,
                IsCompleted = false,
                CompletedAt = null
            };

            _serviceMock.Setup(s => s.UpdateStatusAsync(id, request))
                        .ReturnsAsync(updatedTodo);

            var result = await _controller.UpdateStatus(id, request);

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            ClassicAssert.AreEqual(200, okResult.StatusCode);

            var json = okResult.Value as string;
            var response = JsonSerializer.Deserialize<Response>(json!);
            ClassicAssert.AreEqual("Success", response!.Status);
            ClassicAssert.AreEqual("Status updated", response.Message);

            var todoData = response.Data as JsonElement?;
            ClassicAssert.IsTrue(todoData.HasValue);

            if (todoData!.Value.TryGetProperty("CompletedAt", out var completedAtProp))
            {
                ClassicAssert.IsTrue(completedAtProp.ValueKind == JsonValueKind.Null);
            }
            else
            {
                ClassicAssert.Fail("CompletedAt property not found in response data");
            }
        }

        [Test]
        public async Task UpdateStatus_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            int id = 999;
            var request = new UpdateStatusTodoRequest { IsCompleted = true };

            _serviceMock.Setup(s => s.UpdateStatusAsync(id, request))
                        .ReturnsAsync((Todo)null);

            var result = await _controller.UpdateStatus(id, request);

            ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFound = (NotFoundObjectResult)result;
            ClassicAssert.AreEqual(404, notFound.StatusCode);

            var json = notFound.Value as string;
            var response = JsonSerializer.Deserialize<Response>(json!);
            ClassicAssert.AreEqual("Error", response!.Status);
            ClassicAssert.AreEqual("Task not found", response.Message);
        }

        [Test]
        public async Task Delete_ReturnsOk_WhenTodoIsDeleted()
        {
            int id = 1;
            _serviceMock.Setup(s => s.DeleteTodoAsync(id)).ReturnsAsync(true);

            var result = await _controller.Delete(id);

            ClassicAssert.IsInstanceOf<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            ClassicAssert.AreEqual(200, okResult.StatusCode);

            var json = okResult.Value as string;
            var response = JsonSerializer.Deserialize<Response>(json!);
            ClassicAssert.AreEqual("Success", response!.Status);
            ClassicAssert.AreEqual("Task deleted", response.Message);
            ClassicAssert.IsNull(response.Data);
        }

        [Test]
        public async Task Delete_ReturnsNotFound_WhenTodoDoesNotExist()
        {
            int id = 999;
            _serviceMock.Setup(s => s.DeleteTodoAsync(id)).ReturnsAsync(false);

            var result = await _controller.Delete(id);

            ClassicAssert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFound = (NotFoundObjectResult)result;
            ClassicAssert.AreEqual(404, notFound.StatusCode);

            var json = notFound.Value as string;
            var response = JsonSerializer.Deserialize<Response>(json!);
            ClassicAssert.AreEqual("Error", response!.Status);
            ClassicAssert.AreEqual("Task not found", response.Message);
        }

        [Test]
        public async Task CopyTasks_ReturnsOk_WhenValidRequest()
        {
            var request = new CopyTodoTasksRequest
            {
                FromDate = new DateTime(2024, 1, 1),
                ToDate = new DateTime(2024, 1, 2)
            };

            var copiedTasks = new List<Todo>
    {
        new Todo { Id = 1, Title = "Copied Task 1" },
        new Todo { Id = 2, Title = "Copied Task 2" }
    };

            _serviceMock.Setup(s => s.CopyTasksAsync(request))
                        .ReturnsAsync(copiedTasks);

            var result = await _controller.CopyTasks(request);

            var response = AssertResponse(result, 200, "Tasks copied from 2024-01-01 to 2024-01-02", "Success");
            ClassicAssert.IsNotNull(response.Data);
        }

        [Test]
        public async Task CopyTasks_ReturnsBadRequest_WhenModelIsInvalid()
        {
            var request = new CopyTodoTasksRequest();
            _controller.ModelState.AddModelError("FromDate", "FromDate is required");

            var result = await _controller.CopyTasks(request);

            var response = AssertResponse(result, 400, "Validation failed", "Error");
            ClassicAssert.IsNotNull(response.Data);
        }

        [Test]
        public async Task UpdateSequence_ReturnsOk_WhenValidRequest()
        {
            var request = new ReorderTodoRequest
            {
                Ids = new List<int> { 3, 1, 2 }
            };

            _serviceMock.Setup(s => s.ReorderTodosAsync(request))
                        .ReturnsAsync(true);

            var result = await _controller.UpdateSequence(request);

            var response = AssertResponse(result, 200, "Sequence updated", "Success");
        }

        [Test]
        public async Task UpdateSequence_ReturnsBadRequest_WhenIdsNullOrEmpty()
        {
            var request = new ReorderTodoRequest { Ids = null };

            var result = await _controller.UpdateSequence(request);

            var response = AssertResponse(result, 400, "Invalid data", "Error");
        }

        private static Response AssertResponse(IActionResult result, int expectedStatusCode, string expectedMessage, string expectedStatus)
        {
            ClassicAssert.IsInstanceOf<ObjectResult>(result);
            var objectResult = result as ObjectResult;

            ClassicAssert.AreEqual(expectedStatusCode, objectResult!.StatusCode);

            var json = objectResult.Value as string;
            ClassicAssert.IsNotNull(json);

            var response = JsonSerializer.Deserialize<Response>(json!);
            ClassicAssert.IsNotNull(response);
            ClassicAssert.AreEqual(expectedStatus, response!.Status);
            ClassicAssert.AreEqual(expectedMessage, response.Message);

            return response;
        }
    }
}
