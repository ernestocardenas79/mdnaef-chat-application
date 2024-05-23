namespace UNOSChat.AuthenticationAPI.Dtos;

public abstract class BaseResponse
{
    public int HttpCode { get; set; } = 200;
    public bool IsSuccess { get; set; } = true;

    public virtual IEnumerable<string?> Errors { get; set; }
}
