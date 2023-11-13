using Polly;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace EventBus.RabbitMQ
{
    public class RabbitMqPersistentConnection : IDisposable
    {
        private readonly IConnectionFactory? _connectionFactory;
        private readonly int _retryCount;
        private IConnection? _connection;
        private readonly object _lockObject = new();
        private bool _disposed;
        private IConnectionFactory? connectionFactory;
        private int connectionRetryCount;

        public RabbitMqPersistentConnection(IConnectionFactory? connectionFactory, IConnection? connection, IConnectionFactory connectionFactory1, int retryCount = 5)
        {
            this._connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/"
            };
            _connection = connection;
            this._retryCount = retryCount;
        }

        public RabbitMqPersistentConnection(IConnectionFactory? connectionFactory, int connectionRetryCount, IConnectionFactory? connectionFactory1, IConnection? connection)
        {
            this.connectionFactory = connectionFactory;
            this.connectionRetryCount = connectionRetryCount;
            _connectionFactory = connectionFactory1;
            _connection = connection;
        }

        public RabbitMqPersistentConnection(IConnectionFactory? connectionFactory, int connectionRetryCount)
        {
            this.connectionFactory = connectionFactory;
            this.connectionRetryCount = connectionRetryCount;
            _connection = connectionFactory!.CreateConnection();
        }

        public bool IsConnected => _connection != null && _connection.IsOpen;


        public IModel? CreateModel()
        {
            return _connection?.CreateModel();
        }

        public void Dispose()
        {
            _disposed = true;
            _connection?.Dispose();
        }


        public bool TryConnect()
        {
            lock (_lockObject)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                    }
                );

                policy.Execute(() =>
                {
                    _connection = _connectionFactory?.CreateConnection();
                });

                if (IsConnected)
                {
                    if (_connection != null)
                    {
                        _connection.ConnectionShutdown += Connection_ConnectionShutdown;
                        _connection.CallbackException += Connection_CallbackException;
                        _connection.ConnectionBlocked += Connection_ConnectionBlocked;
                    }

                    // log

                    return true;
                }

                return false;
            }
        }

        private void Connection_ConnectionBlocked(object? sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;

            TryConnect();
        }

        private void Connection_CallbackException(object? sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;

            TryConnect();
        }

        private void Connection_ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {

            if (_disposed) return;

            TryConnect();
        }
    }
}
