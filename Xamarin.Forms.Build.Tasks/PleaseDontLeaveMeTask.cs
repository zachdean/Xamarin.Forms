using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mono.Cecil;
using System.Linq;
using System.Text;
using System.Collections.Generic;

using static Microsoft.Build.Framework.MessageImportance;

namespace Xamarin.Forms.Build.Tasks
{
	public class PleaseDontLeaveMeTask : Task
	{
		[Required]
		public string AssemblyName { get; set; }

		[Required]
		public string GenerationLocation { get; set; }

		[Output]
		public string[] GeneratedFiles { get; set; }

		public IAssemblyResolver DefaultAssemblyResolver { get; set; }

		protected TaskLoggingHelper LoggingHelper { get; set; }

		public string References { get; set; }		

		public override bool Execute()
		{
			
			LoggingHelper = LoggingHelper ?? new TaskLoggingHelper(this);
			LoggingHelper.LogMessage(Normal, $"{References} I FOUND YOU:");
				
			bool ReadOnly = true;
			bool debug = false;

			var resolver = DefaultAssemblyResolver ?? new XamlCAssemblyResolver();

			var readerParameters = new ReaderParameters
			{
				AssemblyResolver = resolver,
				ReadWrite = !ReadOnly,
				ReadSymbols = debug,
			};

			List<string> files = new List<string>();
			string[] assemblies = References.Split(';');


			StringBuilder generatedFileText = new StringBuilder();
			string guid = Guid.NewGuid().ToString("N");
			generatedFileText.AppendLine($"public class {Path.GetFileName(AssemblyName).Replace(".", "_")}_{guid}");
			generatedFileText.AppendLine("{");
			generatedFileText.AppendLine("	void KeepMeAround() {");


			bool somethingGenerated = false;
			foreach (var assembly in assemblies)
			{
				using (var assemblyDefinition = AssemblyDefinition.ReadAssembly(Path.GetFullPath(assembly), readerParameters))
				{
					foreach (var module in assemblyDefinition.Modules)
					{
						if (assemblyDefinition.HasCustomAttributes &&
							assemblyDefinition.CustomAttributes.Any(ca => ca.AttributeType.FullName == "Xamarin.Forms.PleaseDontLeaveMeAttribute"))
						{
							var firstPublicType = module.Types.FirstOrDefault(x => x.IsPublic);
							generatedFileText.AppendLine($"		throw new System.Exception(typeof({firstPublicType.FullName}).Name);");
							somethingGenerated = true;
						}
					}
				}
			}


			generatedFileText.AppendLine("	}");
			generatedFileText.AppendLine("}");

			if(somethingGenerated)
			{
				files.Add($"{Path.Combine(GenerationLocation, Path.GetFileName(AssemblyName).Replace(".", "_"))}.g.cs");
				//LoggingHelper.LogMessage(Normal, $"{generatedFileText}I FOUND YOU:");
				//LoggingHelper.LogMessage(Normal, $"{files.Last()}I FOUND YOU:");
				File.WriteAllText(files.Last(), generatedFileText.ToString());
			}

			GeneratedFiles = files.ToArray();
			return true;
		}
	}
}
