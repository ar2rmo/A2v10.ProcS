﻿using System;
using System.Collections.Generic;
using System.Linq;
using A2v10.ProcS.Interfaces;

namespace A2v10.ProcS
{
	public class StateMachine
	{
		public String Description { get; set; }
		public String InitialState { get; set; }

		public Dictionary<String, State> States { get; set; }

		public void Run(ExecuteContext context)
		{
			if (States == null || States.Count == 0)
				return;
			var instance = context.Instance;
			if (String.IsNullOrEmpty(instance.CurrentState))
			{
				if (!String.IsNullOrEmpty(InitialState))
					instance.CurrentState = InitialState;
				else
					instance.CurrentState = States.First(x => true).Key;
			}
			while (true)
			{
				if (States.TryGetValue(instance.CurrentState, out State state))
				{
					var result = state.ExecuteStep(context);
					if (result != ExecuteResult.Continue)
						return;
				}
			}
		}
	}
}
