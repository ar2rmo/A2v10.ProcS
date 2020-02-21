﻿// Copyright © 2020 Alex Kukhtin, Artur Moshkola. All rights reserved.

using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace A2v10.ProcS.WebApi.Host.MvcExtensions
{
	public class RawJsonBodyInputFormatter : InputFormatter
	{
		public RawJsonBodyInputFormatter()
		{
			this.SupportedMediaTypes.Add("application/json");
		}

		public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
		{
			var request = context.HttpContext.Request;
			using var reader = new StreamReader(request.Body);
			var content = await reader.ReadToEndAsync();
			return await InputFormatterResult.SuccessAsync(content);
		}

		protected override Boolean CanReadType(Type type)
		{
			return type == typeof(String);
		}
	}
}
