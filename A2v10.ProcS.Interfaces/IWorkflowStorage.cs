﻿// Copyright © 2020 Alex Kukhtin. All rights reserved.

using System;
using System.Threading.Tasks;

namespace A2v10.ProcS.Interfaces
{
	public interface IWorkflowStorage
	{
		IWorkflowDefinition WorkflowFromString(String source);
		Task<IWorkflowDefinition> WorkflowFromStorage(IIdentity identity);
	}
}
