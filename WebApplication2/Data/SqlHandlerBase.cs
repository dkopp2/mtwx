using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;

namespace Mtwx.Web.Data
{
    /// <summary>
    /// The base class for SQL command handlers that return a value. The connection must be defined in the 
    /// configuration and the <see cref="DataConnectionType" /> enumeration
    /// This command can return a class or a collection of classes
    /// </summary>
    /// <typeparam name="TRequest">The type of command that is handled by this handler
    /// </typeparam>
    /// <typeparam name="TResponse">The return type.
    /// </typeparam>
    public abstract class SqlHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        /// <summary>
        /// The connection to be used when connecting to the database
        /// </summary>
        protected readonly DataConnectionType ConnectionType;

        protected readonly Func<DataConnectionType, IDbConnection> ConnectionFactory;

        protected readonly List<string> WhereList = new List<string>();
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlHandlerBase{T1, T2}"/> class.
        /// </summary>
        /// <param name="connectionType">
        /// The connection to be used for this handler
        /// </param>
        /// <param name="connectionFactory">The connection factory</param>
        protected SqlHandlerBase(
            DataConnectionType connectionType,
            Func<DataConnectionType, IDbConnection> connectionFactory)
        {
            ConnectionType = connectionType;
            ConnectionFactory = connectionFactory;
        }

        protected SqlHandlerBase(
            Func<DataConnectionType, IDbConnection> connectionFactory) : this(DataConnectionType.AppReadWrite, connectionFactory)
        {

        }

        protected string WhereSql => WhereList.Count > 0 ? $"WHERE {string.Join(" AND ", WhereList)} " : string.Empty;

        public virtual Task<TResponse> Handle(TRequest message, CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                var cmd = GetSql(message);

                // differentiate between a class and a collection class
                if (typeof(TResponse).GetInterface("IEnumerable") == null)
                {
                    using (var cn = ConnectionFactory(ConnectionType))
                    {
                        return cn.Query<TResponse>(cmd, message).FirstOrDefault();
                    }
                }

                using (var cn = ConnectionFactory(ConnectionType))
                {
                    // necessary when an IEnumerable type has been specified for T2
                    var genericType = typeof(TResponse).GetGenericArguments().First();
                    var results = cn.Query(genericType, cmd, message);
                    var methodInfo = typeof(Enumerable).GetMethod("Cast");
                    var genericMethod = methodInfo?.MakeGenericMethod(genericType);
                    return (TResponse)genericMethod?.Invoke(null, new object[] { results });
                }
            }, cancellationToken);
        }

        protected abstract string GetSql(TRequest message);

    }
}