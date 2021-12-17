using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthenticationService.Dto.Common
{
    /// <summary>
    /// 响应体
    /// </summary>
    public class ResponseMessage
    {
        /// <summary>
        /// 结果码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 构造器
        /// </summary>
        public ResponseMessage()
        {
            Code = ResponseCodeDefines.SuccessCode;
        }
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool IsSuccess() => Code == ResponseCodeDefines.SuccessCode;
    }

    /// <summary>
    /// 携带数据的响应体
    /// </summary>
    /// <typeparam name="TEx"></typeparam>
    public class ResponseMessage<TEx> : ResponseMessage
    {
        /// <summary>
        /// 携带数据
        /// </summary>
        public TEx Extension { get; set; }
    }

    /// <summary>
    /// 分页响应体
    /// </summary>
    /// <typeparam name="Tentity"></typeparam>
    public class PagingResponseMessage<Tentity> : ResponseMessage<List<Tentity>>
    {
        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 对象总数
        /// </summary>
        public long TotalCount { get; set; }
        /// <summary>
        /// 分页数量
        /// </summary>
        public int PageCount { get => PageSize <= 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize); }
    }

    public static class ResMsg
    {
        #region Success
        public static ResponseMessage Success(this ResponseMessage source, string msg = "操作成功")
        {
            source.Code = ResponseCodeDefines.SuccessCode;
            source.Message = msg;
            return source;
        }

        public static ResponseMessage<T> Success<T>(this ResponseMessage<T> source, string msg = "操作成功")
        {
            source.Code = ResponseCodeDefines.SuccessCode;
            source.Message = msg;
            return source;
        }
        #endregion

        #region NotAllow
        public static ResponseMessage NotAllow(this ResponseMessage source, string msg = "不允许的操作")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }

        public static ResponseMessage<T> NotAllow<T>(this ResponseMessage<T> source, string msg = "不允许的操作")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }
        #endregion

        #region NotFound
        public static ResponseMessage NotFound(this ResponseMessage source, string msg = "数据不存在")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }

        public static ResponseMessage<T> NotFound<T>(this ResponseMessage<T> source, string msg = "数据不存在")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }

        public static PagingResponseMessage<T> NotFound<T>(this PagingResponseMessage<T> source, string msg = "数据不存在")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }
        #endregion

        #region NoneAuthorization
        public static ResponseMessage NoneAuthorization(this ResponseMessage source, string msg = "未授权的API资源")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }

        public static ResponseMessage<T> NoneAuthorization<T>(this ResponseMessage<T> source, string msg = "未授权的API资源")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }

        public static PagingResponseMessage<T> NoneAuthorization<T>(this PagingResponseMessage<T> source, string msg = "未授权的API资源")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }
        #endregion

        #region ModelStateInvalid
        public static ResponseMessage ModelStateInvalid(this ResponseMessage source, string msg = "对象校验错误")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }

        public static ResponseMessage<T> ModelStateInvalid<T>(this ResponseMessage<T> source, string msg = "对象校验错误")
        {
            source.Code = ResponseCodeDefines.NotAllow;
            source.Message = msg;
            return source;
        }
        #endregion

        #region ArgumentNullError
        public static ResponseMessage ArgumentNullError(this ResponseMessage source, string msg = "参数错误")
        {
            source.Code = ResponseCodeDefines.ArgumentNullError;
            source.Message = msg;
            return source;
        }

        public static ResponseMessage<T> ArgumentNullError<T>(this ResponseMessage<T> source, string msg = "参数错误")
        {
            source.Code = ResponseCodeDefines.ArgumentNullError;
            source.Message = msg;
            return source;
        }

        public static PagingResponseMessage<T> ArgumentNullError<T>(this PagingResponseMessage<T> source, string msg = "参数错误")
        {
            source.Code = ResponseCodeDefines.ArgumentNullError;
            source.Message = msg;
            return source;
        }
        #endregion

        #region ServiceError
        public static ResponseMessage ServiceError(this ResponseMessage source, string msg = "服务错误")
        {
            source.Code = ResponseCodeDefines.ServiceError;
            source.Message = msg;
            return source;
        }

        public static ResponseMessage<T> ServiceError<T>(this ResponseMessage<T> source, string msg = "服务错误")
        {
            source.Code = ResponseCodeDefines.ServiceError;
            source.Message = msg;
            return source;
        }

        public static PagingResponseMessage<T> ServiceError<T>(this PagingResponseMessage<T> source, string msg = "服务错误")
        {
            source.Code = ResponseCodeDefines.ServiceError;
            source.Message = msg;
            return source;
        }
        #endregion
    }

}
