﻿// Copyright © 2020 Alex Kukhtin, Artur Moshkola. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using A2v10.ProcS.Infrastructure;

namespace A2v10.ProcS
{
	public class SequenceActivity : IActivity, IStorable
	{
		public List<IActivity> Activities { get; set; }

		Int32 _currentAction { get; set; }

		public ActivityExecutionResult Execute(IExecuteContext context)
		{
			if (Activities == null || Activities.Count == 0)
				return ActivityExecutionResult.Complete;
			while (true)
			{
				if (_currentAction >= Activities.Count)
					return ActivityExecutionResult.Complete;
				var result = Activities[_currentAction].Execute(context);
				if (result == ActivityExecutionResult.Idle)
					return ActivityExecutionResult.Idle;
				_currentAction += 1;
			}
		}

		#region IStorable
		public IDynamicObject Store()
		{
			throw new NotImplementedException();
		}

		public void Restore(IDynamicObject store)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}
