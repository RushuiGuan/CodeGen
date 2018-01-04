﻿using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Albatross.CodeGen.CSharp {
	public class BuildWebApiProxy : ClassGenerator<ObjectType> {

		const string ControllerPostfix = "Controller";
		public override string Category => "WebApi Proxy";
		public override string Description => "Create a web api proxy class by using the reflection against the controller";
		public override string Name => "webapi proxy";
		GetReflectionOnlyType _getReflectionOnlyType;

		public BuildWebApiProxy(GetReflectionOnlyType handle) {
			this._getReflectionOnlyType = handle;
		}

		public override string GetClassName(ObjectType objType) {
			return GetControllerName(_getReflectionOnlyType.Get(objType)) + "ClientApi";
		}

		public string GetControllerName(Type classType) {
			string controller = classType.Name;
			if (controller.EndsWith(ControllerPostfix)) {
				controller = controller.Substring(0, controller.Length - ControllerPostfix.Length);
			}
			return controller;
		}
		const string HttpGetAttribName = "System.Web.Http.HttpGetAttribute";
		const string HttpDeleteAttribName = "System.Web.Http.HttpDeleteAttribute";
		const string HttpPostAttribName = "System.Web.Http.HttpPostAttribute";
		const string HttpPutAttribName = "System.Web.Http.HttpPutAttribute";


		public override void RenderBody(StringBuilder sb, int tabLevel, ObjectType objType, ClassOptions options, ICodeGeneratorFactory factory) {
			Type controllerType = _getReflectionOnlyType.Get(objType);
			foreach (MethodInfo method in controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)) {
				foreach (CustomAttributeData data in method.GetCustomAttributesData()) {
					if (data.AttributeType.FullName == HttpGetAttribName) {
						BuildGetDelete(sb, tabLevel, method, "Get", controllerType);
					} else if (data.AttributeType.FullName == HttpDeleteAttribName) {
						BuildGetDelete(sb, tabLevel, method, "Delete", controllerType);
					} else if (data.AttributeType.FullName == HttpPostAttribName) {
						BuildPostPut(sb, tabLevel, method, "Post", controllerType);
					} else if (data.AttributeType.FullName == HttpPutAttribName) {
						BuildPostPut(sb, tabLevel, method, "Put", controllerType);
					}
				}
			}
		}


		public StringBuilder BuildGetDelete(StringBuilder sb, int tabLevel, MethodInfo methodInfo, string method, Type controllerType) {
			string controller = GetControllerName(controllerType);
			sb.Tab(tabLevel).Public().Append("async ");
			if (methodInfo.ReturnType == typeof(void)) {
				sb.Append("Task");
			} else {
				sb.Append("Task<").GetTypeName(methodInfo.ReturnType).Append(">");
			}
			sb.Space().Append(methodInfo.Name).OpenParenthesis();
			var parameters = methodInfo.GetParameters();
			foreach (var param in parameters) {
				sb.GetTypeName(param.ParameterType).Space().Append(param.Name);
				if (param != parameters.Last()) {
					sb.Comma().Space();
				}
			}

			sb.CloseParenthesis().OpenScope();
			tabLevel++;
			sb.Tab(tabLevel).Append("string url = new StringBuilder().Action(").Literal(controller).Comma().Space().Literal(methodInfo.Name).CloseParenthesis();
			foreach (var param in parameters) {
				sb.Append(".BuildParam(").Literal(param.Name).Comma().Append(param.Name).CloseParenthesis();
			}
			sb.AsString().Terminate();
			sb.Tab(tabLevel).Append(@"var response = await HttpClient.").Append(method).Append("Async(url);").AppendLine();
			if (methodInfo.ReturnType.FullName == "System.Void") {
				sb.Tab(tabLevel).Append("await response.Handle()").Terminate();
			} else {
				sb.Tab(tabLevel).Append("return await response.Handle<").GetTypeName(methodInfo.ReturnType).Append(">()").Terminate();
			}
			tabLevel--;
			return sb.Tab(tabLevel).CloseScope();
		}
		public StringBuilder BuildPostPut(StringBuilder sb, int tabLevel, MethodInfo methodInfo, string method, Type controllerType) {
			string controller = GetControllerName(controllerType);
			sb.Tab(tabLevel).Public().Append("async ");
			if (methodInfo.ReturnType == typeof(void)) {
				sb.Append("Task");
			} else {
				sb.Append("Task<").GetTypeName(methodInfo.ReturnType).Append(">");
			}
			sb.Space().Append(methodInfo.Name).OpenParenthesis();
			var param = methodInfo.GetParameters().FirstOrDefault();
			if (param != null) {
				sb.GetTypeName(param.ParameterType).Space().Append(param.Name);
			}
			sb.CloseParenthesis().OpenScope();
			tabLevel++;
			sb.Tab(tabLevel).Append("string url = new StringBuilder().Action(").Literal(controller).Comma().Space().Literal(methodInfo.Name).CloseParenthesis().AsString().Terminate();
			sb.Tab(tabLevel).Append(@"var response = await HttpClient.").Append(method);
			if (param != null) {
				sb.Append("AsJsonAsync<").GetTypeName(param.ParameterType).Append(">").OpenParenthesis().Append("url, ").Append(param.Name).CloseParenthesis().Terminate();
			} else {
				sb.Append("Async(url)").Terminate();
			}

			if (methodInfo.ReturnType.FullName == "System.Void") {
				sb.Tab(tabLevel).Append("await response.Handle()").Terminate();
			} else {
				sb.Tab(tabLevel).Append("return await response.Handle<").GetTypeName(methodInfo.ReturnType).Append(">()").Terminate();
			}
			tabLevel--;
			return sb.Tab(tabLevel).CloseScope();
		}
	}
}
