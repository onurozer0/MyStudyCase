using System.Text.Json.Serialization;

namespace SharedLib.DTOs
{
	public class Response<T> where T : class
	{
		public T? Data { get; private set; }
		public int StatusCode { get; private set; }
		public ErrorDto? Error { get; set; }
		[JsonIgnore]
		public bool IsSuccess { get; set; }
		public static Response<T> Success(T data, int statusCode)
		{
			return new Response<T> { StatusCode = statusCode, Data = data, IsSuccess = true };
		}
		public static Response<T> Success(int statusCode)
		{
			return new Response<T> { StatusCode = statusCode, IsSuccess = true };
		}
		public static Response<T> Fail(ErrorDto errorDto, int statusCode)
		{
			return new Response<T> { StatusCode = statusCode, Error = errorDto, IsSuccess = false };
		}
		public static Response<T> Fail(string errorMessage, int statusCode, bool isShow)
		{
			var errorDto = new ErrorDto(errorMessage, isShow);
			return new Response<T> { Error = errorDto, StatusCode = statusCode, IsSuccess = false };
		}
	}
}
