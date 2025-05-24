using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GameplayGraph.Generator
{
    [Generator]
    public class ContainerGenerator : Microsoft.CodeAnalysis.IIncrementalGenerator
	{
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var containerClasses = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (ctx, _) => GetContainerInfo(ctx))
                .Where(static info => info != null);

            var collected = containerClasses.Collect();

            context.RegisterSourceOutput(collected, static (spc, list) =>
            {
                var groups = list.GroupBy(info => info!.FilePath);
                foreach (var group in groups)
                {
                    var builder = new StringBuilder();
                    foreach (var info in group)
                    {
                        GeneratePartial(info!, builder);
                    }

                    var fileName = System.IO.Path.GetFileNameWithoutExtension(group.Key);
                    var asmName = group.First()!.Symbol.ContainingAssembly.Name;
                    var root = FindAsmdefRoot(group.Key, asmName);
                    var hintPath = root != null ? System.IO.Path.Combine(root, "Generated", fileName + ".gen.cs") : fileName + ".gen.cs";
                    spc.AddSource(hintPath.Replace("\\", "/"), SourceText.From(builder.ToString(), Encoding.UTF8));
                }
            });
        }

        private static ContainerInfo? GetContainerInfo(GeneratorSyntaxContext context)
        {
            var classDecl = (ClassDeclarationSyntax)context.Node;
            var symbol = context.SemanticModel.GetDeclaredSymbol(classDecl) as INamedTypeSymbol;
            if (symbol == null)
                return null;

            if (IsContainer(symbol))
            {
                var path = classDecl.SyntaxTree.FilePath;
                return new ContainerInfo(symbol, path);
            }

            return null;
        }

        private static bool IsContainer(INamedTypeSymbol symbol)
        {
            for (var type = symbol.BaseType; type != null; type = type.BaseType)
            {
                var name = type.ToDisplayString();
                if (name.StartsWith("GameplayGraph.PropertyContainer") || name.StartsWith("GameplayGraph.ListContainer") ||
                    type.Name == "PropertyContainer" || type.Name == "ListContainer")
                {
                    return true;
                }
            }
            return false;
        }

        private static void GeneratePartial(ContainerInfo info, StringBuilder builder)
        {
            var ns = info.Symbol.ContainingNamespace.IsGlobalNamespace ? null : info.Symbol.ContainingNamespace.ToDisplayString();
            if (ns != null)
            {
                builder.Append("namespace ").Append(ns).AppendLine()
                       .AppendLine("{");
            }

            builder.Append("    public partial class ").Append(info.Symbol.Name);
            if (info.Symbol.TypeParameters.Length > 0)
            {
                builder.Append('<').Append(string.Join(", ", info.Symbol.TypeParameters.Select(t => t.Name))).Append('>');
            }
            builder.AppendLine()
                   .AppendLine("    {")
                   .AppendLine("    }");

            if (ns != null)
            {
                builder.AppendLine("}");
            }
        }

        private static string? FindAsmdefRoot(string filePath, string asmName)
        {
            var dir = System.IO.Path.GetDirectoryName(filePath);
            while (!string.IsNullOrEmpty(dir))
            {
                var asmdef = System.IO.Path.Combine(dir, asmName + ".asmdef");
                if (System.IO.File.Exists(asmdef))
                    return dir;
                dir = System.IO.Path.GetDirectoryName(dir);
            }
            return null;
        }

        private record ContainerInfo(INamedTypeSymbol Symbol, string FilePath);
    }
}
