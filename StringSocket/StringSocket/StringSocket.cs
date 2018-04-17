﻿// Written by Joe Zachary for CS 3500, November 2012
// Revised by Joe Zachary April 2016
// Revised extensively by Joe Zachary April 2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace CustomNetworking
{
    /// <summary>
    /// The type of delegate that is called when a StringSocket send has completed.
    /// </summary>
    public delegate void SendCallback(bool wasSent, object payload);

    /// <summary>
    /// The type of delegate that is called when a StringSocket receive has completed.
    /// </summary>
    public delegate void ReceiveCallback(string s, object payload);

    /// <summary> 
    /// A StringSocket is a wrapper around a Socket.  It provides methods that
    /// asynchronously read lines of text (strings terminated by newlines) and 
    /// write strings (as opposed to Sockets, which read and write raw bytes).
    ///
    /// StringSockets are thread safe.  This means that two or more threads may
    /// invoke methods on a shared StringSocket without restriction.  The
    /// StringSocket takes care of the synchronization.
    /// 
    /// Each StringSocket contains a Socket object that is provided by the client.  
    /// A StringSocket will work properly only if the client refrains from calling
    /// the contained Socket's read and write methods.
    /// 
    /// We can write a string to a StringSocket ss by doing
    /// 
    ///    ss.BeginSend("Hello world", callback, payload);
    ///    
    /// where callback is a SendCallback (see below) and payload is an arbitrary object.
    /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
    /// successfully written the string to the underlying Socket, or failed in the 
    /// attempt, it invokes the callback.  The parameter to the callback is the payload.  
    /// 
    /// We can read a string from a StringSocket ss by doing
    /// 
    ///     ss.BeginReceive(callback, payload)
    ///     
    /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
    /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
    /// string of text terminated by a newline character from the underlying Socket, or
    /// failed in the attempt, it invokes the callback.  The parameters to the callback are
    /// a string and the payload.  The string is the requested string (with the newline removed).
    /// </summary>
    public class StringSocket : IDisposable
    {
        /// <summary>
        /// Underlying socket
        /// </summary>
        private Socket socket;

        /// <summary>
        /// Encoding used for sending and receiving
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// Used to build incoming strings
        /// </summary>
        private StringBuilder incoming;

        /// <summary>
        /// Used to build up the outgoing strings
        /// </summary>
        private StringBuilder outgoing;

        /// <summary>
        /// Callbacks for received requests
        /// </summary>
        private Queue<ReceiveCallback> receiveCallbacks;

        /// <summary>
        /// Payloads for received requests
        /// </summary>
        private Queue<object> receivePayloads;

        /// <summary>
        /// List of callbacks that need to be called in a FIFO order
        /// </summary>
        private Queue<SendCallback> sendCallbacks;

        /// <summary>
        /// List of payloads for the callbacks given 
        /// </summary>
        private Queue<object> sendPayloads;

        /// <summary>
        /// Bytes being received
        /// </summary>
        private byte[] incomingBytes = new byte[1];

        /// <summary>
        /// Received bytes, decoded into chars
        /// </summary>
        private char[] incomingChars = new char[1];

        /// <summary>
        /// Keeps track of optional shorter length requirement from BeginReceive
        /// </summary>
        private int cutLength = 0;

        /// <summary>
        /// Keeps track of incoming string length to cut off ahead of time, if requested
        /// </summary>
        private int currentLength = 0;

        /// <summary>
        /// Bytes that will be sent
        /// </summary>
        private byte[] pendingBytes = new byte[0];

        /// <summary>
        /// index of the leftmost byte whose send has not been completed
        /// </summary>
        private int pendingIndex = 0;

        /// <summary>
        /// records whether a send is being attempted 
        /// </summary>
        private bool sendIsOngoing = false;

        /// <summary>
        /// Object that syncs access when receiving
        /// </summary>
        private readonly object receiveSync = new object();

        /// <summary>
        /// Object that syncs access when sending
        /// </summary>
        private readonly object sendSync = new object();

        /// <summary>
        /// Creates a StringSocket from a regular Socket, which should already be connected.  
        /// The read and write methods of the regular Socket must not be called after the
        /// StringSocket is created.  Otherwise, the StringSocket will not behave properly.  
        /// The encoding to use to convert between raw bytes and strings is also provided.
        /// </summary>
        internal StringSocket(Socket s, Encoding e)
        {
            socket = s;
            encoding = e;
            // TODO: Complete implementation of StringSocket

            outgoing = new StringBuilder();
            sendCallbacks = new Queue<SendCallback>();
            sendPayloads = new Queue<object>();

            incoming = new StringBuilder();
            receiveCallbacks = new Queue<ReceiveCallback>();
            receivePayloads = new Queue<object>();

            //socket.BeginReceive(incomingBytes, 0, incomingBytes.Length, SocketFlags.None, ReceiveBytes, null);
        }

        /// <summary>
        /// Shuts down this StringSocket.
        /// </summary>
        public void Shutdown(SocketShutdown mode)
        {
            socket.Shutdown(mode);
        }

        /// <summary>
        /// Closes this StringSocket.
        /// </summary>
        public void Close()
        {
            socket.Close();
        }

        /// <summary>
        /// We can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload)
        ///     
        /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
        /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
        /// string of text terminated by a newline character from the underlying Socket, it 
        /// invokes the callback.  The parameters to the callback are a string and the payload.  
        /// The string is the requested string (with the newline removed).
        /// 
        /// Alternatively, we can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload, length)
        ///     
        /// If length is negative or zero, this behaves identically to the first case.  If length
        /// is positive, then it reads and decodes length bytes from the underlying Socket, yielding
        /// a string s.  The parameters to the callback are s and the payload
        ///
        /// In either case, if there are insufficient bytes to service a request because the underlying
        /// Socket has closed, the callback is invoked with null and the payload.
        /// 
        /// This method is non-blocking.  This means that it does not wait until a line of text
        /// has been received before returning.  Instead, it arranges for a line to be received
        /// and then returns.  When the line is actually received (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginReceive
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginReceive must take care of synchronization instead.  On a given StringSocket, each
        /// arriving line of text must be passed to callbacks in the order in which the corresponding
        /// BeginReceive call arrived.
        /// 
        /// Note that it is possible for there to be incoming bytes arriving at the underlying Socket
        /// even when there are no pending callbacks.  StringSocket implementations should refrain
        /// from buffering an unbounded number of incoming bytes beyond what is required to service
        /// the pending callbacks.
        /// </summary>
        public void BeginReceive(ReceiveCallback callback, object payload, int length = 0)
        {
            cutLength = length;
            receiveCallbacks.Enqueue(callback);
            receivePayloads.Enqueue(payload);

            socket.BeginReceive(incomingBytes, 0, incomingBytes.Length, SocketFlags.None, ReceiveBytes, null);
        }

        /// <summary>
        /// Called when some data has been received.
        /// </summary>
        private void ReceiveBytes(IAsyncResult result)
        {
            int bytesRead = socket.EndReceive(result);

            lock (receiveSync)
            {
                if (bytesRead == 0) // socket has closed
                {
                    // todo: should we call Shutdown here too? If so, what's the mode?
                    Close();
                }
                else // socket open; something to send
                {
                    if (cutLength <= 0 || cutLength - currentLength > 0)
                    {
                        currentLength++;

                        Decoder d = encoding.GetDecoder();
                        int numChars = d.GetChars(incomingBytes, 0, bytesRead, incomingChars, 0, false);

                        if (incomingChars[0] != '\n')
                        {
                            incoming.Append(incomingChars, 0, numChars);
                            socket.BeginReceive(incomingBytes, 0, incomingBytes.Length, SocketFlags.None, ReceiveBytes, null);
                        }
                        else // receipt has been terminated by a newline
                        {
                            ResetReceive();
                        }
                    }
                    else
                    {
                        ResetReceive();
                    }
                }
            }
        }

        /// <summary>
        /// Resets receipt-related fields after receiving all data
        /// </summary>
        private void ResetReceive()
        {
            cutLength = 0;
            currentLength = 0;

            object payload = receivePayloads.Dequeue();
            ReceiveCallback callback = receiveCallbacks.Dequeue();
            Task.Run(() => callback(incoming.ToString(), payload));
        }

        /// <summary>
        /// We can write a string to a StringSocket ss by doing
        /// 
        ///    ss.BeginSend("Hello world", callback, payload);
        ///    
        /// where callback is a SendCallback (see below) and payload is an arbitrary object.
        /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
        /// successfully written the string to the underlying Socket it invokes the callback.  
        /// The parameters to the callback are true and the payload.
        /// 
        /// If it is impossible to send because the underlying Socket has closed, the callback 
        /// is invoked with false and the payload as parameters.
        ///
        /// This method is non-blocking.  This means that it does not wait until the string
        /// has been sent before returning.  Instead, it arranges for the string to be sent
        /// and then returns.  When the send is completed (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginSend
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginSend must take care of synchronization instead.  On a given StringSocket, each
        /// string arriving via a BeginSend method call must be sent (in its entirety) before
        /// a later arriving string can be sent.
        /// </summary>
        public void BeginSend(string s, SendCallback callback, object payload)
        {
            // todo: is this non-blocking, per the spec?
            // TODO: Implement BeginSend
            lock (sendSync)
            {
                outgoing.Append(s);
                sendCallbacks.Enqueue(callback);
                sendPayloads.Enqueue(payload);

                if (!sendIsOngoing)
                {
                    sendIsOngoing = true;
                    SendBytes();
                }
            }
        }

        /// <summary>
        /// Sends bytes
        /// </summary>
        private void SendBytes()
        {
            //If we are dealing with bytes right now
            if (pendingIndex < pendingBytes.Length)
            {
                socket.BeginSend(pendingBytes, pendingIndex, pendingBytes.Length - pendingIndex, SocketFlags.None, BytesSent, null);
            }

            //not sending bytes, so we start a byte send
            else if (outgoing.Length > 0)
            {
                pendingBytes = encoding.GetBytes(outgoing.ToString());
                pendingIndex = 0;
                outgoing.Clear();
                socket.BeginSend(pendingBytes, 0, pendingBytes.Length, SocketFlags.None, BytesSent, null);
            }

            //nothing to send or being sent
            else
            {
                sendIsOngoing = false;
            }
        }

        // todo: doc comment
        private void BytesSent(IAsyncResult result)
        {
            int numsent = socket.EndSend(result);

            //lock sending
            lock (sendSync)
            {
                pendingIndex += numsent;

                //We sent all the bytes for the request
                if (pendingIndex == pendingBytes.Length)
                {
                    object payload = sendPayloads.Dequeue();
                    SendCallback callback = sendCallbacks.Dequeue();
                    var t = Task.Run(() => callback(true, payload));
                }

                SendBytes();
            }
        }

        /// <summary>
        /// Frees resources associated with this StringSocket.
        /// </summary>
        public void Dispose()
        {
            Shutdown(SocketShutdown.Both);
            Close();
        }
    }
}
