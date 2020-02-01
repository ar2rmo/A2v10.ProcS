﻿// Copyright © 2020 Alex Kukhtin. All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

using A2v10.ProcS.Interfaces;

namespace A2v10.ProcS
{
	public class WorkflowServiceBus : IServiceBus
	{
		private static readonly Dictionary<Type, Type> _starters = new Dictionary<Type, Type>();
		private static readonly Dictionary<Guid, ISaga> _sagas = new Dictionary<Guid, ISaga>();

		ConcurrentQueue<Object> _messages = new ConcurrentQueue<Object>();

		private readonly IInstanceStorage _instanceStorage;

		public WorkflowServiceBus(IInstanceStorage instanceStorage)
		{
			_instanceStorage = instanceStorage ?? throw new ArgumentNullException(nameof(instanceStorage));
		}

		public void Send(Object message) 
		{
			_messages.Enqueue(message);
		}

		public static void RegisterSaga<TStartMessage, TSaga>()  where TStartMessage : IMessage where  TSaga : ISaga
		{
			_starters.Add(typeof(TStartMessage), typeof(TSaga));
		}

		public async Task Run()
		{
			while (_messages.TryDequeue(out Object message))
			{
				await ProcessMessage(message);
			}
		}

		async Task ProcessMessage(Object message)
		{
			// domain messages
			if (message is IDomainEvent)
			{
				await ProcessDomainEvent(message);
			}
			// try to start new saga
			if (_starters.TryGetValue(message.GetType(), out Type sagaType)) 
			{
				await StartSaga(sagaType, message);
				return;
			}
			
			// process existing sagas
			if (message is IMessage iMsg)
			{
				if (_sagas.TryGetValue(iMsg.Id, out ISaga saga))
					await HandleSaga(saga, message);
			}
		}

		async Task ProcessDomainEvent(Object message)
		{
			foreach (var sagaKV in _sagas) {
				var saga = sagaKV.Value;
				await (Task) saga.Handle(message);
			}
		}

		async Task StartSaga(Type sagaType, Object message)
		{
			var iMsg = message as IMessage;
			var saga = System.Activator.CreateInstance(sagaType, iMsg.Id, this, _instanceStorage) as ISaga;
			await saga.Start(message);
			_sagas.Add(saga.Id, saga);
		}

		async Task HandleSaga(ISaga saga, Object message)
		{
			await (Task) saga.Handle(message);
			if (saga.IsComplete)
				_sagas.Remove(saga.Id);
		}
	}
}
