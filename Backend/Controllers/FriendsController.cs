using Backend.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

public class FriendsController(IUserFriendsService userFriendsService) : ControllerBase
{
    [HttpGet("user/{userId}/friends")]
    public async Task<IActionResult> GetUserFriends(int userId)
    {
        var friends = await userFriendsService.GetUserFriendsAsync(userId);
        return Ok(friends);
    }

    [HttpPost("user/{userId}/friends/add/{friendId}")]
    public async Task<IActionResult> AddFriend(int userId, int friendId)
    {
        await userFriendsService.AddFriendAsync(userId, friendId);
        return NoContent();
    }

    [HttpDelete("user/{userId}/friends/remove/{friendId}")]
    public async Task<IActionResult> RemoveFriend(int userId, int friendId)
    {
        await userFriendsService.RemoveFriendAsync(userId, friendId);
        return NoContent();
    }

    [HttpGet("user/{userId}/friends/are-friends/{friendId}")]
    public async Task<IActionResult> AreFriends(int userId, int friendId)
    {
        var areFriends = await userFriendsService.AreFriendsAsync(userId, friendId);
        return Ok(areFriends);
    }
}