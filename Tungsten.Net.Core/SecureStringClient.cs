using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using W.Logging;
using W.Net.Helpers;
using W.Net.Sockets;

namespace W.Net
{
    /// <summary>
    /// A client which uses assymetric encryption when sending and receiving strings.
    /// </summary>
    public class SecureStringClient : StringClient, ISecureSocket
    {
        private readonly W.Encryption.RSA _rsa;
        private RSAParameters? _remotePublicKey;

        /// <summary>
        /// Constructs a new SecureStringClient
        /// </summary>
        public SecureStringClient() : base()
        {
            _rsa = new W.Encryption.RSA();
            Socket.Connected += (socket, address) =>
            {
                SendPublicKey(); //immediately send the public key
            };
        }

        /// <summary>
        /// Constructs a new SecureStringClient
        /// </summary>
        /// <param name="client">An existing connected TcpClient</param>
        /// <param name="rsa">An existing instance of RSA to be used for encryption</param>
        public SecureStringClient(TcpClient client, W.Encryption.RSA rsa) : base(client)
        {
            _rsa = rsa;
            SendPublicKey(); //immediately send the public key
        }

        private void SendPublicKey()
        {
            var publicKey = _rsa.PublicKey.AsXml<RSAParameters>();
            base.Send(publicKey);
        }
        /// <summary>
        /// Override to alter received data before exposing it via the MessageReceived callback
        /// </summary>
        /// <param name="message">The received data</param>
        /// <returns>The original or modified data</returns>
        protected override string FormatReceivedMessage(byte[] message)
        {
            string msg;
            if (_remotePublicKey != null)
            {
                msg = base.FormatReceivedMessage(message); //Encoded left it base64 encoded, don't unencoded it as Decrypt requires the encoding
                msg = _rsa.Decrypt(msg); //msg should be base64 encoded going into _rsa.Decrypt
            }
            else
            {
                msg = base.FormatReceivedMessage(message).FromBase64();
                _remotePublicKey = msg.FromXml<RSAParameters>();
                msg = null; //not a real message, so set it to null
            }
            return msg;
        }
        /// <summary>
        /// Override to alter data before it is sent
        /// </summary>
        /// <param name="message">The data to send</param>
        /// <returns>The original or modified data</returns>
        protected override byte[] FormatMessageToSend(string message)
        {
            byte[] msg;
            if (_remotePublicKey != null) //is secure
            {
                msg = base.FormatMessageToSend(message); //encrypt will base64 encode it, so don't do it here
                msg = _rsa.Encrypt(msg, (RSAParameters)_remotePublicKey).AsBytes(); //msg should be base64 encoded going into _rsa.Encrypt
            }
            else
            {
                msg = base.FormatMessageToSend(message).AsBase64();
            }
            return msg;
        }
    }
}