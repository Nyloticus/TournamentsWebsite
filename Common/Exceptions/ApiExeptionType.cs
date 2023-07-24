using System.Net;

namespace Common
{

    public enum ApiExeptionType
    {
        [Error("1", HttpStatusCode.OK, Message = "Successfully")]
        Ok,
        [Error("10", HttpStatusCode.BadRequest, Message = "Record no found")]
        BadRequest,

        [Error("11", HttpStatusCode.NotFound, Message = "Record no found")]

        NotFound,
        [Error("409", HttpStatusCode.BadRequest, Message = "Code Already Used")]
        CodeAlreadyUsed,

        [Error("410", HttpStatusCode.Forbidden, Message = "Code is Expired")]
        CodeExpired,

        [Error("411", HttpStatusCode.Forbidden, Message = "Code is not active")]
        CodeIsNotActive,

        [Error("20", HttpStatusCode.BadRequest, Message = "Invalid login")]
        InvalidLogin,

        [Error("40", HttpStatusCode.BadRequest, Message = "Validation error ")]
        ValidationError,

        [Error("50", HttpStatusCode.BadRequest, Message = "The record you attempt to delete have a lot of dependencies ")]
        DeleteRelatedObjectError,

        [Error("51", HttpStatusCode.Forbidden, Message = "Not Authorized to access this method")]
        Forbidden,
        [Error("52", HttpStatusCode.Unauthorized, Message = "Not Authorized to access this method")]
        Unauthorized,






        //Files errors
        [Error("100", HttpStatusCode.UnsupportedMediaType, Message = "Invalid Image file formate")]
        InvalidImage,
        [Error("101", HttpStatusCode.UnsupportedMediaType, Message = "Failed to delete image form cache directory")]
        FailedDeleteImage,
        [Error("102", HttpStatusCode.UnsupportedMediaType, Message = "Failed to save image")]
        FailedSaveImage,

        //database errors
        [Error("501", HttpStatusCode.InternalServerError, Message = "Failed to save data")]
        FailedSaveData,
        [Error("502", HttpStatusCode.InternalServerError, Message = "Failed to delete data")]
        FailedDeleteData,
        [Error("503", HttpStatusCode.InternalServerError, Message = "Failed to update data")]
        FailedUpdateData,
        [Error("504", HttpStatusCode.InternalServerError, Message = "Failed to get data")]
        FailedGetData,
    }
}
