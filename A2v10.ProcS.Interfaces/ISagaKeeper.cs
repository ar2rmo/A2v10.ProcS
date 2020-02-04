﻿// Copyright © 2020 Alex Kukhtin. All rights reserved.

using System;
using System.Threading.Tasks;

namespace A2v10.ProcS.Interfaces
{
	public interface ISagaKeeperKey : IEquatable<ISagaKeeperKey>
	{
		Type SagaType { get; }
		ICorrelationId CorrelationId { get; }
	}

	public interface ISagaKeeper {
		ISaga GetSagaForMessage(IMessage message, out ISagaKeeperKey key, out bool isNew);
		void SagaUpdate(ISaga saga, ISagaKeeperKey key);
	}
}