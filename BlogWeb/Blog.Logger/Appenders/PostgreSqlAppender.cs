using System;
using log4net.Appender;
using log4net.Core;
using Npgsql;

namespace Blog.Logger.Appenders
{
    public class PostgreSqlAppender : AppenderSkeleton
    {
        private string connectionString;

        #region FixFlags

        private FixFlags fixFlags = FixFlags.All;

        virtual public FixFlags Fix
        {
            get
            {
                return this.fixFlags;
            }
            set
            {
                this.fixFlags = value;
            }
        }

        #endregion

        public PostgreSqlAppender(string connectionString)
        {
            this.connectionString = connectionString;
        }

        protected override void Append(LoggingEvent loggingEvent)
        {
            loggingEvent.Fix = this.Fix;

            using (NpgsqlConnection connection = new NpgsqlConnection(this.connectionString))
            {
                string insertLogCommandText = @"INSERT INTO ""Logs""(
                    ""Logger"", 
                    ""Thread"", 
                    ""Level"", 
                    ""Location"", 
                    ""Message"", 
                    ""Exception"", 
                    ""LogDate"")
	                    VALUES(
                    :Logger,
                    :Thread,
                    :Level,
                    :Location,
                    :Message,
                    :Exception,
                    :LogDate); ";

                using (NpgsqlCommand command = new NpgsqlCommand(insertLogCommandText, connection))
                {
                    var logger = command.CreateParameter();
                    logger.Direction = System.Data.ParameterDirection.Input;
                    logger.DbType = System.Data.DbType.String;
                    logger.ParameterName = ":Logger";
                    logger.Value = loggingEvent.LoggerName;
                    command.Parameters.Add(logger);

                    var thread = command.CreateParameter();
                    thread.Direction = System.Data.ParameterDirection.Input;
                    thread.DbType = System.Data.DbType.String;
                    thread.ParameterName = ":Thread";
                    thread.Value = loggingEvent.ThreadName;
                    command.Parameters.Add(thread);

                    var level = command.CreateParameter();
                    level.Direction = System.Data.ParameterDirection.Input;
                    level.DbType = System.Data.DbType.String;
                    level.ParameterName = ":Level";
                    level.Value = loggingEvent.Level.Name;
                    command.Parameters.Add(level);

                    var location = command.CreateParameter();
                    location.Direction = System.Data.ParameterDirection.Input;
                    location.DbType = System.Data.DbType.String;
                    location.ParameterName = ":Location";
                    location.Value = loggingEvent.LocationInformation.FullInfo;
                    command.Parameters.Add(location);

                    var message = command.CreateParameter();
                    message.Direction = System.Data.ParameterDirection.Input;
                    message.DbType = System.Data.DbType.String;
                    message.ParameterName = ":Message";
                    message.Value = loggingEvent.RenderedMessage;
                    command.Parameters.Add(message);

                    var exception = command.CreateParameter();
                    exception.Direction = System.Data.ParameterDirection.Input;
                    exception.DbType = System.Data.DbType.String;
                    exception.ParameterName = ":Exception";
                    exception.Value = loggingEvent.GetExceptionString();
                    command.Parameters.Add(exception);

                    var logDate = command.CreateParameter();
                    logDate.Direction = System.Data.ParameterDirection.Input;
                    logDate.DbType = System.Data.DbType.DateTime2;
                    logDate.ParameterName = ":LogDate";
                    logDate.Value = loggingEvent.TimeStamp.ToUniversalTime();
                    command.Parameters.Add(logDate);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}
