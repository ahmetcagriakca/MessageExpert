using System;
using System.Collections.Generic;
using System.Text;
using MessageExpert.Data.Models.Messaging;

namespace MessageExpert.Domain.Messaging.Services
{
    /// <summary>
    /// <see cref="MessageService"/>
    /// </summary>
	public interface IMessageService
	{
		void SendMessage(string content, string from, string to);
	}
	
}
