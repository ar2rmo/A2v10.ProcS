﻿using System;
using System.Collections.Generic;
using System.Text;

namespace A2v10.ProcS.Infrastructure
{
	public interface IStorable
	{
		IDynamicObject Store();
		void Restore(IDynamicObject store);
	}
}